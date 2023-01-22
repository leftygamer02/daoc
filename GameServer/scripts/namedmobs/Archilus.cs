using System;
using System.Reflection;
using DOL.AI;
using DOL.AI.Brain;
using DOL.GS.PacketHandler;

namespace DOL.GS.Scripts;

public class Archilus : GameNPC
{
    protected string m_SpawnAnnounce;

    public Archilus()
    {
        m_SpawnAnnounce = "{0} will start to \'shake violently\' and spawns out some {1}!";
        TetherRange = 4500;
        ScalingFactor = 25;
    }

    public override bool AddToWorld()
    {
        Name = "Archilus";
        GuildName = "";
        Model = 817;
        Size = 100;
        Level = 58;
        Realm = eRealm.None;
        base.AddToWorld();
        return true;
    }

    /// <summary>
    /// Broadcast relevant messages.
    /// </summary>
    /// <param name="message">The message to be broadcast.</param>
    public void BroadcastMessage(string message)
    {
        foreach (GamePlayer player in GetPlayersInRadius(WorldMgr.OBJ_UPDATE_DISTANCE))
            player.Out.SendMessage(message, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
    }

    public void Spawn(GamePlayer player)
    {
        var mob = new GameNPC();
        SetVariables(mob);
        //Level Range of 40-45
        var level = Util.Random(40, 45);
        mob.Level = (byte) level;
        BroadcastMessage(string.Format(m_SpawnAnnounce, Name, mob.Name));
        mob.AddToWorld();

        mob.StartAttack(player);
    }

    public void SetVariables(GameNPC mob)
    {
        mob.X = X + 350;
        mob.Y = Y + 350;
        mob.Z = Z;
        mob.CurrentRegion = CurrentRegion;
        mob.Heading = Heading;
        //mob.Level = this.Level;
        mob.Realm = Realm;
        mob.Name = "young death shroud";
        mob.Model = Model;
        mob.Size = 50;
        mob.Flags = Flags;
        mob.MeleeDamageType = MeleeDamageType;
        mob.RespawnInterval = -1; // dont respawn
        mob.RoamingRange = RoamingRange;
        mob.MaxDistance = 4000;

        // also copies the stats

        mob.Strength = Strength;
        mob.Constitution = Constitution;
        mob.Dexterity = Dexterity;
        mob.Quickness = Quickness;
        mob.Intelligence = Intelligence;
        mob.Empathy = Empathy;
        mob.Piety = Piety;
        mob.Charisma = Charisma;

        //Fill the living variables
        mob.CurrentSpeed = 200;

        mob.MaxSpeedBase = MaxSpeedBase;
        mob.Size = Size;
        mob.NPCTemplate = NPCTemplate;
        mob.Inventory = Inventory;
        mob.EquipmentTemplateID = EquipmentTemplateID;
        if (mob.Inventory != null)
            mob.SwitchWeapon(ActiveWeaponSlot);

        ABrain brain = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            brain = (ABrain) assembly.CreateInstance(Brain.GetType().FullName, true);
            if (brain != null)
                break;
        }

        if (brain == null)
        {
            mob.SetOwnBrain(new StandardMobBrain());
        }
        else if (brain is StandardMobBrain)
        {
            var sbrain = (StandardMobBrain) brain;
            var tsbrain = (StandardMobBrain) Brain;
            sbrain.AggroLevel = tsbrain.AggroLevel;
            sbrain.AggroRange = tsbrain.AggroRange;
            mob.SetOwnBrain(sbrain);
        }
    }

    public override void Die(GameObject killer)
    {
        Level = 60;
        Size = 100;
        base.Die(killer);
        foreach (GameNPC npc in GetNPCsInRadius(5000))
            if (npc.Name.Contains("young death shroud"))
                npc.Die(killer);
    }

    public override void TakeDamage(GameObject source, eDamageType damageType, int damageAmount, int criticalAmount)
    {
        var player = source as GamePlayer;
        if (player != null)
            if (HealthPercent < 90)
                new ECSGameTimer(this, new ECSGameTimer.ECSTimerCallback(timer => CastShroud(timer, player)), 1000);

        base.TakeDamage(source, damageType, damageAmount, criticalAmount);
    }

    private int CastShroud(ECSGameTimer timer, GamePlayer player)
    {
        Spawn(player);
        return 0;
    }

    public void SendReply(GamePlayer player, string msg)
    {
        player.Out.SendMessage(msg, eChatType.CT_System, eChatLoc.CL_PopupWindow);
    }
}