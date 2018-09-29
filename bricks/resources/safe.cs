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
datablock fxDTSBrickData(CityRPGbrickSSafeData)
{
	category = "CityRPG";
	subCategory = "CityRPG Resources";
	iconName = "Add-Ons\Gamemode_TysCityRPG\bricks\resources\ImgSafe.png";
	brickFile = "Add-Ons/Gamemode_TysCityRPG/bricks/resources/Safe.blb";
	uiName = "Safe";
	
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
function CityRPGbrickSSafeData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	%group = %brick.getGroup();
	%item = CityRPGData.getData(%client.bl_id).valueSSafeItem;	
	%itemname = trim(%item.UIname);
	%trust = getTrustLevel(%client,%group);
	%itemexist = isObject(%item);
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c6Your safe" SPC %item);
			messageClient(%client, '', "\c6- You have \c3$" @ CityRPGData.getData(%client.bl_id).valueSSafe @ "\c6/\c3$" @ $Safe::Small::Max SPC "\c6in the safe.");
			
			messageClient(%client, '', "\c31 \c6- Take money.");
			messageClient(%client, '', "\c32 \c6- Store money.");
			messageClient(%client, '', "\c33 \c6- Store all money.");
			if(%item !$= "")
				MessageClient(%client,'', "\c34 \c6- Take Item. [\c3"@ CityRPGData.getData(%client.bl_id).valueSSafeItem @ "\c6]");
			else
				MessageClient(%client,'', "\c34 \c6- Store Item.");
			messageClient(%client, '', "\c35 \c6- Credits.");
						
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
		
		if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
			if(%item !$= "")
				{
					for(%i = 0; %i < 9; %i++)
					{
						if(!%client.player.tool[%i])
						{
							%client.player.tool[%i] = nameToId(%item);
							messageClient(%client,'MsgItemPickup','',%i,nameToId(%item));
							CityRPGData.getData(%client.bl_id).valueSSafeItem = "";
							if(isObject(%group.client))
								MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has withdrawn a \c3"@ %itemname @"\c6 out of your safe.");
							return;
						}
					}
					MessageClient(%client,'',"\c6You did not have an open slot for a \c3"@ %itemname @"\c6.");
					return;
				}
				else
				{
					%client.stage = 1.3;
					MessageClient(%client,'', "\c6Please enter the item you wish to deposit.");
					for(%i = 0; %i < 9; %i++)
					{
						if(%client.player.tool[%i])
							MessageClient(%client,'',"\c3"@ %i+1 @"\c6 - "@ %client.player.tool[%i].uiName);
					}
					return;
				}
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
			
			if(CityRPGData.getData(%client.bl_id).valueSSafe - mFloor(%input) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueSSafe < 1)
				{
					messageClient(%client, '', "\c6You don't have that much money in the safe to withdraw.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%input = CityRPGData.getData(%client.bl_id).valueSSafe;
			}
			
			messageClient(%client, '', "\c6You have withdrawn \c3$" @ mFloor(%input) @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueSSafe -= mFloor(%input);
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
			
			if((%input+CityRPGData.getData(%client.bl_id).valueSSafe) > $Safe::Small::Max)
			{
				messageClient(%client, '', "\c6You are only allowed to place \c3$" @ $Safe::Small::Max SPC"\c6in this safe type.");
				return;
			}
			messageClient(%client, '', "\c6You have deposited \c3$" @ mFloor(%input) @ "\c6!");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueSSafe += mFloor(%input);
			CityRPGData.getData(%client.bl_id).valueMoney -= mFloor(%input);
			
			%client.SetInfo();
		}
		
		if(%client.stage == 1.3)
		{
			%input -= 1;
			if(!isObject(%client.player.tool[%input]))
			{
				MessageClient(%client,'', "\c6Please enter a valid item to store.");
				return;
			}
			MessageClient(%client,'', "\c6You have stored a \c3" @ trim(%client.player.tool[%input].UIname) @ "\c6 into the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3" @ %client.name @ "\c6 has stored a \c3" @ trim(%client.player.tool[%input].UIname) @ "\c6 into your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CityRPGData.getData(%client.bl_id).valueSSafeItem = %client.player.tool[%input].getName();
			%client.player.tool[%input] = 0;
			messageClient(%client, 'MsgItemPickup', "", %input, 0);
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

		}
	}
}