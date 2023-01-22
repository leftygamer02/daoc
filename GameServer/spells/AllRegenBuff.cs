/*
 * Atlas
 *
 */

using System;
using System.Collections.Generic;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

namespace DOL.GS.Spells;

/// <summary>
/// All Stats buff
/// </summary>
[SpellHandlerAttribute("AllRegenBuff")]
public class AllRegenBuff : PropertyChangingSpell
{
    public static List<int> RegenList = new() {8084, 8080, 8076};
    private int pomID = 8084;
    private int endID = 8080;
    private int healID = 8076;

    public override bool StartSpell(GameLiving target)
    {
        var potionEffectLine = SkillBase.GetSpellLine(GlobalSpellsLines.Potions_Effects);

        var pomSpell = SkillBase.FindSpell(pomID, potionEffectLine);
        var pomSpellHandler =
            ScriptMgr.CreateSpellHandler(target, pomSpell, potionEffectLine) as SpellHandler;

        var endSpell = SkillBase.FindSpell(endID, potionEffectLine);
        var endSpellHandler =
            ScriptMgr.CreateSpellHandler(target, endSpell, potionEffectLine) as SpellHandler;

        var healSpell = SkillBase.FindSpell(healID, potionEffectLine);
        var healthConSpellHandler =
            ScriptMgr.CreateSpellHandler(target, healSpell, potionEffectLine) as SpellHandler;


        pomSpellHandler.StartSpell(target);
        endSpellHandler.StartSpell(target);
        healthConSpellHandler.StartSpell(target);

        return true;
    }

    public override eProperty Property1 => eProperty.PowerRegenerationRate;
    public override eProperty Property2 => eProperty.EnduranceRegenerationRate;
    public override eProperty Property3 => eProperty.HealthRegenerationRate;


    // constructor
    public AllRegenBuff(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line)
    {
    }
}

/// <summary>
/// All Stats buff
/// </summary>
[SpellHandlerAttribute("BeadRegen")]
public class BeadRegen : PropertyChangingSpell
{
    public static List<int> BeadRegenList = new() {31057, 31056, 31055};
    private int pomID = 31057;
    private int endID = 31056;
    private int healID = 31055;

    public override bool StartSpell(GameLiving target)
    {
        if (Caster.CurrentZone.IsRvR)
        {
            if (Caster is GamePlayer p)
                p.Out.SendMessage("You cannot use this item in an RvR zone.", eChatType.CT_SpellResisted,
                    eChatLoc.CL_SystemWindow);
            return false;
        }

        if (Caster.Level > 49)
        {
            if (Caster is GamePlayer p)
                p.Out.SendMessage("You are too powerful for this item's effects.", eChatType.CT_SpellResisted,
                    eChatLoc.CL_SystemWindow);
            return false;
        }

        if (Caster.ControlledBrain != null && Caster.ControlledBrain is AI.Brain.NecromancerPetBrain necroPet &&
            necroPet.Body.InCombatInLast(5000))
        {
            if (Caster is GamePlayer p)
                p.Out.SendMessage("Your pet must be out of combat for 5 seconds to use this.",
                    eChatType.CT_SpellResisted, eChatLoc.CL_SystemWindow);
            return false;
        }

        target = Caster;
        var potionEffectLine = SkillBase.GetSpellLine(GlobalSpellsLines.Potions_Effects);

        var pomSpell = SkillBase.FindSpell(pomID, potionEffectLine);
        var pomSpellHandler =
            ScriptMgr.CreateSpellHandler(target, pomSpell, potionEffectLine) as SpellHandler;

        var endSpell = SkillBase.FindSpell(endID, potionEffectLine);
        var endSpellHandler =
            ScriptMgr.CreateSpellHandler(target, endSpell, potionEffectLine) as SpellHandler;

        var healSpell = SkillBase.FindSpell(healID, potionEffectLine);
        var healthConSpellHandler =
            ScriptMgr.CreateSpellHandler(target, healSpell, potionEffectLine) as SpellHandler;

        pomSpellHandler.StartSpell(target);
        endSpellHandler.StartSpell(target);
        healthConSpellHandler.StartSpell(target);

        if (Caster.ControlledBrain != null && Caster.ControlledBrain is AI.Brain.NecromancerPetBrain necrop)
        {
            var petHealHandler =
                ScriptMgr.CreateSpellHandler(necrop.Body, healSpell, potionEffectLine) as SpellHandler;
            petHealHandler.StartSpell(necrop.Body);
        }

        return true;
    }

    public override eProperty Property1 => eProperty.PowerRegenerationRate;
    public override eProperty Property2 => eProperty.EnduranceRegenerationRate;
    public override eProperty Property3 => eProperty.HealthRegenerationRate;


    // constructor
    public BeadRegen(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line)
    {
    }
}