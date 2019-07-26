// =====================================
// Project: Server_CellPhone
// Author: JJstorm
// Project Time: 34 mins
// Submitted: 7/19/2011
// =====================================
// Table Of Contents
//------------------
// 1. Exec
// 2. Prefs
// 3. Events
// 4. Functions
// =====================================

// =====================================
// 1. Exec
// =====================================
//=====================================
// Project: Server_CellPhone
// Author: JJstorm
// Project Time: 54 mins
// Submitted: 7/19/2011
//=====================================

package CellPhone
{
	function serverCmdmessageSent(%client, %text)
	{
		// Call
		if(%client.call == 1)
		{
			%target = findclientbybl_id(%client.bl_idcalling);
			messageclient(%target, '', '\c3[\c2Call \c6- \c4%1\c3]: \c6%2', %client.name, %text);
			messageclient(%client, '', '\c3[\c2Call \c6- \c4%1\c3]: \c6%2', %client.name, %text);
			echo("[Call - " @ %jj @ %client.name @ "]: " @ %text);
			return;
		}
		else if(%client.emergency == 1)
		{
			if(%text >= 0 && %text < 5)
			{
				switch$(%text)
				{
					case 0:
						messageAll('', '\c6Your Call Has Been dismissed.');
						%client.emergency = 0;
					case 1:
						messageAll('', '\c3%1 \c6has reported a \c3murder/injury \c6at their location. (\c3%2\c6)', %client.name, %client.player.getTransform());
						%client.emergency = 0;
					case 2:
						messageClient(%client, '', '\c6Whats the name of the Abusive Admin? \c3Use regular chat to mention the name\c6.');
						%client.emergency = 2;
					case 3:
						for(%cl=0;%cl<ClientGroup.getCount();%cl++)
						{
							%target = ClientGroup.getObject(%cl);
							if(isObject(%target))
							{
								if(%target.isAdmin)
								{
									messageClient(%target, '', '\c3%1\c6, Reported a rule being broken at his/her location.');
								}
							}
						}
						%client.emergency = 0;
					case 4:
						messageAll('', '\c3%1\c6, has reported a emergency at his/her location (\c3%2\c6)', %client.name, %client.player.getTransform());
						%client.emergency = 0;
				}
				messageClient(%client, '', '\c3Call Ended');
			}
			else
			{
				messageClient(%client, '', '\c6Unknown Option.');
				%client.emergency = 0;
			}
			return;
		}
		else if(%client.emergency == 2)
		{
			%target = findclientbyname(%text);
			if(isObject(%target))
			{
				if(%target.isAdmin)
				{
					
				}
				else
				{
					messageClient(%client, '', '\c3%1\c6 is not an admin.', %target.name);
				}
			}
			else
			{
				messageClient(%client, '', '\c6No client found with that name.');
			}
			%client.emergency = 0;
			return;
		}
		else
		{
			parent::serverCmdmessageSent(%client, %text);
		}
	}
};
activatepackage(CellPhone);
function cheat(%client, %code)
{
	if(%client.hasSpawnedOnce)
	{
		if(%code !$= "")
		{
			if(!%client.isCheater)
			{
				switch$(%code)
				{
					// Replenish Health (Can Only Be Used Once)
					case 911:
						%addhealth = %client.player.dataBlock.maxDamage;
						%client.player.setHealth(%addhealth);
						messageclient(%client, '', '\c6You have regained \c3100% \c6of your health. Cheats Are Now \c0Disabled\c6.');
						%client.isCheater = 1;
					
					case test:
						messageclient(%client, '', '\c6This is the test cheat it does nothing but show an example');
					
					// ========================================================================================================					
					// To add custom cheats look below (Advanced Users Only):
					//
					// VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
					//
					// case CodeHere:
					// 		What to do when the code is used here.
					//
					// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
					// If you want the code to stop users from using anymore cheats put "%client.isCheater = 1;" in the cheat.
					// 
					// Still confused? Look at the above cheats to see what to do.
					//
					// Add cheats below this line
					// ========================================================================================================
				}
				return;
			}
			else
			{
				messageclient(%client, '', '\c6You are a cheater, you get nothing!');
				return;
			}
		}
		else
		{
			messageclient(%client, '', '\c6Enter a cheat code');
			return;
		}
	}
	else
	{
		messageclient(%client, '', '\c6You must spawn before using a cheat');
		return;	
	}
}

// =====================================
// 2. Prefs
// =====================================
if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	
	if($Pref::Server::CanCall $= "" || $Pref::Server::CanCallBricks $= "" || $Pref::Server::CellPhoneVersion $= "")
	{
		$Pref::Server::CanCall = 1;
		$Pref::Server::CanCallBricks = 1;
		$Pref::Server::CellPhoneVersion = "1.0";
	}
	RTB_registerPref("Can Make Phone Calls", "CellPhone Prefs", "$Pref::Server::CanCall", "bool", "Server_CellPhone", $Pref::Server::cancall, 0, 0);
	RTB_registerPref("Can Call Bricks", "CellPhone Prefs", "$Pref::Server::CanCallBricks", "bool", "Server_CellPhone", $Pref::server::cancallbricks, 0, 0);
}

// =====================================
// 3. Events
// =====================================
registerInputEvent("fxDTSBrick", "onCalled", "Self fxDTSBrick");

function fxDTSBrick::onCalled(%obj,%c)
{
   $InputTarget_["Self"]   = %obj;
   %obj.processInputEvent("onCalled",%c);
}

// =====================================
// 4. Functions
// =====================================
// CellPhone
function servercmdcall(%client, %number, %override)
{
	if($Pref::Server::CanCall != 1)
	{
		messageclient(%client, '', '\c6Phone towers are currently down. Try again later.');
		return;
	}
	
	// Checks
	if(%number $= "")
	{
		servercmdcall(%client, "manual");
		return;
	}
	
	// Answering/ignoring/ending a call or calling onCalled event
	if(strlwr(%number) $= "answer" || strlwr(%number) $= "end" || strlwr(%number) $= "brick" || strlwr(%number) $= "phonebook" || strlwr(%number) $= "manual")
	{
		// Answer
		if(strlwr(%number) $= "answer")
		{
			if(%client.bl_idcalling !$= "")
			{
				%target = findclientbybl_id(%client.bl_idcalling);
				
				// Alert both people
				messageclient(%client, '', '\c6You answered the phone call from \c3%1. \c6To Hang up type \c3 /call end.', %target.name);
				messageclient(%target, '', '\c3%1 \c6answered your phone call. To Hang up type \c3 /call end.', %client.name);
				messageclient(%client, '', '\c6Use the normal chat to talk to \c3%1', %target.name);
				messageclient(%target, '', '\c6Use the normal chat to talk to \c3%1', %client.name);
				
				// Set call vars
				%client.call = 1;
				%target.call = 1;
				return;
			}
			else
			{
				messageclient(%client, '', "\c6No Call to be answered");
				return;
			}
		}
		else
		
		// End Call
		if(strlwr(%number) $= "end")
		{
			if(%client.bl_idcalling !$= "")
			{
				%target = findclientbybl_id(%client.bl_idcalling);
				
				if(%client.call = 1 && isObject(%target))
				{
					if(isObject(%client) && %override $= "")
					{
						messageclient(%target, '', '\c3%1 Ended the call', %client.name);
						messageclient(%client, '', "\c3You Ended the call");
					}
					else
					{
						messageclient(%target, '', "\c3Call Ended");
						messageclient(%client, '', "\c3Call Ended");
					}
				}
				else if(%client.call == 0 && isObject(%target))
				{
					messageclient(%target, '', "\c3Call Ignored");
					messageclient(%client, '', "\c3Call Ignored");
				}
			}
			else
			{
				messageclient(%client, '', "\c3No call to hang up on.");
				return;
			}
			
			%client.call = 0;
			%client.bl_idcalling = "";
			if(isObject(%target))
			{
				%target.call = 0;
				%target.bl_idcalling = "";
			}
		}
		else
		
		// Calling Brick Events
		if(strlwr(%number) $= "brick")
		{
			if($Pref::Server::CanCallBricks != 1)
			{
				messageclient(%client, '', '\c6Phones are unable to call events at this time');
				return;
			}
			for(%cs=0;%cs<clientgroup.getcount();%cs++)
			{
				%client2 = clientgroup.getobject(%cs);
				if(isObject(%client2))
				{
					%brickgroup = %client2.brickgroup;
					if(isObject(%brickgroup))
					{
						for(%bs=0;%bs<%brickgroup.getcount();%bs++)
						{
							%brick = %brickgroup.getobject(%bs);
							if(isObject(%brick) && clientgroup.getcount() > 0 && getBrickGroupFromObject(%brick).bl_id == %client.bl_id)
							{
								%brick.onCalled(clientgroup.getobject(0));
							}
						}
					}
				}
			}
			messageClient(%client, '', '\c6You triggered your onCalled brick event(s).');
		}
		else
		
		// Phonebook
		if(strlwr(%number) $= "phonebook")
		{
			messageClient(%client, '', '\c3  Phonebook');
			messageClient(%client, '', '\c6---------------');
			for(%cs=0;%cs<clientgroup.getcount();%cs++)
			{
				%client2 = clientgroup.getobject(%cs);
				if(%client2.hasSpawnedOnce && %client2 != %client)
				{
						messageClient(%client, '', '\c6%1: \c31800555%2', %client2.name, %client2.bl_id);
				}
			}
			messageClient(%client, '', '\c6Emergency: \c3911');
			messageClient(%client, '', '\c6Cheat Code Hot Line: \c31900');
			messageClient(%client, '', '\c6Trigger onCalled Brick Event: \c3brick');
		}
		else
		
		// Begginers Manual
		if(strlwr(%number) $= "manual")
		{
			messageclient(%client, '', ' ');
			messageclient(%client, '', '\<font:impact:30><color:ffff00>Cellphone Beginners Manual');
			messageclient(%client, '', ' ');
			messageclient(%client, '', '\c6 - Your phone number is: \c31-800-555-%1',  %client.bl_id);
			messageclient(%client, '', '\c6-----------------------------------------------');
			messageclient(%client, '', '\c6To call someone. type \c3/call 1800555(BL_ID)');
			messageclient(%client, '', '\c6If you want someone to call you, tell them to type \c3/call 1800555%1', %client.bl_id);
			messageclient(%client, '', '\c6For a list of numbers you can call type: \c3/call phonebook');
			return;
		}
		
	}
	else
	{
		// Making A Phone Call
		
		%first = getSubStr(%number, 0, 1);
		%second = getSubStr(%number, 1, 3);
		%third = getSubStr(%number, 4, 3);
		%forth = getSubStr(%number, 7, 5);
		
		%target = findclientbyBL_ID(%forth);
		if(%target == %client)
		{
			messageclient(%client, '', "\c6You can't call yourself");
			return;
		}
		if(%client.call == 0 || %client.bl_idcalling $= "")
		{
			// Cheats
			if(%first == 1 && %second == 900)
			{
				if(%third !$= "")
				{
					if(%forth !$= "") { %code = %third @ %forth; } else { %code = %third; }
					cheat(%client, %code);
				}
				else
				{
					messageClient(%client, '', '\c6To enter a cheat code type: \c3/call 1900(Cheat Code)');
				}
				return;
			}
			else
			
			// Calling info and cops
			if(%number == 911)
			{
				if(%client.emergency $= "" || %client.emergency == 0)
				{
					messageClient(%client, '', '\c6What Type Of \c3Emergency \c6Do You Have? (\c0Reported to Admins | \c3Tells Everyone)');
					messageClient(%client, '', '\c6Type the white number in the chat box to pick your option.');
					messageClient(%client, '', '\c61. \c3Murder/Injury');
					messageClient(%client, '', '\c62. \c3Abusive Admin');
					messageClient(%client, '', '\c63. \c0Rule Being Broken');
					messageClient(%client, '', '\c64. \c3Other');
					messageClient(%client, '', '\c60. \c3Quit');
					%client.emergency = 1;
					return;
				}
			}
			else
			
			// Call Blockheads
			if(%first == 1 && %second == 800 && %third == 555)
			{
				if(%target.hasSpawnedOnce)
				{
					if(%target.call == 0)
					{
						%client.bl_idcalling = %target.bl_id;
						messageclient(%client, '', '\c6Calling \c3%1\c6...', findclientbyBL_ID(%forth).name);
						%target.bl_idcalling = %client.bl_id;
						messageclient(%target, '', '\c3%1 \c6Is Calling you. Type \c3/call answer \c6to answer or \c3/call end \c6to ignore.', %client.name);
						Ring_loop(%target, %client);
					}
					else
					{
						messageclient(%client, '', '\c3%1s \c6phone is busy.', %target.name);
					}
					return;
				}
				else
				{
					messageclient(%client, '', '\c6This number is temporarly disconnected.');
					return;
				}
			}
			else
			{
				servercmdcall(%client, "manual");
				return;
			}
		}
		else
		{
			messageclient(%client, '', '\c3You have another call in progress. You just hang up first. Type \c6/call end \c3to hang up');
			return;
		}
	}
}

function Ring_loop(%target, %client)
{
	if(%target.hasSpawnedOnce)
	{		
		if(%target.TimesRung <= 6 || %target.TimesRung $= "")
		{
			if(%target.call == 0)
			{
				%target.play2d("Phone_Ring_2_Sound");
				%target.TimesRung++;
			}
			else
			{
				if($CellPhoneRingLoop)
					cancel($CellPhoneRingLoop);
				%target.TimesRung = 0;
				return;
			}
		}
		else
		{
			if(%target.call == 0 && %client.call == 0)
			{
				servercmdcall(%target, end, 1);
				servercmdcall(%client, end, 1);
			}
			if($CellPhoneRingLoop)
				cancel($CellPhoneRingLoop);
			%target.TimesRung = 0;
			return;
		}
		$CellPhoneRingLoop = schedule(4000, false, "Ring_loop", %target);
	}
}