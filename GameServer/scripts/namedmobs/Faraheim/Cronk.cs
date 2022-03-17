/*
Cronk
<author>Kelt</author>
 */
using System;
using System.Collections.Generic;
using System.Text;
using DOL.Database;
using DOL.Events;
using DOL.GS;
using System.Reflection;
using System.Collections;
using DOL.AI.Brain;
using DOL.GS.PacketHandler;
using DOL.GS.Scripts.DOL.AI.Brain;


namespace DOL.GS.Scripts
{

	public class Cronk : GameNPC
	{

		public Cronk() : base()
		{
			
		}
		/// <summary>
		/// Add Cronk to World
		/// </summary>
		public override bool AddToWorld()
		{
			INpcTemplate npcTemplate = NpcTemplateMgr.GetTemplate(60159504);
			LoadTemplate(npcTemplate);
			Strength = npcTemplate.Strength;
			Dexterity = npcTemplate.Dexterity;
			Constitution = npcTemplate.Constitution;
			Quickness = npcTemplate.Quickness;
			Piety = npcTemplate.Piety;
			Intelligence = npcTemplate.Intelligence;
			Charisma = npcTemplate.Charisma;
			Empathy = npcTemplate.Empathy;
			
			Realm = eRealm.None;
			Model = 687;
			Size = 70;
			Level = 63;
			Health = MaxHealth + (Level * Level) / 2;
			MaxDistance = 2500;
			TetherRange = 3500;
			MeleeDamageType = eDamageType.Crush;

			// humanoid
			BodyType = 6;
			Race = 2005;
			
			// tradklan tribe - Faction
			Faction = FactionMgr.GetFactionByID(176);
			Faction.AddFriendFaction(FactionMgr.GetFactionByID(176));

			Name = "Cronk";
			base.SetOwnBrain(new CronkBrain());
			base.AddToWorld();
			
			return true;
		}
		
		public override double AttackDamage(InventoryItem weapon)
		{
			return base.AttackDamage(weapon) * Strength / 100;
		}

		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (log.IsInfoEnabled)
				log.Info("Cronk NPC Initializing...");
		}
	}

	namespace DOL.AI.Brain
	{
		public class CronkBrain : StandardMobBrain
		{
			public CronkBrain() : base()
			{
				AggroLevel = 100;
				AggroRange = 550;
			}

			public override void Think()
			{
				if (Body.InCombat && Body.IsAlive && HasAggro)
				{
					if (Body.TargetObject != null)
					{
						foreach (GamePlayer player in Body.GetPlayersInRadius(2000))
						{
							if (player.IsWithinRadius(Body, Body.AttackRange))
							{
								if (Debuff.TargetHasEffect(Body.TargetObject) == false &&
								    Body.TargetObject.IsVisibleTo(Body))
								{
									new RegionTimer(Body, new RegionTimerCallback(CastDebuff), 1000);
								}
								new RegionTimer(Body, new RegionTimerCallback(CastDD), 5000);
							}
						}
					}
				}
			}
			
			/// <summary>
			/// Broadcast relevant messages to the players.
			/// </summary>
			/// <param name="message">The message to be broadcast.</param>
			public void BroadcastMessage(String message)
			{
				foreach (GamePlayer player in Body.GetPlayersInRadius(WorldMgr.OBJ_UPDATE_DISTANCE))
				{
					player.Out.SendMessage(message, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
				}
			}
			
			/// <summary>
			/// Called whenever the Cronk's body sends something to its brain.
			/// </summary>
			/// <param name="e">The event that occured.</param>
			/// <param name="sender">The source of the event.</param>
			/// <param name="args">The event details.</param>
			public override void Notify(DOLEvent e, object sender, EventArgs args)
			{
				base.Notify(e, sender, args);
			}

			#region DDSpell
			/// <summary>
			/// Cast Cold DD on the Target
			/// </summary>
			/// <param name="timer">The timer that started this cast.</param>
			/// <returns></returns>
			private int CastDD(RegionTimer timer)
			{
				Body.CastSpell(DD, SkillBase.GetSpellLine(GlobalSpellsLines.Mob_Spells));
				return 0;
			}
			
			private Spell m_DD;
			/// <summary>
			/// The DD spell.
			/// </summary>
			protected Spell DD
			{
				get
				{
					if (m_DD == null)
					{
						DBSpell spell = new DBSpell();
						spell.AllowAdd = false;
						spell.CastTime = 0;
						spell.Uninterruptible = true;
						spell.RecastDelay = 20;
						spell.ClientEffect = 2511;
						spell.Icon = 2511;
						spell.Name = "Cold Mob DD";
						spell.Range = 1200;
						spell.Radius = 0;
						spell.Value = 0;
						spell.Duration = 0;
						spell.Damage = 300;
						spell.DamageType = 11;
						spell.SpellID = 2512;
						spell.Target = "Enemy";
						spell.MoveCast = true;
						spell.Type = eSpellType.DirectDamage.ToString();
						m_DD = new Spell(spell, 50);
						SkillBase.AddScriptedSpell(GlobalSpellsLines.Mob_Spells, m_DD);
					}
					return m_DD;
				}
			}

			#endregion
			
			#region DebuffSpell
			/// <summary>
			/// Cast DexQui Debuff on the Target
			/// </summary>
			/// <param name="timer">The timer that started this cast.</param>
			/// <returns></returns>
			private int CastDebuff(RegionTimer timer)
			{
				Body.CastSpell(Debuff, SkillBase.GetSpellLine(GlobalSpellsLines.Mob_Spells));
				return 0;
			}
			
			private Spell m_Debuff;
			/// <summary>
			/// The DD spell.
			/// </summary>
			protected Spell Debuff
			{
				get
				{
					if (m_Debuff == null)
					{
						DBSpell spell = new DBSpell();
						spell.AllowAdd = false;
						spell.CastTime = 0;
						spell.Uninterruptible = true;
						spell.RecastDelay = 70;
						spell.ClientEffect = 2627;
						spell.Icon = 2627;
						spell.Name = "Greater Curse of Blindness";
						spell.Range = 1200;
						spell.Radius = 300;
						spell.Value = 78;
						spell.Duration = 60;
						spell.Damage = 0;
						spell.DamageType = 11;
						spell.SpellID = 2627;
						spell.Target = "Enemy";
						spell.MoveCast = true;
						spell.Type = eSpellType.DexterityQuicknessDebuff.ToString();
						m_Debuff = new Spell(spell, 50);
						SkillBase.AddScriptedSpell(GlobalSpellsLines.Mob_Spells, m_Debuff);
					}
					return m_Debuff;
				}
			}

			#endregion
			
		}
	}
}