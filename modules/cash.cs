// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Drop-Cash-On-Death Module Code File
// ============================================================
// Table of Contents
// 1. Package Data
// 1.1. Drop Money
// 1.2. Money Pickup
// 2. Money Datablock
// ============================================================

// ============================================================
// Section 1 : Package Data
// ============================================================
package CityRPG_Cash
{
	// Section 1.1 : Drop Money
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA)
	{
		if(!getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) && CityRPGData.getData(%client.bl_id).valueMoney && !%client.moneyOnSuicide)
		{
			messageClient(%client, '', "Client:" SPC %client.name);
			messageClient(%client, '', "Money:" SPC CityRPGData.getData(%client.bl_id).valueMoney);
			messageClient(%client, '', "Lumber:" SPC CityRPGData.getData(%client.bl_id).valueResources);
			messageClient(%client, '', "Gang:" SPC CityRPGData.getData(%client.bl_id).valueGangID);
			
			if($CityRPG::pref::misc::cashdrop == 1)
			{
				%cashval = mFloor(CityRPGData.getData(%client.bl_id).valueMoney);
				%cashcheck = 0;
				if(%cashval > 1000)
				{
					%cashval = 1000;
					%cashcheck = 1;
				}
				%cash = new Item()
				{
					datablock = cashItem;
					canPickup = false;
					value = %cashval;
				};
			
				%cash.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 2));
				%cash.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
				
				MissionCleanup.add(%cash);
				%cash.setShapeName("$" @ %cash.value);
				if(%cashcheck == 1)
					CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney - 1000;
				else
					CityRPGData.getData(%client.bl_id).valueMoney = 0;
				%client.SetInfo();
			}
			if($CityRPG::pref::misc::lumberdrop == 1)
			{
				%cashval = mFloor(getWord(CityRPGData.getData(%client.bl_id).valueResources, 0));
				messageClient(%client, '', "Lumber1: " @ getWord(CityRPGData.getData(%client.bl_id).valueResources, 0));
				messageClient(%client, '', "Lumber2: " @ %cashval);
				%cashcheck = 0;
				if(%cashval > 500)
				{
					%cashval = 1000;
					%cashcheck = 1;
				}
				%lumber = new Item()
				{
					datablock = lumberItem;
					canPickup = false;
					value = %cashval;
				};
			
				%lumber.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 2));
				%lumber.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
				
				MissionCleanup.add(%lumber);
				%lumber.setShapeName("\c7" @ %lumber.value SPC "Lumber");
				if(%cashcheck == 1)
					CityRPGData.getData(%client.bl_id).valueResources = CityRPGData.getData(%client.bl_id).valueResources - 500;
				else
					CityRPGData.getData(%client.bl_id).valueResources = 0;
				%client.SetInfo();
			}
			
		}
		parent::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA);
	}
	
	// Section 1.2 : Money Pickup
	function Armor::onCollision(%this, %obj, %col, %thing, %other)
	{
		if(%col.getDatablock().getName() $= "CashItem")
		{
			if(isObject(%obj.client))
			{
				if(isObject(%col))
				{
					if(%obj.client.minigame)
						%col.minigame = %obj.client.minigame;
					
					CityRPGData.getData(%obj.client.bl_id).valueMoney += %col.value;
					messageClient(%obj.client, '', "\c6You have picked up \c3$" @ %col.value SPC "\c6off the ground.");
					
					%obj.client.SetInfo();
					%col.canPickup = false;
					%col.delete();
				}
				else
				{
					%col.delete();
					MissionCleanup.remove(%col);
				}
			}
		}
		else if(%col.getDatablock().getName() $= "mariItem")
		{
			if(isObject(%obj.client))
			{
				if(isObject(%col))
				{
					if(%obj.client.minigame)
						%col.minigame = %obj.client.minigame;
						
					CityRPGData.getData(%obj.client.bl_id).valueMarijuana += %col.value;
					CityRPGData.getData(%obj.client.bl_id).valuetotaldrugs += %col.value;
					messageClient(%obj.client,'',"\c6You have picked up \c3" @ %col.value @ " \c3grams\c6 of marijuana.");
					
					%obj.client.SetInfo();
					%col.canPickup = false;
					%col.delete();
				}
				else
				{
					%col.delete();
					MissionCleanup.remove(%col);
				}
			}
		}
		
		if(isObject(%col))
			parent::onCollision(%this, %obj, %col, %thing, %other);
	}
	
	function CashItem::onAdd(%this, %item, %b, %c, %d, %e, %f, %g)
	{
		parent::onAdd(%this, %item, %b, %c, %d, %e, %f, %g);
		schedule($CityRPG::pref::moneyDieTime, 0, "eval", "if(isObject(" @ %item.getID() @ ")) { " @ %item.getID() @ ".delete(); }");
	}
	function LumberItem::onAdd(%this, %item, %b, %c, %d, %e, %f, %g)
	{
		parent::onAdd(%this, %item, %b, %c, %d, %e, %f, %g);
		schedule($CityRPG::pref::moneyDieTime, 0, "eval", "if(isObject(" @ %item.getID() @ ")) { " @ %item.getID() @ ".delete(); }");
	}
	function mariItem::onAdd(%this, %item, %b, %c, %d, %e, %f, %g)
	{
		parent::onAdd(%this, %item, %b, %c, %d, %e, %f, %g);
		schedule($CityRPG::pref::moneyDieTime, 0, "eval", "if(isObject(" @ %item.getID() @ ")) { " @ %item.getID() @ ".delete(); }");
	}
};
activatePackage(CityRPG_Cash);

// ============================================================
// Section 2 : Money Datablock
// ============================================================
datablock ItemData(cashItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "base/data/shapes/brickWeapon.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	doColorShift = true;
	colorShiftColor = "0 0.6 0 1";
	image = cashImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(cashImage)
{
	shapeFile = "base/data/shapes/brickWeapon.dts";
	emap = true;
	
	doColorShift = true;
	colorShiftColor = cashItem.colorShiftColor;
	canPickup = false;
};

datablock ItemData(lumberItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "base/data/shapes/brickWeapon.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	doColorShift = true;
	colorShiftColor = "0 0.6 0 1";
	image = cashImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(lumberImage)
{
	shapeFile = "base/data/shapes/brickWeapon.dts";
	emap = true;
	
	doColorShift = true;
	colorShiftColor = cashItem.colorShiftColor;
	canPickup = false;
};

datablock ItemData(mariItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/weedbag.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	doColorShift = true;
	image = mariImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(mariImage)
{
	shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/weedbag.dts";
	emap = true;
	
	doColorShift = true;
	canPickup = false;
};