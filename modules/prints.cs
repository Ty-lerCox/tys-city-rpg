function serverCmdPrints(%client, %arg1)
{
	if(%arg1 $= "Default") {
		
	} if(%arg1 $= "None") {
		
	} if(%arg1 $= "Business") {
		
	} if(%arg1 $= "Stats") {
		
	} else {
		messageClient(%client, '', "\c6Please choose from the following: Stats, Business, None");
		messageClient(%client, '', "\c6Ex: \c3/Prints business");
	}
	%client.SetInfo();
}