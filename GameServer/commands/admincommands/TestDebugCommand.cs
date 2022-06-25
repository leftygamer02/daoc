using System.Reflection;
using DOL.GS.Commands;
using log4net;

namespace DOL.GS.Commandss {
    [Cmd("&TestService",
		ePrivLevel.Admin,
		"Sends",
		"/testservice mob|object|specs|spells|teleports"
		)]
	public class TestService : AbstractCommandHandler, ICommandHandler {
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public void OnCommand(GameClient client, string[] args) {
			if (args.Length < 2) {
				DisplaySyntax(client);
				return;
			}
			GamePlayer target = client.Player;

		}
	}
}
