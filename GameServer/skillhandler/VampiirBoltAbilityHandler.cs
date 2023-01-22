using System;
using System.Collections;
using System.Reflection;
using DOL.GS.PacketHandler;
using DOL.GS.Effects;
using DOL.Events;
using log4net;

namespace DOL.GS.SkillHandler;

/// <summary>
/// Handler for Vampiir Bolt clicks
/// </summary>
[SkillHandler(Abilities.VampiirBolt)]
public class VampiirBoltAbilityHandler : SpellCastingAbilityHandler
{
    public override long Preconditions => DEAD | SITTING | MEZZED | STUNNED | TARGET;

    public override int SpellID => 13200 + m_ability.Level;
}