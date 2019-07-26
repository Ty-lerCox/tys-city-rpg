$Kicker = 0;

if(isFile("Add-Ons/System_ReturnToBlockland/hooks/serverControl.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/hooks/serverControl.cs");
	}
	
	RTB_registerPref("Enabled", "Votekick", "$Votekick::Enabled", "bool", "Server_Votekick", true, false, true, "");
	RTB_registerPref("Time between Votekicks", "Votekick", "$Votekick::Cooldown", "int 0 3600", "Server_Votekick", 60, false, true, "");
	RTB_registerPref("Poll Run Time", "Votekick", "$Votekick::PollTime", "int 0 3600", "Server_Votekick", 45, false, true, "");
	RTB_registerPref("Ban Time", "Votekick", "$Votekick::BanTime", "int 1 20160", "Server_Votekick", 5, false, true, "");
	RTB_registerPref("Ban Reason", "Votekick", "$Votekick::BanReason", "string 250", "Server_Votekick", "You were temporarily banned for being a nuisance to the server.", false, true, "");
}
else
{
	$Votekick::Enabled = false;
	$Votekick::Cooldown = 60*5;
	$Votekick::PollTime = 45;
	$Votekick::BanTime = 5;
	$Votekick::BanReason = "You were temporarily banned for being a nuisance to the server.";
}

function serverCmdVotekick(%client, %name)
{
	if(!$Votekick_Temp::Active)
	{
		if($sim::time - $Votekick_Temp::LastPoll > $Votekick::Cooldown)
		{
			if(%name !$= "")
			{
				%name = strlwr(%name);
				%count = ClientGroup.getCount();
				
				if(%count > 2)
				{
					%hit = 0;
					%hits = 0;
					%hitBL_ID = 0;
					%hitBL_IDs = "";
					
					for(%i = 0; %i < %count; %i++)
					{
						%subClient = ClientGroup.getObject(%i);
						
						if(strpos(strlwr(%subClient.name), %name) > -1)
						{
							%hitBL_ID = %subClient.bl_id;
							
							if(%hitBL_IDs $= "")
							{
								%hitBL_IDs = %subClient.bl_id;
								%hit[%hits] = %subClient;
								%hits++;
							}
							else
							{
								%alreadyAdded = false;
								
								for(%i2 = 0; %i2 < getWordCount(%hitBL_IDs); %i2++)
								{
									if(getWord(%hitBL_IDs, %i2) == %hitBL_ID)
									{
										%alreadyAdded = true;
										break;
									}
								}
								
								if(!%alreadyAdded)
								{
									%hitBL_IDs = %hitBL_IDs SPC %subClient.bl_id;
									%hit[%hits] = %subClient;
									%hits++;
								}
							}
						}
						
						if(%hits > 1)
						{
							break;
						}
					}
					
					if(%hits == 1)
					{
						%victim = %hit[0];
						
						if(%victim.bl_id != %client.bl_id)
						{
							if(!%victim.isSuperAdmin && !%victim.isAdmin)
							{
								$Votekick_Temp::Active = true;
								$Votekick_Temp::LastPoll = $sim::time;
								$Votekick_Temp::Schedule = schedule($Votekick::PollTime * 1000, 0, "Votekick_Eval");
								$Votekick_Temp::Victim = %victim;
								$Votekick_Temp::VoteNo = 0;
								$Votekick_Temp::VoteYes = 0;
								$Kicker=%client;
								messageAll('', "\c3" @ %client.name @ "\c6 has started a poll to votekick \c3" @ $Votekick_Temp::Victim.name @ "\c6.");
								messageAll('', "\c6Type \c3/yess\c6 to second this motion or \c3/noo\c6 to object to it within \c3" @ $Votekick::PollTime @ "\c6 seconds");
								
								//echo("[Votekick]" SPC %client.name SPC "(" @ %client.bl_id @ ") initiated a votekick against" SPC %victim SPC "(" @ %victim.bl_id @ ").");
								
								Votekick_Vote(%client, true);
								Votekick_Vote(%victim, false);
							}
							else
							{
								messageClient(%client, '', "\c6You cannot votekick \c5Admins\c6 or \c5Super Admins\c6.");
							}
						}
						else
						{
							messageClient(%client, '', "\c6Don't be a drama queen. Just leave the server normally if you want to get out.");
						}
					}
					else if(%hits > 1)
					{
						messageClient(%client, '', "\c6Multiple clients found with \"\c3" @ %name @ "\c6\" in their name.");
					}
					else if(%hits == 0)
					{
						//echo(%hits);
						messageClient(%client, '', "\c6No clients found with \"\c3" @ %name @ "\c6\" in their name.");
					}
				}
				else
				{
					messageClient(%client, '', "\c6The server needs more people to initiate a votekick.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6Please enter a name.");
			}
		}
		else
		{
			%timeleft = mCeil($Votekick::Cooldown - ($sim::time - $Votekick_Temp::LastPoll));
			messageClient(%client, '', "\c6You must wait another \c3" @ %timeleft @ "\c6 seconds to start another votekick.");
		}
	}
	else
	{
		messageClient(%client, '', "\c6There is already another votekick against \c3" @ $Votekick_Temp::Victim.name @ "\c6.");
	}
}

function serverCmdVoteTimeleft(%client)
{
	if($Votekick_Temp::Active)
	{
		%timeleft = mCeil($Votekick::Cooldown - ($sim::time - $Votekick_Temp::LastPoll));
		messageClient(%client, '', "\c6Timeleft: \c3" @ %timeleft @ " seconds\c6.");
	}
	else
	{
		messageClient(%client, '', "\c6There is no votekick in progress.");
	}
}

function serverCmdCancelVote(%client)
{
	if(%client.isAdmin || %client.isSuperAdmin)
	{
		if($Votekick_Temp::Active)
		{
			messageAll('', "\c3" @ %client.name @ "\c6 has canceled the votekick.");
		}
		else
		{
			messageClient(%client, '', "\c6All $Votekick_Temp data has been cleared.");
		}
		
		Votekick_Clear();
	}
	else
	{
		messageClient(%client, '', "\c6You must be an admin to use this command.");
	}
}

function serverCmdYess(%client)
{
	Votekick_Vote(%client, true);
}

function serverCmdMaybe(%client)
{
	if($Votekick_Temp::Active)
	{
		if(!Votekick_Voted(%client.bl_id))
		{
			$Votekick_Temp::Votes = setField($Votekick_Temp::Votes, getFieldCount($Votekick_Temp::Votes), %client.bl_id);
			$Votekick_Temp::Maybes = setField($Votekick_Temp::Maybes, getFieldCount($Votekick_Temp::Maybes), %client.bl_id);
			
			messageClient(%client, '', "\c6Not all of life's decisions are easy. This is no exception.");
			//echo("[Votekick]" SPC %client.name SPC "(" @ %client.bl_id @ ") couldn't make up his mind.");
			
			if(Votekick_AllVoted())
			{
				cancel($Votekick_Temp::Schedule);
				Votekick_Eval();
			}
		}
	}
}

function serverCmdNoo(%client)
{
	Votekick_Vote(%client, false);
}

function Votekick_AllVoted()
{
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%bl_id = ClientGroup.getObject(%a).bl_id;
		%found = false;
		
		for(%b = 0; %b < getFieldCount($Votekick_Temp::Votes); %b++)
		{
			%check = getField($Votekick_Temp::Votes, %b);
			
			if(%check $= %bl_id)
			{
				%found = true;
				break;
			}
		}
		
		if(!%found)
		{
			return false;
		}
	}
	
	return true;
}

function Votekick_Vote(%client, %inFavor)
{
	if(isObject(%client))
	{
		if($Votekick_Temp::Active)
		{
			if(!Votekick_Voted(%client.bl_id))
			{
				$Votekick_Temp::Votes = setField($Votekick_Temp::Votes, getFieldCount($Votekick_Temp::Votes), %client.bl_id);
				
				if(%inFavor)
				{
					$Votekick_Temp::VoteYes++;
					messageClient(%client, '', "\c6You have voted \c3in favor\c6 of kicking \c3" @ $Votekick_Temp::Victim.name @ "\c6.");
					//echo("[Votekick]" SPC %client.name SPC "(" @ %client.bl_id @ ") voted IN FAVOR of kicking" SPC $Votekick_Temp::Victim.name @ ".");
				}
				else
				{
					$Votekick_Temp::VoteNo++;
					messageClient(%client, '', "\c6You have voted \c3in opposition\c6 of kicking \c3" @ $Votekick_Temp::Victim.name @ "\c6.");
					//echo("[Votekick]" SPC %client.name SPC "(" @ %client.bl_id @ ") voted AGAINST kicking" SPC $Votekick_Temp::Victim.name @ ".");
				}
				
				if(Votekick_AllVoted())
				{
					cancel($Votekick_Temp::Schedule);
					Votekick_Eval();
				}         
			}
			else
			{
				messageClient(%client, '', "\c6Your ID has already been used to vote in this poll.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6There is no pending votekick.");
		}
	}
}

function Votekick_Voted(%checkBL_ID)
{
	for(%a = 0; %a < getFieldCount($Votekick_Temp::Votes); %a++)
	{
		%thisBL_ID = getField($Votekick_Temp::Votes, %a);
		
		if(%thisBL_ID $= %checkBL_ID)
		{
			return true;
		}
	}
	
	return false;
}

function Votekick_Eval()
{
	if(isObject($Votekick_Temp::Victim))
	{
		if($Votekick_Temp::Active)
		{
			$Votekick_Temp::Active = false;
			
			if($Votekick_Temp::VoteYes > $Votekick_Temp::VoteNo)
			{
				%name = $Votekick_Temp::Victim.name;
				%bl_id = $Votekick_Temp::Victim.bl_id;
				
				name(%name).delete("You were removed from the server.");
				
				%cloneKicks = Votekick_KickClones(%bl_id);
				
				if(%cloneKicks > 0)
				{
					messageAll('', "\c6The votekick \c2succeeded\c6 (" @ $Votekick_Temp::VoteYes @ " to " @ $Votekick_Temp::VoteNo @ "). \c3" @ %name @ "\c6, and \c3" @ %cloneKicks @ "\c6 " @ (%cloneKicks > 1 ? "clones" : "clone") @ ", were kicked.");
					//echo("[Votekick]" SPC %name SPC "(" @ %bl_id @ "), and" SPC %cloneKicks SPC "clones, were successfully votekicked.");
				}
				else
				{
					messageAll('', "\c6The votekick \c2succeeded\c6 (" @ $Votekick_Temp::VoteYes @ " to " @ $Votekick_Temp::VoteNo @ "). \c3" @ %name @ "\c6 was kicked.");
					//echo("[Votekick]" SPC %name SPC "(" @ %bl_id @ ") was successfully votekicked.");
				}
			}
			else
			{
				messageAll('', "\c6The votekick \c0failed\c6 (" @ $Votekick_Temp::VoteYes @ " to " @ $Votekick_Temp::VoteNo @ ").");
				messageAll('', "\c6The votekicker will now be removed from the game for failing to votekick.");
				//echo("[Votekick]" SPC %name SPC "(" @ %bl_id @ ") was NOT votekicked.");
				%client = $Kicker;
				%client.delete("Failing to vote kick");
			}
			
			
			%maybes = getFieldCount($Votekick_Temp::Maybes);
			if(%maybes)
				messageAll('', "\c6And \c3" @ %maybes SPC (%maybes > 1 ? "People" : "Person") SPC "\c6couldn't make up their mind!");
			
			
			Votekick_Clear();
		}
		else
		{
			//echo("\c2ERROR: Attempted to Votekick_Eval when no votekick is running");
		}
	}
	else
	{
		//echo("[Votekick] Votekick aborted due to victim leaving server.");
	}
}

function Votekick_KickClones(%bl_id)
{
	while(isObject(%clone = findClientByBL_ID(%bl_id)))
	{
		%clone.delete($Votekick::BanReason);
		%cloneKicks++;
	}
	
	return (%cloneKicks - 1);
}

function Votekick_Clear()
{
	$Votekick_Temp::Active	= false;
	$Votekick_Temp::Maybes	= "";
	$Votekick_Temp::Votes	= "";
	$Votekick_Temp::Victim	= 0;
	$Votekick_Temp::VoteNo	= 0;
	$Votekick_Temp::VoteYes	= 0;
	cancel($Votekick_Temp::Schedule);
}

package Votekick
{
	function gameConnection::autoAdminCheck(%client)
	{
		if($Votekick_Temp::Active)
		{
			%timeleft = mCeil($Votekick::Cooldown - ($sim::time - $Votekick_Temp::LastPoll));
			
			messageClient(%client, '', "\c6There is a vote running to kick \c3" @ $Votekick_Temp::Victim.name @ "\c6 from the server.");
			messageClient(%client, '', "\c6Type \c3/yess\c6 to second this motion or \c3/noo\c6 to object to it within \c3" @ %timeleft @ "\c6 seconds.");
		}
		
		return parent::autoAdminCheck(%client);
	}
	
	function gameConnection::onClientLeaveGame(%client)
	{
		if($Votekick_Temp::Active && isObject($Votekick_Temp::Victim) && $Votekick_Temp::Victim.bl_id == %client.bl_id)
		{
			%isBanned = BanManagerSO.isBanned(%client.bl_id);
			
			if(!%isBanned)
			{
				Votekick_Clear();
				schedule(33, 0, "banBLID", %client.bl_id, $Votekick::BanTime * 2, $Votekick::BanReason);
				schedule(32, 0, "Votekick_KickClones", %client.bl_id);
				//echo("[Votekick]" SPC %client.name SPC "(" @ %client.bl_id @ ") was banned for trying to dodge a votekick.");
			}
			else
			{
				Votekick_Clear();
				schedule(33, 0, "messageAll", '', "\c6The votekick has ended because \c3" @ %client.name @ "\c6 was banned from the server.");
				schedule(32, 0, "Votekick_KickClones", %client.bl_id);
				//echo("[Votekick] Vote has ended due to victim being banned.");
			}
		}
		
		return parent::onClientLeaveGame(%client);
	}
};
activatePackage(Votekick);