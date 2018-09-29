//Duplorcator replacement for duplicator
if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$Duplorcator::PrefsLoaded)
	{
		if(!$RTB::RTBR_ServerControl_Hook)
			exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
		RTB_registerPref("Admin Only", "Duplorcator", "Pref::Duplorcator::AdminOnly", "bool", "Tool_Duplorcator", 1, 0, 0);
		RTB_registerPref("Max Bricks (Admin)", "Duplorcator", "Pref::Duplorcator::MaxBricks", "int 0 50000", "Tool_Duplorcator", 5000, 0, 0);
		RTB_registerPref("Max Bricks (Non-Admin)", "Duplorcator", "Pref::Duplorcator::MaxBricksB", "int 0 50000", "Tool_Duplorcator", 500, 0, 0);
		RTB_registerPref("Selecting Timeout (Admin)", "Duplorcator", "Pref::Duplorcator::SelectionTimeout", "int 0 60", "Tool_Duplorcator", 1, 0, 0);
		RTB_registerPref("Selecting Timeout (Non-Admin)", "Duplorcator", "Pref::Duplorcator::SelectionTimeoutB", "int 0 60", "Tool_Duplorcator", 3, 0, 0);
		RTB_registerPref("Planting Timeout (Admin)", "Duplorcator", "Pref::Duplorcator::PlantTimeout", "int 0 60", "Tool_Duplorcator", 1, 0, 0);
		RTB_registerPref("Planting Timeout (Non-Admin)", "Duplorcator", "Pref::Duplorcator::PlantTimeoutB", "int 0 60", "Tool_Duplorcator", 3, 0, 0);
		RTB_registerPref("Rotate Events", "Duplorcator", "Pref::Duplorcator::RotateEvents", "bool", "Tool_Duplorcator", 1, 0, 0);
		RTB_registerPref("Trust Required", "Duplorcator", "Pref::Duplorcator::TrustLevel", "list None " @ $TrustLevel::None @ " Build " @ $TrustLevel::Build @ " Full " @ $TrustLevel::Full @ " You " @ $TrustLevel::You, "Tool_Duplorcator", 2, 0, 0);
		RTB_registerPref("Max Ghost Bricks", "Duplorcator", "Pref::Duplorcator::MaxGhostBricks", "int 0 2000", "Tool_Duplorcator", 500, 0, 0);
		RTB_registerPref("Quick Ghost Bricks", "Duplorcator", "Pref::Duplorcator::GhostBricksNoDelay", "int 0 2000", "Tool_Duplorcator", 150, 0, 0);
		RTB_registerPref("Load Admin Only", "Duplorcator", "Pref::Duplorcator::AdminLoad", "bool", "Tool_Duplorcator", 0, 0, 0);
		RTB_registerPref("Save Admin Only", "Duplorcator", "Pref::Duplorcator::AdminSave", "bool", "Tool_Duplorcator", 0, 0, 0);
		$Duplorcator::PrefsLoaded = true;
	}
}
else
{
	$Pref::Duplorcator::AdminOnly = true;
	$Pref::Duplorcator::MaxGhostBricks = 150;
	$Pref::Duplorcator::MaxBricks = 5000;
	$Pref::Duplorcator::MaxBricksB = 500;
	$Pref::Duplorcator::TrustLevel = 2; //Trust needed to plant/duplicate
	$Pref::Duplorcator::SelectionTimeout = 0;
	$Pref::Duplorcator::SelectionTimeoutB = 3;
	$Pref::Duplorcator::PlantTimeout = 0;
	$Pref::Duplorcator::PlantTimeoutB = 3;
	$Pref::Duplorcator::RotateEvents = true;
	$Pref::Duplorcator::GhostBricksNoDelay = 145;
	$Pref::Duplorcator::LoadAdmin = false;
	$Pref::Duplorcator::SaveAdmin = false;
}
//Credit to Space Guy for this
function getClosestPaintColor(%rgba)
{
	%prevdist = 100000;
	%colorMatch = 0;
	for(%i = 0;%i < 64;%i++)
	{
		%color = getColorIDTable(%i);
		if(vectorDist(%rgba,getWords(%color,0,2)) < %prevdist && getWord(%rgba,3) - getWord(%color,3) < 0.3 && getWord(%rgba,3) - getWord(%color,3) > -0.3)
		{
			%prevdist = vectorDist(%rgba,%color);
			%colormatch = %i;
		}
	}
	return %colormatch;
}
$Pref::Duplorcator::HighlightColor = getClosestPaintColor("0 1 1 1");
datablock itemData(DuplorcatorItem)
{
	cameraMaxDist   = 0.1;
	canDrop         = 1;
	category        = "Weapon";
	className       = "Tool";
	colorShiftColor = "0 1 1 1";
	density         = 0.2;
	doColorShift    = 1;
	elasticity      = 0.2;
	emap            = 1;
	friction        = 0.6;
	iconName        = "base/client/ui/itemIcons/wand";
	image           = "DuplorcatorImage";
	shapeFile       = "base/data/shapes/wand.dts";
	uiName          = "Duplorcator";
};
datablock particleData(DuplorcatorExplosionParticle)
{
	colors[0]          = "0.3 0.3 1 0.9";
	colors[1]          = "0 0.2 1 0.7";
	colors[2]          = "0.3 0.4 1 0.5";
	gravityCoefficient = 0;
	lifetimeMS         = 400;
	lifetimeVarianceMS = 200;
	sizes[0]           = 0.6;
	sizes[1]           = 0.4;
	sizes[2]           = 0.3;
	spinRandomMax      = 90;
	spinRandomMin      = -90;
	textureName        = "base/data/particles/ring";
	times[1]           = 0.8;
	times[2]           = 1;
};

datablock particleEmitterData(DuplorcatorExplosionEmitter)
{
	ejectionOffset   = 0.5;
	ejectionPeriodMS = 4;
	ejectionVelocity = 3;
	particles        = DuplorcatorExplosionParticle;
	periodVarianceMS = 2;
	thetaMax         = 180;
	velocityVariance = 0;
};
datablock explosionData(DuplorcatorExplosion)
{
	camShakeDuration = 0.5;
	camShakeFreq     = "1 1 1";
	emitter[0]       = DuplorcatorExplosionEmitter;
	faceViewer       = 1;
	lifetimeMS       = 180;
	lightEndRadius   = 0.5;
	lightStartColor  = "0 0.7 1 1";
	lightStartRadius = 1;
	shakeCamera      = 1;
	soundProfile     = "wandHitSound";
};
datablock projectileData(DuplorcatorProjectile)
{
	bounceElasticity = 0;
	bounceFriction   = 0;
	explodeOnDeath   = 1;
	explosion        = DuplorcatorExplosion;
	fadeDelay        = 2;
	gravityMod       = 0;
	lifetime         = 0;
	mask             = $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
	range            = 10;
};
datablock particleData(DuplorcatorParticleA)
{
	colors[0]          = "0.0 0.6 1 0.9";
	colors[1]          = "0 0.2 1 0.7";
	colors[2]          = "0.3 0.4 1 0.5";
	gravityCoefficient = -0.5;
	lifetimeMS         = 400;
	lifetimeVarianceMS = 200;
	sizes[0]           = 0.1;
	sizes[1]           = 0.4;
	sizes[2]           = 0.6;
	spinRandomMax      = 90;
	spinRandomMin      = -90;
	textureName        = "base/data/particles/ring";
	times[1]           = 0.8;
	times[2]           = 1;
};

datablock particleEmitterData(DuplorcatorEmitterA)
{
	ejectionOffset   = 0.09;
	ejectionPeriodMS = 50;
	ejectionVelocity = 0.2;
	particles        = DuplorcatorParticleA;
	periodVarianceMS = 2;
	thetaMax         = 180;
	velocityVariance = 0;
};
datablock particleData(DuplorcatorParticleB)
{
	colors[0]          = "0.0 0.6 1 0.9";
	colors[1]          = "0 0.2 1 0.7";
	colors[2]          = "0.3 0.4 1 0.5";
	gravityCoefficient = -0.4;
	dragCoefficient    = 2;
	lifetimeMS         = 400;
	lifetimeVarianceMS = 200;
	sizes[0]           = 0.4;
	sizes[1]           = 0.6;
	sizes[2]           = 0.9;
	spinRandomMax      = 0;
	spinRandomMin      = 0;
	textureName        = "base/client/ui/brickIcons/1x1";
	times[1]           = 0.5;
	times[2]           = 1;
};

datablock particleEmitterData(DuplorcatorEmitterB)
{
	ejectionOffset   = -0.0;
	ejectionPeriodMS = 10;
	ejectionVelocity = 0;
	particles        = DuplorcatorParticleB;
	periodVarianceMS = 2;
	thetaMin		 = 0.0;
	thetaMax         = 0.1;
	velocityVariance = 0;
	orientParticles  = true;
	phiVariance		 = 10;
};
datablock shapeBaseImageData(DuplorcatorImage : wandImage)
{
	projectile = DuplorcatorProjectile;
	showBricks = true;
	colorShiftColor = "0.000000 1.000000 1.000000 1.000000";
	item = DuplorcatorItem;
	stateEmitter[1] = DuplorcatorEmitterA;
	stateEmitter[3] = DuplorcatorEmitterB;
};

function servercmdduplorcator(%client)
{
	if(!isObject(%client.player))
		return;
	%client.player.updateArm(DuplorcatorImage);
	%client.player.mountImage(DuplorcatorImage,0);
}

function servercmddup(%client)
{
	servercmdDuplorcator(%client);
}

function vectorRot(%vector,%rotation)
{
	if(!%rotation)
		return %vector;
	if(%rotation == 1)
		return getWord(%vector, 1) SPC -getWord(%vector, 0) SPC getWord(%vector, 2);
	if(%rotation == 2)
		return -getWord(%vector, 0) SPC -getWord(%vector, 1) SPC getWord(%vector, 2);
	else if(%rotation == 3)
		return -getWord(%vector, 1) SPC getWord(%vector, 0) SPC getWord(%vector, 2);
}
function FxDTSBrick::restoreOrignalColors(%this)
{
	%this.setColor(%this.originalColor);
	%this.setColorFX(%this.originalColorFX);
}
function FxDtsBrick::getStack(%brick,%player)
{
	if(getTrustLevel(%brick,%player) < $Pref::Duplorcator::TrustLevel)
		return false;
	%stack = new SimSet();
	%stack.add(%brick);
	%stack.brick0 = %brick.saveToVar();
	if(isEventPending(%brick.highlightColorReset))
		cancel(%brick.highlightColorReset);
	else
	{
		%color = %brick.getColorID();
		%colorFX = %brick.getColorFxID();
		%brick.setColor($Pref::Duplorcator::HighlightColor);
		%brick.setColorFX(3);
		%brick.originalColor = %color;
		%brick.originalColorFX = %colorFX;
	}
	%brick.highlightColorReset = %brick.schedule(3000,restoreOrignalColors);
	%final = 1;
	for(%current = 0; %current != %final; %current++)
	{
		if((((%player.client.isAdmin) || (%player.client.canBuild)) && $Pref::Duplorcator::MaxBricks <= %stack.getCount()) || (((!%player.client.isAdmin) && (!%player.client.canBuild)) && $Pref::Duplorcator::MaxBricksB <= %stack.getCount()))
		{
			%stack.err = "Limit";
			return %stack;
		}
		%brick = %stack.getObject(%current);
		if(%current) //Don't select bricks below the first
		{
			for(%i=0;%i<%brick.getNumDownBricks();%i++)
			{
				%appendBrick = %brick.getDownBrick(%i);
				if(%stack.isMember(%appendBrick) || getTrustLevel(%appendBrick,%player) < $Pref::Duplorcator::TrustLevel)
					continue;
				if(isEventPending(%appendBrick.highlightColorReset))
					cancel(%appendBrick.highlightColorReset);
				else
				{
					%color = %appendBrick.getColorID();
					%colorFX = %appendBrick.getColorFxID();
					%appendBrick.setColor($Pref::Duplorcator::HighlightColor);
					%appendBrick.setColorFX(3);
					%appendBrick.originalColor = %color;
					%appendBrick.originalColorFX = %colorFX;
				}
				%appendBrick.highlightColorReset = %appendBrick.schedule(3000,restoreOrignalColors);
				%stack.add(%appendBrick);
				%stack.brick[%stack.getCount()-1] = %appendBrick.saveToVar();
				%final++;
			}
		}
		for(%i=0;%i<%brick.getNumUpBricks();%i++)
		{
			%appendBrick = %brick.getUpBrick(%i);
			if(%stack.isMember(%appendBrick) || getTrustLevel(%appendBrick,%player) < $Pref::Duplorcator::TrustLevel)
				continue;
			if(isEventPending(%appendBrick.highlightColorReset))
				cancel(%appendBrick.highlightColorReset);
			else
			{
				%color = %appendBrick.getColorID();
				%colorFX = %appendBrick.getColorFxID();
				%appendBrick.setColor($Pref::Duplorcator::HighlightColor);
				%appendBrick.setColorFX(3);
				%appendBrick.originalColor = %color;
				%appendBrick.originalColorFX = %colorFX;
			}
			%appendBrick.highlightColorReset = %appendBrick.schedule(3000,restoreOrignalColors);
			%stack.add(%appendBrick);
			%stack.brick[%stack.getCount()-1] = %appendBrick.saveToVar();
			%final++;
		}
	}
	return %stack;
}
function fxDtsBrick::getTooFarDist(%brick, %player)
{
	%playerPos = %player.getHackPosition();
	%worldBox = %brick.getWorldBox();
	for(%i = 0; %i < 3; %i++)
	{
		%player = getWord(%playerPos, %i);
		%min = getWord(%worldBox, %i);
		%max = getWord(%worldBox, %i + 3);
		if(%player > %max)
		{
			%total += mPow(%player - %max, 2);
		}
		else if(%player < %min)
		{
			%total += mPow(%min - %player, 2);
		}
	}
	return mSqrt(%total);
}
function FxDtsBrick::spawnDuplication(%this,%index)
{
	if(%index $= "")
	{
		if(isObject(%this.spawnedSet))
			%this.spawnedSet.deleteAll();
		%index = 0;
	}
	if(%index >= %this.stack.getCount() || %index > $Pref::Duplorcator::MaxGhostBricks)
		return;
	if(!isObject(%this.spawnedSet))
		%this.spawnedSet = new SimSet();

	%set = %this.spawnedSet;
	%rep = %this.stack.getObject(%index);
	%brick = new FxDTSBrick()
	{
		datablock = %rep.getDatablock();
		colorid = %rep.originalColor;
		position = %rep.getPosition();
		angleID = %rep.getAngleID();
		printID = %rep.getPrintID();
		rotation = %rep.rotation;
	};
	%set.add(%brick);
	%index++;
	%this.schedule(0,spawnDuplication,%index);
	%perc = mCeil(%set.getCount() / %this.stack.getCount() * 100);
	%ghost = "";
	if(%perc < 100)
		%ghost = " | Ghosting: \c4" @ %perc @ "%";
	%b = (%this.stack.getCount() > 1  ? "bricks":"brick");
	bottomPrint(%this.selector,"\c4Duplication <color:99AAAA>Mode<br>\c4" @ %this.stack.getCount() SPC "<color:99AAAA>" @ %b @ " selected" @ %ghost);
}
function FxDtsBrick::moveDuplication(%this,%val)
{
	if(!isObject(%this.spawnedSet) || !%this.duplicationBrick)
		return;
	if(%val $= "" && %this.spawnedSet.getCount() >= $Pref::Duplorcator::GhostBricksNoDelay)
	{
		cancel(%this.moveDelay);
		%this.moveDelay = %this.schedule(200,moveDuplication,true);
		return;
	}
	%startPosition = getValueFromBrickStr(%this.stack.brick0,"position");
	%angleOffset = %this.getAngleID() - getValueFromBrickStr(%this.stack.brick0,"angleID");
	if(%angleOffset < 0)
		%angleOffset += 4;
	%offset = vectorSub(%startPosition,%this.getPosition());

	for(%i=0;%i<%this.spawnedSet.getCount();%i++)
	{
		%data = %this.stack.brick[%i];
		%pos = getValueFromBrickStr(%data,"position");
		%angleID = getValueFromBrickStr(%data,"angleID");
		%brick = %this.spawnedSet.getObject(%i);
		%angle = %angleID + %angleOffset;
		if(%angle > 3)
			%angle -= 4;
		//Do rotation based on angleID
		switch(%angle)
		{
			case 0:
				%rotation = "1 0 0 0";
			case 1:
				%rotation = "0 0 1 90";
			case 2:
				%rotation = "0 0 1 180";
			default:
				%rotation = "0 0 -1 90";
		}
		//Initial position
		%pos = vectorSub(%pos,%startPosition);
		%position = vectorAdd(%this.getPosition(),vectorRot(%pos,%angleOffset));
		%brick.setTransform(%position);
		%brick.rotation = %rotation;
		%brick.angleID = %angle;
	}
}
//Not as cool as Randy's lol
function FXDtsBrick::saveDuplication(%this,%name)
{
	%client = %this.selector;
	if(!%this.duplicationBrick)
		return false;
	%fo = new FileObject();
	%fo.openForWrite("config/Duplorcations/" @ %name @ ".txt");
	%index = 0;
	while((%brick = %this.stack.brick[%index]) !$= "")
	{
		%fo.writeLine(%brick);
		%index++;
	}
	%fo.close();
	%fo.delete();
	commandtoclient(%client,'centerprint',"<color:99AAAA>Duplication successfully saved as '\c4" @ %name @ "<color:99AAAA>'",3);
	return true;
}
function FXDTSBrick::loadDuplication(%this,%name)
{
	%client = %this.selector;

	if(!isFile("config/Duplorcations/" @ %name @ ".txt"))
	{
		commandtoclient(%client,'centerprint',"<color:99AAAA>Duplication '\c4" @ %name @ "<color:99AAAA>' does not exist",3);
		return false;
	}
	%client.hasDup = true;
	%fo = new FileObject();
	%fo.openForRead("config/Duplorcations/" @ %name @ ".txt");
	%index = 1;
	%stack = new SimSet();
	%stack.brick0 = %fo.readLine();
	%pos = %this.getPosition();
	%client.player.tempBrick = eval("return new FxDTSBrick(){" @ %stack.brick0 @ "position=\"" @ %pos @ "\";};");
	%client.player.tempBrick.duplicationBrick = true;
	%client.player.tempBrick.stack = %stack;
	%client.player.tempBrick.selector = %client;
	while(!%fo.isEOF())
	{
		%stack.brick[%index] = %fo.readLine();
		%index++;
	}
	%stack.numBricks = %index;
	%fo.close();
	%fo.delete();
	commandtoclient(%client,'centerprint',"<color:99AAAA>Duplication '\c4" @ %name @ "<color:99AAAA>'successfully loaded",3);
	schedule(1,0,bottomPrint,%client,"\c4Duplication <color:99AAAA>Mode<br>\c4" @ %stack.numBricks SPC "<color:99AAAA>bricks" @ " selected");
	%this.schedule(0,delete);
}
function servercmdLoadDup(%client,%name)
{
	if(!isObject(%client.player) || %name $= "")
		return;
	if($Pref::Duplorcator::AdminOnly)
	{
		commandToClient(%client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to use the \c4Duplorcator",3);
		return;
	}
	if($Pref::Duplorcator::AdminLoad)
	{
		commandToClient(%client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to load duplications",3);
		return;
	}
	if(!isObject(%client.player.tempBrick))
	{
		%brick = new FXDtsBrick()
		{
			datablock = "brick1x1fData";
			position = %client.player.getPosition();
			forced = true;
		};
		%client.player.tempBrick = %brick;
	}
	%client.player.tempBrick.selector = %client;
	if(!%client.player.tempBrick.loadDuplication(%name) && %client.player.tempBrick.forced)
		%client.player.tempBrick.delete();

}
function servercmdSaveDup(%client,%name)
{
	if(!%client.hasDup || %name $= "" || !%client.player.tempBrick.duplicationBrick)
		return;
	if($Pref::Duplorcator::AdminOnly)
	{
		commandToClient(%client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to use the \c4Duplorcator",3);
		return;
	}
	$Pref::Duplorcator::AdminLoad = true;
	if($Pref::Duplorcator::AdminLoad)
	{
		commandToClient(%client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to save duplications",3);
		return;
	}
	%client.player.tempBrick.saveDuplication(%name);
}
function FxDtsBrick::savetoVar(%this)
{
	%blid = getSubStr(%this.getGroup().getName(),stripos(%this.getGroup().getName(),"_")+1,strLen(%this.getGroup().getName()));
	%cnt = -1;
	%data = %data @ "\tbrickname = \"" @ getSubStr(%this.getName(),1,strLen(%this.getname())) @ "\";";
	%data = %data @ "\trayCasting = \"" @ %this.isRayCasting() @ "\";";
	%data = %data @ "\tcolliding = \"" @ %this.isColliding() @ "\";";
	%data = %data @ "\trendering = \"" @ %this.isRendering() @ "\";";
	%data = %data @ "\tposition = \"" @ %this.getPosition() @ "\";";
	%data = %data @ "\trotation = \"" @ %this.rotation @ "\";";
	%data = %data @ "\tdatablock = \"" @ %this.getDatablock().getName() @ "\";";
	%data = %data @ "\tangleID = \"" @ %this.getAngleID() @ "\";";
	%data = %data @ "\tcolorID = \"" @ (%this.originalColor !$= "" ? %this.originalColor:%this.getColorID()) @ "\";";
	%data = %data @ "\tshapeFXID = \"" @ %this.shapeFXID @ "\";";
	%data = %data @ "\tprintID = \"" @ %this.getPrintID() @ "\";";
	%data = %data @ "\tcolorFXID = \"" @ (%this.originalColorFX !$= "" ? %this.originalColorFX:%this.getColorFXID()) @ "\";";
	%data = %data @ "\tisBaseplate = \"" @ %this.isBaseplate() @ "\";";
	%data = %data @ "\tisWater = \"" @ %this.physicalZone.isWater @ "\";";
	%data = %data @ "\tOwnerBLID = \"" @ %blid @ "\";";
	if(isObject(%this.vehicle))
		%data = %data @ "\tvehicledata = \"" @ %this.vehicle.getDatablock() @ "\";";
	%data = %data @ "\trecolorVehicle = \"" @ %this.reColorVehicle @ "\";";
	if(isObject(%this.light))
		%data = %data @ "\tlightdata = \"" @ %this.light.getDatablock().getName() @ "\";";
	if(isObject(%this.emitter))
		%data = %data @ "\temitterdata = \"" @ %this.emitter.emitter @ "\";";
	%data = %data @ "\temitterDirection = \"" @ %this.emitterDirection @ "\";";
	if(isObject(%this.item))
		%data = %data @ "\titemdata = \"" @ %this.item.getDatablock() @ "\";";
	if(isObject(%this.audioEmitter))
		%data = %data @ "\taudioEmitterData = \"" @ %this.audioEmitter.profile @ "\";";
	%data = %data @ "\titemRespawnTime = \"" @ %this.itemRespawnTime @ "\";";
	%data = %data @ "\titemPosition = \"" @ %this.itemPosition @ "\";";
	%data = %data @ "\titemDirection = \"" @ %this.itemDirection @ "\";";
	%data = %data @ "\tnumEvents = \"" @ %this.numEvents @ "\";";
	if(%this.numEvents)
	{
		for(%i=0;%i<%this.numEvents;%i++)
		{
			%data = %data @ "\teventEnabled" @ %i @ " = \"" @ %this.eventEnabled[%i] @ "\";";
			%data = %data @ "\teventDelay" @ %i @ " = \"" @ %this.eventDelay[%i] @ "\";";
			%data = %data @ "\teventInput" @ %i @ " = \"" @ %this.eventInput[%i] @ "\";";
			%data = %data @ "\teventInputIDX" @ %i @ " = \"" @ %this.eventInputIDX[%i] @ "\";";
			%data = %data @ "\teventTarget" @ %i @ " = \"" @ %this.eventTarget[%i] @ "\";";
			%data = %data @ "\teventTargetIDX" @ %i @ " = \"" @ %this.eventTargetIDX[%i] @ "\";";
			%data = %data @ "\teventNT" @ %i @ " = \"" @ %this.eventNT[%i] @ "\";";
			%data = %data @ "\teventOutput" @ %i @ " = \"" @ %this.eventOutput[%i] @ "\";";
			%data = %data @ "\teventOutputIDX" @ %i @ " = \"" @ %this.eventOutputIDX[%i] @ "\";";
			%data = %data @ "\teventOutputParameter" @ %i @ "_1 = \"" @ %this.eventOutputParameter[%i,1] @ "\";";
			%data = %data @ "\teventOutputParameter" @ %i @ "_2 = \"" @ %this.eventOutputParameter[%i,2] @ "\";";
			%data = %data @ "\teventOutputParameter" @ %i @ "_3 = \"" @ %this.eventOutputParameter[%i,3] @ "\";";
			%data = %data @ "\teventOutputParameter" @ %i @ "_4 = \"" @ %this.eventOutputParameter[%i,4] @ "\";";
			%data = %data @ "\teventOutputAppendClient" @ %i @ " = \"" @ %this.eventOutputAppendClient[%i] @ "\";";
			%data = %data @ "\teventOutputAppendClient" @ %i @ " = \"" @ %this.eventOutputAppendClient[%i] @ "\";";
			%data = %data @ "\teventOutputAppendClient" @ %i @ " = \"" @ %this.eventOutputAppendClient[%i] @ "\";";
			%data = %data @ "\teventOutputAppendClient" @ %i @ " = \"" @ %this.eventOutputAppendClient[%i] @ "\";";
		}
	}
	%i = -1;
	return %data;
}
function getValueFromBrickStr(%string,%value)
{
	%start = stripos(%string,%value) + strLen(%value)+4;
	if(%start == -1)
		return false;
	%line = getSubStr(%string,%start,strLen(%string));
	%line = getSubStr(%line,0,stripos(%line,"\""));
	return %line;
}
function DuplorcatorImage::onStopFire(%this,%player)
{
	%player.stopThread(2);
}
function DuplorcatorImage::onFire(%this,%player)
{
	%player.playThread(2,armAttack);

	%start = %player.getEyePoint();
	%end = vectorAdd(%player.getEyePoint(),VectorScale(%player.getEyeVector(),10));
	%masks = %this.projectile.mask;
	%ray = ContainerRayCast(%start,%end,%masks,%player);
	if(isObject(%ray))
	{
		%p = new Projectile()
		{
			datablock = %this.projectile;
			initialPosition = restWords(%ray);
		};
		%p.explode();
		if(%ray.getClassName() !$= "FxDtsBrick")
			return;
		if(((!%player.client.isAdmin) && (!%player.client.canBuild)) && $Pref::Duplorcator::AdminOnly)
		{
			commandToClient(%player.client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to use the \c4Duplorcator",3);
			return;
		}
		if(getTrustLevel(firstWord(%ray),%player) < $Pref::Duplorcator::TrustLevel)
		{
			commandtoclient(%player.client,'centerPrint',%ray.getGroup().name @ " does not trust you enough to do that.");
			return;
		}
		if((%player.client.isAdmin) || (%player.client.canBuild))
			%length = $Pref::Duplorcator::SelectionTimeout*1000;
		else
			%length = $Pref::Duplorcator::SelectionTimeoutB*1000;
		if(getSimTime() - %player.lastDupTime < %length)
		{
			messageClient(%player.client,'MsgPlantError_Flood');
			%timeout = getSimTime() - %player.lastDupTime;
			%timeout = %length - %timeout;
			%timeout /= 1000;
			%timeout = mCeil(%timeout);
			%timeout = %timeout @ " <color:99AAAA>" @ (%timeout > 1 ? "seconds":"second");
			commandtoClient(%player.client,'centerprint',"<color:99AAAA>You must wait \c4" @ %timeout @ "<color:99AAAA> before selecting again",3);
			return;
		}
		%player.lastDupTime = getSimTime();
		%stack = %ray.getStack(%player);
		%stack.numBricks = %stack.getCount();
		%color = %player.currSprayCan;
		if(isObject(%player.tempBrick))
		{
			%color = %player.tempBrick.getColorID();
			%player.tempBrick.delete();
		}
		%start = %stack.getObject(0);
		%brick = new FxDTSBrick()
		{
			datablock = %start.getDatablock();
			position = %start.getPosition();
			rotation = %start.rotation;
			printID = %start.getPrintID();
			angleID = %start.getAngleID();
			colorID = %start.originalColor;
			shapeFXID = %start.shapeFXID;

			duplicationBrick = true;
			stack = %stack;
			selector = %player.client;
			originalColor = %color;
		};
		%player.client.hasDup = true;
		%player.tempBrick = %brick;
		%b = (%stack.getCount() > 1  ? "bricks":"brick");
		bottomPrint(%player.client,"\c4Duplication <color:99AAAA>Mode<br>" @ %stack.getCount() SPC %b @ " selected");
		if($Pref::Duplorcator::MaxGhostBricks > 1 && %stack.getCount() > 1)
			%brick.spawnDuplication();
		if(%stack.err $= "Limit")
			messageClient(%player.client,'MsgPlantError_Limit');
	}
}
package Duplorcator
{
	function servercmdDuplicator(%client)
	{
		if(!isObject(DuplicatorImage))
			servercmdDuplorcator(%client);
		else
			Parent::servercmdDuplicator(%client);
	}
	function DuplorcatorImage::onMount(%this,%player)
	{
		Parent::onMount(%this,%player);
		if(isObject(%player.tempBrick) && %player.tempBrick.duplicationBrick)
			return;
		bottomPrint(%player.client,"\c4Normal <color:99AAAA>Mode<br>No bricks selected");
	}
	function DuplorcatorImage::onUnMount(%this,%player)
	{
		Parent::onUnMount(%this,%player);
		if(!%player.client.hasDup)
			bottomPrint(%player.client,"\c4Normal <color:99AAAA>Mode<br>No bricks selected",0.5);
	}
	function servercmdPlantBrick(%client)
	{
		%brick = %client.player.tempBrick;
		if(!%brick.duplicationBrick)
		{
			Parent::servercmdPlantBrick(%client);
			return;
		}
		if($Pref::Duplorcator::AdminOnly && ((!%player.client.isAdmin) && (!%player.client.canBuild)))
		{
			commandToClient(%client,'centerPrint',"<color:99AAAA>You must be an \c4Admin<color:99AAAA> to use the \c4Duplorcator",3);
			return;
		}
		//Check too far distance
		%dist = %brick.getTooFarDist(%client.player);
		if(%dist > $Pref::Server::ToofarDistance)
		{
			messageClient(%client,'MsgPlantError_TooFar');
			return;
		}
		//Check Plant timeout
		if((%player.client.isAdmin) || (%player.client.canBuild))
			%length = $Pref::Duplorcator::PlantTimeout*1000;
		else
			%length = $Pref::Duplorcator::PlantTimeoutB*1000;
		if(getSimTime() - %client.player.lastPlantTime < %length)
		{
			messageClient(%client,'MsgPlantError_Flood');
			%timeout = getSimTime() - %client.player.lastPlantTime;
			%timeout = %length - %timeout;
			%timeout /= 1000;
			%timeout = mCeil(%timeout);
			%timeout = %timeout @ " <color:99AAAA>" @ (%timeout > 1 ? "seconds":"second");
			commandtoClient(%client,'CenterPrint',"<color:99AAAA>You must wait \c4" @ %timeout @ "<color:99AAAA> before planting again",3);
			return;
		}
		%client.player.lastPlantTime = getSimTime();
		//Start the duplication!
		%stack = %brick.stack;
		%start = %stack.brick0;
		%offset = vectorSub(getValueFromBrickStr(%start,"position"),%brick.getPosition());
		%angleOffset = %brick.getAngleID() - getValueFromBrickStr(%start,"angleID");
		if(%angleOffset < 0)
			%angleOffset += 4;
		for(%i=0;%i<%stack.numBricks;%i++)
		{
			%brickText = %stack.brick[%i];
			//%curr = %stack.getObject(%i);

			%brickPos = getValueFromBrickStr(%brickText,"position");
			%angleID = getValueFromBrickStr(%brickText,"angleID");
			%emitterDirection = getValueFromBrickStr(%brickText,"emitterDirection");
			%itemDirection = getValueFromBrickStr(%brickText,"itemDirection");
			%itemPosition = getValueFromBrickStr(%brickText,"itemPosition");

			%angle = %angleID + %angleOffset;
			if(%angle > 3)
				%angle -= 4;
			//Emitter directions
			%emitterDirection = %emitterDirection - 2;
			if(%emitterDirection >= 0)
			{
				%emitterDirection = %emitterDirection + %angleOffset;
				if(%emitterDirection > 3)
					%emitterDirection -= 4;
			}
			%emitterDirection+=2;
			%itemDirection = %itemDirection-2;
			if(%itemDirection >= 0)
			{
				%itemDirection = %itemDirection + %angleOffset;
				if(%itemDirection > 3)
					%itemDirection -= 4;
			}
			%itemDirection+=2;
			if(%itemDirection == 0)
				%itemDirection = 2;
			%itemPosition = %itemPosition - 2;
			if(%itemPosition >= 0)
			{
				%itemPosition = %itemPosition + %angleOffset;
				if(%itemPosition > 3)
					%itemPosition -= 4;
			}
			%itemPosition+=2;
			//Do rotation based on angleID
			switch(%angle)
			{
				case 0:
					%rotation = "1 0 0 0";
				case 1:
					%rotation = "0 0 1 90";
				case 2:
					%rotation = "0 0 1 180";
				default:
					%rotation = "0 0 -1 90";
			}
			//Initial position
			%pos = vectorSub(%brickPos,getValueFromBrickStr(%start,"position"));
			%position = vectorAdd(%brick.getPosition(),vectorRot(%pos,%angleOffset));
			%dup = eval("return new FxDTSBrick(){" @ %brickText @ "position=\"" @ %position @ "\"" @ ";rotation=\"" @ %rotation @ "\"" @ ";angleID=\"" @ %angle @ "\"" @ ";emitterDirection=\"" @ %emitterDirection @ "\"" @ ";itemDirection=\"" @ %itemDirection @ "\"" @ ";itemPosition=\"" @ %itemPosition @ "\";};");
			%dup.setTrusted(1);
			%client.brickGroup.add(%dup);
			%dup.setNTObjectName(%dup.brickName);
			//New doors support, fuck you Badspot
			if(%dup.getDatablock().isDoor)
			{
				%skipEvents = %dup.getDatablock().skipDoorEvents;
				%dup.getDatablock().skipDoorEvents = true;
				%dup.noContentEvents = 1;
			}
			//Prevent overriding jvs events
			if($AddOn__JVS_Content && isFunction(%dup.getClassName(),"contentTypeID") && %dup.contentTypeID() > -1)
			{
				//Old doors support, modified to prevent console spam from new doors, fuck you again Badspot
				%dup.noContentEvents = 1;
			}
			%err = %dup.plant();
			//Was placed, now fix the datablock (skip preset events hack)
			if(%dup.getDatablock().isDoor)
				%dup.getDatablock().skipDoorEvents = %skipEvents;
			if(%err)
			{
				if(!isObject(%success) && %error $= "")
					%error = %err;
				%dup.delete();
				
				continue;
			}
			%dup.plantedTrustCheck(); //Correctly call onPlant and all those other functions, this is a key feature
			%lower = %dup.getDownBrick(0);
			if(isObject(%lower) && getTrustLevel(%lower,%dup) < $Pref::Duplorcator::TrustLevel)
			{
				%dup.delete();
				$Server::BrickCount--;
				if(!isObject(%success))
				{
					%error = 10;
					%noTrust = %lower.getGroup().name;
				}
				continue;
			}
			//Apply special attributes
			//Item
			if(isObject(%dup.itemData))
				%dup.setItem(%dup.itemData);
			if(isObject(%dup.lightData))
				%dup.setLight(%dup.lightData);
			if(isObject(%dup.emitterData)) //Schedule it because non-rendering bricks are not getting their emitters, odd.
				%dup.schedule(0,setEmitter,%dup.emitterData);
			if(isObject(%dup.audioEmitterData))
				%dup.setMusic(%dup.audioEmitterData);
			if(isObject(%dup.vehicleData))
			{
				%dup.setVehicle(%dup.vehicleData);
				%dup.respawnVehicle();
			}

			if(!%dup.colliding)
				%dup.setColliding(false);
			if(!%dup.raycasting)
				%dup.setRaycasting(false);
			if(!%dup.rendering)
				%dup.setRendering(false);
				
			if(!isObject(%success))
				%success = new SimSet();
			%success.add(%dup);

			//Rotate events
			if(%dup.numEvents && $Pref::Duplorcator::RotateEvents)
			{
				for(%i=0;%i<%dup.numEvents;%i++)
				{
					%target = %dup.eventTarget[%i];
					if(%target !$= "Self")
						continue;
					%output = %dup.eventOutput[%i];
					if(getSubStr(%output,0,strLen("fireRelay")) $= "fireRelay")
					{
						%dir = getSubStr(%output,strLen("fireRelay"),strLen(%output));
						switch$(%dir)
						{
							case "North":
								%dir = 0;
							case "West":
								%dir = 1;
							case "South":
								%dir = 2;
							case "East":
								%dir = 3;
							default:
								continue;
						}
						%dir -= %angleOffset;
						if(%dir < 0)
							%dir += 4;
						switch(%dir)
						{
							case 0:
								%output = "fireRelayNorth";
							case 1:
								%output = "fireRelayWest";
							case 2:
								%output = "fireRelaySouth";
							case 3:
								%output = "fireRelayEast";
						}
						%id = 23+%dir;
						%dup.eventOutput[%i] = %output;
						%dup.eventOutputIDX[%i] = %id;
					}
					if(getWordCount(%dup.eventOutputParameter[%i,1]) == 3)
						%dup.eventOutputParameter[%i,1] = vectorRot(%dup.eventOutputParameter[%i,1],%angleOffset);
					//Check if any directional values
					%id = %dup.eventOutputIDX[%i];
					%list = $OutputEvent_parameterListfxDTSBrick_[%id];
					for(%j=0;%j<getFieldCount(%list);%j++)
					{
						%param = getField(%list,%i);
						if(stripos(%param,"North") != -1 && stripos(%param,"South") != -1 && stripos(%param,"East") != -1 && stripos(%param,"West") != -1)
						{
							//It is a directional parameter!
							%off = 0;
							if(stripos(%param,"Up") != -1)
								%off++;
							if(stripos(%param,"Down") != -1)
								%off++;
							%dir = %dup.eventOutputParameter[%i,1]-%off;
							%dir -= %angleOffset;
							if(%dir < 0)
								%dir += 4;
							if(%dir == 1)
								%dir = 3;
							else if(%dir == 3)
								%dir = 1;
							%dup.eventOutputParameter[%i,1] = %dir+%off;
						}
					}
				}
			}
		}
		%count = 0;
		if(isObject(%success))
		{
			%count = %success.getCount();
			%brick.duplicationStack = %success;
			%client.undoStack.push(%success TAB "DUPLICATION");
			serverPlay3d(brickPlantSound,%brick.getPosition());
		}
		if(%count == 0)
		{
			switch(%error)
			{
				case 1:
					%err = 'MsgPlantError_Overlap';
				case 2:
					%err = 'MsgPlantError_Float';
				case 3:
					%err = 'MsgPlantError_Stuck';
				case 5:
					%err = 'MsgPlantError_Buried';
			}
			messageClient(%client,%err);
			if(%error == 10) //Trust
				commandtoclient(%client,'CenterPrint',%noTrust @ " does not trust you enough to do that.",3);
			else
				commandtoclient(%client,'centerprint',"\c4" @ %count @ "<color:99AAAA>/\c4" @ %stack.numBricks @ "<color:99AAAA> bricks duplicated successfully",3);
		}
		else
			commandtoclient(%client,'centerprint',"\c4" @ %count @ "<color:99AAAA>/\c4" @ %stack.numBricks @ "<color:99AAAA> bricks duplicated successfully",3);
	}
	function servercmdUndoBrick(%client)
	{
		%undo = %client.undostack.val[%client.undoStack.head-1];
		if(getField(%undo,1) $= "DUPLICATION")
		{
			%stack = getField(%undo,0);
			%brick = %stack.getObject(0);
			%client.undostack.val[%client.undoStack.head-1] = %brick TAB "PLANT";
			for(%i=0;%i<%stack.getCount();%i++)
				%stack.getObject(%i).killBrick();
			%stack.delete();
		}
		Parent::servercmdUndoBrick(%client);
	}
	function FxDtsBrick::onRemove(%brick,%this)
	{
		parent::onRemove(%brick,%this);
		if(!%brick.duplicationBrick)
			return;
		%client = %brick.selector;
		%client.hasDup = false;
		if(isObject(%client.player) && %client.player.getMountedImage(0) == DuplorcatorImage.getID())
			bottomPrint(%client,"\c4Normal <color:99AAAA>Mode<br>No bricks selected");
		else
			bottomPrint(%client,"\c4Normal <color:99AAAA>Mode<br>No bricks selected",3);
		if(isObject(%brick.spawnedSet))
		{
			%brick.spawnedSet.deleteAll();
			%brick.spawnedSet.delete();
		}
		%brick.stack.delete();
	}
	function FxDtsBrick::setDatablock(%this,%datablock)
	{
		if(%this.duplicationBrick && %datablock != %this.getDatablock())
		{
			%this.setColor(%this.originalColor);
			%this.duplicationBrick = false;
			%this.stack.delete();
			if(isObject(%this.spawnedSet))
			{
				%this.spawnedSet.deleteAll();
				%this.spawnedSet.delete();
			}
			%client = %this.selector;
			%client.hasDup = false;
			bottomPrint(%client,"\c4Normal <color:99AAAA>Mode<br>No bricks selected",3);
			%this.selector = 0;
		}
		Parent::setDatablock(%this,%datablock);
	}
	function FxDtsBrick::setTransform(%this,%transform)
	{
		parent::setTransform(%this,%transform);
		if(%this.duplicationBrick)
			%this.moveDuplication();
	}
};
if(isPackage("Duplorcator"))
	deactivatePackage("Duplorcator");
ActivatePackage("Duplorcator");