// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Storage Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGbrickVaultData)
{
	category = "CityRPG";
	subCategory = "CityRPG Resources";
	iconName = "Add-Ons\Gamemode_TysCityRPG\bricks\resources\ImgSafe.png";
	brickFile = "Add-Ons/Gamemode_TysCityRPG/bricks/resources/Vault.blb";
	uiName = "Vault";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	CityRPGBrickPlayerPrivliage = true;
	CityRPGBrickCost = 300;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "5 5 0";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================