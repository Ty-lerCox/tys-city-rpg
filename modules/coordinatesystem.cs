$Cords::SafeZones::Disabled = 0;
$Cords::SafeZones::Invert = 0;

$Cords::SafeZone1::Cord1 = "-57.0789 -61.8862 8.61";
$Cords::SafeZone1::Cord2 = "6.23034 1.52315 8.61";
$Cords::SafeZone2::Cord1 = "";
$Cords::SafeZone2::Cord2 = "";
$Cords::SafeZone3::Cord1 = "";
$Cords::SafeZone3::Cord2 = "";
$Cords::SafeZone4::Cord1 = "";
$Cords::SafeZone4::Cord2 = "";

// $Cords::SafeZone1::Cord1 = "";
// $Cords::SafeZone1::Cord2 = "";
// $Cords::SafeZone2::Cord1 = "";
// $Cords::SafeZone2::Cord2 = "";
// $Cords::SafeZone3::Cord1 = "";
// $Cords::SafeZone3::Cord2 = "";
// $Cords::SafeZone4::Cord1 = "";
// $Cords::SafeZone4::Cord2 = "";

//safeZone
function serverCmdCreateSafeZone(%client, %zoneNum, %CordNum)
{
	if(!%client.isAdmin)
		return messageClient(%client, '', "Admin Only");
	
	if(%zoneNum == 1)
	{
		if(%CordNum == 1)
			$Cords::SafeZone1::Cord1 = %client.player.getPosition();
		else if(%CordNum == 2)
			$Cords::SafeZone1::Cord2 = %client.player.getPosition();
		else
			messageClient(%client, '', "Specify which cord");
	} 
	else if(%zoneNum == 2)
	{
		if(%CordNum == 1)
			$Cords::SafeZone2::Cord1 = %client.player.getPosition();
		else if(%CordNum == 2)
			$Cords::SafeZone2::Cord2 = %client.player.getPosition();
		else
			messageClient(%client, '', "Specify which cord");
	} 
	else if(%zoneNum == 3)
	{
		if(%CordNum == 1)
			$Cords::SafeZone3::Cord1 = %client.player.getPosition();
		else if(%CordNum == 2)
			$Cords::SafeZone3::Cord2 = %client.player.getPosition();
		else
			messageClient(%client, '', "Specify which cord");
	} 
	else if(%zoneNum == 4)
	{
		if(%CordNum == 1)
			$Cords::SafeZone4::Cord1 = %client.player.getPosition();
		else if(%CordNum == 2)
			$Cords::SafeZone4::Cord2 = %client.player.getPosition();
		else
			messageClient(%client, '', "Specify which cord");
	} else
		messageClient(%client, '', "Specify which zone");
}

function inSafeZone(%client)
{
	if($Cords::SafeZones::Invert)
	{
		%pos = %client.player.getPosition();
		if(getX(%pos) > getX($Cords::SafeZone1::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone1::Cord2))
				if(getY(%pos) < getY($Cords::SafeZone1::Cord1))
					if(getY(%pos) > getY($Cords::SafeZone1::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone2::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone2::Cord2))
				if(getY(%pos) < getY($Cords::SafeZone2::Cord1))
					if(getY(%pos) > getY($Cords::SafeZone2::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone3::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone3::Cord2))
				if(getY(%pos) < getY($Cords::SafeZone3::Cord1))
					if(getY(%pos) > getY($Cords::SafeZone3::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone4::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone4::Cord2))
				if(getY(%pos) < getY($Cords::SafeZone4::Cord1))
					if(getY(%pos) > getY($Cords::SafeZone4::Cord2))
						return true;
	} else {
		%pos = %client.player.getPosition();
		if(getX(%pos) > getX($Cords::SafeZone1::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone1::Cord2))
				if(getY(%pos) > getY($Cords::SafeZone1::Cord1))
					if(getY(%pos) < getY($Cords::SafeZone1::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone2::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone2::Cord2))
				if(getY(%pos) > getY($Cords::SafeZone2::Cord1))
					if(getY(%pos) < getY($Cords::SafeZone2::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone3::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone3::Cord2))
				if(getY(%pos) > getY($Cords::SafeZone3::Cord1))
					if(getY(%pos) < getY($Cords::SafeZone3::Cord2))
						return true;
		if(getX(%pos) > getX($Cords::SafeZone4::Cord1))
			if(getX(%pos) < getX($Cords::SafeZone4::Cord2))
				if(getY(%pos) > getY($Cords::SafeZone4::Cord1))
					if(getY(%pos) < getY($Cords::SafeZone4::Cord2))
						return true;
	}
		return false;
}

function serverCmdsafeAmI(%client)
{
	if(inSafeZone(%client))
		messageClient(%client,'',"Safe");
	else
		messageClient(%client,'',"Not safe");
}

function serverCmdClearSafeZone(%client, %zoneNum)
{

}

//useful commands
function serverCmdGetCords(%client)
{
	messageClient(%client,'',"Cords:" SPC getXClient(%client) SPC getYClient(%client) SPC getZClient(%client));
}

function getX(%pos)
{
	return getWords(%pos, 0, 0);
}

function getY(%pos)
{
	return getWords(%pos, 1, 1);
}

function getZ(%pos)
{
	return getWords(%pos, 2, 2);
}

function getXClient(%client)
{
	return getWords(%client.player.getPosition(), 0, 0);
}

function getYClient(%client)
{
	return getWords(%client.player.getPosition(), 1, 1);
}

function getZClient(%client)
{
	return getWords(%client.player.getPosition(), 2, 2);
}

//spot & GOTO commands
function serverCmdSpot(%client)
{
	if(%client.isAdmin)
	{
		%client.spot = %client.player.getPosition();
		messageClient(%client,'',"pos saved. type /goto to return");
	} else
		messageClient(%client,'',"must be an admin.");
}

function serverCmdgoto(%client)
{
	if(%client.isAdmin)
	{
		if(%client.spot)
		{
			if(!isObject(%client))
				return;
			//name(deve).player.setTransform(%client.spot);
			//name(ty).player.setTransform(-135 -573.5 12.9);
			//-135 -573.5 12.9
			%client.player.setTransform(%client.spot);
		}
		else
			messageClient(%client,'',"Please type /spot");
	}
}
