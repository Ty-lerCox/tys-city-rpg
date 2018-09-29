// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Education Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGEducationBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Education Brick";
	
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
function CityRPGEducationBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");
			messageClient(%client, '', "\c31 \c6- Increase \c3ShopEdu\c6 Level.");
			messageClient(%client, '', "\c32 \c6- Increase \c3LawEdu\c6 Level.");
			messageClient(%client, '', "\c33 \c6- Increase \c3MedicEdu\c6 Level.");
			messageClient(%client, '', "\c34 \c6- Increase \c3CriminalEdu\c6 Level.");
			messageClient(%client, '', "\c35 \c6- Increase \c3JusticeEdu\c6 Level.");
			messageClient(%client, '', "\c36 \c6- \c3Current\c6 Education Levels.");
						
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
            serverCmdshopedu(%client, "accept");
            return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
            serverCmdlawedu(%client, "accept");
            return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
            serverCmdmedicedu(%client, "accept");
            return;
		}
		
		if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
            serverCmdcriminaledu(%client, "accept");
            return;
		}
		
		if(strReplace(%input, "5", "") !$= %input || strReplace(%input, "five", "") !$= %input)
		{
            serverCmdjusticeedu(%client, "accept");
            return;
		}
		
		if(strReplace(%input, "6", "") !$= %input || strReplace(%input, "six", "") !$= %input)
		{
			serverCmdstats(%client);
			return;
		}
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
}