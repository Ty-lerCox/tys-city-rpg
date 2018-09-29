function servercmdPurge(%client)
{
	if(%client.isSuperAdmin)
	{
		if(!isActivePackage(purge))
		{
		messageAll('', "<font:Arial Black:24>\c6" @ %client.name SPC "has triggered a purge! All crimes are legal including murder!");
		messageClient(%client,'',"End the purge with '/EndPurge'");
		activatePackage(purge);
		}		
		else
		{
		messageClient(%client,'',"The purge is already happening!");
		}
	}
}
 
function servercmdEndPurge(%client)
{
	if(%client.isSuperAdmin)
	{
		if(isActivePackage(purge))
		{
			messageAll('', "<font:Arial Black:24>\c6" @ %client.name SPC "ended the purge");
			deactivatePackage(purge);
			for (%c = 0; %c < ClientGroup.getCount(); %c++) 
			{
				%client = ClientGroup.getObject(%c);
				messageClient(%client,'',"\c6Any demierits you earned were reduced to zero due to the purge.");
				messageClient(%client,'',"\c6Any records you earned were removed due to the purge.");
				CityRPGData.getData(%client.bl_id).valueDemerits = 0;
				CityRPGData.getData(%client.bl_id).valueJailData = "0" SPC getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1);
			}
		}
	}
}
package purge
{
	function CityRPG_IllegalAttack()
	{
		return false;
	}
 
	function CityRPGPlayerBatonImage::onHitObject()
	{
		return;
	}
	
};
 
function isActivePackage(%package)
{
  if (!isPackage(%package))
    return 0;
 
  %count = getNumActivePackages();
 
  for (%i = 0; %i < %count; %i++)
  {
    if (getActivePackage(%i) $= %package)
      return 1;
  }
 
  return 0;
}