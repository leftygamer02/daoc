using System;
using System.Collections;
using System.Reflection;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.GS.Effects;
using DOL.GS.Spells;
using DOL.Events;
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
        
		private const int m_range = 1500; // pbaoe
        private const int m_radius = 500; //
        private const eDamageType m_damageType = eDamageType.Spirit;

		private DBSpell m_dbspell;
        private Spell m_spell = null;
        private SpellLine m_spellline;

        public override int MaxLevel { get { return 1; } }
		public override int CostForUpgrade(int level) { return 10; }
		public override int GetReUseDelay(int level) { return 900; } // 15 mins

        private SpellHandler CreateSpell(GamePlayer caster)
        {
            m_dbspell = new DBSpell();
            m_dbspell.Name = "Severing The Tether";
            m_dbspell.Icon = 358;
            m_dbspell.ClientEffect = 2797;
            m_dbspell.Damage = 0;
			m_dbspell.DamageType = (int)m_damageType;
            m_dbspell.Target = "Enemy";
            m_dbspell.Radius = m_radius;
			m_dbspell.Type = eSpellType.OffensiveProc.ToString();
            m_dbspell.Value = 0;
            m_dbspell.LifeDrainReturn = 70;
            m_dbspell.Duration = 30;
            m_dbspell.Pulse = 0;
            m_dbspell.PulsePower = 0;
            m_dbspell.Power = 0;
            m_dbspell.CastTime = 0;
            m_dbspell.EffectGroup = 0;
            m_dbspell.RecastDelay = GetReUseDelay(0); // Spell code is responsible for disabling this ability and will use this value.
            m_dbspell.Range = m_range;
            m_dbspell.Description = "All pets affected by this spell will immediately turn against their masters.";
			m_spell = new Spell(m_dbspell, caster.Level);
            m_spellline = new SpellLine("RAs", "RealmAbilities", "RealmAbilities", true);
            return new SpellHandler(caster, m_spell, m_spellline);
        }

        public override void Execute(GameLiving living)
		{
			if (CheckPreconditions(living, DEAD | SITTING | MEZZED | STUNNED)) return;
			GamePlayer m_caster = living as GamePlayer;
			if (m_caster == null || m_caster.castingComponent == null)
				return;

            GameLiving m_target = m_caster.TargetObject as GameLiving;

            
            
            bool hitPet = false;
            foreach (GameNPC npc in m_target.GetNPCsInRadius((ushort)m_spell.Radius))
            {
	            if (npc is GamePet pet && GameServer.ServerRules.IsAllowedToAttack(m_caster, pet, true))
	            {
		            hitPet = true;
		            new AtlasOF_SeverTetherECSEffect(new ECSGameEffectInitParams(pet, 30, 1, CreateSpell(m_caster)));
	            }
            }

            

            // We do not need to handle disabling the skill here. This ability casts a spell and is linked to that spell.
            // The spell casting code will disable this ability in SpellHandler's FinishSpellcast().
		}
	}
}
