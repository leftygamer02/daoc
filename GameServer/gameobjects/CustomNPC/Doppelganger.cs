/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using System;
using System.Collections.Generic;
using DOL.AI;
using DOL.Database;
using DOL.GS.Keeps;
using DOL.GS.PlayerClass;
using DOL.GS.ServerProperties;
using DOL.Language;
using log4net;
using System.Reflection;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
    public class Doppelganger : GameSummoner
    {
        /// <summary>
        /// Defines a logger for this class.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        override public int PetSummonThreshold { get { return 50; } }

        override public NpcTemplate PetTemplate { get { return m_petTemplate; } }
        static private NpcTemplate m_petTemplate = null;

        override public byte PetLevel { get { return 50; } }
        override public byte PetSize { get { return 50; } }

        public Doppelganger() : base() { }
        public Doppelganger(ABrain defaultBrain) : base(defaultBrain) { }
        public Doppelganger(INpcTemplate template) : base(template) { }

        static Doppelganger()
        {
           DBNpcTemplate chthonian = DOLDB<DBNpcTemplate>.SelectObject(DB.Column("Name").IsEqualTo("chthonian crawler"));
            if (chthonian != null)
                m_petTemplate = new NpcTemplate(chthonian);
        }

        /// <summary>
        /// Realm point value of this living
        /// </summary>
        public override int RealmPointsValue
        {
            get { return Properties.DOPPELGANGER_REALM_POINTS / 2; } // this is /2 because we have RP rate 2x currently
        }

        /// <summary>
        /// Bounty point value of this living
        /// </summary>
        public override int BountyPointsValue
        {
            get { return Properties.DOPPELGANGER_BOUNTY_POINTS; }
        }

        protected const ushort doppelModel = 2248;
        
        public override int MaxHealth
        {
            get
            {
                return base.MaxHealth * 8;
            }
        }
        
        public override double AttackDamage(InventoryItem weapon)
        {
            return base.AttackDamage(weapon) * 4;
        }

        /// <summary>
        /// Gets/sets the object health
        /// </summary>
        public override int Health
        {
            get { return base.Health; }
            set
            {
                base.Health = value;

                if (value >= MaxHealth)
                {
                    if (Model == doppelModel)
                        Disguise();
                }
            }
        }

        public override bool AddToWorld()
        {
            Flags = 0;
            Disguise();
            GameLocation spawnlocation = GetRandomLocation();
            Z = spawnlocation.Z;
            X = spawnlocation.X;
            Y = spawnlocation.Y;
            m_respawnInterval = 100;
            Heading = spawnlocation.Heading;
            CurrentRegionID = spawnlocation.RegionID;

            string message = $"{Name} has been last spotted in {CurrentZone.Description}";

            foreach (GameClient pclient in WorldMgr.GetAllPlayingClients())
            {
                if (pclient == null)
                    continue;
                
                pclient.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_ChatWindow);
            }
            
            Console.WriteLine(spawnlocation.Name);
            return base.AddToWorld();
        }

        public GameLocation GetRandomLocation()
        {
            GameLocation spawnlocation = new GameLocation("doppleganger", 0, 0, 0, 0, 0);

            // Albion locations
            GameLocation location1 = new GameLocation("Forest Sauvage DG", 1, 594571, 449523, 5125, 1014);
            GameLocation location2 = new GameLocation("Pennine Mountains DG", 1, 589003, 382870, 5818, 3620);
            GameLocation location3 = new GameLocation("Hadrian's Wall DG", 1, 635536, 330629, 5183, 761);
            GameLocation location4 = new GameLocation("Snowdonia DG", 1, 531330, 336193, 7120, 4086);
            
            // Midgard locations
            GameLocation location5 = new GameLocation("Uppland DG", 100, 755349, 640806, 4780, 982);
            GameLocation location6 = new GameLocation("Jamtland Mountains DG", 100, 692850, 645156, 5417, 1500);
            GameLocation location7 = new GameLocation("Odin's Gate DG", 100, 638904, 607120, 5512, 2854);
            GameLocation location8 = new GameLocation("Yggdra Forest DG", 100, 687558, 695521, 7251, 4033);
            
            // Hibernia locations
            GameLocation location9 = new GameLocation("Cruachan Gorge DG", 200, 346904, 386003, 3243, 2904);
            GameLocation location10 = new GameLocation("Breifine DG", 200, 412592, 386623, 4686, 844);
            GameLocation location11 = new GameLocation("Emain Macha DG", 200, 435842, 311014, 6308, 3980);
            GameLocation location12 = new GameLocation("Snowdonia DG", 200, 433660, 447794, 3607, 603);

            int randomLoc = Util.Random(1, 12);

            switch (randomLoc)
            {
                case 1:
                    spawnlocation = location1;
                    break;
                case 2:
                    spawnlocation = location2;
                    break;
                case 3:
                    spawnlocation = location3;
                    break;
                case 4:
                    spawnlocation = location4;
                    break;
                case 5:
                    spawnlocation = location5;
                    break;
                case 6:
                    spawnlocation = location6;
                    break;
                case 7:
                    spawnlocation = location7;
                    break;
                case 8:
                    spawnlocation = location8;
                    break;
                case 9:
                    spawnlocation = location9;
                    break;
                case 10:
                    spawnlocation = location10;
                    break;
                case 11:
                    spawnlocation = location11;
                    break;
                case 12:
                    spawnlocation = location12;
                    break;
            }

            return spawnlocation;
        }

        /// <summary>
        /// Load a npc from the npc template
        /// </summary>
        /// <param name="obj">template to load from</param>
        public override void LoadFromDatabase(DataObject obj)
        {
            base.LoadFromDatabase(obj);

            Disguise();
        }

        /// <summary>
        /// Starts a melee or ranged attack on a given target.
        /// </summary>
        /// <param name="attackTarget">The object to attack.</param>
        public override void StartAttack(GameObject attackTarget)
        {
            Level = 68;
            
            if (Model != doppelModel)
            {
                Model = doppelModel;
                Name = "doppelganger";
                Inventory = new GameNPCInventory(GameNpcInventoryTemplate.EmptyTemplate);
                BroadcastLivingEquipmentUpdate();
            }
            
            // Don't allow ranged attacks
            if (ActiveWeaponSlot == eActiveWeaponSlot.Distance)
            {
                bool standard = Inventory.GetItem(eInventorySlot.RightHandWeapon) != null;
                bool twoHanded = Inventory.GetItem(eInventorySlot.TwoHandWeapon) != null;

                if (standard && twoHanded)
                {
                    if (Util.Random(1) < 1)
                        SwitchWeapon(eActiveWeaponSlot.Standard);
                    else
                        SwitchWeapon(eActiveWeaponSlot.TwoHanded);
                }
                else if (twoHanded)
                    SwitchWeapon(eActiveWeaponSlot.TwoHanded);
                else
                    SwitchWeapon(eActiveWeaponSlot.Standard);
            }
            attackComponent.StartAttack(attackTarget);
        }
        
        public override void StopAttack()
        {
            Disguise();
            base.StopAttack();
        }
        
        public override void Die(GameObject killer)
         {
             if (killer != this)
                 base.Die(killer);

             foreach (GameClient client in WorldMgr.GetClientsOfZone(killer.CurrentZone.ID))
             {
                 if (client == null)
                     continue;
                 
                 if (client.Player.Realm != killer.Realm)
                     continue;
                 
                 
                 AtlasROGManager.GenerateOrbAmount(client.Player,100);
                 client.Player.UpdatePlayerStatus();
             }
         }

        /// <summary>
        /// Disguise the doppelganger as an invader
        /// </summary>
        protected void Disguise()
        {
            if (Util.Chance(50))
                Gender = eGender.Male;
            else
                Gender = eGender.Female;
            Level = 50;

            ICharacterClass characterClass = new DefaultCharacterClass();

            switch (Util.Random(2))
            {
                case 0: // Albion
                    Name = $"Albion {LanguageMgr.GetTranslation(LanguageMgr.DefaultLanguage, "GamePlayer.RealmTitle.Invader")}";

                    switch (Util.Random(4))
                    {
                        case 0: // Archer
                            Inventory = ClothingMgr.Albion_Archer.CloneTemplate();
                            SwitchWeapon(eActiveWeaponSlot.Distance);
                            characterClass = new ClassScout();
                            break;
                        case 1: // Caster
                            Inventory = ClothingMgr.Albion_Caster.CloneTemplate();
                            characterClass = new ClassTheurgist();
                            break;
                        case 2: // Fighter
                            Inventory = ClothingMgr.Albion_Fighter.CloneTemplate();
                            characterClass = new ClassArmsman();
                            break;
                        case 3: // GuardHealer
                            Inventory = ClothingMgr.Albion_Healer.CloneTemplate();
                            characterClass = new ClassCleric();
                            break;
                        case 4: // Stealther
                            Inventory = ClothingMgr.Albion_Stealther.CloneTemplate();
                            characterClass = new ClassInfiltrator();
                            break;
                    }
                    break;
                case 1: // Hibernia
                    Name = $"Hibernia {LanguageMgr.GetTranslation(LanguageMgr.DefaultLanguage, "GamePlayer.RealmTitle.Invader")}";

                    switch (Util.Random(4))
                    {
                        case 0: // Archer
                            Inventory = ClothingMgr.Hibernia_Archer.CloneTemplate();
                            SwitchWeapon(eActiveWeaponSlot.Distance);
                            characterClass = new ClassRanger();
                            break;
                        case 1: // Caster
                            Inventory = ClothingMgr.Hibernia_Caster.CloneTemplate();
                            characterClass = new ClassEldritch();
                            break;
                        case 2: // Fighter
                            Inventory = ClothingMgr.Hibernia_Fighter.CloneTemplate();
                            characterClass = new ClassArmsman();
                            break;
                        case 3: // GuardHealer
                            Inventory = ClothingMgr.Hibernia_Healer.CloneTemplate();
                            characterClass = new ClassDruid();
                            break;
                        case 4: // Stealther
                            Inventory = ClothingMgr.Hibernia_Stealther.CloneTemplate();
                            characterClass = new ClassNightshade();
                            break;
                    }
                    break;
                case 2: // Midgard
                    Name = $"Midgard {LanguageMgr.GetTranslation(LanguageMgr.DefaultLanguage, "GamePlayer.RealmTitle.Invader")}";

                    switch (Util.Random(4))
                    {
                        case 0: // Archer
                            Inventory = ClothingMgr.Midgard_Archer.CloneTemplate();
                            SwitchWeapon(eActiveWeaponSlot.Distance);
                            characterClass = new ClassHunter();
                            break;
                        case 1: // Caster
                            Inventory = ClothingMgr.Midgard_Caster.CloneTemplate();
                            characterClass = new ClassRunemaster();
                            break;
                        case 2: // Fighter
                            Inventory = ClothingMgr.Midgard_Fighter.CloneTemplate();
                            characterClass = new ClassWarrior();
                            break;
                        case 3: // GuardHealer
                            Inventory = ClothingMgr.Midgard_Healer.CloneTemplate();
                            characterClass = new ClassHealer();
                            break;
                        case 4: // Stealther
                            Inventory = ClothingMgr.Midgard_Stealther.CloneTemplate();
                            characterClass = new ClassShadowblade();
                            break;
                    }
                    break;
            }

            var possibleRaces = characterClass.EligibleRaces;
            var indexPick = Util.Random(0, possibleRaces.Count - 1);
            Model = (ushort)possibleRaces[indexPick].GetModel(Gender);

            bool distance = Inventory.GetItem(eInventorySlot.DistanceWeapon) != null;
            bool standard = Inventory.GetItem(eInventorySlot.RightHandWeapon) != null;
            bool twoHanded = Inventory.GetItem(eInventorySlot.TwoHandWeapon) != null;

            if (distance)
                SwitchWeapon(eActiveWeaponSlot.Distance);
            else if (standard && twoHanded)
            {
                if (Util.Random(1) < 1)
                    SwitchWeapon(eActiveWeaponSlot.Standard);
                else
                    SwitchWeapon(eActiveWeaponSlot.TwoHanded);
            }
            else if (twoHanded)
                SwitchWeapon(eActiveWeaponSlot.TwoHanded);
            else
                SwitchWeapon(eActiveWeaponSlot.Standard);
            
        }
    }
    
    public class DoppelgangerTracker : GameNPC
    {

        public DoppelgangerTracker() : base() { }
        public override bool AddToWorld()
        {
            GuildName = "Tracker";
            Level = 50;
            Flags |= eFlags.PEACE;
            base.AddToWorld();
            return true;
        }

        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
                return false;

            string message = "";
            
            List<GameNPC> doppelGanger = WorldMgr.GetNPCsByType(typeof(Doppelganger), eRealm.None);

            if (doppelGanger == null || doppelGanger.Count <= 0)
            {
                message = "Sorry, but there no invaders have been spotted in the world right now.";
            }
            
            else
            {
                message = "The following invaders have been spotted in the world: \n";
                foreach (Doppelganger dg in doppelGanger)
                {
                    message += "\n" + dg.Name + " in " + dg.CurrentZone.Description;
                }
            }
                     
            SayTo(player, message);
            
            return true;
        }
    }
}
