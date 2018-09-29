// ============================================================
// Project				:	CityRPG
// Author				:	Iban & Jookia & Ty(ID997)
// Description			:	Pet Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGPetBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Pet Brick";
	
	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGPetBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
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
			%totalPoints = CityRPGData.getData(%client.bl_id).valuePetPoints;
			%usedPoints = UsedPoints(%client);
			%AvaliablePoints = AvaliablePoints(%client);
			messageClient(%client, '', "\c6Welcome to the \c3Adoption Centre! \c6Please type the number corresponding to the options below.");
			messageClient(%client, '', "\c6Pet Points - Total Points:\c3" SPC %totalPoints SPC "\c6 Used:\c3" SPC %usedPoints SPC "\c6 Available\c3:" SPC %AvaliablePoints);
			messageClient(%client, '', "\c31 \c6- /PetHelp");
			messageClient(%client, '', "\c32 \c6- Purchase Pet Points -\c3 $" @ $Pets::Point::Cost @ "/each");
			messageClient(%client, '', "\c33 \c6- Get Dog - Pet Points:\c3" SPC $Pets::Dog::Cost);
			messageClient(%client, '', "\c34 \c6- Get Monkey - Pet Points:\c3" SPC $Pets::Monkey::Cost);
			messageClient(%client, '', "\c35 \c6- Get Horse - Pet Points:\c3" SPC $Pets::Horse::Cost);
			messageClient(%client, '', "\c36 \c6- Get Skeleton - Pet Points:\c3" SPC $Pets::Skeleton::Cost);
			//messageClient(%client, '', "\c37 \c6- Get Child - $" @ $Pets::Child::Cost);
						
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
			serverCmdPetHelp(%client);
			
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			%client.stage = 1.1;
			
			messageClient(%client, '', "\c6How many points?");
			
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			%client.stage = 1.2;
			
			messageClient(%client, '', "\c6What would you like to name your animal?");			
			
			return;
		}
		
		if(strReplace(%input, "4", "") !$= %input)
		{
			%client.stage = 1.3;
			
			messageClient(%client, '', "\c6What would you like to name your animal?");			
			
			return;
		}
		
		if(strReplace(%input, "5", "") !$= %input)
		{
			%client.stage = 1.4;
			
			messageClient(%client, '', "\c6What would you like to name your animal?");			
			
			return;
		}
		
		if(strReplace(%input, "6", "") !$= %input)
		{
			%client.stage = 1.5;
			
			messageClient(%client, '', "\c6What would you like to name your animal?");			
			
			return;
		}
		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			BuyPetPoints(%client, %input);
		}
		
		if(%client.stage == 1.2)
		{
			GetDog(%client, %input);
		}
		
		if(%client.stage == 1.3)
		{
			GetMonkey(%client, %input);
		}
		
		if(%client.stage == 1.4)
		{
			GetHorse(%client, %input);
		}
		
		if(%client.stage == 1.5)
		{
			GetSkeleton(%client, %input);
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