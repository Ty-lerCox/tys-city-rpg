// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Business Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGBusinessBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Business";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGBusinessBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(!$Business::StartCost)
		$Business::StartCost = 10000;
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			if($Business::Loaded) 
			{
				messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");			
				if(CityRPGData.getData(%client.bl_id).valueBusID)
				{
					messageClient(%client, '', "\c31 \c6- Business account.");
					messageClient(%client, '', "\c32 \c6- Your account.");
					messageClient(%client, '', "\c33 \c6- Management.\c0" SPC "(coming soon)");
					if(CityRPGData.getData(%client.bl_id).valueBusPosition $= "CEO")
					{
						messageClient(%client, '', "\c34 \c6- Destroy Business.");
					} else {
						messageClient(%client, '', "\c34 \c6- Destroy Stocks.");
					}
					messageClient(%client, '', "\c35 \c5- Toggle Prints");
				} else {
					messageClient(%client, '', "\c31 \c6- Start A Business. (\c3$" @ $Business::StartCost @ "\c6)");
					messageClient(%client, '', "\c32 \c6- How businesses work!!");
					messageClient(%client, '', "\c33 \c6- Join a business!");
					messageClient(%client, '', "\c34 \c5- Toggle Prints");
				}
							
				%client.stage = 0;
			} else {
				messageClient(%client, '', "Businesses are currently not loaded. Please ask an administrator to load them.");
			}
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			messageClient(%client, '', "\c6Thanks, come again.");
			%client.SetInfo();
			saveBusiness();
			%client.stage = "";
		}
		
		return;
	}
	%cost = 0;
	%YesNo = 0;
	%input = strLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			if(!(CityRPGData.getData(%client.bl_id).valueBusID))
			{
				serverCmdBusiness(%client, "Start");
				%client.stage = 1.1;
			} else
				serverCmdBusiness(%client, "BusinessAccount");
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			%client.stage = 1.2;
			if(CityRPGData.getData(%client.bl_id).valueBusID)
			{
				serverCmdBusiness(%client, "YourAccount");
			} else {
				messageClient(%client, '', "\c6Understanding Businesses:");
				messageClient(%client, '', "--\c6Owning a business is confusing at first but you can learn quick.");
				messageClient(%client, '', "--\c6Businesses are based around other players buying stocks from ur business.");
				messageClient(%client, '', "--\c6You can sell stock by /trade stock");
				messageClient(%client, '', "--\c6The more stock holders and managers your business has the more money you'll make.");
				messageClient(%client, '', "--Note:\c6 Your business can lose money! Becareful to watch the economy, because it effects ur businesss!");
				messageClient(%client, '', "--\c6More money in ur business account, the more you'll make from each stock.");
			}
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			
			if(CityRPGData.getData(%client.bl_id).valueBusID)
			{
				serverCmdBusiness(%client, "Manage");
			} else {
				messageClient(%client, '', "\c6Please give the CEO's Blockland ID:");
				%client.stage = 1.3;
			}
			return;
		}
		
		if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
			%client.stage = 1.2;
			if(CityRPGData.getData(%client.bl_id).valueBusID)
			{
				if(CityRPGData.getData(%client.bl_id).valueBusPosition $= "CEO")
				{
					serverCmdBusiness(%client, "Destroy", %Cost, %YesNo, %Name);
				} else {
					serverCmdBusiness(%client, "ClearStock", %Cost, %YesNo, %Name);
				}
			} else {
				togglePrints(%client);
			}
			return;
		}
		
		if(strReplace(%input, "5", "") !$= %input || strReplace(%input, "five", "") !$= %input)
		{
			%client.stage = 1.2;
			togglePrints(%client);
			return;
		}
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			%client.ccost = %text;
			%client.stage = 2.0;
			serverCmdBusiness(%client, "Start", %text);
			return;
		}
		
		if(%client.stage == 1.2)
		{
			if(CityRPGData.getData(%client.bl_id).valueBusID)
			{
				serverCmdBusiness(%client, "Destroy", %Cost, %text, %Name);
			}
			return;
		}
		
		if(%client.stage == 1.3)
		{
			serverCmdJoinBusiness(%client, %text);
			return;
		}
		
		return;
	}
	
	if(mFloor(%client.stage) == 2)
	{
		if(%client.stage == 2.0)
		{
			%client.yesno = %text;
			%client.stage = 3.0;
			serverCmdBusiness(%client, "Start", %client.ccost, %text);
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
	
	if(mFloor(%client.stage) == 3)
	{
		if(%client.stage == 3.0)
		{
			serverCmdBusiness(%client, "Start", %client.ccost, %client.yesno, %text);
			return;
		}
	}
}