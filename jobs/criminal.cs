// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Criminal Code file
// ============================================================

$CityRPG::jobs::name = "Criminal";
$CityRPG::jobs::type = "crim";
$CityRPG::jobs::initialInvestment = 50;
$CityRPG::jobs::pay = 25;
$CityRPG::jobs::tools = "CityRPGPicklockItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 1;

$CityRPG::jobs::sellItems = false;
$CityRPG::jobs::sellFood = false;
$CityRPG::jobs::sellServices = false;

$CityRPG::jobs::law = false;
$CityRPG::jobs::canPardon = false;
$CityRPG::jobs::usecrimecars = true;

$CityRPG::jobs::thief = true;
$CityRPG::jobs::hideJobName = true;

$CityRPG::jobs::offerer = false;
$CityRPG::jobs::claimer = false;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "FF0000";
$CityRPG::jobs::helpline = "\c3Criminals can pickpocket people and break open doors.";

$CityRPG::jobs::outfit = "none none none none blackShirt blackShirt skin blackPants blackShoes smileyEvil1 hoodie";