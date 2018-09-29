// ============================================================
// Project				:	CityRPG
// Author				:	Ty(ID997)
// Description			:	Vote Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGVoteBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";
	
	uiName = "Vote Brick";
	
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
function CityRPGVoteBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{	
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			messageClient(%client, '', "-----\c6-----\c0-----");
            messageClient(%client, '', "\c6Welcome to our voting booth.");
            if($Mayor::Voting == 1)
            {
			    messageClient(%client, '', "\c31 \c6- Apply for Mayor. (Costs: $" @ $Mayor::Cost @ ")");
 			    messageClient(%client, '', "\c32 \c6- Vote!");
 			    messageClient(%client, '', "\c33 \c6- Get Candidate list!");
                messageClient(%client, '', "\c34 \c6- View Score!");
 			} else {
                messageClient(%client, '', "\c31 \c6- Vote to impeach the Mayor! \c3($" @ $Mayor::ImpeachCost @ ") <font:arial:20>\c7MEANING: This will vote to remove the Mayor!");
                messageClient(%client, '', "There isn't an election!");
            }

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
            if($Mayor::Voting == 0)
                VoteImpeach(%client);
            else
			    serverCmdRegisterCandidates(%client, %text);
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
            %client.stage = 1.1;

			messageClient(%client, '', "\c6Type the candidate's name you'd like to vote for:");
		    return;
        }

		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
            getCandidates(%client);
            return;
        }

        if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
		{
            serverCmdtopC(%client);
            return;
        }
        
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	%input = strLwr(%text);
	
    if(mFloor(%client.stage) == 1)
	{
		if(%client.stage == 1.1)
		{
			serverCmdvoteElection(%client, %text);
            return;
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
			
			CityRPGData.getData(%client.bl_id).valueVote += mFloor(%input);
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