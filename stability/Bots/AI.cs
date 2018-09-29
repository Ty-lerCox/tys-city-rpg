////////////////////
//Event_Botss/AI.cs//
////////////////////
//By Amade (ID 10716)

//Event registry
if(!$BotEventsLoaded)
{
	if(isFunction("RegisterSpecialVar"))
	{
		RegisterSpecialVar("Player", "sightrange", "%this.sightrange");
		RegisterSpecialVar("Player", "fieldofview", "%this.fov");
		RegisterSpecialVar("Player", "fov", "%this.fov");
		RegisterSpecialVar("Player", "leashlength", "%this.leashlength");
		RegisterSpecialVar("Player", "leash", "%this.leashlength");
		RegisterSpecialVar("Player", "aimode", "%this.getAImode()");
	}

	RegisterInputEvent("FxDtsBrick", "onBotDetectAlly", "Self fxDtsBrick" TAB "Client GameConnection" TAB "Player Player" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent("FxDtsBrick", "onBotDetectEnemy", "Self fxDtsBrick" TAB "Client GameConnection" TAB "Player Player" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent("FxDtsBrick", "onBotChaseTarget", "Self fxDtsBrick" TAB "Client GameConnection" TAB "Player Player" TAB "Bot Player" TAB "Minigame Minigame");
	RegisterInputEvent("FxDtsBrick", "onBotLoseTarget", "Self fxDtsBrick" TAB "Client GameConnection" TAB "Player Player" TAB "Bot Player" TAB "Minigame Minigame");

	registerOutputEvent("FxDtsBrick", "SetBotWeapon", "list Closerange 0 Longrange 1" TAB "datablock itemData");
	registerOutputEvent("FxDtsBrick", "SetBotAiValues", "list SightRange 0 FieldOfView 1 Leash 2 ClawDamage 3 HorizLookspeed 4 VertLookspeed 5" TAB "int 0 360");
	registerOutputEvent("FxDtsBrick", "SetBotAiProperties", "list Aggressive 0 DefendsSelf 2 Inactive 1 AttackTeam 4 HasClaws 5 PrefersRanged 8 CanMove 6 CanAim 7" TAB "bool" TAB "string 50 70", true);
	registerOutputEvent("Player", "SetBotWeapon", "list Closerange 0 Longrange 1" TAB "datablock itemData");
	registerOutputEvent("Player", "SetBotAiValues", "list SightRange 0 FieldOfView 1 Leash 2 ClawDamage 3 HorizLookspeed 4 VertLookspeed 5" TAB "int 0 360");
	registerOutputEvent("Player", "SetBotAiProperties", "list Aggressive 0 DefendsSelf 2 Inactive 1 AttackTeam 4 HasClaws 5 PrefersRanged 8 CanMove 6 CanAim 7" TAB "bool" TAB "string 50 70", true);
}

function FxDtsBrick::SetBotWeapon(%this, %type, %item)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.SetBotWeapon(%type, %item);
		}
	}
}
function FxDtsBrick::SetBotAiValues(%this, %var, %val)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.SetBotAiValues(%this, %var, %val);
		}
	}
}
function FxDtsBrick::SetBotAiProperties(%this, %prop, %bool, %team, %client)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.SetBotAiProperties(%this, %prop, %bool, %team, %client);
		}
	}
}

//Omg supergiant AI function
function Player::BotEvents_AIsched(%this)
{
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	cancel(%this.jumpsched);
	cancel(%this.BotAttackLoop);
	%schedlen = getRandom(1250, 2000);
	%this.BotAttackLoop = %this.schedule(%schedlen, BotEvents_AIsched);
	%tepos = %this.getEyePoint();
	%tlpos = %this.getPosition();
	if(vectorDist(%tlpos, %this.spawnPosition) > %this.leashlength && %this.canMove)
	{
		if(isObject(%this.target))
		{
			OnBotLoseTarget(%this, %this.target);
		}
		%this.target = 0;
		%this.setMoveDestination(%this.spawnPosition);
		jumpToZ(%this, getWord(%this.spawnPosition, 2));
		%this.setImageTrigger(0, 0);
		%this.setImageTrigger(1, 0);
		%this.setMoveObject(0);
		%this.clearAim();
		return;
	}
	%hasTarget = (isObject(%target = %this.target) && %target.getState() !$= "Dead" && vectorDist(%tepos, %targpos = %target.getPosition()) <= %this.sightrange * 2);
	if(%hasTarget)
	{
		%dist = vectorDist(%tlpos, %targpos);
		if(%dist > 5)
		{
			if(isObject(%image = %this.item[1]))
			{
				%this.updateArm(%image);
				%this.mountImage(%image, 0);
			}
		}
		else if(isObject(%image = %this.item[0]))
		{
			%this.updateArm(%image);
			%this.mountImage(%image, 0);
		}
		if(BotCanSee(%this, %target))
		{
			if(%dist > 5 && %dist < 25 && %this.prefersRanged)
			{
				%this.setMoveObject(0);
				%this.stop();
			}
			else if(%this.canMove)
			{
				%this.setMoveObject(%target);
			}
			%this.setImageTrigger(0, 1);
			%this.schedule(%schedlen * 1/4, setImageTrigger, 1, 1);
			%this.schedule(%schedlen * 3/4, setImageTrigger, 0, 0);
			%this.schedule(%schedlen, setImageTrigger, 1, 0);
			%this.lastSeenTarget = getSimTime();
			%this.lastSeenTargetPos = %targpos;
			if(%this.canMove)
			{
				switch$(BotPathfind(%this, %target))
				{
					case "Jump": %this.setImageTrigger(2, 1); %this.schedule(%schedlen / 2, setImageTrigger, 2, 0);
					case "Crouch": %this.setImageTrigger(3, 1); %this.schedule(%schedlen / 2, setImageTrigger, 3, 0);
				}
			}
			%noWander = true;
		}
		else
		{
			%this.setImageTrigger(0, 0);
			%this.setImageTrigger(1, 0);
			%this.setMoveObject(0);
			%this.clearAim();
			if(%this.lastSeenTarget + 5000 < getSimTime())
			{
				%hasTarget = false;
				if(isObject(%this.target))
				{
					OnBotLoseTarget(%this, %this.target);
				}
			}
			if(%this.canMove)
			{
				%this.setMoveDestination(%this.lastSeenTargetPos);
			}
			%noWander = true;
		}
	}
	if(!%hasTarget)
	{
		%this.setImageTrigger(0, 0);
		%this.setImageTrigger(1, 0);
		%this.lastSeenTarget = 0;
		%this.lastSeenTargetPos = 0;
		%this.target = 0;
		%this.setMoveObject(0);
		%this.clearAim();
	}
	InitContainerRadiusSearch(%tepos, %this.sightrange, $Typemasks::PlayerObjectType);
	while(%hit = ContainerSearchNext())
	{
		if(BotCanSee(%this, %hit) && %hit.getState() !$= "Dead")
		{
			%hmini = isObject(%hit.client) ? %hit.client.minigame : %hit.brickGroup.client.minigame;
			if(%hmini == %this.client.minigame)
			{
				%hpos = %hit.getHackPosition();
				%eyevec = %this.getEyeVector();
				%dvec = vectorNormalize(vectorSub(%hpos, %tepos));
				%nFOV = mAcos(vectorMult(%eyevec, %dvec)) * 90;
				if(%nFOV <= %this.fov)
				{
					if(BotsshouldAttack(%this, %hit))
					{
						OnBotDetectEnemy(%this, %hit);
						if(!%haveenemy)
						{
							%this.target = %hit;
							if(%this.canMove && (vectorDist(%hpos, %tepos) > 5 && !(vectorDist(%hpos, %tepos) < 25 && %this.prefersRanged) && isObject(%this.item[0])))
							{
								%this.setMoveObject(%hit);
							}
							%this.setAimObject(%hit);
							OnBotChaseTarget(%this, %hit);
							%haveenemy = 1;
						}
					}
					else
					{
						OnBotDetectAlly(%this, %hit);
					}
				}
			}
		}
	}
	if(!%noWander && (%this.canMove || %this.canAim))
	{
		%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 50 SPC (getRandom() - 0.5) * 50 SPC 0);
		while(isObject(firstWord(containerRaycast(%tlpos, %dest, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleBlockerObjectType, %this))))
		{
			%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 20 SPC (getRandom() - 0.5) * 20 SPC 0);
			if(%i++ > 10) //This is here so if the object is encased in bricks or something the server doesn't crash
			{
				break;
			}
		}
		if(%this.canMove)
		{
			%this.setMoveDestination(%dest, false);
		}
		else if(%this.canAim)
		{
			%this.setAimLocation(%dest);
		}
	}
}

function Player::SetBotWeapon(%this, %type, %item)
{
	if(%this.getClassName() $= "AIplayer")
	{
		%this.item[%type] = %item.image;
		if(!isObject(%this.getMountedImage(0)) && isObject(%item.image))
		{
			%this.mountImage(%item.image, 0);
		}
		if(isObject(%this.getMountedImage(0)) && !isObject(%item.image))
		{
			%this.unmountImage(0);
		}
		FixArmReady(%this);
	}
}

function Player::SetBotAiValues(%this, %var, %value)
{
	if(%this.getClassName() $= "AIplayer")
	{
		switch(%var)
		{
			case 0: %this.sightrange = %value;
			case 1: %this.fov = %value;
			case 2: %this.leashlength = %value;
			case 3: %this.clawdamage = %value;
			case 4: %this.maxYawSpeed = %value;
			case 5: %this.maxPitchSpeed = %value;
		}
	}
}

function Player::SetBotAiProperties(%this, %prop, %bool, %team, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%team = filterVariableString(%team, %brick, %client, %client.player);
	}
	if(%this.getClassName() $= "AIplayer")
	{
		if(%bool && %prop < 5)
		{
			%this.aggressive = 0;
			%this.inactive = 0;
			%this.defendself = 0;
			if(%prop == 1 && %bool)
			{
				cancel(%this.BotAttackLoop);
				if(%this.getMoveObject() == %this.target)
				{
					%this.setMoveObject(0);
					%this.stop();
				}
				if(%this.getAimObject() == %this.target)
				{
					%this.clearAim();
				}
				%this.target = "";
			}
			else if(!isEventPending(%this.BotAttackLoop) && %bool)
			{
				%this.BotAttackLoop = %this.schedule(33, BotEvents_AIsched);
			}
		}
		switch(%prop)
		{
			case 0: %this.aggressive = %bool;
			case 1: %this.inactive = %bool;
			case 2: %this.defendself = %bool;
			case 4: %this.killteam[%team] = %bool; %this.defendself = 1;
			case 5: %this.hasclaws = %bool;
			case 6: %this.canmove = %bool;
			case 7: %this.canaim = %bool;
			case 8: %this.prefersRanged = %bool;
		}
	}
}

function Player::GetAImode(%this)
{
	return (%this.aggressive ? "aggressive" : (%this.defendself ? "defendself" : "inactive"));
}

function BotPathfind(%obj, %target)
{
	%oh = %obj.getEyePoint();
	%th = %target.getEyePoint();
	%tl = vectorAdd(%target.getPosition(), "0 0 0.1");
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%oh, %th, %typemasks | %types, %obj);
	%hit = firstWord(%headRaycast);
	if(isObject(%hit))
	{
		%headCanSee = %hit == %target;
	}
	%legRaycast = containerRaycast(%oh, %tl, %typemasks | %types, %obj);
	%hit = firstWord(%legRaycast);
	if(isObject(%hit))
	{
		%legsCanSee = %hit == %target;
	}
	if(%legsCanSee && !%headCanSee)
	{
		return "Crouch";
	}
	if(%headCanSee && !%legsCanSee)
	{
		return "Jump";
	}
	%oz = getWord(%obj.getPosition(), 2) + 0.1;
	%tz = getWord(%tl, 2);
	%ow = %obj.getWaterCoverage() > 0.5 ? 1 : 0;
	%tw = %target.getWaterCoverage() > 0.5 ? 1 : 0;
	if(%ow && %tw)
	{
		if(%oz < %tz)
		{
			return "Crouch";
		}
		if(%tz < %oz)
		{
			return "Jump";
		}
		return "None";
	}
	if(%tz > %oz + 0.6)
	{
		return "Jump";
	}
	return "None";
}
function BotsshouldAttack(%obj, %target)
{
	if(!isObject(%obj) || !isObject(%target) || %obj == %target || %obj.inactive)
	{
		return 0;
	}
	%oteam = %obj.teamname;
	%tteam = %target.teamname;
	//This is commented out because I can't easily make all zombies consider Botss on the zombie friendly.
	//if(%target.isZombie && (%oteam $= "zombie" || %oteam $= "zombies"))
	//{
		//return 0;
	//}
	if(%oteam $= %tteam && strLen(%oteam) > 1)
	{
		return 0;
	}
	else if(%obj.killteam[%tteam])
	{
		return 1;
	}
	else if(%obj.killteam[%tteam] $= "0")
	{
		return 0;
	}
	return %obj.aggressive;
}
function BotCanSee(%obj, %target)
{
	%opos = %obj.getEyePoint();
	%th = %target.getEyePoint();
	%tl = %target.getPosition();
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%opos, %th, %typemasks | %types, %obj);
	%hit = firstWord(%headRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else //Has to be a brick
		{
			if(!%hit.isRendering())
			{
				return true;
			}
			if(getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	%legRaycast = containerRaycast(%opos, %tl, %typemasks, %obj);
	%hit = firstWord(%legRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else
		{
			if(!%hit.isRendering())
			{
				return true;
			}
			if(getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	return false;
}

function OnBotDetectAlly(%bot, %ally)
{
	%brick = %bot.spawnBrick;
	if(!isObject(%brick))
	{
		return;
	}
	$InputTarget_["Self"] = %brick;
	$InputTarget_["Bot"] = %bot;
	$InputTarget_["Player"] = %ally;
	$InputTarget_["Client"] = %ally.client;
	%brick.processInputEvent("onBotDetectAlly", %brick.getGroup().client);
}
function OnBotDetectEnemy(%bot, %enemy)
{
	%brick = %bot.spawnBrick;
	if(!isObject(%brick))
	{
		return;
	}
	$InputTarget_["Self"] = %brick;
	$InputTarget_["Bot"] = %bot;
	$InputTarget_["Player"] = %enemy;
	$InputTarget_["Client"] = %enemy.client;
	%brick.processInputEvent("onBotDetectEnemy", %brick.getGroup().client);
}
function OnBotChaseTarget(%bot, %target)
{
	%brick = %bot.spawnBrick;
	if(!isObject(%brick))
	{
		return;
	}
	$InputTarget_["Self"] = %brick;
	$InputTarget_["Bot"] = %bot;
	$InputTarget_["Player"] = %target;
	$InputTarget_["Client"] = %target.client;
	%brick.processInputEvent("onBotChaseTarget", %brick.getGroup().client);
}
function OnBotLoseTarget(%bot, %target)
{
	%brick = %bot.spawnBrick;
	if(!isObject(%brick) || !isObject(%target))
	{
		return;
	}
	$InputTarget_["Self"] = %brick;
	$InputTarget_["Bot"] = %bot;
	$InputTarget_["Player"] = %target;
	$InputTarget_["Client"] = %target.client;
	%brick.processInputEvent("onBotLoseTarget", %brick.getGroup().client);
}

function VectorMult(%vecA, %vecB)
{
	%xa = getWord(%vecA, 0);
	%ya = getWord(%vecA, 1);
	%za = getWord(%vecA, 2);
	%xb = getWord(%vecB, 0);
	%yb = getWord(%vecB, 1);
	%zb = getWord(%vecB, 2);
	return %xa*%xb SPC %ya*%yb SPC %za*%zb;
}