//Written by: Rarw Muffinz


$AdvTimeout = 60;
$AdvCharPrice = 5;
//Dollars for ad words
 
function servercmdAdv(%client , %ad, %ad2, %ad3, %ad4, %ad5, %ad6, %ad7, %ad8, %ad9, %ad10)
{
	//First step is to check if the user is not in the timeout
	if(!%client.canAdv == 1 || %client.isAdmin)
	{
		//echo(strlen(%ad));
		//Alright timeout hasnt been reached
		//check if the ad is nothing
		if(strlen(%ad) == 0)
		{
			messageClient(%client,'',"\c6Advertising costs \c3$" @ $AdvCharPrice SPC "\c6for each character in your advertisement.");
			messageClient(%client,'',"\c6Type /Adv [advertisement]");
			return;
		}	
		//alright not a nothing ad
		//Time to price
		%price = (strlen(%ad) + strlen(%ad2) + strlen(%ad3) + strlen(%ad4) + strlen(%ad5) + strlen(%ad6) + strlen(%ad7) + strlen(%ad8) + strlen(%ad9) + strlen(%ad10)) * $AdvCharPrice;
		
		//ok price calculated. time to check if the user has the money
		if(%price > cityRPGData.getData(%client.bl_id).valueMoney )
		{
			messageClient(%client, '', "\c6 You need \c3$" @ %price @ " \c6dollars to publish this ad!");
		}
		else
		{
			messageClient(%client, '', "\c6 You paid \c3$" @ %price @ " \c6dollars to publish this ad!");
			CityRPGData.getData(%client.bl_id).valueMoney -= %price;
			messageAll('', "\c6" SPC %ad SPC %ad2 SPC %ad3 SPC %ad4 SPC %ad5 SPC %ad6 SPC %ad7 SPC %ad8 SPC %ad9 SPC %ad10 SPC "\c6-" SPC %client.name);
			AdvTimeout(%client);
		}
	
	}
	else
	{
		messageClient(%client, '', "\c6 Its been less than " @ $AdvTimeout @ " seconds since you used that command!");
		return;
	}
}
 
function AdvTimeout(%client)
{
	// 1 = no
	// 0 = yes
	
	%client.canAdv = 1;
	schedule($AdvTimeout * 1000, 0 , "AdvTimein", %client);
	//echo("timeeed");
}
 
function AdvTimein(%client)
{
	//echo(%client);
	%client.canAdv = 0;
}