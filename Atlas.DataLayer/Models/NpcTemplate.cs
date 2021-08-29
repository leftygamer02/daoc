using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class NpcTemplate : DataObjectBase
    {
        public string TranslationID { get; set; }
        public string Name { get; set; }
        public string Suffix { get; set; }
        public string ClassType { get; set; }
        public string GuildName { get; set; }
        public string ExamineArticle { get; set; }
        public string MessageArticle { get; set; }
        public string Model { get; set; }
        public int Gender { get; set; }
        public string Size { get; set; }
        public string Level { get; set; }
        public int MaxSpeed { get; set; }
        public string ItemListName { get; set; }
        public int Flags { get; set; }
        public int MeleeDamageType { get; set; }
        public int ParryChance { get; set; }
        public int EvadeChance { get; set; }
        public int BlockChance { get; set; }
        public int LeftHandSwingChance { get; set; }
        public string Spells { get; set; }
        public string Styles { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Dexterity { get; set; }
        public int Quickness { get; set; }
        public int Intelligence { get; set; }
        public int Piety { get; set; }
        public int Charisma { get; set; }
        public int Empathy { get; set; }
        public string Abilities { get; set; }
        public int AggroLevel { get; set; }
        public int AggroRange { get; set; }
        public int RaceID { get; set; }
        public int BodyType { get; set; }
        public int MaxDistance { get; set; }
        public int TetherRange { get; set; }
        public int VisibleWeaponSlots { get; set; }
        public string PackageID { get; set; }
        public int? LootTableID { get; set; }
        public string Brain { get; set; }
        public int FactionID { get; set; }
        public int HouseNumber { get; set; }
        public int Realm { get; set; }
        public string Guild { get; set; }
        public string EquipmentTemplateName { get; set; }

        public virtual Race Race { get; set; }
        public virtual LootTable LootTable { get; set; }

        public virtual ICollection<NpcSpawnGroup> NpcSpawnGroups { get; set; }

        public NpcTemplate()
        {
            NpcSpawnGroups = new HashSet<NpcSpawnGroup>();
        }
    }
}
