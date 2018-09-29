// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	LaborPlus Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGLaborPlusBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "LaborPlus Brick";
	
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
function CityRPGLaborPlusBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{	
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c3Welcome! \c6Please type the number corresponding to the options below.");
			messageClient(%client, '', "\c31 \c6- Sell all resources.");
			//messageClient(%client, '', "\c32 \c6- Buy lumber. (Cost each: \c3$" @ getLumberCost(%client) @ "\c6)");

			%client.stage = 0;
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			messageClient(%client, '', "\c6Thanks, come again.");
			
			%client.stage = "";
		}
		
		return;
	}
	
	%input = strLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			messageClient(%client, '', "\c6You've sold your resources!");
            %client.buyResources();
			return;
		}
		
		//if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		//{
			//messageClient(%client, '', "\c6Please enter the amount of lumber you'd like to buy:  (Cost each: $" @ getLumberCost() @ ")");
            //%client.stage = 1.1;
			//return;
         //}
        
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	%input = strLwr(%text);
	
    if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			buyLumber(%client,%text);
            return;
		}
		
		return;
	}
}