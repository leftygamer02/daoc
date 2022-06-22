using DOL.Database;

namespace DOL.GS;

public class XPItemUtils
{
    public static XPItem GetRandomForPlayer(GamePlayer player)
    {
        var xpItems = DOLDB<XPItem>.SelectObjects(DB.Column("MinLevel").IsLessThan(player.Level).And(DB.Column("MaxLevel").IsGreatherThan(player.Level)).And(DB.Column("Realm").IsEqualTo(player.Realm)));
        
        if (xpItems.Count == 0)
            return null;
        
        var random = Util.Random(0, xpItems.Count - 1);
        var xpitem = xpItems[random];

        return xpitem;
    }
    public static XPItem GetFromID(string id)
    {
        var xpItem = DOLDB<XPItem>.SelectObject(DB.Column("XPItemID").IsEqualTo(id));
        return xpItem;
    }
}