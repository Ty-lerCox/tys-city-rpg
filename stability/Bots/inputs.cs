////////////////////////
//Event_Botss/inputs.cs//
////////////////////////
//By Amade (ID 10716)

if(!$BotEventsLoaded)
{
	RegisterInputEvent(fxDtsBrick, "onBotActivated", "Self fxDtsBrick" TAB "Client GameConnection" TAB "Player Player" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent(fxDtsBrick, "onBotKill", "Self fxDtsBrick" TAB "Victim Player" TAB "Victim(Client) GameConnection" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent(fxDtsBrick, "onBotKilled", "Self fxDtsBrick" TAB "Killer Player" TAB "Killer(Client) GameConnection" TAB "Corpse Player" TAB "Minigame Minigame");
	RegisterInputEvent(fxDtsBrick, "onBotDamage", "Self fxDtsBrick" TAB "Victim Player" TAB "Victim(Client) GameConnection" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent(fxDtsBrick, "onBotDamaged", "Self fxDtsBrick" TAB "Attacker Player" TAB "Attacker(Client) GameConnection" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent(fxDtsBrick, "onBotsspawn", "Self fxDtsBrick" TAB "Bot Player");
	RegisterInputEvent(fxDtsBrick, "onBotTouched", "Self fxDtsBrick" TAB "Player Player" TAB "Client GameConnection" TAB "Bot Player");
}

package BotEvents
{
	function minigameCanDamage(%a, %b)
	{
		%p = parent::minigameCanDamage(%a, %b);
		if(%p == 1 || !isObject(%a) || !isObject(%b))
		{
			return %p;
		}
		if(%a.getType() & $Typemasks::PlayerObjectType && %b.getType() & $Typemasks::PlayerObjectType)
		{
			%miniA = isObject(%a.client) ? %a.client.minigame : %a.brickGroup.client.minigame;
			%miniB = isObject(%b.client) ? %b.client.minigame : %b.brickGroup.client.minigame;
			if(isObject(%miniA) && %miniA == %miniB && %miniA.teamExists[0])
			{
				%teamA = %a.teamname;
				%teamB = %b.teamname;
				%Aisbot = %a.getClassName() $= "AIplayer";
				%Bisbot = %b.getClassName() $= "AIplayer";
				if(%Aisbot || %Bisbot)
				{
					if(%teamA $= %teamB && %miniA.gameFriendlyFire)
					{
						return 0;
					}
					return 1;
				}
			}
		}
		return %p;
	}
	function fxDTSbrick::spawnVehicle(%this)
	{
		parent::spawnVehicle(%this);
		%this.schedule(33, botVehicleSpawn);
	}
	function fxDTSbrick::botVehicleSpawn(%this)
	{
		if(isObject(%obj = %this.vehicle))
		{
			if(%obj.getClassName() $= "AIplayer" && isObject(%brick = %obj.spawnBrick))
			{
				//Default AI values:
				%obj.inactive = true;
				%obj.aggressive = false;
				%obj.defendself = false;
				%obj.canmove = true;
				%obj.canaim = true;
				%obj.sightrange = 20;
				%obj.fov = 180;
				%obj.leashlength = 50;
				%obj.clawdamage = 0;

				%obj.spawnPosition = %obj.getPosition();
				%data = %obj.getDatablock();
				if(!%obj.isZombie) //This stuff screws with zombies
				{
					%obj.mountImage(BlankImage, 2);
					%obj.mountImage(BlankImage, 3);
					%client = %this.getGroup().client;
					%obj.client = new AIconnection()
					{
						bot = %obj;
						minigame = %client.minigame;
						brickGroup = %client.brickGroup; //So the owner can still ride bot vehicles (like the horse and cannon)
					};
					MissionCleanup.add(%obj.client);
				}
				$InputTarget_["Self"] = %brick;
				$InputTarget_["Bot"] = %obj;
				%this.processInputEvent("onBotsspawn", %this.getGroup().client);
			}
		}
	}
	function armor::onCollision(%this, %obj, %col, %fade, %pos, %normal)
	{
		%p = parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
		if(!isObject(%obj) || !isObject(%col))
		{
			return %p;
		}
		if(%obj.getClassName() $= "AIPlayer")
		{
			if(%obj.hasClaws && minigameCanDamage(%obj, %col))
			{
				%col.damage(%obj, %pos, %obj.clawdamage, $DamageType::Direct);
			}
			if(isObject(%brick = %obj.spawnBrick) && %obj.lastTouchedTime + 200 < getSimTime())
			{
				%obj.lastTouchedTime = getSimTime();
				%client = %col.client;
				%minigame = %client.minigame;
				$InputTarget_["Self"] = %brick;
				$InputTarget_["Player"] = %col;
				$InputTarget_["Client"] = %client;
				$InputTarget_["Bot"] = %obj;
				$InputTarget_["Minigame"] = %minigame;
				%brick.processInputEvent("onBotTouched", %brick.getGroup().client);
			}
		}
	}
	function player::activateStuff(%this)
	{
		%parent = parent::activateStuff(%this);
		%start = %this.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%this.getEyeVector(), 5));
		%ray = containerRayCast(%start, %end, $Typemasks::PlayerObjectType | $Typemasks::FXbrickObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType, %this);
		if(isObject(%hit = getWord(%ray, 0)))
		{
			if(%hit.getClassName() $= "AIPlayer" && isObject(%brick = %hit.spawnBrick))
			{
				%minigame = %this.client.minigame;
				$InputTarget_["Self"] = %brick;
				$InputTarget_["Client"] = %this.client;
				$InputTarget_["Player"] = %this;
				$InputTarget_["Bot"] = %hit;
				$InputTarget_["Minigame"] = %minigame;
				%brick.processInputEvent("onBotActivated", %this.client);
			}
		}
		return %parent;
	}
	function Player::damage(%this, %obj, %pos, %amt, %type)
	{
		if(isObject(%client = %this.client) && %client.getClassName() $= "AIconnection")
		{
			if(%this.getDatablock().maxDamage - %this.getDamageLevel() <= %amt)
			{
				%client.delete();
			}
		}
		%p = parent::damage(%this, %obj, %pos, %amt, %type);
		if(%this.getDamagePercent() >= 1 && %this.getClassName() $= "AIPlayer" && isObject(%brick = %this.spawnBrick))
		{
			%this.setImageTrigger(0, 0);
			%this.setImageTrigger(1, 0);
			if(isObject(%obj))
			{
				switch$(%obj.getClassname())
				{
					case "Player": %killer = %obj;
					case "AIplayer": %killer = %obj;
					case "GameConnection": %killer = %obj.player;
					case "AIconnection": %killer = %obj.player;
					case "Projectile": %killer = %obj.sourceObject;
					case "Vehicle": %killer = %obj.getMountedObject(0);
				}
			}
			$InputTarget_["Self"] = %brick;
			$InputTarget_["Killer"] = %killer;
			$InputTarget_["Killer(Client)"] = %killer.client;
			$InputTarget_["Corpse"] = %this;
			$InputTarget_["Minigame"] = %brick.getGroup().client.minigame;
			%brick.processInputEvent("onBotKilled", %brick.getGroup().client);
		}
		if(isObject(%obj) && isObject(%this))
		{
			switch$(%obj.getClassname())
			{
				case "GameConnection": %obj = %obj.player;
				case "AIconnection": %obj = isObject(%obj.player) ? %obj.player : %obj.bot;
				case "Projectile": %obj = %obj.sourceobject;
			}
		}
		else
		{
			return %p;
		}
		if(isObject(%brick = %this.spawnBrick) && %obj != %this && strLen(%type) && %this.getClassName() $= "AIplayer")
		{
			if(%this.lastAttackedEvent + 100 < (%time = getSimTime()))
			{
				if(%this.target != %obj)
				{
					%mode = %this.getAImode();
					if(%mode $= "aggressive" || %mode $= "defendself")
					{
						%this.target = %obj;
						%this.setMoveObject(%obj);
						%this.setAimObject(%obj);
					}
				}
				%this.lastAttackedEvent = %time;
				%this.lastAttacker = %obj;
				$InputTarget_["Self"] = %brick;
				$InputTarget_["Attacker"] = %this.lastAttacker;
				$InputTarget_["Attacker(Client)"] = %obj.client;
				$InputTarget_["Bot"] = %this;
				$InputTarget_["Minigame"] = %obj.client.minigame;
				%brick.processInputEvent("onBotDamaged", %obj.client);
			}
		}
		if(isObject(%brick = %obj.spawnBrick) && %this != %obj && strLen(%type) && %obj.getClassName() $= "AIplayer")
		{
			if(%obj.lastAttackEvent + 100 < (%time = getSimTime()))
			{
				%obj.lastAttackEvent = %time;
				%obj.lastAttacker = %this;
				$InputTarget_["Self"] = %brick;
				$InputTarget_["Victim"] = %this;
				$InputTarget_["Victim(Client)"] = %this.client;
				$InputTarget_["Bot"] = %obj;
				$InputTarget_["Minigame"] = %this.client.minigame;
				%brick.processInputEvent("onBotDamage", %obj.client);
			}
		}
		return %p;
	}
	function paintProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
	{
		if($Pref::Server::DisableBotPainting && %col.getClassName() $= "AIplayer" && isObject(%col.spawnBrick) && %col.appearanceModified)
		{
			return;
		}
		parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
	}
	function Player::instantRespawn(%this)
	{
		if(isObject(%brick = %this.spawnBrick) && !%this.client.bl_id)
		{
			%brick.spawnVehicle();
		}
		else
		{
			parent::instantRespawn(%this);
		}
	}
	function AIplayer::setDatablock(%this, %newData)
	{
		if(isObject(%client = %this.client))
		{
			%this.client = "";
			%res = 1;
		}
		%parent = parent::setDatablock(%this, %newData);
		if(%res)
		{
			%this.client = %client;
		}
		return %parent;
	}
	function WandImage::onFire(%this, %obj, %slot)
	{
		if(isObject(%obj))
		{
			if(%obj.getClassName() $= "AIplayer")
			{
				return;
			}
		}
		return parent::onFire(%this, %obj, %slot);
	}
	function HammerImage::onFire(%this, %obj, %slot)
	{
		if(isObject(%obj))
		{
			if(%obj.getClassName() $= "AIplayer")
			{
				return;
			}
		}
		return parent::onFire(%this, %obj, %slot);
	}
	//for some reason this doesn't work
	// function getTrustLevel(%a, %b) //to prevent crashes if Botss should destroy their own bricks
	// {
		// %p = parent::getTrustLevel(%a, %b);
		// if(%p < 2 || !isObject(%a) || !isObject(%b) || %a.circumventBEtrust)
		// {
			// return %p;
		// }
		// if(getSubStr(%a.getClassName(), 0, 2) $= "AI")
		// {
			// return 0;
		// }
		// return %p;
	// }
	function GameConnection::SpawnPlayer(%this)
	{
		parent::SpawnPlayer(%this);
		%obj = %this.player;
		if(%this.minigame.teamExists[0])
		{
			%obj.teamname = %this.minigame.teamName[%this.tdmteam];
		}
	}
};
activatePackage(BotEvents);

//This isn't packaged because it's rewriting the function in a way that would break other packages to it
function FxDTSbrick::onPlayerTouch(%this, %obj) //Only editing this because with the way I made Botss damage it considers them players too
{
	//Basic passing on of arguments to datablock's ::onPlayerTouch
	%this.getDatablock().onPlayerTouch(%this, %obj);

	if(%this.numEvents <= 0)
	{
		return;
	}
	$InputTarget_["Self"] = %this;
	if(%obj.getClassName() $= "Player")
	{
		%client = %obj.client;
		$InputTarget_["Client"] = %client;
		$InputTarget_["Minigame"] = %client;
		$InputTarget_["Player"] = %obj;
		%this.processInputEvent("onPlayerTouch", %client);
	}
	if(%obj.getClassName() $= "AIplayer")
	{
		$InputTarget_["Minigame"] = %obj.client.minigame; //Bot's minigame
		if(isObject(%driver = %obj.getMountedObject(0)))
		{
			$InputTarget_["Driver"] = %driver;
			if(isObject(%client = %driver.getControllingClient()))
			{
				$InputTarget_["Client"] = %client;
				$InputTarget_["Minigame"] = %client.minigame;
			}
		}
		$InputTarget_["Bot"] = %obj;
		%this.processInputEvent("onBotTouch", %this.getGroup().client);
	}
}