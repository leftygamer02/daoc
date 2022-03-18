/*
 *
 * Atlas - XP Items 
 * 
 */

using DOL.Database.Attributes;

namespace DOL.Database
{
	/// <summary>
	/// Database Storage of Tasks
	/// </summary>
	[DataTable(TableName="XPItems")]
	public class DBXPItems : DataObject
	{
		protected int	m_xpitemid;
		protected int m_realm;
		protected string m_mobname;
		protected string m_mobregion;
		protected string m_itemname;
		protected int m_minlevel;
		protected int m_maxlevel;
		protected string m_npcname;
		protected string m_npcregion;

		public DBXPItems()
		{
		}

		[PrimaryKey(AutoIncrement=true)]
		public int XPItemID
		{
			get {return m_xpitemid;}
			set
			{
				Dirty = true;
				m_xpitemid = value;
			}
		}
		
		[DataElement(AllowDbNull=false, Unique=false)]
		public int Realm
		{
			get {return m_realm;}
			set
			{
				Dirty = true;
				m_realm = value;
			}
		}
		

		[DataElement(AllowDbNull=false,Unique=false)]
		public string MobName
		{
			get {return m_mobname;}
			set
			{
				Dirty = true;
				m_mobname = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public string MobRegion
		{
			get {return m_mobregion;}
			set
			{
				Dirty = true;
				m_mobregion = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public string ItemName
		{
			get {return m_itemname;}
			set
			{
				Dirty = true;
				m_itemname = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public int MinLevel
		{
			get {return m_minlevel;}
			set
			{
				Dirty = true;
				m_minlevel = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public int MaxLevel
		{
			get {return m_maxlevel;}
			set
			{
				Dirty = true;
				m_maxlevel = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public string NPCName
		{
			get {return m_npcname;}
			set
			{
				Dirty = true;
				m_npcname = value;
			}
		}
		
		[DataElement(AllowDbNull=false,Unique=false)]
		public string NPCRegion
		{
			get {return m_npcregion;}
			set
			{
				Dirty = true;
				m_npcregion = value;
			}
		}
	}
	
}
