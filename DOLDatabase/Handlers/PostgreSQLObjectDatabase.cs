/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;

using Npgsql;

using DOL.Database.Connection;
using IsolationLevel = DOL.Database.Transaction.IsolationLevel;
using DOL.Database.Attributes;

namespace DOL.Database.Handlers
{
    public class PostgreSQLObjectDatabase : SQLObjectDatabase
    {
		/// <summary>
		/// Create a new instance of <see cref="PostgreSQLObjectDatabase"/>
		/// </summary>
		/// <param name="ConnectionString">Database Connection String</param>
		public PostgreSQLObjectDatabase(string ConnectionString)
			: base(ConnectionString)
		{
			// Options of MySQL connection string
			//if (!this.ConnectionString.Contains("Treat Tiny As Boolean") && !this.ConnectionString.Contains("TreatTinyAsBoolean"))
			//{
			//	this.ConnectionString += ";Treat Tiny As Boolean=False";
			//}

			//if (!this.ConnectionString.Contains("Allow User Variables") && !this.ConnectionString.Contains("AllowUserVariables"))
			//{
			//	this.ConnectionString += ";Allow User Variables=True";
			//}

			//if (!this.ConnectionString.Contains("Convert Zero Datetime") && !this.ConnectionString.Contains("ConvertZeroDateTime"))
			//{
			//	this.ConnectionString += ";Convert Zero Datetime=True";
			//}
		}


		#region PostgreSQL Implementation

		/// <summary>
		/// Convert a Table ElementBinding to Database Type string (Upper)
		/// </summary>
		/// <param name="bind">ElementBindind to Convert</param>
		/// <param name="table">DataTableHandler for Special cases</param>
		/// <returns>Database Type string ToUpper</returns>
		protected virtual string GetDatabaseType(ElementBinding bind, DataTableHandler table)
		{
			string type = null;
			// Check Value Type
			if (bind.ValueType == typeof(char))
			{
				type = "char";
			}
			else if (bind.ValueType == typeof(DateTime))
			{
				type = "timestamp without time zone";
			}
			else if (bind.ValueType == typeof(sbyte))
			{
				type = "smallint";
			}
			else if (bind.ValueType == typeof(short))
			{
				type = "smallint";
			}
			else if (bind.ValueType == typeof(int))
			{
				type = "integer";
			}
			else if (bind.ValueType == typeof(long))
			{
				type = "bigint";
			}
			else if (bind.ValueType == typeof(byte))
			{
				type = "smallint";
			}
			else if (bind.ValueType == typeof(ushort))
			{
				type = "integer";
			}
			else if (bind.ValueType == typeof(uint))
			{
				type = "integer";
			}
			else if (bind.ValueType == typeof(ulong))
			{
				type = "bigint";
			}
			else if (bind.ValueType == typeof(float))
			{
				// Float Value have less precision than C# Single.
				type = "real";
			}
			else if (bind.ValueType == typeof(double))
			{
				type = "double precision";
			}
			else if (bind.ValueType == typeof(bool))
			{
				type = "boolean";
			}
			else if (bind.ValueType == typeof(string))
			{
				if (bind.DataElement != null && bind.DataElement.Varchar > 0)
				{
					type = string.Format("character varying({0})", bind.DataElement.Varchar);
				}
				else if (table.Table.PrimaryKey.Any(key => key.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase))
						 || table.Table.Constraints.OfType<UniqueConstraint>().Any(cstrnt => cstrnt.Columns.Any(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase)))
						 || (table.Table.ExtendedProperties["INDEXES"] != null && (table.Table.ExtendedProperties["INDEXES"] as Dictionary<string, DataColumn[]>)
							 .Any(kv => kv.Value.Any(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase)))))
				{
					// If is in Primary Key Constraint or Unique Constraint or Index row, cast to Varchar.
					type = "character varying(255)";
				}
				else
				{
					type = "text";
				}
			}
			else
			{
				type = "bytea";
			}			

			return type;
		}

		/// <summary>
		/// Get Database Column Definition for ElementBinding
		/// </summary>
		/// <param name="bind">ElementBinding for Column Definition</param>
		/// <param name="table">DataTableHanlder for Special cases</param>
		/// <returns>Column Definitnion string.</returns>
		protected virtual string GetColumnDefinition(ElementBinding bind, DataTableHandler table)
		{
			string type = GetDatabaseType(bind, table);
			string defaultDef = null;
			Type[] numberTypes =
			{
				typeof(int), typeof(byte), typeof(bool), typeof(long), typeof(short),
				typeof(ushort), typeof(ulong), typeof(uint), typeof(double)
			};

			// Check for Default Value depending on Constraints and Type
			if (bind.PrimaryKey != null && bind.PrimaryKey.AutoIncrement)
			{
				if (bind.ValueType == typeof(ulong) || bind.ValueType == typeof(long))
					type = "bigserial";
				else
					type = "serial";

				defaultDef = ""; //$"NOT NULL DEFAULT nextval('{table.TableName}_{bind.ColumnName}_seq'::regclass)";
			}
			else if (bind.DataElement != null && bind.DataElement.AllowDbNull)
			{
				defaultDef = "DEFAULT NULL";
			}
			else if (bind.ValueType == typeof(DateTime))
			{
				defaultDef = "NOT NULL DEFAULT '2000-01-01 00:00:00'";
			}
			else if (bind.ValueType == typeof(bool))
            {
				defaultDef = "NOT NULL";
            }
			else if (numberTypes.Contains(bind.ValueType))
			{
				defaultDef = "NOT NULL DEFAULT 0";
			}
			else
			{
				defaultDef = "NOT NULL";
			}

			return string.Format("\"{0}\" {1} {2}", bind.ColumnName, type, defaultDef);
		}

		#endregion

		#region Create / Alter Table
		/// <summary>
		/// Check for Table Existence, Create or Alter accordingly
		/// </summary>
		/// <param name="table">Table Handler</param>
		public override void CheckOrCreateTableImpl(DataTableHandler table)
		{
			var currentTableColumns = new List<TableRowBindind>();
			try
			{
				ExecuteSelectImpl(string.Format(@"SELECT f.attname AS name, pg_catalog.format_type(f.atttypid,f.atttypmod) AS type, f.attnotnull AS notnull, CASE WHEN p.contype = 'p' THEN 't' ELSE 'f' END AS primarykey
								FROM pg_attribute f JOIN pg_class c ON c.oid = f.attrelid JOIN pg_type t ON t.oid = f.atttypid LEFT JOIN pg_attrdef d ON d.adrelid = c.oid AND d.adnum = f.attnum LEFT JOIN pg_namespace n ON n.oid = c.relnamespace  
								LEFT JOIN pg_constraint p ON p.conrelid = c.oid AND f.attnum = ANY (p.conkey) LEFT JOIN pg_class AS g ON p.confrelid = g.oid WHERE c.relkind = 'r'::char AND c.relname = '{0}' AND f.attnum > 0 ORDER BY f.attnum;", 
								table.TableName.ToLower()),
					new[] { new QueryParameter[] { } },
					reader =>
					{
						while (reader.Read())
						{
							var column = reader.GetString(0);
							var colType = reader.GetString(1);
							var allowNull = !reader.GetBoolean(2);
							var primary = reader.GetString(3).ToLower() == "t";
							currentTableColumns.Add(new TableRowBindind(column, colType, allowNull, primary));
							if (log.IsDebugEnabled)
								log.DebugFormat("CheckOrCreateTable: Found Column {0} in existing table {1}", column, table.TableName.ToLower());
						}
						if (log.IsDebugEnabled)
							log.DebugFormat("CheckOrCreateTable: {0} columns existing in table {1}", currentTableColumns.Count, table.TableName.ToLower());
					});
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("CheckOrCreateTable: ", e);
			}

			// Create Table or Alter Table
			if (currentTableColumns.Any())
			{
				AlterTable(currentTableColumns, table);
			}
			else
			{
				if (log.IsWarnEnabled)
					log.WarnFormat("Table {0} doesn't exist, creating it...", table.TableName.ToLower());

				CreateTable(table);
			}
		}


		/// <summary>
		/// Helper Method to build Table Definition String
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		protected string GetTableDefinition(DataTableHandler table)
		{
			var columnDef = table.FieldElementBindings
				.Select(bind => GetColumnDefinition(bind, table));

			var primaryFields = string.Format("CONSTRAINT {1} PRIMARY KEY ({0})",
											  string.Join(", ", table.Table.PrimaryKey.Select(pk => $"{FieldQualifier}{pk.ColumnName}{FieldQualifier}")),
											  string.Join(", ", table.Table.PrimaryKey.Select(pk => $"{FieldQualifier}{table.TableName.ToLower()}_{pk.ColumnName}_pkey{FieldQualifier}")));

			var uniqueFields = table.Table.Constraints.OfType<UniqueConstraint>().Where(cstrnt => !cstrnt.IsPrimaryKey)
				.Select(cstrnt => string.Format("CONSTRAINT \"{0}\" UNIQUE ({1})", cstrnt.ConstraintName,
												string.Join(", ", cstrnt.Columns.Select(col => $"{FieldQualifier}{col.ColumnName}{FieldQualifier}"))));

			var command = string.Format("CREATE TABLE IF NOT EXISTS \"{0}\" ({1})", table.TableName.ToLower(),
										string.Join(", \n", columnDef.Concat(new[] { primaryFields }).Concat(uniqueFields)));

			// Create Table First
			return command;
		}

		/// <summary>
		/// Helper Method to build Table Indexes Definition String
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		protected IEnumerable<string> GetIndexesDefinition(DataTableHandler table)
		{
			// Indexes and Constraints
			var uniqueFields = table.Table.Constraints.OfType<UniqueConstraint>().Where(cstrnt => !cstrnt.IsPrimaryKey)
				.Select(cstrnt => string.Format("CREATE UNIQUE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", cstrnt.ConstraintName,
												string.Join(", ", cstrnt.Columns.Select(col => string.Format("\"{0}\"", col.ColumnName))),
												table.TableName.ToLower()));

			var indexes = table.Table.ExtendedProperties["INDEXES"] as Dictionary<string, DataColumn[]>;

			var indexesFields = indexes == null ? new string[] { }
				: indexes.Select(index => string.Format("CREATE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", index.Key,
													string.Join(", ", index.Value.Select(col => string.Format("\"{0}\"", col.ColumnName))),
													table.TableName.ToLower()));

			return uniqueFields.Concat(indexesFields);
		}

		/// <summary>
		/// Create a New Table from DataTableHandler Definition
		/// </summary>
		/// <param name="table">DataTableHandler Definition to Create in Database</param>
		protected void CreateTable(DataTableHandler table)
		{
			ExecuteNonQueryImpl(GetTableDefinition(table));

			foreach (var commands in GetIndexesDefinition(table))
				ExecuteNonQueryImpl(commands);
		}

		/// <summary>
		/// Check if this Table need Alteration
		/// </summary>
		/// <param name="currentColumns">Current Existing Columns</param>
		/// <param name="table">DataTableHandler to Implement</param>
		protected bool CheckTableAlteration(IEnumerable<TableRowBindind> currentColumns, DataTableHandler table)
		{
			// Check for Any differences in Columns
			if (table.FieldElementBindings
				.Any(bind => {
					var column = currentColumns.FirstOrDefault(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase));

					if (column != null)
					{
						// Check Null
						if ((bind.DataElement != null && bind.DataElement.AllowDbNull) != column.AllowDbNull)
                        {
							log.Info($"NULL mismatch on column {column.ColumnName}. Current allow null: {column.AllowDbNull}, expected allow null: {bind.DataElement.AllowDbNull}");

							return true;
						}
							

						// Check Type
						if (!GetDatabaseType(bind, table).Equals(column.ColumnType, StringComparison.OrdinalIgnoreCase))
                        {
							log.Error($"Type mismatch on column {table.TableName}.{column.ColumnName}. Current type {column.ColumnType}, expected type {GetDatabaseType(bind, table)}");

							AlterColumn(table, column, bind);
						}
							

						// Field are identical
						return false;
					}
					// Field missing
					log.Info($"Missing column '{column.ColumnName}' on table '{table.TableName.ToLower()}'");
					return true;
				}))
				return true;

			// Check for Any Difference in Primary Keys
			if (table.Table.PrimaryKey.Length != currentColumns.Count(col => col.Primary)
				|| table.Table.PrimaryKey.Any(pk => {
					var column = currentColumns.FirstOrDefault(col => col.ColumnName.Equals(pk.ColumnName, StringComparison.OrdinalIgnoreCase));

					if (column != null && column.Primary)
						return false;

					log.Info($"PKEY mismatch with column '{column.ColumnName}' on table '{table.TableName.ToLower()}'");
					return true;
				}))
				return true;

			// No Alteration Needed
			return false;
		}

		/// <summary>
		/// Check if Table Indexes Need Alteration
		/// </summary>
		/// <param name="table">DataTableHandler to Implement</param>
		protected void CheckTableIndexAlteration(DataTableHandler table)
		{
			// Query Existing Indexes
			var currentIndexes = new List<Tuple<string, string>>();
			try
			{
				ExecuteSelectImpl("select indexname, indexdef from pg_indexes where schemaname = 'public' and tablename = @tableName and indexname not like '%_pkey'",
								  new[] { new[] { new QueryParameter("@tableName", table.TableName.ToLower()) } },
								  reader =>
								  {
									  while (reader.Read())
										  currentIndexes.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
								  });
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("CheckTableIndexAlteration: ", e);

				throw;
			}

			var sortedIndexes = currentIndexes.Select(ind => {
				var unique = ind.Item2.Trim().StartsWith("CREATE UNIQUE", StringComparison.OrdinalIgnoreCase);
				var columns = ind.Item2.Substring(ind.Item2.IndexOf('(')).Split(',').Select(sp => sp.Trim('\"', '(', ')', ' '));
				return new { KeyName = ind.Item1, Unique = unique, Columns = columns.ToArray() };
			}).ToArray();
			if (log.IsDebugEnabled)
				log.DebugFormat("CheckTableIndexAlteration: {0} Indexes existing in table {1}", sortedIndexes.Length, table.TableName.ToLower());

			var tableIndexes = table.Table.ExtendedProperties["INDEXES"] as Dictionary<string, DataColumn[]>;

			var alterQueries = new List<string>();

			// Check for Index Removal
			foreach (var existing in sortedIndexes)
			{
				if (log.IsDebugEnabled)
					log.DebugFormat("CheckTableIndexAlteration: Found Index \"{0}\" (Unique:{1}) on ({2}) in existing table {3}", existing.KeyName, existing.Unique, string.Join(", ", existing.Columns), table.TableName);

				DataColumn[] realindex;
				if (tableIndexes.TryGetValue(existing.KeyName, out realindex))
				{
					// Check for index modifications
					if (realindex.Length != existing.Columns.Length
						|| !realindex.All(col => existing.Columns.Any(c => c.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase))))
					{
						log.Info($"Dropping index {existing.KeyName} due to column mismatch");

						alterQueries.Add(string.Format("DROP INDEX \"{0}\"", existing.KeyName));
						alterQueries.Add(string.Format("CREATE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", existing.KeyName, string.Join(", ", realindex.Select(col => string.Format("\"{0}\"", col))), table.TableName.ToLower()));
					}
				}
				else
				{
					// Check for Unique
					var realunique = table.Table.Constraints.OfType<UniqueConstraint>().FirstOrDefault(cstrnt => !cstrnt.IsPrimaryKey && cstrnt.ConstraintName.Equals(existing.KeyName, StringComparison.OrdinalIgnoreCase));
					if (realunique == null)
					{
						log.Info($"Dropping unique index {existing.KeyName} because it was not found on the table definition");
						alterQueries.Add(string.Format("DROP INDEX \"{0}\"", existing.KeyName));
					}
					else if (realunique.Columns.Length != existing.Columns.Length
							 || !realunique.Columns.All(col => existing.Columns.Any(c => c.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase))))
					{
						log.Info($"Dropping unique index {existing.KeyName} due to column mismatch");

						alterQueries.Add(string.Format("DROP INDEX \"{0}\"", existing.KeyName));
						alterQueries.Add(string.Format("CREATE UNIQUE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", existing.KeyName, string.Join(", ", realunique.Columns.Select(col => string.Format("\"{0}\"", col))), table.TableName.ToLower()));
					}
				}
			}

			// Missing Indexes
			foreach (var missing in tableIndexes.Where(kv => sortedIndexes.All(c => !c.KeyName.Equals(kv.Key, StringComparison.OrdinalIgnoreCase))))
				alterQueries.Add(string.Format("CREATE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", missing.Key, string.Join(", ", missing.Value.Select(col => string.Format("\"{0}\"", col))), table.TableName.ToLower()));

			foreach (var missing in table.Table.Constraints.OfType<UniqueConstraint>().Where(cstrnt => !cstrnt.IsPrimaryKey && sortedIndexes.All(c => !c.KeyName.Equals(cstrnt.ConstraintName, StringComparison.OrdinalIgnoreCase))))
				alterQueries.Add(string.Format("CREATE UNIQUE INDEX IF NOT EXISTS \"{0}\" ON \"{2}\" ({1})", missing.ConstraintName, string.Join(", ", missing.Columns.Select(col => string.Format("\"{0}\"", col))), table.TableName.ToLower()));

			if (!alterQueries.Any())
				return;

			if (log.IsDebugEnabled)
				log.DebugFormat("Altering Table Indexes {0} this could take a few minutes...", table.TableName.ToLower());

			foreach (var query in alterQueries)
            {
				log.Info($"Altering indexes on table {table.TableName.ToLower()}: {query}");

				ExecuteNonQueryImpl(query);
			}
				
		}

		/// <summary>
		/// Alter an Existing Table to Match DataTableHandler Definition
		/// </summary>
		/// <param name="currentColumns">Current Existing Columns</param>
		/// <param name="table">DataTableHandler to Implement</param>
		protected void AlterTable(IEnumerable<TableRowBindind> currentColumns, DataTableHandler table)
		{
            // If Column are not modified Alter Table is not needed...
            if (!CheckTableAlteration(currentColumns, table))
            {
                // Table not Altered check for Indexes and return
                CheckTableIndexAlteration(table);
                return;
            }

            if (log.IsInfoEnabled)
				log.InfoFormat("Altering Table {0} this could take a few minutes...", table.TableName.ToLower());

			var currentIndexes = new List<string>();
			try
			{
				ExecuteSelectImpl("select indexname, indexdef from pg_indexes where schemaname = 'public' and tablename = @tableName",
								  new[] { new[] { new QueryParameter("@tableName", table.TableName.ToLower()) } },
								  reader =>
								  {
									  while (reader.Read())
										  currentIndexes.Add(reader.GetString(0));
								  });
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("AlterTableImpl: ", e);

				if (log.IsWarnEnabled)
					log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, no modifications...", table.TableName.ToLower());

				throw;
			}

			using (var conn = new NpgsqlConnection(ConnectionString))
			{
				conn.Open();
				using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
				{
					try
					{
						// Delete Indexes
						//foreach (var index in currentIndexes)
						//{
						//	using (var command = new NpgsqlCommand(string.Format("DROP INDEX \"{0}\"", index), conn))
						//	{
						//		command.Transaction = tran;
						//		command.ExecuteNonQuery();
						//	}
						//}

						// Rename Table
						using (var command = new NpgsqlCommand(string.Format("ALTER TABLE \"{0}\" RENAME TO \"{0}_bkp\"", table.TableName.ToLower()), conn))
						{
							command.Transaction = tran;
							command.ExecuteNonQuery();
						}

						// Create New Table
						using (var command = new NpgsqlCommand(GetTableDefinition(table), conn))
						{
							command.Transaction = tran;
							command.ExecuteNonQuery();
						}

						// Create Indexes
						foreach (var index in GetIndexesDefinition(table))
						{
							using (var command = new NpgsqlCommand(index, conn))
							{
								command.Transaction = tran;
								command.ExecuteNonQuery();
							}
						}

						// Copy Data, Convert Null to Default when needed...
						var matchingColumns = table.FieldElementBindings.Join(currentColumns, bind => bind.ColumnName, col => col.ColumnName, (bind, col) => new { bind, col }, StringComparer.OrdinalIgnoreCase);
						var columns = matchingColumns.Select(match => {
							if (match.bind.DataElement != null && match.bind.DataElement.AllowDbNull == false && match.col.AllowDbNull == true)
							{
								if (match.bind.ValueType == typeof(DateTime))
									return new { Target = match.bind.ColumnName, Source = string.Format("IFNULL(\"{0}\", {1})", match.bind.ColumnName, "'2000-01-01 00:00:00'") };
								if (match.bind.ValueType == typeof(string))
									return new { Target = match.bind.ColumnName, Source = string.Format("IFNULL(\"{0}\", {1})", match.bind.ColumnName, "''") };

								return new { Target = match.bind.ColumnName, Source = string.Format("IFNULL(\"{0}\", {1})", match.bind.ColumnName, "0") };
							}

							return new { Target = match.bind.ColumnName, Source = string.Format("\"{0}\"", match.bind.ColumnName) };
						});

						using (var command = new NpgsqlCommand(string.Format("INSERT INTO \"{0}\" ({1}) SELECT {2} FROM \"{0}_bkp\"", table.TableName.ToLower(), string.Join(", ", columns.Select(c => FieldQualifier + c.Target + FieldQualifier)), string.Join(", ", columns.Select(c => c.Source))), conn))
						{
							if (log.IsDebugEnabled)
								log.DebugFormat("AlterTableImpl, Insert/Select: {0}", command.CommandText);

							command.Transaction = tran;
							command.ExecuteNonQuery();
						}

						// Drop Renamed Table
						using (var command = new NpgsqlCommand(string.Format("DROP TABLE \"{0}_bkp\"", table.TableName.ToLower()), conn))
						{
							command.Transaction = tran;
							command.ExecuteNonQuery();
						}

						tran.Commit();
						if (log.IsInfoEnabled)
							log.InfoFormat("AlterTableImpl: Table {0} Altered...", table.TableName.ToLower());
					}
					catch (Exception e)
					{
						tran.Rollback();
						if (log.IsDebugEnabled)
							log.Debug("AlterTableImpl: ", e);

						if (log.IsWarnEnabled)
							log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, rollback...\n{1}", table.TableName.ToLower(), e);

						throw;
					}

				}
			}
		}

		protected void AlterColumn(DataTableHandler table, TableRowBindind currentColumn, ElementBinding expectedColumn)
        {
			using (var conn = new NpgsqlConnection(ConnectionString))
			{
				conn.Open();
				using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
				{
					try
					{
						//modify column type
						using (var command = new NpgsqlCommand())
						{
							command.Connection = conn;
							var newType = GetDatabaseType(expectedColumn, table).ToLower();
							if (newType == "boolean")
							{
								command.CommandText = String.Format(@"ALTER TABLE {0} ALTER COLUMN {1}{2}{1} DROP DEFAULT;
														ALTER TABLE {0} ALTER {1}{2}{1} TYPE bool USING CASE WHEN {1}{2}{1}=0 THEN FALSE ELSE TRUE END;
														ALTER TABLE {0} ALTER COLUMN {1}{2}{1} SET DEFAULT FALSE;",
														table.TableName.ToLower(),
														FieldQualifier,
														expectedColumn.ColumnName);
							}
                            else
                            {
								command.CommandText = String.Format("ALTER TABLE {0} ALTER {1}{2}{1} TYPE {3};",
														table.TableName.ToLower(),
														FieldQualifier,
														expectedColumn.ColumnName,
														newType);
							}
							

							command.Transaction = tran;
							command.ExecuteNonQuery();
						}

						tran.Commit();
						if (log.IsInfoEnabled)
							log.InfoFormat("AlterTableImpl: Table {0} Altered...", table.TableName.ToLower());
					}
					catch (Exception e)
					{
						tran.Rollback();
						if (log.IsDebugEnabled)
							log.Debug("AlterTableImpl: ", e);

						if (log.IsWarnEnabled)
							log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, rollback...\n{1}", table.TableName.ToLower(), e);

						throw;
					}

				}
			}

			
        }
		#endregion

		#region Property implementation
		/// <summary>
		/// The connection type to DB (xml, mysql,...)
		/// </summary>
		public override ConnectionType ConnectionType { get { return ConnectionType.DATABASE_POSTGRESQL; } }

		public override string FieldQualifier => "\"";
		#endregion

		#region SQLObject Implementation
		/// <summary>
		/// Implementation of Scalar Query with Parameters for Prepared Query
		/// </summary>
		/// <param name="SQLCommand">Scalar Command</param>
		/// <param name="parameters">Collection of Parameters for Single/Multiple Read</param>
		/// <param name="retrieveLastInsertID">Return Last Insert ID of each Command instead of Scalar</param>
		/// <returns>Objects Returned by Scalar</returns>
		protected override object[] ExecuteScalarImpl(string SQLCommand, IEnumerable<IEnumerable<QueryParameter>> parameters, bool retrieveLastInsertID)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat("ExecuteScalarImpl: {0}", SQLCommand);

			SQLCommand = Escape(SQLCommand);

			var obj = new List<object>();
			var repeat = false;
			var current = 0;
			do
			{
				repeat = false;

				if (!parameters.Any()) throw new ArgumentException("No parameter list was given.");

				using (var conn = new NpgsqlConnection(ConnectionString))
				{
					using (var cmd = conn.CreateCommand())
					{
						try
						{
							cmd.CommandText = SQLCommand;
							conn.Open();
							long start = (DateTime.UtcNow.Ticks / 10000);

							foreach (var parameter in parameters.Skip(current))
							{
								FillSQLParameter(parameter, cmd.Parameters);
								cmd.Prepare();

								if (retrieveLastInsertID)
								{
									using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
									{
										try
										{
											cmd.Transaction = tran;
											try
											{
												cmd.ExecuteNonQuery();

												cmd.CommandText = "SELECT lastval();";
												cmd.Parameters.Clear();

												obj.Add(cmd.ExecuteScalar());
											}
											catch (Exception ex)
											{
												if (HandleSQLException(ex))
												{
													obj.Add(-1);
													if (log.IsErrorEnabled)
														log.ErrorFormat("ExecuteScalarImpl: Constraint Violation for command \"{0}\"\n{1}\n{2}", SQLCommand, ex, Environment.StackTrace);
												}
												else
												{
													throw;
												}
											}
											tran.Commit();
										}
										catch (Exception te)
										{
											tran.Rollback();
											if (log.IsErrorEnabled)
												log.ErrorFormat("ExecuteScalarImpl: Error in Transaction (Rollback) for command : {0}\n{1}", SQLCommand, te);
										}
									}
								}
								else
								{
									var result = cmd.ExecuteScalar();
									obj.Add(result);
								}
								current++;
							}

							if (log.IsDebugEnabled)
								log.DebugFormat("ExecuteScalarImpl: SQL ScalarQuery exec time {0}ms", ((DateTime.UtcNow.Ticks / 10000) - start));
							else if (log.IsWarnEnabled && (DateTime.UtcNow.Ticks / 10000) - start > 500)
								log.WarnFormat("ExecuteScalarImpl: SQL ScalarQuery took {0}ms!\n{1}", ((DateTime.UtcNow.Ticks / 10000) - start), SQLCommand);
						}
						catch (Exception e)
						{
							if (!HandleException(e))
							{
								if (log.IsErrorEnabled)
									log.ErrorFormat("ExecuteScalarImpl: UnHandled Exception for command \"{0}\"\n{1}", SQLCommand, e);

								throw;
							}
							repeat = true;
						}
						finally
						{
							CloseConnection(conn);
						}
					}
				}
			}
			while (repeat);

			return obj.ToArray();
		}

        protected override IList<IList<DataObject>> SelectObjectsImpl(DataTableHandler tableHandler, string whereExpression, IEnumerable<IEnumerable<QueryParameter>> parameters, IsolationLevel isolation)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.SelectObjectsImpl(tableHandler, whereExpression, parameters, isolation);
        }

        protected override IEnumerable<bool> AddObjectImpl(DataTableHandler tableHandler, IEnumerable<DataObject> dataObjects)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.AddObjectImpl(tableHandler, dataObjects);
        }

        protected override IEnumerable<bool> SaveObjectImpl(DataTableHandler tableHandler, IEnumerable<DataObject> dataObjects)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.SaveObjectImpl(tableHandler, dataObjects);
        }

        protected override IEnumerable<bool> DeleteObjectImpl(DataTableHandler tableHandler, IEnumerable<DataObject> dataObjects)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.DeleteObjectImpl(tableHandler, dataObjects);
        }

        protected override IEnumerable<DataObject> FindObjectByKeyImpl(DataTableHandler tableHandler, IEnumerable<object> keys)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.FindObjectByKeyImpl(tableHandler, keys);
        }

        protected override IList<IList<DataObject>> MultipleSelectObjectsImpl(DataTableHandler tableHandler, IEnumerable<WhereClause> whereClauseBatch)
        {
            tableHandler.TableName = tableHandler.TableName.ToLower();
            return base.MultipleSelectObjectsImpl(tableHandler, whereClauseBatch);
        }

		protected override int GetObjectCountImpl<TObject>(string whereExpression)
		{
			string tableName = AttributesUtils.GetTableOrViewName(typeof(TObject));
			DataTableHandler tableHandler;
			if (!TableDatasets.TryGetValue(tableName, out tableHandler))
				throw new DatabaseException(string.Format("Table {0} is not registered for Database Connection...", tableName));

			string command = null;
			if (string.IsNullOrEmpty(whereExpression))
				command = string.Format("SELECT COUNT(*) FROM {1}{0}{1}", tableName.ToLower(), FieldQualifier);
			else
				command = string.Format("SELECT COUNT(*) FROM {2}{0}{2} WHERE {1}", tableName.ToLower(), whereExpression, FieldQualifier);

			var count = ExecuteScalarImpl(command);

			return count is long ? (int)((long)count) : (int)count;
		}

		#endregion

		protected override DbConnection CreateConnection(string connectionsString)
		{
			return new NpgsqlConnection(ConnectionString);
		}

		protected override void CloseConnection(DbConnection connection)
		{
			connection.Close();
		}

		protected override DbParameter ConvertToDBParameter(QueryParameter queryParameter)
		{
			var dbParam = new NpgsqlParameter();
			dbParam.ParameterName = queryParameter.Name;

			if (queryParameter.Value is char)
				dbParam.Value = Convert.ToUInt16(queryParameter.Value);
			else if (queryParameter.Value is uint)
				dbParam.Value = Convert.ToInt64(queryParameter.Value);
			else if (queryParameter.Value is ulong)
				dbParam.Value = unchecked((long)Convert.ToUInt64(queryParameter.Value));
			else if (queryParameter.Value is ushort)
				dbParam.Value = Convert.ToInt32(queryParameter.Value);
			else if (queryParameter.Value == null)
				dbParam.Value = string.Empty;
			else
				dbParam.Value = queryParameter.Value;

			return dbParam;
		}

		/// <summary>
		/// Handle Non Fatal SQL Query Exception
		/// </summary>
		/// <param name="e">SQL Excepiton</param>
		/// <returns>True if handled, False otherwise</returns>
		protected override bool HandleSQLException(Exception e)
		{
			if (e is NpgsqlException sqle)
			{				
				switch (sqle.ErrorCode.ToString())
				{
					case PostgresErrorCodes.IntegrityConstraintViolation:
					case PostgresErrorCodes.ForeignKeyViolation:
					case PostgresErrorCodes.UniqueViolation:
					case PostgresErrorCodes.NotNullViolation:
						return true;
					default:
						return false;
				}
			}
			return false;
		}

		/// <summary>
		/// escape the strange character from string
		/// </summary>
		/// <param name="rawInput">the string</param>
		/// <returns>the string with escaped character</returns>
		public override string Escape(string rawInput)
		{
			return rawInput.Replace("'", "''");
		}
	}
}
