using System;
using System.Reflection;
using DOL.GS.Commands;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.Commandss {
    [Cmd("&TestService",
		ePrivLevel.Admin,
		"Test a service. Used for debugging performance",
		"/testservice mob|object|specs|spells|teleports"
		)]
	public class TestServiceCommand : AbstractCommandHandler, ICommandHandler {
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public void OnCommand(GameClient client, string[] args) {
			if (args.Length < 2) {
				DisplaySyntax(client);
				return;
			}
			GamePlayer player = client.Player;

			switch(args[1]) {
				case "mob":
					_testMobService(player);
					break;
				default:
					break;
			}			
		}

		private void _testMobService(GamePlayer player) {
			int count = 0;
			foreach (GameNPC living in player.GetNPCsInRadius(ushort.MaxValue, true)) {
				if (!living.IsAlive || living.ObjectState != GameObject.eObjectState.Active) {
					continue;
				}
				kill(player.Client, living);
				count++;
			}
			player.Out.SendMessage($"{count} mobs successfully killed", eChatType.CT_System, eChatLoc.CL_ChatWindow);
		}
		private void kill(GameClient client, GameNPC targetMob) {
			try {
				lock (targetMob.XPGainers.SyncRoot) {
					targetMob.attackComponent.AddAttacker(client.Player);
					targetMob.AddXPGainer(client.Player, targetMob.Health);
					targetMob.Die(client.Player);
					client.Out.SendMessage("Mob '" + targetMob.Name + "' killed", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				}
			} catch (Exception e) {
				client.Out.SendMessage(e.ToString(), eChatType.CT_System, eChatLoc.CL_SystemWindow);
			}
		}
	}
}
