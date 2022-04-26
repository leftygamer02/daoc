
using DOL.Database;
namespace DOL.GS.RealmAbilities
{
	public class AtlasOF_SeveringTheTether : TimedRealmAbility, ISpellCastingAbilityHandler
    {
		public AtlasOF_SeveringTheTether(DBAbility dba, int level) : base(dba, level) { }

        // ISpellCastingAbilityHandler
        public Spell Spell { get { return m_spell; } }
        public SpellLine SpellLine { get { return m_spellline; } }
        public Ability Ability { get { return this; } }
        
		private const int m_range = 1500;
        private const int m_radius = 1000;
        private const int m_duration = 60 * 1000; // 1 min

		private DBSpell m_dbspell;
        private Spell m_spell = null;
        private SpellLine m_spellline;

        public override int MaxLevel { get { return 1; } }
        public override ushort Icon => 4260;
        public override int CostForUpgrade(int level) { return 10; }
		public override int GetReUseDelay(int level) { return 600; } // 10 mins

        private void CreateSpell(GamePlayer caster)
        {
            m_dbspell = new DBSpell();
            m_dbspell.Name = "Severing The Tether";
            m_dbspell.Icon = 4260;
            m_dbspell.ClientEffect = 612;
            m_dbspell.Damage = 0;
			m_dbspell.DamageType = 14; // matter
            m_dbspell.Target = "Enemy";
            m_dbspell.Radius = m_radius;
			m_dbspell.Type = eSpellType.SeveringTheTether.ToString();
            m_dbspell.Value = 0;
            m_dbspell.Duration = m_duration;
            m_dbspell.Pulse = 0;
            m_dbspell.Frequency = 0;
            m_dbspell.PulsePower = 0;
            m_dbspell.Power = 0;
            m_dbspell.CastTime = 0;
            m_dbspell.EffectGroup = 0;
            m_dbspell.RecastDelay = GetReUseDelay(0); // Spell code is responsible for disabling this ability and will use this value.
            m_dbspell.Range = m_range;
			m_spell = new Spell(m_dbspell, caster.Level);
            m_spellline = new SpellLine("RAs", "RealmAbilities", "RealmAbilities", true);
        }

        public override void Execute(GameLiving living)
		{
			if (CheckPreconditions(living, DEAD | SITTING | MEZZED | STUNNED)) return;
			GamePlayer m_caster = living as GamePlayer;
			if (m_caster == null || m_caster.castingComponent == null)
				return;

            var m_target = m_caster.TargetObject;
            if (m_target == null)
	            m_target = m_caster;

            CreateSpell(m_caster);

            if (m_spell != null)
            {
                m_caster.castingComponent.StartCastSpell(m_spell, m_spellline, this);
            }

            // We do not need to handle disabling the skill here. This ability casts a spell and is linked to that spell.
            // The spell casting code will disable this ability in SpellHandler's FinishSpellcast().
		}
	}
}
