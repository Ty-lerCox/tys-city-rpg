// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Other Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGOtherBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Other Brick";
	
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
function CityRPGOtherBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{	
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			%cost = 500 * averageEdu(%client);
			messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");	
			messageClient(%client, '', "\c31 \c6- Clear your record! (\c3$" @ %cost @ "\c6)");
			messageClient(%client, '', "\c32 \c6- Vote to ban someone for 10mins.");
			messageClient(%client, '', "\c33 \c6- Cell Phone Manual.");
			messageClient(%client, '', "\c34 \c6- Set layout color.");
			messageClient(%client, '', "\c35 \c6- Displays Toggle.");

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
            serverCmdbuyErase(%client);
            return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			messageClient(%client, '', "\c6This feature has been removed.");
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
            servercmdcall(%client, %number, %override);
			return;
		}
		
		if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
            %client.stage = 1.2;
            messageClient(%client,'',"\c6Please choose from the following: <color:3F5D7D>DarkBlue\c6, <color:279B61>Green\c6, <color:008AB8>LightBlue\c6, <color:A3E496>LightGreen\c6, <color:993333>DarkRed\c6, <color:CC3333>Red\c6, <color:FFCC33>Yellow\c6, <color:CC6699>Pink");
			return;
		}
		
		if(strReplace(%input, "5", "") !$= %input || strReplace(%input, "five", "") !$= %input)
		{
            servercmddisplay(%client);
			return;
		}
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{			
			serverCmdOther(%client, %text);
            return;
		}
		
		if(%client.stage == 1.3)
		{			
			serverCmdvoteBan(%client, %text);
            return;
		}
		
		if(%client.stage == 1.2)
		{
			if(mFloor(%input) < 1)
			{
				serverCmdLayout(%client, %text);
				return;
			}
			
			if(CityRPGData.getData(%client.bl_id).valueMoney - mFloor(%input) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueMoney < 1)
				{
					messageClient(%client, '', "\c6You don't have that much money to deposit.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%input = CityRPGData.getData(%client.bl_id).valueMoney;
			}
			
			messageClient(%client, '', "\c6You have deposited \c3$" @ mFloor(%input) @ "\c6!");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueOther += mFloor(%input);
			CityRPGData.getData(%client.bl_id).valueMoney -= mFloor(%input);
			
			%client.SetInfo();
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