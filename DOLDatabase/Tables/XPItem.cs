using DOL.Database.Attributes;

namespace DOL.Database
{
	/// <summary>
	/// Table that holds the mapping of XP items
	/// </summary>
	[DataTable(TableName = "XPItem")]
	public class XPItem : DataObject
	{
		//important data
		public string m_mobName;
		public int m_mobRegion;
		public string m_itemName;
		public string m_itemTemplate;
		public int m_minLevel;
		public int m_maxLevel;
		public int m_realm;
		public int m_id;
		
		/// <summary>
		/// Primary Key Auto Increment.
		/// </summary>
		[PrimaryKey(AutoIncrement = true)]
		public int XPItemID
		{
			get { return m_id; }
			set
			{
				Dirty = true;
				m_id = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string MobName
		{
			get { return m_mobName; }
			set
			{
				Dirty = true;
				m_mobName = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MobRegion
		{
			get { return m_mobRegion; }
			set
			{
				Dirty = true;
				m_mobRegion = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string ItemName
		{
			get { return m_itemName; }
			set
			{
				Dirty = true;
				m_itemName = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public string ItemTemplate
		{
			get { return m_itemTemplate; }
			set
			{
				Dirty = true;
				m_itemTemplate = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MinLevel
		{
			get { return m_minLevel; }
			set
			{
				Dirty = true;
				m_minLevel = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int MaxLevel
		{
			get { return m_maxLevel; }
			set
			{
				Dirty = true;
				m_maxLevel = value;
			}
		}
		
		[DataElement(AllowDbNull = false, Index = false)]
		public int Realm
		{
			get { return m_realm; }
			set
			{
				Dirty = true;
				m_realm = value;
			}
		}
	}
}
#region sql
// CREATE TABLE IF NOT EXISTS xpitems (
//     `XPItemID` INT,
//     `MobName` VARCHAR(25) CHARACTER SET utf8,
//     `MobRegion` INT,
//     `ItemName` VARCHAR(37) CHARACTER SET utf8,
//     `ItemTemplate` VARCHAR(37) CHARACTER SET utf8,
//     `MinLevel` INT,
//     `MaxLevel` INT,
//     `Realm` INT,
//     `LastTimeRowUpdated` DATETIME
// );
// INSERT INTO xpitems VALUES
//     (1,'alp luachra',200,'Alp Luachra Head','Alp_Luachra_Head',31,37,3,'2022-06-22 00:00:00'),
//     (2,'crafted guardian',51,'Ancient Crafted Guardian Leg','Ancient_Crafted_Guardian_Leg',40,44,1,'2022-06-22 00:00:00'),
//     (3,'granite giant',1,'Ancient Granite Stone','Ancient_Granite_Stone',41,43,1,'2022-06-22 00:00:00'),
//     (4,'arbordon abductor',181,'Arbordon Arm','Arbordon_Arm',36,40,3,'2022-06-22 00:00:00'),
//     (5,'aurora',100,'Aurora Corpse','Aurora_Corpse',46,49,2,'2022-06-22 00:00:00'),
//     (6,'timberland badger',100,'Badger Stomach','Badger_Stomach',32,38,2,'2022-06-22 00:00:00'),
//     (7,'small black orm',100,'Black Orm Gland','Black_Orm_Gland',36,40,2,'2022-06-22 00:00:00'),
//     (8,'twisted sylvan',181,'Black Swirling Orb','Black_Swirling_Orb',32,38,3,'2022-06-22 00:00:00'),
//     (9,'sanidon',181,'Block of Sanidon Sand','Block_of_Sanidon_Sand',40,44,3,'2022-06-22 00:00:00'),
//     (10,'cyclops',1,'Bloody Cyclops Eye','Bloody_Cyclops_Eye',41,45,1,'2022-06-22 00:00:00'),
//     (11,'forest stalker',1,'Blood Red Berry Wine','Blood_Red_Berry_Wine',27,36,1,'2022-06-22 00:00:00'),
//     (12,'aged boreal cockatrice',100,'Cockatrice Feather','Boreal_Cockatrice_Feather',49,50,2,'2022-06-22 00:00:00'),
//     (13,'Ellyll villager',1,'Bottle of Ellyll Wine','Bottle_of_Ellyll_Wine',45,47,1,'2022-06-22 00:00:00'),
//     (14,'thrawn ogre guardsman',51,'Bronze Thrawn Ogre Thresher Spear Tip','Bronze_Thrawn_Ogre_Thresher_Spear_Tip',46,48,1,'2022-06-22 00:00:00'),
//     (15,'bucca',1,'Bucca Silver Mirror','Bucca_Silver_Mirror',23,27,1,'2022-06-22 00:00:00'),
//     (16,'bugbear nuisance',181,'Bug Bear Snout','Bug_Bear_Snout',44,46,3,'2022-06-22 00:00:00'),
//     (17,'bwgan hunter',1,'Bwgan Hunter Eye','Bwgan_Hunter_Eye',22,31,1,'2022-06-22 00:00:00'),
//     (18,'caer caddug bodyguard',51,'Caer Caddug Bodyguard Bracer','Caer_Caddug_Bodyguard_Bracer',44,46,1,'2022-06-22 00:00:00'),
//     (19,'cave lioness',151,'Cave Lioness Pelt','Cave_Lioness_Pelt',38,42,2,'2022-06-22 00:00:00'),
//     (20,'cervideth',181,'Cervideth Antlers','Cervideth_Antlers',44,46,3,'2022-06-22 00:00:00'),
//     (21,'ghoul desecrator',51,'Clawed Mummified Hand','Clawed_Mummified_Hand',49,50,1,'2022-06-22 00:00:00'),
//     (22,'cliff crawler',1,'Cliff Crawler Leg','Cliff_Crawler_Leg',42,46,1,'2022-06-22 00:00:00'),
//     (23,'contraption',51,'Oily Contraption Bits','Contraption_Pieces',32,38,1,'2022-06-22 00:00:00'),
//     (24,'spraggonale',200,'Copper Moonstone Flagon','copper_moonstone_flagon',21,30,3,'2022-06-22 00:00:00'),
//     (25,'coral thing',151,'Coral','Coral',38,42,2,'2022-06-22 00:00:00'),
//     (26,'Cornish hen',1,'Cornish Hen Tail','Cornish_Hen_Tail',24,30,1,'2022-06-22 00:00:00'),
//     (27,'pitch skeleton',51,'Crackling Pitch Skeleton Hand','Crackling_Pitch_Skeleton_Hand',49,50,1,'2022-06-22 00:00:00'),
//     (28,'dark fire',20,'Crystallized Darkfire','Crystallized_Darkfire',43,45,1,'2022-06-22 00:00:00'),
//     (29,'Danaoin fisherman',1,'Danaoin Fishing Fly','Danaoin_Fishing_Fly',40,44,1,'2022-06-22 00:00:00'),
//     (30,'Danaoin farmer',1,'Danaoin Harvest List','Danaoin_Harvest_List',44,46,1,'2022-06-22 00:00:00'),
//     (31,'Danaoin clerk',1,'Danaoin Poison','Danaoin_Poison',39,43,1,'2022-06-22 00:00:00'),
//     (32,'phantom wickerman',200,'Darkened Terror Claw','darkened_terror_claw',28,30,3,'2022-06-22 00:00:00'),
//     (33,'urchin pilferer',51,'Dark Urchin Spines','Dark_Urchin_Spines',44,46,1,'2022-06-22 00:00:00'),
//     (34,'deamhan aeir',200,'Deamhan Aeir Essence','deamhan_aeir_essence',34,40,3,'2022-06-22 00:00:00'),
//     (35,'undead Briton invader',200,'Death Robe','death_robe',43,47,3,'2022-06-22 00:00:00'),
//     (36,'death shroud',181,'Death Shroud Silk','Death_Shroud_Silk',49,50,3,'2022-06-22 00:00:00'),
//     (37,'corpse mold',51,'Decayed Water Orb','Decayed_Water_Orb',42,44,1,'2022-06-22 00:00:00'),
//     (38,'freybug',1,'Decorative Arrows','Decorative_Arrows',35,41,1,'2022-06-22 00:00:00'),
//     (39,'deep goblin',20,'Deep Goblin Hair','Deep_Goblin_Hair',42,45,1,'2022-06-22 00:00:00'),
//     (40,'young brown drake',1,'Drake Claw','Drake_Claw',47,49,1,'2022-06-22 00:00:00'),
//     (41,'drakoran leech',51,'Drakoran Leech Hand','Drakoran_Leech_Hand',28,35,1,'2022-06-22 00:00:00'),
//     (42,'drakoran marauder',51,'Drakoran Marauder Helm','Drakoran_Marauder_Helm',24,33,1,'2022-06-22 00:00:00'),
//     (43,'drakoran skirmisher',51,'Tattooed Drakoran Skirmisher Head','Drakoran_Skirmisher_Head',20,29,1,'2022-06-22 00:00:00'),
//     (44,'drakulv missionary',100,'Drakulv Scale','Drakulv_Scale',41,47,2,'2022-06-22 00:00:00'),
//     (45,'undead monk',1,'Dried Monk Skull','Dried_Monk_Skull',29,38,1,'2022-06-22 00:00:00'),
//     (46,'dunter',1,'Dunter Head','Dunter_Head',33,39,1,'2022-06-22 00:00:00'),
//     (47,'dusky sylvanshade',181,'Dusky Sylvanshade Head','Dusky_Sylvanshade_Head',45,47,3,'2022-06-22 00:00:00'),
//     (48,'grave goblin shaman',20,'Enchanted Metal Studs','Enchanted_Metal_Studs',38,42,1,'2022-06-22 00:00:00'),
//     (49,'fiery fiend',1,'Ever Burning Ember','Ever_Burning_Ember',41,45,1,'2022-06-22 00:00:00'),
//     (50,'faerie drake',200,'Faerie Drake Hide','Faerie_Drake_Hide',22,31,3,'2022-06-22 00:00:00'),
//     (51,'faerie frog',1,'Faerie Frog Eye','Faerie_Frog_Eye',28,37,1,'2022-06-22 00:00:00'),
//     (52,'fell geist',51,'Faint Geist Essence','Faint_Geist_Essence',49,50,1,'2022-06-22 00:00:00'),
//     (53,'faraheim stag',151,'Faraheim Stag Body','Faraheim_Stag_Body',49,50,2,'2022-06-22 00:00:00'),
//     (54,'bwca',1,'Fell Creature''s Tooth','Fell_Creatures_Tooth',30,39,1,'2022-06-22 00:00:00'),
//     (55,'fenrir tracker',100,'Fenrir Tracker Paw','Fenrir_Tracker_Paw',40,44,2,'2022-06-22 00:00:00'),
//     (56,'unearthed cave bear',200,'Fine Bear Pelt','Fine_Bear_Pelt',49,50,3,'2022-06-22 00:00:00'),
//     (57,'fomorian geddon',181,'Fomorian Geddon Great Bow','Fomorian_Geddon_Great_Bow',30,39,3,'2022-06-22 00:00:00'),
//     (58,'fomorian tremulare',181,'Fomorian Hormone Gland','Fomorian_Hormone_Gland',32,38,3,'2022-06-22 00:00:00'),
//     (59,'forest viper',100,'Forest Viper Venom','Forest_Viper_Venom',40,44,2,'2022-06-22 00:00:00'),
//     (60,'amadan touched',200,'Forgotten Silver Jasper Locket','Forgotten_Silver_Jasper_Locket',30,39,3,'2022-06-22 00:00:00'),
//     (61,'icestrider chiller',100,'Frosted Gimel Root','Frosted_Gimel_Root',43,45,2,'2022-06-22 00:00:00'),
//     (62,'gangrenous mass',51,'Gangrenous Mass Pustule','Gangrenous_Mass_Pustule',48,50,1,'2022-06-22 00:00:00'),
//     (63,'ghostly Hibernian invader',100,'Ghostly Necklace','Ghostly_Necklace',42,46,2,'2022-06-22 00:00:00'),
//     (64,'phantom magi',51,'Ghostly Soul Shroud','Ghostly_Soul_Shroud',46,48,1,'2022-06-22 00:00:00'),
//     (65,'ghoulic viper',20,'Ghoulic Viper Fang','ghoulic_viper_fang',41,45,1,'2022-06-22 00:00:00'),
//     (66,'giant boar',1,'Giant Boar Claw','Giant_Boar_Claw',34,40,1,'2022-06-22 00:00:00'),
//     (67,'giant lizard',1,'Giant Lizard Sinew','Giant_Lizard_Sinew',36,40,1,'2022-06-22 00:00:00'),
//     (68,'giant snowcrab',100,'Giant Snow Crab Claw','Giant_Snow_Crab_Claw',33,39,2,'2022-06-22 00:00:00'),
//     (69,'glimmerling',200,'Glimmering Gem','Glimmering_Gem',41,43,3,'2022-06-22 00:00:00'),
//     (70,'blind boogey',23,'Glowing Soul Gem','Glowing_Soul_Gem',35,41,1,'2022-06-22 00:00:00'),
//     (71,'unseelie underviewer',220,'Glowing Veined Stone','Glowing_Veined_Stone',42,46,3,'2022-06-22 00:00:00'),
//     (72,'sjoalf worshipper',151,'Golden Chain Necklace','Golden_Chain_Necklace',36,40,2,'2022-06-22 00:00:00'),
//     (73,'cliff dweller hunter',200,'Gold Lined Drinking Horn','Gold_Lined_Drinking_Horn',36,40,3,'2022-06-22 00:00:00'),
//     (74,'gorge shriller',181,'Gorge Shriller Wing','Gorge_Shriller_Wing',42,44,3,'2022-06-22 00:00:00'),
//     (75,'great boar',1,'Great Boar Hide','Great_Boar_Hide',42,46,1,'2022-06-22 00:00:00'),
//     (76,'grovewood',200,'Grovewood Bark','Grovewood_Bark',38,42,3,'2022-06-22 00:00:00'),
//     (77,'thrawn ogre thresher',51,'Gruesome Thrawn Ogre Head','Gruesome_Thrawn_Ogre_Head',46,48,1,'2022-06-22 00:00:00'),
//     (78,'dullahan',200,'Horseshoe','Horseshoe',48,49,3,'2022-06-22 00:00:00'),
//     (79,'iceberg',100,'Iceberg Tooth','Iceberg_Tooth',41,44,2,'2022-06-22 00:00:00'),
//     (80,'frost giant',100,'Ice Cold Giant Blood','Ice_Cold_Giant_Blood',37,41,2,'2022-06-22 00:00:00'),
//     (81,'ire wolf',200,'Ire Wolf Claw','Ire_Wolf_Claw',27,36,3,'2022-06-22 00:00:00'),
//     (82,'Ixthiar Broodmother',181,'Ixthiar Reaper Mandible','Ixthiar_Reaper_Mandible',42,46,3,'2022-06-22 00:00:00'),
//     (83,'creeping ooze',20,'Jeweled Polished Skull','jeweled_polished_skull',42,45,1,'2022-06-22 00:00:00'),
//     (84,'hagbui shaman',100,'Jeweled Shaman Totem','Jeweled_Shaman_Totem',40,43,2,'2022-06-22 00:00:00'),
//     (85,'juvenile megafelid',200,'Juvenile Megafelid Tooth','juvenile_megafelid_tooth',36,38,3,'2022-06-22 00:00:00'),
//     (86,'townsman',1,'Key of the Lost','Key_of_the_Lost',30,39,1,'2022-06-22 00:00:00'),
//     (87,'white boar',200,'Long White Boar Tusk','Long_White_Boar_Tusk',40,43,3,'2022-06-22 00:00:00'),
//     (88,'werewolf churl',127,'Lost Pearl','Lost_Pearl',34,40,2,'2022-06-22 00:00:00'),
//     (89,'clay jotun',100,'Magic Clay','Magic_Clay',40,43,2,'2022-06-22 00:00:00'),
//     (90,'savage wyvern',100,'Malefic Tooth','Malefic_Tooth',43,48,2,'2022-06-22 00:00:00'),
//     (91,'megaloceros fawn',151,'Megaloceros Fawn Claw','Megaloceros_Fawn_Claw',28,37,2,'2022-06-22 00:00:00'),
//     (92,'mephitic ghoul',100,'Melted Ghoul Flesh','Melted_Ghoul_Flesh',26,35,2,'2022-06-22 00:00:00'),
//     (93,'mist menace',151,'Mist Water Sack','Mist_Water_Sack',24,33,2,'2022-06-22 00:00:00'),
//     (94,'morvalt landflyke',151,'Morvalt Landflyke Head','Morvalt_Landflyke_Head',26,35,2,'2022-06-22 00:00:00'),
//     (95,'morvalt beskydda',151,'Morvalt Skydda Eye','Morvalt_Skydda_Eye',34,40,2,'2022-06-22 00:00:00'),
//     (96,'morvalt streber',151,'Morvalt Streber''s Head','Morvalt_Strebers_Head',30,39,1,'2022-06-22 00:00:00'),
//     (97,'apple fly',51,'Mottled Apple Fly Wings','Mottled_Apple_Fly_Wings',26,35,1,'2022-06-22 00:00:00'),
//     (98,'mud crab',100,'Mud Crab Claw','Mud_Crab_Claw',35,41,2,'2022-06-22 00:00:00'),
//     (99,'mud frog',100,'Mud Frog Tongue','Mud_Frog_Tongue',30,39,2,'2022-06-22 00:00:00'),
//     (100,'myling',151,'Myling Essence','Myling_Essence',30,39,1,'2022-06-22 00:00:00'),
//     (101,'octonid',181,'Octonid Tentacle','Octonid_Tentacle',44,46,3,'2022-06-22 00:00:00'),
//     (102,'grogan',200,'Pastry Puff','Pastry_Puff',46,48,3,'2022-06-22 00:00:00'),
//     (103,'peallaidh',1,'Peallaidh Hide','Peallaidh_Hide',37,41,1,'2022-06-22 00:00:00'),
//     (104,'drowning victim',151,'Pendant of the Forsaken','Pendant of the Forsaken',22,31,2,'2022-06-22 00:00:00'),
//     (105,'phaeghoul',200,'Phaeghoul Red Hand','Phaeghoul_Red_Hand',37,41,3,'2022-06-22 00:00:00'),
//     (106,'pictish warrior',1,'Pictish Warrior Ring','pictish_warrior_ring',41,47,1,'2022-06-22 00:00:00'),
//     (107,'ashmonger',100,'Polished Piece of Obsidian','Polished_Piece_of_Obsidian',25,34,2,'2022-06-22 00:00:00'),
//     (108,'shadowhunter she-wolf',1,'Pristine She-wolf Pelt','pristine_she-wolfpelt',42,46,1,'2022-06-22 00:00:00'),
//     (109,'rage wolf',200,'Rage Wolf Tooth','rage_wolf_tooth',31,32,3,'2022-06-22 00:00:00'),
//     (110,'ravaging marine shriller',181,'Ravaging Marine Shriller Hide','Ravaging_Marine_Shriller_Hide',46,48,3,'2022-06-22 00:00:00'),
//     (111,'cruach imp',200,'Red Cruach Wings','Red_Cruach_Wings',33,39,3,'2022-06-22 00:00:00'),
//     (112,'icestrider interceptor',100,'Red Eyes','Red_Eye',47,49,2,'2022-06-22 00:00:00'),
//     (113,'undead apple gatherer',51,'Rotted Apple Gatherer Flesh','Rotted_Apple_Gatherer_Flesh',28,37,1,'2022-06-22 00:00:00'),
//     (114,'corpse delver',51,'Rotted Corpse Delver Eye','Rotted_Corpse_Delver_Eye',44,46,1,'2022-06-22 00:00:00'),
//     (115,'ebon skeleton',51,'Rotted Ebon Skeleton Tibia','Rotted_Ebon_Skeleton_Tibia',48,49,1,'2022-06-22 00:00:00'),
//     (116,'hagbui runemaster',100,'Runemaster Sealed Pages','Runemaster_Sealed_Pages',42,45,2,'2022-06-22 00:00:00'),
//     (117,'decaying marshman',51,'Rusted Pendant','Rusted_Pendant',36,40,1,'2022-06-22 00:00:00'),
//     (118,'farmer',1,'Sack of Grain','Sack_of_Grain',32,38,1,'2022-06-22 00:00:00'),
//     (119,'tidal sheerie',200,'Sack of Grain','Sack_of_Grain2',35,41,3,'2022-06-22 00:00:00'),
//     (120,'scaled fiend',20,'Scaled Fiend Heart','scaled_fiend_heart',42,44,1,'2022-06-22 00:00:00'),
//     (121,'cursed mora',100,'Scroll of Eternal Sorrow','Scroll_of_Eternal_Sorrow',23,32,2,'2022-06-22 00:00:00'),
//     (122,'sett dweller',200,'Sett Fur','Sett_Fur',22,31,3,'2022-06-22 00:00:00'),
//     (123,'ghastly midgard invader',200,'Shadowy Helm','Shadowy_Helm',43,46,3,'2022-06-22 00:00:00'),
//     (124,'greater water elemental',51,'Shimmering Water Orb','Shimmering_Water_Orb',48,49,1,'2022-06-22 00:00:00'),
//     (125,'headless footman',51,'Shredded Footman''s Cloak','Shredded_Footmans_Cloak',46,48,1,'2022-06-22 00:00:00'),
//     (126,'siabra venator',200,'Siabra Charm Ring','Siabra_Charm_Ring',43,45,3,'2022-06-22 00:00:00'),
//     (127,'gurite waylayer',200,'Siabra Waylayer Sash','Siabra_Waylayer_Sash',26,35,3,'2022-06-22 00:00:00'),
//     (128,'skeletal dwarf invader',200,'Skeletal Dwarf Invader Skull','Skeletal_Dwarf_Invader_Skull',43,47,3,'2022-06-22 00:00:00'),
//     (129,'moorlich',1,'Skin Cream','skin_cream',48,49,1,'2022-06-22 00:00:00'),
//     (130,'boggart',1,'Sleeping Kitten','Sleeping_Kitten',46,48,1,'2022-06-22 00:00:00'),
//     (131,'spectral manslayer',200,'Spectral Essence','spectral_essence',48,50,3,'2022-06-22 00:00:00'),
//     (132,'speghoul',200,'Speghoul Heart','Speghoul_Heart',40,44,3,'2022-06-22 00:00:00'),
//     (133,'bananach',200,'Spirit Catcher Stone','Spirit_Catcher_Stone',40,43,3,'2022-06-22 00:00:00'),
//     (134,'hagbui spiritmaster',100,'Spirit Stone','Spirit_Stone1',43,47,2,'2022-06-22 00:00:00'),
//     (135,'steam element',151,'Steam','Steam',32,38,2,'2022-06-22 00:00:00'),
//     (136,'mercenary tomb raider',1,'Stolen Signet Ring','Stolen_Signet_Ring',26,35,1,'2022-06-22 00:00:00'),
//     (137,'streaming wisp',200,'Streaming Wisp Husk','Streaming_Wisp_Husk',24,33,3,'2022-06-22 00:00:00'),
//     (138,'svartalf infiltrator',100,'Svartalf Poison Recipe','Svartalf_Poison_Recipe',45,47,2,'2022-06-22 00:00:00'),
//     (139,'terra crab',125,'Terra Crab Claw','Terra_Crab_Claw',38,42,2,'2022-06-22 00:00:00'),
//     (140,'tittering tree dervish',181,'Tittering Tree Dervish Bark','Tittering_Tree_Dervish_Bark',48,49,3,'2022-06-22 00:00:00'),
//     (141,'torc',200,'Torc Tooth','torc_tooth',34,37,3,'2022-06-22 00:00:00'),
//     (142,'svartalf foister',100,'Trip String','Trip_String',49,50,2,'2022-06-22 00:00:00'),
//     (143,'fallen troll',100,'Troll Teeth','Troll_Teeth',44,47,2,'2022-06-22 00:00:00'),
//     (144,'umber bear',200,'Umber Bear Tooth','umber_bear_tooth',44,46,3,'2022-06-22 00:00:00'),
//     (145,'vehement guardian',200,'Vehement Gizzard','Vehement_Gizzard',40,44,3,'2022-06-22 00:00:00'),
//     (146,'cutthroat vines',181,'Venom Sac','Venom_Sac',38,42,3,'2022-06-22 00:00:00'),
//     (147,'deamhan hound',200,'Vial of Deamhan Breath','Vial_of_Deamhan_Breath',40,44,3,'2022-06-22 00:00:00'),
//     (148,'ghastly albion invader',100,'Vial of Ghastly Vapor','Vial_of_Ghastly_Vapor',42,45,2,'2022-06-22 00:00:00'),
//     (149,'spectral briton invader',200,'Vial of Glowing Gases','Vial_of_Glowing_Gases',43,47,3,'2022-06-22 00:00:00'),
//     (150,'moss monster',200,'Vial of Moss Juice','Vial_of_Moss_Juice',40,43,3,'2022-06-22 00:00:00'),
//     (151,'werewolf warder',100,'Warder''s Ear','Warders_Ear',27,36,2,'2022-06-22 00:00:00'),
//     (152,'fire ant gatherer',100,'Warm Fire Ant Larva','Warm_Fire_Ant_Larva',21,30,2,'2022-06-22 00:00:00'),
//     (153,'treekeep',100,'Warm Tree Sap','Warm_Tree_Sap',22,31,2,'2022-06-22 00:00:00'),
//     (154,'washed-up skeleton',151,'Washed Up Skeleton Skull','Washed_Up_Skeleton_Skull',36,40,2,'2022-06-22 00:00:00'),
//     (155,'lesser zephyr',200,'Wind Swept Leaves','Wind_Swept_Leaves',25,34,3,'2022-06-22 00:00:00'),
//     (156,'strangler',51,'Worn Strangler Cord','Worn_Strangler_Cord',42,44,1,'2022-06-22 00:00:00'),
//     (157,'yellowed skeleton',151,'Yellowed Bone Beads','yellowed_bone_beads',34,40,1,'2022-06-22 00:00:00'),
//     (158,'curmudgeon puggard',200,'Yellow Cumudgeon Tooth','Yellow_Curmudgeon_Tooth',42,50,3,'2022-06-22 00:00:00');
#endregion