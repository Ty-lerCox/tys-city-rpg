/////////////////////////
//Event_Botss/outputs.cs//
/////////////////////////
//By Amade (ID 10716)

$nodelist = "armor bicorn bucket cape chest cophat epaulets epauletsranka epauletsrankb epauletsrankc epauletsrankd femchest flarehelmet headskin helmet knithat larm larmslim lhand lhook lpeg lshoe lski pack pants plume pointyhelmet quiver rarm rarmslim rhand rhook rpeg rshoe rski scouthat septplume shoulderpads skirthip skirttrimleft skirttrimright tank triplume visor";

function isnode(%node)
{
	%words = getWordCount($nodelist);
	for(%i = 0; %i < %words; %i++)
	{
		if(getWord($nodelist, %i) $= %node)
		{
			return true;
		}
	}
	return false;
}

//Event Registry
if(!$BotEventsLoaded)
{
	//registerOutputEvent("Classname", "EventName", arguments, bool appendClient);
	registerOutputEvent("FxDTSbrick", "BotJump");
	registerOutputEvent("FxDTSbrick", "BotFireWeapon", "int 0 30000");
	registerOutputEvent("FxDTSbrick", "BotEmote", "list Alarm 0 Confusion 1 Hate 2 Love 3 PainLow 4 PainMid 5 PainHigh 6 Win 7");
	registerOutputEvent("FxDTSbrick", "BotPlayAnimation", "list NONE 0 Activate 1 Attack 2 ChargeSpear 3 FlailArms 4 Jump 5 PlantBrick 6 PlayDead 7 RightArmUp 8 LeftArmUp 9 BothArmsUp 10 ShakeHead 11 Sit 12 SpinWeapon 13 Talk 14 ThrowSpear 15" TAB "int 0 4 3");
	registerOutputEvent("FxDTSbrick", "EditBotAppearance", "list setNodeColor 0 hideNode 1 unhideNode 2 setDecal 3 setFace 4" TAB "string 70 70" TAB "paintcolor 0" TAB "string 30 30", true);
	registerOutputEvent("FxDTSbrick", "MoveBotToBrick", "string 50 70", true);
	registerOutputEvent("FxDTSbrick", "SetBotAim", "list NONE 0 NearestPlayer 1 LookAtNamedBrick 2 Player 3 Vector 4" TAB "string 50 70", true);
	registerOutputEvent("FxDTSbrick", "SetBotMovement", "list Stop 0 GoToBrick(Run,Name) 1 FollowPlayer(Run,Duration) 2 Wander(Run,Radius) 3 ReturnToSpawn(Run) 4 GoToPlayerAim(Run) 5 Vector(Run,Direction) 6" TAB "bool" TAB "string 50 70", true);
	registerOutputEvent("FxDTSbrick", "SetBotName", "string 50 70", true);
	registerOutputEvent("FxDTSbrick", "SetBotTeamName", "string 50 70", true);
	registerOutputEvent("FxDTSbrick", "SetBotIsCrouching", "bool");
	registerOutputEvent("Player", "BotJump");
	registerOutputEvent("Player", "BotFireWeapon", "int 0 30000");
	registerOutputEvent("Player", "DoEmote", "list Alarm 0 Confusion 1 Hate 2 Love 3 PainLow 4 PainMid 5 PainHigh 6 Win 7");
	registerOutputEvent("Player", "EditAppearance", "list setNodeColor 0 hideNode 1 unhideNode 2 setDecal 3 setFace 4" TAB "string 70 70" TAB "paintcolor 0" TAB "string 30 30", true);
	registerOutputEvent("Player", "MoveToBrick", "string 50 70", true);
	registerOutputEvent("Player", "PlayAnimation", "list NONE 0 Activate 1 Attack 2 ChargeSpear 3 FlailArms 4 Jump 5 PlantBrick 6 PlayDead 7 RightArmUp 8 LeftArmUp 9 BothArmsUp 10 ShakeHead 11 Sit 12 SpinWeapon 13 Talk 14 ThrowSpear 15" TAB "int 0 4 3");
	registerOutputEvent("Player", "SetBotName", "string 50 70", true);
	registerOutputEvent("Player", "SetBotAim", "list NONE 0 NearestPlayer 1 LookAtNamedBrick 2 Player 3 Vector 4" TAB "string 50 70", true);
	registerOutputEvent("Player", "SetBotIsCrouching", "bool");
	registerOutputEvent("Player", "SetBotMovement", "list Stop 0 GoToBrick(Run,Name) 1 FollowPlayer(Duration) 2 Wander(Run,Radius) 3 ReturnToSpawn(Run) 4 GoToPlayerAim(Run) 5 Vector(Run,Direction) 6" TAB "bool" TAB "string 50 70", true);
	registerOutputEvent("Player", "SetTeamName", "string 50 70", true);
}

function FxDTSbrick::setBotIsCrouching(%this, %bool)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassname() $= "AIplayer")
		{
			%obj.setBotIsCrouching(%bool);
		}
	}
}
function Player::setBotIsCrouching(%this, %bool)
{
	if(isObject(%this) && %this.getClassName() $= "AIplayer")
	{
		%this.mountImage(BlankImage, 3); //This likes to unmount
		%this.setImageTrigger(3, %bool);
	}
}

function FxDTSbrick::setBotTeamName(%this, %name, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%name = filterVariableString(%name, %client.processingBrick, %client, %client.player);
	}
	%this.vehicle.teamName = %name;
	if(isObject(%bot = %this.vehicle) && %bot.getClassName() $= "AIplayer")
	{
		%owner = %this.getGroup().client;
		if(isObject(%mini = %owner.minigame) && (%mini.owner == %owner || %mini.useAllPlayersBricks))
		{
			for(%i = 0; %mini.teamExists[%i]; %i++)
			{
				if(%mini.teamName[%i] $= %name)
				{
					%bot.client.tdmTeam = %i;
					break;
				}
			}
		}
	}
}
function Player::SetTeamName(%this, %name, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%name = filterVariableString(%name, %client.processingBrick, %client, %client.player);
	}
	%this.teamName = %name;
	if(%this.getClassName() $= "AIplayer")
	{
		%owner = %this.spawnBrick.getGroup().client;
		if(isObject(%mini = %owner.minigame) && (%mini.owner == %owner || %mini.useAllPlayersBricks))
		{
			for(%i = 0; %mini.teamExists[%i]; %i++)
			{
				if(%mini.teamName[%i] $= %name)
				{
					%this.client.tdmTeam = %i;
					break;
				}
			}
		}
	}
}

function FxDTSbrick::setBotName(%this, %name, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%name = filterVariableString(%name, %brick, %client, %client.player);
	}
	%this.vehicle.client.name = %name;
}
function Player::setBotName(%this, %name, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%name = filterVariableString(%name, %brick, %client, %client.player);
	}
	%this.client.name = %name; //Will not interfere with player names as the variable on an actual client is protected
}

function FxDTSbrick::MoveBotToBrick(%this, %brickname, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%brickname = filterVariableString(%name, %brickname, %client, %client.player);
	}
	if(isObject(%obj = %this.vehicle))
	{
		if(isObject(%brickname = %this.getGroup().NTobject_[%brickname, 0]))
		{
			%offset = "0 0 " @ %brickname.getDatablock().brickSizeZ * 0.1;
			%position = vectorAdd(%brickname.getPosition(), %offset);
			%rotation = getWords(%obj.getTransform(), 3, 6);
			%transform = %position SPC %rotation;
			%obj.setTransform(%transform);
		}
	}
}

function Player::MoveToBrick(%this, %brickname, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%brickname = filterVariableString(%brickname, %brick, %client, %client.player);
	}
	%blid = isObject(%mini = %this.client.minigame) ? %mini.owner.bl_id : %this.client.bl_id;
	%bricknameGroup = "BrickGroup_" @ %blid;
	if(isObject(%brickname = %bricknameGroup.NTobject_[%brickname, 0]))
	{
		%offset = "0 0 " @ %brickname.getDatablock().brickSizeZ * 0.1;
		%position = vectorAdd(%brickname.getPosition(), %offset);
		%rotation = getWords(%this.getTransform(), 3, 6);
		%transform = %position SPC %rotation;
		%this.setTransform(%transform);
	}
}

function Player::PlayAnimation(%this, %anim, %slot)
{
	switch(%anim)
	{
		case 0: %this.playThread(%slot, "root");
		case 1: %this.playThread(%slot, "activate");
		case 2: %this.playThread(%slot, "armattack");
		case 3: %this.playThread(%slot, "spearcharge");
		case 4: %this.playThread(%slot, "activate2");
		case 5: %this.playThread(%slot, "jump");
		case 6: %this.playThread(%slot, "plant");
		case 7: %this.playThread(%slot, "death1");
		case 8: %this.playThread(%slot, "armreadyright");
		case 9: %this.playThread(%slot, "armreadyleft");
		case 10: %this.playThread(%slot, "armreadyboth");
		case 11: %this.playThread(%slot, "undo");
		case 12: %this.playThread(%slot, "sit");
		case 13: %this.playThread(%slot, "rotCW");
		case 14: %this.playThread(%slot, "talk");
		case 15: %this.playThread(%slot, "spearthrow");
	}
}

function FxDTSbrick::BotPlayAnimation(%this, %anim, %slot)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.playAnimation(%anim, %slot);
		}
	}
}

function Player::doEmote(%this, %num)
{
	switch(%num)
	{
		case 0: if($AddOnLoaded__Emote_Alarm)
		{
			%this.emote(alarmprojectile);
		}
		case 1: if($AddOnLoaded__Emote_Confusion)
		{
			%this.emote(wtfimage);
		}
		case 2: if($AddOnLoaded__Emote_Hate)
		{
			%this.emote(hateimage);
		}
		case 3: if($AddOnLoaded__Emote_Love)
		{
			%this.emote(loveimage);
		}
		case 4: %this.emote(painlowimage);
		case 5: %this.emote(painmidimage);
		case 6: %this.emote(painhighimage);
		case 7: %this.emote(winstarprojectile); serverPlay3D(rewardSound, %this.getHackPosition());
	}
}

function FxDTSbrick::BotEmote(%this, %num)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.doEmote(%num);
		}
	}
}

function Player::BotJump(%this)
{
	if(%this.getClassName() $= "AIplayer")
	{
		%this.setImageTrigger(2, 1);
		%this.schedule(33, setImageTrigger, 2, 0);
	}
}

function FxDTSbrick::BotJump(%this)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.BotJump();
		}
	}
}

function FxDTSbrick::editBotAppearance(%this, %action, %node, %color, %str, %client)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.editAppearance(%action, %node, %color, %str, %client);
		}
	}
}

function Player::editAppearance(%this, %action, %node, %color, %str, %client)
{
	if(isFunction("FilterVariableString"))
	{
		%brick = %client.processingBrick;
		%node = filterVariableString(%node, %brick, %client, %client.player);
		%str = filterVariableString(%str, %brick, %client, %client.player);
	}
	%color = getWordCount(%str) == 4 ? %str : getColorIDTable(%color);
	if(isObject(%this))
	{
		if(%this.getDatablock().shapeFile !$= "base/data/shapes/player/m.dts")
		{
			%node = "all";
		}
		%this.appearanceModified = true;
		%node = strlwr(%node);
		%node = strReplace(%node, "foot", "shoe");
		%node = strReplace(%node, "feet", "LShoe RShoe LPeg RPeg");
		%node = strReplace(%node, "legs", "LShoe RShoe LPeg RPeg");
		%node = strReplace(%node, "hands", "LHand RHand LHook RHook");
		%node = strReplace(%node, "arms", "LArm Rarm LArmSlim RArmSlim");
		%node = strReplace(%node, "head", "headskin");
		%node = strReplace(%node, "headskinskin", "headskin");
		if(%action < 3)
		{
			if(%node $= "all")
			{
				switch(%action)
				{
					case 0: %this.setNodeColor("ALL", %color);
					case 1: %this.hideNode("ALL");
					case 2: %this.unhideNode("ALL"); %this.setNodeColor("ALL", %color); %this.setHeadUp(true);
				}
				return;
			}
			for(%i = 0; %i < getWordCount(%node); %i++)
			{
				if(!isnode(getWord(%node, %i)))
				{
					return;
				}
			}
		}
		if(%action == 1 || %action == 2)
		{
			%bool = %action == 2;
			for(%i = 0; %i < getWordCount(%node); %i++)
			{
				switch$(getWord(%node, %i))
				{
					case "armor": %this.setHeadUp(%bool); break;
					case "bucket": %this.setHeadUp(%bool); break;
					case "cape": %this.setHeadUp(%bool); break;
					case "pack": %this.setHeadUp(%bool); break;
					case "quiver": %this.setHeadUp(%bool); break;
					case "tank": %this.setHeadUp(%bool); break;
				}
			}
		}
		for(%i = 0; %i < getWordCount(%node); %i++)
		{
			%n = getWord(%node, %i);
			switch(%action)
			{
				case 0: %this.setNodeColor(%n, %color);
				case 1: %this.hidenode(%n);
				case 2: %this.unhidenode(%n); %this.setNodeColor(%n, %color);
				case 3: %this.setDecalName(%n);
				case 4: %this.setFaceName(%n);
			}
		}
	}
}

function Player::setBotAim(%this, %targetType, %namedBrick)
{
	if(isObject(%this) && %this.getClassName() $= "AIplayer")
	{
		cancel(%this.playerLookLoop);
		%this.setAimObject(0);
		%this.clearAim();
		if(%targetType == 0)
		{
			return;
		}
		if(%targetType == 1)
		{
			%this.lookAtPlayers();
		}
		if(%targetType == 2 && isObject(%this.spawnBrick))
		{
			%group = %this.spawnBrick.getGroup();
			if(isObject(%brick = %group.NTobject_[%namedBrick, 0]))
			{
				%this.setAimLocation(%brick.getPosition());
			}
		}
		if(%targetType == 3)
		{
			%this.setAimObject($InputTarget_["Player"]);
		}
		if(%targetType == 4)
		{
			%pos = %this.getEyePoint();
			%vec = vectorAdd(%pos, vectorScale(%namedBrick, 1));
			%this.setAimLocation(%vec);
		}
	}
}

function FxDTSbrick::setBotAim(%this, %targetType, %namedBrick)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.setBotAim(%targetType, %namedBrick);
		}
	}
}

function AIPlayer::lookAtPlayers(%this)
{
	if(isObject(%this))
	{
		%dist = 1000;
		for(%i = 0; %i < clientGroup.getCount(); %i++)
		{
			if(isObject(%player = clientGroup.getObject(%i).player))
			{
				if((%distB = vectorDist(%this.getPosition(), %player.getPosition())) < %dist)
				{
					%closestplayer = %player;
					%dist = %distB;
				}
			}
		}
		if(isObject(%closestplayer))
		{
			%this.setAimObject(%closestplayer);
		}
		%this.playerLookLoop = %this.schedule(5000, lookAtPlayers);
	}
}

function Player::SetBotMovement(%this, %event, %bool, %str, %client)
{
	if(%this.getClassName() $= "AIPlayer")
	{
		%this.clearMoveY();
		%this.clearMoveX();
		%this.setMoveObject(0); //Clear existing movement
		%this.stop();
		cancel(%this.wanderLoop);
		cancel(%this.driveLoop);
		if(%event == 0)
		{
			return;
		}
		if(%this.isDriving())
		{
			%this.setMoveSpeed(1);
			if(%event == 1 && isObject(%this.spawnBrick))
			{
				%group = %this.spawnBrick.getGroup();
				if(isObject(%brick = %group.NTobject_[%str]))
				{
					%this.setDriveDestination(%brick.getPosition());
				}
			}
			if(%event == 2)
			{
				return;//Following players in a vehicle... Meh
			}
			if(%event == 3)
			{
				%this.wander(%bool, %str);
			}
			if(%event == 4)
			{
				%this.setDriveDestination(%this.spawnPosition);
			}
		}
		else
		{
			if(%event == 1)
			{
				if(isObject(%brick = ("_" @ %str)))
				{
					if(%brick.getClassName() $= "FxDTSbrick")
					{
						switch(%bool)
						{
							case 1: %this.setMoveSpeed(1);
							default: %this.setMoveSpeed(0.7);
						}
						%this.setMoveDestination(%brick.getPosition(), false);
					}
				}
			}
			if(%event == 2)
			{
				if(%bool)
				{
					%this.setMoveSpeed(1);
				}
				else
				{
					%this.setMoveSpeed(0.7);
				}
				if(isObject(%player = $InputTarget_["Player"]))
				{
					%this.setMoveObject(%player);
					if(%str > 0)
					{
						%this.schedule(%str, setMoveObject, 0);
					}
				}
			}
			if(%event == 3)
			{
				%this.wander(%bool, %str);
			}
			if(%event == 4)
			{
				if(%bool)
				{
					%this.setMoveSpeed(1);
				}
				else
				{
					%this.setMoveSpeed(0.7);
				}
				%this.setMoveDestination(%this.spawnPosition, false);
				jumpToZ(%this, getWord(%this.spawnPosition, 2));
			}
			if(%event == 5)
			{
				if(isObject(%obj = $InputTarget_["Player"]))
				{
					%start = %obj.getEyePoint();
					%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 200));
					%ray = containerRaycast(%start, %end, $Typemasks::FXbrickAlwaysObjectType | $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType, %obj);
					%pos = posFromRaycast(%ray);
					%this.setMoveDestination(%pos, false);
					jumpToZ(%this, getWord(%pos, 2));
				}
			}
			if(%event == 6)
			{
				//%this, %event, %bool, %str, %client
				switch(%bool)
				{
					case 1: %this.setMoveSpeed(1);
					default: %this.setMoveSpeed(0.7);
				}
				%pos = %this.getEyePoint();
				%vec = vectorAdd(%pos, vectorScale(%str, 1));
				%this.setAimLocation(%vec);
				%this.setMoveY(1);
			}
		}
	}
}
function FxDTSbrick::SetBotMovement(%this, %event, %bool, %str)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.setBotMovement(%event, %bool, %str);
		}
	}
}

function Player::BotFireWeapon(%this, %int)
{
	if(%this.getClassName() $= "AIplayer")
	{
		%this.setImageTrigger(0, 1);
		%this.schedule(%int, setImageTrigger, 0, 0);
	}
}
function FxDTSbrick::BotFireWeapon(%this, %int)
{
	if(isObject(%obj = %this.vehicle))
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%obj.BotFireWeapon(%int);
		}
	}
}

function player::isDriving(%this)
{
	%res = 0;
	if(isObject(%obj = %this.getObjectMount()))
	{
		if(%this.isPilot())
		{
			%res = 1;
		}
		if(strPos(%obj.getClassName(), "Player") != -1)
		{
			if(%obj.getMountedObject(0) == %this)
			{
				%res = 1;
			}
		}
	}
	return %res;
}

//This doesn't work well just in case you needed a driving AI
function AIplayer::SetDriveDestination(%this, %pos)
{
	if(isObject(%this))
	{
		%this.clearMoveX();
		%this.clearMoveY();
		if(vectorDist(%tpos, %pos) > 1 && %this.isDriving())
		{
			%tpos = setWord(%this.getPosition(), 2, "0");
			%pos = setWord(%pos, 2, "0");
			%vec = %this.getForwardVector();
			%dvec = vectorNormalize(vectorSub(%pos, %tpos));
			%dir = getWord(vectorSub(%dvec, %vec), 1);
			%this.setMoveY(1);
			switch(%dir > 0)
			{
				case false: %this.setMoveX(1); %dir = -%dir;
				case true: %this.setMoveX(-1);
			}
			%this.driveLoop = %this.schedule(%dir * 1000, setDriveDestination, %pos);
		}
	}
}

function Player::wander(%this, %run, %radius)
{
	if(isObject(%this))
	{
		if(%this.getClassName() $= "AIplayer")
		{
			if(%radius < 1)
			{
				%radius = 25;
			}
			if(%run)
			{
				%this.setMoveSpeed(1);
			}
			else
			{
				%this.setMoveSpeed(0.7);
			}
			%tpos = %this.getPosition();
			%ranV = getRandom(-25, 25) SPC getRandom(-25, 25) SPC 0;
			%dest = vectorAdd(%tpos, %ranV);
			while(isObject(firstWord(containerRaycast(%tpos, %dest, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleBlockerObjectType))))
			{
				%ranV = getRandom(-25, 25) SPC getRandom(-25, 25) SPC 0;
				%dest = vectorAdd(%tpos, %ranV);
				if(%i++ > 10) //So the server doesn't crash if it's encased in something
				{
					break;
				}
			}
			if(vectorDist(%dest, %this.spawnPosition) > %radius)
			{
				%dest = %this.spawnPosition;
				jumpToZ(%this, getWord(2, %this.spawnPosition));
			}
			if(%this.isDriving())
			{
				%this.setDriveDestination(%dest);
			}
			else
			{
				%this.setMoveDestination(%dest, false);
			}
			%this.wanderloop = %this.schedule(5000, wander, %run, %radius);
		}
	}
}

function jumpToZ(%obj, %z)
{
	if(isObject(%obj) && strLen(getWord(%dest = %obj.spawnPosition, 2)) > 0)
	{
		if(%obj.getClassName() $= "AIplayer")
		{
			%pos = %obj.getPosition();
			%obj.setMoveX(0);
			if(getRandom() > 0.5)
			{
				%obj.setMoveX(getRandom(-1,1));
			}
			if(getWord(%pos, 2) < %z)
			{
				%obj.botjump();
				%obj.jumpsched = %obj.schedule(1000, jumpToZ, %z);
			}
		}
	}
}