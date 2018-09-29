//Tool_Rod.cs
datablock AudioProfile(RP_RodDrawSound)
{
   filename    = "./stability/FishingRod/1.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(RP_RodHitSound)
{
   filename    = "./stability/FishingRod/REEL_IN.wav";
   description = AudioClosest3d;
   preload = true;
};


//effects

datablock ExplosionData(RP_RodExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 500;

   soundProfile = RP_RodHitSound;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = false;
   camShakeFreq = "20.0 22.0 20.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;
};


//projectile
AddDamageType("RP_Rod",   '<bitmap:add-ons/Tool_RP_Fishing_Rod/CI_RP_Rod> %1',    '%2 <bitmap:add-ons/Tool_RP_Fishing_Rod/CI_RP_Rod> %1',0.75,1);
datablock ProjectileData(RP_RodProjectile)
{
   directDamage        = 0;
   directDamageType  = $DamageType::RP_Rod;
   radiusDamageType  = $DamageType::RP_Rod;
   explosion           = RP_RodExplosion;
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

   uiName = "RP_Rod Slice";
};


//////////
// item //
//////////
datablock ItemData(RP_Rod)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./stability/FishingRod/RP_Rod.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "RP FishingRod";
	iconName = "./stability/FishingRod/icon_RP_Rod";
	doColorShift = true;
	colorShiftColor = "0.471 0.471 0.471 1.000";

	 // Dynamic properties defined by the scripts
	image = RP_RodImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(RP_RodImage)
{
   // Basic Item properties
   shapeFile = "./stability/FishingRod/RP_Rod.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

   eyeOffset = "0.7 1.2 -0.25";

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = Rod;
   ammo = " ";
   projectile = RP_RodProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.471 0.471 0.471 1.000";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = RP_RodDrawSound;

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

function RP_RodImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function RP_RodProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		%col.onFish(%client);
}

function RP_RodImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function RP_RodProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick")
	{
		messageClient(%client, '', "Fishing bricks");
	}
}