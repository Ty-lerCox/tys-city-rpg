//CityRPGBrickBaton.cs
datablock AudioProfile(CityRPGBrickBatonDrawSound)
{
   filename    = "./CityRPGBatonDraw.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(CityRPGBrickBatonHitSound)
{
   filename    = "./CityRPGBatonHit.wav";
   description = AudioClosest3d;
   preload = true;
};


//effects
datablock ParticleData(CityRPGBrickBatonExplosionParticle)
{
   dragCoefficient      = 2;
   gravityCoefficient   = 1.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   spinRandomMin = -90;
   spinRandomMax = 90;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 300;
   textureName          = "base/data/particles/chunk";
   colors[0]     = "0.7 0.7 0.9 0.9";
   colors[1]     = "0.9 0.9 0.9 0.0";
   sizes[0]      = 0.5;
   sizes[1]      = 0.25;
};

datablock ParticleEmitterData(CityRPGBrickBatonExplosionEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 8;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "CityRPGBrickBatonExplosionParticle";

   uiName = "CityRPGBrickBaton Hit";
};

datablock ExplosionData(CityRPGBrickBatonExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 500;

   soundProfile = CityRPGBrickBatonHitSound;

   particleEmitter = CityRPGBrickBatonExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "20.0 22.0 20.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 3;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


//projectile
//AddDamageType("CityRPGBrickBaton",   '<bitmap:add-ons/Gamemode_TysCityRPG/shapes/CI_CityRPGBrickBaton> %1',    '%2 <bitmap:add-ons/Weapon_CityRPGBrickBaton/CI_CityRPGBrickBaton> %1',0.75,1);
datablock ProjectileData(CityRPGBrickBatonProjectile)
{
   directDamage        = 35;
   directDamageType  = $DamageType::CityRPGBrickBaton;
   radiusDamageType  = $DamageType::CityRPGBrickBaton;
   explosion           = CityRPGBrickBatonExplosion;
   //particleEmitter     = as;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "CityRPGBrickBaton Slice";
};


//////////
// item //
//////////
datablock ItemData(CityRPGBrickBatonItem)
{
    shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Baton.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "BrickBaton";
    iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/Baton";
    doColorShift = false;
    colorShiftColor = "0.471 0.471 0.471 1.000";

     // Dynamic properties defined by the scripts
    image = CityRPGBrickBatonImage;

    canDrop = false;
	
	noSpawn = true;
	canArrest = true;
	
	showBricks = 1;
	
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(CityRPGBrickBatonImage)
{
	// raycastWeaponRange = 6;
	// raycastWeaponTargets = $TypeMasks::All;
	// raycastDirectDamage = 25;
	// raycastDirectDamageType = $DamageType::CityRPGBrickBaton;
	// raycastExplosionProjectile = CityRPGBrickBatonProjectile;
	// raycastExplosionSound = CityRPGBrickBatonHitSound;
	// SpaceCasts
	
	shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Baton.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

	offset = "0 0.4 0";
	
	eyeOffset = "0 0 0";
	

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = CityRPGBrickBatonItem;
   ammo = " ";
   projectile = CityRPGBrickBatonProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = false;
   colorShiftColor = "0.471 0.471 0.471 1.000";
	
	item = CityRPGBrickBatonItem;
	projectile = CityRPGBrickBatonProjectile;
	colorShiftColor = "0.5 0.5 0.5 1";
	showBricks = 0;
	
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = CityRPGBrickBatonDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";
};

function CityRPGBrickBatonImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "armAttack");
}

function CityRPGBrickBatonImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function addEvid(%col,%client)
{
    if(%col.isPlanted()) 
    {
        %col.bagPlant(%col,%client);
        commandToClient(%client,'centerPrint',"\c6You can turn this in as \c3Evidence \c6at the Police Department.",3);
        CityRPGData.getData(%client.bl_id).valueevidence++;
    }
}


function CityRPGBrickBatonProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%client = %obj.client;
		%brickData = %col.getDatablock();
		
		if(%brickData.isDrug)
		{
			if(%col.isPlanted()) 
			{
				schedule(3000, 0, addEvid, %col, %client);
			}
			return;
		} 
		else if((%client.isAdmin) || (%client.bl_id == 997) || (%client.bl_id == 4896))
		{
			if(%client.transfer $= "Brick")
			{
				%before = %col.client.name;
				if(!(%client.setOwnership $= ""))
				{
					%brick = %col;
					%clientbg = getBrickGroupFromObject(%client.setOwnership);
					%clientbg.add(%brick);
					%brick.client = %client.setOwnership;
					%brick.stackBL_ID = %clientbg.bl_id;
					messageClient(%client, '', "\c6Brick:\c3" SPC %brick SPC "\c6from\c3" SPC %before SPC "\c6to\c3" SPC %client.setOwnership.name);
				} else {
					%brick = %col;
					//472428
					//11523
					//getBrickGroupFromObject(id(44793)).add(472428);
					//472428.client = id(44793);
					//472428.stackBL_ID = getBrickGroupFromObject(id(44793)).bl_id;
					%clientbg = getBrickGroupFromObject(%client);
					%clientbg.add(%brick);
					%brick.client = %client;
					%brick.stackBL_ID = %clientbg.bl_id;
					messageClient(%client, '', "\c6Brick:\c3" SPC %brick SPC "\c6from\c3" SPC %before SPC "\c6to\c3 you");
				}
				return;
			} else if(%client.transfer $= "Lot") {
				%target = %client.setOwnership;
				%newBrickID = %col;
				%newBrickID.transferLot(%target.brickGroup);
				%newBrickID.setName("");
				for(%a = 0; %a < %target.brickGroup.getCount(); %a++)
				{
					%foundBrick = %target.brickGroup.getObject(%a);
					%foundBrick.tran = NULL;
				}
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%target.player) ? %target.player : 0));
				return;
			} else if(%client.transfer $= "Quick") {
				if(%client.setBlock $= "")
				{
					%client.setBlock = %col.client;
					messageClient(%client,'',"\c6Setblock set to client:\c3" SPC %client.setBlock SPC "(" @ %client.setBlock.name @ ")");
					return;
				} else {
					%target = %client.setBlock;
					%newBrickID = %col;
					%newBrickID.transferLot(%target.brickGroup);
					%newBrickID.setName("");
					for(%a = 0; %a < %target.brickGroup.getCount(); %a++)
					{
						%foundBrick = %target.brickGroup.getObject(%a);
						%foundBrick.tran = NULL;
					}
					%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%target.player) ? %target.player : 0));
					%client.setBlock = "";
					return;
				}
			} else {
				if(!%client.warrant)
					messageClient(%client, '', "Brick or Lot or Quick");
			}
		}
		if(%client.warrant) 
		{
			KillingBricks(%client, %col);
			return;
		}
	}
	else
		return messageClient(%client,'',"Try using the player Baton");
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
	//if(%doNoEvil) { %this.raycastDirectDamage = 10; }
	//parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	//if(%doNoEvil) { %this.raycastDirectDamage = 25; }
}

function CityRPGBrickBatonItem::onPickup(%this, %item, %obj)
{
	%item.delete();
	
	for(%a = 0; %a < %obj.getDatablock().maxTools; %a++)
	{
		if(!isObject(%obj.tool[%a]) || %obj.tool[%a].getName() !$= "CityRPGLBItem")
			if(%freeSpot $= "" && %obj.tool[%a] $= "") { %freeSpot = %a; }
		else
			%alreadyOwns = true;
	}
	
	if(%freeSpot !$= "" && !%alreadyOwns)
	{
		%obj.tool[%freeSpot] = CityRPGLBItem.getID();
		messageClient(%obj.client, 'MsgItemPickup', "", %freeSpot, %obj.tool[%freeSpot]);
	}
}