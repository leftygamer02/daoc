using DOL.GS.PacketHandler;
using DOL.GS.Spells;

namespace DOL.GS.Effects;

public class AtlasOF_HailOfBlowsECSEffect : StatBuffECSEffect
{
    public AtlasOF_HailOfBlowsECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
        EffectType = eEffect.MeleeHasteBuff;
    }

    public override ushort Icon => 4240;

    public override string Name => "Hail Of Blows";

    public override bool HasPositiveEffect => true;
}