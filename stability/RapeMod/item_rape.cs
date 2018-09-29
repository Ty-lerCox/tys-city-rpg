//effects
datablock ParticleData(rapeExplosionParticle)
{
   dragCoefficient      = 2;
   gravityCoefficient   = 1.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   spinRandomMin = -90;
   spinRandomMax = 90;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 300;
   colors[0]     = "0 0 0 0";
   colors[1]     = "0 0 0 0";
   sizes[0]      = 0.5;
   sizes[1]      = 0.25;
};

datablock ParticleEmitterData(rapeExplosionEmitter)
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
   particles = "rapeExplosionParticle";
};

datablock ExplosionData(rapeExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 500;

   particleEmitter = rapeExplosionEmitter;
   particleDensity = 0;
   particleRadius = 0.0;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = false;
   camShakeFreq = "0 0 0";
   camShakeAmp = "0 0 0";
   camShakeDuration = 0;
   camShakeRadius = 0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 0;
   lightStartColor = "0.0 0 0";
   lightEndColor = "0 0 0";
};


//projectile
datablock ProjectileData(rapeProjectile)
{
   directDamage        = 0;
   explosion           = rapeExplosion;

   brickExplosionRadius = 0;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

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
   lightRadius = 1;
   lightColor  = "0 0 0.5";

   uiName = "";
};


//////////
// item //
//////////
datablock ItemData(rapeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "base/data/shapes/empty.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Rape";
	doColorShift = False;
	colorShiftColor = "0 0 0 1.000";

	 // Dynamic properties defined by the scripts
	image = rapeImage;
	canDrop = false;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(rapeImage)
{
   // Basic Item properties
   shapeFile = "base/data/shapes/empty.dts";
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

   eyeOffset = "0 0 0";

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = rapeItem;
   ammo = "";
   projectile = rapeProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = false;

   //casing = " ";
   doColorShift = false;
   colorShiftColor = "0 0 0 1.000";

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
	stateSound[0]					= weaponSwitchSound;

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

function rapeImage::onPreFire(%this, %obj, %slot)
{
	return;
}

function rapeImage::onMount(%this, %obj)
{
	%obj.playThread(2, armreadyboth);
}

function rapeImage::onStopFire(%this, %obj, %slot)
{	
	return;
}

function rapeProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.client.minigame == %obj.client.minigame && isObject(%col.client.minigame))
	{
		RapePlayer(%obj.client, %col.client);
	}
	else
	{
		return;
	}
}