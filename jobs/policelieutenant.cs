// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Policeman Code file
// ============================================================

$CityRPG::jobs::name = "Police Lieutenant";
$CityRPG::jobs::type = "law";
$CityRPG::jobs::initialInvestment = 95;
$CityRPG::jobs::pay = 45;
$CityRPG::jobs::tools = "CityRPGPlayerBatonItem CityRPGBrickBatonItem taserItem gc_PistolItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 4;

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
$CityRPG::jobs::helpline = "\c3Police Lietenatns can arrest criminals, but require a clean record.";

$CityRPG::jobs::outfit = "none copHat none none copShirt copShirt skin blackPants blackShoes default Mod-Police";