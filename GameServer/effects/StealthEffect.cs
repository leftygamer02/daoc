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
using System.Collections.Generic;
using DOL.Language;

namespace DOL.GS.Effects;

/// <summary>
/// The helper class for the stealth ability
/// </summary>
public class StealthEffect : StaticEffect, IGameEffect
{
    /// <summary>
    /// The owner of the effect
    /// </summary>
    private GamePlayer m_player;

    /// <summary>
    /// Start the stealth on player
    /// </summary>
    public void Start(GamePlayer player)
    {
        m_player = player;
        player.EffectList.Add(this);
    }

    /// <summary>
    /// Stop the effect on target
    /// </summary>
    public override void Stop()
    {
        if (m_player.HasAbility(Abilities.Camouflage))
        {
            var camouflage =
                (CamouflageECSGameEffect) EffectListService.GetAbilityEffectOnTarget(m_player, eEffect.Camouflage);
            if (camouflage != null)
                EffectService.RequestImmediateCancelEffect(camouflage, false);
        }

        m_player.EffectList.Remove(this);
    }

    /// <summary>
    /// Called when effect must be canceled
    /// </summary>
    public override void Cancel(bool playerCancel)
    {
        m_player.Stealth(false);
    }

    /// <summary>
    /// Name of the effect
    /// </summary>
    public override string Name => LanguageMgr.GetTranslation(m_player.Client, "Effects.StealthEffect.Name");

    /// <summary>
    /// Remaining Time of the effect in milliseconds
    /// </summary>
    public override int RemainingTime => 0;

    /// <summary>
    /// Icon to show on players, can be id
    /// </summary>
    public override ushort Icon => 0x193;

    /// <summary>
    /// Delve Info
    /// </summary>
    public override IList<string> DelveInfo => new string[0];
}