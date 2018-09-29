// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Medical-Inturn Code file
// ============================================================

$CityRPG::jobs::name = "Surgeon";
$CityRPG::jobs::type = "medic";
$CityRPG::jobs::initialInvestment = 2500;
$CityRPG::jobs::pay = 250;
$CityRPG::jobs::tools = "PillItem PillItem TF2MedigunItem TF2UbersawItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 20;

$CityRPG::jobs::sellItems = false;
$CityRPG::jobs::sellFood = false;
$CityRPG::jobs::sellServices = false;

$CityRPG::jobs::law = false;
$CityRPG::jobs::useparacars = true;
$CityRPG::jobs::canPardon = false;

$CityRPG::jobs::thief = false;
$CityRPG::jobs::hideJobName = false;

$CityRPG::jobs::offerer = false;
$CityRPG::jobs::claimer = false;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "E50000";
$CityRPG::jobs::helpline = "\c3Surgeons are the beast of them all, drive paramedic vehicles, doesn't require law, and you get a nice pay.";

$CityRPG::jobs::outfit = "none none none none whiteShirt whiteShirt skin whitePants whiteShoes default DrKleiner";