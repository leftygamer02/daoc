/*
 * Atlas Custom Quest - Atlas 1.65v Classic Freeshard
 */
/*
*Author         : Kelt
*Editor			: Kelt, Clait
*Source         : Custom
*Date           : 15 June 2022
*Quest Name     : Basic Script
*Quest Classes  : all
*Quest Version  : v1.0
*
*Changes:
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.GS.PlayerTitles;
using log4net;

namespace DOL.GS.Quests.Albion
{
	public class BasicScript : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const string questTitle = "[Atlas] Fusi confuse me";
		protected const int minimumLevel = 1;
		protected const int maximumLevel = 50;

		private static GameNPC Clait = null; 
		private static GameNPC Fusi = null;

		// Constructors
		public BasicScript() : base()
		{
		}

		public BasicScript(GamePlayer questingPlayer) : base(questingPlayer)
		{
		}

		public BasicScript(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public BasicScript(GamePlayer questingPlayer, DBQuest dbQuest) : base(questingPlayer, dbQuest)
		{
		}


		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_QUESTS)
				return;
			

			#region defineNPCs

			GameNPC[] npcs = WorldMgr.GetNPCsByName("Clait", eRealm.Albion);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 73 && npc.X == 331647 && npc.Y == 452121)
					{
						Clait = npc;
						break;
					}

			if (Clait == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Clait , creating it ...");
				Clait = new GameNPC();
				Clait.Model = 10;
				Clait.Name = "Clait";
				Clait.GuildName = "Atlas Lead";
				Clait.Realm = eRealm.Albion;
				Clait.CurrentRegionID = 73;
				Clait.LoadEquipmentTemplateFromDatabase("Emolia");
				Clait.Size = 50;
				Clait.Level = 55;
				Clait.X = 331647;
				Clait.Y = 452121;
				Clait.Z = 8112;
				Clait.Heading = 404;
				Clait.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Clait.SaveIntoDatabase();
				}
			}
			
			npcs = WorldMgr.GetNPCsByName("Fusi", eRealm.Albion);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 73 && npc.X == 331437 && npc.Y == 451985)
					{
						Fusi = npc;
						break;
					}

			if (Fusi == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Fusi , creating it ...");
				Fusi = new GameNPC();
				Fusi.Model = 2346;
				Fusi.Name = "Fusi";
				Fusi.GuildName = "";
				Fusi.Realm = eRealm.Albion;
				Fusi.CurrentRegionID = 73;
				Fusi.Size = 200;
				Fusi.Level = 55;
				Fusi.X = 331437;
				Fusi.Y = 451985;
				Fusi.Z = 8112;
				Fusi.Heading = 4084;
				Fusi.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Fusi.SaveIntoDatabase();
				}
			}
			// end npc

			#endregion

			
			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Clait, GameObjectEvent.Interact, new DOLEventHandler(TalkToClait));
			GameEventMgr.AddHandler(Clait, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToClait));
			
			GameEventMgr.AddHandler(Fusi, GameObjectEvent.Interact, new DOLEventHandler(TalkToFusi));
			GameEventMgr.AddHandler(Fusi, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToFusi));

			/* Now we bring to Clait the possibility to give this quest to players */
			Clait.AddQuestToGive(typeof (BasicScript));

			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//if not loaded, don't worry
			if (Clait == null)
				return;
			// remove handlers
			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Clait, GameObjectEvent.Interact, new DOLEventHandler(TalkToClait));
			GameEventMgr.RemoveHandler(Clait, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToClait));
			
			GameEventMgr.RemoveHandler(Fusi, GameObjectEvent.Interact, new DOLEventHandler(TalkToFusi));
			GameEventMgr.RemoveHandler(Fusi, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToFusi));

			/* Now we remove to Clait the possibility to give this quest to players */
			Clait.RemoveQuestToGive(typeof (BasicScript));
		}

		protected static void TalkToClait(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if(Clait.CanGiveQuest(typeof (BasicScript), player)  <= 0)
				return;

			//We also check if the player is already doing the quest
			BasicScript quest = player.IsDoingQuest(typeof (BasicScript)) as BasicScript;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					switch (quest.Step)
					{
						case 1:
							Clait.SayTo(player, "Please speak with Fusi...");
							break;
						case 2:
							Clait.SayTo(player, "Oh no never! She will be [vegan]!");
							break;
					}
				}
				else
				{
					Clait.SayTo(player, "Hello, I am Clait."+
					                       "Fusi is confusing me, can you [speak with Fusi]?");
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs) args;
				if (quest == null)
				{
					switch (wArgs.Text)
					{
						case "speak with Fusi":
							player.Out.SendQuestSubscribeCommand(Clait, QuestMgr.GetIDForQuestType(typeof(BasicScript)), "Will you help Clait "+questTitle+"?");
							break;
					}
				}
				else
				{
					switch (wArgs.Text)
					{
						case "vegan":
							quest.FinishQuest();
							break;
						case "abort":
							player.Out.SendCustomDialog("Do you really want to abort this quest, \nall items gained during quest will be lost?", new CustomDialogResponse(CheckPlayerAbortQuest));
							break;
					}
				}
			}
			else if (e == GameLivingEvent.ReceiveItem)
			{
				ReceiveItemEventArgs rArgs = (ReceiveItemEventArgs) args;
				if (quest != null)
				{
				}
			}
		}
		
		protected static void TalkToFusi(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			//We also check if the player is already doing the quest
			BasicScript quest = player.IsDoingQuest(typeof (BasicScript)) as BasicScript;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					switch (quest.Step)
					{
						case 1:
							Fusi.SayTo(player, "Meow Meow, [meooooow]!");
							break;
						case 2:
							Fusi.SayTo(player, "MEOW!!!");
							break;
					}
				}
				else
				{
					
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs) args;
				if (quest == null)
				{
					switch (wArgs.Text)
					{
						
					}
				}
				else
				{
					switch (wArgs.Text)
					{
						case "meooooow":
							quest.Step = 2;
							break;
					}
				}
			}
		}

		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof (BasicScript)) != null)
				return true;

			// This checks below are only performed is player isn't doing quest already

			//if (player.HasFinishedQuest(typeof(Academy_47)) == 0) return false;

			//if (!CheckPartAccessible(player,typeof(CityOfCamelot)))
			//	return false;

			if (player.Level < minimumLevel || player.Level > maximumLevel)
				return false;

			return true;
		}

		private static void CheckPlayerAbortQuest(GamePlayer player, byte response)
		{
			BasicScript quest = player.IsDoingQuest(typeof (BasicScript)) as BasicScript;

			if (quest == null)
				return;

			if (response == 0x00)
			{
				SendSystemMessage(player, "Good, now go out there and finish your work!");
			}
			else
			{
				SendSystemMessage(player, "Aborting Quest " + questTitle + ". You can start over again if you want.");
				quest.AbortQuest();
			}
		}

		protected static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(BasicScript)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			if(Clait.CanGiveQuest(typeof (BasicScript), player)  <= 0)
				return;

			if (player.IsDoingQuest(typeof (BasicScript)) != null)
				return;

			if (response == 0x00)
			{
			}
			else
			{
				//Check if we can add the quest!
				if (!Clait.GiveQuest(typeof (BasicScript), player, 1))
					return;

			}
		}

		//Set quest name
		public override string Name
		{
			get { return questTitle; }
		}

		// Define Steps
		public override string Description
		{
			get
			{
				switch (Step)
				{
					case 1:
						return "Speak with Fusi.";
					case 2:
						return "Speak with Clait.";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player==null || player.IsDoingQuest(typeof (BasicScript)) == null)
				return;

		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...
		}

		public override void FinishQuest()
		{
			m_questPlayer.GainExperience(eXPSource.Quest, 200, false);
			m_questPlayer.AddMoney(Money.GetMoney(0,0,1,32,Util.Random(50)), "You receive {0} as a reward.");

			base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...
			
		}
	}
}
