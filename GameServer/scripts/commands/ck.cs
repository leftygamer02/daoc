/*
*
* Atlas - Shows battleground CK owner Realm 
*
*/

using DOL.GS.PacketHandler;
using System.Collections;
using System.Collections.Generic;
using DOL.Language;
using DOL.GS.Keeps;
using DOL.GS.ServerRules;

namespace DOL.GS.Commands;

[CmdAttribute(
    "&ck",
    ePrivLevel.Player,
    "Displays who owns the CK while in a battleground.", "/ck")]
public class CkCommandHandler : AbstractCommandHandler, ICommandHandler
{
    public void OnCommand(GameClient client, string[] args)
    {
        if (IsSpammingCommand(client.Player, "ck"))
            return;

        var bgName = client.Player.CurrentZone.Description;

        if (GameServer.KeepManager.GetBattleground(client.Player.CurrentRegionID) != null)
        {
            var keepList =
                GameServer.KeepManager.GetKeepsOfRegion(client.Player.CurrentRegionID);
            foreach (var keep in keepList) ChatUtil.SendSystemMessage(client, KeepStringBuilder(keep));
        }
        else
        {
            client.Out.SendMessage("You need to be in a battleground to use this command.", eChatType.CT_Important,
                eChatLoc.CL_SystemWindow);
        }
    }

    private string KeepStringBuilder(AbstractGameKeep keep)
    {
        var buffer = "";
        buffer += keep.Name + ": " + GlobalConstants.RealmToName(keep.Realm);
        if (keep.Guild != null) buffer += " (" + keep.Guild.Name + ")";

        buffer += "\n";
        return buffer;
    }
}