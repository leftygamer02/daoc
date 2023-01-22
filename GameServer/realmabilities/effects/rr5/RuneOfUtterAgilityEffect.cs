using System;
using System.Collections;
using System.Collections.Generic;
using DOL.GS.PacketHandler;
using DOL.GS.RealmAbilities;

namespace DOL.GS.Effects;

/// <summary>
/// Mastery of Concentration
/// </summary>
public class RuneOfUtterAgilityEffect : TimedEffect
{
    private GameLiving owner;

    public RuneOfUtterAgilityEffect()
        : base(15000)
    {
    }

    public override void Start(GameLiving target)
    {
        base.Start(target);
        owner = target;
        var player = target as GamePlayer;
        if (player != null)
        {
            foreach (GamePlayer p in player.GetPlayersInRadius(WorldMgr.VISIBILITY_DISTANCE))
                p.Out.SendSpellEffectAnimation(player, player, Icon, 0, false, 1);

            player.BuffBonusCategory4[(int) eProperty.EvadeChance] += 90;
        }
    }

    public override void Stop()
    {
        var player = owner as GamePlayer;
        if (player != null)
            player.BuffBonusCategory4[(int) eProperty.EvadeChance] -= 90;
        base.Stop();
    }

    public override string Name => "Rune Of Utter Agility";

    public override ushort Icon => 3073;

    public override IList<string> DelveInfo
    {
        get
        {
            var list = new List<string>();
            list.Add("Increases your evade chance up to 90% for 30 seconds.");
            return list;
        }
    }
}