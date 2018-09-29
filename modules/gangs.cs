$GangWarz::Score::Mobboss = 3;
$GangWarz::Score::GodFather = 5;
$GangWarz::Score::Mobster = 1;

function serverCmdgang(%client, %do, %arg1, %arg2, %arg3)
{
	if(%Do $= "Join")
	{
		if(getGang(CityRPGData.getData(%arg1).valueGangID, "Joinable"))
		{
			CityRPGData.getData(%client.bl_id).valueGangID = %arg1;
			CityRPGData.getData(%client.bl_id).valueGangPosition = "Mobster";
			%client.setInfo();
		} else
			messageClient(%client, '', "This gang is currently LOCKED.");
		
	} else if(%Do $= "Create") {
		//if player has license
        if(CityRPGData.getData(%client.bl_id).valueMoney >= $Gangs::StartCost)
        {
			if(!(%arg1 $= ""))
			{
				CityRPGData.getData(%client.bl_id).valueGangID = %client.bl_id;
				CityRPGData.getData(%client.bl_id).valueGangPosition = "GodFather";
				messageClient(%client, '', "\c6You have started a gang.");
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name", %arg1);
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money", 0);
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber", 0);
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score", 0);
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Joinable", false);
				inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "");
				CityRPGData.getData(%client.bl_id).valueMoney -= $Gangs::StartCost;
				%client.setInfo();
			} else {
				messageClient(%client, '', "\c6Please enter the name of your gang:");
			}
		} else {
			messageClient(%client, '', "\c6You don't have the required amount of money to start a gang.");
		}
	} else if(%Do $= "Help") {
		messageClient(%client, '', "\c6Gangs are like groups. They allow players to hold global storage account where you and ur friends can deposit and withdraw lumber and money from.");
		messageClient(%client, '', "\c6Your gang can also buy a gang color. This allows for you to be more noticable in the chat.");
		messageClient(%client, '', "\c6Gangs can also set property, allowing anyone in the gang to place a brick in that lot.");
		messageClient(%client, '', "\c6You can also buy a gang brick for $2500. This allows you to access all this data from your home.");
	} else if(%Do $= "Destroy") {
		CityRPGData.getData(%client.bl_id).valueGangID = "";
		CityRPGData.getData(%client.bl_id).valueGangPosition = "";
		messageClient(%client, '', "\c6You have destroyed your gang.");
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name", "");
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money", 0);
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber", 0);
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score", 0);
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Joinable", false);
		inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color", "");
	}
}

function PromoteGangMember(%client, %arg1)
{
	if(CityRPGData.getData(%arg1).valueGangID == CityRPGData.getData(%client.bl_id).valueGangID)
	{
		CityRPGData.getData(%arg1).valueGangPosition = "Mobboss";
		messageClient(%client, '', "\c3" @ %arg1.name SPC "is now a mobboss in ur gang!");
	} else {
		messageClient(%client, '', "\c6This player isn't in your gang. Please ask him to join your gang through the gang brick.");
	}
}

function RemoveGangMember(%client, %arg1)
{
	if(CityRPGData.getData(%arg1).valueGangID == CityRPGData.getData(%client.bl_id).valueGangID)
	{
		CityRPGData.getData(%arg1).valueGangPosition = "";
		CityRPGData.getData(%arg1).valueGangID = "";
		messageClient(%client, '', "\c3" @ %arg1.name SPC "has been removed from your gang!");
	} else {
		messageClient(%client, '', "\c6This player isn't in your gang. Please ask him to join your gang through the gang brick.");
	}
}

