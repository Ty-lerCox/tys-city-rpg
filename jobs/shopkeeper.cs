// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Shop Keeper Code file
// ============================================================

$CityRPG::jobs::name = "Shop Keeper";
$CityRPG::jobs::type = "shop";
$CityRPG::jobs::initialInvestment = 100;
$CityRPG::jobs::pay = 50;
$CityRPG::jobs::tools = "";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 5;

$CityRPG::jobs::sellItems = true;
$CityRPG::jobs::sellFood = true;
$CityRPG::jobs::sellServices = false;
$CityRPG::jobs::sellClothes = true;

$CityRPG::jobs::law = false;
$CityRPG::jobs::canPardon = false;

$CityRPG::jobs::thief = false;
$CityRPG::jobs::hideJobName = false;

$CityRPG::jobs::offerer = false;
$CityRPG::jobs::claimer = false;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "0066CC";
$CityRPG::jobs::helpline = "\c3Shop Keepers can sell food and items to other people.";

$CityRPG::jobs::outfit = "none none none none greenShirt greenShirt skin bluePants blackShoes memeHappy worm_engineer";