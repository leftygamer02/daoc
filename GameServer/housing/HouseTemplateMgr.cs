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

// See SQL at end of this file for inserting required housing menu items - Tolakram

using System;
using System.Collections.Generic;
using System.Linq;
using DOL.Database;
using DOL.GS.ServerProperties;

namespace DOL.GS.Housing
{
    public sealed class HouseTemplateMgr
    {
        private static MerchantTradeItems _albionLotMarkerItems;
        private static MerchantTradeItems _midgardLotMarkerItems;
        private static MerchantTradeItems _hiberniaLotMarkerItems;
        private static MerchantTradeItems _customLotMarkerItems;
        private static MerchantTradeItems _indoorNpcShopItemsAll;
        private static MerchantTradeItems _indoorNpcShopItemsAlb;
        private static MerchantTradeItems _indoorNpcShopItemsHib;
        private static MerchantTradeItems _indoorNpcShopItemsMid;

        public static MerchantTradeItems IndoorBindstoneShopItems { get; private set; }
        public static MerchantTradeItems IndoorBindstoneShopItemsMid { get; private set; }
        public static MerchantTradeItems IndoorBindstoneShopItemsAlb { get; private set; }
        public static MerchantTradeItems IndoorBindstoneShopItemsHib { get; private set; }
        public static MerchantTradeItems IndoorCraftShopItems { get; private set; }
        public static MerchantTradeItems IndoorMenuItems { get; private set; }
        public static MerchantTradeItems IndoorShopItems { get; private set; }
        public static MerchantTradeItems IndoorVaultShopItems { get; private set; }
        public static MerchantTradeItems IndoorVaultShopItemsMid { get; private set; }
        public static MerchantTradeItems IndoorVaultShopItemsAlb { get; private set; }
        public static MerchantTradeItems IndoorVaultShopItemsHib { get; private set; }
        public static MerchantTradeItems OutdoorMenuItems { get; private set; }
        public static MerchantTradeItems OutdoorShopItems { get; private set; }

        public static void Initialize()
        {
            CheckItemTemplates();
            CheckMerchantItemTemplates();
            LoadItemLists();
            CheckNPCTemplates();
        }

        public static long GetLotPrice(DBHouse house)
        {
            var diff = DateTime.Now - house.CreationTime;

            var price = Properties.HOUSING_LOT_PRICE_START -
                        (long) (diff.TotalHours * Properties.HOUSING_LOT_PRICE_PER_HOUR);
            if (price < Properties.HOUSING_LOT_PRICE_MINIMUM) return Properties.HOUSING_LOT_PRICE_MINIMUM;

            return price;
        }

        public static MerchantTradeItems GetLotMarkerItems(GameLotMarker marker)
        {
            witch(GameServer.ServerRules.GetLotMarkerListName(marker.CurrentRegionID).ToLower())

            ase "housing_alb_lotmarker":
            eturn _albionLotMarkerItems;
            ase "housing_mid_lotmarker":
            eturn _midgardLotMarkerItems;
            ase "housing_hib_lotmarker":
            eturn _hiberniaLotMarkerItems;
            ase "housing_custom_lotmarker":
            eturn _customLotMarkerItems;


            eturn new MerchantTradeItems(GameServer.ServerRules.GetLotMarkerListName(marker.CurrentRegionID));
        }

        public static MerchantTradeItems GetNpcShopItems(GamePlayer player)
        {
            r allRealmsTypes = new[] {eameServerType.GST_PvE, eGameServerType.GST_PvP}
                (allRealmsTypes.Contains(GameServer.Instance.Configuration.ServerType))
            turn _indoorNpcShopItemsAll;

            itch(player.Realm)
            se eRealm.Albion:
            turn _indoorNpcShopItemsAlb;
            se eRealm.Hibernia:
            turn _indoorNpcShopItemsHib;
            se eRealm.Midgard:
            turn _indoorNpcShopItemsMid;

            turn _indoorNpcShopItemsAll;
        }

        private static void LoadItemLists()
        {
            ndoorMenuItems = new MerchantTradeItems("housing_indoor_menu");
            ndoorShopItems = new MerchantTradeItems("housing_indoor_shop");
            utdoorMenuItems = new MerchantTradeItems("housing_outdoor_menu");
            utdoorShopItems = new MerchantTradeItems("housing_outdoor_shop");
            ndoorVaultShopItems = new MerchantTradeItems("housing_indoor_vault");
            ndoorVaultShopItemsMid = new MerchantTradeItems("housing_indoor_vault_mid");
            ndoorVaultShopItemsAlb = new MerchantTradeItems("housing_indoor_vault_alb");
            ndoorVaultShopItemsHib = new MerchantTradeItems("housing_indoor_vault_hib");
            ndoorCraftShopItems = new MerchantTradeItems("housing_indoor_craft");
            ndoorBindstoneShopItems = new MerchantTradeItems("housing_indoor_bindstone");
            ndoorBindstoneShopItemsMid = new MerchantTradeItems("housing_indoor_bindstone_mid");
            ndoorBindstoneShopItemsAlb = new MerchantTradeItems("housing_indoor_bindstone_alb");
            ndoorBindstoneShopItemsHib = new MerchantTradeItems("housing_indoor_bindstone_hib");
            indoorNpcShopItemsAll = new MerchantTradeItems("housing_indoor_npc");
            indoorNpcShopItemsAlb = new MerchantTradeItems("housing_indoor_alb_npc");
            indoorNpcShopItemsHib = new MerchantTradeItems("housing_indoor_hib_npc");
            indoorNpcShopItemsMid = new MerchantTradeItems("housing_indoor_mid_npc");
            albionLotMarkerItems = new MerchantTradeItems("housing_alb_lotmarker");
            midgardLotMarkerItems = new MerchantTradeItems("housing_mid_lotmarker");
            hiberniaLotMarkerItems = new MerchantTradeItems("housing_hib_lotmarker");
            customLotMarkerItems = new MerchantTradeItems("housing_custom_lotmarker");
        }

        private static void CheckItemTemplates()
        {
            (!Properties.LOAD_HOUSING_ITEMS)
            turn;

            lot marker
            eckItemTemplate("Albion cottage deed", "housing_alb_cottage_deed", 498, 0, 10000000, 0, 0, 0, 0, 1);
            eckItemTemplate("Albion house deed", "housing_alb_house_deed", 498, 0, 50000000, 0, 0, 0, 0, 1);
            eckItemTemplate("Albion villa deed", "housing_alb_villa_deed", 498, 0, 100000000, 0, 0, 0, 0, 1);
            eckItemTemplate("Albion mansion deed", "housing_alb_mansion_deed", 498, 0, 250000000, 0, 0, 0, 0, 1);
            eckItemTemplate("Midgard cottage deed", "housing_mid_cottage_deed", 498, 0, 10000000, 0, 0, 0, 0, 2);
            eckItemTemplate("Midgard house deed", "housing_mid_house_deed", 498, 0, 50000000, 0, 0, 0, 0, 2);
            eckItemTemplate("Midgard villa deed", "housing_mid_villa_deed", 498, 0, 100000000, 0, 0, 0, 0, 2);
            eckItemTemplate("Midgard mansion deed", "housing_mid_mansion_deed", 498, 0, 250000000, 0, 0, 0, 0, 2);
            eckItemTemplate("Hibernia cottage deed", "housing_hib_cottage_deed", 498, 0, 10000000, 0, 0, 0, 0, 3);
            eckItemTemplate("Hibernia house deed", "housing_hib_house_deed", 498, 0, 50000000, 0, 0, 0, 0, 3);
            eckItemTemplate("Hibernia villa deed", "housing_hib_villa_deed", 498, 0, 100000000, 0, 0, 0, 0, 3);
            eckItemTemplate("Hibernia mansion deed", "housing_hib_mansion_deed", 498, 0, 250000000, 0, 0, 0, 0, 3);
            eckItemTemplate("Porch deed", "housing_porch_deed", 498, 0, 5000000, 0, 0, 0, 0, 0);
            eckItemTemplate("Porch remove deed", "housing_porch_remove_deed", 498, 0, 500000, 0, 0, 0, 0, 0);
            eckItemTemplate("Consignment Merchant", "housing_consignment_deed", 593, 0, 1000000, 0, 0, 0, 0, 0);
            eckItemTemplate("deed of guild transfer", "housing_deed_of_guild_transfer", 498, 0, 500000, 0, 0, 0, 0, 0);
            eckItemTemplate("House removal deed", "housing_house_removal_deed", 498, 0, 50000000, 0, 0, 0, 0, 0);

            default indoor npc
            eckItemTemplate("Hastener", "housing_hastener", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID, 0, 0);
            eckItemTemplate("Smith", "housing_smith", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 1, 0, 0);
            eckItemTemplate("Enchanter", "housing_enchanter", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 2, 0, 0);
            eckItemTemplate("Emblemer", "housing_emblemer", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 3, 0, 0);
            eckItemTemplate("Healer", "housing_healer", 593, (int) eO bjectType.HouseNPC, 30000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 4, 0, 0);
            eckItemTemplate("Recharger", "housing_recharger", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 5, 0, 0);
            eckItemTemplate("Hibernia Teleporter", "housing_hib_teleporter", 593, (int) eO bjectType.HouseNPC, 1000000,
                0, 0
            Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 6, 0, 3);
            eckItemTemplate("Albion Teleporter", "housing_alb_teleporter", 593, (int) eO bjectType.HouseNPC, 1000000, 0,
                0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 6, 0, 1);
            eckItemTemplate("Midgard Teleporter", "housing_mid_teleporter", 593, (int) eO bjectType.HouseNPC, 1000000,
                0, 0
            Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 6, 0, 2);
            eckItemTemplate("Apprentice Merchant", "housing_apprentice_merchant", 593, (int) eO bjectType.HouseNPC, 1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 7, 0, 0);
            eckItemTemplate("Grandmaster Merchant", "housing_grandmaster_merchant", 593, (int) eO bjectType.HouseNPC, 5
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 8, 0, 0);
            eckItemTemplate("Incantation Merchant", "housing_incantation_merchant", 593, (int) eO bjectType.HouseNPC, 1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 9, 0, 0);
            eckItemTemplate("Poison and Dye Supplies", "housing_poison_dye_supplies", 593, (int) eO bjectType.HouseNPC,
                1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 10, 0, 0);
            eckItemTemplate("Potion, Tincture, and Enchantment Supplies",
                "housing_potion_tincture_enchantment_supplies", 5
            3, (int) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 11, 0, 0);
            eckItemTemplate("Poison and Potion Supplies", "housing_poison_potion_supplies", 593, (
                nt) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 12, 0, 0);
            eckItemTemplate("Dye, Tincture, and Enchantment Supplies", "housing_dye_tincture_enchantment_supplies", 593,
                (
                    nt) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 13, 0, 0);
            eckItemTemplate("Taxidermy Supplies", "housing_taxidermy_supplies", 593, (int) eO bjectType.HouseNPC,
                1000000, 0
            0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 14, 0, 0);
            eckItemTemplate("Siegecraft Supplies", "housing_siegecraft_supplies", 593, (int) eO bjectType.HouseNPC, 1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 15, 0, 0);
            eckItemTemplate("Vault Keeper", "housing_vault_keeper", 593, (int) eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 16, 0, 0);
            eckItemTemplate("Dye Supply Master", "housing_dye_supply_master", 593, (int) eO bjectType.HouseNPC, 1000000,
                0, 0
            Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 17, 0, 0);
            eckItemTemplate("Grandmaster Merchant", "housing_mid_grandmaster_merchant", 593,
                (int) eO bjectType.HouseNPC, 5
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 18, 0, 0);
            eckItemTemplate("Grandmaster Merchant", "housing_alb_grandmaster_merchant", 593,
                (int) eO bjectType.HouseNPC, 5
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 19, 0, 0);
            eckItemTemplate("Grandmaster Merchant", "housing_hib_grandmaster_merchant", 593,
                (int) eO bjectType.HouseNPC, 5
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 20, 0, 0);
            eckItemTemplate("Potion, Tincture, and Enchantment Supplies", "
            ousing_mid_potion_tincture_enchantment_supplies", 593, (int)eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 30, 0, 0);
            eckItemTemplate("Potion, Tincture, and Enchantment Supplies", "
            ousing_alb_potion_tincture_enchantment_supplies", 593, (int)eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 31, 0, 0);
            eckItemTemplate("Potion, Tincture, and Enchantment Supplies", "
            ousing_hib_potion_tincture_enchantment_supplies", 593, (int)eO bjectType.HouseNPC, 1000000, 0, 0, P
            operties.HOUSING_STARTING_NPCTEMPLATE_ID + 32, 0, 0);
            eckItemTemplate("Apprentice Merchant", "housing_mid_apprentice_merchant", 593, (int) eO bjectType.HouseNPC,
                1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 36, 0, 0);
            eckItemTemplate("Apprentice Merchant", "housing_alb_apprentice_merchant", 593, (int) eO bjectType.HouseNPC,
                1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 37, 0, 0);
            eckItemTemplate("Apprentice Merchant", "housing_hib_apprentice_merchant", 593, (int) eO bjectType.HouseNPC,
                1
            00000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 38, 0, 0);
            eckItemTemplate("Dye, Tincture, and Enchantment Supplies", "housing_mid_dye_tincture_enchantment_supplies",
                5
            3, (int) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 39, 0, 0);
            eckItemTemplate("Dye, Tincture, and Enchantment Supplies", "housing_alb_dye_tincture_enchantment_supplies",
                5
            3, (int) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 40, 0, 0);
            eckItemTemplate("Dye, Tincture, and Enchantment Supplies", "housing_hib_dye_tincture_enchantment_supplies",
                5
            3, (int) eO bjectType.HouseNPC, 1000000, 0, 0, Properties.HOUSING_STARTING_NPCTEMPLATE_ID + 41, 0, 0);

            indoor craft
            eckItemTemplate("alchemy table", "housing_alchemy_table", 1494, (int) eO bjectType.HouseInteriorObject, 1
            000000, 0, 0, 0, 0, 0);
            eckItemTemplate("forge", "housing_forge", 1495, (int) eO bjectType.HouseInteriorObject, 10000000, 0, 0, 0,
                    0, 0
                ;
            eckItemTemplate("lathe", "housing_lathe", 1496, (int) eO bjectType.HouseInteriorObject, 10000000, 0, 0, 0,
                    0, 0
                ;

            indoor bindstone
            eckItemTemplate("Albion bindstone", "housing_alb_bindstone", 1488, (int) eO bjectType.HouseBindstone,
                10000000, 0
            0, 0, 0, 1);
            eckItemTemplate("Midgard bindstone", "housing_mid_bindstone", 1492, (int) eO bjectType.HouseBindstone, 1
            000000, 0, 0, 0, 0, 2);
            eckItemTemplate("Hibernia bindstone", "housing_hib_bindstone", 1490, (int) eO bjectType.HouseBindstone, 1
            000000, 0, 0, 0, 0, 3);

            indoor vault
            eckItemTemplate("Albion vault", "housing_alb_vault", 1489, (int) eO bjectType.HouseVault, 10000000, 0, 0, 0,
                    0, 1
                ;
            eckItemTemplate("Midgard vault", "housing_mid_vault", 1493, (int) eO bjectType.HouseVault, 10000000, 0, 0,
                0, 0
            2);
            eckItemTemplate("Hibernia vault", "housing_hib_vault", 1491, (int) eO bjectType.HouseVault, 10000000, 0, 0,
                0, 0
            3);
        }

        private static void CheckMerchantItemTemplates()
        {
            if (!Properties.LOA
            D
            _HOUSING_ITE

            {
                return;

                kers

                ing keritems =
                {
                    se_deed", "housing_alb_vil
                    l
                    _deed", "housing_alb_man
                    s
                    on_deed",
                    deed", "housing_cons
                    i
                    nment_deed",//"housing_deed
                    _
                    f_guild_transfer"
                };
                l
                    b_ alblotmarkeritems)
                ;


                string[]
                m
                    dlotmarkeritems =

                    {
                        se_deed", "housing_mid_vil
                        l
                        _deed", "housing_mid_man
                        s
                        on_deed",
                        deed", "housing_cons
                        i
                        nment_deed",//"housing_deed
                        _
                        f_guild_transfer"
                    };
                i
                    d_ midlotmarkeritems)
                ;


                string[]
                h
                    blotmarkeritems =

                    {
                        se_deed", "housing_hib_vil
                        l
                        _deed", "housing_hib_man
                        s
                        on_deed",
                        deed", "housing_cons
                        i
                        nment_deed",//"housing_deed
                        _
                        f_guild_transfer"
                    };
                i
                    b_ hiblotmarkeritems)
                ;


                string[]
                    c
                stomlotmarkeritem
                    s


                se_deed", "housing_alb_vil
                    l
                _deed", "housing_alb_man
                    s
                on_deed",


                se_deed", "housing_hib_vil
                    l
                _deed", "housing_hib_man
                    s
                on_deed",


                se_deed", "housing_mid_vil
                    l
                _deed", "housing_mid_man
                    s
                on_deed",


                deed", "housing_cons
                    i
                nment_deed", //"housing_dee
                    d
                of_guild_transfer"
            }
            ;
            u
                st ", customlotmarker
            i
                tems);

            //hook
            p
                ints
            var
                ind new List<str
            "housi
            g
                has
            ener
            "
                , "hou
            s
                ng g_enchanter", "hou
                s
            ng_emblemer", "
            h
                using_healer", "hou
                s
            ng_recharger", "ho
                u
            ing_apprentice_m
                e
            chant",


            "housing_grandmaster_merch
            an antation_merchant", "housing_p
                o
            son_dye_supplies", "housing_po
                t
            on_tincture_enchantment_suppl
                i
            s",
            "housing_poison_potion_sup
            pl axidermy_supplies", "housing_sie
                g
            craft_supplies", "housing_va
                u
            t_keeper",
            "h
            o
                sing_dye_supply_master
            "
            //hookpoints
            i
            nd d = new List<
            "housi
            g
                has
            ener
            "
                , "hou
            s
                ng g_enchanter", "hou
                s
            ng_emblemer", "
            h
                using_healer", "hou
                s
            ng_recharger", "ho
                u
            ing_mid_apprenti
                c
            _merchant",


            "housing_mid_grandmaster_m
            er _incantation_merchant", "housing_p
                o
            son_dye_supplies", "housing_mi
                d
            potion_tincture_enchantment_s
                u
            plies",
            "housing_poison_potion_sup
            pl axidermy_supplies", "housing_sie
                g
            craft_supplies", "housing_va
                u
            t_keeper",
            "h
            o
                sing_dye_supply_master
            ",        e_tincture_enchantment_supp
            l
                es"
        };

        //hookpoints
        private i
            nd b = new List<
        "housi

        private g
            has

        ener
        "
        , "hou

        private s
            ng g_enchanter", "

        private hou
            s

        ng_emblemer", "

        private h
            using_healer", "

        private hou
            s

        ng_recharger", "

        private ho
            u

        private ing_alb_apprenti
            c

        _merchant",
        "housing_alb_grandmaster_m
        private er _incantation_merchant", "

        private housing_p
            o

        son_dye_supplies", "

        private housing_al
            b

        private potion_tincture_enchantment_s
            u

        plies",
        "housing_poison_potion_sup
        private pl axidermy_supplies", "

        private housing_sie
            g

        craft_supplies", "

        private housing_va
            u

        t_keeper",
        "h

        private o
            sing_dye_supply_master

        ",        e_tincture_enchantment_supp

        private l
            es"
    };

    //hookpoints
    i
    nd b = new List <
        "housi
    g
        has
    ener
    "
        , "hou
    s
        ng g_enchanter", "hou
        s
    ng_emblemer", "
    h
        using_healer", "hou
        s
    ng_recharger", "ho
        u
    ing_hib_apprenti
        c
    _merchant",


    "housing_hib_grandmaster_m
    er _incantation_merchant", "housing_p
        o
    son_dye_supplies", "housing_hi
        b
    potion_tincture_enchantment_s
        u
    plies",
    "housing_poison_potion_sup
    pl axidermy_supplies", "housing_sie
        g
    craft_supplies", "housing_va
        u
    t_keeper",
    "h
    o
        sing_dye_supply_master
    ",        e_tincture_enchantment_supp
    l
        es"
};

var indoorNpc = i
n > se)
    in
oorN
    p
c.AddR
    a
n
ge(new[]
        {
            "h
            o
            us porter", 
            "
            housing_
            a
            lb_
            t
            e
            e
            orter", "housing_mid_tel
            e
            orter" });
            C
            h
            ckMerchantItems("housing
            i
            n
            do oorNpc);
            var indoorNpcAlb
            =
            new List<
            s
            ti cBa
            eAlb);
            indo
            o
            rNpcAl
            b
            .
            AddRange(new[] {
            "h        eleporter"
        }
    )
    ;


h
ckMerchantItems("housing
i
    n
do
{
    indoorNpcAlb)
};


var indoorNpcH
    i
        = new List<
            s
ti cBa
eHib);


indo
    o
rNpcHi
b
    .AddRange(new[]
        {
            "h        eleporter"
        }
    )
    ;


h
ckMerchantItems("housing
i
    n
do
{
    indoorNpcHib)
};


var indoorNpcM
    i
        = new List<
            s
ti cBa
eMid);


indo
    o
rNpcMi
d
    .AddRange(new[]
        {
            "h        eleporter"
        }
    )
    ;


h
ckMerchantItems("housing
i
    n
do
{
    indoorNpcMid)
};


string[] indoo
r
    indstone =
    {
        "o        ndston
        e
        "
        "housing_mid_b
        n
        s
        one", "housing_alb_bind
        s
        one" };
        Che
        c
        MerchantItems("housing_
        n
        do ", indoorbindstone
        )
        ;
        tring[] indoorb
        i
        n
        _binds
        t
        o
        e" };
        h
        c
        MerchantItems("housing_
        n
        do _mid", indoorbinds
        t
        onemid);
        string[] indoorb
        i
        n
        _binds
        t
        o
        e" };
        h
        c
        MerchantItems("housing_
        n
        do _alb", indoorbinds
        t
        onealb);
        string[] indoorb
        i
        n
        _binds
        t
        o
        e" };
        h
        c
        MerchantItems("housing_
        n
        do _hib", indoorbinds
        t
        onehib);
        string[]
        ndoorcraft = { "ho
        u
        sn ble", 
        "
        h
        using_forge
        ,
        "
        ousing_lathe" };
        CheckMercha
        n
        Items("housing_
        n
        do ndoorcraft);
        string[] indoor
        v
        ult = { "ho
        u
        sn, "hou
        s
        i
        g_mid_vault
        ,
        "
        ousing_alb_vault" }
        ;
        CheckMe
        r
        hantItems("housing_
        n
        do ndoorvault);
        string[] indoorv
        a
        ltmid = { "
        h
        ou lt" };
        Chec
        M
        r
        hantItems("housing_
        n
        do ", indoorvaultmid)
        ;
        string[] indo
        o
        vaultalb = { "
        h
        ou lt" };
        Chec
        M
        r
        hantItems("housing_
        n
        do ", indoorvaultalb)
        ;
        string[] indo
        o
        vaulthib = { "
        h
        ou lt" };
        Chec
        M
        r
        hantItems("housing_
        n
        do ", indoorvaulthib)
        ;
    }

private static void CheckMerchantItems(string merchantid, ICollection<string> itemids)
{
    var mer
    chan
        ercha tI em.SelectOb
        j
    ects(DB.Co um n("ItemListID"
    ).IsEqualTo(merchan td i
    n
    t
        slot = ;


    f or each(string ite
    mid


    if (me ch antitems.Any(x
        => x.ItemTemplateI D = 


    {
        slot += 1;


        ntinu;
    }

    var ne
        witem = new Merchan ttem
    {
        ItemListID = mer
        c
            hantid


        It
            e
        mTempl
        a
            eID = i
        temid,

        lotPosition =
            lot %
            30,


        Pa
            g
        eN
        u
            mber =
                slot / 30
    }
    ;


    eSe
    ver.
        a
        a
    ba(newite
        )
        ;


    slot +
        = 
}
}

private static void CheckItemTemplate(string name, string id, int model, int objtype, int copper, int dps,
    int spd,
    int bonus, int weight, int realm)
{
    e
        FindObject
    y
    ey < I te mTempl
        (te mp

    lateite m = n
    l
        )


    return;


    It em Tem pl ate
        a
    m
        del = m del
        ,


    Leve
        l
            = 0,
        pe =
            ob j
        y
    pe Id_nb = id,
        I
    Pick
        ble = true,
        I
    s
        ropabl
            =
            rue


    D
    P
        _AF
            = dps,
        S
    PD


    Wei
        ht = w
    e
    ght
        er,
        Bonus = b
    o
        nus,
        Rea
    l
        m = (byte) re
    a
    l
        m,
        a
    ckageID = "P
    ay
        rHou
    si Game
    Se eA plateitem);
}

private static void CheckNPCTemplates()


i(!P op er
ti NPC)


re urn
    / / hese ar d
    lt
kN PC empl t
(Pr
    o

G_ NPCTEM
L
    TE_
ID sten
    r,  "P
i
", " ,
"
    ;

te(Pr
p
rties.
    H
    EMPLA
E
ID + 1,
    , "B        la        c
s
    i
th",

"Sm        it        h" ,

Chec
    N
CTe pla e(Pro pert
ie A
RI TE_ID + 2,
"
DOL.GS.E
    n
chanter",

"Enchanter",

"
"", "", "");


Chec
    NPCTemplate(Prope
        r

ties.HOUSING CT
M
    P
LATE_ID +
    3
    , "DOL.GS.Emblem
NP mb r", "0
",                   NPCTemplate(Properties.HOUS        _NPCTEMPLATE_ID 
    +
    4, "DOL.G
S
    .GameHealer", "Healer", "Healer
"
"0", "", "");


Che
    c
NPCTemplat
    e
Pro
    p
rt
    i
s.
    H
    OU G_NPCTEMPLATE_ID
    + 5, "DOL.
G
S.Recharger", "Recharger", "Rec
    a
g
    e
", "0", "", "");


Chec
    k
PCTempl
    a
e(P
r
    pe
r
    ie
s
    .H ING_NPCTEMPLATE_
I
D + 6, "DO
L
    .GS.GameNPC", "Realm Teleporter, 
"
T
    leporter", "0", ""
    ,
"");


CheckNP
    C
emp
    l
te
(
    ro
p
    er _STARTING_NPCTEM
    P
LATE_ID +
    7
    , "DOL.GS.GameMerchant", "Appre
t
    c
e
    Merchant", "Mercha
    n
", "0", "", "hou
    s
ng_apprent
    i
e")
    ;


emplate(Properti
e
s.HOUSING_
    S
TARTING_NPCTEMPLATE_ID + 8, "DO
    .S
    .ameMerchant", "Gran
    d
aster Me
r
    hant", "
M
    rch
a
    t"
    ,
"0
"
    , _grandmaster");


CheckNPCTemplate(Properties.HOU
I
    G
_
    TARTING_NPCTEMPLAT
E
ID + 9, "DO
L
GS.GameMerc
    h
nt"
    ,
"I
n
    an
t
    at ", "Merchant", "
0
", "", "ho
    u
sing_incantation");

C
    e

c
    NPCTemplate(Prop
        e

ties.HOUSING_START
    I
G_NPCTEMPLAT
    E
ID
    +
    10
    ,
"D
O
L.ant", "Bane Merc
h
    ant", "Mer
    c
hant", "0", "", "housing_poison
    d
e
"
    ;
CheckNP
    C
emplate(Properties.HO
U
    ING_STARTI
N
    _NP
C
    EM
P
ATE_ID + 11, "DOL.GS
    .Ga "Potion, Tinctu
r
    e, and Enc
    h
antment Supplies", "Merchant", 
0
    ,

", "housing_potion_ti
    n
ture_enchantment");


Ch
    e
kNP
    C
em
    p
ate(Properties.HOUSIN
G
    _S EMPLATE_ID + 12,

"DOL.GS.Ga
m
    eMerchant", "Poison and Potion
    u
p
    l
es", "Merchant", "0",

", "housing_poison_pot
    i
n");


Ch
    e
kNPCTemplate(Properti
e
s.TING_NPCTEMPLATE
    _
ID + 13, "
D
OL.GS.GameMerchant", "Dye, Tinc
u
    e,
    nd Enchantment Suppli
e
", "Merchant", 
"
", "", "ho
    u
ing
    _
ye
    _
incture_enchantment"
    )
;
CheckNPCTemplate
(
    Properties
        .HOUSING_STARTING_NPCTEMPLATE_ID
    +
    14
    ,
    "DOL.GS.GameMerchant"
    ,
    "Taxidermy Supplies", "Merchant", "0", "", "
h
    using_taxi
d
    rmy
"
    ;


CheckNPCTemplate(Properties
    .HO NG_NPCTEMPLATE_I
D
    + 15, "DO
L
    .GS.GameMerchant", "Siegecraft
    u
pl
    i
s", "Merchant", "0", 
"
    , "housing_siegecraft");


Che
    c
NPC
    T
mp
    l
te(Properties.HOUSING_S
T
    AR LATE_ID + 16, "D
O
L.GS.GameV
    a
ultKeeper", "Vault Keeper", "Va
    l
K
    e
per", "0", "", "");


CheckNPCTemplate(Properties.HOUS
I
    G_STARTING
_
    PCT
E
    PL
A
E_ID + 17, "DOL.GS.GameMerchant",
"
Dy ter", "Merchant"
    ,
"0", "",
"
housing_dye");
Chec
    N
CT
    e
plate(Properties.HOUS
I
    G_STARTING_NPCTEMPLA
T
_ID + 18,
"
OL.
    G
    .G
    a
eMerchant", "Grandm
a
    st,  "Merchant", "0
"
    , "", "hou
s
    ing_mid_grandmaster");


C
    h
ckNPCTemplate(Propert
i
s.HOUSING_STARTING_NP
    C
EMPLATE_ID

19
    ,
"D
O
    .GS.GameMerchant", "
G
    ra chant", "Merchan
    t
", "0", ""
    ,
"housing_alb_grandmaster");


CheckNPCTemplate(Prop
e
ties.HOUSING_S
    T
RTING_NPCTEMPL
    A
E_I
D
    +
    2
    ,
"
DO chant", "Grandma
    s
ter Mercha
n
    t", "Merchant", "0", "", "housi
    g
hi
    b
grandmaster");


CheckNPCTemplat
    e
Properties
    .
    OUS
    I
G_
    S
ARTING_NPCTEM
    P
LA "DOL.GS.GameMerc
h
    ant", "Pot
i
    on, Tincture, and Enchantment S
p
    li
e
", "Merchant", "0", "
"
"housing_mid_potion_t
i
    cture_ench
a
    tme
n
")
    ;
CheckNPCTempl
    a
te HOUSING_STARTING
_
    NPCTEMPLAT
E
_ID + 31, "DOL.GS.GameMerchant"

Po
t
    on, Tincture, and Enc
    h
ntment Supplies", "Mer
    c
ant", "0",

", 
"
ou
    s
ng_alb_potion_tincture_en
    c
ha CheckN

P
    CTemplate(
        P
            roperties.

HOUSING_STARTING_NPCT
    M
LA
    T
_ID + 32, "DOL.GS.Gam
e
    erchant", "Potion, Tin
c
    ure, and E
    n
han
    t
en
    t
Supplies", "Merchant", "0
"
    , _hib_potion_tinc
    t
ure_enchan
    t
ment");
CheckNPCTem
    l
te
(
    roperties.HOUSING_STA
R
ING_NPCTEMPLATE_ID + 36, "DOL.GS.GameMerchan
t
    , "Apprent
i
    e M
    e
ch
    a
t", "Merchant", "0", "", "housing_mid_app
    r
en CheckNPC

T
    emplate(Pr
        o

perties.HOUSING_STARTING_NPCTEM
    L
TE
    _
D + 37, "DOL.GS.GameM
e
    chant", "Apprentice Merchant", "Merchant", "
0
    , "", "hou
s
    ng_
a
    b_
a
    prentice");
CheckNPCTemplate(
    P
ro ING_STARTING_NPC
T
EMPLATE_ID
    + 38, "DOL.GS.GameMerchant", "A
p
    en
t
    ce Merchant", "Mercha
    n
", "0", "", "housing_hib_apprentice");


Check
    N
CTe
    m

la
    t
    (Properties.HOUSING_STARTING_NPCTEMPLATE_
        I

D S.GameMerchant",

"Dye, Tinc
t
    ure, and Enchantment Supplies",
"
er
    c
ant", "0", "", "housi
    n
_mid_dye_tincture_enc
    h
ntment");


CheckNPCTemplate(Proper
t
    ie ARTING_NPCTEMPLA
    T
E_ID + 40,

"DOL.GS.GameMerchant", "Dye, Ti
c
    ur
e
    and Enchantment Supp
l
    es", "Merchant", "0",

", "housin
    g
alb
    _
ye
    _
incture_enchantment");


kNPCTemplate(Pro
p
erties.HOU
    S
ING_STARTING_NPCTEMPLATE_ID + 4
    ,
"D
O
    .GS.GameMerchant", "D
    y, Tincture, and Encha
    n
ment Suppl
i
    s",

Me
    r
hant", "0", "", "housing
    _
hi re_enchantment")
    ;

}

private static void CheckNPCTemplate(int templateID, string classType, string name, string guild, string model,
    string inventory, string merchantListID)
{
    Npc plate = NpcTempl
    at eM gr.GetTem
        p
    late(templateID);
    if
    m
        la
    t
        != null)


    return;


    DBNpcTemplate dbTempl
    ate
    plate
    {
        Name = name,
        T e
        p
            at
        e
            d = templateID,
            Clas sT ype

        cl
            ssType,
            Gu ldN am =
            guil
        d
            ,


        Model = model,

        Si
            Level =
                "
        50",


        ItemsListTemplateID = mer
        h
            nt
        L
            ID,
            E
        q
            ipmentTem la eID = nve
        tory,


        Pack
            ageID =
                "Hou
        s
            in
    }
    ;
    template = new NpcT
    emp
        );
    t
        e
    mpla te.SaveI
        n
    toDa ta base();
    NpcTemp
        a
    eM r.ddT em l
        at
    e
        (template);
}
}
}

/*

 * INDOOR MENU ITEMS

INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu0', 'housing_indoor_menu0', 'royal blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 53, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu1', 'housing_indoor_menu1', 'dark blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 54, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu10', 'housing_indoor_menu10', 'black carpet', 0, NULL, NULL, NULL, NULL, NULL, 74, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu11', 'housing_indoor_menu11', 'royal orange carpet', 0, NULL, NULL, NULL, NULL, NULL, 77, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu12', 'housing_indoor_menu12', 'royal yellow carpet', 0, NULL, NULL, NULL, NULL, NULL, 83, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu13', 'housing_indoor_menu13', 'royal blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 53, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu14', 'housing_indoor_menu14', 'dark blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 54, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu15', 'housing_indoor_menu15', 'royal turquoise carpet', 0, NULL, NULL, NULL, NULL, NULL, 57, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu16', 'housing_indoor_menu16', 'royal teal carpet', 0, NULL, NULL, NULL, NULL, NULL, 60, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu17', 'housing_indoor_menu17', 'royal red carpet', 0, NULL, NULL, NULL, NULL, NULL, 66, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu18', 'housing_indoor_menu18', 'violet carpet', 0, NULL, NULL, NULL, NULL, NULL, 84, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu19', 'housing_indoor_menu19', 'green carpet', 0, NULL, NULL, NULL, NULL, NULL, 69, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu2', 'housing_indoor_menu2', 'royal turquoise carpet', 0, NULL, NULL, NULL, NULL, NULL, 57, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu20', 'housing_indoor_menu20', 'royal green carpet', 0, NULL, NULL, NULL, NULL, NULL, 70, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu21', 'housing_indoor_menu21', 'brown carpet', 0, NULL, NULL, NULL, NULL, NULL, 62, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu22', 'housing_indoor_menu22', 'dark gray carpet', 0, NULL, NULL, NULL, NULL, NULL, 72, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu23', 'housing_indoor_menu23', 'black carpet', 0, NULL, NULL, NULL, NULL, NULL, 74, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu24', 'housing_indoor_menu24', 'royal orange carpet', 0, NULL, NULL, NULL, NULL, NULL, 77, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu25', 'housing_indoor_menu25', 'royal yellow carpet', 0, NULL, NULL, NULL, NULL, NULL, 83, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu26', 'housing_indoor_menu26', 'carpet removal service', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu27', 'housing_indoor_menu27', 'carpet removal service', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 69, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu28', 'housing_indoor_menu28', 'royal blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 53, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu29', 'housing_indoor_menu29', 'dark blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 54, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu3', 'housing_indoor_menu3', 'royal teal carpet', 0, NULL, NULL, NULL, NULL, NULL, 60, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu30', 'housing_indoor_menu30', 'royal turquoise carpet', 0, NULL, NULL, NULL, NULL, NULL, 57, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu31', 'housing_indoor_menu31', 'royal teal carpet', 0, NULL, NULL, NULL, NULL, NULL, 60, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu32', 'housing_indoor_menu32', 'royal red carpet', 0, NULL, NULL, NULL, NULL, NULL, 66, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu33', 'housing_indoor_menu33', 'violet carpet', 0, NULL, NULL, NULL, NULL, NULL, 84, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu34', 'housing_indoor_menu34', 'green carpet', 0, NULL, NULL, NULL, NULL, NULL, 69, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu35', 'housing_indoor_menu35', 'royal green carpet', 0, NULL, NULL, NULL, NULL, NULL, 70, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu36', 'housing_indoor_menu36', 'brown carpet', 0, NULL, NULL, NULL, NULL, NULL, 62, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu37', 'housing_indoor_menu37', 'dark gray carpet', 0, NULL, NULL, NULL, NULL, NULL, 72, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu38', 'housing_indoor_menu38', 'black carpet', 0, NULL, NULL, NULL, NULL, NULL, 74, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu39', 'housing_indoor_menu39', 'royal orange carpet', 0, NULL, NULL, NULL, NULL, NULL, 77, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu4', 'housing_indoor_menu4', 'royal red carpet', 0, NULL, NULL, NULL, NULL, NULL, 66, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu40', 'housing_indoor_menu40', 'royal yellow carpet', 0, NULL, NULL, NULL, NULL, NULL, 83, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu41', 'housing_indoor_menu41', 'royal blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 53, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu42', 'housing_indoor_menu42', 'dark blue carpet', 0, NULL, NULL, NULL, NULL, NULL, 54, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu43', 'housing_indoor_menu43', 'royal turquoise carpet', 0, NULL, NULL, NULL, NULL, NULL, 57, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu44', 'housing_indoor_menu44', 'royal teal carpet', 0, NULL, NULL, NULL, NULL, NULL, 60, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu45', 'housing_indoor_menu45', 'royal red carpet', 0, NULL, NULL, NULL, NULL, NULL, 66, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu46', 'housing_indoor_menu46', 'violet carpet', 0, NULL, NULL, NULL, NULL, NULL, 84, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu47', 'housing_indoor_menu47', 'green carpet', 0, NULL, NULL, NULL, NULL, NULL, 69, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu48', 'housing_indoor_menu48', 'royal green carpet', 0, NULL, NULL, NULL, NULL, NULL, 70, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu49', 'housing_indoor_menu49', 'brown carpet', 0, NULL, NULL, NULL, NULL, NULL, 62, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu5', 'housing_indoor_menu5', 'violet carpet', 0, NULL, NULL, NULL, NULL, NULL, 84, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu50', 'housing_indoor_menu50', 'dark gray carpet', 0, NULL, NULL, NULL, NULL, NULL, 72, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu51', 'housing_indoor_menu51', 'black carpet', 0, NULL, NULL, NULL, NULL, NULL, 74, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu52', 'housing_indoor_menu52', 'royal orange carpet', 0, NULL, NULL, NULL, NULL, NULL, 77, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu53', 'housing_indoor_menu53', 'royal yellow carpet', 0, NULL, NULL, NULL, NULL, NULL, 83, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu54', 'housing_indoor_menu54', 'carpet removal service', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 70, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu55', 'housing_indoor_menu55', 'carpet removal service', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu56', 'housing_indoor_menu56', 'carpet removal service', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 71, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu57', 'housing_indoor_menu57', 'interior guild banner', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 66, NULL, NULL, NULL, NULL, NULL, 555, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu58', 'housing_indoor_menu58', 'interior guild shield', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 67, NULL, NULL, NULL, NULL, NULL, 61, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu59', 'housing_indoor_menu59', 'interior banner removal', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 66, NULL, NULL, NULL, NULL, NULL, 555, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu6', 'housing_indoor_menu6', 'green carpet', 0, NULL, NULL, NULL, NULL, NULL, 69, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu60', 'housing_indoor_menu60', 'interior shield removal', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 67, NULL, NULL, NULL, NULL, NULL, 61, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu7', 'housing_indoor_menu7', 'royal green carpet', 0, NULL, NULL, NULL, NULL, NULL, 70, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu8', 'housing_indoor_menu8', 'brown carpet', 0, NULL, NULL, NULL, NULL, NULL, 62, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_indoor_menu9', 'housing_indoor_menu9', 'dark gray carpet', 0, NULL, NULL, NULL, NULL, NULL, 72, 0, 0, NULL, 52, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);

INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu0', 'housing_indoor_menu', 'housing_indoor_menu0', 0, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu1', 'housing_indoor_menu', 'housing_indoor_menu1', 0, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu2', 'housing_indoor_menu', 'housing_indoor_menu2', 0, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu3', 'housing_indoor_menu', 'housing_indoor_menu3', 0, 3, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu4', 'housing_indoor_menu', 'housing_indoor_menu4', 0, 4, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu5', 'housing_indoor_menu', 'housing_indoor_menu5', 0, 5, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu6', 'housing_indoor_menu', 'housing_indoor_menu6', 0, 6, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu7', 'housing_indoor_menu', 'housing_indoor_menu7', 0, 7, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu8', 'housing_indoor_menu', 'housing_indoor_menu8', 0, 8, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu9', 'housing_indoor_menu', 'housing_indoor_menu9', 0, 9, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu10', 'housing_indoor_menu', 'housing_indoor_menu10', 0, 10, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu11', 'housing_indoor_menu', 'housing_indoor_menu11', 0, 11, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu12', 'housing_indoor_menu', 'housing_indoor_menu12', 0, 12, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu13', 'housing_indoor_menu', 'housing_indoor_menu13', 0, 13, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu14', 'housing_indoor_menu', 'housing_indoor_menu14', 0, 14, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu15', 'housing_indoor_menu', 'housing_indoor_menu15', 0, 15, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu16', 'housing_indoor_menu', 'housing_indoor_menu16', 0, 16, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu17', 'housing_indoor_menu', 'housing_indoor_menu17', 0, 17, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu18', 'housing_indoor_menu', 'housing_indoor_menu18', 0, 18, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu19', 'housing_indoor_menu', 'housing_indoor_menu19', 0, 19, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu20', 'housing_indoor_menu', 'housing_indoor_menu20', 0, 20, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu21', 'housing_indoor_menu', 'housing_indoor_menu21', 0, 21, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu22', 'housing_indoor_menu', 'housing_indoor_menu22', 0, 22, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu23', 'housing_indoor_menu', 'housing_indoor_menu23', 0, 23, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu24', 'housing_indoor_menu', 'housing_indoor_menu24', 0, 24, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu25', 'housing_indoor_menu', 'housing_indoor_menu25', 0, 25, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu26', 'housing_indoor_menu', 'housing_indoor_menu26', 0, 26, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu27', 'housing_indoor_menu', 'housing_indoor_menu27', 0, 27, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu28', 'housing_indoor_menu', 'housing_indoor_menu28', 1, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu29', 'housing_indoor_menu', 'housing_indoor_menu29', 1, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu30', 'housing_indoor_menu', 'housing_indoor_menu30', 1, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu31', 'housing_indoor_menu', 'housing_indoor_menu31', 1, 3, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu32', 'housing_indoor_menu', 'housing_indoor_menu32', 1, 4, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu33', 'housing_indoor_menu', 'housing_indoor_menu33', 1, 5, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu34', 'housing_indoor_menu', 'housing_indoor_menu34', 1, 6, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu35', 'housing_indoor_menu', 'housing_indoor_menu35', 1, 7, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu36', 'housing_indoor_menu', 'housing_indoor_menu36', 1, 8, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu37', 'housing_indoor_menu', 'housing_indoor_menu37', 1, 9, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu38', 'housing_indoor_menu', 'housing_indoor_menu38', 1, 10, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu39', 'housing_indoor_menu', 'housing_indoor_menu39', 1, 11, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu40', 'housing_indoor_menu', 'housing_indoor_menu40', 1, 12, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu41', 'housing_indoor_menu', 'housing_indoor_menu41', 1, 13, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu42', 'housing_indoor_menu', 'housing_indoor_menu42', 1, 14, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu43', 'housing_indoor_menu', 'housing_indoor_menu43', 1, 15, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu44', 'housing_indoor_menu', 'housing_indoor_menu44', 1, 16, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu45', 'housing_indoor_menu', 'housing_indoor_menu45', 1, 17, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu46', 'housing_indoor_menu', 'housing_indoor_menu46', 1, 18, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu47', 'housing_indoor_menu', 'housing_indoor_menu47', 1, 19, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu48', 'housing_indoor_menu', 'housing_indoor_menu48', 1, 20, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu49', 'housing_indoor_menu', 'housing_indoor_menu49', 1, 21, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu50', 'housing_indoor_menu', 'housing_indoor_menu50', 1, 22, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu51', 'housing_indoor_menu', 'housing_indoor_menu51', 1, 23, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu52', 'housing_indoor_menu', 'housing_indoor_menu52', 1, 24, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu53', 'housing_indoor_menu', 'housing_indoor_menu53', 1, 25, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu54', 'housing_indoor_menu', 'housing_indoor_menu54', 1, 26, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu55', 'housing_indoor_menu', 'housing_indoor_menu55', 1, 27, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu56', 'housing_indoor_menu', 'housing_indoor_menu56', 1, 28, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu57', 'housing_indoor_menu', 'housing_indoor_menu57', 2, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu58', 'housing_indoor_menu', 'housing_indoor_menu58', 2, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu59', 'housing_indoor_menu', 'housing_indoor_menu59', 2, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_indoor_menu60', 'housing_indoor_menu', 'housing_indoor_menu60', 2, 3, NULL);


 * OUTDOOR MENU ITEMS

INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu0', 'housing_outdoor_menu0', 'commoner\'s roof', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 59, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu1', 'housing_outdoor_menu1', 'burgess\'s roof', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 59, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 500000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu10', 'housing_outdoor_menu10', 'new wood door', 0, NULL, NULL, NULL, NULL, NULL, 4, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 100000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu11', 'housing_outdoor_menu11', 'four panel wooden door', 0, NULL, NULL, NULL, NULL, NULL, 5, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 150000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu12', 'housing_outdoor_menu12', 'iron door with knocker', 0, NULL, NULL, NULL, NULL, NULL, 6, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu13', 'housing_outdoor_menu13', 'fine wooden door', 0, NULL, NULL, NULL, NULL, NULL, 7, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 250000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu14', 'housing_outdoor_menu14', 'fine paneled door', 0, NULL, NULL, NULL, NULL, NULL, 8, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 300000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu15', 'housing_outdoor_menu15', 'embossed iron door', 0, NULL, NULL, NULL, NULL, NULL, 9, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 500000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu16', 'housing_outdoor_menu16', 'sand supports', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu17', 'housing_outdoor_menu17', 'river stone supports', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 100000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu18', 'housing_outdoor_menu18', 'driftwood supports', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 200000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu19', 'housing_outdoor_menu19', 'charcoal grey supports', 0, NULL, NULL, NULL, NULL, NULL, 3, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 300000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu2', 'housing_outdoor_menu2', 'noble\'s roof', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 59, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 5000000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu20', 'housing_outdoor_menu20', 'pearl grey supports', 0, NULL, NULL, NULL, NULL, NULL, 4, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 400000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu21', 'housing_outdoor_menu21', 'aged beige supports', 0, NULL, NULL, NULL, NULL, NULL, 5, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 500000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu22', 'housing_outdoor_menu22', 'winter moss supports', 0, NULL, NULL, NULL, NULL, NULL, 6, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 600000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu23', 'housing_outdoor_menu23', 'northern ivy supports', 0, NULL, NULL, NULL, NULL, NULL, 7, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 700000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu24', 'housing_outdoor_menu24', 'white oak supports', 0, NULL, NULL, NULL, NULL, NULL, 8, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 800000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu25', 'housing_outdoor_menu25', 'onyx supports', 0, NULL, NULL, NULL, NULL, NULL, 9, 0, 0, NULL, 63, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 900000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu26', 'housing_outdoor_menu26', 'sand porch', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu27', 'housing_outdoor_menu27', 'river stone porch', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 20000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu28', 'housing_outdoor_menu28', 'driftwood porch', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 30000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu29', 'housing_outdoor_menu29', 'charcoal grey porch', 0, NULL, NULL, NULL, NULL, NULL, 3, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 40000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu3', 'housing_outdoor_menu3', 'commoner\'s walls', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 60, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu30', 'housing_outdoor_menu30', 'pearl grey porch', 0, NULL, NULL, NULL, NULL, NULL, 4, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu31', 'housing_outdoor_menu31', 'aged beige porch', 0, NULL, NULL, NULL, NULL, NULL, 5, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 60000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu32', 'housing_outdoor_menu32', 'winter moss porch', 0, NULL, NULL, NULL, NULL, NULL, 6, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 70000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu33', 'housing_outdoor_menu33', 'northern ivy porch', 0, NULL, NULL, NULL, NULL, NULL, 7, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 80000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu34', 'housing_outdoor_menu34', 'white oak porch', 0, NULL, NULL, NULL, NULL, NULL, 8, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 90000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu35', 'housing_outdoor_menu35', 'onyx porch', 0, NULL, NULL, NULL, NULL, NULL, 9, 0, 0, NULL, 62, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 100000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu36', 'housing_outdoor_menu36', 'royal blue awning', 0, NULL, NULL, NULL, NULL, NULL, 53, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu37', 'housing_outdoor_menu37', 'dark blue awning', 0, NULL, NULL, NULL, NULL, NULL, 54, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu38', 'housing_outdoor_menu38', 'royal turquoise awning', 0, NULL, NULL, NULL, NULL, NULL, 57, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu39', 'housing_outdoor_menu39', 'royal teal awning', 0, NULL, NULL, NULL, NULL, NULL, 60, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu4', 'housing_outdoor_menu4', 'burgess\'s walls', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 60, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 500000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu40', 'housing_outdoor_menu40', 'royal red awning', 0, NULL, NULL, NULL, NULL, NULL, 66, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu41', 'housing_outdoor_menu41', 'violet awning', 0, NULL, NULL, NULL, NULL, NULL, 84, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu42', 'housing_outdoor_menu42', 'green awning', 0, NULL, NULL, NULL, NULL, NULL, 69, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu43', 'housing_outdoor_menu43', 'royal green awning', 0, NULL, NULL, NULL, NULL, NULL, 70, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu44', 'housing_outdoor_menu44', 'brown awning', 0, NULL, NULL, NULL, NULL, NULL, 62, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu45', 'housing_outdoor_menu45', 'dark gray awning', 0, NULL, NULL, NULL, NULL, NULL, 72, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu46', 'housing_outdoor_menu46', 'black awning', 0, NULL, NULL, NULL, NULL, NULL, 74, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu47', 'housing_outdoor_menu47', 'royal orange awning', 0, NULL, NULL, NULL, NULL, NULL, 77, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu48', 'housing_outdoor_menu48', 'royal yellow awning', 0, NULL, NULL, NULL, NULL, NULL, 83, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu49', 'housing_outdoor_menu49', 'white awning', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 56, NULL, NULL, NULL, NULL, NULL, 522, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu5', 'housing_outdoor_menu5', 'noble\'s walls', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 60, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 5000000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu50', 'housing_outdoor_menu50', 'exterior guild banner', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 57, NULL, NULL, NULL, NULL, NULL, 555, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu51', 'housing_outdoor_menu51', 'exterior guild shield', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 58, NULL, NULL, NULL, NULL, NULL, 61, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu52', 'housing_outdoor_menu52', 'exterior banner removal', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 57, NULL, NULL, NULL, NULL, NULL, 555, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu53', 'housing_outdoor_menu53', 'exterior shield removal', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 58, NULL, NULL, NULL, NULL, NULL, 61, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu54', 'housing_outdoor_menu54', 'sand shutters', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu55', 'housing_outdoor_menu55', 'river stone shutters', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 20000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu56', 'housing_outdoor_menu56', 'driftwood shutters', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 30000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu57', 'housing_outdoor_menu57', 'charcoal grey shutters', 0, NULL, NULL, NULL, NULL, NULL, 3, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 40000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu58', 'housing_outdoor_menu58', 'pearl grey shutters', 0, NULL, NULL, NULL, NULL, NULL, 4, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 50000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu59', 'housing_outdoor_menu59', 'aged beige shutters', 0, NULL, NULL, NULL, NULL, NULL, 5, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 60000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu6', 'housing_outdoor_menu6', 'wooden double door', 0, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu60', 'housing_outdoor_menu60', 'winter moss shutters', 0, NULL, NULL, NULL, NULL, NULL, 6, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 70000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu61', 'housing_outdoor_menu61', 'northern ivy shutters', 0, NULL, NULL, NULL, NULL, NULL, 7, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 80000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu62', 'housing_outdoor_menu62', 'white oak shutters', 0, NULL, NULL, NULL, NULL, NULL, 8, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 90000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu63', 'housing_outdoor_menu63', 'onyx shutters', 0, NULL, NULL, NULL, NULL, NULL, 9, 0, 0, NULL, 64, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 100000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu7', 'housing_outdoor_menu7', 'wooden door with chain', 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 40000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu8', 'housing_outdoor_menu8', 'iron door', 0, NULL, NULL, NULL, NULL, NULL, 2, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 150000, NULL, NULL);
INSERT INTO `itemtemplate` (`ItemTemplate_ID`, `Id_nb`, `Name`, `Level`, `Durability`, `MaxDurability`, `Condition`, `MaxCondition`, `Quality`, `DPS_AF`, `SPD_ABS`, `Hand`, `Type_Damage`, `Object_Type`, `Item_Type`, `Color`, `Emblem`, `Effect`, `Weight`, `Model`, `Extension`, `Bonus`, `Bonus1`, `Bonus2`, `Bonus3`, `Bonus4`, `Bonus5`, `ExtraBonus`, `Bonus1Type`, `Bonus2Type`, `Bonus3Type`, `Bonus4Type`, `Bonus5Type`, `ExtraBonusType`, `IsPickable`, `IsDropable`, `MaxCount`, `PackSize`, `Charges`, `MaxCharges`, `SpellID`, `ProcSpellID`, `Realm`, `IsTradable`, `Bonus6`, `Bonus7`, `Bonus8`, `Bonus9`, `Bonus10`, `Bonus6Type`, `Bonus7Type`, `Bonus8Type`, `Bonus9Type`, `Bonus10Type`, `Charges1`, `MaxCharges1`, `SpellID1`, `ProcSpellID1`, `PoisonSpellID`, `PoisonMaxCharges`, `PoisonCharges`, `CanDropAsLoot`, `AllowedClasses`, `CanUseEvery`, `PackageID`, `Flags`, `BonusLevel`, `Description`, `IsIndestructible`, `IsNotLosingDur`, `LevelRequirement`, `Price`, `ClassType`, `ProcChance`) VALUES ('housing_outdoor_menu9', 'housing_outdoor_menu9', 'aged wood door', 0, NULL, NULL, NULL, NULL, NULL, 3, 0, 0, NULL, 61, NULL, NULL, NULL, NULL, NULL, 520, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, 10000, NULL, NULL);

INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu0', 'housing_outdoor_menu', 'housing_outdoor_menu0', 0, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu1', 'housing_outdoor_menu', 'housing_outdoor_menu1', 0, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu2', 'housing_outdoor_menu', 'housing_outdoor_menu2', 0, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu3', 'housing_outdoor_menu', 'housing_outdoor_menu3', 0, 3, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu4', 'housing_outdoor_menu', 'housing_outdoor_menu4', 0, 4, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu5', 'housing_outdoor_menu', 'housing_outdoor_menu5', 0, 5, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu6', 'housing_outdoor_menu', 'housing_outdoor_menu6', 0, 6, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu7', 'housing_outdoor_menu', 'housing_outdoor_menu7', 0, 7, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu8', 'housing_outdoor_menu', 'housing_outdoor_menu8', 0, 8, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu9', 'housing_outdoor_menu', 'housing_outdoor_menu9', 0, 9, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu10', 'housing_outdoor_menu', 'housing_outdoor_menu10', 0, 10, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu11', 'housing_outdoor_menu', 'housing_outdoor_menu11', 0, 11, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu12', 'housing_outdoor_menu', 'housing_outdoor_menu12', 0, 12, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu13', 'housing_outdoor_menu', 'housing_outdoor_menu13', 0, 13, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu14', 'housing_outdoor_menu', 'housing_outdoor_menu14', 0, 14, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu15', 'housing_outdoor_menu', 'housing_outdoor_menu15', 0, 15, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu16', 'housing_outdoor_menu', 'housing_outdoor_menu16', 0, 16, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu17', 'housing_outdoor_menu', 'housing_outdoor_menu17', 0, 17, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu18', 'housing_outdoor_menu', 'housing_outdoor_menu18', 0, 18, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu19', 'housing_outdoor_menu', 'housing_outdoor_menu19', 0, 19, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu20', 'housing_outdoor_menu', 'housing_outdoor_menu20', 0, 20, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu21', 'housing_outdoor_menu', 'housing_outdoor_menu21', 0, 21, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu22', 'housing_outdoor_menu', 'housing_outdoor_menu22', 0, 22, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu23', 'housing_outdoor_menu', 'housing_outdoor_menu23', 0, 23, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu24', 'housing_outdoor_menu', 'housing_outdoor_menu24', 0, 24, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu25', 'housing_outdoor_menu', 'housing_outdoor_menu25', 0, 25, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu26', 'housing_outdoor_menu', 'housing_outdoor_menu26', 1, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu27', 'housing_outdoor_menu', 'housing_outdoor_menu27', 1, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu28', 'housing_outdoor_menu', 'housing_outdoor_menu28', 1, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu29', 'housing_outdoor_menu', 'housing_outdoor_menu29', 1, 3, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu30', 'housing_outdoor_menu', 'housing_outdoor_menu30', 1, 4, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu31', 'housing_outdoor_menu', 'housing_outdoor_menu31', 1, 5, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu32', 'housing_outdoor_menu', 'housing_outdoor_menu32', 1, 6, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu33', 'housing_outdoor_menu', 'housing_outdoor_menu33', 1, 7, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu34', 'housing_outdoor_menu', 'housing_outdoor_menu34', 1, 8, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu35', 'housing_outdoor_menu', 'housing_outdoor_menu35', 1, 9, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu36', 'housing_outdoor_menu', 'housing_outdoor_menu36', 1, 10, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu37', 'housing_outdoor_menu', 'housing_outdoor_menu37', 1, 11, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu38', 'housing_outdoor_menu', 'housing_outdoor_menu38', 1, 12, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu39', 'housing_outdoor_menu', 'housing_outdoor_menu39', 1, 13, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu40', 'housing_outdoor_menu', 'housing_outdoor_menu40', 1, 14, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu41', 'housing_outdoor_menu', 'housing_outdoor_menu41', 1, 15, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu42', 'housing_outdoor_menu', 'housing_outdoor_menu42', 1, 16, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu43', 'housing_outdoor_menu', 'housing_outdoor_menu43', 1, 17, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu44', 'housing_outdoor_menu', 'housing_outdoor_menu44', 1, 18, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu45', 'housing_outdoor_menu', 'housing_outdoor_menu45', 1, 19, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu46', 'housing_outdoor_menu', 'housing_outdoor_menu46', 1, 20, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu47', 'housing_outdoor_menu', 'housing_outdoor_menu47', 1, 21, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu48', 'housing_outdoor_menu', 'housing_outdoor_menu48', 1, 22, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu49', 'housing_outdoor_menu', 'housing_outdoor_menu49', 1, 23, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu50', 'housing_outdoor_menu', 'housing_outdoor_menu50', 2, 0, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu51', 'housing_outdoor_menu', 'housing_outdoor_menu51', 2, 1, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu52', 'housing_outdoor_menu', 'housing_outdoor_menu52', 2, 2, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu53', 'housing_outdoor_menu', 'housing_outdoor_menu53', 2, 3, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu54', 'housing_outdoor_menu', 'housing_outdoor_menu54', 2, 4, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu55', 'housing_outdoor_menu', 'housing_outdoor_menu55', 2, 5, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu56', 'housing_outdoor_menu', 'housing_outdoor_menu56', 2, 6, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu57', 'housing_outdoor_menu', 'housing_outdoor_menu57', 2, 7, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu58', 'housing_outdoor_menu', 'housing_outdoor_menu58', 2, 8, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu59', 'housing_outdoor_menu', 'housing_outdoor_menu59', 2, 9, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu60', 'housing_outdoor_menu', 'housing_outdoor_menu60', 2, 10, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu61', 'housing_outdoor_menu', 'housing_outdoor_menu61', 2, 11, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu62', 'housing_outdoor_menu', 'housing_outdoor_menu62', 2, 12, NULL);
INSERT INTO `merchantitem` (`MerchantItem_ID`, `ItemListID`, `ItemTemplateID`, `PageNumber`, `SlotPosition`, `PackageID`) VALUES ('housing_outdoor_menu63', 'housing_outdoor_menu', 'housing_outdoor_menu63', 2, 13, NULL);



*/