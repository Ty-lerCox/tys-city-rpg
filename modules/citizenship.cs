function serverCmdCitizen(%client, %city)
{
	%client.Civilian = %city;
	messageClient('',"\c6You're now civilian of \c3" @ %client.Civilian);
}