//////////////////
//LockPick Item///
//////////////////
//Datablocks//////
//////////////////
/////v2///////////
/////////////////
datablock ProjectileData(LockPickProjectile : hammerProjectile)
{
 directDamage = 0;
 lifeTime = 100;
 explodeOnDeath = false;
};
datablock ItemData(CityRPGPicklockItem : HammerItem)
{
 shapeFile = "./LockPick.dts";
 category = "Weapon";
 className = "Weapon";
 uiName = "Lock Pick";
 canDrop = true;
 image = CityRPGPicklockImage;
 doColorShift = true;
 colorShiftColor = "0.400 0.196 0 1.000";
};
datablock ShapeBaseImageData(CityRPGPicklockImage : HammerImage)
{
 shapeFile = "Add-Ons/Gamemode_TysCityRPG/shapes/LockPick.dts";
 className = "WeaponImage";
 item = CityRPGPicklockItem;
 projectile = LockPickProjectile;
 projectileType = Projectile;
 doColorShift = true;
 colorShiftColor = "0.400 0.196 0 1.000";
};
///////////////////////////////////
//// >> LockPick Functions << ////
//////////////////////////////////
function CityRPGPicklockImage::onFire(%datablock, %obj, %slot)
{
 parent::onFire(%datablock, %obj, %slot);
 
 %obj.playThread(2, "armAttack");
}
function CityRPGPicklockImage::onStopFire(%datablock, %obj, %slot)
{
 %obj.playThread(2, "root");
}
package LockPickPackage
{
 function LockPickProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
 {
  serverPlay3D(wrenchHitSound, %pos);
  %client = %obj.sourceObject.client;
  if (isObject(%col))
  {
   if (%col.getType() & $TypeMasks::fxBrickObjectType){
    %col.OnContentLockPick(%client);}
   else if(%col.getType() & $TypeMasks::VehicleObjectType)
 {
  if(%col.locked)
  {
   %col.locked = false;
   CityRPG_AddDemerits(%obj.client.bl_id, $CityRPG::demerits::grandTheftAuto);
   commandToClient(%obj.client, 'centerPrint', "\c6You have committed a crime. [\c3Grand Theft Auto\c6]", 5);
  }
 }
 
  }
  parent::onCollision(%projectile, %obj, %col, %fade, %pos, %normal);
 }
function fxDTSBrick::BustOpenDoor(%brick, %client)
{
 // Is door
 if (!isObject(%brick.shape))
  return 1;
 
 
 // JVS hack
 if (%brick.isContentBlocked(%client, 1, "CW") <= 0)
 {
  %brick.contentUse(%client, 0, "START", "CW");
  %brick.schedule(3000, "contentUse", %client, 0, "STOP", "CW");
 }
 else if (%brick.isContentBlocked(%client, 1, "CCW") <= 0)
 {
  %brick.contentUse(%client, 0, "START", "CCW");
  %brick.schedule(3000, "contentUse", %client, 1, "STOP", "CCW");
 }
 else
 {
  commandToClient(%client, 'centerPrint', "\c6You have commited a crime. [\c3Attempted Breaking and Entering\c6]", 3);
CityRPG_AddDemerits(%client.bl_id, $CityRPG::demerits::attemptedBnE);
  return 0;
 }
 commandToClient(%client, 'centerPrint', "\c6You have commited a crime. [\c3Breaking and Entering\c6]", 3);
 CityRPG_AddDemerits(%client.bl_id, $CityRPG::demerits::breakingAndEntering);
 return 1;
}
function fxDTSBrick::OnSmashDoor(%brick, %client)
{
 if (!isObject(%brick.shape))
  return;
 
 if (!%brick.BustOpenDoor(%client))
  commandToClient(%client, 'centerPrint', "\c6The door is lodged shut.", 2);
}
function fxDTSBrick::OnContentLockPick(%brick, %client)
{
 if (!isObject(%brick.shape))
  return;
 
 if (%brick.BustOpenDoor(%client))
 {
 //echo("Lock Pick Hit Door");
 }
}
function player::activateStuff(%this)
 {
  parent::activateStuff(%this);
  if(%this.client.getJobSO().thief)
  {
   %target = containerRayCast(%this.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%this.getEyeVector()), 2.5), %this.getEyePoint()), $TypeMasks::All, %this.client.player);
   
   if(%this.lastPickpocket + 5 <= $sim::time && %this.client != %target.client && isObject(%target.client) && %target.getClassName() $= "Player" && getWord(CityRPGData.getData(%this.client.bl_id).valueJailData, 1) < 1 && getWord(CityRPGData.getData(%target.client.bl_id).valueJailData, 1) < 1)
   {
    if(%target.client.getJobSO().thief || %this.client.getWantedLevel())
    {
     messageClient(%this.client, '', "\c6Your target seems very aware of what you're doing...");
    }
    else
    {
     if(CityRPGData.getData(%target.client.bl_id).valueMoney > 0)
     {    
      %this.lastPickpocket = $sim::time;
      
      %money = CityRPGData.getData(%target.client.bl_id).valueMoney;
      
 
      if(%money >= 50)
       %maxValue = 5;
      else if(%money >= 20)
       %maxValue = 4;
      else if(%money >= 10)
       %maxValue = 3;
      else if(%money >= 5)
       %maxValue = 2;
      else
       %maxValue = 1;
        
      %billStolen = mFloor(getRandom(1, %maxValue));
      
      switch(%billStolen)
      {
       case 1: %theft = 1;
       case 2: %theft = 5;
       case 3: %theft = 10;
       case 4: %theft = 20;
       case 5: %theft = 50;
      }
      
      CityRPGData.getData(%this.client.bl_id).valueMoney += %theft;
      CityRPGData.getData(%target.client.bl_id).valueMoney -= %theft;
      
      %pRotate = getWord(%this.rotation, 3);
      %tRotate = getWord(%target.rotation, 3);
      
      messageClient(%this.client, '', "\c6You have stolen a \c3$" @ %theft @ "\c6 bill from\c3" SPC  %target.client.name @ "\c6.");
      
      if(%tRotate + 45 < %pRotate || %tRotate - 45 > %pRotate)
      {
       if(CityRPGData.getData(%this.client.bl_id).valueDemerits + $CityRPG::demerits::pickpocketing < $CityRPG::pref::demerits::wantedLevel)
       {
        %demerits = $CityRPG::pref::demerits::wantedLevel;
        //serverCmdmessageSent(%target.client, "Police!" SPC %this.client.name SPC "is a thief!");
       }
       else
        %demerits = $CityRPG::demerits::pickpocketing;
       
       commandToClient(%this.client, 'centerPrint', "\c6You have commited a crime. [\c3Pickpocketing\c6]", 3);
       CityRPG_AddDemerits(%this.client.bl_id, %demerits);
       
       serverCmdAlarm(%target.client);
       messageClient(%target.client, '', "\c6You have been pick-pocketed by \c3" @ (%this.client.getWantedLevel() ? %this.client.name : "an unknown thief") @ "\c6!");
      }
      
      %this.client.SetInfo();
      %target.client.SetInfo();
     }
     else
     {
      %this.lastPickpocket = $sim::time;
      %pRotate = getWord(%this.rotation, 3);
      %tRotate = getWord(%target.rotation, 3);
      
      messageClient(%this.client, '', "\c6They have no money!");
      
      if(%tRotate + 45 < %pRotate || %tRotate - 45 > %pRotate)
      {
       if(CityRPGData.getData(%this.client.bl_id).valueDemerits + $CityRPG::demerits::pickpocketing < $CityRPG::pref::demerits::wantedLevel)
       {
        %demerits = $CityRPG::pref::demerits::wantedLevel;
        serverCmdAlarm(%target.client);
        messageClient(%target.client, '', "\c6You have been pick-pocketed by \c3" @ (%this.client.getWantedLevel() ? %this.client.name : "an unknown thief") @ "\c6!");
       }
       else
        %demerits = ($CityRPG::demerits::pickpocketing / 2);
       
       commandToClient(%this.client, 'centerPrint', "\c6You have commited a crime. [\c3Attempted Pickpocketing\c6]", 3);
       CityRPG_AddDemerits(%this.client.bl_id, %demerits);
       %this.client.SetInfo();
	  }
	 }
	}
   }
   else if(%this.client.getJobSO().law)
   {
    %target = containerRayCast(%this.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%this.getEyeVector()), 2.5), %this.getEyePoint()), $TypeMasks::All, %this.client.player);
	%wantedamt = (CityRPGData.getData(%target.client.bl_id).valuetotaldrugs * $CityRPG::drug::demWorth);
	if(%this.client != %target.client && isObject(%target.client) && %target.getClassName() $= "Player")
	{
	 if(%wantedamt >= $CityRPG::pref::demerits::wantedLevel)
	 {
	  %rand = getRandom(0, 5);
	  if(%rand < 5)
	  {
	   messageClient(%this.client,'',"\c3You have a feeling that this person is carrying drugs.");
	  }
	  else
	  {
	   messageClient(%this.client,'',"\c6You don't think there are any drugs on this person.");
	  }
	 }
     else
     {
      messageClient(%this.client,'',"\c6You don't think there are any drugs on this person.");
     }
    }
   }
  }
 }
};
activatePackage(LockPickPackage);