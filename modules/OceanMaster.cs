// Election
function startElection()
{
	messageAll('',"\c2Election\c6 has began!");
	resetCandidates();
	$Mayor::Mayor::ElectionID = getRandom(1, 30000);
	$Mayor::Active = 0;
	$Mayor::Voting = 1;
	$Mayor::Current = "\c2Election\c6 has began!";
	messageClient(%client, '', "\c6" @ $Mayor::Mayor::ElectionID);
	%time = $Mayor::Time * 60000;
	//%time2 = (($Mayor::Time * 60000)/2);
	//%time3 = (($Mayor::Time * 60000)/2/2);
	//%time4 = (($Mayor::Time * 60000)/2/2/2);
	schedule(%time, 0, stopElection);
	//schedule(%time4, 0, electionTimer, "Election Half Time!!!");
	//schedule(%time3, 0, electionTimer, "Election Is Close To Being Over!");
	//schedule(%time2, 0, electionTimer, "Election Is Ending Very Soon!");
}

function serverCmdstopElection(%client)
{
	if(%client.isAdmin)
	{
		$Mayor::Current = getWinner();
		$Mayor::Active = 1;
		$Mayor::Voting = 0;
		resetCandidates();
		resetimpeachers();
		messageAll('', "\c3The election has ended!");
		messageAll('', "\c3" @ $Mayor::Current SPC "\c6has won the election!");
	}
}

function stopElection()
{
	$Mayor::Current = getWinner();
	$Mayor::Active = 1;
	$Mayor::Voting = 0;
	resetCandidates();
	resetimpeachers();
	messageAll('', "\c3The election has ended!");
	messageAll('', "\c3" @ $Mayor::Current SPC "\c6has won the election!");
}

//restart
function serverCmdrestartElection(%client)
{
	if(%client.isAdmin)
	{
		resetCandidates();
		$Mayor::Mayor::ElectionID = getRandom(1, 30000);
		$Mayor::Active = 1;
		$Mayor::Current = "\c2Election\c6 has began!";
		messageClient(%client, '', "\c6" @ $Mayor::Mayor::ElectionID);
	}
}

//vote
function serverCmdvoteElection(%client, %arg2) 
{
    if(isObject(%arg1 = findClientByName(%arg2)))
    {
        if(%arg1 $= "") //if not blank
        {
            messageClient(%client, '', "Please try putting in a user's name.");
        } else {
            if($Mayor::Voting == 1 && $Mayor::Active == 0) //if election
            {
                if(CityRPGData.getData(%client.bl_id).valueElectionID != $Mayor::Mayor::ElectionID) //if hasn't voted
                {
					if(getCandidatesTF(%arg1.name))
					{
						messageClient(%client, '', "\c6You have voted for\c3" SPC %arg1.name @ "\c6.");
						CityRPGData.getData(%client.bl_id).valueElectionID = $Mayor::Mayor::ElectionID;
						%voteIncrease = getMayor($Mayor::Mayor::ElectionID, %arg1.name) + 1;
						inputMayor($Mayor::Mayor::ElectionID, %arg1.name, %voteIncrease);
					} else {
						messageClient(%client, '', "This player isn't a candidate.");
					}
                } else {
                    messageClient(%client, '', "You've already voted!");
                }
            } else {
		        messageClient(%client, '', "There isn't an election!");
            }
        }
    }
}

//vote
function serverCmdvoteElectionn(%client, %arg2) 
{
	if(%client.isAdmin)
	{
		if(isObject(%arg1 = findClientByName(%arg2)))
		{
			if(%arg1 $= "") //if not blank
			{
				messageClient(%client, '', "Please try putting in a user's name.");
			} else {
				if($Mayor::Voting == 1 && $Mayor::Active == 0) //if election
				{
					if(getCandidatesTF(%arg1.name))
					{
						messageClient(%client, '', "\c6You have voted for\c3" SPC %arg1.name @ "\c6.");
						CityRPGData.getData(%client.bl_id).valueElectionID = $Mayor::Mayor::ElectionID;
						%voteIncrease = getMayor($Mayor::Mayor::ElectionID, %arg1.name) + 1;
						inputMayor($Mayor::Mayor::ElectionID, %arg1.name, %voteIncrease);
					} else {
						messageClient(%client, '', "This player isn't a candidate.");
					}
				} else {
					messageClient(%client, '', "There isn't an election!");
				}
			}
		}
	}
}

//register
function serverCmdRegisterCandidates(%client)
{
    if(CityRPGData.getData(%client.bl_id).valueMoney >= $Mayor::Cost)
    {
        inputCandidates(%client.name);
        messageClient(%client, '', "\c6You are now a candidate for the next election!");
        CityRPGData.getData(%client.bl_id).valueMoney -= $Mayor::Cost;
    } else {
        messageClient(%client, '', "\c6You don't have $" @ $Mayor::Cost @ "!");
    }
}

//setJob
function serverCmdMeMayor(%client)
{
	if(%client.name $= $Mayor::Current)
	{
		jobset(%client, 26);
	}
}

// Looper
function automaticMayor()
{
	$Mayor::Mayor::Requirement = 15;
	if($Mayor::Active == 0) //if active Mayor
	{
		if((playerCount() >= $Mayor::Mayor::Requirement) || ($Mayor::Force::Start == 1))
		{
			if($Mayor::Voting == 0)
				startElection();
		} else if($Mayor::Voting == 1) {
			
		} else {
			$Mayor::Voting = 0;
		}
	} else if($Mayor::Voting == 0 && $Mayor::Active == 0) {	
	}
}

//databases
function inputCandidates(%string)
{
	for(%i = 0; %i < 25; %i++)
	{
		if($candidates[%i] $= "") {
			$candidates[%i] = %string;
			%i = 26;
		} else if($candidates[%i] $= %string) {
			%i = 26;
		}
	}
}

function getCandidates(%client)
{
    messageClient(%client,'',"\c6List of candidates!");
    %listnum = 0;
    for(%i = 0; %i < 25; %i++)
    {
        if($candidates[%i] $= "")
        {
        } else {
            %listnum++;
            messageClient(%client,'',"-\c6" @ %listnum @ "\c0-\c6" @ $candidates[%i]);
        }
    }
}

function getCandidatesTF(%arg1)
{
    for(%i = 0; %i < 25; %i++)
    {
        if($candidates[%i] $= %arg1)
        {
			return true;
        }
    }
	return false;
}

function resetCandidates(%client)
{
    messageClient(%client,'',"All candidates have been reset!");
    for(%i = 0; %i < 25; %i++)
    {
        $candidates[%i] = "";
    }
}

//other
function serverCmdMayorForceStart(%client)
{
	if(%client.isAdmin)
	{
		if($Mayor::Force::Start == 0)
		{
			$Mayor::Force::Start = 1;
			$Mayor::Active = 0;
			messageClient(%client, '', "\c2Enabled");
		} else {
			$Mayor::Force::Start = 0;
			messageClient(%client, '', "Disabled");
		}
	}
}

function serverCmdtopC(%client)
{
    //if(%client.isAdmin)
    //{
        messageClient(%client,'',"\c6Candidates!");
        %listnum = 0;
        for(%i = 0; %i < 25; %i++)
        {
            if($candidates[%i] !$= "")
            {
                %listnum++;
                messageClient(%client,'',"-\c6" @ %listnum @ "\c0-\c6" @ $candidates[%i] SPC "\c6has\c0" SPC getMayor($Mayor::Mayor::ElectionID, $candidates[%i]) SPC "\c6votes!");
            }
        }
    //}
}

function getWinner()
{
	%top = 0;
	%toBeat = "";
	for(%i = 0; %i < 25; %i++)
	{
		if($candidates[%i] !$= "")
		{
			%current = getMayor($Mayor::Mayor::ElectionID, $candidates[%i]);
			if(%current > %top)
			{
				%toBeat = $candidates[%i];
				%top = %current;
			}
		}
	}
	return %toBeat;
}