using System;
using DOL.Database;
using DOL.GS.PacketHandler;
using DOL.GS.Spells;

namespace DOL.GS.Effects;

public class AtlasOF_VanishECSEffect : ECSGameAbilityEffect
{
    public new SpellHandler SpellHandler;

    public AtlasOF_VanishECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
        EffectType = eEffect.Vanish;
        EffectService.RequestStartEffect(this);
    }

    public override ushort Icon => 4280;

    public override string Name => "Vanish";

    public override bool HasPositiveEffect => true;

    public override void OnStartEffect()
    {
        OwnerPlayer.Stealth(true);
        base.OnStartEffect();
    }
}