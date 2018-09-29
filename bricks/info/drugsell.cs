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
datablock fxDTSBrickData(CityRPGDrugsellBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Drug Sell Brick";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGDrugsellBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c3What do you want to sell? \c6(New? /drughelp)");
			messageClient(%client, '', "\c31 \c6- Sell Marijuana.");
			messageClient(%client, '', "\c32 \c6- Sell Opium.");
			
			%client.drugname = "";
			%client.selling = false;
						
			%client.stage = 0;
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			%client.selling = false;
			messageClient(%client, '', "\c6You are no longer selling.");
			%client.drugname = "";
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
			
			if(CityRPGData.getData(%client.bl_id).valueMarijuana >= 1)
			{
				messageClient(%client,'',"\c6You have started selling.");
				%client.drugname = "marijuana";
				%client.selling = true;
			}
			else
			{
				messageClient(%client,'',"\c3You don't have any marijuana to sell.");
				%client.drugname = "";
				%client.selling = false;
			}
		}
	}

	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			%client.stage = 1.2;
			
			if(CityRPGData.getData(%client.bl_id).valueopium >= 1)
			{
				messageClient(%client,'',"\c6You have started selling.");
				%client.drugname = "opium";
				%client.selling = true;
			}
			else
			{
				messageClient(%client,'',"\c3You don't have any opium to sell.");
				%client.drugname = "";
				%client.selling = false;
			}
		}
	}
}