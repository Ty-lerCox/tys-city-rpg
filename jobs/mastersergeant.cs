// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Master Sergeant Code file
// ============================================================

$CityRPG::jobs::name = "Master Sergeant";
$CityRPG::jobs::type = "law";
$CityRPG::jobs::initialInvestment = 1500;
$CityRPG::jobs::pay = 150;
$CityRPG::jobs::tools = "TacticalVestItem CityRPGPlayerBatonItem CityRPGBrickBatonItem gc_PistolItem ShotgunItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 15;

$CityRPG::jobs::sellItems = false;
$CityRPG::jobs::sellFood = false;
$CityRPG::jobs::sellServices = false;

$CityRPG::jobs::law = true;
$CityRPG::jobs::usepolicecars = true;
$CityRPG::jobs::canPardon = false;

$CityRPG::jobs::thief = false;
$CityRPG::jobs::hideJobName = false;

$CityRPG::jobs::offerer = true;
$CityRPG::jobs::claimer = true;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "0000CC";
$CityRPG::jobs::helpline = "\c3Medium rank in the Army. Kimbos at spawn. Require clean record.";

$CityRPG::jobs::outfit = "none none none none greenShirt greenShirt skin blackPants blueShoes BrownSmiley default";