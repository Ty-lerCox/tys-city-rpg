////////////////////////
//Event_Botss/server.cs//
////////////////////////
//By Amade (ID 10716)

// VERSION 8.0 //

if($BotEventsLoaded)
{
	//exec("./inputs.cs");
	exec("./outputs.cs");
	exec("./AI.cs");
	return;
}

////////////
//RTB PREF//
////////////

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	RTB_registerPref("Disable Bot Painting","Bot Events","$Pref::Server::DisableBotPainting","bool","Event_Botss", true, false, false); //Pref option name, pref category, pref variable, pref type (same as event registry), add-on name, default value, requires restart, host only
}
else if($Pref::Server::DisableBotPainting != true && $Pref::Server::DisableBotPainting != false)
{
	$Pref::Server::DisableBotPainting = true; //If you don't have RTB, enter the value here: true / false
}

//////////////
//DATABLOCKS//
//////////////
datablock ShapeBaseImageData(BlankImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	mountPoint = 2;
};

datablock playerData(BotsstandardArmor : playerStandardArmor)
{
	uiName = "Standard Bot";
	rideable = true;
};

/////////////////
//SPECIAL KILLS//
/////////////////
exec("./Support_SpecialKills.cs");

addSpecialDamageMsg("Bot","%4%2%3%1","%4%2%3%1");

function isSpecialKill_Bot(%this, %sObj, %sClient, %mini)
{
	if(isObject(%sObj))
	{
		switch$(%sObj.getClassName())
		{
			case "AIconnection": %c = %sObj;
			case "AIplayer": %c = %sObj.client;
			case "Projectile": %c = %sObj.client ? %sObj.client : 0;
		}
		if(%c != %this && isObject(%c) && %c.getClassName() $= "AIconnection")
		{
			return 2 TAB %c.name;
		}
	}
	return 0;
}

//////////////////
//HELP FUNCTIONS//
//////////////////

//Tell the client the nodes/decals they are using (preferences, not current)
function serverCmdAppearanceHelp(%client)
{
	messageClient(%client, 'chatMessage',"\c2Decal: \c3" @ %client.decalName @ "\c2 Face: \c3" @ %client.faceName @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Hat: \c3" @ $hat[%client.hat] @ "\c2 Accent: \c3" @ $accent[%client.accent] @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Head: \c3Headskin\c2.");
	messageClient(%client, 'chatMessage',"\c2Shoulder: \c3" @ $secondpack[%client.secondpack] @ "\c2 Back: \c3" @ $pack[%client.pack] @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Right Arm: \c3" @ $rarm[%client.rarm] @ "\c2 Left Arm: \c3" @ $larm[%client.larm] @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Right Hand: \c3" @ $rhand[%client.rhand] @ "\c2 Left Hand: \c3" @ $lhand[%client.lhand] @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Hip: \c3" @ $hip[%client.hip] @ " \c2Chest: \c3" @ $chest[%client.chest] @ "\c2.");
	messageClient(%client, 'chatMessage',"\c2Right Leg: \c3" @ $rleg[%client.rleg] @ "\c2 Left Leg: \c3" @ $lleg[%client.lleg] @ "\c2.");
}

//Thanks to Space Guy for this function. Used in /applyAv
function getClosestPaintColor(%rgba)
{
	%prevdist = 100000;
	%colorMatch = 0;
	for(%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		if(vectorDist(%rgba, getWords(%color, 0, 2)) < %prevdist && getWord(%rgba, 3) - getWord(%color, 3) < 0.3 && getWord(%rgba, 3) - getWord(%color, 3) > -0.3)
		{
			%prevdist = vectorDist(%rgba,%color);
			%colormatch = %i;
		}
	}
	return %colormatch;
}

function FxDTSbrick::getOutputParameter(%this, %event, %param)
{
	return %this.eventOutputParameter[%event, %param];
}
function FxDTSbrick::setOutputParameter(%this, %eventN, %paramN, %value)
{
	%this.eventOutputParameter[%eventN, %paramN] = %value;
}

//Auto-eventer, applies events to a brick giving the bot their avatar
function serverCmdApplyAv(%client)
{
	return; //not in use
	if($OutputEvent_NamePlayer_[$EditAppearance] !$= "EditAppearance")
	{
		initApplyAv();
	}
	if(isObject(%brick = %client.wrenchBrick))
	{
		if(%client.lastApplyAv + 5000 >= getSimTime())
		{
			%client.centerPrint("<color:FF0000>You may not apply your avatar more than once every 5 seconds!", 5);
			return;
		}
		%client.lastApplyAv = getSimTime();
		for(%i = 0; %i < %brick.numEvents; %i++)
		{
			echo(%brick.eventOutput[%i]);
			if(%brick.eventInput[%i] == "OnBotsspawn")// && %brick.eventOutput[%i] == $EditAppearance)
			{
				continue;
			}
			%eventDelay[%i] = %brick.eventDelay[%i];
			%eventEnabled[%i] = %brick.eventEnabled[%i];
			%eventInput[%i] = %brick.eventInput[%i];
			%eventInputIdx[%i] = %brick.eventInputIdx[%i];
			%eventOutput[%i] = %brick.eventOutput[%i];
			%eventOutputAppendClient[%i] = %brick.eventOutputAppendClient[%i];
			%eventOutputIdx[%i] = %brick.eventOutputIdx[%i];
			for(%j = 1; %brick.getOutputParameter(%i, %j) !$= ""; %j++)
			{
				%eventOutputParameter[%i, %j] = %brick.getOutputParameter(%i, %j);
			}
			%eventTarget[%i] = %brick.eventTarget[%i];
			%eventTargetIdx[%i] = %brick.eventTargetIdx[%i];
		}
		%oldNumEvents = %i;
		serverCmdClearEvents(%client);
		%blanks = 9;
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 1, "ALL", 0);
		if(%client.hat)
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $Hat[%client.hat], getClosestPaintColor(%client.hatColor));
			%blanks++;
			if(%client.accent)
			{
				serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, getWord($AccentsAllowed[$hat[%client.hat]], %client.accent), getClosestPaintColor(%client.accentColor));
			%blanks++;
			}
		}
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, "headSkin", getClosestPaintColor(%client.headColor));
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $Chest[%client.chest], getClosestPaintColor(%client.chestColor));
		if(%client.secondPack)
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $SecondPack[%client.secondpack], getClosestPaintColor(%client.secondPackColor));
			%blanks++;
		}
		if(%client.pack)
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $Pack[%client.pack], getClosestPaintColor(%client.packColor));
			%blanks++;
		}
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $LArm[%client.larm], getClosestPaintColor(%client.larmColor));
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $RArm[%client.rarm], getClosestPaintColor(%client.rarmColor));
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $LHand[%client.lhand], getClosestPaintColor(%client.lhandColor));
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $RHand[%client.rhand], getClosestPaintColor(%client.rhandColor));
		serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $hip[%client.hip], getClosestPaintColor(%client.hipColor));
		if(%client.hip == 0)
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $LLeg[%client.lleg], getClosestPaintColor(%client.llegColor));
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, $RLeg[%client.rleg], getClosestPaintColor(%client.rlegColor));
		}
		else //Skirt
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, "SkirtTrimLeft", getClosestPaintColor(%client.llegColor));
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 2, "SkirtTrimRight", getClosestPaintColor(%client.rlegColor));
		}
		if(%client.decalName !$= "AAA-None")
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 3, %client.decalName, 0);
			%blanks++;
		}
		if(%client.faceName !$= "smiley")
		{
			serverCmdAddEvent(%client, 1, $OnBotsspawn, 0, 1, "", $EditAppearance, 4, %client.faceName, 0);
			%blanks++;
		}
		for(%i = 0; %i < %oldNumEvents; %i++)
		{
			if(%eventInput[%i] $= "")
			{
				%blanks++;
				continue;
			}
			echo(%eventDelay[%i] SPC %eventEnabled[%i] SPC %eventInput[%i] SPC %eventOutputAppendClient[%i] SPC %eventOutputIdx[%i] SPC %eventOutputParameter[%i, 0] SPC %eventTarget[%i] SPC %eventTargetIdx[%i]);
			%brick.eventDelay[%i + %blanks] = %eventDelay[%i];
			%brick.eventEnabled[%i + %blanks] = %eventEnabled[%i];
			%brick.eventInput[%i + %blanks] = %eventInput[%i];
			%brick.eventInputIdx[%i + %blanks] = %eventInputIdx[%i];
			%brick.eventOutput[%i + %blanks] = %eventOutput[%i];
			%brick.eventOutputAppendClient[%i + %blanks] = %eventOutputAppendClient[%i];
			%brick.eventOutputIdx[%i + %blanks] = %eventOutputIdx[%i];
			for(%j = 1; %eventOutputParameter[%i, %j] !$= ""; %j++)
			{
				%brick.eventOutputParameter[%i + %blanks, %j] = %eventOutputParameter[%i, %j];
			}
			%brick.eventTarget[%i + %blanks] = %eventTarget[%i];
			%brick.eventTargetIdx[%i + %blanks] = %eventTargetIdx[%i];
		}
		%brick.numEvents = %oldNumEvents + %blanks;
	}
}

//Find the event numbers for input "OnBotsspawn" and output "EditAppearance"
function initApplyAv()
{
	%i = -1;
	while($InputEvent_NamefxDTSBrick_[%i++] !$= "OnBotsspawn")
	{
		continue;
	}
	$OnBotsspawn = %i;
	%i = -1;
	while(strLwr($OutputEvent_NamePlayer_[%i++]) !$= "editappearance")
	{
		continue;
	}
	$EditAppearance = %i;
}

/////////////////
//OTHER SCRIPTS//
/////////////////
//exec("./inputs.cs");
exec("./outputs.cs");
exec("./AI.cs");