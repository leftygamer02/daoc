using System;
using DOL.GS;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;
using System.Reflection;

namespace DOL.GS.Scripts
{
    public class ChallengeTeleporter : GameNPC
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override bool AddToWorld()
        {
	        Level = 50;
            Size = 55;
            Flags |= eFlags.PEACE;
            
            switch (Realm)
            {
	            case eRealm.Albion:
	            {
		            Name = "Dark Occultist Fria";
		            Model = 1024;
	            } break;
	            case eRealm.Hibernia:
	            {
		            Name = "Dark Occultist Elif";
		            Model = 1088;
	            } break;
	            case eRealm.Midgard:
	            {
		            Name = "Dark Occultist Isa";
		            Model = 1073;
		            
	            } break;
            }
            return base.AddToWorld();
        }
		public override bool Interact(GamePlayer player)
		{
			if (!base.Interact(player)) return false;
			TurnTo(player.X, player.Y);
			
			switch (Realm)
			{
				case eRealm.Albion:
				{
					player.Out.SendMessage($"Hello {player.CharacterClass.Name}, blub blub blub [Demonic Prison].", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
				} break;
				case eRealm.Hibernia:
				{
					player.Out.SendMessage($"Hello {player.CharacterClass.Name}, blub blub blub [Demonic Prison].", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
				} break;
				case eRealm.Midgard:
				{
					player.Out.SendMessage($"Hello {player.CharacterClass.Name}, blub blub blub [Demonic Prison].", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
				} break;
			}
			
			return true;
		}
		public override bool WhisperReceive(GameLiving source, string str)
		{
			if(!base.WhisperReceive(source,str)) return false;
		  	if(!(source is GamePlayer)) return false;
			GamePlayer t = (GamePlayer) source;
			TurnTo(t.X,t.Y);
			
			switch (Realm)
			{
				case eRealm.Albion:
				{
					switch (str)
					{
						case "Demonic Prison":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								// check if grp has 8 ppl to start the challenge
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER || t.Client.Account.PrivLevel > 1)
								{
									t.Out.SendMessage($"The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  $"Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"[Start the Challenge]", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
									Emote(eEmote.Salute);
								}
								else
								{
									t.Out.SendMessage("The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  "Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"But I see that you just bring {t.Group?.MemberCount.ToString()} people for this challenge." +
									                  "Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
									Emote(eEmote.Induct);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;

						case "Start the Challenge":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER ||
								    t.Client.Account.PrivLevel > 1)
								{
									// alb chapter 2 darkness rising dungeon
									CloneInstance instance =
										(CloneInstance) WorldMgr.CreateInstance(336, typeof(CloneInstance));
									instance.LoadFromDatabase("Demonic Prison Alb");

									if (t.Group != null)
									{
										foreach (var grpPlayer in t.Group.GetMembersInTheGroup())
										{
											grpPlayer.MoveTo(instance.InstanceEntranceLocation);
										}
									}
									else if (t.Client.Account.PrivLevel > 1)
									{
										t.MoveTo(instance.InstanceEntranceLocation);
									}
								}
								else
								{
									t.Out.SendMessage(
										$"Come back when you have willing fighters and mages to go on this mission!",
										eChatType.CT_Say, eChatLoc.CL_PopupWindow);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;
					}

				} break;
				case eRealm.Hibernia:
				{
					switch (str)
					{
						case "Demonic Prison":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								// check if grp has 8 ppl to start the challenge
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER || t.Client.Account.PrivLevel > 1)
								{
									t.Out.SendMessage($"The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  $"Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"[Start the Challenge]", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
									Emote(eEmote.Salute);
								}
								else
								{
									t.Out.SendMessage("The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  "Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"But I see that you just bring {t.Group?.MemberCount.ToString()} people for this challenge." +
									                  "Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
									Emote(eEmote.Induct);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;

						case "Start the Challenge":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER ||
								    t.Client.Account.PrivLevel > 1)
								{
									// hib chapter 2 darkness rising dungeon
									CloneInstance instance =
										(CloneInstance) WorldMgr.CreateInstance(338, typeof(CloneInstance));
									instance.LoadFromDatabase("Demonic Prison Hib");

									if (t.Group != null)
									{
										foreach (var grpPlayer in t.Group.GetMembersInTheGroup())
										{
											grpPlayer.MoveTo(instance.InstanceEntranceLocation);
										}
									}
									else if (t.Client.Account.PrivLevel > 1)
									{
										t.MoveTo(instance.InstanceEntranceLocation);
									}
								}
								else
								{
									t.Out.SendMessage(
										$"Come back when you have willing fighters and mages to go on this mission!",
										eChatType.CT_Say, eChatLoc.CL_PopupWindow);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;
					}
				} break;
				case eRealm.Midgard:
				{
					switch (str)
					{
						case "Demonic Prison":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								// check if grp has 8 ppl to start the challenge
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER || t.Client.Account.PrivLevel > 1)
								{
									t.Out.SendMessage($"The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  $"Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"[Start the Challenge]", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
									Emote(eEmote.Salute);
								}
								else
								{
									t.Out.SendMessage("The prison is a demonic place of torture. " +
									                  $"Broken souls from all realms who died fighting Legion ended up there. " +
									                  $"Even I was there and had to turn my back on this place. " +
									                  "Are you ready to visit this place to slay the leader of this demonic force?\n\n" +
									                  $"But I see that you just bring {t.Group?.MemberCount.ToString()} people for this challenge." +
									                  "Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
									Emote(eEmote.Induct);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;

						case "Start the Challenge":
							if (t.Group != null || t.Client.Account.PrivLevel > 1)
							{
								if (t.Group?.MemberCount == ServerProperties.Properties.GROUP_MAX_MEMBER ||
								    t.Client.Account.PrivLevel > 1)
								{
									// mid chapter 2 darkness rising dungeon
									CloneInstance instance =
										(CloneInstance) WorldMgr.CreateInstance(337, typeof(CloneInstance));
									instance.LoadFromDatabase("Demonic Prison Mid");

									if (t.Group != null)
									{
										foreach (var grpPlayer in t.Group.GetMembersInTheGroup())
										{
											grpPlayer.MoveTo(instance.InstanceEntranceLocation);
										}
									}
									else if (t.Client.Account.PrivLevel > 1)
									{
										t.MoveTo(instance.InstanceEntranceLocation);
									}
								}
								else
								{
									t.Out.SendMessage(
										$"Come back when you have willing fighters and mages to go on this mission!",
										eChatType.CT_Say, eChatLoc.CL_PopupWindow);
								}
							}
							else
							{
								t.Out.SendMessage($"Come back when you have willing fighters and mages to go on this mission!", eChatType.CT_Say,eChatLoc.CL_PopupWindow);
							}
							break;
					}

				} break;
			}
			
			return true;
		}
		
		private void SendReply(GamePlayer target, string msg)
			{
				target.Client.Out.SendMessage(
					msg,
					eChatType.CT_Say,eChatLoc.CL_PopupWindow);
			}
		
		[ScriptLoadedEvent]
        public static void OnScriptCompiled(DOLEvent e, object sender, EventArgs args)
        {
            log.Info("\t Challenge Teleporter for demonic-prison, initialized: true");
        }	
    }
}