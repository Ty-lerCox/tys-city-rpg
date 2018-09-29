$Game::Max::Destroy = 3000;
$Game::WarrantMins = 5;
function KillingBricks(%client, %col)
{
	if(!%client.brickDelay)
	{
		if($Debug)
			talk(%col.getVolume());
		if(%col.client.isAdmin)
			return commandToClient(%client, 'centerPrint', "Not these bricks...", 1);
		if(%col.getVolume() > $Game::Max::Destroy)
			return commandToClient(%client, 'centerPrint', "Too Large...", 1);
		%time = 60000 * $Game::WarrantMins;
		%col.fakeKillBrick(3,9);
		%client.brickDelay = true;
		schedule(500, 0, clearBrickDelay, %client);
		if(!%client.beginLoseWarrant)
		{
			schedule(%time, 0, serverCmdloseWarrant, %client);
			%client.beginLoseWarrant=true;
		}
	}
	else
	{
		commandToClient(%client, 'centerPrint', "Please wait 0.5secs between each brick.", 1);
	}
}

function clearBrickDelay(%client)
{
	%client.brickDelay = false;
}

function serverCmdloseWarrant(%client)
{
	messageClient(%client,'',"Warrant has worn off.");
	%client.warrant = false;
	%client.beginLoseWarrant=false;
}