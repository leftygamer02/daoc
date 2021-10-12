
namespace DOL.GS.RealmAbilities
{
	/// <summary>
	/// Mastery of Pain ability
	/// </summary>
	public class AtlasOF_MasteryOfPain : MasteryOfPain
	{
		public AtlasOF_MasteryOfPain(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level) { }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugDexLevel(player, 2); }

        // MoP is 5% per level unlike most other Mastery RAs.
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer5AmountForLevel(level); } 
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Parry ability
    /// </summary>
    public class AtlasOF_MasteryOfParrying : MasteryOfParrying
	{
        public AtlasOF_MasteryOfParrying(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level) { }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugDexLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Blocking ability
    /// </summary>
    public class AtlasOF_MasteryOfBlocking : MasteryOfBlocking
    {
        public AtlasOF_MasteryOfBlocking(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level) { }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugDexLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Healing ability
    /// </summary>
    public class AtlasOF_MasteryOfHealing : MasteryOfHealingAbility
    {
        public AtlasOF_MasteryOfHealing(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level) { }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugAcuityLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Arms ability
    /// </summary>
    public class AtlasOF_MasteryOfArms : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfArms(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.MeleeSpeed) { }
        protected override string ValueUnit { get { return "%"; } }

        public override bool CheckRequirement(GamePlayer player)
        { 
            // Atlas custom change - Friar pre-req is AugDex3 instead of a 100% useless AugStr3.
            if (player.CharacterClass.ID == (byte)eCharacterClass.Friar)
            {
                return AtlasRAHelpers.HasAugDexLevel(player, 3);
            }
            
            return AtlasRAHelpers.HasAugStrLevel(player, 3);
        }

        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Archery ability
    /// </summary>
    public class AtlasOF_MasteryOfArchery : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfArchery(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.ArcherySpeed) { }
        protected override string ValueUnit { get { return "%"; } }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugDexLevel(player, 3); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of the Art ability
    /// </summary>
    public class AtlasOF_MasteryOfTheArt : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfTheArt(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.CastingSpeed) { }
        protected override string ValueUnit { get { return "%"; } }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugAcuityLevel(player, 3); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Magery ability
    /// </summary>
    public class AtlasOF_MasteryOfMagery : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfMagery(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.SpellDamage) { }
        protected override string ValueUnit { get { return "%"; } }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugAcuityLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of the Arcane ability
    /// </summary>
    public class AtlasOF_MasteryOfTheArcane : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfTheArcane(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.BuffEffectiveness) { }
        protected override string ValueUnit { get { return "%"; } }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugAcuityLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Mastery of Water ability. The best of all abilities.
    /// </summary>
    public class AtlasOF_MasteryOfWater : RAPropertyEnhancer
    {
        public AtlasOF_MasteryOfWater(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.WaterSpeed) { }
        protected override string ValueUnit { get { return "%"; } }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }

    /// <summary>
    /// Dodger.
    /// </summary>
    public class AtlasOF_Dodger : RAPropertyEnhancer
    {
        public AtlasOF_Dodger(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.EvadeChance) { }
        protected override string ValueUnit { get { return "%"; } }
        public override bool CheckRequirement(GamePlayer player) { return AtlasRAHelpers.HasAugQuiLevel(player, 2); }
        public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
        public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
    }
}