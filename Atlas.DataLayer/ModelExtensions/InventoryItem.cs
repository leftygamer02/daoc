using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.DataLayer.Models
{
    public partial class InventoryItem
    {
		public const string BLANK_ITEM = "blank_item";

		public InventoryItem(ItemTemplate template)
        {
			this.CharacterID = null;
			this.ItemTemplate = template;
			this.ItemTemplateID = template.Id;
			this.Color = template.Color;
			this.Emblem = template.Emblem;
			this.Count = template.PackSize;
			this.Extension = template.Extension;
			this.Condition = template.MaxCondition;
			this.Durability = template.MaxDurability;
			this.ItemBonus = template.ItemBonus;

			this.Spells = template.Spells.Select(x => new InventoryItemSpell()
			{
				Charges = x.Charges,
				IsPoison = x.IsPoison,
				MaxCharges = x.MaxCharges,
				ProcChance = x.ProcChance,
				SpellID = x.SpellID
			}).ToList();

			this.Bonuses = template.Bonuses.ToList();
		}

		public InventoryItem(ItemUnique template)
		{
			this.CharacterID = null;
			this.ItemTemplate = template;
			this.ItemTemplateID = template.Id;
			this.Color = template.Color;
			this.Emblem = template.Emblem;
			this.Count = template.PackSize;
			this.Extension = template.Extension;
			this.Condition = template.MaxCondition;
			this.Durability = template.MaxDurability;
			this.ItemBonus = template.ItemBonus;

			this.Spells = template.Spells.Select(x => new InventoryItemSpell()
			{
				Charges = x.Charges,
				IsPoison = x.IsPoison,
				MaxCharges = x.MaxCharges,
				ProcChance = x.ProcChance,
				SpellID = x.SpellID
			}).ToList();

			this.Bonuses = template.Bonuses.ToList();
		}

		public InventoryItem(InventoryItem template)
        {
			ItemTemplate = template.ItemTemplate;
			ItemTemplateID = template.ItemTemplateID;
			ItemUniqueID = template.ItemUniqueID;
			Color = template.Color;
			Extension = template.Extension;
			SlotPosition = template.SlotPosition;
			Count = template.Count;
			Creator = template.Creator;
			IsCrafted = template.IsCrafted;
			SellPrice = template.SellPrice;
			Condition = template.Condition;
			Durability = template.Durability;
			Emblem = template.Emblem;
			Cooldown = template.Cooldown;
			Experience = template.Experience;
			OwnerLot = template.OwnerLot;
			ItemBonus = template.ItemBonus;

			this.Spells = template.Spells.Select(x => new InventoryItemSpell()
			{
				Charges = x.Charges,
				IsPoison = x.IsPoison,
				MaxCharges = x.MaxCharges,
				ProcChance = x.ProcChance,
				SpellID = x.SpellID
			}).ToList();

			this.Bonuses = template.Bonuses.ToList();

		}

		[NotMapped]
		public string Name 
		{ 
			get { return this.ItemTemplate.Name; } 
			set { this.ItemTemplate.Name = value; }
		}

		[NotMapped]
		public bool AllowAdd { get; set; }

		[NotMapped]
		public virtual byte ConditionPercent
		{
			get
			{
				return (byte)Math.Round((ItemTemplate.MaxCondition > 0) ? (double)Condition / ItemTemplate.MaxCondition * 100 : 0);
			}
		}

		private DateTime _lastUsedDateTime;
		[NotMapped]
		public virtual int CanUseAgainIn
		{
			get
			{
				try
				{
					TimeSpan elapsed = DateTime.Now.Subtract(_lastUsedDateTime);
					TimeSpan reuse = new TimeSpan(0, 0, ItemTemplate.CanUseEvery);
					return (reuse.CompareTo(elapsed) < 0)
						? 0
						: ItemTemplate.CanUseEvery - elapsed.Seconds - 60 * elapsed.Minutes - 3600 * elapsed.Hours;
				}
				catch (ArgumentOutOfRangeException)
				{
					return 0;
				}
			}
			set
			{
				_lastUsedDateTime = DateTime.Now.AddSeconds(value - ItemTemplate.CanUseEvery);
			}
		}

		/// <summary>
		/// Repair cost for this item in the current state.
		/// </summary>
		[NotMapped]
		public virtual long RepairCost
		{
			get
			{
				return ((ItemTemplate.MaxCondition - Condition) * ItemTemplate.Price) / ItemTemplate.MaxCondition;
			}
		}

		[NotMapped]
		public ICollection<ItemBonus> Bonuses { get; set; }

	}
}
