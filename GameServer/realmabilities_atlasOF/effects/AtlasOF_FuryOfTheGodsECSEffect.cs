using DOL.GS.PacketHandler;

namespace DOL.GS.Effects;

public class AtlasOF_FuryOfTheGodsECSEffect : DamageAddECSEffect
{
    public AtlasOF_FuryOfTheGodsECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
    }

    public override ushort Icon => 4251;

    public override string Name => "Fury Of The Gods";

    public override bool HasPositiveEffect => true;
}