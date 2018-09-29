// ============================================================
// Project				:	CityRPG
// Author				:	Moppy
// Description			:	Marijuana Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGMariData)
{
	brickFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Bricks/brick_6x6.blb";
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/cannabis";
	
	category = "CityRPG";
	subCategory = "CityRPG Drugs";
	
	uiName = "Marijuana";
	drugType = "Marijuana";
	
	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "GrassEmitter";
	isDrug = true;
	hasDrug = false;
	isMarijuana = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;
	
	watered = 0;
	grew = 0;
	
	price = $CityRPG::drugs::marijuana::placePrice;
	
	harvestAmt = $CityRPG::drugs::marijuana::harvestAmt;
	growthTime = $CityRPG::drugs::marijuana::growthTime;
	
	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};