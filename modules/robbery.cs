$Robbery::Cops = 3;
$Robbery::Timer = 15;
$Robbery::Cooldown = 500;
$Robbery::DemsPerPlayer = 100;
$Robbery::MaxPerPlayer = 1000;
$Robbery::Divider = 5;
$Robbery::AdminApproval = false;
$Robbery::Allowed = true;
$Robbery::noDrive = true;
$Robbery::MessageAll = true;

function serverCmdRobCount(%client){messageClient(%client,'',$Robbery::relayCount @ "/" @ $Robbery::Cooldown);}

//Brick Data
datablock fxDTSbrickData(CityRPGBankRobberyBrickData : brick4x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Bank Robbery Brick";
	
 	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "5 5 1";
	trigger = 0;
};
 
// ============================================================
// Section 2 : Trigger Data
// ============================================================
 
function serverCmdenableRobbery(%client){if(%client.isAdmin)$Robbery::Allowed=true;}
function CityRPGBankRobberyBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			if(%client.getJobSO().type !$= "crim")
			{
				if((%client.getJobSO().type $= "donator") || (%client.getJobSO().type $= "sponsor"))
					echo("crim");
				else
					return messageClient(%client,'',"Must be in criminal job to do this. -" SPC %client.getJobSO().type);
			}
			
			if(%client.player.getdatablock().getname() !$= "Player9SlotPlayer")
				return messageClient(%client,'',"You must be a Player9SlotPlayer to do this!" SPC %client.player.getdatablock().getname());
				
			if($Robbery::relayCount <= $Robbery::Cooldown)
				return messageClient(%client,'',"\c6Please wait before trying to rob again");
				
			if(($Robbery::AdminApproval) && (!$Robbery::Allowed))
			{
				messageClient(%client,'',"\c6An admin must allow this action. Please contact an admin.");
				%client.stage = 0;
				%client.robbing = false;
				return;
			}
			
			messageClient(%client, '',"\c31 \c6- Begin Robbing the bank.");
			%client.stage = 0;
			%client.robbing = false;
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			%client.stage = "";
			messageClient(%client, '', "\c6Too scared to pull it off?");
			%client.robbing = false;
		}
		return;
	}
	
	%input = StrLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			if(%client.getJobSO().type !$= "crim")
			{
				if((%client.getJobSO().type $= "donator") || (%client.getJobSO().type $= "sponsor"))
					echo("crim");
				else
					return messageClient(%client,'',"Must be in criminal job to do this. -" SPC %client.getJobSO().type);
			}
			
			if(%client.player.getdatablock().getname() !$= "Player9SlotPlayer")
				return messageClient(%client,'',"You must be a Player9SlotPlayer to do this!" SPC %client.player.getdatablock().getname());
				
			if($Robbery::relayCount <= $Robbery::Cooldown)
				return messageClient(%client,'',"\c6Please wait before trying to rob again");
				
			if(($Robbery::AdminApproval) && (!$Robbery::Allowed))
			{
				messageClient(%client,'',"\c6An admin must allow this action. Please contact an admin.");
				%client.stage = 0;
				%client.robbing = false;
				return;
			}
			
			//Dont forget to reset %client.stage before closing this "if"!
 
			//if(false)
			if(getCopCount("bool",$Robbery::Cops))
			{
				messageClient(%client,'',"\c6Now is not the time. Cops Online:" SPC getCopCount("int",$Robbery::Cops) SPC "out of" SPC $Robbery::Cops);
				%client.stage = "";
				return;
			}
			
			%edu = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
			
			if(!(%edu > 9))
			{
				messageClient(%client,'',"\c6You need \c310 Criminal Education \c6to take this offer. You have:\c3" SPC %edu @ "\c6.");
				%client.stage = "";
				return;
			}
			
			if($Robbery::MessageAll)
			{
				messageAll('',"!!!BANK ROBBERY!!! \c5Someone is beginning to hack into the bank!");
			}
			
			CityRPGData.getData(%client.bl_id).valueDemerits +=  100;
			
			for(%i = 0; %i < ClientGroup.getCount(); %i++)
			{
				%subClient = ClientGroup.getObject(%i);
				if(%subClient.getJobSO().law)
				{
					messageClient(%subClient,'',"!!!BANK ROBBERY!!! PLEASE HELP THE TOWN. Head to the bank to kill \c5" @ %client.name @ ".");
				}
			}
			
			%client.robbing = true;
			messageClient(%client,'',"\c6Ok heres the deal, take this to the second island town hall, there my guy will give you a key.");
			messageClient(%client,'',"\c6The robbery will start in\c3" SPC $Robbery::Timer SPC "seconds\c6, move off of the brick to cancel robbery");
			if(%client.robbing)
			{
				%timer = $Robbery::Timer * 1000;
				schedule(%timer, 0, "robberyStart", %client);
			}
			else
			{
			%client.stage = "";
				return;
			}
		}
	}
}
 
function RobberyStart(%client)
{
	if(!%client.robbing)
	{
		messageClient(%client,'',"\c6Im sorry to hear that.");
		return;
	}
	CityRPGData.getData(%client.bl_id).valueDemerits +=  200;
	messageClient(%client,'',"\c6You know your instructions...");
	%client.hasSuitcase = true;
	messageClient(%client,'',"\c6You were given the suitcase.");
	if($Robbery::MessageAll)
	{
		messageAll('',"!!!BANK ROBBERY!!! \c5" @ %client.name @ " is stealing you money. Head to the bank to kill them.");
		messageAll('',"!!!BANK ROBBERY!!! \c5" @ %client.name @ " is stealing you money. Head to the bank to kill them.");
		messageAll('',"!!!BANK ROBBERY!!! \c5" @ %client.name @ " is stealing you money. Head to the bank to kill them.");
	}
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%subClient = ClientGroup.getObject(%i);
		if(%subClient.getJobSO().law)
		{
			messageClient(%subClient,'',"!!!BANK ROBBERY!!! PLEASE HELP THE TOWN. Head to the bank to kill \c5" @ %client.name @ ".");
			messageClient(%subClient,'',"!!!BANK ROBBERY!!! PLEASE HELP THE TOWN. Head to the bank to kill \c5" @ %client.name @ ".");
		}
	}
	return;
}
 
//Ending Brick
datablock fxDTSbrickData(CityRPGBankRobberyEndBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Bank Robbery End Brick";
	
 	CityRPGBrickType = 2;
	CityRPGBrickAdmin = true;
	
	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};
 
function CityRPGBankRobberyEndBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			if(%client.hasSuitcase)
			{
				messageClient(%client, '', "\c6Psst, Im the one your looking for");
				messageClient(%client, '', "\c31 \c6- Exchange Suitcase");
				%client.stage = 0;
			} else {
				messageClient(%client,'',"\c6I have nothing for you.");
				%client.stage = "";
				return;
			}
		}
		
		if(%triggerStatus == false && %client.stage !$= "")
		{
			if(!%client.hasSuitcase)
			{%client.stage = ""; return;}
			messageClient(%client, '', "\c6You don't know me, I don't know you.");
			
			%client.stage = "";
		}
		
		return;
	}
	%input = strLwr(%text);
	
	if(mFloor(%client.stage) == 0)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			if(%client.hasSuitcase)
			{
				%payout = 0;
				for (%c = 0; %c < ClientGroup.getCount(); %c++) 
				{
					%target = ClientGroup.getObject(%c);
					if(%target != %client)
					{
						//if(%totalPayout < 1)
						//{
							%dems += $Robbery::DemsPerPlayer;
							%pay = CityRPGData.getData(%target.bl_id).valueBank / $Robbery::Divider;
							if(%pay > $Robbery::MaxPerPlayer)
								%pay = $Robbery::MaxPerPlayer;
							CityRPGData.getData(%target.bl_id).valueBank -= %pay;
							messageClient(%target,'',"\c6Due to a recent robbery,\c3 $" @ %pay SPC "\c6was stolen from your account.");
							//if(%payout > 10000)
							//	%payout = 10000;
							%payout += %pay;
						//}
						//else
						//	return;
					}
				}
				
				messageClient(%client,'',"\c6You've collected \c3$" @ %payout SPC "\c6from your robbery.");
				messageAll('',%client.name SPC "\c6collected \c0$" @ %payout SPC "\c6from the robbery.");
				messageClient(%client,'',"\c6You've collected \c3" @ %dems SPC "\c6demerits from your robbery.");
				cityRPGData.getData(%client.bl_id).valueMoney +=  %payout;
				cityRPGData.getData(%client.bl_id).valueDemerits +=  %dems;
				if($Robbery::AdminApproval)
					$Robbery::Allowed = false;
				%client.robbery = 0;
				%client.hasSuitcase = false;
				$Robbery::relayCount = 0;
				return;
			} else
				messageClient(%client,'',"\c6I have nothing for you.");
				return;
		}
	}
}

function getCopCount(%returnType,%requiredCount)
{
	%count = 0;
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%subClient = ClientGroup.getObject(%i);
		if(%subClient.getJobSO().law)
		{
			%count++;
		}
	}
	if(%returnType $= "bool")
	{
		if(%requiredCount >= %count)
			return true;
		else
			return false;
	} else {
		return %count;
	}
}