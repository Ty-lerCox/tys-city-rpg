// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Shovel Module Code File
// ============================================================
// Table of Contents
// 1. Shovel
// 1.1. Shovel Datablocks
// 1.2. Visual Functionality
// ============================================================

// ============================================================
// Section 1 : Datablocks
// ============================================================
if(!isObject(CityRPGShovelItem))
{
	AddDamageType("Shovel", '<bitmap:Add-Ons/Gamemode_TysCityRPG/shapes/ci/shovel> %1', '%2 <bitmap:Add-Ons/Gamemode_TysCityRPG/shapes/ci/shovel> %1', 0.5, 1);
	
	// Section 1.1 : Shovel Datablocks
	datablock ProjectileData(CityRPGShovelProjectile)
	{	
		directDamage		= 10;
		directDamageType	= $DamageType::Shovel;
		radiusDamageType	= $DamageType::Shovel;
		
		muzzleVelocity		= 50;
		velInheritFactor	= 1;
	
		armingDelay			= 0;
		lifetime			= 100;
		fadeDelay			= 70;
		bounceElasticity	= 0;
		bounceFriction		= 0;
		isBallistic			= false;
		gravityMod 			= 0.0;
	
		hasLight			= false;
		lightRadius			= 3.0;
		lightColor			= "0 0 0.5";
	};
	
	datablock ItemData(CityRPGShovelItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/Gamemode_TysCityRPG/shapes/shovel.2.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Shovel";
		iconName		= "Add-Ons/Gamemode_TysCityRPG/shapes/ItemIcons/shovel";
		doColorShift	= true;
		colorShiftColor = "0.6 0.6 0.6 1.000";
	
		image			= CityRPGShovelImage;
		
		canDrop = true;
		
		// CityRPGG Properties
		noSpawn = true;
	};
	
	datablock ShapeBaseImageData(CityRPGShovelImage)
	{
		// SpaceCasts
		raycastWeaponRange = 6;
		raycastWeaponTargets = $TypeMasks::All;
		raycastDirectDamage = 0;
		raycastDirectDamageType = $DamageType::Shovel;
		raycastExplosionProjectile = hammerProjectile;
		raycastExplosionSound = hammerHitSound;
		
		shapeFile		= "Add-Ons/Gamemode_TysCityRPG/shapes/shovel.2.dts";
		emap			= true;
		mountPoint		= 0;
		offset			= "0 0 0";
		correctMuzzleVector = false;
		eyeOffset		= "0.7 1.2 -1.45";
		eyeRotation		= "0 1 0 180";
		className		= "WeaponImage";
		rotation		= "0 1 0 180";
		
		item			= CityRPGShovelItem;
		ammo			= " ";
		projectile		= CityRPGShovelProjectile;
		projectileType	= Projectile;
		
		melee			= true;
		doRetraction	= false;
		armReady		= true;
		
		doColorShift	= true;
		colorShiftColor	= "0.6 0.6 0.6 1.000";
	
		stateName[0]					= "Activate";
		stateTimeoutValue[0]			= 0.5;
		stateTransitionOnTimeout[0]		= "Ready";
	
		stateName[1]					= "Ready";
		stateTransitionOnTriggerDown[1]	= "PreFire";
		stateAllowImageChange[1]		= true;
	
		stateName[2]					= "PreFire";
		stateScript[2]					= "onPreFire";
		stateAllowImageChange[2]		= false;
		stateTimeoutValue[2]			= 0.1;
		stateTransitionOnTimeout[2]		= "Fire";
	
		stateName[3]					= "Fire";
		stateTransitionOnTimeout[3]		= "CheckFire";
		stateTimeoutValue[3]			= 0.2;
		stateFire[3]					= true;
		stateAllowImageChange[3]		= false;
		stateSequence[3]				= "Fire";
		stateScript[3]					= "onFire";
		stateWaitForTimeout[3]			= true;
	
		stateName[4]					= "CheckFire";
		stateTransitionOnTriggerUp[4]	= "StopFire";
		stateTransitionOnTriggerDown[4]	= "Fire";
	
		stateName[5]					= "StopFire";
		stateTransitionOnTimeout[5]		= "Ready";
		stateTimeoutValue[5]			= 0.2;
		stateAllowImageChange[5]		= false;
		stateWaitForTimeout[5]			= true;
		stateSequence[5]				= "StopFire";
		stateScript[5]					= "onStopFire";
	};
}

// Section 1.2 : Visual Functionality
function CityRPGShovelImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function CityRPGShovelImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function CityRPGShovelImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%brickData = %col.getDatablock();
		if(%brickData.isDirt)
		{
			%col.onDig(%obj.client);
		}
	}
	
	parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
}