// ============================================================
// Project                                :        CityRPG
// Author                                :        /Ty
// Description                        :        Sponsor Cop Code file
// ============================================================

$CityRPG::jobs::name = "Sponsor Cop";
$CityRPG::jobs::type = "sponsor";
$CityRPG::jobs::initialInvestment = 0;
$CityRPG::jobs::pay = 1000;
$CityRPG::jobs::tools = "TacticalVestItem CityRPGPlayerBatonItem CityRPGBrickBatonItem gc_PistolItem ShotgunItem gc_SniperRifleItem";
$CityRPG::jobs::datablock = Player9SlotPlayer;
$CityRPG::jobs::education = 0;

$CityRPG::jobs::sellItems = true;
$CityRPG::jobs::sellFood = true;
$CityRPG::jobs::sellServices = true;

$CityRPG::jobs::law = true;
$CityRPG::jobs::usepolicecars = true;
$CityRPG::jobs::canPardon = true;

$CityRPG::jobs::thief = false;
$CityRPG::jobs::hideJobName = false;

$CityRPG::jobs::offerer = true;
$CityRPG::jobs::claimer = true;

$CityRPG::jobs::labor = false;

$CityRPG::jobs::tmHexColor = "0000CC";
$CityRPG::jobs::helpline = "\c6All the perks of being police chief + can sell items, and can pardon.";

$CityRPG::jobs::outfit = "none copHat none none copShirt copShirt skin blackPants blackShoes default Mod-Police";
