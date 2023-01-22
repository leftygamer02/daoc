using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.Tests.Unit.Gameserver.PropertyCalc;

[TestFixture]
internal class UT_MeleeDamagePercentCalculator
{
    [Test]
    public void CalcValue_50StrengthBuff_6()
    {
        var npc = NewNPC();
        npc.BaseBuffBonusCategory[eProperty.Strength] = 50;

        var actual = MeleeDamageBonusCalculator.CalcValue(npc, MeleeDamageProperty);

        Assert.AreEqual(6, actual);
    }

    [Test]
    public void CalcValue_NPCWith50StrengthDebuff_Minus6()
    {
        var npc = NewNPC();
        npc.DebuffCategory[eProperty.Strength] = 50;

        var actual = MeleeDamageBonusCalculator.CalcValue(npc, MeleeDamageProperty);

        Assert.AreEqual(-6, actual);
    }

    private MeleeDamagePercentCalculator MeleeDamageBonusCalculator => new();
    private eProperty MeleeDamageProperty => eProperty.MeleeDamage;

    private FakeNPC NewNPC()
    {
        return new();
    }
}