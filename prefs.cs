// ============================================================
// Project				:	CityRPG
// Author				:	Iban  & /Ty(ID997) (Edited by Gadgethm)
// Description			:	Server Preferences
// ============================================================
// Table of Contents
// 1. Basic Preferences
// 2. Price Preferences
// 2.1 Weapon Price Preferences
// 3. Tick Preferences
// 4. Demerit Preferences
// 5. Vehicle Preferences
// 7. Allowed Inmate Items
// 8. Food
// 9. Banned Events
// 10. Hunger Colors and States
// ============================================================

// ============================================================
// Section 1 : Random Preferences
// ============================================================
$CityRPGVersion = 1;
$CityRPG::pref::misc::cashdrop = 1;

$CityRPG::pref::demerits::pardonCost = 1000;
$CityRPG::pref::demerits::recordShredCost = 5000;
$CityRPG::pref::demerits::demeritCost = 1.4;

$CityRPG::pref::demerits::demoteLevel = 400;
$CityRPG::pref::demerits::wantedLevel = 75;

$CityRPG::pref::demerits::reducePerTick = 25;
$CityRPG::pref::demerits::minBounty = 100;
$CityRPG::pref::demerits::maxBounty = 7500;

$CityRPG::pref::resources::tree::regrowTime = 2.5;
$CityRPG::pref::resources::ore::noOreColorID = 49;
$CityRPG::pref::resources::ore::hasOreColorID = 51;

$CityRPG::pref::realestate::maxLots = 500;

//new edits

$Admin::Color = "<color:99ffff>";
$ExpCost = 1.5;
$Announcer::Relay = 175;
$Game::Speed::Cost = 2000;
$Game::Steroid::Cost = 2000;
$Game::PayCheck::Max = 2500;
$ATM::Min = 25;
$ATM::Max = 300;
$ATM::Demerits = 500;
$Economics::Relay = 2;
$Economics::Greatest = 100;
$Economics::Least = -35;
$Economics::Cap = 150;
$Mayor::Cost = 500;
$Mayor::ImpeachCost = 100;
$Mayor::Time = 10;
$Game::Divide = 75;
$Safe::Small::Max = 150;


///////////

$CityRPG::pref::giveDefaultTools = true;
$CityRPG::pref::defaultTools = "hammerItem wrenchItem printGun";

$CityRPG::pref::moneyDieTime = 9999999999;

$CityRPG::pref::players::startingCash = 50;

// ATM Hacking
$CityRPG::pref::hack::education = 3;
$CityRPG::pref::hack::demerits = 1000;
$CityRPG::pref::hack::stealmin = 100;
$CityRPG::pref::hack::stealmax = 1000;
$CityRPG::pref::hack::revivetime = 5; //minutes

// ============================================================
// Section 2 : Price Preferences
// ============================================================
$CityRPG::prices::vehicleSpawn = 1500;
$CityRPG::prices::jailingBonus = 100;
$CityRPG::prices::reset = 100;
$CityRPG::prices::resourcePrice = 1;

// Section 2.1 - Weapon Prices
$CityRPG::prices::weapon::name[$CityRPG::guns] = "keyItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 5;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 0;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "PillItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 30;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "taserItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 40;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "crowbarItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 42;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_AssaultRifleItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 200;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_AssaultRifleSDItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 220;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "C4Item";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 300;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_PistolItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 80;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_PistolSDItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 100;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_RevolverItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 100;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "TacticalVestItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 120;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_BattleRifleItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 250;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_BattleRifleSDItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 280;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "ShotgunItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 260;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_SniperRifleItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 450;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "butterflyknifeItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 100;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_MGItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 600;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_SMGItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 180;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_SMGSDItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 185;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_UziItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 140;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;

$CityRPG::prices::weapon::name[$CityRPG::guns] = "gc_UziSDItem";
$CityRPG::prices::weapon::price[$CityRPG::guns] = 160;
$CityRPG::prices::weapon::mineral[$CityRPG::guns++] = 1;



$ListAmt = 21; // has to equal to the amount of items on the list

$CityRPG::guns = "";

// Section 2.2 - vehicle Prices

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "YachtVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 4000;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "TugBoatVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 3000;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "SailBoatVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 3800;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "BullittVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1500;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "NewJeepOffroadVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 900;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "NewJeepRoofVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 900;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "minisportscarVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1500;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "JeepVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 400;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "HorseArmor";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 100;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "CVPIVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1200;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "citybusvehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 2000;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "ChargerDaytonaVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1000;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "ChargerVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1000;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "boxtruckVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 1500;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "BlockstangVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 800;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "BlockoCarVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 750;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "BlockoSedanVehicle";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 750;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "BikeArmor";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 100;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$CityRPG::prices::vehicle::name[$CityRPG::vehicles] = "Beach_BikeArmor";
$CityRPG::prices::vehicle::price[$CityRPG::vehicles] = 100;
$CityRPG::prices::vehicle::mineral[$CityRPG::vehicles++] = 0;

$vehicleListAmt = 19; // has to equal to the amount of items on the list

$CityRPG::vehicles = "";

// this more of a manual away of doing it, but it's a sure-fire way for it to work

//$CityRPG::prices::weapon::name[1] = "M1911Item";
//$CityRPG::prices::weapon::price[1] = 10000;
//$CityRPG::prices::weapon::mineral[1] = 10; // the key just doesn't work.. I don't even think it's in the mod version you have. You'd have to make one yourself

// $CityRPG::prices::weapon::name[1] = "M9Items";
// $CityRPG::prices::weapon::price[1] = 112;
// $CityRPG::prices::weapon::mineral[1] = 10;

// $CityRPG::prices::weapon::name[2] = "taserItem";
// $CityRPG::prices::weapon::price[2] = 50;
// $CityRPG::prices::weapon::mineral[2] = 5;

// $CityRPG::prices::weapon::name[3] = "G17Item";
// $CityRPG::prices::weapon::price[3] = 300;
// $CityRPG::prices::weapon::mineral[3] = 20;

// $CityRPG::prices::weapon::name[4] = "M1911Item";
// $CityRPG::prices::weapon::price[4] = 10000;
// $CityRPG::prices::weapon::mineral[4] = 30;

// $CityRPG::prices::weapon::name[5] = "AK74MItem";
// $CityRPG::prices::weapon::price[5] = 500;
// $CityRPG::prices::weapon::mineral[5] = 30;

// $CityRPG::prices::weapon::name[6] = "M16Items";
// $CityRPG::prices::weapon::price[6] = 650;
// $CityRPG::prices::weapon::mineral[6] = 30;

// $CityRPG::prices::weapon::name[7] = "Hk416Item";
// $CityRPG::prices::weapon::price[7] = 600;
// $CityRPG::prices::weapon::mineral[7] = 30;

// $CityRPG::prices::weapon::name[8] = "AN94Item";
// $CityRPG::prices::weapon::price[8] = 450;
// $CityRPG::prices::weapon::mineral[8] = 30;

// $CityRPG::prices::weapon::name[9] = "M9Items";
// $CityRPG::prices::weapon::price[9] = 100;
// $CityRPG::prices::weapon::mineral[9] = 30;

// $CityRPG::prices::weapon::name[10] = "SCARLItems";
// $CityRPG::prices::weapon::price[10] = 600;
// $CityRPG::prices::weapon::mineral[10] = 30;

// $CityRPG::prices::weapon::name[11] = "SCARItem";
// $CityRPG::prices::weapon::price[11] = 460;
// $CityRPG::prices::weapon::mineral[11] = 30;

// $CityRPG::prices::weapon::name[12] = "M4Items";
// $CityRPG::prices::weapon::price[12] = 550;
// $CityRPG::prices::weapon::mineral[12] = 30;

// $CityRPG::prices::weapon::name[13] = "AKs74uItem";
// $CityRPG::prices::weapon::price[13] = 400;
// $CityRPG::prices::weapon::mineral[13] = 30;

// $CityRPG::prices::weapon::name[14] = "SG553Item";
// $CityRPG::prices::weapon::price[14] = 580;
// $CityRPG::prices::weapon::mineral[14] = 30;

// $CityRPG::prices::weapon::name[15] = "G36Items";
// $CityRPG::prices::weapon::price[15] = 475;
// $CityRPG::prices::weapon::mineral[15] = 30;

// $CityRPG::prices::weapon::name[16] = "G53Items";
// $CityRPG::prices::weapon::price[16] = 425;
// $CityRPG::prices::weapon::mineral[16] = 30;

// $CityRPG::prices::weapon::name[17] = "grappleRope";
// $CityRPG::prices::weapon::price[17] = 125;
// $CityRPG::prices::weapon::mineral[17] = 10;

// $CityRPG::prices::weapon::name[18] = "C4Item";
// $CityRPG::prices::weapon::price[18] = 500;
// $CityRPG::prices::weapon::mineral[18] = 10;

// $CityRPG::prices::weapon::name[19] = "TacticalVestItem";
// $CityRPG::prices::weapon::price[19] = 200;
// $CityRPG::prices::weapon::mineral[19] = 10;

// $ListAmt = 19; // has to equal to the amount of items on the list

// $CityRPG::guns = "";

//When adding to this index, be sure to add a forceRequiredAddon("Item_Here");
//in server.cs, or else the item mod will be broken.

// ============================================================
// Section 3 : Tick Preferences
// ============================================================
$CityRPG::tick::interest = 1.00;
$CityRPG::tick::creditInterest = 1.000;
$CityRPG::tick::interestTick = 999;
$CityRPG::tick::speed = 3;
$CityRPG::tick::promotionLevel = 24;

// ============================================================
// Section 3.5 : Drug Preferences
// ============================================================
// Misc Drug Prefs
$CityRPG::drug::minSellSpeed = 1; // In seconds
$CityRPG::drug::maxSellSpeed = 4; // In seconds
$CityRPG::drug::minBuyAmt = 1; // Minimum grams of weed player is capable of selling // Grams 1 - 5 have special names
$CityRPG::drug::maxBuyAmt = 5; // Maximum ^
$CityRPG::drug::sellPrice = 10; // About the real value of a gram of weed in the US // The actual price randomly changes by a couple digits
$CityRPG::drug::maxdrugplants = 99;
$CityRPG::drug::sellTimes = 50;
$CityRPG::drug::demWorth = 3; // The amount of dems each gram is worth. If their grams are worth the wanted limit or higher, they can be jailed.

// Drug Color Prefs
$CityRPG::drug::startcolor = 45;
$CityRPG::drug::emittertype = GrassEmitter;

// Drug Evidence Prefs
$CityRPG::drug::evidenceWorth = 1000; // How much someone can turn in drug evidence for

// Drug Types
// -Marijuana
$CityRPG::drugs::marijuana::placePrice = 1800; // How much it costs to plant the brick
$CityRPG::drugs::marijuana::harvestMin = 9; //9 Amount of grams you get from harvest
$CityRPG::drugs::marijuana::harvestMax = 14; //14
$CityRPG::drugs::marijuana::growthTime = 8; //8 In minutes
$CityRPG::drugs::marijuana::basePrice = getRandom(9,11); // Price per gram
// -Speed
$CityRPG::drugs::Speed::placePrice = $Game::Speed::Cost; // How much it costs to plant the brick
$CityRPG::drugs::Speed::harvestMin = 9; // Amount of grams you get from harvest
$CityRPG::drugs::Speed::harvestMax = 14;
$CityRPG::drugs::Speed::growthTime = 8; // In minutes
$CityRPG::drugs::Speed::basePrice = getRandom(3,5); // Price per gram
// -Steroid
$CityRPG::drugs::Steroid::placePrice = $Game::Steroid::Cost; // How much it costs to plant the brick
$CityRPG::drugs::Steroid::harvestMin = 9; // Amount of grams you get from harvest
$CityRPG::drugs::Steroid::harvestMax = 14;
$CityRPG::drugs::Steroid::growthTime = 8; // In minutes
$CityRPG::drugs::Steroid::basePrice = getRandom(3,5); // Price per gram
// -Opium
$CityRPG::drugs::opium::placePrice = 3000;
$CityRPG::drugs::opium::harvestMin = 11;
$CityRPG::drugs::opium::harvestMax = 18;
$CityRPG::drugs::opium::growthTime = 8;
$CityRPG::drugs::opium::basePrice = getRandom(12,16);

// ============================================================
// Section 4 : Demerit Preferences
// ============================================================
$CityRPG::demerits::hittingInnocents = 5;
$CityRPG::demerits::attemptedMurder = 15;
$CityRPG::demerits::murder = 100;
$CityRPG::demerits::breakingAndEntering = 10;
$CityRPG::demerits::attemptedBnE = 5;
$CityRPG::demerits::bountyPlacing = 250;
$CityRPG::demerits::bountyClaiming = 500;
$CityRPG::demerits::pickpocketing = 25;
$CityRPG::demerits::bankRobbery = 3000;
$CityRPG::demerits::tasingBros = 25;
$CityRPG::demerits::grandTheftAuto = 75;

// ============================================================
// Section 5 : Vehicle Preferences
// ============================================================
$CityRPG::vehicles::allowSpawn = true;

$CityRPG::vehicles::banned[0] = "FlyingWheeledJeepVehicle";
$CityRPG::vehicles::banned[1] = "MiniJetVehicle";
$CityRPG::vehicles::banned[2] = "StuntPlaneVehicle";
$CityRPG::vehicles::banned[3] = "BiplaneVehicle";
$CityRPG::vehicles::banned[4] = "MagicCarpetVehicle";
$CityRPG::vehicles::banned[5] = "TankVehicle";
$CityRPG::vehicles::banned[6] = "horseArmor";
$CityRPG::vehicles::banned[7] = "BlackhawkVehicle";
$CityRPG::vehicles::banned[7] = "TankTurretVehicle";

// ============================================================
// Section 7 : Allowed Inmate Items
// ============================================================
$CityRPG::demerits::jail::image["CityRPGLumberjackImage"] = true;
$CityRPG::demerits::jail::image["CityRPGShovelImage"] = true;
$CityRPG::demerits::jail::image["CityRPGPickaxeImage"] = true;

// Inmate Spawn Items
$CityRPG::demerits::jail::item[0] = "CityRPGPickaxeItem";
$CityRPG::demerits::jail::item[1] = "CityRPGLumberjackItem";

// ============================================================
// Section 8 : Food
// ============================================================
$CityRPG::portion[1] = "Small";
$CityRPG::portion[2] = "Medium";
$CityRPG::portion[3] = "Large";
$CityRPG::portion[4] = "Extra-Large";
$CityRPG::portion[5] = "Super-Sized";
$CityRPG::portion[6] = "Americanized";

// // ============================================================
// // Section 9 : Banned Events
// // ============================================================
// // $CityRPG::bannedEvent[nameSpace, name] = true; results in ban
// // Note: This system is archaic.

// // AIPlayer
// $CityRPG::bannedEvent["AIPlayer", "setPlayerScale"] = true;

// // fxDTSBrick
// $CityRPG::bannedEvent["fxDTSBrick", "spawnExplosion"] = true;
// $CityRPG::bannedEvent["fxDTSBrick", "spawnProjectile"] = true;

// // gameConnection
// $CityRPG::bannedEvent["gameConnection", "bottomPrint"] = true;
// $CityRPG::bannedEvent["gameConnection", "incScore"] = true;
// $CityRPG::bannedEvent["gameConnection", "instantRespawn"] = true;

// // Player
// $CityRPG::bannedEvent["Player", "addHealth"] = true;
// $CityRPG::bannedEvent["Player", "addVelocity"] = true;
// $CityRPG::bannedEvent["Player", "changeDatablock"] = true;
// $CityRPG::bannedEvent["Player", "clearTools"] = true;
// $CityRPG::bannedEvent["Player", "kill"] = true;
// $CityRPG::bannedEvent["Player", "setHealth"] = true;
// $CityRPG::bannedEvent["Player", "setPlayerScale"] = true;
// $CityRPG::bannedEvent["Player", "spawnExplosion"] = true;
// $CityRPG::bannedEvent["Player", "spawnProjectile"] = true;
// $CityRPG::bannedEvent["Player", "setVelocity"] = true;

// // Minigame
// $CityRPG::bannedEvent["MinigameSO", "chatMsgAll"] = true;
// $CityRPG::bannedEvent["MinigameSO", "centerPrintAll"] = true;
// $CityRPG::bannedEvent["MinigameSO", "bottomPrintAll"] = true;

// ============================================================
// Section 10 : Hunger Colors and States
// ============================================================
$CityRPG::food::stateCount = 0;

$CityRPG::food::color[$CityRPG::food::stateCount++] = "FF0000";
$CityRPG::food::state[$CityRPG::food::stateCount] = "10<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "FD2000";
$CityRPG::food::state[$CityRPG::food::stateCount] = "20<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "FF5900";
$CityRPG::food::state[$CityRPG::food::stateCount] = "30<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "FD7E00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "40<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "FD7E00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "50<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "F7FD00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "60<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "C6FF00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "70<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "7EFD00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "80<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "73FF00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "90<color:ffffff>%";

$CityRPG::food::color[$CityRPG::food::stateCount++] = "00FF00";
$CityRPG::food::state[$CityRPG::food::stateCount] = "100<color:ffffff>%";

$testingstatus = 0;

$growticks = 0;

// ===============================================================
// Section:12 Reg Prefs
// ===============================================================
if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	
	// City Prefs
	RTB_registerPref("Game | Font", "Ty's CityRPG", "$Extras::Font", "string 255", "Gamemode_TysCityRPG", $Extras::Font, 0, 0);
	RTB_registerPref("Game | Vehicle Damage", "Ty's CityRPG", "$Game::VehicleDamage", "string 255", "Gamemode_TysCityRPG", $Game::VehicleDamage, 0, 0);
	RTB_registerPref("Game | Cash Drop", "Ty's CityRPG", "$CityRPG::pref::misc::cashdrop", "int 0 1", "Gamemode_TysCityRPG", $CityRPG::pref::misc::cashdrop, 0, 0);
	RTB_registerPref("Game | Lumber Drop", "Ty's CityRPG", "$CityRPG::pref::misc::lumberdrop", "int 0 1", "Gamemode_TysCityRPG", $CityRPG::pref::misc::lumberdrop, 0, 0);
	RTB_registerPref("Game | Max Lots", "Ty's CityRPG", "$CityRPG::pref::realestate::maxLots", "int 0 9999999", "Gamemode_TysCityRPG", $CityRPG::pref::realestate::maxLots, 0, 0);
	RTB_registerPref("Game | Tick Speed", "Ty's CityRPG", "$CityRPG::tick::speed", "int 0 10", "Gamemode_TysCityRPG", $CityRPG::tick::speed, 0, 0);
	RTB_registerPref("Game | Reset Cost", "Extras CityRPG", "$CityRPG::prices::reset", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::prices::reset, 0, 0);
	RTB_registerPref("Game | Jailing Bonus", "Extras CityRPG", "$CityRPG::prices::jailingBonus", "int 0 150", "Gamemode_TysCityRPG", $CityRPG::prices::jailingBonus, 0, 0);
	RTB_registerPref("Game | Start Cash", "Extras CityRPG", "$CityRPG::pref::players::startingCash", "int 0 1000", "Gamemode_TysCityRPG", $CityRPG::pref::players::startingCash, 0, 0);
	RTB_registerPref("Game | Max Lots", "Extras CityRPG", "$CityRPG::pref::realestate::maxLots", "int 0 9999999", "Gamemode_TysCityRPG", $CityRPG::pref::realestate::maxLots, 0, 0);
	RTB_registerPref("Game | Pardon Cost", "Extras CityRPG", "$CityRPG::pref::demerits::pardonCost", "int 0 1000", "Gamemode_TysCityRPG", $CityRPG::pref::demerits::pardonCost, 0, 0);
	RTB_registerPref("Game | Lumber Divide", "Extras CityRPG", "$Game::Divide", "int 0 1000", "Gamemode_TysCityRPG", $Game::Divide, 0, 0);
	RTB_registerPref("Announcer | Relay", "Ty's CityRPG", "$Announcer::Relay", "int 0 1000", "Gamemode_TysCityRPG", $Announcer::Relay, 0, 0);
	RTB_registerPref("Announcer | Message", "Ty's CityRPG", "$Announcer::Message", "string 255", "Gamemode_TysCityRPG", $Announcer::Message, 0, 0);
	RTB_registerPref("Drugs | Speed Cost", "Ty's CityRPG", "$Game::Speed::Cost", "int 0 10000", "Gamemode_TysCityRPG", $Game::Speed::Cost, 0, 0);
	RTB_registerPref("Drugs | Steroid Cost", "Ty's CityRPG", "$Game::Steroid::Cost", "int 0 10000", "Gamemode_TysCityRPG", $Game::Steroid::Cost, 0, 0);
	RTB_registerPref("ATM | Min", "Ty's CityRPG", "$ATM::Min", "int 0 10000", "Gamemode_TysCityRPG", $ATM::Min, 0, 0);
	RTB_registerPref("ATM | Max", "Ty's CityRPG", "$ATM::Max", "int 0 10000", "Gamemode_TysCityRPG", $ATM::Max, 0, 0);
	RTB_registerPref("ATM | Demerits", "Ty's CityRPG", "$ATM::Demerits", "int 0 10000", "Gamemode_TysCityRPG", $ATM::Demerits, 0, 0);
	RTB_registerPref("Safe | SMax", "Ty's CityRPG", "$Safe::Small::Max", "int 0 10000", "Gamemode_TysCityRPG", $Safe::Small::Max, 0, 0);
	RTB_registerPref("Economic | Relay", "Ty's CityRPG", "$Economics::Relay", "int 0 50", "Gamemode_TysCityRPG", $Economics::Relay, 0, 0);
	RTB_registerPref("Economic | Max", "Ty's CityRPG", "$Economics::Greatest", "int -500 500", "Gamemode_TysCityRPG", $Economics::Greatest, 0, 0);
	RTB_registerPref("Economic | Min", "Ty's CityRPG", "$Economics::Least", "int -500 500", "Gamemode_TysCityRPG", $Economics::Least, 0, 0);
    RTB_registerPref("Economic | Cap", "Ty's CityRPG", "$Economics::Cap", "int -5000 5000", "Gamemode_TysCityRPG", $Economics::Cap, 0, 0);
    RTB_registerPref("Mayor | Voting", "Ty's CityRPG", "$Mayor::Voting", "int 0 1", "Gamemode_TysCityRPG", $Mayor::Voting, 0, 0);
	RTB_registerPref("Mayor | Active", "Ty's CityRPG", "$Mayor::Active", "int 0 1", "Gamemode_TysCityRPG", $Mayor::Active, 0, 0);
    RTB_registerPref("Mayor | Current", "Ty's CityRPG", "$Mayor::Current", "string 255", "Gamemode_TysCityRPG", $Mayor::Current, 0, 0);
    RTB_registerPref("Mayor | VP", "Ty's CityRPG", "$Mayor::VP", "string 255", "Gamemode_TysCityRPG", $Mayor::VP, 0, 0);
    RTB_registerPref("Mayor | Cost", "Ty's CityRPG", "$Mayor::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Mayor::Cost, 0, 0);
    RTB_registerPref("Mayor | Pay", "Ty's CityRPG", "$Mayor::Pay", "int 0 50000", "Gamemode_TysCityRPG", $Mayor::Pay, 0, 0);
    RTB_registerPref("Mayor | Relay", "Ty's CityRPG", "$Mayor::Relay", "int 0 50000", "Gamemode_TysCityRPG", $Mayor::Relay, 0, 0);
    RTB_registerPref("Mayor | Impeach Cost", "Ty's CityRPG", "$Mayor::ImpeachCost", "int 0 50000", "Gamemode_TysCityRPG", $Mayor::ImpeachCost, 0, 0);
    RTB_registerPref("Mayor | Time", "Ty's CityRPG", "$Mayor::Time", "int 0 50000", "Gamemode_TysCityRPG", $Mayor::Time, 0, 0);
    RTB_registerPref("Extra | Admin Death", "Ty's CityRPG", "$Extras::AdminDeathDrop", "int 0 1", "Gamemode_TysCityRPG", $Extras::AdminDeathDrop, 0, 0);
	RTB_registerPref("Crime | Rape", "Extras CityRPG", "$Rape::Demerits", "int 0 1000", "Gamemode_TysCityRPG", $Rape::Demerits, 0, 0);
	RTB_registerPref("Crime | Min Bounty", "Extras CityRPG", "$CityRPG::pref::demerits::minBounty", "int 0 1000", "Gamemode_TysCityRPG", $CityRPG::pref::demerits::minBounty, 0, 0);
	RTB_registerPref("Crime | Max Bounty", "Extras CityRPG", "$CityRPG::pref::demerits::maxBounty", "int 0 1000000", "Gamemode_TysCityRPG", $CityRPG::pref::demerits::maxBounty, 0, 0);
	RTB_registerPref("Crime | Demerit Cost", "Extras CityRPG", "$CityRPG::pref::demerits::demeritCost", "int 0 10", "Gamemode_TysCityRPG", $CityRPG::pref::demerits::demeritCost, 0, 0);
	RTB_registerPref("Crime | Hitting Innocents", "Extras CityRPG", "$CityRPG::demerits::hittingInnocents", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::hittingInnocents, 0, 0);
	RTB_registerPref("Crime | Attempted Murder", "Extras CityRPG", "$CityRPG::demerits::attemptedMurder", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::attemptedMurder, 0, 0);
	RTB_registerPref("Crime | Murder", "Extras CityRPG", "$CityRPG::demerits::murder", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::murder, 0, 0);
	RTB_registerPref("Crime | Breaking & Entering", "Extras CityRPG", "$CityRPG::demerits::breakingandentering", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::breakingandentering, 0, 0);
	RTB_registerPref("Crime | Tasing Innocents", "Extras CityRPG", "$CityRPG::demerits::tasingBros", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::tasingBros, 0, 0);
	RTB_registerPref("Crime | Placeing Bounty", "Extras CityRPG", "$CityRPG::demerits::bountyPlacing", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::bountyPlacing, 0, 0);
	RTB_registerPref("Crime | Claiming Bounty", "Extras CityRPG", "$CityRPG::demerits::bountyClaiming", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::bountyClaiming, 0, 0);
	RTB_registerPref("Crime | Pick Pocket", "Extras CityRPG", "$CityRPG::demerits::pickpocketing", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::pickpocketing, 0, 0);
	RTB_registerPref("Crime | Garnd Theft Auto", "Extras CityRPG", "$CityRPG::demerits::grandtheftauto", "int 0 5000", "Gamemode_TysCityRPG", $CityRPG::demerits::grandtheftauto, 0, 0);
	//RTB_registerPref("Brick | Vote", "Extras CityRPG", "$Bricks::Vote::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::Vote::Cost, 0, 0);
	//RTB_registerPref("Brick | Bounty", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Other", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Education", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Job", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Stats", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Police", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Realestate", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Donate", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
	//RTB_registerPref("Brick | Family", "Extras CityRPG", "$Bricks::::Cost", "int 0 50000", "Gamemode_TysCityRPG", $Bricks::::Cost, 0, 0);
//<font:BrowalliaUPC:33>$CityRPG::pref::demerits::maxBounty
}