/*
 * Atlas
 *
 */

using System;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;
using System.Collections.Generic;

namespace DOL.GS.Spells;

/// <summary>
/// All Stats buff
/// </summary>
[SpellHandlerAttribute("AllStatsBarrel")]
public class AllStatsBarrel : SingleStatBuff
{
    public static List<int> BuffList = new() {8090, 8091, 8094, 8092, 8095, 8093 /*,8071*/};
    private int strengthID = 8090;
    private int conID = 8091;
    private int strenghtConID = 8094;
    private int dexID = 8092;
    private int dexQuickID = 8095;
    private int acuityID = 8093;
    private int hasteID = 8071;

    public override bool StartSpell(GameLiving target)
    {
        var potionEffectLine = SkillBase.GetSpellLine(GlobalSpellsLines.Potions_Effects);

        var strengthSpell = SkillBase.FindSpell(strengthID, potionEffectLine);
        var strenghtSpellHandler =
            ScriptMgr.CreateSpellHandler(target, strengthSpell, potionEffectLine) as SpellHandler;

        var conSpell = SkillBase.FindSpell(conID, potionEffectLine);
        var conSpellHandler =
            ScriptMgr.CreateSpellHandler(target, conSpell, potionEffectLine) as SpellHandler;

        var strengthConSpell = SkillBase.FindSpell(strenghtConID, potionEffectLine);
        var strenghtConSpellHandler =
            ScriptMgr.CreateSpellHandler(target, strengthConSpell, potionEffectLine) as SpellHandler;

        var dexSpell = SkillBase.FindSpell(dexID, potionEffectLine);
        var dexSpellHandler =
            ScriptMgr.CreateSpellHandler(target, dexSpell, potionEffectLine) as SpellHandler;

        var dexQuickSpell = SkillBase.FindSpell(dexQuickID, potionEffectLine);
        var dexQuickSpellHandler =
            ScriptMgr.CreateSpellHandler(target, dexQuickSpell, potionEffectLine) as SpellHandler;

        var acuitySpell = SkillBase.FindSpell(acuityID, potionEffectLine);
        var acuitySpellHandler =
            ScriptMgr.CreateSpellHandler(target, acuitySpell, potionEffectLine) as SpellHandler;

        //Spell hasteSpell = SkillBase.FindSpell(hasteID, potionEffectLine);
        //SpellHandler hasteSpellHandler = ScriptMgr.CreateSpellHandler(target, hasteSpell, potionEffectLine) as SpellHandler;

        strenghtSpellHandler.StartSpell(target);
        conSpellHandler.StartSpell(target);
        strenghtConSpellHandler.StartSpell(target);
        dexSpellHandler.StartSpell(target);
        dexQuickSpellHandler.StartSpell(target);
        acuitySpellHandler.StartSpell(target);
        //hasteSpellHandler.StartSpell(target);

        return true;
    }

    public override eProperty Property1 => eProperty.Strength;

    public override eProperty Property2 => eProperty.Constitution;

    public override eProperty Property3 => eProperty.Dexterity;

    public override eProperty Property4 => eProperty.Quickness;

    public override eProperty Property5 => eProperty.Acuity;

    // constructor
    public AllStatsBarrel(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line)
    {
    }
}