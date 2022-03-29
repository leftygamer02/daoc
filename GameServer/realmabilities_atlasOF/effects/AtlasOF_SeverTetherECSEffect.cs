using System;
using DOL.Database;
using DOL.GS.PacketHandler;
using DOL.GS.Spells;

namespace DOL.GS.Effects
{
    public class AtlasOF_SeverTetherECSEffect : ECSGameAbilityEffect
    {
        public AtlasOF_SeverTetherECSEffect(ECSGameEffectInitParams initParams)
            : base(initParams)
        {
            EffectType = eEffect.SeverTether;
            if (Owner is GamePet p) OwnerPet = p;
            EffectService.RequestStartEffect(this);
        }

        public override ushort Icon { get { return 3048; } }
        public override string Name { get { return "Severing The Tether"; } }
        public override bool HasPositiveEffect { get { return true; } }

        private GamePet OwnerPet;

        public override void OnStartEffect()
        {
            if (OwnerPet == null && Owner is GamePet p) OwnerPet = p;

           AttackOwner();
            
            base.OnStartEffect();
        }

        public override void OnEffectPulse()
        {
            AttackOwner();
            base.OnEffectPulse();
        }

        public override void OnStopEffect()
        {
            OwnerPet.Brain;
            base.OnStopEffect();
        }

        private void AttackOwner()
        {
            OwnerPet.TargetObject = OwnerPet.Owner;
            OwnerPet.StartAttack(OwnerPet.Owner);
        }
    }
}
