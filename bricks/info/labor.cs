// ============================================================
// Project				:	peopleRP
// Author				:	Iban
// Description			:	Bounty Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGLaborBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Labor Brick";
	 	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGLaborBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	%client.buyResources();
}