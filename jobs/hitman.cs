// ============================================================
// Project				:	CityRPG
// Author				:	/Ty
// Description			:	Bounty Hunter Code file
// ============================================================

$CityRPG::jobs::name = "Hitman";
$CityRPG::jobs::type = "crim";
$CityRPG::jobs::initialInvestment = 1500;
$CityRPG::jobs::pay = 150;
$CityRPG::jobs::tools = "gc_PistolItem gc_SniperRifleItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 15;

$CityRPG::jobs::sellItems = false;
$CityRPG::jobs::sellFood = false;
$CityRPG::jobs::sellServices = false;

$CityRPG::jobs::law = false;
$CityRPG::jobs::canPardon = false;

$CityRPG::jobs::thief = true;
$CityRPG::jobs::hideJobName = true;

$CityRPG::jobs::offerer = false;
$CityRPG::jobs::claimer = true;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "FF7F00";
$CityRPG::jobs::helpline = "\c3The Hitman can arrest criminals and claim bounties, but don't need a good record.";

$CityRPG::jobs::outfit = "none none none none greyShirt greyShirt skin greyPants blueShoes default Mod-Suit";