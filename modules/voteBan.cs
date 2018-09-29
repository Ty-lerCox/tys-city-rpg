function serverCmdVoteBan(%client, %target) //voting to ban player for __ time
{
    $BanPlayer::Neededcount = ClientGroup.getCount() / 2 + 1;

    if(%target $= "")
    {
        messageClient(%client, '', "\c6Please enter a valid player's name.");
    } else {
        if(!getDatabannersDatabase(%client.BL_ID)) //if hasn't voted already
        {
            if($BanPlayer::Player $= "") //if active ban isn't present and declare time limit
            {
                if(isObject(%name = findClientByName(%target)))
                {
                    if(%name $= "") //if not blank
                    {
                        messageClient(%client, '', "\c6Please enter players name.");
                        return;
                    } else {
                        %name.VotesBan++; //add vote
                        $BanPlayer::Player = %name; //declare new target for server
                        messageClient(%client, '', "\c6You have posted a vote to ban\c0" SPC %name.name SPC "\c6for 10mins."); //message
                        messageAll('',"" @ %client.name SPC "\c6has voted for\c0" SPC %name.name SPC "\c6to be banned from the server for 10mins.");
                        messageAll('',"\c6Current vote count:\c0" SPC %name.VotesBan @ "\c6. Needed:\c0" SPC $BanPlayer::Neededcount);
                        if(%name.VotesBan >= $BanPlayer::Neededcount) 
                        {
                            messageAll('',"\c6" @ %name.name SPC "has been banned from the server.");
                            serverCmdBan(%name, %name.bl_id, 10, "asfd");
                        }
                        bannersDatabase(%client.BL_ID);
                        $BanPlayer::Player = %name.name;
                        schedule(120000,0,"resetBanVotes",%name);
                    }
                }
            } else { //else make active
                if(isObject(%name = findClientByName(%target)))
                {
                    if($BanPlayer::Player $= %name.name)
                    {
                        if(%name $= "") //if not blank
                        {
                            messageClient(%client, '', "\c6Please enter players name.");
                            return;
                        } else {
                            %name.VotesBan++; //add vote
                            $BanPlayer::Player = %name; //declare new target for server
                            messageClient(%client, '', "\c6You voted to ban\c0" SPC %name.name SPC "\c6for 10mins."); //message
                            messageAll('',"" @ %client.name SPC "\c6has voted for\c0" SPC %name.name SPC "\c6to be banned from the server for 10mins.");
                            messageAll('',"\c6Current vote count:\c0" SPC %name.VotesBan @ ". \c6Needed:\c0" SPC $BanPlayer::Neededcount);
                            if(%name.VotesBan >= $BanPlayer::Neededcount) 
                            {
                                messageAll('',"\c6" @ %name.name SPC "has been banned from the server.");
                                banBLID(%name.bl_id, 10, "Server has banned you for 10mins.");
                            }
                            bannersDatabase(%client.BL_ID);
                        }
                    } else {
                        
                    }
                }
            }
        } else {
            messageClient(%client, '', "\c6You have already voted!");
        }
    }
}

function resetBanVotes(%name)
{
    messageAll('',"" @ %client.name SPC "\c6The banning system ended the votes for banning\c0" SPC %name.name);
	for(%c = 0; %c < ClientGroup.getCount(); %c++)
	{
		%subClient = ClientGroup.getObject(%c);
        %subClient.VotesBan = 0;
        %subClient.BanPlayer = 0;
        $BanPlayer::Player = "";
	}
    resetbanners(%client);
}

function serverCmdClearBan(%client)
{
    if(%client.isAdmin)
    {
	    for(%c = 0; %c < ClientGroup.getCount(); %c++)
	    {
		    %subClient = ClientGroup.getObject(%c);
            %subClient.VotesBan = 0;
            resetbanners(%client);
            %subClient.BanPlayer = 0;
            $BanPlayer::Player = "";
		}
        messageClient(%client, '', "All ban votes have been reset!");
    }
}


function getDatabannersDatabase(%string)
{
	for(%i = 0; %i < 25; %i++)
	{
		if($banners[%i] $= %string) {
            return true;
		}
	}
    return false;
}

function bannersDatabase(%string)
{
	for(%i = 0; %i < 25; %i++)
	{
		if($banners[%i] $= "") {
			$banners[%i] = %string;
			%i = 26;
		} else if($banners[%i] $= %string) {
			%i = 26;
		}
	}
}


function resetbanners(%client)
{
    messageClient(%client,'',"All banners have been reset!");
    for(%i = 0; %i < 25; %i++)
    {
        $banners[%i] = "";
    }
}