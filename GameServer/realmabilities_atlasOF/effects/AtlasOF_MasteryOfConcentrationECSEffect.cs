namespace DOL.GS.Effects;

public class MasteryOfConcentrationECSEffect : ECSGameAbilityEffect
{
    public MasteryOfConcentrationECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
        EffectType = eEffect.MasteryOfConcentration;
        EffectService.RequestStartEffect(this);
    }

    public override ushort Icon => 4238;

    public override string Name => "Mastery of Concentration";

    public override bool HasPositiveEffect => true;
}