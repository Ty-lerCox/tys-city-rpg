//CityRPGAdminLot.cs
datablock AudioProfile(CityRPGAdminLotDrawSound)
{
   filename    = "./CityRPGAdminLotDraw.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(CityRPGAdminLotHitSound)
{
   filename    = "./CityRPGAdminLotHit.wav";
   description = AudioClosest3d;
   preload = true;
};


//effects
datablock ParticleData(CityRPGAdminLotExplosionParticle)
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

datablock ParticleEmitterData(CityRPGAdminLotExplosionEmitter)
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
   particles = "CityRPGAdminLotExplosionParticle";

   uiName = "CityRPGAdminLot Hit";
};

datablock ExplosionData(CityRPGAdminLotExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 500;

   soundProfile = CityRPGAdminLotHitSound;

   particleEmitter = CityRPGAdminLotExplosionEmitter;
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
//AddDamageType("CityRPGAdminLot",   '<bitmap:add-ons/Gamemode_TysCityRPG/shapes/CI_CityRPGAdminLot> %1',    '%2 <bitmap:add-ons/Weapon_CityRPGAdminLot/CI_CityRPGAdminLot> %1',0.75,1);
datablock ProjectileData(CityRPGAdminLotProjectile)
{
   directDamage        = 35;
   directDamageType  = $DamageType::CityRPGAdminLot;
   radiusDamageType  = $DamageType::CityRPGAdminLot;
   explosion           = CityRPGAdminLotExplosion;
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

   uiName = "CityRPGAdminLot Slice";
};


//////////
// item //
//////////
datablock ItemData(CityRPGAdminLotItem)
{
    shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/baton.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "AdminLot";
    iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/baton";
    doColorShift = false;
    colorShiftColor = "0.471 0.471 0.471 1.000";

     // Dynamic properties defined by the scripts
    image = CityRPGAdminLotImage;

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
datablock ShapeBaseImageData(CityRPGAdminLotImage)
{
	raycastWeaponRange = 6;
	raycastWeaponTargets = $TypeMasks::All;
	raycastDirectDamage = 25;
	raycastDirectDamageType = $DamageType::CityRPGAdminLot;
	raycastExplosionProjectile = CityRPGAdminLotProjectile;
	raycastExplosionSound = CityRPGAdminLotHitSound;
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
   item = CityRPGAdminLotItem;
   ammo = " ";
   projectile = CityRPGAdminLotProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = false;
   colorShiftColor = "0.471 0.471 0.471 1.000";
	
	item = CityRPGAdminLotItem;
	projectile = CityRPGAdminLotProjectile;
	colorShiftColor = "0.5 0.5 0.5 1";
	showBricks = 0;
	
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = CityRPGAdminLotDrawSound;

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

function CityRPGAdminLotImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "armAttack");
}

function CityRPGAdminLotImage::onStopFire(%this, %obj, %slot)
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

function CityRPGAdminLotImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
{
	%newBrickID.transferLot(%client.brickGroup);
	%newBrickID.setName("");
}

function CityRPGAdminLotItem::onPickup(%this, %item, %obj)
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