using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DOL.AI;
using DOL.AI.Brain;
using DOL.Events;
using DOL.Database;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.Scripts;

public class SplitMob : GameNPC
{
    public bool m_First = true;

    public SplitMob()
    {
    }

    public static bool AnyMinions(GameNPC checker)
    {
        foreach (GameNPC npc in checker.GetNPCsInRadius(10000))
            if (npc.Name.Contains("Minion"))
                return true;

        if (checker.Name.Contains("Minion")) return true;

        return false;
    }

    public void Split(GamePlayer player)
    {
        var check = false;
        if (Level < 45)
            if (!AnyMinions(this))
                check = true;

        if (check == true)
            return;
        Level -= 2;
        Health = MaxHealth;
        Size = (byte) Math.Max(Size - 5, 20);
        var mob = new SplitMob();
        SetVariables(mob);
        mob.AddToWorld();

        mob.StartAttack(player);
    }

    public void SetVariables(GameNPC mob)
    {
        mob.X = X + 10;
        mob.Y = Y + 10;
        mob.Z = Z;
        mob.CurrentRegion = CurrentRegion;
        mob.Heading = Heading;
        mob.Level = Level;
        mob.Realm = Realm;
        mob.Name = "Split's Minion";
        mob.Model = Model;
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
        mob.CurrentSpeed = 0;

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

    public void ResetToOriginal(GameNPC npc)
    {
        npc.Level = 70;
        npc.Health = MaxHealth;
    }

    public override void Die(GameObject killer)
    {
        Level = 60;
        Size = 100;
        base.Die(killer);
        if (Name == "Split")
        {
            foreach (GamePlayer player in GetPlayersInRadius(3000))
            {
                SendReply(player, "You have defeated " + Name + " and you gain 5000 bounty points");
                player.GainBountyPoints(5000, false);
            }

            foreach (GameNPC npc in GetNPCsInRadius(5000))
                if (npc.Name.Contains("Minion"))
                    npc.RemoveFromWorld();
        }
    }

    public override void TakeDamage(GameObject source, eDamageType damageType, int damageAmount, int criticalAmount)
    {
        var player = source as GamePlayer;
        if (player != null)
            if (HealthPercent < 50)
                Split(player);

        base.TakeDamage(source, damageType, damageAmount, criticalAmount);
    }

    public void SendReply(GamePlayer player, string msg)
    {
        player.Out.SendMessage(msg, eChatType.CT_System, eChatLoc.CL_PopupWindow);
    }
}