using Atlas.DataLayer.Models;
using DOL.Events;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.GS.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DOL.GS.Scripts
{
    static class ZoneBonusRotator
    {
        // Region ID's
        public const int ALBION_CLASSIC_ID = 1;
        public const int ALBION_SI_ID = 51;
        public const int MIDGARD_CLASSIC_ID = 100;
        public const int MIDGARD_SI_ID = 151;
        public const int HIBERNIA_CLASSIC_ID = 200;
        public const int HIBERNIA_SI_ID = 181;

        // RvR Zone ID's
        private static List<int> albionRvRZones = new List<int>() { 11, 12, 14, 15};
        private static List<int> midgardRvRZones = new List<int>() { 111, 112, 113, 115 };
        private static List<int> hiberniaRvRZones = new List<int>() { 210, 211, 212, 214 };

        // PvE Zone ID's
        private static List<int> albionClassicZones = new List<int>();
        private static List<int> albionSIZones = new List<int>();
        private static List<int> midgardClassicZones = new List<int>();
        private static List<int> midgardSIZones = new List<int>();
        private static List<int> hiberniaClassicZones = new List<int>();
        private static List<int> hiberniaSIZones = new List<int>();

        // Current OF Realm with Bonuses
        private static int currentRvRRealm = 0;

        // Current PvE Zones with Bonuses
        private static int currentAlbionZone;
        private static int currentAlbionZoneSI;
        private static int currentMidgardZone;
        private static int currentMidgardZoneSI;
        private static int currentHiberniaZone;
        private static int currentHiberniaZoneSI;

        private static Atlas.DataLayer.Models.Zone albDBZone;
        private static Atlas.DataLayer.Models.Zone albDBZoneSI;
        private static Atlas.DataLayer.Models.Zone midDBZone;
        private static Atlas.DataLayer.Models.Zone midDBZoneSI;
        private static Atlas.DataLayer.Models.Zone hibDBZone;
        private static Atlas.DataLayer.Models.Zone hibDBZoneSI;

        private static SimpleScheduler scheduler = new SimpleScheduler();

        public static int PvETimer { get; set; }
        public static int RvRTimer { get; set; } 
        public static int PvEExperienceBonusAmount { get; set; }
        public static int RvRExperienceBonusAmount { get; set; } 
        public static int RPBonusAmount { get; set; } 
        public static int BPBonusAmount { get; set; } 

        [GameServerStartedEvent]
        public static void OnServerStart(DOLEvent e, object sender, EventArgs arguments)
        {
            PvETimer = 7200 * 1000;
            RvRTimer = 2700 * 1000;
            PvEExperienceBonusAmount = 100;
            RvRExperienceBonusAmount = 200;
            RPBonusAmount = 100;
            BPBonusAmount = 100;
             
            GetZones();
            UpdatePvEZones();
            UpdateRvRZones();
            GameEventMgr.AddHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(PlayerEntered));
            
        }

        [GameServerStoppedEvent]
        public static void OnServerStopped(DOLEvent e, object sender, EventArgs arguments)
        {
            // Should be changed to keep data saved in DB for restart
            ClearPvEZones();
            ClearRvRZones();

            GameEventMgr.RemoveHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(PlayerEntered));
        }
        
        /// <summary>
        /// Gets all ZoneID's for PvE Zones
        /// </summary>
        public static void GetZones()
        {
            // Get Albion ZoneID's
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == ALBION_CLASSIC_ID))
            {
                if (!albionRvRZones.Contains(zone.Id))
                {
                    albionClassicZones.Add(zone.Id);
                }
            }
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == ALBION_SI_ID))
            {
                albionSIZones.Add(zone.Id);
            }

            // Get Midgard ZoneID's
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == MIDGARD_CLASSIC_ID))
            {
                if (!midgardRvRZones.Contains(zone.Id))
                {
                    midgardClassicZones.Add(zone.Id);
                }
            }
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == MIDGARD_SI_ID))
            {
                midgardSIZones.Add(zone.Id);
            }

            // Get Hibernia ZoneID's
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == HIBERNIA_CLASSIC_ID))
            {
                if (!hiberniaRvRZones.Contains(zone.Id))
                {
                    hiberniaClassicZones.Add(zone.Id);
                }
            }
            foreach (var zone in GameServer.Database.Zones.Where(x => x.RegionID == HIBERNIA_SI_ID))
            {
                hiberniaSIZones.Add(zone.Id);
            }
        }
        public static void PlayerEntered(DOLEvent e, object sender, EventArgs arguments)
        {
            GamePlayer player = sender as GamePlayer;
            TellClient(player.Client);
        }

        private static int UpdatePvEZones()
        {
            ClearPvEZones();

            GetNextPvEZones();

            albDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == albionClassicZones[currentAlbionZone]);
            albDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == albionSIZones[currentAlbionZoneSI]);
            midDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == midgardClassicZones[currentMidgardZone]);
            midDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == midgardSIZones[currentMidgardZoneSI]);
            hibDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == hiberniaClassicZones[currentHiberniaZone]);
            hibDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == hiberniaSIZones[currentHiberniaZoneSI]);

            // Set XP Bonuses in DB
            albDBZone.Experience = PvEExperienceBonusAmount;
            albDBZoneSI.Experience = PvEExperienceBonusAmount;
            midDBZone.Experience = PvEExperienceBonusAmount;
            midDBZoneSI.Experience = PvEExperienceBonusAmount;
            hibDBZone.Experience = PvEExperienceBonusAmount;
            hibDBZoneSI.Experience = PvEExperienceBonusAmount;

            // Save XP Bonuses in DB
            GameServer.Instance.SaveDataObject(albDBZone);
            GameServer.Instance.SaveDataObject(albDBZoneSI);
            GameServer.Instance.SaveDataObject(midDBZone);
            GameServer.Instance.SaveDataObject(midDBZoneSI);
            GameServer.Instance.SaveDataObject(hibDBZone);
            GameServer.Instance.SaveDataObject(hibDBZoneSI);

            // Update Bonuses In-Game
            WorldMgr.Zones[(ushort)albionClassicZones[currentAlbionZone]].BonusExperience = PvEExperienceBonusAmount;
            WorldMgr.Zones[(ushort)albionSIZones[currentAlbionZoneSI]].BonusExperience = PvEExperienceBonusAmount;
            WorldMgr.Zones[(ushort)midgardClassicZones[currentMidgardZone]].BonusExperience = PvEExperienceBonusAmount;
            WorldMgr.Zones[(ushort)midgardSIZones[currentMidgardZoneSI]].BonusExperience = PvEExperienceBonusAmount;
            WorldMgr.Zones[(ushort)hiberniaClassicZones[currentHiberniaZone]].BonusExperience = PvEExperienceBonusAmount;
            WorldMgr.Zones[(ushort)hiberniaSIZones[currentHiberniaZoneSI]].BonusExperience = PvEExperienceBonusAmount;

            foreach (GameClient client in WorldMgr.GetAllClients())
            {
                TellClient(client);
            }

            scheduler.Start(UpdatePvEZones, PvETimer);

            return 0;
        }

        private static int UpdateRvRZones()
        {
            ClearRvRZones();

            // Get new RvR Realm and set Bonuses for each Zone
            GetNextRvRZone();

            switch (currentRvRRealm)
            {
                case 1:
                    foreach (int i in albionRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = RvRExperienceBonusAmount;
                        zone.Realmpoints = RPBonusAmount;
                        zone.Bountypoints = BPBonusAmount;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = RvRExperienceBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = RPBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = BPBonusAmount;
                    }
                    break;
                case 2:
                    foreach (int i in midgardRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = RvRExperienceBonusAmount;
                        zone.Realmpoints = RPBonusAmount;
                        zone.Bountypoints = BPBonusAmount;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = RvRExperienceBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = RPBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = BPBonusAmount;
                    }
                    break;
                case 3:
                    foreach (int i in hiberniaRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = RvRExperienceBonusAmount;
                        zone.Realmpoints = RPBonusAmount;
                        zone.Bountypoints = BPBonusAmount;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = RvRExperienceBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = RPBonusAmount;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = BPBonusAmount;
                    }
                    break;
            }

            foreach (GameClient client in WorldMgr.GetAllClients())
            {
                TellClient(client);
            }

            scheduler.Start(UpdateRvRZones, RvRTimer);

            return 0;
        }

        /// <summary>
        /// Rotate through the realms for Bonuses
        /// 1) Albion
        /// 2) Midgard
        /// 3) Hibernia
        /// </summary>
        private static void GetNextRvRZone()
        {
            if (currentRvRRealm < 3)
                currentRvRRealm += 1; // currentRvRRealm++;// currentRealm + 1;
            else
                currentRvRRealm = 1;
        }

        private static void GetNextPvEZones()
        {
            if (currentAlbionZone < albionClassicZones.Count - 1)
                currentAlbionZone += 1;
            else
                currentAlbionZone = 0;

            if (currentAlbionZoneSI < albionSIZones.Count - 1)
                currentAlbionZoneSI += 1;
            else
                currentAlbionZoneSI = 0;

            if (currentMidgardZone < midgardClassicZones.Count - 1)
                currentMidgardZone += 1;
            else
                currentMidgardZone = 0;

            if (currentMidgardZoneSI < midgardSIZones.Count - 1)
                currentMidgardZoneSI += 1;
            else
                currentMidgardZoneSI = 0;

            if (currentHiberniaZone < hiberniaClassicZones.Count - 1)
                currentHiberniaZone += 1;
            else
                currentHiberniaZone = 0;

            if (currentHiberniaZoneSI < hiberniaSIZones.Count - 1)
                currentHiberniaZoneSI += 1;
            else
                currentHiberniaZoneSI = 0;
        }

        private static void TellClient(GameClient client)
        {
            client.Out.SendMessage(GetText(), eChatType.CT_Important, eChatLoc.CL_SystemWindow);
        }

        public static string GetText()
        {
            string realm = "";
            switch (currentRvRRealm)
            {
                case 1:
                    realm = "Albion";
                    break;
                case 2:
                    realm = "Midgard";
                    break;
                case 3:
                    realm = "Hibernia";
                    break;
            }
            return "\nOF Bonus Region: " + realm + "\n\n" +
                "Albion Classic: " + albDBZone.Name + " (XP +" + albDBZone.Experience + "%)\n" +
                "Albion SI: " + albDBZoneSI.Name + " (XP +" + albDBZoneSI.Experience + "%)\n\n" +
                "Midgard Classic: " + midDBZone.Name + " (XP +" + midDBZone.Experience + "%)\n" +
                "MIdgard SI: " + midDBZoneSI.Name + " (XP +" + midDBZoneSI.Experience + "%)\n\n" +
                "Hibernia Classic: " + hibDBZone.Name + " (XP +" + hibDBZone.Experience + "%)\n" +
                "Hibernia SI: " + hibDBZoneSI.Name + " (XP +" + hibDBZoneSI.Experience + "%)\n\n";
        }

        public static List<string> GetTextList()
        {
            List<string> temp = new List<string>();
            string realm = "";
            switch (currentRvRRealm)
            {
                case 1:
                    realm = "Albion";
                    break;
                case 2:
                    realm = "Midgard";
                    break;
                case 3:
                    realm = "Hibernia";
                    break;
            }
            temp.Add("Current OF Bonus Region: " + realm);
            temp.Add("Bonus RP: " + RPBonusAmount + "%");
            temp.Add("Bonus BP: " + BPBonusAmount + "%");
            temp.Add("Bonus XP: " + RvRExperienceBonusAmount + "%");
            temp.Add("");
            temp.Add("Current Albion Zones: ");
            temp.Add("Classic Zone: " + albDBZone.Name + " (XP +" + albDBZone.Experience + "%)");
            temp.Add("SI Zone: " + albDBZoneSI.Name + " (XP +" + albDBZoneSI.Experience + "%)");
            temp.Add("");
            temp.Add("Current Midgard Zones: ");
            temp.Add("Classic Zone: " + midDBZone.Name + " (XP +" + midDBZone.Experience + "%)");
            temp.Add("SI Zone: " + midDBZoneSI.Name + " (XP +" + midDBZoneSI.Experience + "%)");
            temp.Add("");
            temp.Add("Current Hibernia Zones: ");
            temp.Add("Classic Zone: " + hibDBZone.Name + " (XP +" + hibDBZone.Experience + "%)");
            temp.Add("SI Zone: " + hibDBZoneSI.Name + " (XP +" + hibDBZoneSI.Experience + "%)");

            return temp;
        }

        private static void ClearRvRZones()
        {
            // Clear RvR Zone
            switch (currentRvRRealm)
            {
                case 1:
                    foreach (int i in albionRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = 0;
                        zone.Realmpoints = 0;
                        zone.Bountypoints = 0;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = 0;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = 0;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = 0;
                    }
                    break;
                case 2:
                    foreach (int i in midgardRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = 0;
                        zone.Realmpoints = 0;
                        zone.Bountypoints = 0;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = 0;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = 0;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = 0;
                    }
                    break;
                case 3:
                    foreach (int i in hiberniaRvRZones)
                    {
                        var zone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == i);
                        zone.Experience = 0;
                        zone.Realmpoints = 0;
                        zone.Bountypoints = 0;
                        GameServer.Instance.SaveDataObject(zone);

                        WorldMgr.Zones[(ushort)i].BonusExperience = 0;
                        WorldMgr.Zones[(ushort)i].BonusRealmpoints = 0;
                        WorldMgr.Zones[(ushort)i].BonusBountypoints = 0;
                    }
                    break;
            }
        }

        private static void ClearPvEZones()
        {
            // Clear PvE Zones
            albDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == albionClassicZones[currentAlbionZone]);
            albDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == albionSIZones[currentAlbionZoneSI]);
            midDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == midgardClassicZones[currentMidgardZone]);
            midDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == midgardSIZones[currentMidgardZoneSI]);
            hibDBZone = GameServer.Database.Zones.FirstOrDefault(x => x.Id == hiberniaClassicZones[currentHiberniaZone]);
            hibDBZoneSI = GameServer.Database.Zones.FirstOrDefault(x => x.Id == hiberniaSIZones[currentHiberniaZoneSI]);

            albDBZone.Experience = 0;
            albDBZoneSI.Experience = 0;
            midDBZone.Experience = 0;
            midDBZoneSI.Experience = 0;
            hibDBZone.Experience = 0;
            hibDBZoneSI.Experience = 0;

            GameServer.Instance.SaveDataObject(albDBZone);
            GameServer.Instance.SaveDataObject(albDBZoneSI);
            GameServer.Instance.SaveDataObject(midDBZone);
            GameServer.Instance.SaveDataObject(midDBZoneSI);
            GameServer.Instance.SaveDataObject(hibDBZone);
            GameServer.Instance.SaveDataObject(midDBZoneSI);

            WorldMgr.Zones[(ushort)albionClassicZones[currentAlbionZone]].BonusExperience = 0;
            WorldMgr.Zones[(ushort)albionSIZones[currentAlbionZoneSI]].BonusExperience = 0;
            WorldMgr.Zones[(ushort)midgardClassicZones[currentMidgardZone]].BonusExperience = 0;
            WorldMgr.Zones[(ushort)midgardSIZones[currentMidgardZoneSI]].BonusExperience = 0;
            WorldMgr.Zones[(ushort)hiberniaClassicZones[currentHiberniaZone]].BonusExperience = 0;
            WorldMgr.Zones[(ushort)hiberniaSIZones[currentHiberniaZoneSI]].BonusExperience = 0;
        }
    }
}
