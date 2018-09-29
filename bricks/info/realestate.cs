// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Real Estate Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGREBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Real Estate Brick";
	
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
function CityRPGREBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true)
		{
			messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");
			
			messageClient(%client, '', "\c31 \c6- View Lots");
			messageClient(%client, '', "\c32 \c6- Buy Lot");
			messageClient(%client, '', "\c33 \c6- Sell Lot");
			messageClient(%client, '', "\c34 \c6- Cancel Sale");
			messageClient(%client, '', "\c35 \c6- Set Citizenship");
			
			%client.stage = 0;
			%client.pickedLot = "";
		}
		
		if(%triggerStatus == false)
		{
			messageClient(%client, '', "\c6Thanks, come again.");
			
			%client.stage = "";
			%client.pickedLot = "";
		}
		
		return;
	}
	
	%input = strLwr(%text);
	
	if(%client.stage == 2.1)
	{
		%found = false;
		
		for(%a = 1; %a <= CityRPGData.dataCount; %a++)
		{
			if(getWord(CityRPGData.data[%a].valueLotData, 0) == 0 || CityRPGData.data[%a].ID == %client.bl_id)
			{
				continue;
			}
			
			%data = CityRPGData.data[%a].valueLotData;
			
			for(%b = 1; %b < getWordCount(%data); %b++)
			{
				if(%b % 2 == 0)
					continue;
				
				if(strLwr(getWord(%data, %b)) $= %input)
				{
					%found = true;
					%newBrick = "_" @ getWord(%data, %b);
					%newBrickID = %newBrick.getID();
					%price = getWord(%data, %b + 1);
					%owner = getBrickGroupFromObject(%newBrickID);
				}
			}
		}
		
		if(!%found)
		{
			messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
			
			return;
		}
		
		if(CityRPGData.getData(%client.bl_id).valueMoney < %price)
		{
			messageClient(%client, '', "\c6You need at least \c3$" @ %price SPC "\c6to buy this lot.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			return;
		}
		
		messageClient(%client, '', "\c3You have bought \c3" @ getSubStr(%newBrick, 1, strLen(%newBrick) - 1) SPC "\c6from \c3" @ %owner.name @ "\c6.");
		
		if(isObject(%owner.client))
		{
			messageClient(%owner.client, '', "\c3" @ %client.name SPC "\c6has bought \c3" @ getSubStr(%newBrick, 1, strLen(%newBrick) - 1) SPC "\c6from you.");
		}
		
		CityRPGData.getData(%client.bl_id).valueMoney -= %price;
		CityRPGData.getData(%owner.bl_id).valueMoney += %price;
		
		%newBrickID.transferLot(%client.brickGroup);
		%newBrickID.setName("");
		
		%client.brickGroup.lotsOwned++;
		%owner.lotsOwned--;
		
		%client.brickGroup.taxes += %newBrickID.getDatablock().taxAmount;
		%owner.taxes -= %newBrickID.getDatablock().taxAmount;
		
		%client.setInfo();
		%owner.client.setInfo();
		
		for(%a = 0; %a < %client.brickGroup.getCount(); %a++)
		{
			%foundBrick = %client.brickGroup.getObject(%a);
			%foundBrick.tran = NULL;
		}
		
		%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
		
		return;
	}
	
	if(%client.stage == 3.1)
	{
		if(%input $= "")
		{
			return;
		}
		
		%picked = false;
		
		%brickGroup = %client.brickGroup;
		
		for(%a = 0; %a < %brickGroup.getCount(); %a++)
		{
			if(%brickGroup.getObject(%a).getDatablock().CityRPGBrickType == 1)
			{
				if(%brickGroup.getObject(%a).getName() $= "")
				{
					continue;
				}
				
				%brick = %brickGroup.getObject(%a);
				
				if(strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)) $= %input)
				{
					if(strReplace(strLwr(CityRPGData.getData(%client.bl_id).valueLotData), strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)), "") !$= strLwr(CityRPGData.getData(%client.bl_id).valueLotData))
					{
						continue;
					}
					
					%picked = true;
					
					%client.pickedBrick = %brickGroup.getObject(%a);
				}
			}
		}
		
		if(!%picked)
		{
			messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
			
			return;
		}
		
		%client.stage = 3.2;
		
		messageClient(%client, '', "\c6Please enter the amount you wish to sell the lot for.");
		
		return;
	}
	
	if(%client.stage == 3.2)
	{
		%price = mFloor(%input);
		
		if(%price < 1)
		{
			messageClient(%client, '', "\c6Please enter a valid price!");
			
			return;
		}
		
		messageClient(%client, '', "\c6Your offer has been put up.");
		
		if(getWord(CityRPGData.getData(%client.bl_id).valueLotData, 0) > 0)
		{
			%rest = getWords(CityRPGData.getData(%client.bl_id).valueLotData, 1, getWordCount(CityRPGData.getData(%client.bl_id).valueLotData) - 1) @ " ";
		}
		
		CityRPGData.getData(%client.bl_id).valueLotData = getWord(CityRPGData.getData(%client.bl_id).valueLotData, 0) + 1 SPC %rest @ getSubStr(%client.pickedBrick.getName(), 1, strLen(%client.pickedBrick.getName()) - 1) SPC %price;
		
		%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
		
		return;
	}
	
	if(%client.stage == 4.1)
	{
		if(%input $= "")
		{
			return;
		}
		
		%picked = false;
		
		%data = CityRPGData.getData(%client.bl_id).valueLotData;
		
		%newData = getWord(%data, 0) - 1;
		
		for(%a = 1; %a < getWordCount(%data); %a++)
		{
			if(%cancelNext)
			{
				%cancelNext = false;
				
					continue;
			}
			
			%newData = %newData SPC getWord(%data, %a);
			
			if(%a % 2 == 0)
			{
				continue;
			}
			
			if(strLwr(getWord(%data, %a)) $= %input)
			{
				%picked = true;
				
				%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
			
					%cancelNext = true;
			}
		}
			
		if(!%picked)
		{
			messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
			
		return;
	}
		
	messageClient(%client, '', "\c6Your offer has been withdrawn.");
		
		CityRPGData.getData(%client.bl_id).valueLotData = %newData;
		
		%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
		
		return;
	}
	
	if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
	{
		%found = false;
	
		for(%a = 1; %a <= CityRPGData.dataCount; %a++)
		{
			if(getWord(CityRPGData.data[%a].valueLotData, 0) == 0)
			{
				continue;
			}
			
			%data = CityRPGData.data[%a].valueLotData;
			
			for(%b = 1; %b < getWordCount(%data); %b++)
			{
				if(%b % 2 == 0)
				{
					continue;
				}
				
				%newBrick = "_" @ getWord(%data, %b);
				
				if(isObject(%newBrick))
				{
					%found = true;
					
					messageClient(%client, '', "\c3" @ getWord(%data, %b) @ "\c6, \c3" @ %newBrick.getDatablock().uiName SPC "\c6- Price: \c3$" @ getWord(%data, %b + 1) SPC "\c6Owner: \c3" @ ("brickGroup_" @ CityRPGData.data[%a].ID).name);
				}
			}
		}
		
		if(!%found)
		{
			messageClient(%client, '', "\c6There are no lots to display.");
		}
		
		%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
		
		return;
	}
	
	if((strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input))
	{
		if(%client.brickGroup.lotsOwned < $CityRPG::pref::realestate::maxLots)
		{
			messageClient(%client, '', "\c6Please enter the lot which you wish to buy.");
			
			%client.stage = 2.1;
			
			return;
		}
		else
		{
			messageClient(%client, '', "\c6You already own enough lots.");
			%client.stage = 0;
			return;
		}
	}
	
	if((strReplace(%input, "3", "") !$= %input || strReplace(%input, "two", "") !$= %input))
	{
		%nameLecture = true;
		%brick404 = true;
		
		%brickGroup = %client.brickGroup;
		
		for(%a = 0; %a < %brickGroup.getCount(); %a++)
		{
			if(%brickGroup.getObject(%a).getDatablock().CityRPGBrickType == 1)
			{
				%brick = %brickGroup.getObject(%a);
				
				if(strReplace(strLwr(CityRPGData.getData(%client.bl_id).valueLotData), strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)), "") !$= strLwr(CityRPGData.getData(%client.bl_id).valueLotData))
				{
					continue;
				}
				
				%brick404 = false;
			
				if(%brickGroup.getObject(%a).getName() !$= "")
				{
					%brick = %brickGroup.getObject(%a);
					
					%nameLecture = false;
					
					messageClient(%client, '', "\c6" @ getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1) SPC "\c6- \c3" @ %brick.getDatablock().uiName);
				}
			}
		}
		
		if(%brick404)
		{
			messageClient(%client, '', "\c6There are no lots to display.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			return;
		}
		
		if(%nameLecture)
		{
			messageClient(%client, '', "\c6You must give a name to one of your lots!");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			return;
		}
		
		messageClient(%client, '', "\c6Please enter the name of the lot you wish to sell.");
		
		%client.stage = 3.1;
		
		return;
	}
	
	if((strReplace(%input, "4", "") !$= %input || strReplace(%input, "two", "") !$= %input))
	{
		%data = CityRPGData.getData(%client.bl_id).valueLotData;
		
		%found = false;
		
		for(%a = 1; %a < getWordCount(%data); %a++)
		{
			if(%a % 2 == 0)
			{
				continue;
			}
			
			%found = true;
			
			if(isObject("_" @ getWord(%data, %a)))
			{
				%brick = "_" @ getWord(%data, %a);
				
				messageClient(%client, '', "\c6" @ getWord(%data, %a) SPC "\c6- \c3" @ %brick.getDatablock().uiName);
			}
			else
			{
				messageClient(%client, '', "\c6" @ getWord(%data, %a) SPC "\c6- \c3Unknown Lot Type");
			}
		}
		
		if(!%found)
		{
			messageClient(%client, '', "\c6There are no lots to display.");
			
			return;
		}
		
		messageClient(%client, '', "\c6Please enter the name of the lot you wish to withdraw from sales.");
		
		%client.stage = 4.1;
		
		return;
	}
	
	if((strReplace(%input, "5", "") !$= %input || strReplace(%input, "five", "") !$= %input))
	{
		
		return;
	}
	
	messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
	
	return;
}

package CityRPG_RealEsate
{
	function fxDtsBrick::setName(%brick, %name)
	{
		%oldName = %brick.getName();
		
		if(isObject(%name) && (%brick.getDatablock().CityRPGBrickType == 1 || %name.getDatablock().CityRPGBrickType == 1))
		{
			%a = 1;
			
			while(isObject(%name @ %a))
			{
				%a++;
			}
			
			%name = %name @ %a;
		}
		
		parent::setName(%brick, %name);
		
		if(%name !$= "")
		{
			for(%a = 1; %a <= CityRPGData.dataCount; %a++)
			{
				if(getWord(CityRPGData.data[%a].valueLotData, 0) == 0)
				{
					continue;
				}
				
				%data = CityRPGData.data[%a].valueLotData;
				
				for(%b = 1; %b < getWordCount(%data); %b++)
				{
					if(%b % 2 == 0)
					{
						continue;
					}
					
					if(strLwr(getWord(%data, %b)) $= strLwr(getSubStr(%name, 1, strLen(%name) - 1)))
					{
						parent::setName(%brick, "");
						
						return;
					}
				}
			}
		}
		
		if(%oldName !$= "")
		{
			if(%brick.getName() $= "")
			{
				%data = CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData;
				
				%newData = getWord(%data, 0) - 1;
				
				for(%a = 1; %a < getWordCount(%data); %a++)
				{
					if(%cancelNext)
					{
						%cancelNext = false;
						
						continue;
					}
					
					%newData = %newData SPC getWord(%data, %a);
					
					if(%a % 2 == 0)
					{
						continue;
					}
					
					%newBrick = "_" @ getWord(%data, %a);
					
					if(isObject(%newBrick) && %newBrick $= %oldName)
					{
						%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
						
						%cancelNext = true;
					}
				}
				
				CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData = %newData;
			}
			else
			{
				CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData = strReplace(CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData, getSubStr(%oldName, 1, strLen(%oldName) - 1), getSubStr(%name, 1, strLen(%name) - 1));
			}
		}
	}
};
activatePackage(CityRPG_RealEsate);