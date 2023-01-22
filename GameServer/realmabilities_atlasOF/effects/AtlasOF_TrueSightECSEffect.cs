using System;
using DOL.Database;
using DOL.GS.PacketHandler;
using DOL.GS.Spells;

namespace DOL.GS.Effects;

public class AtlasOF_TrueSightECSEffect : ECSGameAbilityEffect
{
    public new SpellHandler SpellHandler;

    public AtlasOF_TrueSightECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
        EffectType = eEffect.TrueSight;
        EffectService.RequestStartEffect(this);
    }

    public override ushort Icon => 4279;

    public override string Name => "True Sight";

    public override bool HasPositiveEffect => true;
}