using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;
using Microsoft.Extensions.Hosting;
using Atlas.DataLayer.Models;

namespace Atlas.DataLayer
{
    public class AtlasContext : DbContext
    {
        private readonly string connectionString;
        private readonly eConnectionType engine;

        public AtlasContext(eConnectionType connectionType, string connectionString) : base()
        {
            this.connectionString = connectionString;
            this.engine = connectionType;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (engine)
            {
                case eConnectionType.POSTGRESQL:
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                case eConnectionType.MYSQL:
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    break;
                case eConnectionType.SQLSERVER:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case eConnectionType.SQLITE:
                    optionsBuilder.UseSqlite(connectionString);
                    break;
            }

            

        }

        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountCustomParam> AccountCustomParams { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<ArtifactBonus> ArtifactBonuses { get; set; }
        public DbSet<ArtifactItem> ArtifactItems { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Battleground> Battlegrounds { get; set; }
        public DbSet<BindPoint> BindPoints { get; set; }
        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterAbility> CharacterAbilities { get; set; }
        public DbSet<CharacterCraftingSkill> CharacterCraftingSkills { get; set; }
        public DbSet<CharacterCustomParam> CharacterCustomParams { get; set; }
        public DbSet<CharacterDataQuest> CharacterDataQuests { get; set; }
        public DbSet<CharacterDisabledSpell> CharacterDisabledSpells { get; set; }
        public DbSet<CharacterFriend> CharacterFriends { get; set; }
        public DbSet<CharacterIgnore> CharacterIgnores { get; set; }
        public DbSet<CharacterMasterLevel> CharacterMasterLevels { get; set; }
        public DbSet<CharacterOneTimeDrop> CharacterOneTimeDrops { get; set; }
        public DbSet<CharacterSpec> CharacterSpecs { get; set; }
        public DbSet<CharacterTask> CharacterTasks { get; set; }
        public DbSet<ClassRealmAbility> ClassRealmAbilities { get; set; }
        public DbSet<ClassSpecialization> ClassSpecializations { get; set; }
        public DbSet<CraftedItem> CraftedItems { get; set; }
        public DbSet<CraftedItemComponent> CraftedItemComponents { get; set; }
        public DbSet<DataQuest> DataQuests { get; set; }
        public DbSet<DataQuestRewardQuest> DataQuestRewardQuests { get; set; }
        public DbSet<DbHouse> DbHouses { get; set; }
        public DbSet<DbHouseCharPerms> DbHouseCharPerms { get; set; }
        public DbSet<DbHousePermissions> DbHousePermissions { get; set; }
        public DbSet<DbIndoorItem> DbIndoorItems { get; set; }
        public DbSet<DbOutdoorItem> DbOutdoorItems { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<FactionAggroLevel> FactionAggroLevels { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildAlliance> GuildAlliances { get; set; }
        public DbSet<GuildRank> GuildRanks { get; set; }
        public DbSet<HouseConsignmentMerchant> HouseConsignmentMerchants { get; set; }
        public DbSet<HouseHookpointItem> HouseHookpointItems { get; set; }
        public DbSet<HouseHookpointOffset> HouseHookpointOffsets { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemSpell> InventoryItemSpells { get; set; }
        public DbSet<ItemBonus> ItemBonuses { get; set; }
        public DbSet<ItemSpell> ItemSpells { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<ItemUnique> ItemUnique { get; set; }
        public DbSet<JumpPoint> JumpPoints { get; set; }
        public DbSet<Keep> Keeps { get; set; }
        public DbSet<KeepCaptureLog> KeepCaptureLogs { get; set; }
        public DbSet<KeepComponent> KeepComponents { get; set; }
        public DbSet<KeepHookpoint> KeepHookpoints { get; set; }
        public DbSet<KeepHookpointItem> KeepHookpointItems { get; set; }
        public DbSet<KeepPosition> KeepPositions { get; set; }
        public DbSet<LinkedFaction> LinkedFactions { get; set; }
        public DbSet<LootGenerator> LootGenerators { get; set; }
        public DbSet<LootOtd> LootOtds { get; set; }
        public DbSet<LootTable> LootTables { get; set; }
        public DbSet<LootTableItem> LootTableItems { get; set; }
        public DbSet<MerchantItem> MerchantItems { get; set; }
        public DbSet<MinotaurRelic> MinotaurRelics { get; set; }
        public DbSet<MobBehavior> MobBehaviors { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NpcEquipment> NpcEquipments { get; set; }
        public DbSet<NpcSpawnGroup> NpcSpawnGroup { get; set; }
        public DbSet<NpcTemplate> NpcTemplates { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<PathPoints> PathPoints { get; set; }
        public DbSet<PlayerBoat> PlayerBoats { get; set; }
        public DbSet<PlayerEffect> PlayerEffects { get; set; }
        public DbSet<PvpKillsLog> PvpKillsLog { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Relic> Relics { get; set; }
        public DbSet<Salvage> Salvages { get; set; }
        public DbSet<SalvageYield> SalvageYields { get; set; }
        public DbSet<ServerInfo> ServerInfo { get; set; }
        public DbSet<ServerProperty> ServerProperties { get; set; }
        public DbSet<ServerPropertyCategory> ServerPropertyCategories { get; set; }
        public DbSet<ServerStats> ServerStats { get; set; }
        public DbSet<SinglePermission> SinglePermissions { get; set; }
        public DbSet<SpawnGroup> SpawnGroups { get; set; }
        public DbSet<SpawnPoint> SpawnPoints { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<SpecializationAbility> SpecializationAbilities { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<SpellCustomValues> SpellCustomValues { get; set; }
        public DbSet<SpellEffect> SpellEffects { get; set; }
        public DbSet<SpellLine> SpellLines { get; set; }
        public DbSet<SpellLineSpell> SpellLineSpells { get; set; }
        public DbSet<StarterEquipment> StartEquipments { get; set; }
        public DbSet<StartupLocation> StartupLocations { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<StyleSpell> StyleSpells { get; set; }
        public DbSet<Teleport> Teleports { get; set; }
        public DbSet<WorldObject> WorldObjects { get; set; }
        public DbSet<ZonePoint> ZonePoints { get; set; }
        public DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(a => a.AccountCustomParams)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountID)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Appeals)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountID)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Bans)
                .WithOne(b => b.Account)
                .HasForeignKey(b => b.AccountID)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.SinglePermissions)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountID)
                .IsRequired();

            modelBuilder.Entity<Character>()
                .HasMany(c => c.FriendList)
                .WithOne(f => f.Character)
                .HasForeignKey(f => f.CharacterID)
                .IsRequired();

            modelBuilder.Entity<Character>()
                .HasMany(c => c.IgnoreList)
                .WithOne(i => i.Character)
                .HasForeignKey(i => i.CharacterID)
                .IsRequired();

            modelBuilder.Entity<Guild>()
                .HasOne(g => g.GuildAlliance)
                .WithMany(ga => ga.Guilds)
                .HasForeignKey(g => g.GuildAllianceID);

            modelBuilder.Entity<Guild>()
                .HasMany(c => c.Characters)
                .WithOne(g => g.Guild)
                .HasForeignKey(g => g.GuildID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Guild>()
                .HasMany(g => g.GuildRanks)
                .WithOne(r => r.Guild)
                .HasForeignKey(r => r.GuildID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemTemplate>()
                .HasOne(s => s.SalvageYield)
                .WithOne(s => s.ItemTemplate)
                .HasForeignKey<SalvageYield>(x => x.ItemTemplateID);

            modelBuilder.Entity<Region>()
                .HasMany(r => r.ZonePointSources)
                .WithOne(z => z.SourceRegion)
                .HasForeignKey(z => z.SourceRegionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Region>()
                .HasMany(r => r.ZonePointTargets)
                .WithOne(z => z.TargetRegion)
                .HasForeignKey(z => z.TargetRegionID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
