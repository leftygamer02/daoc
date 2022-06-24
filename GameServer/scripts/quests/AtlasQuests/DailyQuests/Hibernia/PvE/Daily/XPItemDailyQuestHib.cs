using System;
using System.Linq;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;
using DOL.GS.Quests;
using log4net;

namespace DOL.GS.DailyQuest.Hibernia
{
	public class XPItemDailyQuestHib : Quests.DailyQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const string questTitle = "[Daily] Pest Control";
		private const string NPCName = "Dean";
		private const int minimumLevel = 20;
		private const int maximumLevel = 49;

		// Kill Goal
		private const int MAX_KILLED = 20;
		
		private static GameNPC QuestNPC = null; // Start NPC

		private int MobsKilled = 0;
		private XPItem mobToKill = null;
		private string m_mobName;
		private string m_mobRegion;

		// Constructors
		public XPItemDailyQuestHib() : base()
		{
		}

		public XPItemDailyQuestHib(GamePlayer questingPlayer) : base(questingPlayer)
		{

		}

		public XPItemDailyQuestHib(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public XPItemDailyQuestHib(GamePlayer questingPlayer, DBQuest dbQuest) : base(questingPlayer, dbQuest)
		{
		}
		
		public override int Level
		{
			get
			{
				// Quest Level
				return minimumLevel;
			}
		}

		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_QUESTS)
				return;

			#region defineNPCs

			GameNPC[] npcs = WorldMgr.GetNPCsByName(NPCName, eRealm.Hibernia);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 200 && npc.X == 334962 && npc.Y == 420687)
					{
						QuestNPC = npc;
						break;
					}

			if (QuestNPC == null)
			{
				if (log.IsWarnEnabled)
					log.Warn($"Could not find {NPCName} , creating it ...");
				QuestNPC = new GameNPC();
				QuestNPC.Model = 355;
				QuestNPC.Name = "Dean";
				QuestNPC.GuildName = "Advisor to the King";
				QuestNPC.Realm = eRealm.Hibernia;
				//Druim Ligen Location
				QuestNPC.CurrentRegionID = 200;
				QuestNPC.Size = 50;
				QuestNPC.Level = 59;
				QuestNPC.X = 334962;
				QuestNPC.Y = 420687;
				QuestNPC.Z = 5184;
				QuestNPC.Heading = 1571;
				QuestNPC.AddToWorld();
				// if (SAVE_INTO_DATABASE)
				// {
				// 	QuestNPC.SaveIntoDatabase();
				// }
			}
			#endregion
			
			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest,  SubscribeQuest);
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, SubscribeQuest);

			GameEventMgr.AddHandler(QuestNPC, GameObjectEvent.Interact, TalkToDean);
			GameEventMgr.AddHandler(QuestNPC, GameLivingEvent.WhisperReceive, TalkToDean);

			/* Now we bring to QuestNPC the possibility to give this quest to players */
			QuestNPC.AddQuestToGive(typeof (XPItemDailyQuestHib));

			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//if not loaded, don't worry
			if (QuestNPC == null)
				return;
			// remove handlers
			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, SubscribeQuest);
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, SubscribeQuest);

			GameEventMgr.RemoveHandler(QuestNPC, GameObjectEvent.Interact, TalkToDean);
			GameEventMgr.RemoveHandler(QuestNPC, GameLivingEvent.WhisperReceive, TalkToDean);

			/* Now we remove to QuestNPC the possibility to give this quest to players */
			QuestNPC.RemoveQuestToGive(typeof (XPItemDailyQuestHib));
		}

		private static string GetXPItem(GamePlayer player)
		{
			var quest = XPItemUtils.GetRandomForPlayer(player);
			if (quest == null)
				return "";
			foreach (var abstractQuest in player.QuestList)
			{
				if (abstractQuest is XPItemDailyQuestHib hibQ)
				{
					hibQ.mobToKill = quest;
					hibQ.m_mobName = quest.m_mobName;
					hibQ.m_mobRegion = WorldMgr.GetRegion((ushort) quest.MobRegion).Description;
					hibQ.SaveQuestParameters();
				}
			}
			return quest.MobName;
		}
		private static void TalkToDean(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			if (((SourceEventArgs) args).Source is not GamePlayer player)
				return;
			
			if(QuestNPC.CanGiveQuest(typeof (XPItemDailyQuestHib), player)  <= 0)
				return;

			//We also check if the player is already doing the quest
			var quest = player.IsDoingQuest(typeof (XPItemDailyQuestHib)) as XPItemDailyQuestHib;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					switch (quest.Step)
					{
						case 1:
							QuestNPC.SayTo(player, "Check your journal, I've put a not there for you!");
							break;
						case 2:
							QuestNPC.SayTo(player, "Hello " + player.Name + ", have you [culled] the pests?");
							break;
					}
				}
				else
				{
					QuestNPC.SayTo(player, $"Hello {player.Name}, I am {QuestNPC.Name}. I help the King with logistics, and he's tasked me with getting things done around here.\n" +
					                   "I don't know if you have noticed but we've got a bit of a pest problem around here. I've tried using traps, poison and even begged them to leave but I can't do it all by myself. \n\n" +
					                   "Would you help me [culling] those invading creatures?");
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				var wArgs = (WhisperReceiveEventArgs) args;
				if (quest == null)
				{
					switch (wArgs.Text.ToLower())
					{
						case "culling":
							player.Out.SendQuestSubscribeCommand(QuestNPC, QuestMgr.GetIDForQuestType(typeof(XPItemDailyQuestHib)), $"Will you help {QuestNPC.Name} with {questTitle}?");
							break;
					}
				}
				else
				{
					switch (wArgs.Text)
					{
						case "culled":
							if (quest.Step == 2)
							{
								player.Out.SendMessage("Thank you for your contribution!", eChatType.CT_Chat, eChatLoc.CL_PopupWindow);
								quest.FinishQuest();
							}
							break;
						case "abort":
							player.Out.SendCustomDialog("Do you really want to abort this quest, \nall items gained during quest will be lost?", new CustomDialogResponse(CheckPlayerAbortQuest));
							break;
					}
				}
			}
		}
		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof (XPItemDailyQuestHib)) != null)
				return true;

			if (player.Level < minimumLevel || player.Level > maximumLevel)
				return false;

			return true;
		}
		
		public override void LoadQuestParameters()
		{
			MobsKilled = GetCustomProperty(QuestPropertyKey) != null ? int.Parse(GetCustomProperty(QuestPropertyKey)) : 0;
			mobToKill = GetCustomProperty(XPItemKey) != null ? XPItemUtils.GetFromID(GetCustomProperty(XPItemKey)) : null;
			if (mobToKill != null) mobToKill.m_mobName = mobToKill.MobName;
			if (mobToKill != null) mobToKill.m_mobRegion = mobToKill.MobRegion;
		}

		public override void SaveQuestParameters()
		{
			if (mobToKill == null) return;
			SetCustomProperty(QuestPropertyKey, MobsKilled.ToString());
			SetCustomProperty(XPItemKey, mobToKill.m_id.ToString());
		}
		
		public static void CheckPlayerAbortQuest(GamePlayer player, byte response)
		{
			if (player.IsDoingQuest(typeof (XPItemDailyQuestHib)) is not XPItemDailyQuestHib quest)
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

		public static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(XPItemDailyQuestHib)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			if(QuestNPC.CanGiveQuest(typeof (XPItemDailyQuestHib), player)  <= 0)
				return;

			if (player.IsDoingQuest(typeof (XPItemDailyQuestHib)) != null)
				return;

			if (response == 0x00)
			{
				player.Out.SendMessage("Oh.. Come back later if you change your mind.", eChatType.CT_Say, eChatLoc.CL_PopupWindow);
			}
			else
			{
				//Check if we can add the quest!
				if (!QuestNPC.GiveQuest(typeof(XPItemDailyQuestHib), player, 1))
					return;
						
				string mobName = GetXPItem(player);
				QuestNPC.SayTo(player, $"Good, now go out there and finish your work! Please kill {MAX_KILLED} {mobName} and come back to me!.");

				if (mobName == null || mobName == "")
				{
					foreach (var quest in player.QuestList)
					{
						if (quest is XPItemDailyQuestHib)
						{
							quest.AbortQuest();
						}
					}
				}
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
						XPItemDailyQuestHib quest = m_questPlayer.QuestList.FirstOrDefault(x => x is XPItemDailyQuestHib) as XPItemDailyQuestHib;
						string region;
						try
						{
							region = WorldMgr.GetRegion((ushort) quest.mobToKill.MobRegion).Description;
						}
						catch (Exception e)
						{
							region = null;
						}
						if (region == null || quest == null) return "Error encountered when loading quest text. Try relogging to fix.";
						return $"{NPCName} asked you to kill {quest.mobToKill.m_mobName} in {region} ({MobsKilled} | {MAX_KILLED})";
					case 2:
						return $"Return to {NPCName} for your Reward.";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			var player = sender as GamePlayer;

			if (player == null || player.IsDoingQuest(typeof(XPItemDailyQuestHib)) == null)
				return;
			
			if (sender != m_questPlayer)
				return;

			if (Step != 1 || e != GameLivingEvent.EnemyKilled) return;
			var gArgs = (EnemyKilledEventArgs) args;

			if (gArgs.Target.Name.ToLower() != mobToKill.MobName) return;
			MobsKilled++;
			player.Out.SendMessage($"{questTitle}: ({MobsKilled} | {MAX_KILLED})", eChatType.CT_ScreenCenter, eChatLoc.CL_SystemWindow);
			player.Out.SendQuestUpdate(this);
					
			if (MobsKilled >= MAX_KILLED)
			{
				// FinishQuest or go back to QuestNPC
				Step = 2;
			}

		}
		
		public override string QuestPropertyKey
		{
			get => "XPItemDailyQuestHib";
			set { ; }
		}
		
		private string XPItemKey
		{
			get => "XPItemID";
			set { ; }
		}
		
		public override void FinishQuest()
		{
			m_questPlayer.GainExperience(eXPSource.Quest, (m_questPlayer.ExperienceForNextLevel - m_questPlayer.ExperienceForCurrentLevel)/10, false);
			m_questPlayer.AddMoney(Money.GetMoney(0,0,m_questPlayer.Level,50,Util.Random(50)), "You receive {0} as a reward.");
			AtlasROGManager.GenerateOrbAmount(m_questPlayer, 100);
			MobsKilled = 0;
			base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...
		}
		
	}
}
