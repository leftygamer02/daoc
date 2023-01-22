/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using System;
using DOL.GS;

namespace DOL.Events;

/// <summary>
/// This class holds all possible player events.
/// Only constants defined here!
/// </summary>
public class GamePlayerEvent : GameLivingEvent
{
    /// <summary>
    /// Constructs a new GamePlayer event
    /// </summary>
    /// <param name="name">the event name</param>
    protected GamePlayerEvent(string name)
        : base(name)
    {
    }

    /// <summary>
    /// Tests if this event is valid for the specified object
    /// </summary>
    /// <param name="o">The object for which the event wants to be registered</param>
    /// <returns>true if valid, false if not</returns>
    public override bool IsValidFor(object o)
    {
        return o is GamePlayer;
    }

    /// <summary>
    /// The GameEntered event is fired whenever the player enters the game.
    /// NOTE:  when this event fires, the player's client will still *not* be included in WorldMgr.GetAllPlayingClients()
    /// </summary>
    public static readonly GamePlayerEvent GameEntered = new("GamePlayer.GameEntered");

    /// <summary>
    /// The RegionChanged event is fired whenever the player changes the region
    /// </summary>
    public static readonly GamePlayerEvent RegionChanged = new("GamePlayer.RegionChanged");

    /// <summary>
    /// The Released event is fired whenever the player has released
    /// </summary>
    public static readonly GamePlayerEvent Released = new("GamePlayer.Released");

    /// <summary>
    /// The Revive event is fired whenever the player is resurrected and at /release
    /// </summary>
    public static readonly GamePlayerEvent Revive = new("GamePlayer.Revive");

    /// <summary>
    /// The Linkdeath event is fired whenever the player went linkdead
    /// </summary>
    public static readonly GamePlayerEvent Linkdeath = new("GamePlayer.Linkdeath");

    /// <summary>
    /// The GiveItem event is fired whenever the player wants to give away an item
    /// </summary>
    public static readonly GamePlayerEvent GiveItem = new("GamePlayer.GiveItem");

    /// <summary>
    /// The GiveMoney event is fired whenever the player wants to give away money
    /// </summary>
    public static readonly GamePlayerEvent GiveMoney = new("GamePlayer.GiveMoney");

    /// <summary>
    /// The Quit event is fired whenever the player the player quits the game
    /// </summary>
    public static readonly GamePlayerEvent Quit = new("GamePlayer.Quit");

    /// <summary>
    /// The UseSlot event is fired whenever the player tries to use the slot
    /// </summary>
    public static readonly GamePlayerEvent UseSlot = new("GamePlayer.UseSlot");

    /// <summary>
    /// The StealthStateChanged event is fired whenever the player's stealth state changes
    /// </summary>
    public static readonly GamePlayerEvent StealthStateChanged = new("GamePlayer.StealthStateChanged");

    /// <summary>
    /// The OnLevelSecondStage event is fired whenever the player gains experience for level second stage
    /// </summary>
    public static readonly GamePlayerEvent LevelSecondStage = new("GamePlayer.LevelSecondStage");

    /// <summary>
    /// The OnLevelSecondStage event is fired whenever the player gains experience for next level
    /// </summary>
    public static readonly GamePlayerEvent LevelUp = new("GamePlayer.LevelUp");

    /// <summary>
    /// The AcceptQuest event is fired whenever the player accepts a quest offer generated via BaseQuestPart.AddAction(eActionType.OfferQuest)
    /// </summary>
    public static readonly GamePlayerEvent AcceptQuest = new("GamePlayer.AcceptQuest");

    /// <summary>
    /// The DeclineQuest event is fired whenever the player declines a quest offer generated via BaseQuestPart.AddAction(eActionType.OfferQuest)
    /// </summary>
    public static readonly GamePlayerEvent DeclineQuest = new("GamePlayer.DeclineQuest");

    /// <summary>
    /// The ContinueQuest event is fired whenever the player continues a quest abort generated via BaseQuestPart.AddAction(eActionType.OfferQuestAbort)
    /// </summary>
    public static readonly GamePlayerEvent ContinueQuest = new("GamePlayer.ContinueQuest");

    /// <summary>
    /// The AbortQuest event is fired whenever the player aborts a quest generated via BaseQuestPart.AddAction(eActionType.OfferQuestAbort)
    /// </summary>
    public static readonly GamePlayerEvent AbortQuest = new("GamePlayer.AbortQuest");

    /// <summary>
    /// The UseAbility event is fired whenever the player uses ability
    /// </summary>
    public static readonly GamePlayerEvent UseAbility = new("GamePlayer.UseAbility");

    /// <summary>
    /// The RRLevelUp event is fired whenever the player gains a realm rank
    /// </summary>
    public static readonly GamePlayerEvent RRLevelUp = new("GamePlayer.RRLevelUp");

    /// <summary>
    /// The RLLevelUp event is fired whenever the player gains a realm level
    /// </summary>
    public static readonly GamePlayerEvent RLLevelUp = new("GamePlayer.RLLevelUp");

    /// <summary>
    /// The SwimmingStatus event is fired whenever the player IsSwimming flag changes
    /// </summary>
    public static readonly GamePlayerEvent SwimmingStatus = new("GamePlayer.SwimmingStatus");

    /// <summary>
    /// The BecomeML event is fired whenever the player chose is ML path
    /// </summary>
    public static readonly GamePlayerEvent BecomeML = new("GamePlayer.BecomeML");

    /// <summary>
    /// The ChampionLevelUp event is fired whenever the player gain a champion level
    /// </summary>
    public static readonly GamePlayerEvent ChampionLevelUp = new("GamePlayer.ChampionLevelUp");

    /// <summary>
    /// The QuestRewardChosen event is fired whenever the player finishes a RewardQuest.
    /// </summary>
    public static readonly GamePlayerEvent QuestRewardChosen = new("GamePlayer.QuestRewardChosen");

    /// <summary>
    /// The ModelChanged event is fired whenever the player model changes.
    /// </summary>
    public static readonly GamePlayerEvent ModelChanged = new("GamePlayer.ModelChanged");

    /// <summary>
    /// The AcceptGroup event is fired whenever the player accept a Group invitation.
    /// </summary>
    public static readonly GamePlayerEvent AcceptGroup = new("GamePlayer.AcceptGroup");

    /// <summary>
    /// The LeaveGroup event is fired when the player disbands/is removed from a group.
    /// </summary>
    public static readonly GamePlayerEvent LeaveGroup = new("GamePlayer.LeaveGroup");

    public static readonly GamePlayerEvent NextCraftingTierReached = new("GamePlayer.ReachedNewCraftTitle");

    /// <summary>
    /// The ChangeTarget event is fired when a player changes target
    /// </summary>
    public static readonly GamePlayerEvent ChangeTarget = new("GamePlayer.ChangeTarget");

    /// <summary>
    /// The ExecuteCommand event is fired when a player executes a command!
    /// </summary>
    public static readonly GamePlayerEvent ExecuteCommand = new("GamePlayer.ExecuteCommand");

    /// <summary>
    /// The ChangeAnonymous event is fired when a player change its Anonymous Status
    /// </summary>
    public static readonly GamePlayerEvent ChangeAnonymous = new("GamePlayer.ExecuteCommand");

    #region Statistics

    /// <summary>
    /// The KillsTotalPlayersChanged event is fired when any of the KillsxxxPlayersChanged property changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsTotalPlayersChanged = new("GamePlayer.KillsTotalPlayersChanged");

    /// <summary>
    /// The KillsAlbionPlayersChanged event is fired when KillsAlbionPlayers property changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsAlbionPlayersChanged = new("GamePlayer.KillsAlbionPlayersChanged");

    /// <summary>
    /// The KillsMidgardPlayersChanged event is fired when KillsMidgardPlayers property changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsMidgardPlayersChanged = new("GamePlayer.KillsMidgardPlayersChanged");

    /// <summary>
    /// The KillsHiberniaPlayersChanged event is fired when KillsHiberniaPlayers property changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsHiberniaPlayersChanged =
        new("GamePlayer.KillsHiberniaPlayersChanged");

    /// <summary>
    /// The KillsTotalDeathBlowsChanged event is fired when KillsAlbionDeathBlows, KillsMidgardDeathBlows or KillsHiberniaDeathBlows properties changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsTotalDeathBlowsChanged =
        new("GamePlayer.KillsTotalDeathBlowsChanged");

    /// <summary>
    /// The KillsTotalSoloChanged event is fired when KillsAlbionSolo, KillsMidgardSolo or KillsHiberniaSolo properties changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsTotalSoloChanged = new("GamePlayer.KillsTotalSoloChanged");

    /// <summary>
    /// The CapturedKeepsChanged event is fired when CapturedKeeps properties changes.
    /// </summary>
    public static readonly GamePlayerEvent CapturedKeepsChanged = new("GamePlayer.CapturedKeepsChanged");

    /// <summary>
    /// The CapturedTowersChanged event is fired when CapturedTowers properties changes.
    /// </summary>
    public static readonly GamePlayerEvent CapturedTowersChanged = new("GamePlayer.CapturedTowersChanged");

    /// <summary>
    /// The CapturedRelicsChanged event is fired when CapturedRelics properties changes.
    /// </summary>
    public static readonly GamePlayerEvent CapturedRelicsChanged = new("GamePlayer.CapturedRelicsChanged");

    /// <summary>
    /// The KillsDragonChanged event is fired when KillsDragon properties changes.
    /// </summary>
    public static readonly GamePlayerEvent
        KillsDragonChanged = new("GamePlayer.KillsDragonChanged");

    /// <summary>
    /// The KillsEpicBossChanged event is fired when KillsEpicBoss properties changes.
    /// </summary>
    public static readonly GamePlayerEvent KillsEpicBossChanged = new("GamePlayer.KillsEpicBossChanged");

    /// <summary>
    /// The KillsLegionChanged event is fired when KillsLegion properties changes.
    /// </summary>
    public static readonly GamePlayerEvent
        KillsLegionChanged = new("GamePlayer.KillsLegionChanged");

    #endregion
}