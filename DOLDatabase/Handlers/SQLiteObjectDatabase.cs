﻿/*
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
using System.Reflection;
using System.Data;
using DataTable = System.Data.DataTable;

using DOL.Database.Connection;
using DOL.Database.Transaction;
using IsolationLevel = DOL.Database.Transaction.IsolationLevel;

using System.Data.SQLite;

using log4net;

namespace DOL.Database.Handlers
{
	public class SQLiteObjectDatabase : SQLObjectDatabase
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Create a new instance of <see cref="SQLiteObjectDatabase"/>
		/// </summary>
		/// <param name="ConnectionString">Database Connection String</param>
		public SQLiteObjectDatabase(string ConnectionString)
			: base(ConnectionString)
		{
		}
		
		#region SQLite Implementation
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
			if (bind.ValueType == typeof(char)
			    || bind.ValueType == typeof(sbyte)
			    || bind.ValueType == typeof(short)
			    || bind.ValueType == typeof(int)
			    || bind.ValueType == typeof(long)
			    || bind.ValueType == typeof(byte)
			    || bind.ValueType == typeof(ushort)
			    || bind.ValueType == typeof(uint)
			    || bind.ValueType == typeof(ulong)
			    || bind.ValueType == typeof(bool)
			  )
			{
				type = "INTEGER";
			}
			else if (bind.ValueType == typeof(DateTime))
			{
				type = "DATETIME";
			}
			else if (bind.ValueType == typeof(float))
			{
				type = "FLOAT";
			}
			else if (bind.ValueType == typeof(double))
			{
				type = "DOUBLE";
			}
			else if (bind.ValueType == typeof(string))
			{
				if (bind.DataElement != null && bind.DataElement.Varchar > 0)
				{
					type = string.Format("VARCHAR({0})", bind.DataElement.Varchar);
				}
				else if (table.Table.PrimaryKey.Any(key => key.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase))
				         || table.Table.Constraints.OfType<UniqueConstraint>().Any(cstrnt => cstrnt.Columns.Any(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase)))
				         || (table.Table.ExtendedProperties["INDEXES"] != null && (table.Table.ExtendedProperties["INDEXES"] as Dictionary<string, DataColumn[]>)
				             .Any(kv => kv.Value.Any(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase)))))
				{
					// If is in Primary Key Constraint or Unique Constraint or Index row, cast to Varchar.
					type = "VARCHAR(255)";
				}
				else
				{
					type = "TEXT";
				}
			}
			else
			{
				type = "BLOB";
			}
			
			if (bind.PrimaryKey != null && bind.PrimaryKey.AutoIncrement)
				type = "INTEGER";
			
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
						
			// Check for Default Value depending on Constraints and Type
			if (bind.PrimaryKey != null && bind.PrimaryKey.AutoIncrement)
			{
				defaultDef = "NOT NULL PRIMARY KEY AUTOINCREMENT";
			}
			else if (bind.DataElement != null && bind.DataElement.AllowDbNull)
			{
				defaultDef = "DEFAULT NULL";
			}
			else if (bind.ValueType == typeof(DateTime))
			{
				defaultDef = "NOT NULL DEFAULT '2000-01-01 00:00:00'";
			}
			else
			{
				defaultDef = "NOT NULL";
			}
			
			return string.Format("`{0}` {1} {2}", bind.ColumnName, type, defaultDef);
		}
		#endregion
		
		#region Create / Alter Table
		/// <summary>
		/// Check for Table Existence, Create or Alter accordingly
		/// </summary>
		/// <param name="table">Table Handler</param>
		protected override void CheckOrCreateTableImpl(DataTableHandler table)
		{
			var currentTableColumns = new List<TableRowBindind>();
			try
			{
				ExecuteSelectImpl(string.Format("PRAGMA TABLE_INFO(`{0}`)", table.TableName),
				                  reader =>
				                  {
				                  	while (reader.Read())
				                  	{
				                  		var column = reader.GetString(1);
				                  		var colType = reader.GetString(2);
				                  		var allowNull = !reader.GetBoolean(3);
				                  		var primary = reader.GetInt64(5) > 0;
				                  		currentTableColumns.Add(new TableRowBindind(column, colType, allowNull, primary));
				                  		if (log.IsDebugEnabled)
				                  			log.DebugFormat("CheckOrCreateTable: Found Column {0} in existing table {1}", column, table.TableName);
				                  	}
				                  	if (log.IsDebugEnabled)
				                  		log.DebugFormat("CheckOrCreateTable: {0} columns in table existing", currentTableColumns.Count);
				                  }, IsolationLevel.DEFAULT);
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("CheckOrCreateTable: ", e);

				if (log.IsWarnEnabled)
					log.WarnFormat("Table {0} doesn't exist, creating it...", table.TableName);
			}
			
			// Create Table or Alter Table
			if (currentTableColumns.Any())
				AlterTable(currentTableColumns, table);
			else
				CreateTable(table);
		}
		
		/// <summary>
		/// Create a New Table from DataTableHandler Definition
		/// </summary>
		/// <param name="table">DataTableHandler Definition to Create in Database</param>
		protected void CreateTable(DataTableHandler table)
		{
			var columnDef = table.FieldElementBindings
				.Select(bind => GetColumnDefinition(bind, table));
			
			var primaryFields = new string[]{};
			if (!table.FieldElementBindings.Any(bind => bind.PrimaryKey != null && bind.PrimaryKey.AutoIncrement))
				primaryFields = new [] { string.Format("PRIMARY KEY ({0})",
				                              string.Join(", ", table.Table.PrimaryKey.Select(pk => string.Format("`{0}`", pk.ColumnName)))) };
			
			// Create Table First
			var command = string.Format("CREATE TABLE IF NOT EXISTS `{0}` ({1})", table.TableName,
			                            string.Join(", \n", columnDef.Concat(primaryFields)));

			ExecuteNonQueryImpl(command);
			
			// Then Indexes and Constraints
			var uniqueFields = table.Table.Constraints.OfType<UniqueConstraint>().Where(cstrnt => !cstrnt.IsPrimaryKey)
				.Select(cstrnt => string.Format("CREATE UNIQUE INDEX IF NOT EXISTS `{0}` ON `{2}` ({1})", cstrnt.ConstraintName,
				                                string.Join(", ", cstrnt.Columns.Select(col => string.Format("`{0}`", col.ColumnName))),
				                                table.TableName));
			
			var indexes = table.Table.ExtendedProperties["INDEXES"] as Dictionary<string, DataColumn[]>;
			
			var indexesFields = indexes == null ? new string[] { }
				: indexes.Select(index => string.Format("CREATE INDEX IF NOT EXISTS `{0}` ON `{2}` ({1})", index.Key,
			                                        string.Join(", ", index.Value.Select(col => string.Format("`{0}`", col.ColumnName))),
			                                        table.TableName));

			foreach (var commands in uniqueFields.Concat(indexesFields))
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
			         			return true;
			         		
			         		// Check Type
			         		if (!GetDatabaseType(bind, table).Equals(column.ColumnType, StringComparison.OrdinalIgnoreCase))
			         			return true;
			         		
			         		// Field are identical
			         		return false;
			         	}
			         	// Field missing
			         	return true;
			         }))
				return true;
			
			// Check for Any Difference in Primary Keys
			if (table.Table.PrimaryKey.Count() != currentColumns.Count(col => col.Primary)
				|| table.Table.PrimaryKey.Any(pk => {
			                                  	var column = currentColumns.FirstOrDefault(col => col.ColumnName.Equals(pk.ColumnName, StringComparison.OrdinalIgnoreCase));
			                                  	
			                                  	if (column != null && column.Primary)
			                                  		return false;
			                                  	
			                                  	return true;
			                                  }))
				return true;
			
			// No Alteration Needed
			return false;
		}
		
		/// <summary>
		/// Alter an Existing Table to Match DataTableHandler Definition
		/// </summary>
		/// <param name="currentColumns">Current Existing Columns</param>
		/// <param name="table">DataTableHandler to Implement</param>
		protected void AlterTable(IEnumerable<TableRowBindind> currentColumns, DataTableHandler table)
		{
			// TODO : Check for Index Alteration
			if  (!CheckTableAlteration(currentColumns, table))
				return;
			
			if (log.IsDebugEnabled)
				log.DebugFormat("Altering Table {0} this could take a few minutes...", table.TableName);
			
			// Rename Table
			ExecuteNonQueryImpl(string.Format("ALTER TABLE `{0}` RENAME TO `{0}_bkp`", table.TableName));
			
			// Drop Indexes
			var currentIndexes = new List<string>();
			try
			{
				ExecuteSelectImpl("SELECT name FROM sqlite_master WHERE type == 'index' AND sql is not null AND tbl_name == @tableName",
				                  new KeyValuePair<string, object>("@tableName", string.Format("{0}_bkp", table.TableName)),
				                  reader =>
				                  {
				                  	while (reader.Read())
				                  		currentIndexes.Add(reader.GetString(0));
				                  }, IsolationLevel.DEFAULT);
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("AlterTableImpl: ", e);

				if (log.IsWarnEnabled)
					log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, trying to rollback...", table.TableName);
				
				// Rename Table before any modifications
				ExecuteNonQueryImpl(string.Format("ALTER TABLE `{0}_bkp` RENAME TO `{0}`", table.TableName));
				throw;
			}
			
			foreach(var index in currentIndexes)
				ExecuteNonQueryImpl(string.Format("DROP INDEX `{0}`", index));
			
			try
			{
				// Create New Table with Indexes
				CreateTable(table);
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("AlterTableImpl: ", e);
				
				if (log.IsWarnEnabled)
					log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, trying to rollback...", table.TableName);
				
				
				// Rename Table as before any modifications
				ExecuteNonQueryImpl(string.Format("ALTER TABLE `{0}_bkp` RENAME TO `{0}`", table.TableName));
				throw;
			}
			
			try
			{
				// Copy Data
				var columns = table.FieldElementBindings.Where(bind => currentColumns.Any(col => col.ColumnName.Equals(bind.ColumnName, StringComparison.OrdinalIgnoreCase)))
					.Select(bind => string.Format("`{0}`", bind.ColumnName));
				
				ExecuteNonQueryImpl(string.Format("INSERT INTO `{0}` ({1}) SELECT {1} FROM `{0}_bkp`", table.TableName, string.Join(", ", columns)));
			}
			catch (Exception e)
			{
				if (log.IsDebugEnabled)
					log.Debug("AlterTableImpl: ", e);
				
				if (log.IsWarnEnabled)
					log.WarnFormat("AlterTableImpl: Error While Altering Table {0}, trying to rollback...", table.TableName);
				
				// Roll Back Creation
				ExecuteNonQueryImpl(string.Format("DROP TABLE `{0}`", table.TableName));
				// Rename Table as before any modifications
				ExecuteNonQueryImpl(string.Format("ALTER TABLE `{0}_bkp` RENAME TO `{0}`", table.TableName));
				throw;
			}
			
			// Drop Renamed Table
			ExecuteNonQueryImpl(string.Format("DROP TABLE `{0}_bkp`", table.TableName));
		}
		#endregion

		#region Property implementation
		/// <summary>
		/// The connection type to DB (xml, mysql,...)
		/// </summary>
		public override ConnectionType ConnectionType { get { return ConnectionType.DATABASE_SQLITE; } }
		#endregion
		
		#region SQLObject Implementation
		/// <summary>
		/// Raw SQL Select Implementation with Parameters for Prepared Query
		/// </summary>
		/// <param name="SQLCommand">Command for reading</param>
		/// <param name="parameters">Collection of Parameters for Single/Multiple Read</param>
		/// <param name="Reader">Reader Method</param>
		/// <param name="Isolation">Transaction Isolation</param>
		protected override void ExecuteSelectImpl(string SQLCommand, IEnumerable<IEnumerable<KeyValuePair<string, object>>> parameters, Action<IDataReader> Reader, IsolationLevel Isolation)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat("SQL: {0}", SQLCommand);

			var repeat = false;
			var current = 0;
			do
			{
				repeat = false;

				using (var conn = new SQLiteConnection(ConnectionString))
				{
				    using (var cmd = new SQLiteCommand(SQLCommand, conn))
					{
						try
						{
					    	conn.Open();
					    	cmd.Prepare();
					    	
					    	long start = (DateTime.UtcNow.Ticks / 10000);
					    	
					    	// Register Parameter
					    	foreach(var keys in parameters.First().Select(kv => kv.Key))
					    		cmd.Parameters.Add(new SQLiteParameter(keys));
					    	
					    	foreach(var parameter in parameters.Skip(current))
					    	{
					    		foreach(var param in parameter)
					    			cmd.Parameters[param.Key].Value = param.Value;
					    	
							    using (var reader = cmd.ExecuteReader())
							    {
							    	try
							    	{
							        	Reader(reader);
							    	}
							    	catch (Exception es)
							    	{
							    		if(log.IsWarnEnabled)
							    			log.Warn("Exception in Select Callback : ", es);
							    	}
							    	finally
							    	{
							    		reader.Close();
							    	}
							    }
							    current++;
					    	}
					    
						    if (log.IsDebugEnabled)
								log.DebugFormat("SQL Select ({0}) exec time {1}ms", Isolation, ((DateTime.UtcNow.Ticks / 10000) - start));
							else if ((DateTime.UtcNow.Ticks / 10000) - start > 500 && log.IsWarnEnabled)
								log.WarnFormat("SQL Select ({0}) took {1}ms!\n{2}", Isolation, ((DateTime.UtcNow.Ticks / 10000) - start), SQLCommand);
						
						}
						catch (Exception e)
						{
							if (!HandleException(e))
							{
								if (log.IsErrorEnabled)
									log.ErrorFormat("ExecuteSelect: \"{0}\"\n{1}", SQLCommand, e);
								
								throw;
							}
							repeat = true;
						}
						finally
						{
							conn.Close();
						}
				    }
				}
			}
			while (repeat);
		}
		
		/// <summary>
		/// Implementation of Raw Non-Query with Parameters for Prepared Query
		/// </summary>
		/// <param name="SQLCommand">Raw Command</param>
		/// <param name="parameters">Collection of Parameters for Single/Multiple Read</param>
		/// <returns>True if the Command succeeded</returns>
		protected override int[] ExecuteNonQueryImpl(string SQLCommand, IEnumerable<IEnumerable<KeyValuePair<string, object>>> parameters)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat("ExecuteNonQueryImpl: {0}", SQLCommand);

			var affected = new List<int>();
			var repeat = false;
			var current = 0;
			do
			{
				repeat = false;

				using (var conn = new SQLiteConnection(ConnectionString))
				{
					using (var cmd = new SQLiteCommand(SQLCommand, conn))
					{
						try
						{
					    	conn.Open();
					    	cmd.Prepare();
						    
					    	long start = (DateTime.UtcNow.Ticks / 10000);
						    
					    	// Register Parameter
					    	foreach(var keys in parameters.First().Select(kv => kv.Key))
					    		cmd.Parameters.Add(new SQLiteParameter(keys));
					    	
					    	foreach(var parmeter in parameters.Skip(current))
					    	{
					    		foreach(var param in parmeter)
					    			cmd.Parameters[param.Key].Value = param.Value;
					    	
							    var result = cmd.ExecuteNonQuery();
							    affected.Add(result);
							    current++;
							    
							    if (result < 1 && log.IsWarnEnabled)
							    	log.WarnFormat("ExecuteNonQuery: No Change for raw query \"{0}\"", SQLCommand);
					    	}
						    
						    
						    if (log.IsDebugEnabled)
								log.DebugFormat("SQL NonQuery exec time {0}ms", ((DateTime.UtcNow.Ticks / 10000) - start));
							else if ((DateTime.UtcNow.Ticks / 10000) - start > 500 && log.IsWarnEnabled)
								log.WarnFormat("SQL NonQuery took {0}ms!\n{1}", ((DateTime.UtcNow.Ticks / 10000) - start), SQLCommand);
						}
						catch (Exception e)
						{
							if (!HandleException(e))
							{
								if(log.IsErrorEnabled)
									log.ErrorFormat("ExecuteNonQuery: \"{0}\"\n{1}", SQLCommand, e);
								
								throw;
							}
							repeat = true;
						}
						finally
						{
							conn.Close();
						}
					}
				}
			} 
			while (repeat);
			
			return affected.ToArray();
		}

		/// <summary>
		/// Implementation of Scalar Query with Parameters for Prepared Query
		/// </summary>
		/// <param name="SQLCommand">Scalar Command</param>
		/// <param name="parameters">Collection of Parameters for Single/Multiple Read</param>
		/// <param name="retrieveLastInsertID">Return Last Insert ID of each Command instead of Scalar</param>
		/// <returns>Objects Returned by Scalar</returns>
		protected override object[] ExecuteScalarImpl(string SQLCommand, IEnumerable<IEnumerable<KeyValuePair<string, object>>> parameters, bool retrieveLastInsertID)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat("ExecuteScalarImpl: {0}", SQLCommand);
			
			var obj = new List<object>();
			var repeat = false;
			var current = 0;
			do
			{
				repeat = false;
				
				using (var conn = new SQLiteConnection(ConnectionString))
				{					    
					using (var cmd = new SQLiteCommand(SQLCommand, conn))
					{
						try
						{
						    conn.Open();
					    	cmd.Prepare();
						    long start = (DateTime.UtcNow.Ticks / 10000);

						    // Register Parameter
					    	foreach(var keys in parameters.First().Select(kv => kv.Key))
					    		cmd.Parameters.Add(new SQLiteParameter(keys));
					    	
					    	foreach(var parmeter in parameters.Skip(current))
					    	{
					    		foreach(var param in parmeter)
					    			cmd.Parameters[param.Key].Value = param.Value;
					    		
					    		if (retrieveLastInsertID)
					    		{
					    			using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
					    			{
					    				try
					    				{
						    				cmd.Transaction = tran;
						    				cmd.ExecuteNonQuery();
						    				
							    			using (var lastid = new SQLiteCommand("SELECT LAST_INSERT_ROWID()", conn))
							    			{
							    				var result = lastid.ExecuteScalar();
							    				obj.Add(result);
							    			}
							    			
							    			tran.Commit();
					    				}
					    				catch (Exception te)
					    				{
					    					tran.Rollback();
					    					if (log.IsErrorEnabled)
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
								log.DebugFormat("SQL ScalarQuery exec time {0}ms", ((DateTime.UtcNow.Ticks / 10000) - start));
							else if ((DateTime.UtcNow.Ticks / 10000) - start > 500 && log.IsWarnEnabled)
								log.WarnFormat("SQL ScalarQuery took {0}ms!\n{1}", ((DateTime.UtcNow.Ticks / 10000) - start), SQLCommand);
						}
						catch (Exception e)
						{
	
							if (!HandleException(e))
							{
								if(log.IsErrorEnabled)
									log.ErrorFormat("ExecuteScalar: \"{0}\"\n{1}", SQLCommand, e);
								
								throw;
							}
	
							repeat = true;
						}
						finally
						{
							conn.Close();
						}
					}
				}
			} 
			while (repeat);

			return obj.ToArray();
		}
		#endregion
		
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
