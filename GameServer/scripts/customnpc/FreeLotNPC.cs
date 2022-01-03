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
            //RILAN
            Middleham = 21, //200-210
            Semer_Water = 22, //211-220
            Whernside = 23, //221-230
            Drayworth = 24, //231-240
            Stoneleigh = 25, //241-250
            Eshton = 26, //251-260
            Nappa = 27, //261-270
            Kingsdale = 28, //271-280
            Dibble_Bridge = 29, //281-290
            West_Winton = 30, //291-300
            Glenn_Abbey = 31, //301-310
            Long_Preston = 32, //311-320
            Skipton = 33, //321-330
            Baugh_Fell = 34, //331-340
            Armistead = 35, //341-350
            Hardraw = 36, //351-360
            Reeth = 37, //361-370
            Grassington = 38, //371-380
            Litton = 39, //381-390
            Clayfield = 40, //391-400
            //DALTON
            Helena = 41, //401-410
            Townsend = 42, //411-420
            Jeneva = 43, //421-429
            Pallmyra = 44, //430-438
            Jerrick = 45, //439-448
            Branwyn = 46, //449-458
            Wickam = 47, //459-468
            Devereaux = 48, //469-478
            Gorlash = 49, //479-488
            Wurthen = 50, //489-498
            Stockhart = 51, //499-508
            Mildburgh = 52, //509-518
            Estmund = 53, //519-528
            Geldwine_West = 54, //529-538
            Geldwine_East = 55, //539-548
            Almarick = 56, //549-558
            Norwood = 57, //559-568
            Redmond = 58, //569-578
            Reilly = 59, //579-588
            Elswith = 60, //589-598
            //OLD SARUM
            Flint = 61, //601-610
            Cheshire = 62, //611-620
            Norfolk = 63, //621-630
            Denbigh = 64, //631-640
            Bedford = 65, //641-650
            Kent = 66, //651-660
            Sussex = 67, //661-670
            Carnarvon = 68, //671-680
            Merioneth = 69, //681-690
            Carmarthen = 70, //691-700
            Brecknock = 71, //701-710
            Glamorgan = 72, //711-720
            Pembroke = 73, //721-730
            Anglesey = 74, //731-739
            Worcester = 75, //740-749
            Gloucester = 76, //750-759
            Warwick = 77, //760-769
            Montgomery = 78, //770-779
            Radnor = 79, //780-789
            Dorset = 80, //790-798
            //BRISWORTHY
            Nethermuir = 81, //801-810
            Grampian = 82, //811-820
            Forfarshire = 83, //821-830
            Galloway = 84, //831-840
            Lothian = 85, //841-850
            Dumfries = 86, //851-860
            Shetland = 87, //861-870
            Highland = 88, //871-880
            Stathclyde = 89, //881-890
            Argyll = 90, //891-900
            Tayside = 91, //901-910
            Clackshire = 92, //911-920
            Kincardine = 93, //921-929
            Fife = 94, //930-939
            Arboath = 95, //940-949
            Orkney = 96, //950-959
            Glasgow = 97, //960-969
            Falkirk = 98, //970-978
            Ayrshire = 99, //979-988
            Aberdeen = 100, //989-998
            //AYLESBURY
            Chesham = 101, //1001-1010
            Peacehaven = 102, //1011-1020
            Mawdelsey = 103, //1021-1030
            Andover = 104, //1031-1040
            Cherwell = 105, //1041-1050
            Evesham = 106, //1051-1060
            Redesdale = 107, //1061-1070
            Farnborough = 108, //1071-1080
            Falstone = 109, //1081-1090
            Lymington = 110, //1091-1100
            Drayton = 111, //1101-1109
            Leyland = 112, //1110-1119
            Perthorne = 113, //1120-1129
            Gorleston = 114, //1130-1139
            Crowborough = 115, //1140-1149
            Mayfield = 116, //1150-1159
            Walsham = 117, //1160-1169
            Tandridge = 118, //1170-1179
            Chiltern = 119, //1180-1189
            Amersham = 120, //1190-1199
            //CHILTERN
            Lordling_Wood = 121, //1201-1210
            Brun_Grange = 122, //1211-1220
            Buckland_Woods = 123, //1221-1230
            Hale_Wood = 124, //1231-1240
            Oakengrovers = 125, //1241-1250
            Coombe_Hill = 126, //1251-1260
            Dancersend = 127, //1261-1270
            Boswell = 128, //1271-1280
            Boddington_Hill = 129, //1281-1290
            Haddington_Hill = 130, //1291-1300
            Halton_Camp = 131, //1301-1310
            Aston_Hill = 132, //1311-1320
            Halton = 133, //1321-1330
            Concord = 134, //1331-1339
            Chambers_Green = 135, //1340-1349
            Chivery = 136, //1350-1359
            Longcraft = 137, //1360-1369
            Milesfield = 138, //1370-1379
            Halton_Woods = 139, //1380-1388
            Spencersgreen = 140, //1389-1398
            //


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