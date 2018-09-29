// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	JobPlus Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGJobPlusBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "JobPlus Brick";
	
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
function CityRPGJobPlusBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{	
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");	
			messageClient(%client, '', "\c31 \c6- View JobPlus List.");
			messageClient(%client, '', "\c32 \c6- Apply for JobPlus.");

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
            %client.stage = 1.2;
			serverCmdhelp(%client, "Jobs");
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
            %client.stage = 1.1;

			messageClient(%client, '', "\c6Job name:");
			return;
		}
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			if(firstWord(%text) $= "moderator")
				%JobPlus2 = "mod";
			if(firstWord(%text) $= "donator")
				%JobPlus2 = "donator";
			if(firstWord(%text) $= "sponsor")
				%JobPlus2 = "sponsor";
			serverCmdJobPluss(%client, %text, %JobPlus2, %JobPlus3, %JobPlus4, %JobPlus5);
            return;
		}
		
		if(%client.stage == 1.2)
		{
			serverCmdhelp(%client, "Jobs", %text);
            return;
		}
		
		return;
	}
	
	if(mFloor(%client.stage) == 2)
	{
		if(%client.stage == 2.0)
		{
			messageClient(%client, '', "\c6Are you looking to \c3store\c6 or \c3take\c6 an item?");
			return;
		}
		else if(%client.stage == 2.1)
		{
			messageClient(%client, '', "\c6Please enter in the ID matching the Item you want to store.");
			
			for(%a = 0; %a < %client.player.getDatablock().maxTools; %a++)
			{
				%tool = %client.player.tool[%a];
				
				if(isObject(%tool))
				{
					messageClient(%client, '', "\c3" @ %a @ "\c6 - \c3" @ %tool.uiName);
				}
				
				return;
			}
		}
	}
}