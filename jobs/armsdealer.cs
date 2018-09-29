// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Arms Dealer Code file
// ============================================================

$CityRPG::jobs::name = "Arms Dealer";
$CityRPG::jobs::type = "shop";
$CityRPG::jobs::initialInvestment = 70;
$CityRPG::jobs::pay = 35;
$CityRPG::jobs::tools = "";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 3;

// $CityRPG::jobs::shopExp		= 0;
// $CityRPG::jobs::lawExp		= 0;
// $CityRPG::jobs::medicExp	= 0;
// $CityRPG::jobs::crimExp		= 0;
// $CityRPG::jobs::justiceExp	= 0;

$CityRPG::jobs::sellItems = true;
$CityRPG::jobs::sellFood = false;
$CityRPG::jobs::sellServices = false;
$CityRPG::jobs::sellClothes = false;

$CityRPG::jobs::law = false;
$CityRPG::jobs::canPardon = false;

$CityRPG::jobs::thief = false;
$CityRPG::jobs::hideJobName = false;

$CityRPG::jobs::offerer = false;
$CityRPG::jobs::claimer = false;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "AAAAAA";
$CityRPG::jobs::helpline = "\c3Arms Dealers can sell items (specifically weapons) to other people.";

$CityRPG::jobs::outfit = "none none none none blackShirt blackShirt skin blackPants blackShoes smileyPirate3 Alyx";