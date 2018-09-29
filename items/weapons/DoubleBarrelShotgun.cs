//audio
datablock AudioProfile(shotgunOpenSound)
{
   filename    = "./shapes/BrickIcons/open.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(shotguncloseSound)
{
   filename    = "./shapes/BrickIcons/close.wav";
   description = AudioClose3d;
   preload = true;
};

//shell
datablock DebrisData(doublebarrelshotgunShellDebris)
{
	shapeFile = "./shapes/BrickIcons/dbshotgunShell.dts";
	lifetime = 2.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 2;
};

//emitter
datablock ParticleData(shotgunSmokeParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 525;
	lifetimeVarianceMS   = 55;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.5 0.5 0.5 0.5";
	colors[1]     = "0.5 0.5 0.5 0.0";
	sizes[0]      = 0.15;
	sizes[1]      = 0.1;

	useInvAlpha = false;
};
datablock ParticleEmitterData(shotgunSmokeEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "shotgunSmokeParticle";
};

AddDamageType("dbShotgun",   '<bitmap:add-ons/Weapon_DoubleBarrelShotgun/CI_dbshotgun> %1',    '%2 <bitmap:add-ons/Weapon_DoubleBarrelShotgun/CI_dbshotgun> %1',0.5,1);
datablock ProjectileData(doublebarrelshotgunProjectile)
{
   projectileShapeName = "add-ons/Weapon_Gun/bullet.dts";
   directDamage        = 15;
   directDamageType    = $DamageType::dbShotgun;
   radiusDamageType    = $DamageType::dbShotgun;

   brickExplosionRadius = 0.2;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 15;
   brickExplosionMaxVolume = 20;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 30;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 250;
   verticalImpulse     = 200;
   explosion           = gunExplosion;

   muzzleVelocity      = 150;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};

//////////
// item //
//////////
datablock ItemData(doublebarrelshotgunItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/BrickIcons/doublebarrelshotgun.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "DB Shotgun";
	iconName = "./shapes/BrickIcons/Icon_DBShotgun";
	doColorShift = true;
	colorShiftColor = "0.47 0.2588 0.0157 1.000";

	 // Dynamic properties defined by the scripts
	image = doublebarrelshotgunImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(doublebarrelshotgunImage)
{
   // Basic Item properties
   shapeFile = "./shapes/BrickIcons/doublebarrelshotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = doublebarrelshotgunItem;
   ammo = " ";
   projectile = doublebarrelshotgunProjectile;
   projectileType = Projectile;

   casing = doublebarrelshotgunShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 1000;

   doColorShift = true;
   colorShiftColor = doublebarrelshotgunItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateTransitionOnTimeout[0]     = "Reload";
	stateSound[0]			  = weaponSwitchSound;

	stateName[1]                    = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]        = true;
	stateTimeoutValue[1]		  = 0.01;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Smoke";
	stateTimeoutValue[2]            = 0.14;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]		  = true;
	stateEmitter[2]			  = gunFlashEmitter;
	stateEmitterTime[2]		  = 0.05;
	stateEmitterNode[2]		  = "muzzleNode";
	stateSound[2]			  = gunShot1Sound;

	stateName[3] 			  = "Smoke";
	stateEmitter[3]			  = shotgunSmokeEmitter;
	stateEmitterTime[3]		  = 0.1;
	stateEmitterNode[3]		  = "muzzleNode";
	stateTimeoutValue[3]            = 0.5;
	stateWaitForTimeout[2]		  = true;
	stateTransitionOnTriggerUp[3]     = "Reload";

	stateName[4]			  = "Reload";
	stateTimeoutValue[4]		  = 0.5;
	stateSequence[4]			  = "open";
	stateTransitionOnTimeout[4]	  = "shell";
	stateWaitForTimeout[4]		  = true;
	stateSound[4]			  = shotgunOpenSound;
	
	stateName[5]				= "shell";
	stateTimeoutValue[5]		  = 0.1;
	stateSequence[5]			= "eject";
	stateEjectShell[5]       	  = true;
	stateTransitionOnTimeout[5]	  = "shell2";
	stateWaitForTimeout[5]		  = true;
	
	stateName[6]				= "shell2";
	stateTimeoutValue[6]		  = 0.1;
	stateEjectShell[6]       	  = true;
	stateTransitionOnTriggerDown[6] = "Close";
	
	stateName[7]				= "Close";
	stateSequence[7]			= "close";
	stateTimeoutValue[7]		= 0.5;
	stateTransitionOnTimeout[7]	= "Wait";
	stateWaitForTimeout[7]		= true;
	stateSound[7]			  = shotgunCloseSound;
	
	stateName[8]			  = "Wait";
	stateTimeoutValue[8]		  = 0.1;
	stateTransitionOnTimeout[8]	  = "Ready";
	
};

function doublebarrelshotgunImage::onFire(%this,%obj,%slot)
{
	if((%obj.lastFireTime+%this.minShotTime) > getSimTime())
		return;
	%obj.lastFireTime = getSimTime();
      
	%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%obj.client.player.getEyeVector(),"-3")));
	%obj.playThread(2, shiftAway);

	%projectile = %this.projectile;
	%spread = 0.0075;
	%shellcount = 12;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
