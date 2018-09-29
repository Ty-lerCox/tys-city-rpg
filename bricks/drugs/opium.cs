// ============================================================
// Project				:	CityRPG
// Author				:	Moppy
// Description			:	Opium Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGOpiumData)
{
	brickFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Bricks/brick_6x6.blb";
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/opium";
	
	category = "CityRPG";
	subCategory = "CityRPG Drugs";
	
	uiName = "Opium";
	drugType = "Opium";
	
	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "GrassEmitter";
	isDrug = true;
	hasDrug = false;
	isOpium = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;
	
	watered = 0;
	grew = 0;
	
	price = $CityRPG::drugs::Opium::placePrice;
	
	harvestAmt = $CityRPG::drugs::Opium::harvestAmt;
	growthTime = $CityRPG::drugs::Opium::growthTime;
	
	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};