using System;
using System.Reflection;
using DOL.AI;
using DOL.AI.Brain;
using DOL.GS.PacketHandler;
using DOL.Database;

namespace DOL.GS.Scripts;

public class ElroTheAncient : GameEpicBoss
{
    public ElroTheAncient()
    {
        TetherRange = 4500;
        ScalingFactor = 55;
    }

    public override int GetResist(eDamageType damageType)
    {
        switch (damageType)
        {
            case eDamageType.Slash: return 20; // dmg reduction for melee dmg
            case eDamageType.Crush: return 20; // dmg reduction for melee dmg
            case eDamageType.Thrust: return 20; // dmg reduction for melee dmg
            default: return 30; // dmg reduction for rest resists
        }
    }

    public override double AttackDamage(InventoryItem weapon)
    {
        return base.AttackDamage(weapon) * Strength / 100;
    }

    public override int AttackRange
    {
        get => 350;
        set { }
    }

    public override bool HasAbility(string keyName)
    {
        if (IsAlive && keyName == GS.Abilities.CCImmunity)
            return true;

        return base.HasAbility(keyName);
    }

    public override double GetArmorAF(eArmorSlot slot)
    {
        return 350;
    }

    public override double GetArmorAbsorb(eArmorSlot slot)
    {
        // 85% ABS is cap.
        return 0.20;
    }

    public override int MaxHealth => 30000;

    #region Stats

    public override short Charisma
    {
        get => base.Charisma;
        set => base.Charisma = 200;
    }

    public override short Piety
    {
        get => base.Piety;
        set => base.Piety = 200;
    }

    public override short Intelligence
    {
        get => base.Intelligence;
        set => base.Intelligence = 200;
    }

    public override short Empathy
    {
        get => base.Empathy;
        set => base.Empathy = 400;
    }

    public override short Dexterity
    {
        get => base.Dexterity;
        set => base.Dexterity = 200;
    }

    public override short Quickness
    {
        get => base.Quickness;
        set => base.Quickness = 80;
    }

    public override short Strength
    {
        get => base.Strength;
        set => base.Strength = 300;
    }

    #endregion

    public override bool AddToWorld()
    {
        Name = "Elro the Ancient";
        GuildName = "";
        Model = 767;
        Size = 150;
        Level = 65;
        Realm = eRealm.None;
        RespawnInterval =
            ServerProperties.Properties.SET_EPIC_GAME_ENCOUNTER_RESPAWNINTERVAL * 60000; //1min is 60000 miliseconds
        base.AddToWorld();
        return true;
    }

    public void Spawn(GamePlayer player)
    {
        var mob = new GameNPC();
        SetVariables(mob);
        //Level Range of 50-55
        var level = Util.Random(50, 55);
        mob.Level = (byte) level;
        mob.Size = 50;
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
        mob.Name = "ancient treant";
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
        base.Die(killer);
        foreach (GameNPC npc in GetNPCsInRadius(5000))
            if (npc.Name.Contains("ancient treant"))
                npc.Die(killer);
    }

    public override void TakeDamage(GameObject source, eDamageType damageType, int damageAmount, int criticalAmount)
    {
        var player = source as GamePlayer;
        if (player != null)
        {
            if (HealthPercent < 95 && HealthPercent > 90)
                new ECSGameTimer(this, new ECSGameTimer.ECSTimerCallback(timer => CastTreant(timer, player)), 1000);

            else if (HealthPercent < 60 && HealthPercent > 55)
                new ECSGameTimer(this, new ECSGameTimer.ECSTimerCallback(timer => CastTreant(timer, player)), 1000);

            else if (HealthPercent < 25 && HealthPercent > 30)
                new ECSGameTimer(this, new ECSGameTimer.ECSTimerCallback(timer => CastTreant(timer, player)), 1000);
        }

        base.TakeDamage(source, damageType, damageAmount, criticalAmount);
    }

    private int CastTreant(ECSGameTimer timer, GamePlayer player)
    {
        foreach (GamePlayer enemy in GetPlayersInRadius(2500)) Spawn(enemy);

        return 0;
    }

    public void SendReply(GamePlayer player, string msg)
    {
        player.Out.SendMessage(msg, eChatType.CT_System, eChatLoc.CL_PopupWindow);
    }
}