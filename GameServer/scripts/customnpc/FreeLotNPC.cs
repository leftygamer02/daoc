using DOL.GS;
using DOL.GS.Housing;
using System.Collections.Generic;
using System;
using DOL.Database;

namespace DOL.GS.Scripts
{
    public class FreeLotNPC : GameNPC
    {
        List<eHousingZone> albZones = new List<eHousingZone>();
        List<eHousingZone> midZones = new List<eHousingZone>();
        List<eHousingZone> hibZones = new List<eHousingZone>();

        public FreeLotNPC():base()
        {
            albZones.Add(eHousingZone.Caerwent);
            albZones.Add(eHousingZone.Rilan);
            albZones.Add(eHousingZone.Dalton);
            albZones.Add(eHousingZone.Old_Sarum);

            midZones.Add(eHousingZone.Erikstaad);
            midZones.Add(eHousingZone.Carlingford);
            midZones.Add(eHousingZone.Wyndham);
            midZones.Add(eHousingZone.Arothi);

            hibZones.Add(eHousingZone.Meath);
            hibZones.Add(eHousingZone.Torrylin);
            hibZones.Add(eHousingZone.Kilcullen);
            hibZones.Add(eHousingZone.Dunshire);

        }

        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
                return false;

            string msg = "";

            msg += "Hello " + player.Name + ", I heard that you were looking for a house.\n\n";

            msg += "The following zones have free houses:\n";

            List<eHousingZone> searchZones = new List<eHousingZone>();

            switch (player.Realm)
            {
                case eRealm.Albion: searchZones = albZones; break;
                case eRealm.Midgard: searchZones = midZones; break;
                case eRealm.Hibernia: searchZones = hibZones; break;
            }

            int totalFree = 0;
            foreach (eHousingZone zone in searchZones)
            {

                int free = GetFreeHousesForZone(zone);
                if (free == 0)
                    continue;
                totalFree += free;

                string zoneStr = zone.ToString().Replace('_', ' ');

               msg += "\n[" + zoneStr + "] - " + free + " free";
            }

            if (totalFree == 0)
                msg += "\n\nI'm sorry, no zones have free houses at this time.";
            else
                msg += "\n\nThere are currently " + totalFree + " free lots across all zones!";

            SayTo(player, msg);

            return true;
        }

        public override bool WhisperReceive(GameLiving source, string text)
        {
            if (!base.WhisperReceive(source, text))
                return false;

            GamePlayer player = source as GamePlayer;
            if (player == null)
                return false;

            //listen for zones
            List<eHousingZone> searchZones = new List<eHousingZone>();

            switch (source.Realm)
            {
                case eRealm.Albion: searchZones = albZones; break;
                case eRealm.Midgard: searchZones = midZones; break;
                case eRealm.Hibernia: searchZones = hibZones; break;
            }

            foreach (eHousingZone zone in searchZones)
            {

                string zoneStr = zone.ToString().Replace('_', ' ');
                if (zoneStr != text)
                    continue;

                string msg = "";
                int totalFree = 0;

                List<eVillage> searchVillages = GetVillagesInZone(zone);
                foreach (eVillage village in searchVillages)
                {

                    int free = GetFreeHousesForVillage(village);
                    if (free == 0)
                        continue;
                    totalFree += free;

                    string villageStr = village.ToString().Replace('_', ' ');

                    msg += "\n" + villageStr + " - " + free + " free";
                }

                if (totalFree == 0)
                    msg += "\n\nI'm sorry, no villages have free houses in that zone.";
                else
                    msg += "\n\nThere are currently " + totalFree + " free lots in villages across " + zoneStr + "!";

                SayTo(player, msg);
            }

            return true;
        }

        //http://www.valmerwolf.com/mappe/mappe.htm
        public enum eVillage : int
        {
            None = 0,
            //CAERWENT
            Eccleston = 1, //1-10
            Revington = 2, //11-20
            Hoghton = 3, //21-30
            Worton = 4, //31-40
            Eastby = 5, //41-50
            Devonshire = 6, //51-60
            Bardale = 7, //61-70
            Rylestone = 8, //71-80
            Silverdale = 9, //81-90
            Oackwick = 10, //91-100
            Winterburn = 11, //101-109
            Ingleton = 12, //110-119
            Foxup = 13, //120-129
            Twisleton = 14, //130-139
            Hawkswick = 15, //140-149
            Waterford_North = 16, //150-159
            Waterford_South = 17, //160-169
            Kilnsey = 18, //170-179
            Arncliffe = 19, //180-189
            Gwathrop = 20, //190-199
            Middleham = 21, //200-209
            Semer_Water = 22, //210-219
            Whernside = 23, //220-229
            
            //TODO OTHERS
        }

        public enum eHousingZone : int
        {
            None = 0,
            //ALBION
            Caerwent = 1, //1-200
            Rilan = 2, //201-400
            Dalton = 3, //401-600
            Old_Sarum = 4, //601-800
            //MIDGARD
            Erikstaad = 9, //1601-1800
            Carlingford = 10, //1801-2000
            Wyndham = 11, //2001-2200
            Arothi = 12, //2201-2400
            //HIBERNIA
            Meath = 17, //3201-3400
            Torrylin = 18, //3401-3600
            Kilcullen = 19, //3601-3800
            Dunshire = 20 //3801-4000
        }

        public static eVillage GetHousingVillage(int lot)
        {
            int village = (lot + 10) / 10;
            return (eVillage)village;
        }

        public static eHousingZone GetHousingZone(int lot)
        {
            double rez = ((lot - 1) / 200) + 1;

            int zone = (int)rez;
            return (eHousingZone)zone;
        }

        /// <summary>
        /// gets the amount of free houses for a zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static int GetFreeHousesForZone(eHousingZone zone)
        {
            int free = 0;

            int from = 1 + (((int)zone - 1) * 200);
            int to = from + 199;

            IList<DBHouse> houses = GameServer.Database.SelectObjects<DBHouse>("HouseNumber >= " + from + " AND HouseNumber <= " + to);

            foreach (DBHouse house in houses)
            {
                if (string.IsNullOrEmpty(house.OwnerID))
                    free++;
            }

            return free;
        }

        /// <summary>
        /// Gets the amount of free houses for a village
        /// </summary>
        /// <param name="village"></param>
        /// <returns></returns>
        public static int GetFreeHousesForVillage(eVillage village)
        {
            int free = 0;

            int from = 1 + (((int)village - 1) * 10);
            int to = from + 9;

            IList<DBHouse> houses = GameServer.Database.SelectObjects<DBHouse>("HouseNumber >= " + from + " AND HouseNumber <= " + to);

            foreach (DBHouse house in houses)
            {
                if (string.IsNullOrEmpty(house.OwnerID))
                    free++;
            }

            return free;
        }

        /// <summary>
        /// Get all villages in the zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static List<eVillage> GetVillagesInZone(eHousingZone zone)
        {
            List<eVillage> villages = new List<eVillage>();

            int zoneFrom = 1 + (((int)zone - 1) * 200);
            int zoneTo = zoneFrom + 199;

            GameServer.Instance.Logger.Info("Getting villages for zone " + zone.ToString() + " lots " + zoneFrom + " - " + zoneTo);

            foreach (int villageID in Enum.GetValues(typeof(eVillage)))
            {
                int villageFrom = 1 + ((villageID - 1) * 10);
                int villageTo = villageFrom + 9;

                GameServer.Instance.Logger.Info("Comparing village " + ((eVillage)villageID).ToString() + " lots " + villageFrom + " - " + villageTo);

                if (villageFrom >= zoneFrom && villageTo <= zoneTo)
                    villages.Add((eVillage)villageID);
            }
            return villages;
        }
    }
}