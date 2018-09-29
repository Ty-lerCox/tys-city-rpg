exec("./overrides.cs");
exec("./RTBPrefs.cs");
exec("./item_rape.cs");

package rapePackage
{
	function Armor::onTrigger(%datablock, %obj, %slot, %value)
	{
		if(%obj.isBeingRaped && %slot == $Pref::RapeMod::Key && isObject(%obj) && isObject(%obj.client.rapist.player))
		{
			if(%obj.bars < 50 && %obj.remainingBars > 0)
			{
				%obj.remainingBars--;
				%obj.bars++;
				%obj.client.centerPrint("<just:center><color:FF0000>Free yourself!<color:FFFF00>" SPC mFloor((%obj.bars / 50) * 100) @ "<color:FF0000>%" , 1);
			}
			else if(%obj.bars >= 50 && %obj.remainingBars <= 0)
			{
				
				%obj.client.centerPrint("<just:center><color:00FF00>Freedom!", 2);
				
				%obj.client.rapist.centerPrint("<just:center><color:FF0000>They escaped!", 3);
				
				%obj.client.rapist.player.emote(hateImage);
				
				%obj.client.rapist.player.addVelocity(getRandom(-10, 10) SPC getRandom(-10, 10) SPC getRandom(5, 10));
				
				%obj.client.rapist.player.schedule(1000, kill);
				
				%obj.bars = 0;
				%obj.remainingBars = 50;
				
				%obj.client.rapist.player.bars = 0;
				%obj.client.rapist.player.remainingBars = 50;
				
				%obj.playThread(0, root);
				%obj.client.rapist.player.playThread(0, root);
				
				cancel(%obj.freezeLoop);
				cancel(%obj.client.rapist.freezeLoop);
				
				%obj.isBeingRaped = 0;
				%obj.client.rapist.player.isRaping = 0;
				
				%obj.client.rapist.rapeVictim = "";
				%obj.client.rapist = "";
				
				
			}
			else
			{
				%obj.bars = 0;
				%obj.remainingBars = 50;
			}
			return;
		}
		
		
		else if(%obj.isRaping && %slot == $Pref::RapeMod::Key && isObject(%obj) && isObject(%obj.client.rapeVictim.player))
		{
			if(%obj.bars < 50 && %obj.remainingBars > 0)
			{
				%obj.remainingBars--;
				%obj.bars++;
				%obj.client.centerPrint("<just:center><color:FF00FF>Penetrate them!<color:FFFF00>" SPC mFloor((%obj.bars / 50) * 100) @ "<color:FF00FF>%", 1);
			}
			else if(%obj.bars >= 50 && %obj.remainingBars <= 0)
			{			
				
				if(!%obj.client.rapeVictim.player.hasSTD)
				{
					%obj.client.rapeVictim.centerPrint("<just:center><color:FF0000>You now have an STD!", 3);
					%obj.client.rapeVictim.player.hasSTD = 1;
				}
				
				%obj.emote(loveImage);
				%obj.client.rapeVictim.player.emote(hateImage);
				
				%obj.client.centerPrint("<just:center><color:00FF00>Penetration!", 2);
				
				%obj.bars = 0;
				%obj.remainingBars = 50;
				
				%obj.client.rapeVictim.player.bars = 0;
				%obj.client.rapeVictim.player.remainingBars = 50;
				
				%obj.playThread(0, root);
				%obj.client.rapeVictim.player.playThread(0, root);
				
				cancel(%obj.freezeLoop);
				cancel(%obj.client.rapeVictim.player.freezeLoop);
				
				%obj.isRaping = 0;
				%obj.client.rapeVictim.player.isBeingRaped = 0;
				
				%obj.client.rapeVictim.rapist = "";
				%obj.client.rapeVictim = "";
				
			}
			else
			{
				%obj.bars = 0;
				%obj.remainingBars = 50;
			}
			
			return;
			
		}
		return Parent::onTrigger(%datablock, %obj, %slot, %value);
	}	
};
activatePackage(rapePackage);

function serverCmdRape(%c, %t)
{
	if((CityRPGData.getData(%c.bl_id).valueJobID == 5) || (CityRPGData.getData(%c.bl_id).valueJobID == 16) || (CityRPGData.getData(%c.bl_id).valueJobID == 17) || (CityRPGData.getData(%c.bl_id).valueJobID == 18))
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 0))
		{
			%t = findClientByName(%t);
			if($Pref::RapeMod::MiniGameOnly == 1 && (%t.minigame != %c.minigame || !isObject(%t.minigame)))
			{
				return;
			}
			
			initContainerRadiusSearch(%c.player.getPosition(), 2, $Typemasks::PlayerObjectType);
			
			while((%search = containerSearchNext()) != 0)
			{
				if(%search == %t.player)
				{
					RapePlayer(%c, %t);
				}
			}
		}
	} else {
		messageClient(%c, '', "\c6You must have a job as: ciminal, mobster, mobboss, godfather");
	}
	
}

function RapePlayer(%c, %t)
{
	if(isObject(%t.player) && isObject(%c.player) && %t != %c && !%t.player.isBeingRaped && !%c.player.isRaping && !%c.player.isBeingRaped && !%t.player.isRaping && $Pref::RapeMod::Enabled == 1)
	{
		if($Rape::Demerits > 1)
			$Rape::Demerits = 300;
		CityRPGData.getData(%c.bl_id).valueDemerits += $Rape::Demerits;
		%t.player.dismount();
		%c.player.dismount();
		
		%pos = %t.player.getPosition();
		
		if(%telePos $= "" && %telePos2 $= "")
		{
			%telePos = %pos;
			%telePos2 = getWord(%pos, 0) SPC getWord(%pos, 1) SPC getWord(%pos, 2) + 1;
			
		}
		
		if($Pref::RapeMod::Key == 2)
		{
			%key = "tap jump";
		}
		else
		{
			%key = "click";
		}
		
		%c.centerPrint("<just:center><color:FFFFFF>You are now raping somebody!<br>Repeatedly" SPC %key SPC "to penetrate them!", 5);
		%t.centerPrint("<just:center><color:FFFFFF>You are now being raped!<br>Repeatedly" SPC %key SPC "to escape!", 5);
		
		%c.player.setTransform(getWord(%pos, 0) SPC getWord(%pos, 1) SPC getWord(%pos, 2) + 1);
		
		%c.player.playThread(0, crouchBack);
		%t.player.playThread(0, crouch);
		
		%t.player.isBeingRaped = 1;
		%c.player.isRaping = 1;
		
		%t.rapist = %c;
		%c.rapeVictim = %t;
		
		%t.player.freeze(%telePos);
		%c.player.freeze(%telePos2); 
		
	}
}

//thanks to []----[] for this function
//function Player::ForceLook(%this, %pitch)
//{
	//%this.pitchAI = new AIConnection();
	//%this.pitchAI.setControlObject(%this);
	//%this.pitchAI.setMove("pitch", %pitch);
	//%this.pitchAI.setControlObject(%this);
	//%this.client.schedule(30, setControlObject, %this);
	//%this.pitchAI.schedule(100, delete);
//}
//nope

function Player::Freeze(%this, %location)
{
	cancel(%this.freezeLoop);
	
	%this.setTransform(%location);
	%this.setVelocity("0 0 0");
	%this.freezeLoop = %this.schedule(1, freeze, %location);
	
}

function STDLoop()
{
	if(isEventPending($STDLoop))
	{
		cancel($STDLoop);
	}

	%count = ClientGroup.getCount();
	for(%i = 0; %i < %count; %i++)
	{
		%c = ClientGroup.getObject(%i);
		if(!isObject(%c.player))
		{
			continue;
		}
		
		if(%c.player.hasSTD)
		{
		
			%c.player.addHealth("-10");
			%c.bottomPrint("<color:FF0000>You have an STD!", 4);
	
		}
	}
	
	$STDLoop = schedule(5000, 0, STDLoop);
}


function serverCmdStopRape(%c)
{
	if(%c.player.isRaping)
	{
		%c.rapeVictim.player.remainingBars = 50;
			
		%c.rapeVictim.player.bars = 0;
			
		%c.rapeVictim.player.playThread(0, root);
			
		%c.rapeVictim.player.isBeingRaped = 0;
			
		%c.rapeVictim.rapist = "";
			
		%c.rapeVictim.centerPrint("<color:00FF00>Your rapist has stopped raping you!", 4);
			
		cancel(%c.rapeVictim.player.freezeLoop);
		
		%c.player.remainingBars = 50;
			
		%c.player.bars = 0;
			
		%c.player.playThread(0, root);
			
		%c.player.isRaping = 0;
			
		%c.rapeVictim = "";
			
		%c.centerPrint("<color:FFFFFF>You have stopped raping!", 4);
			
		cancel(%c.player.freezeLoop);
	}
}
		
		