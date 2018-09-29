// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Gangs Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGGangsBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Gangs";
	
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
function CityRPGGangsBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	$Gangs::Color::DB = 20000;
	$Gangs::Color::G = 35000;
	$Gangs::Color::LB = 30000;
	$Gangs::Color::LG = 25000;
	$Gangs::Color::DR = 50000;
	$Gangs::Color::R = 30000;
	$Gangs::Color::Y = 10000;
	$Gangs::Color::P = 15000;
	
	if(!$Gangs::StartCost)
		$Gangs::StartCost = 100;
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c3Welcome! \c6Please type the number corresponding to the options below.");			
			
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			{
				messageClient(%client, '', "\c31 \c6- Join a gang.");
				messageClient(%client, '', "\c32 \c6- Create a gang. (\c3$" @ $Gangs::StartCost @ "\c6)");
				messageClient(%client, '', "\c33 \c6- Gang Help.");
				messageClient(%client, '', "\c34 \c5- Toggle Gang Prints.");
			} else {
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") > 0)
				{
					messageClient(%client, '', "\c6- You have \c3$" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") SPC "\c6in the bank.");
				}
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") > 0)
				{
					messageClient(%client, '', "\c6- You have \c3" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") SPC "Lumber \c6in the Storage.");
				}
				if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather"))
				{
					messageClient(%client, '', "\c31 \c6- Store/Withdraw Money.");
					messageClient(%client, '', "\c32 \c6- Store/Withdraw Lumber.");
					messageClient(%client, '', "\c33 \c6- Gang Color.");
					if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather")
						messageClient(%client, '', "\c34 \c6- Destroy Gang.");
					else
						messageClient(%client, '', "\c34 \c6- Leave Gang.");
					messageClient(%client, '', "\c35 \c5- Toggle Gang Prints.");
					if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather")
					{
						messageClient(%client, '', "\c36 \c5- Promote a Player to Mobboss. \c7(Doing this allows player to access storage)");
						messageClient(%client, '', "\c37 \c5- Remove player from gang.");
						messageClient(%client, '', "\c38 \c5- Toggle Join-able.");
					}
				} else {
					messageClient(%client, '', "\c3- \c6To access other gang stuff you must be a higher rank. You're a Mobster. Must be a Mobboss or GodFather(Leader)");
					messageClient(%client, '', "\c31 \c5- Toggle Gang Prints.");
					messageClient(%client, '', "\c32 \c6- Leave Gang.");
				}
			}
			
			%client.stage = 0;
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			messageClient(%client, '', "\c6Thanks, come again.");
			saveGang();
			%client.stage = "";
		}
		
		return;
	}
	%arg2 = 0;
	%arg1 = 0;
	%input = strLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			{
				messageClient(%client, '', "\c6Enter the ID of the godfather:");
				%client.stage = 1.4;
			} else {
				if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather"))
				{
					messageClient(%client, '', "\c6Store/Withdraw Money:");
					messageClient(%client, '', "\c31 \c6- Withdraw Money.");
					messageClient(%client, '', "\c32 \c6- Deposit Money.");
					%client.stage = 1.1;
				} else {
					if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Gangs")
					{
						serverCmdPrints(%client, "Stats");
					} else {
						serverCmdPrints(%client, "Gangs");
					}
				}
			}
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			{
				serverCmdgang(%client, "Create");
				%client.stage = 2.5;
			} else {
				if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather"))
				{
					messageClient(%client, '', "\c6Store/Withdraw Lumber:");
					messageClient(%client, '', "\c31 \c6- Withdraw Lumber.");
					messageClient(%client, '', "\c32 \c6- Deposit Lumber.");
					%client.stage = 1.2;
				} else {
					CityRPGData.getData(%client.bl_id).valueGangID = "";
					CityRPGData.getData(%client.bl_id).valueGangPosition = "";
					messageClient(%client, '', "\c6You have left the gang.");
				}
			}
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			{
				serverCmdgang(%client, "Help");
			} else {
				if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather"))
				{
					messageClient(%client, '', "\c6Chose a color to buy: <color:3F5D7D>DarkBlue \c7$" @ $Gangs::Color::DB @ "\c6, <color:279B61>Green \c7$" @ $Gangs::Color::G @ "\c6, <color:008AB8>LightBlue \c7$" @ $Gangs::Color::LB @ "\c6, <color:A3E496>LightGreen \c7$" @ $Gangs::Color::LG @ "\c6, <color:993333>DarkRed \c7$" @ $Gangs::Color::DR @ "\c6,");
					messageClient(%client, '', "<color:CC3333>Red \c7$" @ $Gangs::Color::R @ "\c6, <color:FFCC33>Yellow \c7$" @ $Gangs::Color::Y @ "\c6, <color:CC6699>Pink \c7$" @ $Gangs::Color::P);
					%client.stage = 1.3;
				}
			}
			return;
		}
		
		if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			{	
				if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Gangs")
				{
					serverCmdPrints(%client, "Stats");
				} else {
					serverCmdPrints(%client, "Gangs");
				}
			} else {
				if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather")
				{
					serverCmdgang(%client, "Destroy");
				} else if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss")
				{
					CityRPGData.getData(%client.bl_id).valueGangID = "";
					CityRPGData.getData(%client.bl_id).valueGangPosition = "";
					messageClient(%client, '', "\c6You have left the gang.");
				}
			}
			return;
		}
		
		if(strReplace(%input, "5", "") !$= %input || strReplace(%input, "five", "") !$= %input)
		{
			if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Gangs")
			{
				serverCmdPrints(%client, "Stats");
			} else {
				serverCmdPrints(%client, "Gangs");
			}
			return;
		}
		
		if(strReplace(%input, "6", "") !$= %input || strReplace(%input, "six", "") !$= %input)
		{
			%client.stage = 1.5;
			messageClient(%client, '', "\c6Enter the ID of the player you'd like to promote: ");
			return;
		}
		
		if(strReplace(%input, "7", "") !$= %input || strReplace(%input, "seven", "") !$= %input)
		{
			%client.stage = 1.6;
			messageClient(%client, '', "\c6Enter the ID of the player you'd like to remove: ");
			return;
		}
		
		if(strReplace(%input, "8", "") !$= %input || strReplace(%input, "eight", "") !$= %input)
		{
			if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather"))
			{
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Joinable"))
				{
					messageClient(%client,'',"Your gang is no-longer join-able.");
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Joinable", false);
				} else {
					messageClient(%client,'',"Your gang is now join-able.");
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Joinable", true);
				}
			} else
				messageClient(%client,'',"Must be a mobboss or godfather");
			return;
		}
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			if(%text == 1) 
			{
				messageClient(%client, '', "\c6Please enter the amount of money you wish to withdraw.");
				%client.stage = 2.1;
			} else if(%text == 2) {
				messageClient(%client, '', "\c6Please enter the amount of money you wish to deposit.");
				%client.stage = 2.2;
			}
			return;
		}
		
		if(%client.stage == 1.2)
		{
			if(%text == 1) 
			{
				messageClient(%client, '', "\c6Please enter the amount of lumber you wish to withdraw.");
				%client.stage = 2.3;
			} else if(%text == 2) {
				messageClient(%client, '', "\c6Please enter the amount of lumber you wish to deposit.");
				%client.stage = 2.4;
			}
			return;
		}
		
		if(%client.stage == 1.3)
		{
			if(%text $= "DarkBlue") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::DB)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:3F5D7D>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:3F5D7D>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::DB;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "Green") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::G)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:279B61>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:279B61>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::G;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "LightBlue") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::LB)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:008AB8>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:008AB8>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::LB;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "LightGreen") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::LG)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:A3E496>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:A3E496>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::LG;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "DarkRed") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::DR)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:993333>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:993333>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::DR;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "Red") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::R)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:CC3333>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:CC3333>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::R;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "Yellow") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::Y)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:FFCC33>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:FFCC33>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::Y;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else if(%text $= "Pink") {
				if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::Color::P)
				{
					messageClient(%client,'',"\c6Your gang color has been set to: <color:CC6699>" @ %text);
					inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "<color:CC6699>");
					CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::Color::P;
				} else
					messageClient(%client,'',"\c6You don't have enough money.");
			} else {
				messageClient(%client,'',"\c6Invalid Color");
			}
			return;
		}
		
		if(%client.stage == 1.4)
		{
			serverCmdgang(%client, "Join", %text);
			return;
		}
		
		if(%client.stage == 1.5)
		{
			PromoteGangMember(%client, %text);
			return;
		}
		
		if(%client.stage == 1.6)
		{
			RemoveGangMember(%client, %text);
			return;
		}
		
		return;
	}
	
	if(mFloor(%client.stage) == 2)
	{
		if(%client.stage == 2.1)
		{
			if(mFloor(%text) < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of money to withdraw.");
				
				return;
			}
			
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") - mFloor(%text) < 0)
			{
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") < 1)
				{
					messageClient(%client, '', "\c6You don't have that much money in the bank to withdraw.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%text = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money");
			}
			
			messageClient(%client, '', "\c6You have withdrawn \c3$" @ mFloor(%text) @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			%amount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") - mFloor(%text);
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money", %amount);
			CityRPGData.getData(%client.bl_id).valueMoney += mFloor(%text);
			
			%client.SetInfo();
		}
		else if(%client.stage == 2.2)
		{
			if(mFloor(%text) < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of money to deposit.");
				
				return;
			}
			
			if(CityRPGData.getData(%client.bl_id).valueMoney - mFloor(%text) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueMoney < 1)
				{
					messageClient(%client, '', "\c6You don't have that much money to deposit.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%text = CityRPGData.getData(%client.bl_id).valueMoney;
			}
			
			messageClient(%client, '', "\c6You have deposited \c3$" @ mFloor(%text) @ "\c6!");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			%amount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") + mFloor(%text);
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money", %amount);
			CityRPGData.getData(%client.bl_id).valueMoney -= mFloor(%text);
			
			%client.SetInfo();
		} 
		else if(%client.stage == 2.3) 
		{
			if(mFloor(%input) < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of Lumber to withdraw.");
				
				return;
			}
			
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") - mFloor(%input) < 0)
			{
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") < 1)
				{
					messageClient(%client, '', "\c6You don't have that much Lumber in the Storage to withdraw.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%input = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber");
			}
			
			messageClient(%client, '', "\c6You have withdrawn \c3" @ mFloor(%input) @ " Lumber\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			%amount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") - mFloor(%text);
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber", %amount);
			CityRPGData.getData(%client.bl_id).valueResources += mFloor(%input);
			
			%client.SetInfo();
		}
		else if(%client.stage == 2.4) 
		{
			if(mFloor(%text) < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of Lumber to deposit.");
				
				return;
			}
			
			if(CityRPGData.getData(%client.bl_id).valueResources - mFloor(%text) < 0)
			{
				if(CityRPGData.getData(%client.bl_id).valueResources < 1)
				{
					messageClient(%client, '', "\c6You don't have that much Lumber to deposit.");
					
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
					
					return;
				}
				
				%text = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0);
			}
			
			messageClient(%client, '', "\c6You have deposited \c3" @ mFloor(%text) SPC "Lumber\c6!");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			%amount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") + mFloor(%text);
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber", %amount);
			CityRPGData.getData(%client.bl_id).valueResources -= mFloor(%text);
			
			%client.SetInfo();
		}
		else if(%client.stage == 2.5) 
		{
			serverCmdgang(%client, "Create", %text);
		}
	}
	if(mFloor(%client.stage) == 3)
	{
		if(%client.stage == 3.0)
		{
			
		}
	}
}