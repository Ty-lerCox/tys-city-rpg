//---
//	@package VCE
//	@title Misc
//	@author Zack0wack0/www.zack0wack0.com
//	@time 9:20 PM 16/03/2011
//---
//Moddable special values
function VCE_Client_setFaceName(%client,%name)
{
	if(isObject(%client.player))
		%client.player.setFaceName(%name);
}
function VCE_Client_setDecalName(%client,%name)
{
	if(isObject(%client.player))
		%client.player.setDecalName(%name);
}
function VCE_Client_setClanPrefix(%client,%tag)
{
	%len = strLen(%tag);
	if(%len > 4)
		%tag = getSubStr(%tag,0,4);
	%client.clanPrefix = %tag;
}
function VCE_Client_setClanSuffix(%client,%tag)
{
	%len = strLen(%tag);
	if(%len > 4)
		%tag = getSubStr(%tag,0,4);
	%client.clanSuffix = %tag;
}
function VCE_Client_setScore(%client,%amount)
{
	%client.setScore(%amount);
}
function VCE_Player_setItem(%player,%name,%slot)
{	
	%item = $uiNameTable_Items[%name];
	if(%slot < 0 || %slot > %player.getDatablock().maxTools || !isObject(%item))
		return;
	%tool = %player.tool[%slot];
	%player.tool[%slot] = %item;
	messageClient(%player.client,'MsgItemPickup','',%slot,%item);
	if(%tool <= 0)
		%player.weaponCount++;
}
function VCE_Player_setCurrentItem(%player,%name)
{
	%item = $uiNameTable_Items[%name];
	if(!isObject(%item))
		return;
	%player.unMountImage(0);
	%player.mountImage(%item.image,0);
}
function VCE_Player_setHealth(%player,%amount)
{
	%player.setDamageLevel(%player.getDatablock().maxDamage - %amount);
}
function VCE_Player_setDamage(%player,%amount)
{
	%player.setDamageLevel(%amount);
}
function VCE_Player_setEnergy(%player,%amount)
{
	%player.setEnergyLevel(%amount);
}
function VCE_Player_setVelocity(%player,%velocity)
{
	%player.setVelocity(%velocity);
}
function VCE_Player_setPosition(%player,%position)
{
	%player.setTransform(%position SPC getWords(%player.getTransform(),3,7));
}
function VCE_Brick_setColor(%brick,%color)
{
	%brick.setColor(%color);
}
function VCE_Brick_setShapeFX(%brick,%shape)
{
	%brick.setShapeFX(%shape);
}
function VCE_Brick_setColorFX(%brick,%colorfx)
{
	%brick.setColorFX(%colorfx);
}
function VCE_Brick_setPrintCount(%brick,%count)
{
	%brick.setPrintCount(%count);
}
function VCE_Brick_setPrint(%brick,%id)
{
	%brick.setPrint(%id);
}
function VCE_Brick_setBrickName(%brick,%name)
{
	%brick.setNTObjectName(%name);
}
function VCE_Brick_setPrintName(%brick,%name)
{
	if($printNameTable[%name] $= "")
		return;
	%brick.setPrint($printNameTable[%name]);
}
function VCE_Vehicle_setPosition(%vehicle,%position)
{
	%vehicle.setTransform(%position SPC getWords(%player.getTransform(),3,7));
}
function VCE_Vehicle_setDamage(%vehicle,%amount)
{
	%vehicle.setDamageLevel(%amount);
}
function VCE_Vehicle_setVelocity(%vehicle,%velocity)
{
	%vehicle.setVelocity(%velocity);
}
function VCE_Vehicle_setHealth(%vehicle,%amount)
{
	%vehicle.setDamageLevel(%vehicle.getDatablock().maxDamage - %amount);
}
//Shitfuck
function fxDtsBrick::getBrickName(%brick)
{
	%objname = %brick.getName();
	if(strLen(%objname) > 1)
	{
		return getSubStr(%objname,1,strLen(%objname)-1);
	}
	return;
}
function Vehicle::getDriverName(%vehicle)
{
	%driver = %vehicle.getMountObject(0);
	if(isObject(%driver.client))
		return %driver.client.getPlayerName();
	return;
}
function Vehicle::getDriverBL_ID(%vehicle)
{
	%driver = %vehicle.getMountObject(0);
	if(isObject(%driver.client))
		return %driver.client.bl_id;
	return;
}
function fxDtsBrick::getPrintName(%this)
{
	if(%this.getDataBlock().subCategory $= "Prints")
	{
		%texture = getPrintTexture(%this.getPrintID());
		%path = filePath(%texture);
		%underPos = strPos(%path,"_");
		%name = getSubStr(%path,%underPos + 1,strPos(%path,"_",14) - 14) @ "/" @ fileBase(%texture);
		if($printNameTable[%name] !$= "")
			return %name;
	}
}
function mPercent(%num,%total)
{
	return (%num / %total) * 100;
}
function isInt(%string)
{
	return %string $= mFloatLength(%string,0);
}
function getDate()
{
        return getWord(getDateTime(),0);
}
function getTime()
{
        return getWord(getDateTime(),1);
}