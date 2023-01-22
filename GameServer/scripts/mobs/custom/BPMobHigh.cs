//Written by Sirru

using System;
using System.Collections;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;
using DOL.GS.Spells;

namespace DOL.GS.Scripts;

/// <summary>
/// Represents an in-game GameHealer NPC
/// </summary>
public class BPMobHigh : GameNPC
{
    public override void Die(GameObject killer)
    {
        var player = killer as GamePlayer;
        if (player is GamePlayer && IsWorthReward)

            player.GainBountyPoints(Level * 5);

        DropLoot(killer);

        base.Die(killer);

        if (Faction != null && killer is GamePlayer)
        {
            var player3 = killer as GamePlayer;
            Faction.KillMember(player3);
        }

        StartRespawn();
    }
}