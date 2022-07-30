using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DOL.GS;
using DOL.Database;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
    public class CloneInstance : Instance
    {
        public CloneInstance(ushort ID, GameTimer.TimeManager time, RegionData dat)
            : base(ID, time, dat)
        {
        }
        
        //Change instance level...
        //I've checked, this should be called correctly: player will be added/removed in time.
        public override void OnPlayerEnterInstance(GamePlayer player)
        {
            base.OnPlayerEnterInstance(player);
            UpdateInstanceLevel();
        }
 
        public override void OnPlayerLeaveInstance(GamePlayer player)
        {
            base.OnPlayerLeaveInstance(player);
            UpdateInstanceLevel();
        }
 
        public override void LoadFromDatabase(string instanceName)
        {
            base.LoadFromDatabase(instanceName);
            Assembly ambly = Assembly.GetAssembly(typeof(GameServer));
 
 
            IList<Mob> mobs = GameServer.Database.SelectObjects<Mob>("`Region` = '" + Skin.ToString() + "'");
            if (mobs.Count > 0)
            {
                foreach (Mob mob in mobs)
                {
                    GameNPC myMob = null;
                    string error = string.Empty;
 
                    if (mob.Guild.Length > 0 && mob.Realm is >= 0 and <= (int)eRealm._Last)
                    {
                        Type type = ScriptMgr.FindNPCGuildScriptClass(mob.Guild, (eRealm)mob.Realm);
                        if (type != null)
                        {
                            try
                            {
                                Type[] constructorParams;
                                if (mob.NPCTemplateID != -1)
                                {
                                    constructorParams = new Type[] { typeof(INpcTemplate) };
                                    ConstructorInfo handlerConstructor = typeof(GameNPC).GetConstructor(constructorParams);
                                    INpcTemplate template = NpcTemplateMgr.GetTemplate(mob.NPCTemplateID);
                                    myMob = (GameNPC)handlerConstructor.Invoke(new object[] { template });
                                }
                                else
                                {
                                    myMob = (GameNPC)type.Assembly.CreateInstance(type.FullName);
                                }
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                    
                    if (myMob == null)
                    {
                        string classtype = ServerProperties.Properties.GAMENPC_DEFAULT_CLASSTYPE;
 
                        if (mob.ClassType != null && mob.ClassType.Length > 0 && mob.ClassType != Mob.DEFAULT_NPC_CLASSTYPE)
                        {
                            classtype = mob.ClassType;
                        }
 
                        try
                        {
                            myMob = (GameNPC)ambly.CreateInstance(classtype, false);
                        }
                        catch
                        {
                            error = classtype;
                        }
 
                        if (myMob == null)
                        {
                            foreach (Assembly asm in ScriptMgr.Scripts)
                            {
                                try
                                {
                                    myMob = (GameNPC)asm.CreateInstance(classtype, false);
                                    error = string.Empty;
                                }
                                catch
                                {
                                    error = classtype;
                                }
 
                                if (myMob != null)
                                    break;
                            }
 
                            if (myMob == null)
                            {
                                myMob = new GameNPC();
                                error = classtype;
                            }
                        }
                    }

                    if (myMob != null)
                    {
                        try
                        {
                            myMob.LoadFromDatabase(mob);
                            myMob.CurrentRegionID = this.ID;
                            myMob.RespawnInterval = -1;
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
 
                        myMob.AddToWorld();
 
                        if (myMob.Name.ToLower() == "spawn-in")
                        {
                            m_entranceLocation = new GameLocation(instanceName + "entranceRegion" + ID, ID, myMob.X, myMob.Y, myMob.Z, myMob.Heading);
                        }
                    }
                }
            }
        }
        
        //This void is outside of Instance,
        //because i want people to think carefully about how they change levels in their instance.
        public void UpdateInstanceLevel()
        {
        }
 
        /// <summary>
        /// Expire the missions - the instance has exploded.
        /// </summary>
        public override void OnCollapse()
        {
            //We expire the mission as players can no longer reach or access the region once collapsed.
            base.OnCollapse();
        }
    }
}