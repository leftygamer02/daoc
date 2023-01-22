namespace DOL.GS.Effects;

public class StrikingTheSoulECSEffect : ECSGameAbilityEffect
{
    public StrikingTheSoulECSEffect(ECSGameEffectInitParams initParams)
        : base(initParams)
    {
        EffectType = eEffect.StrikingTheSoul;
        EffectService.RequestStartEffect(this);
    }

    public override ushort Icon => 4271;

    public override string Name => "Striking the Soul";

    public override bool HasPositiveEffect => true;

    public override void OnStartEffect()
    {
        if (OwnerPlayer == null)
            return;

        OwnerPlayer.BuffBonusCategory4[(int) eProperty.ToHitBonus] += (int) Effectiveness;
        OwnerPlayer.Out.SendUpdateWeaponAndArmorStats();
    }

    public override void OnStopEffect()
    {
        if (OwnerPlayer == null)
            return;

        OwnerPlayer.BuffBonusCategory4[(int) eProperty.ToHitBonus] -= (int) Effectiveness;
        OwnerPlayer.Out.SendUpdateWeaponAndArmorStats();
    }
}