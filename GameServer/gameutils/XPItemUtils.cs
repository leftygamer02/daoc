using DOL.Database;

namespace DOL.GS;

public class XPItemUtils
{
    public static XPItem GetRandomForPlayer(GamePlayer player)
    {
        var xpItems = DOLDB<XPItem>.SelectObjects(DB.Column("MinLevel").IsLessThan(player.Level).And(DB.Column("MaxLevel").IsGreatherThan(player.Level)).And(DB.Column("Realm").IsEqualTo(player.Realm)));
        
        var random = Util.Random(0, xpItems.Count);
        var xpitem = xpItems[random];

        return xpitem;
    }
}