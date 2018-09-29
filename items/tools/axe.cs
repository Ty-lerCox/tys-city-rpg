// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Lumberjack Axe Code File
// ============================================================
// Table of Contents
// 1. Weapon Data
// 1.1. Datablocks
// 1.2. Visual Functionality
// ============================================================

// ============================================================
// Section 1 : Weapon Data
// ============================================================

// Secton 2.1 : Datablocks
if(!isObject(CityRPGLumberjackItem))
{
	datablock projectileData(CityRPGLumberjackProjectile : swordProjectile)
	{
		directDamage		= 15;
		directDamageType	= $DamageType::Sword;
		radiusDamageType	= $DamageType::Sword;
		
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
	
	datablock itemData(CityRPGLumberjackItem : swordItem)
	{
		uiName = "Lumberjack Axe";
		image = CityRPGLumberjackImage;
		shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/lumberjack.1.dts";
		
		// CityRPG Properties
		noSpawn			= true;
	};
	
	datablock shapeBaseImageData(CityRPGLumberjackImage : swordImage)
	{
		// SpaceCasts
		raycastWeaponRange = 6;
		raycastWeaponTargets = $TypeMasks::All;
		raycastDirectDamage = 0;
		raycastDirectDamageType = $DamageType::Sword;
		raycastExplosionProjectile = swordProjectile;
		raycastExplosionSound = swordHitSound;
		
		item = CityRPgLumberjackItem;
		shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/lumberjack.1.dts";
		projectile = CityRPGLumberjackProjectile;
		armReady = true;
		melee = true;
	};
}

// Section 2.2 : Visual Functionality
function CityRPGLumberjackImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "armAttack");
}

function CityRPGLumberjackImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function CityRPGLumberjackImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%brickData = %col.getDatablock();
		if(%brickData.isTree)
		{
			%col.onChop(%obj.client);
		}
	}
	
	parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
}