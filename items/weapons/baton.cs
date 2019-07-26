//CityRPGBaton.cs
datablock AudioProfile(CityRPGBatonDrawSound)
{
   filename    = "./CityRPGBatonDraw.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(CityRPGBatonHitSound)
{
   filename    = "./CityRPGBatonHit.wav";
   description = AudioClosest3d;
   preload = true;
};


//effects
datablock ParticleData(CityRPGBatonExplosionParticle)
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

datablock ParticleEmitterData(CityRPGBatonExplosionEmitter)
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
   particles = "CityRPGBatonExplosionParticle";

   uiName = "CityRPGBaton Hit";
};

datablock ExplosionData(CityRPGBatonExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 500;

   soundProfile = CityRPGBatonHitSound;

   particleEmitter = CityRPGBatonExplosionEmitter;
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
//AddDamageType("CityRPGBaton",   '<bitmap:add-ons/Gamemode_TysCityRPG/shapes/CI_CityRPGBaton> %1',    '%2 <bitmap:add-ons/Weapon_CityRPGBaton/CI_CityRPGBaton> %1',0.75,1);
datablock ProjectileData(CityRPGBatonProjectile)
{
   directDamage        = 35;
   directDamageType  = $DamageType::CityRPGBaton;
   radiusDamageType  = $DamageType::CityRPGBaton;
   explosion           = CityRPGBatonExplosion;
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

   uiName = "CityRPGBaton Slice";
};


//////////
// item //
//////////
datablock ItemData(CityRPGBatonItem)
{
    shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/baton.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "Baton";
    iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/baton";
    doColorShift = false;
    colorShiftColor = "0.471 0.471 0.471 1.000";

     // Dynamic properties defined by the scripts
    image = CityRPGBatonImage;

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
datablock ShapeBaseImageData(CityRPGBatonImage)
{
	raycastWeaponRange = 6;
	raycastWeaponTargets = $TypeMasks::All;
	raycastDirectDamage = 25;
	raycastDirectDamageType = $DamageType::CityRPGBaton;
	raycastExplosionProjectile = CityRPGBatonProjectile;
	raycastExplosionSound = CityRPGBatonHitSound;
	// SpaceCasts
	
	shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/baton.dts";
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
   item = CityRPGBatonItem;
   ammo = " ";
   projectile = CityRPGBatonProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = false;
   colorShiftColor = "0.471 0.471 0.471 1.000";
	
	item = CityRPGBatonItem;
	projectile = CityRPGBatonProjectile;
	colorShiftColor = "0.5 0.5 0.5 1";
	showBricks = 0;
	
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = CityRPGBatonDrawSound;

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

function CityRPGBatonImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "armAttack");
}

function CityRPGBatonImage::onStopFire(%this, %obj, %slot)
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

function raycastExplosionProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick)
{
	//echo("onCol");
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
			} else {
				if(!%client.warrant)
					messageClient(%client, '', "Brick or Lot");
			}
		}
		if(%client.warrant) 
		{
			KillingBricks(%client, %col);
			return;
		}
	}
	else if(%col.getClassName() $= "Player")
	{
		%client = %obj.client;
		if((%col.getType() & $typeMasks::playerObjectType) && isObject(%col.client))
		{
			if(%col.client.getWantedLevel())
			{
				if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
				{
					%col.setDamageLevel(%this.raycastDirectDamage + 1);
					%col.client.arrest(%client);
				}
				else
					commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6has resisted arrest!", 3);
			}
			else if(((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $CityRPG::pref::demerits::wantedLevel)
			{
				%col.client.getWantedLevel();
				if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
				{
					CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
					%col.setDamageLevel(%this.raycastDirectDamage + 1);
					%col.client.arrest(%client);
					CityRPGData.getData(%col.client.bl_id).valuemarijuana -= CityRPGData.getData(%col.client.bl_id).valuemarijuana;
					CityRPGData.getData(%col.client.bl_id).valuetotaldrugs -= CityRPGData.getData(%col.client.bl_id).valuetotaldrugs;
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is carrying drugs, and is resisting arrest!", 3);
					if(CityRPGData.getData(%col.client.bl_id).valueDemerits < $CityRPG::pref::demerits::wantedLevel)
					{
						CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
					}
				}
			}
			else if(CityRPGData.getData(%col.client.bl_id).valueBounty > 0)
				commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is not wanted alive.", 3);
			else
				%doNoEvil = true;
		}
	}
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
	//if(%doNoEvil) { %this.raycastDirectDamage = 10; }
	//parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	//if(%doNoEvil) { %this.raycastDirectDamage = 25; }
}

function CityRPGBatonImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
{
	//echo("hitobj");
	%didAction = false;
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%didAction = true;
		raycastExplosionProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
	}
	else if(%col.getClassName() $= "Player")
	{
		%didAction = true;
		%client = %obj.client;
		if((%col.getType() & $typeMasks::playerObjectType) && isObject(%col.client))
		{
			if(%col.client.getWantedLevel())
			{
				//if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
				//{
					%col.setDamageLevel(%this.raycastDirectDamage + 1);
					%col.client.arrest(%client);
				//}
				//else
					//commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6has resisted arrest!", 3);
			}
			else if(((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $CityRPG::pref::demerits::wantedLevel)
			{
				%col.client.getWantedLevel();
				if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
				{
					CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
					%col.setDamageLevel(%this.raycastDirectDamage + 1);
					%col.client.arrest(%client);
					CityRPGData.getData(%col.client.bl_id).valuemarijuana -= CityRPGData.getData(%col.client.bl_id).valuemarijuana;
					CityRPGData.getData(%col.client.bl_id).valuetotaldrugs -= CityRPGData.getData(%col.client.bl_id).valuetotaldrugs;
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is carrying drugs, and is resisting arrest!", 3);
					if(CityRPGData.getData(%col.client.bl_id).valueDemerits < $CityRPG::pref::demerits::wantedLevel)
					{
						CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
					}
				}
			}
			else if(CityRPGData.getData(%col.client.bl_id).valueBounty > 0)
				commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is not wanted alive.", 3);
			else
				%doNoEvil = true;
		}
	}
	//parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
	if(!%didAction)
		raycastExplosionProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
	if(%doNoEvil) { %this.raycastDirectDamage = 10; }
	parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	if(%doNoEvil) { %this.raycastDirectDamage = 25; }
}

function CityRPGBatonItem::onPickup(%this, %item, %obj)
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