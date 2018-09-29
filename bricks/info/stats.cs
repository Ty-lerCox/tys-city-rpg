// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Stats Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGStatsBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Stats Brick";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	CityRPGBrickPlayerPrivliage = true;

	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGStatsBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			serverCmdstats(%client, %name);
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			messageClient(%client, '', "\c6Thanks, come again.");
			
			%client.stage = "";
		}
		
		return;
	}
	
	%input = strLwr(%text);
	
}