// ============================================================
// Project                                :        CityRPG
// Author                                :        /Ty
// Description                                :        Sponsor Criminal Code file
// ============================================================

$CityRPG::jobs::name = "Sponsor Criminal";
$CityRPG::jobs::type = "sponsor";
$CityRPG::jobs::initialInvestment = 0;
$CityRPG::jobs::pay = 1000;
$CityRPG::jobs::tools = "gc_SniperRifleItem gc_PistolItem TacticalVestItem ShotgunItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 0;

$CityRPG::jobs::sellItems = true;
$CityRPG::jobs::sellFood = true;
$CityRPG::jobs::sellServices = true;
$CityRPG::jobs::sellClothes = true;

$CityRPG::jobs::law = false;
$CityRPG::jobs::usepolicecars = true;
$CityRPG::jobs::usecrimecars = true;
$CityRPG::jobs::canPardon = true;

$CityRPG::jobs::thief = true;
$CityRPG::jobs::hideJobName = true;

$CityRPG::jobs::offerer = true;
$CityRPG::jobs::claimer = true;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "0000CC";
$CityRPG::jobs::helpline = "\c6Can sell items, can use police and criminal cars, can pardon, & can thief.";

$CityRPG::jobs::outfit = "none none none none blueShirt blueShirt skin blackPants blackShoes smileyBlonde Mod-Suit";
