namespace DOL.GS.SpellEffects;

public struct HealEffectComponent : IEffectComponent
{
    public int Value;
    public GameLiving Target;
    public GameLiving Caster;
    public eSpellEffect Type { get; set; }
    public ushort SpellEffectId { get; set; }

    public HealEffectComponent(int value, GameLiving target, GameLiving caster, ushort spellEffectId)
    {
        Value = value;
        Target = target;
        Caster = caster;
        Type = eSpellEffect.Heal;
        SpellEffectId = spellEffectId;
    }
}