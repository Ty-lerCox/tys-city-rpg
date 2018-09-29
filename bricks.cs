//============================================================
// Project				:	CityRPG
// Author				:	Iban & Jookia & Ty(ID997) (Edited by Gadgethm)
// Description			:	Civilian Code file
// ============================================================
// Table of Contents
// 1. Handling Script Start
// 2. Lots
// 2.2 Sale Lots
// 3. Data Bricks
// 4. Spawns
// 5. Resources
// 6. Other
// 7. Decals
// 8. Drugs
// ============================================================

// ============================================================
// Section 1 : Handling Script Start
// ============================================================
$CityRPG::temp::brickError = forceRequiredAddOn("player_no_jet");

if($CityRPG::temp::brickError)
{
	if($CityRPG::temp::brickError == $error::addOn_disabled)
		playerNoJet.uiName = "";
	
	if($CityRPG::temp::brickError == $error::addOn_notFound)
		return;
}

$CityRPG::loadedDatablocks = true;

if(!$CityRPG::loadedDatablocks)
{
	return;
}

datablock triggerData(CityRPGLotTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};

datablock triggerData(CityRPGInputTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};


// ============================================================
// Section 2 : Lots
// ============================================================
//House Lots
datablock fxDTSBrickData(CityRPGSmallHouseLotBrickData : brick16x16FData)
 {
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16LotIcon";
	
	 category = "CityRPG";
	 subCategory = "House Lots";
	
	 uiName = "Small House";
	 lotType = "House";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 16 60";
	 trigger = 0;
	
	 initialPrice = 500;
	 taxAmount = 10;
};

datablock fxDTSBrickData(CityRPGHalfSmallHouseLotBrickData : brick16x32FData)
{
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32LotIcon";
	
	 category = "CityRPG";
	 subCategory = "House Lots";
	
	 uiName = "Half Medium House";
	 lotType = "House";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 32 60";
	 trigger = 0;
	
	 initialPrice = 750;
	 taxAmount = 15;
};

datablock fxDTSBrickData(CityRPGMediumHouseLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32LotIcon";
	
	category = "CityRPG";
	subCategory = "House Lots";
	
	uiName = "Medium House";
	lotType = "House";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "32 32 80";
	trigger = 0;
	
	initialPrice = 1500;
	taxAmount = 25;
};

datablock fxDTSBrickData(CityRPGLargeHouseLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64LotIcon";
	
	category = "CityRPG";
	subCategory = "House Lots";
	
	uiName = "Large House";
	lotType = "House";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "64 64 160";
	trigger = 0;
	
	initialPrice = 4500;
	taxAmount = 60;
};

 //Store Lots
 datablock fxDTSBrickData(CityRPGSmallStoreLotBrickData : brick16x16FData)
 {
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16LotIcon";
	
	 category = "CityRPG";
	 subCategory = "Store Lots";
	
	 uiName = "Small Store";
	 lotType = "Store";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 16 60";
	 trigger = 0;
	
	 initialPrice = 800;
	 taxAmount = 15;
 };

 datablock fxDTSBrickData(CityRPGHalfSmallStoreLotBrickData : brick16x32FData)
 {
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32LotIcon";
	
	 category = "CityRPG";
	 subCategory = "Store Lots";
	
	 uiName = "Half Medium Store";
	 lotType = "Store";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 32 4800";
	 trigger = 0;
	
	 initialPrice = 1300;
	 taxAmount = 20;
 };

datablock fxDTSBrickData(CityRPGMediumStoreLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32LotIcon";
	
	category = "CityRPG";
	subCategory = "Store Lots";
	
	uiName = "Medium Store";
	lotType = "Store";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "32 32 6400";
	trigger = 0;
	
	initialPrice = 2800;
	taxAmount = 30;
};

datablock fxDTSBrickData(CityRPGLargeStoreLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64LotIcon";
	
	category = "CityRPG";
	subCategory = "Store Lots";
	
	uiName = "Large Store";
	lotType = "Store";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "64 64 12800";
	trigger = 0;
	
	initialPrice = 5100;
	taxAmount = 70;
};

 //NoTax Store Lots
 datablock fxDTSBrickData(CityRPGSmallNoTaxStoreLotBrickData : brick16x16FData)
 {
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16LotIcon";
	
	 category = "CityRPG";
	 subCategory = "NoTax Store Lots";
	
	 uiName = "NoTax Small Store";
	 lotType = "Store";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 16 60";
	 trigger = 0;
	
	 initialPrice = 800*5;
	 taxAmount = 0;
 };

 datablock fxDTSBrickData(CityRPGHalfSmallNoTaxStoreLotBrickData : brick16x32FData)
 {
	 iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32LotIcon";
	
	 category = "CityRPG";
	 subCategory = "NoTax Store Lots";
	
	 uiName = "NoTax Half Medium Store";
	 lotType = "Store";
	
	 CityRPGBrickType = 1;
	 CityRPGBrickAdmin = false;
	
	 triggerDatablock = CityRPGLotTriggerData;
	 triggerSize = "16 32 4800";
	 trigger = 0;
	
	 initialPrice = 1300*5;
	 taxAmount = 0;
 };

datablock fxDTSBrickData(CityRPGMediumNoTaxStoreLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32LotIcon";
	
	category = "CityRPG";
	subCategory = "NoTax Store Lots";
	
	uiName = "NoTax Medium Store";
	lotType = "Store";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "32 32 6400";
	trigger = 0;
	
	initialPrice = 2800*5;
	taxAmount = 0;
};

datablock fxDTSBrickData(CityRPGLargeNoTaxStoreLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64LotIcon";
	
	category = "CityRPG";
	subCategory = "NoTax Store Lots";
	
	uiName = "NoTax Large Store";
	lotType = "Store";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CityRPGLotTriggerData;
	triggerSize = "64 64 12800";
	trigger = 0;
	
	initialPrice = 5100*5;
	taxAmount = 0;
};

// House Zone Lots
// datablock fxDTSBrickData(CityRPGSmallHouseZoneBrickData : brick16x16FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin House Zones";
	// lotType = "House";
	
	// uiName = "House Small Zone";

	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGSmallHouseLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGHalfSmallHouseZoneBrickData : brick16x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin House Zones";
	// lotType = "House";
	
	// uiName = "House Half-Small Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGHalfSmallLotZoneBrickData;
// };

// datablock fxDTSBrickData(CityRPGMediumHouseZoneBrickData : brick32x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin House Zones";
	// lotType = "House";
	
	// uiName = "House Medium Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGMediumHouseLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGLargeHouseZoneBrickData : brick64x64FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin House Zones";
	// lotType = "House";
	
	// uiName = "House Large Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGLargeHouseLotBrickData;
// };

// // Store Zone Lots
// datablock fxDTSBrickData(CityRPGSmallStoreZoneBrickData : brick16x16FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin Store Zones";
	// lotType = "Store";
	
	// uiName = "Store Small Zone";

	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGSmallStoreLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGHalfSmallStoreZoneBrickData : brick16x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin Store Zones";
	// lotType = "Store";
	
	// uiName = "Store Half-Small Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGHalfSmallLotZoneBrickData;
// };

// datablock fxDTSBrickData(CityRPGMediumStoreZoneBrickData : brick32x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin Store Zones";
	// lotType = "Store";
	
	// uiName = "Store Medium Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGMediumStoreLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGLargeStoreZoneBrickData : brick64x64FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "Admin Store Zones";
	// lotType = "Store";
	
	// uiName = "Store Large Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGLargeStoreLotBrickData;
// };

// // Industrial Lots
// datablock fxDTSBrickData(CityRPGSmallIndustrialLotBrickData : brick16x16FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16LotIcon";
	
	// category = "CityRPG";
	// subCategory = "Industrial Lots";
	
	// uiName = "Small Industrial";
	// lotType = "Industrial";
	
	// CityRPGBrickType = 1;
	// CityRPGBrickAdmin = false;
	
	// triggerDatablock = CityRPGLotTriggerData;
	// triggerSize = "16 16 4800";
	// trigger = 0;
	
	// initialPrice = 500;
	// taxAmount = 10;
// };

// datablock fxDTSBrickData(CityRPGHalfSmallIndustrialLotBrickData : brick16x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32LotIcon";
	
	// category = "CityRPG";
	// subCategory = "Industrial Lots";
	
	// uiName = "Half Medium Industrial";
	// lotType = "Industrial";
	
	// CityRPGBrickType = 1;
	// CityRPGBrickAdmin = false;
	
	// triggerDatablock = CityRPGLotTriggerData;
	// triggerSize = "16 32 4800";
	// trigger = 0;
	
	// initialPrice = 750;
	// taxAmount = 15;
// };

// datablock fxDTSBrickData(CityRPGMediumIndustrialLotBrickData : brick32x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32LotIcon";
	
	// category = "CityRPG";
	// subCategory = "Industrial Lots";
	
	// uiName = "Medium Industrial";
	// lotType = "Industrial";
	
	// CityRPGBrickType = 1;
	// CityRPGBrickAdmin = false;
	
	// triggerDatablock = CityRPGLotTriggerData;
	// triggerSize = "32 32 6400";
	// trigger = 0;
	
	// initialPrice = 1500;
	// taxAmount = 25;
// };

// datablock fxDTSBrickData(CityRPGLargeIndustrialLotBrickData : brick64x64FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64LotIcon";
	
	// category = "CityRPG";
	// subCategory = "Industrial Lots";
	
	// uiName = "Large Industrial";
	// lotType = "Industrial";
	
	// CityRPGBrickType = 1;
	// CityRPGBrickAdmin = false;
	
	// triggerDatablock = CityRPGLotTriggerData;
	// triggerSize = "64 64 12800";
	// trigger = 0;
	
	// initialPrice = 4500;
	// taxAmount = 60;
// };

// // Sale Lots
// datablock fxDTSBrickData(CityRPGSmallZoneBrickData : brick16x16FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "CityRPG Zones";
	
	// uiName = "Small Zone";

	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGSmallLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGHalfSmallZoneBrickData : brick16x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "CityRPG Zones";
	
	// uiName = "Half-Small Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGHalfSmallLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGMediumZoneBrickData : brick32x32FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "CityRPG Zones";
	
	// uiName = "Medium Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGMediumLotBrickData;
// };

// datablock fxDTSBrickData(CityRPGLargeZoneBrickData : brick64x64FData)
// {
	// iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64ZoneIcon";
	
	// category = "CityRPG";
	// subCategory = "CityRPG Zones";
	
	// uiName = "Large Zone";
	
	// CityRPGBrickAdmin = true;
	// CityRPGMatchingLot = CityRPGLargeLotBrickData;
// };


// ============================================================
// Section 3 : Data Bricks
// ============================================================
exec("./bricks/info/bank.cs");
exec("./bricks/info/police.cs");
exec("./bricks/info/bounty.cs");
exec("./bricks/info/labor.cs");
exec("./bricks/info/realestate.cs");
exec("./bricks/info/criminalbank.cs");
exec("./bricks/info/atm.cs");
exec("./bricks/info/playeratm.cs");
exec("./bricks/info/education.cs");
exec("./bricks/info/job.cs");
exec("./bricks/info/stats.cs");
exec("./bricks/info/drugsell.cs");
exec("./bricks/info/donate.cs");
exec("./bricks/info/family.cs");
exec("./bricks/info/vote.cs");
exec("./bricks/info/other.cs");
exec("./bricks/info/lottery.cs");
exec("./bricks/info/storage.cs");
exec("./bricks/info/business.cs");
exec("./bricks/info/gangs.cs");
exec("./bricks/info/LaborPlus.cs");
exec("./bricks/info/pets.cs");
exec("./bricks/info/city.cs");

// ============================================================
// Section 4 : Spawns
// ============================================================
datablock fxDtsBrickData(CityRPGPersonalSpawnBrickData : brickSpawnPointData)
{
	category = "CityRPG";
	subCategory = "CityRPG Spawns";
	
	uiName = "Personal Spawn";
	
	specialBrickType = "";
	
	CityRPGBrickType = 3;
	CityRPGBrickAdmin = false;
	
	spawnData = "personalSpawn";
};

datablock fxDtsBrickData(CityRPGJailSpawnBrickData : brickSpawnPointData)
{
	category = "CityRPG";
	subCategory = "CityRPG Spawns";
	
	uiName = "Jail Spawn";
	
	specialBrickType = "";
	
	CityRPGBrickType = 3;
	CityRPGBrickAdmin = true;
	
	spawnData = "jailSpawn";
};

// ============================================================
// Section 5 : Resources
// ============================================================
exec("./bricks/resources/tree.cs");
exec("./bricks/resources/ore.cs");
exec("./bricks/resources/smallore.cs");
exec("./bricks/resources/safe.cs");
exec("./bricks/resources/vault.cs");

// ============================================================
// Section 6 : Other
// ============================================================

datablock fxDTSBrickData(CityRPGPermaSpawnData : brick2x2FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Permaspawn Brick";

	CityRPGBrickAdmin = true;
	CityRPGPermaspawn = 1;
};

datablock fxDTSBrickData(CityRPGPoliceVehicleData : brickVehicleSpawnData)
{
	category = "CityRPG";
	subCategory = "CityRPG Spawns";
	uiName = "Police Vehicle Spawn";
	CityRPGBrickAdmin = true;
};

datablock fxDTSBrickData(CityRPGCrimeVehicleData : brickVehicleSpawnData)
{
	category = "CityRPG";
	subCategory = "CityRPG Spawns";
	uiName = "Crime Vehicle Spawn";
	CityRPGBrickAdmin = true;
};

datablock fxDTSBrickData(CityRPGParaVehicleData : brickVehicleSpawnData)
{
	category = "CityRPG";
	subCategory = "CityRPG Spawns";
	uiName = "Paramedic Vehicle Spawn";
	CityRPGBrickAdmin = true;
};

DoorSO.addDoorTypeFromFile("Add-Ons/Gamemode_TysCityRPG/bricks/doors/unbreakable.cs");
doorBrickUnbreakable.unbreakable = true;
doorBrickUnbreakable.CityRPGBrickAdmin = true;

// ============================================================
// Section 7 : Decals
// ============================================================
datablock decalData(CityRPGLogo)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/decals/CityRPGLogo.Gad's";
	preload = true;
};

datablock decalData(CityRPG_Lot_Icon_1)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16LotIcon";
	preload = true;
};

datablock decalData(CityRPG_Lot_Icon_2)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32LotIcon";
	preload = true;
};

datablock decalData(CityRPG_Lot_Icon_3)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32LotIcon";
	preload = true;
};

datablock decalData(CityRPG_Lot_Icon_4)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64LotIcon";
	preload = true;
};

datablock decalData(CityRPG_Zone_Icon_1)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16ZoneIcon";
	preload = true;
};

datablock decalData(CityRPG_Zone_Icon_2)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32ZoneIcon";
	preload = true;
};

datablock decalData(CityRPG_Zone_Icon_3)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32ZoneIcon";
	preload = true;
};

datablock decalData(CityRPG_Zone_Icon_4)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64ZoneIcon";
	preload = true;
};

datablock decalData(CityRPG_Ore_Icon)
{
	textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/4x Cube";
	preload = true;
};

// ============================================================
// Section 8 : Drugs
// ============================================================
exec("./bricks/drugs/marijuana.cs");
exec("./bricks/drugs/opium.cs");
exec("./bricks/drugs/speed.cs");
exec("./bricks/drugs/steroid.cs");