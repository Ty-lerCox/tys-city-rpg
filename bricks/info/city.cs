// ============================================================
// Project				:	CityRPG
// Author				:	Iban & Jookia & Ty(ID997)
// Description			:	City Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGCityBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "City Brick";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function resetAccessableCity(%client)
{
    %client.AccessableCity = 0;
}

function CityRPGCityBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%client.getWantedLevel())
	{
		messageClient(%client, '', "\c6The service refuses to serve you.");
		return;
	}
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', " ");
			messageClient(%client, '', "\c6Welcome to the City.");
			messageClient(%client, '', "\c3Set Citizenship:");
			messageClient(%client, '', "\c31 \c6- Island 1");
			messageClient(%client, '', "\c32 \c6- Island 2");
			messageClient(%client, '', "\c33 \c6- Island 3");
			
			%client.stage = 0;
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			messageClient(%client, '', "\c6Goodbye..");
			
			%client.stage = "";
		}
		
		return;
	}
	
	%input = strLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{			
			City(%client, 1);
			return;
		}

		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			City(%client, 2);
			return;
        }
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			City(%client, 3);
			return;
        }
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			if(mFloor(%input) < 1)
			{
				messageClient(%client, '', "\c6Error.");
				
				return;
			}
			
			if(CityRPGData.getData(%client.bl_id).valueBank - mFloor(%input) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueCity < 1)
				{
					messageClient(%client, '', "\c6Insufficient funds.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%input = CityRPGData.getData(%client.bl_id).valueCity;
			}
			
			messageClient(%client, '', "\c6You have withdrawn \c3$" @ mFloor(%input) @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueBank -= mFloor(%input);
			CityRPGData.getData(%client.bl_id).valueMoney += mFloor(%input);
			
			%client.SetInfo();
		}
		return;
	}
}