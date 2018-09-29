// ============================================================
// Project				:	CityRPG
// Author				:	Ty
// Description			:	Steroid Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGSteroidData)
{
	brickFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Bricks/brick_6x6.blb";
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/mushroom";
	
	category = "CityRPG";
	subCategory = "CityRPG Drugs";
	
	uiName = "Steroid";
	drugType = "Steroid";
	
	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "ArrowVanishEmitter";
	isDrug = true;
	hasDrug = false;
	isSteroid = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;
	
	watered = 0;
	grew = 0;
	
	price = 4500;

	harvestAmt = $CityRPG::drugs::Steroid::harvestAmt;
	growthTime = $CityRPG::drugs::Steroid::growthTime;
	
	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};