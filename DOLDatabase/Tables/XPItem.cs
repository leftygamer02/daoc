using DOL.Database.Attributes;

namespace DOL.Database
{
	/// <summary>
	/// Table that holds the mapping of XP items
	/// </summary>
	[DataTable(TableName = "XPItem")]
	public class XPItem : DataObject
	{
		//important data
		public string m_mobName;
		public int m_mobRegion;
		public string m_itemName;
		public string m_itemTemplate;
		public int m_minLevel;
		public int m_maxLevel;
		public int m_realm;
		public int m_id;
		
		/// <summary>
		/// Primary Key Auto Increment.
		/// </summary>
		[PrimaryKey(AutoIncrement = true)]
		public int XPItemID
		{
			get { return m_id; }
			set
			{
				Dirty = true;
				m_id = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string MobName
		{
			get { return m_mobName; }
			set
			{
				Dirty = true;
				m_mobName = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MobRegion
		{
			get { return m_mobRegion; }
			set
			{
				Dirty = true;
				m_mobRegion = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string ItemName
		{
			get { return m_itemName; }
			set
			{
				Dirty = true;
				m_itemName = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string ItemTemplate
		{
			get { return m_itemTemplate; }
			set
			{
				Dirty = true;
				m_itemTemplate = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MinLevel
		{
			get { return m_minLevel; }
			set
			{
				Dirty = true;
				m_minLevel = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MaxLevel
		{
			get { return m_maxLevel; }
			set
			{
				Dirty = true;
				m_maxLevel = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int Realm
		{
			get { return m_realm; }
			set
			{
				Dirty = true;
				m_realm = value;
			}
		}
	}
}