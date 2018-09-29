// ============================================================
// Project				:	CityRPG
// Author				:	Moppy
// Description			:	Ore Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGOreData)
{
	brickFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Bricks/4x Cube.blb";
	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/4x Cube";
	
	category = "CityRPG";
	subCategory = "CityRPG Resources";
	
	uiName = "Ore";
	
	isRock = true;
	hasOre = true;
	resources = 15;
	
	CityRPGBrickType = 4;
	CityRPGBrickAdmin = true;
};

// ============================================================
// Section 2 : Events
// ============================================================
function fxDTSBrick::adjustColorOnOreContent(%this)
{
	if(isObject(%this))
	{
		if(%this.getDatablock().isRock && %this.getDatablock().CityRPGBrickType == 4)
		{
			%this.hasOre = !%this.hasOre;
			%this.setColor(0);
			
			if(!%this.hasOre)
			{
				%this.schedule(getRandom(60000, 120000), "adjustColorOnOreContent");
			}
		}
	}
}

// ============================================================
// Section 3 : Package Data
// ============================================================
package CityRPG_OrePackage
{
	function fxDTSBrick::onPlant(%brick,%client)
	{	
		parent::onPlant(%brick);
		
		if(%brick.getDatablock().hasOre && %brick.getDatablock().CityRPGBrickType == 4)
		{
			%brick.hasOre = true;
			%brick.setColor(0);
		}
	}
	function fxDTSBrick::setColor(%this,%color)
	{
		if(%this.getDataBlock().isRock)
		{
			if(!%this.hasOre)
			{
				parent::setColor(%this, $CityRPG::pref::resources::ore::noOreColorID);
			}
			else
			{
				parent::setColor(%this, $CityRPG::pref::resources::ore::hasOreColorID);
			}
		}
		else if(!%this.getDataBlock().isDrug)
		{
			parent::setColor(%this, %color);
		}
		else if(%this.getDataBlock().isDrug && %this.getID().canchange)
		{
			parent::setColor(%this, %this.getID().currentColor);
		}
	}
	
	function fxDTSBrick::setColorFX(%this, %FX)
	{
		if(!%this.getDataBlock().isDrug)
		{
			parent::setColorFX(%this, %FX);
		}
	}
	
	function fxDTSBrick::setShapeFX(%this, %FX)
	{
		if(!%this.getDataBlock().isDrug)
		{
			parent::setShapeFX(%this, %FX);
		}
	}
	
	function fxDTSBrick::setEmitter(%this, %Emitter)
	{
		if(!%this.getDataBlock().isDrug)
		{
			parent::setEmitter(%this, %Emitter);
		}
		else if(%this.getDataBlock().isDrug && %this.getID().cansetemitter == true)
		{
			parent::setEmitter(%this, %Emitter);
		}
	}
};
activatePackage(CityRPG_OrePackage);