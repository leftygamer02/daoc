﻿using DOL.GS.Commands;

/* Disabled functionality as it's not very classic/SI
namespace DOL.GS.Scripts
{
    [CmdAttribute(
       "&conquest",
       ePrivLevel.Player,
         "Displays the current conqust status.", "/conquest")]
    public class ConquestCommandHandler : AbstractCommandHandler, ICommandHandler
    {
        public void OnCommand(GameClient client, string[] args)
        {
            if (!IsSpammingCommand(client.Player, "task"))
            {
                client.Out.SendCustomTextWindow("Conquest Information", ConquestService.ConquestManager.GetTextList(client.Player));
            }
        }
    }
}
*/
