function VoteImpeach(%client)
{
    if(($Mayor::ImpeachCost == 0) || ($Mayor::ImpeachCost $= ""))
        $Mayor::ImpeachCost = 500;
    $Mayor::ImpeachRequirement = 15;
    if($Mayor::Current $= "" || $Mayor::Current $= "None")
        messageClient(%client,'',"\c6You can't impeach someone who doesn't exist!");
    else {
        if(!getDataimpeachersDatabase(%client.BL_ID))
        {
            if(CityRPGData.getData(%client.bl_id).valueMoney < $Mayor::ImpeachCost)
            {
                messageClient(%client,'',"You don't have the required money to impeach the Mayor!");
                return;
            }
            CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney - $Mayor::ImpeachCost;
            $Mayor::Impeach++;
            messageAll('', %client.name SPC "\c6has voted to impeach the Mayor.");
            messageAll('',"\c6Current vote count:\c0" SPC $Mayor::Impeach @ "\c6. Needed:\c0" SPC $Mayor::ImpeachRequirement);
            impeachersDatabase(%client.BL_ID);
            if($Mayor::Impeach >=  $Mayor::ImpeachRequirement)
            {
                resetImpeachVotes();
				$Mayor::Active = 0;
				$Mayor::Voting = 0;
				resetCandidates();
                $Mayor::Current = "";
                messageAll('',"\c6>>\c0THE MAYOR HAS BEEN IMPEACHED!");
            }
        } else {
            messageClient(%client,'',"\c6Chill out, you have already voted to impeach the Mayor!");
        }
    }
}

function serverCmdforceImpeach(%client)
{
	if(%client.isAdmin)
	{
		resetImpeachVotes();
		$Mayor::Active = 0;
		$Mayor::Voting = 0;
		resetCandidates();
		$Mayor::Current = "";
		messageAll('',"\c6>>\c0THE MAYOR HAS BEEN IMPEACHED!\c6 Forced by:" SPC %client.name);
	}
}
function resetImpeachVotes()
{
	for(%c = 0; %c < ClientGroup.getCount(); %c++)
	{
		%subClient = ClientGroup.getObject(%c);
        %subClient.impeachVoted = 0;
        $Mayor::Impeach = 0;
	}
}

function ClearImpeach(%client)
{
    if(%client.isAdmin)
    {
        messageClient(%client,'',"\c6The impeachment system has been cleared.");
	    for(%c = 0; %c < ClientGroup.getCount(); %c++)
	    {
		    %subClient = ClientGroup.getObject(%c);
            resetimpeachers(%client);
            $Mayor::Impeach = 0;
	    }
    }
}

function getDataimpeachersDatabase(%string)
{
	for(%i = 0; %i < 25; %i++)
	{
		if($impeachers[%i] $= %string) {
            return true;
		}
	}
    return false;
}

function impeachersDatabase(%string)
{
	for(%i = 0; %i < 25; %i++)
	{
		if($impeachers[%i] $= "") {
			$impeachers[%i] = %string;
			%i = 26;
		} else if($impeachers[%i] $= %string) {
			%i = 26;
		}
	}
}


function resetimpeachers()
{
    messageClient(%client,'',"All impeachers have been reset!");
    for(%i = 0; %i < 25; %i++)
    {
        $impeachers[%i] = "";
    }
}