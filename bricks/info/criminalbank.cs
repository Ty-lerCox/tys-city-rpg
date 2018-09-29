// ============================================================
// Project				:	CityRPG
// Author				:	Iban & Jookia & Ty(ID997)
// Description			:	Criminal Bank Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGCriminalBankBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Criminal Bank Brick";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGCriminalBankBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.(Criminal Bank Brick)");
			if(CityRPGData.getData(%client.bl_id).valueBank > 0)
			{
				messageClient(%client, '', "\c6- You have \c3$" @ CityRPGData.getData(%client.bl_id).valueBank SPC "\c6in the bank.");
			}
			
			messageClient(%client, '', "\c31 \c6- Withdraw money.");
			messageClient(%client, '', "\c32 \c6- Deposit money.");
			messageClient(%client, '', "\c33 \c6- Deposit all money.");
						
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
			%client.stage = 1.1;
			
			messageClient(%client, '', "\c6Please enter the amount of money you wish to withdraw.");
			
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			%client.stage = 1.2;
			
			messageClient(%client, '', "\c6Please enter the amount of money you wish to deposit.");
			
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			%client.stage = 1.2;
			
			serverCmdMessageSent(%client, CityRPGData.getData(%client.bl_id).valueMoney);
			
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
				messageClient(%client, '', "\c6Please enter a valid amount of money to withdraw.");
				
				return;
			}
			
			if(CityRPGData.getData(%client.bl_id).valueBank - mFloor(%input) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueBank < 1)
				{
					messageClient(%client, '', "\c6You don't have that much money in the bank to withdraw.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%input = CityRPGData.getData(%client.bl_id).valueBank;
			}
			
			messageClient(%client, '', "\c6You have withdrawn \c3$" @ mFloor(%input) @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueBank -= mFloor(%input);
			CityRPGData.getData(%client.bl_id).valueMoney += mFloor(%input);
			
			%client.SetInfo();
		}
		
		if(%client.stage == 1.2)
		{
			if(mFloor(%input) < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of money to deposit.");
				
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
			
			CityRPGData.getData(%client.bl_id).valueBank += mFloor(%input);
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