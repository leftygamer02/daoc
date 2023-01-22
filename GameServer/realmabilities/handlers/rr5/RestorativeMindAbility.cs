using System;
using System.Collections.Generic;
using DOL.Database;
using DOL.GS.Effects;

namespace DOL.GS.RealmAbilities;

/// <summary>
/// Anger of the Gods RA
/// </summary>
public class RestorativeMindAbility : RR5RealmAbility
{
    public RestorativeMindAbility(DBAbility dba, int level) : base(dba, level)
    {
    }

    /// <summary>
    /// Action
    /// </summary>
    /// <param name="living"></param>
    public override void Execute(GameLiving living)
    {
        if (CheckPreconditions(living, DEAD | SITTING | MEZZED | STUNNED)) return;


        var deactivate = false;

        var player = living as GamePlayer;
        if (player != null)
        {
            if (player.Group != null)
            {
                SendCasterSpellEffectAndCastMessage(living, 7071, true);
                foreach (var member in player.Group.GetPlayersInTheGroup())
                {
                    var aog = member.EffectList.GetOfType<RestorativeMindEffect>();
                    if (!CheckPreconditions(member, DEAD) && aog == null
                                                          && living.IsWithinRadius(member, 2000))
                    {
                        var effect = new RestorativeMindEffect();
                        effect.Start(member);
                        deactivate = true;
                    }
                }
            }
            else
            {
                var aog = player.EffectList.GetOfType<RestorativeMindEffect>();
                if (!CheckPreconditions(player, DEAD) && aog == null)
                {
                    var effect = new RestorativeMindEffect();
                    effect.Start(player);
                    deactivate = true;
                }
            }
        }

        if (deactivate)
            DisableSkill(living);
    }

    public override int GetReUseDelay(int level)
    {
        return 600;
    }


    public override void AddEffectsInfo(IList<string> list)
    {
        list.Add(
            "Group Frigg that heals health, power, and endurance over 30 seconds for a total of 50%. (5% is granted every 3 seconds regardless of combat state)");
        list.Add("");
        list.Add("Target: Group");
        list.Add("Duration: 30 sec");
        list.Add("Casting time: instant");
    }
}