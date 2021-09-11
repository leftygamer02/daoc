
/* Copy regions */
INSERT INTO newatlas.regions (Id, NAME, Description, IP,PORT,EXPANSION,housingenabled, divingenabled,waterlevel,classtype,isfrontier,createdate, modifydate)
SELECT RegionID, NAME, Description, ip, PORT, EXPANSION, housingenabled,divingenabled,waterlevel,classtype, isfrontier,NOW(), NOW() FROM atlas.regions ORDER BY RegionID;

/* Copy zones */
INSERT INTO newatlas.zones (RegionID, NAME, IsLava, DivingFlag,WaterLevel,OffsetY, OffsetX, Width, Height, Experience, Realmpoints, Bountypoints, Coin,Realm,CreateDate, ModifyDate)
SELECT r2.Id,zones.NAME, zones.IsLava,zones.DivingFlag,zones.WaterLevel, zones.OffsetY, zones.OffsetX, zones.Width, zones.Height, zones.Experience, zones.Realmpoints, zones.Bountypoints,zones.Coin, zones.Realm, NOW(), NOW() 
FROM atlas.zones
INNER JOIN atlas.regions r1 ON zones.RegionID = r1.RegionID
INNER JOIN newatlas.regions r2 ON r1.Name = r2.Name;

/* Copy races */
INSERT INTO newatlas.races (Id, NAME, ResistBody, ResistCold, ResistCrush, ResistEnergy, ResistHeat, ResistMatter,ResistSlash, ResistSpirit, ResistThrust, ResistNatural, CreateDate, ModifyDate)
SELECT ID, NAME, ResistBody, ResistCold, ResistCrush, ResistEnergy, ResistHeat, ResistMatter,ResistSlash, ResistSpirit, ResistThrust, ResistNatural, NOW(),NOW()
FROM atlas.race;

/* Copy Areas */
INSERT INTO newatlas.areas (TranslationID, Description, X,Y,Z,Radius, RegionID, ClassType, CanBroadcast,Sound,CheckLOS, Points,CreateDate, ModifyDate)
SELECT TranslationID, Description, X,Y,Z,Radius, Region, ClassType, CanBroadcast,Sound,CheckLOS, Points, NOW(),NOW() FROM atlas.area;

/* Copy Abilities */
INSERT INTO newatlas.abilities (Id, KeyName, NAME, InternalID, Description, IconID, Implementation, AbilityOrder, CreateDate, ModifyDate)
SELECT AbilityID,KeyName, NAME, InternalID, Description,IconID, Implementation,1, NOW(),NOW() FROM atlas.ability;

/* Copy battelgrounds */
INSERT INTO newatlas.battlegrounds (RegionID, MinLevel,MaxLevel,MaxRealmLevel, KeyName, CreateDate, ModifyDate)
SELECT RegionID, MinLevel,MaxLevel,MaxRealmLevel, Battleground_ID, NOW(),NOW() FROM atlas.battleground;

/* Copy Bindpoints */
INSERT INTO newatlas.bindpoints (X,Y,Z,Radius,RegionID,Realm,CreateDate,ModifyDate)
SELECT X,Y,Z,Radius,Region,Realm,NOW(),NOW() FROM atlas.bindpoint WHERE Region IN (SELECT RegionID FROM atlas.regions);

/* Copy Class realm abilities */
INSERT INTO newatlas.classrealmabilities (ClassID, AbilityID, AbilityOrder, CreateDate, ModifyDate)
SELECT CharClass,ability.AbilityID,ROW_NUMBER() OVER (PARTITION BY CharClass ORDER BY ClassXRealmAbility_ID) AS AbilityOrder, NOW(),NOW()
FROM atlas.classxrealmability
INNER JOIN atlas.ability ON ability.KeyName = classxrealmability.AbilityKey;

/* Copy specializations */
INSERT INTO newatlas.specializations (Id,KeyName, NAME, Icon,Description, Implementation, CreateDate, ModifyDate)
SELECT SpecializationID, KeyName, NAME, Icon,Description, Implementation, NOW(),NOW() FROM atlas.specialization;

/* Copy specialization abilities */
INSERT INTO newatlas.specializationabilities (SpecializationID, AbilityID, SpecLevel, AbilityLevel, ClassID, CreateDate, ModifyDate)
SELECT sp.SpecializationID,a.AbilityID, sa.SpecLevel, sa.AbilityLevel, sa.ClassId, NOW(),NOW() FROM atlas.specxability sa
INNER JOIN atlas.specialization sp ON sp.KeyName = sa.Spec
INNER JOIN atlas.ability a ON a.KeyName = sa.AbilityKey;

/* Copy Class specializations */
INSERT INTO newatlas.classspecializations (ClassID, SpecializationID, LevelAcquired, CreateDate, ModifyDate)
SELECT cs.ClassID, sp.SpecializationID, cs.LevelAcquired, NOW(),NOW() FROM atlas.classxspecialization cs
INNER JOIN atlas.specialization sp ON sp.KeyName = cs.specKeyName;

/* Copy doors */
INSERT INTO newatlas.doors (NAME, TYPE, Z,Y,X, Heading, InternalID, Guild, LEVEL, Realm, Flags, Locked, Health, MaxHealth, CreateDate, ModifyDate)
SELECT NAME, TYPE, Z,Y,X,Heading,InternalID, guild, LEVEL, Realm, Flags, Locked, Health, MaxHealth, NOW(),NOW() FROM atlas.door;

/* Copy Factions */
INSERT INTO newatlas.factions (Id, NAME, BaseAggroLevel, CreateDate, ModifyDate)
SELECT ID, NAME, BaseAggroLevel, NOW(),NOW() FROM atlas.faction;

/* Copy Linked Factions */
INSERT INTO newatlas.linkedfactions (FactionID, RelatedFactionID, IsFriend, CreateDate, ModifyDate)
SELECT FactionID, LinkedFactionID, IsFriend, NOW(),NOW() FROM atlas.linkedfaction;

/* Copy Jump Points */
INSERT INTO newatlas.jumppoints (NAME, RegionID, xpos,ypos,zpos, heading, CreateDate, ModifyDate)
SELECT NAME, Region, Xpos,Ypos,Zpos,Heading, NOW(),NOW() FROM atlas.jumppoint;

/* Copy Keeps */
INSERT INTO newatlas.keeps (Id, NAME, RegionID, X,Y,Z, Heading, Realm, LEVEL, ClaimedGuildName, AlbionDifficultyLevel, MidgardDifficultyLevel, HiberniaDifficultyLevel,
OriginalRealm,KeepType,BaseLevel,SkinType,CreateInfo,CreateDate,ModifyDate)
SELECT KeepID,NAME,Region,X,Y,Z,Heading,Realm,LEVEL,ClaimedGuildName, AlbionDifficultyLevel, MidgardDifficultyLevel, HiberniaDifficultyLevel, OriginalRealm,
KeepType,BaseLevel,SkinType,CreateInfo,NOW(),NOW()
FROM atlas.keep;

/* Copy Keep componenents */
INSERT INTO newatlas.keepcomponents (X,Y,Heading, Health, Skin, KeepID, CreateInfo, CreateDate, ModifyDate)
SELECT X,Y,Heading,Health,Skin,KeepID, CreateInfo,NOW(),NOW() FROM atlas.keepcomponent WHERE KeepID IN (SELECT KeepID FROM atlas.Keep);

/* TODO: MORE KEEP MIGRATIONS */



/* Copy Spells */
INSERT INTO newatlas.spells (Id, ClientEffect, Icon,NAME, Description,Target, `RANGE`, POWER, CastTime, Damage,DamageType, TYPE, Duration, Frequency, Pulse,
PulsePower,Radius,RecastDelay,ResurrectHealth,ResurrectMana,VALUE, Concentration,LifeDrainReturn,AmnesiaChance,Message1,Message2,Message3,Message4,
InstrumentRequirement, SpellGroup,EffectGroup, SubSpellID,MoveCast,Uninterruptible,IsPrimary,IsSecondary,AllowBolt,SharedTimerGroup,PackageID, IsFocus,
ToolTipId,CreateDate,ModifyDate)
SELECT SpellID,ClientEffect, Icon,NAME, Description,Target, `RANGE`, POWER, CastTime, Damage,DamageType, TYPE, Duration, Frequency, Pulse,
PulsePower,Radius,RecastDelay,ResurrectHealth,ResurrectMana,VALUE, Concentration,LifeDrainReturn,AmnesiaChance,Message1,Message2,Message3,Message4,
InstrumentRequirement, SpellGroup,EffectGroup, NULL,MoveCast,Uninterruptible,IsPrimary,IsSecondary,AllowBolt,SharedTimerGroup,PackageID, 
CASE IsFocus WHEN 0 THEN 0 ELSE 1 END IsFocus,
ToolTipId, NOW(),NOW() FROM atlas.spell WHERE SubSpellID = 0;

/* copy spells with subspells */
INSERT INTO newatlas.spells (Id, ClientEffect, Icon,NAME, Description,Target, `RANGE`, POWER, CastTime, Damage,DamageType, TYPE, Duration, Frequency, Pulse,
PulsePower,Radius,RecastDelay,ResurrectHealth,ResurrectMana,VALUE, Concentration,LifeDrainReturn,AmnesiaChance,Message1,Message2,Message3,Message4,
InstrumentRequirement, SpellGroup,EffectGroup, SubSpellID,MoveCast,Uninterruptible,IsPrimary,IsSecondary,AllowBolt,SharedTimerGroup,PackageID, IsFocus,
ToolTipId,CreateDate,ModifyDate)
SELECT SpellID,ClientEffect, Icon,NAME, Description,Target, `RANGE`, POWER, CastTime, Damage,DamageType, TYPE, Duration, Frequency, Pulse,
PulsePower,Radius,RecastDelay,ResurrectHealth,ResurrectMana,VALUE, Concentration,LifeDrainReturn,AmnesiaChance,Message1,Message2,Message3,Message4,
InstrumentRequirement, SpellGroup,EffectGroup, SubSpellID,MoveCast,Uninterruptible,IsPrimary,IsSecondary,AllowBolt,SharedTimerGroup,PackageID, 
CASE IsFocus WHEN 0 THEN 0 ELSE 1 END IsFocus,
ToolTipId, NOW(),NOW() FROM atlas.spell WHERE SubSpellID > 0;

/* copy spell lines */
INSERT INTO newatlas.spelllines (Id, SpecializationID, KeyName, NAME, SpecKeyName, IsBaseLine, PackageID, ClassIDHint, CreateDate, ModifyDate)
SELECT sl.SpellLineID, sp.SpecializationID, sl.KeyName, sl.NAME, sl.Spec,sl.IsBaseLine, sl.PackageID, sl.ClassIDHint,NOW(),NOW() 
FROM atlas.spellline sl
LEFT JOIN atlas.specialization sp ON sp.KeyName = sl.Spec;

/* copy spell line spells */
INSERT INTO newatlas.spelllinespells (SpellLineID, SpellID, LEVEL, CreateDate, ModifyDate)
SELECT sl.Id, ls.SpellID, LEVEL, NOW(),NOW() FROM atlas.linexspell ls
INNER JOIN newatlas.spelllines sl ON sl.KeyName = ls.LineName;


/* Copy Teleports */
INSERT INTO newatlas.Teleports (`Type`, TeleportID, Realm, RegionID, X,Y,Z, Heading, CreateDate, ModifyDate)
SELECT `Type`,TeleportID, Realm,RegionID, X,Y,Z,Heading, NOW(),NOW() FROM atlas.teleport
WHERE RegionID IN (SELECT Id FROM newatlas.Regions);

/* Copy Zone Points */
INSERT INTO newatlas.ZonePoints (ClientId, TargetX, TargetY, TargetZ, TargetRegionID, TargetHeading, SourceX, SourceY, SourceZ, SourceRegionID, Realm, ClassType, CreateDate,ModifyDate)
SELECT Id, TargetX, TargetY, TargetZ, TargetRegion, TargetHeading, SourceX, SourceY, SourceZ, 
CASE WHEN SourceRegion = 0 THEN NULL ELSE SourceRegion END, Realm, ClassType, NOW(), NOW()
FROM atlas.zonepoint
WHERE TargetRegion IN (SELECT Id FROM newatlas.Regions)
AND (SourceRegion = 0 OR SourceRegion IN (SELECT Id FROM newatlas.Regions));


/* Copy item templates */
INSERT INTO newatlas.ItemTemplates (translationid, keyname, NAME, LEVEL, durability, maxdurability, isnotlosingdur, `CONDITION`, maxcondition, quality, dps_af, spd_abs,
hand, typedamage, objecttype, itemtype,color, emblem, effect, weight, model, extension, canuseevery, ispickable, isdropable, candropasloot, istradable, price, maxcount,
isindestructible, packsize, realm, allowedclasses, flags, bonuslevel, levelrequirement, packageid, description, classtype, itembonus, discriminator, createdate, modifydate)
SELECT translationid, id_nb, NAME, LEVEL, durability, maxdurability, isnotlosingdur, `CONDITION`, maxcondition, quality, dps_af, spd_abs,
hand, type_damage, object_type, item_type,color, emblem, effect, weight, model, extension, canuseevery, ispickable, isdropable, candropasloot, istradable, price, maxcount,
isindestructible, packsize, realm, allowedclasses, flags, bonuslevel, levelrequirement, packageid, description, classtype, bonus, 'ItemTemplate', NOW(),NOW()
FROM atlas.ItemTemplate;

/* Copy item bonuses */
/* BONUS 1 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus1Type, it1.Bonus1,1,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus1 > 0;

/* BONUS 2 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus2Type, it1.Bonus2,2,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus2 > 0;

/* BONUS 3 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus3Type, it1.Bonus3,3,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus3 > 0;

/* BONUS 4 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus4Type, it1.Bonus4,4,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus4 > 0;

/* BONUS 5 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus5Type, it1.Bonus5,5,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus5 > 0;

/* BONUS 6 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus6Type, it1.Bonus6,6,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus6 > 0;

/* BONUS 7 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus7Type, it1.Bonus7,7,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus7 > 0;

/* BONUS 8 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus8Type, it1.Bonus8,8,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus8 > 0;

/* BONUS 9 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus9Type, it1.Bonus9,9,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus9 > 0;

/* BONUS 10 */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.Bonus10Type, it1.Bonus10,10,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.bonus10 > 0;

/* Extra bonus */
INSERT INTO newatlas.ItemBonuses (ItemTemplateID, BonusType, BonusValue, BonusOrder, CreateDate, ModifyDate)
SELECT it2.Id, it1.ExtrabonusType, it1.ExtraBonus,11,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.ExtraBonus > 0;


/* Copy Item Spells */
INSERT INTO newatlas.ItemSpells (ItemTemplateID, SpellID, Charges, MaxCharges,ProcChance,IsPoison, CreateDate, ModifyDate)
SELECT it2.Id, it1.SpellID, it1.Charges,it1.MaxCharges,0,false,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.SpellID > 0 AND it1.SpellID IN (SELECT Id FROM newatlas.spells);

INSERT INTO newatlas.ItemSpells (ItemTemplateID, SpellID, Charges, MaxCharges,ProcChance,IsPoison, CreateDate, ModifyDate)
SELECT it2.Id, it1.SpellID1, it1.Charges1,it1.MaxCharges1,0,false,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.SpellID1 > 0 AND it1.SpellID1 IN (SELECT Id FROM newatlas.spells);

/* Copy item procs */
INSERT INTO newatlas.ItemSpells (ItemTemplateID, SpellID, Charges, MaxCharges,ProcChance,IsPoison, CreateDate, ModifyDate)
SELECT it2.Id, it1.ProcSpellID, 0,0,it1.ProcChance,false,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.ProcSpellID > 0 AND it1.ProcSpellID IN (SELECT Id FROM newatlas.spells);

INSERT INTO newatlas.ItemSpells (ItemTemplateID, SpellID, Charges, MaxCharges,ProcChance,IsPoison, CreateDate, ModifyDate)
SELECT it2.Id, it1.ProcSpellID1, 0,0,it1.ProcChance,false,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.ProcSpellID1 > 0 AND it1.ProcSpellID1 IN (SELECT Id FROM newatlas.spells);

/* Copy item poisons */
INSERT INTO newatlas.ItemSpells (ItemTemplateID, SpellID, Charges, MaxCharges,ProcChance,IsPoison, CreateDate, ModifyDate)
SELECT it2.Id, it1.PoisonSpellID, it1.PoisonCharges,it1.PoisonMaxCharges,20,false,NOW(),NOW() 
FROM atlas.ItemTemplate it1
INNER JOIN newatlas.itemtemplates it2 ON it1.Id_nb = it2.KeyName
WHERE it1.PoisonSpellID > 0 AND it1.PoisonSpellID IN (SELECT Id FROM newatlas.spells);


/* Copy Styles */
/* copy styles with no style openers */
INSERT INTO newAtlas.Styles (ClassID, SpecializationID, NAME, SpecKeyName, SpecLevelRequirement, Icon, EnduranceCost, StealthRequirement,
OpeningRequirementType, OpeningRequirementValue, AttackResultRequirement, WeaponTypeRequirement, GrowthRate, BonusToHit, BonusToDefense,
TwoHandAnimation,RandomProc, ArmorHitLocation, GrowthOffset, CreateDate, ModifyDate)
SELECT st.ClassID, sp.Id,st.NAME, st.SpecKeyName, st.SpecLevelRequirement, st.Icon, st.EnduranceCost, StealthRequirement,
OpeningRequirementType, OpeningRequirementValue, AttackResultRequirement, WeaponTypeRequirement, GrowthRate, BonusToHit, BonusToDefense,
TwoHandAnimation,RandomProc, ArmorHitLocation, GrowthOffset, NOW(), NOW()
FROM atlas.Style st
INNER JOIN Newatlas.specializations sp ON st.SpecKeyName = sp.KeyName
WHERE (st.OpeningRequirementType != 0 OR st.OpeningRequirementValue = 0);

/* copy styles with style openers */
/* 2nd in chain */
INSERT INTO newAtlas.Styles (ClassID, SpecializationID, NAME, SpecKeyName, SpecLevelRequirement, Icon, EnduranceCost, StealthRequirement,
OpeningRequirementType, OpeningRequirementValue, AttackResultRequirement, WeaponTypeRequirement, GrowthRate, BonusToHit, BonusToDefense,
TwoHandAnimation,RandomProc, ArmorHitLocation, GrowthOffset, CreateDate, ModifyDate)
SELECT st.ClassID, sp.Id,st.NAME, st.SpecKeyName, st.SpecLevelRequirement, st.Icon, st.EnduranceCost, st.StealthRequirement,
st.OpeningRequirementType, st3.Id, st.AttackResultRequirement, st.WeaponTypeRequirement, st.GrowthRate, st.BonusToHit, st.BonusToDefense,
st.TwoHandAnimation,st.RandomProc, st.ArmorHitLocation, st.GrowthOffset, NOW(), NOW()
FROM atlas.Style st
INNER JOIN Newatlas.specializations sp ON st.SpecKeyName = sp.KeyName
INNER JOIN atlas.Style st2 ON st2.ID = st.OpeningRequirementValue AND st2.ClassId = st.ClassId
INNER JOIN newatlas.Styles st3 ON st3.ClassID = st2.ClassID AND st3.SpecKeyName = st2.speckeyname AND st3.Name = st2.Name
WHERE st.OpeningRequirementType = 0 AND st.OpeningRequirementValue > 0
AND NOT EXISTS (SELECT 1 FROM newatlas.styles st4 WHERE st4.ClassID = st.ClassID AND st4.SpecKeyName = st.SpecKeyName AND st4.Name = st.Name);

/* 3rd in chain */
INSERT INTO newAtlas.Styles (ClassID, SpecializationID, NAME, SpecKeyName, SpecLevelRequirement, Icon, EnduranceCost, StealthRequirement,
OpeningRequirementType, OpeningRequirementValue, AttackResultRequirement, WeaponTypeRequirement, GrowthRate, BonusToHit, BonusToDefense,
TwoHandAnimation,RandomProc, ArmorHitLocation, GrowthOffset, CreateDate, ModifyDate)
SELECT st.ClassID, sp.Id,st.NAME, st.SpecKeyName, st.SpecLevelRequirement, st.Icon, st.EnduranceCost, st.StealthRequirement,
st.OpeningRequirementType, st3.Id, st.AttackResultRequirement, st.WeaponTypeRequirement, st.GrowthRate, st.BonusToHit, st.BonusToDefense,
st.TwoHandAnimation,st.RandomProc, st.ArmorHitLocation, st.GrowthOffset, NOW(), NOW()
FROM atlas.Style st
INNER JOIN Newatlas.specializations sp ON st.SpecKeyName = sp.KeyName
INNER JOIN atlas.Style st2 ON st2.ID = st.OpeningRequirementValue AND st2.ClassId = st.ClassId
INNER JOIN newatlas.Styles st3 ON st3.ClassID = st2.ClassID AND st3.SpecKeyName = st2.speckeyname AND st3.Name = st2.Name
WHERE st.OpeningRequirementType = 0 AND st.OpeningRequirementValue > 0
AND NOT EXISTS (SELECT 1 FROM newatlas.styles st4 WHERE st4.ClassID = st.ClassID AND st4.SpecKeyName = st.SpecKeyName AND st4.Name = st.Name);

/* 4th in chain */
INSERT INTO newAtlas.Styles (ClassID, SpecializationID, NAME, SpecKeyName, SpecLevelRequirement, Icon, EnduranceCost, StealthRequirement,
OpeningRequirementType, OpeningRequirementValue, AttackResultRequirement, WeaponTypeRequirement, GrowthRate, BonusToHit, BonusToDefense,
TwoHandAnimation,RandomProc, ArmorHitLocation, GrowthOffset, CreateDate, ModifyDate)
SELECT st.ClassID, sp.Id,st.NAME, st.SpecKeyName, st.SpecLevelRequirement, st.Icon, st.EnduranceCost, st.StealthRequirement,
st.OpeningRequirementType, st3.Id, st.AttackResultRequirement, st.WeaponTypeRequirement, st.GrowthRate, st.BonusToHit, st.BonusToDefense,
st.TwoHandAnimation,st.RandomProc, st.ArmorHitLocation, st.GrowthOffset, NOW(), NOW()
FROM atlas.Style st
INNER JOIN Newatlas.specializations sp ON st.SpecKeyName = sp.KeyName
INNER JOIN atlas.Style st2 ON st2.ID = st.OpeningRequirementValue AND st2.ClassId = st.ClassId
INNER JOIN newatlas.Styles st3 ON st3.ClassID = st2.ClassID AND st3.SpecKeyName = st2.speckeyname AND st3.Name = st2.Name
WHERE st.OpeningRequirementType = 0 AND st.OpeningRequirementValue > 0
AND NOT EXISTS (SELECT 1 FROM newatlas.styles st4 WHERE st4.ClassID = st.ClassID AND st4.SpecKeyName = st.SpecKeyName AND st4.Name = st.Name);


/* Copy style spells */
INSERT INTO newatlas.stylespells (SpellID, ClassID, StyleID, Chance, CreateDate, ModifyDate)
SELECT sxs.SpellID, sxs.ClassID, st2.Id,sxs.Chance, NOW(),NOW() FROM atlas.stylexspell sxs
INNER JOIN atlas.style st1 ON st1.StyleID = sxs.StyleID
INNER JOIN newatlas.styles st2 ON st1.ClassID = st2.ClassID AND st1.SpecKeyName = st2.speckeyname AND st1.name = st2.Name;

/* Copy startup locations */
INSERT INTO newatlas.startuplocations (Xpos, YPos, ZPos, Heading, RegionID, MinVersion, Realm, RaceID, ClassID, ClientRegionID, CreateDate, ModifyDate)
SELECT Xpos, YPos, ZPos, Heading, Region, MinVersion, RealmID, RaceID, ClassID,
CASE WHEN ClientRegionID = 0 THEN NULL ELSE ClientRegionID END, 
NOW(),NOW()
FROM atlas.startuplocation WHERE Region IN (SELECT id FROM newatlas.regions);

/* Copy paths */
INSERT INTO newatlas.paths (PathName, PathType, RegionID, CreateDate, ModifyDate)
SELECT PathID, PathType, CASE WHEN RegionID = 0 THEN NULL ELSE RegionID END, NOW(),NOW() 
FROM atlas.path;

/* Copy Path Points */
INSERT INTO newatlas.pathpoints (PathID, Step, X,Y,Z,MaxSpeed, WaitTime, CreateDate, ModifyDate)
SELECT p2.Id, p1.Step, p1.X,p1.Y,p1.Z, p1.MaxSpeed, p1.WaitTime, NOW(),NOW()
FROM atlas.pathpoints p1
INNER JOIN newatlas.paths p2 ON p1.PathID = p2.PathName;


/* Copy NPC Equipments */
INSERT INTO newatlas.npcequipments (Slot, Model, Color, Effect, Extension, Emblem, EquipmentTemplateName,CreateDate, ModifyDate)
SELECT Slot,Model,Color,Effect,Extension, Emblem,TemplateID,NOW(),NOW() FROM atlas.npcequipment;

/* Copy NPC Templates */
ALTER TABLE newatlas.npctemplates ADD COLUMN OldId TEXT NULL;
ALTER TABLE newatlas.npctemplates	ADD INDEX `OldId` (`OldId`);
ALTER TABLE newatlas.spawngroups ADD INDEX `Name_RegionId` (`Name`, `RegionId`);

/*INSERT INTO newatlas.npctemplates (NAME, Suffix, ClassType, GuildName, ExamineArticle, MessageArticle, Model, Gender, Size, LEVEL, maxSpeed, ItemListName, Flags, 
MeleeDamageType, ParryChance,EvadeChance,BlockChance,LeftHandSwingChance,spells,styles,Strength,Constitution,Dexterity,Quickness,Intelligence,Piety, Charisma,
Empathy,abilities,Aggrolevel,aggrorange, RaceID,BodyType,MaxDistance,TetherRange,VisibleWeaponSlots,PackageID,EquipmentTemplateName,CreateDate,ModifyDate, OldId)
SELECT NAME, Suffix, ClassType, GuildName, ExamineArticle, MessageArticle, Model, Gender, Size, LEVEL, maxSpeed, ItemsListTemplateID, Flags, 
MeleeDamageType, ParryChance,EvadeChance,BlockChance,LeftHandSwingChance,spells,styles,Strength,Constitution,Dexterity,Quickness,Intelligence,Piety, Charisma,
Empathy,abilities,Aggrolevel,aggrorange, Race,BodyType,MaxDistance,TetherRange,VisibleWeaponSlots,PackageID, EquipmentTemplateID, NOW(),NOW(), TemplateId
FROM atlas.npctemplate;*/

INSERT INTO newatlas.npctemplates (NAME, Suffix, ClassType, GuildName, ExamineArticle, MessageArticle, Model, Gender, Size, LEVEL, maxSpeed, ItemListName, Flags, 
MeleeDamageType, ParryChance,EvadeChance,BlockChance,LeftHandSwingChance,spells,styles,Strength,Constitution,Dexterity,Quickness,Intelligence,Piety, Charisma,
Empathy,abilities,Aggrolevel,aggrorange, RaceID,BodyType,MaxDistance,TetherRange,VisibleWeaponSlots,PackageID,EquipmentTemplateName,
Brain, FactionID, HouseNumber, Realm, 
CreateDate,ModifyDate, OldId)
SELECT m.NAME, m.Suffix, m.ClassType, npc.GuildName, m.ExamineArticle, m.MessageArticle, m.Model, m.Gender, m.Size, npc.LEVEL, npc.MaxSpeed, npc.ItemsListTemplateID, m.Flags, 
m.MeleeDamageType, npc.ParryChance,npc.EvadeChance,npc.BlockChance,npc.LeftHandSwingChance,npc.spells,npc.styles,m.Strength,m.Constitution,m.Dexterity,m.Quickness,m.Intelligence,m.Piety, m.Charisma,
m.Empathy,abilities,m.Aggrolevel,m.aggrorange, m.Race,m.BodyType,m.MaxDistance,npc.TetherRange,m.VisibleWeaponSlots,m.PackageID, npc.EquipmentTemplateID,
m.Brain, m.FactionID, m.HouseNumber, m.Realm,
NOW(),NOW(), m.NPCTemplateId
FROM atlas.mob m
INNER JOIN atlas.npctemplate npc ON m.NPCTemplateID = npc.TemplateId;


/* Create Spawn Groups from Mob table */
INSERT INTO newatlas.spawngroups (ClassType, NAME, DayNightSpawn, RegionId, CreateDate, ModifyDate)
SELECT ClassType,NAME, 0,Region, NOW(),NOW() FROM atlas.mob 
WHERE Region IN (SELECT Id FROM newatlas.regions)
GROUP BY `NAME`, region;

/* Create NPC Spawn Group from Mob table */
INSERT INTO newatlas.npcspawngroup (NpcTemplateID, SpawnGroupID, SpawnChance, CreateDate, ModifyDate)
SELECT npcId, SpawnId, 100, NOW(), NOW() 
FROM (
	SELECT (SELECT MAX(id) FROM newatlas.npctemplates WHERE OldId = m.NPCTemplateID) AS npcId, sp.Id AS SpawnId
	FROM atlas.Mob m
	INNER JOIN newatlas.SpawnGroups sp ON m.Name = sp.Name AND m.region = sp.RegionID
	GROUP BY m.Name, m.Region, m.NPCTemplateID
) AS A
WHERE npcId IS NOT NULL;

/* Create Spawn Points from Mob table */
INSERT INTO newatlas.spawnpoints (RegionID, SpawnGroupID, PathID, X,Y,Z,Heading, AggroLevel, AggroRange, MaxDistance,
RoamingRange, RespawnInterval, IsCloakHoodUp, CreateDate, ModifyDate)
SELECT sp.RegionId,sp.Id,p.Id, m.X,m.Y,m.Z,m.Heading, m.AggroLevel, m.AggroRange, m.MaxDistance, m.RoamingRange, m.RespawnInterval,
m.IsCloakHoodUp,NOW(),NOW()
FROM atlas.Mob m
INNER JOIN newatlas.SpawnGroups sp ON m.Name = sp.Name AND m.region = sp.RegionID
LEFT JOIN newatlas.Paths p ON p.PathName = m.PathID
GROUP BY m.Name, m.Region, m.NPCTemplateID;

ALTER TABLE newatlas.npctemplates DROP OldId;




