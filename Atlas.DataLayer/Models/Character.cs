using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Atlas.DataLayer.Models
{
    public class Character : DataObjectBase
    {
        public int AccountID { get; set; }
        public int? GuildID { get; set; }

        public int AccountSlot { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? LastPlayed { get; set; }

        public bool HasGravestone { get; set; }
        public int GravestoneRegion { get; set; }
        public int Constitution { get; set; }
        public int Dexterity { get; set; }
        public int Strength { get; set; }
        public int Quickness { get; set; }
        public int Intelligence { get; set; }
        public int Piety { get; set; }
        public int Empathy { get; set; }
        public int Charisma { get; set; }


        public long BountyPoints { get; set; }
        public long RealmPoints { get; set; }
        public int RealmLevel { get; set; }
        public long Experience { get; set; }
        public int MaxEndurance { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Endurance { get; set; }
        public int Concentration { get; set; }

        public int Gender { get; set; }
        public int Race { get; set; }
        public int Level { get; set; }
        public int Class { get; set; }
        public int Realm { get; set; }

        public int CreationModel { get; set; }
        public int CurrentModel { get; set; }

        public int ActiveWeaponSlot { get; set; }
        
        public int RegionID { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Zpos { get; set; }
        public int BindXpos { get; set; }
        public int BindYpos { get; set; }
        public int BindZpos { get; set; }
        public int BindRegionID { get; set; }
        public int BindHeading { get; set; }
        public int BindHouseXpos { get; set; }
        public int BindHouseYpos { get; set; }
        public int BindHouseZpos { get; set; }
        public int BindHouseRegion { get; set; }
        public int BindHouseHeading { get; set; }
        
        public int DeathCount { get; set; }
        public int ConLostAtDeath { get; set; }
        public int Direction { get; set; }
        public int MaxSpeed { get; set; }
        public int Copper { get; set; }
        public int Silver { get; set; }
        public int Gold { get; set; }
        public int Platinum { get; set; }
        public int Mithril { get; set; }
        

        public bool IsCloakHoodUp { get; set; }
        public bool IsCloakInvisible { get; set; }
        public bool IsHelmInvisible { get; set; }
        public bool SpellQueue { get; set; }
        public bool IsLevelSecondStage { get; set; }
        public bool FlagClassName { get; set; }
        public bool Advisor { get; set; }
        public int GuildRank { get; set; }
        public long PlayedTime { get; set; }
        public long DeathTime { get; set; }
        public bool ReceiveROG { get; set; }

        public int RespecAmountAllSkill { get; set; }
        public int RespecAmountSingleSkill { get; set; }
        public int RespecAmountRealmSkill { get; set; }
        public int RespecAmountDOL { get; set; }
        public int RespecAmountChampionSkill { get; set; }
        public bool IsLevelRespecUsed { get; set; }
        public int RespecBought { get; set; }

        public bool SafetyFlag { get; set; }

        public int PrimaryCraftingSkill { get; set; }
        public bool CancelStyle { get; set; }
        public bool IsAnonymous { get; set; }
        public int CustomisationStep { get; set; }
        public int EyeSize { get; set; }
        public int LipSize { get; set; }
        public int EyeColor { get; set; }
        public int HairColor { get; set; }
        public int FaceType { get; set; }
        public int HairStyle { get; set; }
        public int MoodType { get; set; }
        
        public bool UsedLevelCommand { get; set; }
        public string CurrentTitleType { get; set; }
        
        public int KillsAlbionPlayers { get; set; }
        public int KillsMidgardPlayers { get; set; }
        public int KillsHiberniaPlayers { get; set; }
        public int KillsAlbionDeathBlows { get; set; }
        public int KillsMidgardDeathBlows { get; set; }
        public int KillsHiberniaDeathBlows { get; set; }
        public int KillsAlbionSolo { get; set; }
        public int KillsMidgardSolo { get; set; }
        public int KillsHiberniaSolo { get; set; }

        public int CapturedKeeps { get; set; }
        public int CapturedTowers { get; set; }
        public int CapturedRelics { get; set; }
        public int KillsDragon { get; set; }
        public int DeathsPvP { get; set; }
        public int KillsLegion { get; set; }
        public int KillsEpicBoss { get; set; }

        public bool GainXP { get; set; }
        public bool GainRP { get; set; }
        public bool Autoloot { get; set; }
        public DateTime? LastFreeLeveled { get; set; }
        public int LastFreeLevel { get; set; }
        public string GuildNote { get; set; }
        public bool ShowXFireInfo { get; set; }
        public bool NoHelp { get; set; }
        public bool ShowGuildLogins { get; set; }
        public bool Champion { get; set; }
        public int ChampionLevel { get; set; }
        public long ChampionExperience { get; set; }
        public int ML { get; set; }
        public long MLExperience { get; set; }
        public int MLLevel { get; set; }
        public bool MLGranted { get; set; }
        public bool RPFlag { get; set; }
        public bool IgnoreStatistics { get; set; }
        public bool NotDisplayedInHerald { get; set; }
        public int ActiveSaddleBags { get; set; }
        public DateTime? LastLevelUp { get; set; }
        public long PlayedTimeSinceLevel { get; set; }


        public virtual Account Account { get; set; }
        public virtual Guild Guild { get; set; }
        public virtual Region Region { get; set; }

        public virtual ICollection<CharacterCustomParam> CustomParams { get; set; }
        public virtual ICollection<CharacterSpec> Specs { get; set; }
        public virtual ICollection<CharacterAbility> Abilities { get; set; }
        public virtual ICollection<CharacterCraftingSkill> CraftingSkills { get; set; }
        public virtual ICollection<CharacterDisabledSpell> DisabledSpells { get; set; }
        public virtual ICollection<CharacterFriend> FriendList { get; set; }
        public virtual ICollection<CharacterIgnore> IgnoreList { get; set; }
        public virtual ICollection<CharacterDataQuest> DataQuests { get; set; }
        public virtual ICollection<CharacterOneTimeDrop> OneTimeDrops { get; set; }
        public virtual ICollection<CharacterMasterLevel> MasterLevels { get; set; }
        public virtual ICollection<CharacterTask> CharacterTasks { get; set; }
        public virtual ICollection<PlayerEffect> PlayerEffects { get; set; }
        public virtual ICollection<SinglePermission> SinglePermissions { get; set; }

        public Character()
        {
            CustomParams = new HashSet<CharacterCustomParam>();
            Specs = new HashSet<CharacterSpec>();
            Abilities = new HashSet<CharacterAbility>();
            CraftingSkills = new HashSet<CharacterCraftingSkill>();
            DisabledSpells = new HashSet<CharacterDisabledSpell>();
            FriendList = new HashSet<CharacterFriend>();
            IgnoreList = new HashSet<CharacterIgnore>();
            DataQuests = new HashSet<CharacterDataQuest>();
            OneTimeDrops = new HashSet<CharacterOneTimeDrop>();
            MasterLevels = new HashSet<CharacterMasterLevel>();
            CharacterTasks = new HashSet<CharacterTask>();
            PlayerEffects = new HashSet<PlayerEffect>();
            SinglePermissions = new HashSet<SinglePermission>();
            //InventoryItems = new HashSet<InventoryItem>();
        }

    }
}
