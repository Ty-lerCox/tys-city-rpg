function serverCmdGangWarz(%client)
{
	if($GangWarz::Disabled)
		return;
	if((CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather") || (CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss"))
	{
		if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz"))
		{
			messageClient(%client,'',"\c3Gang Warz");
			messageClient(%client,'',"\c6Gang Warz has been \c3disabled \c6for your gang");
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz", false);
		} else {
			messageClient(%client,'',"\c3Gang Warz");
			messageClient(%client,'',"\c6Gang Warz has been \c3enabled \c6for your gang");
			messageClient(%client,'',"\c6Your gang has now been given the right to the following:");
			messageClient(%client,'',"\c31) \c6May kill other gangs that are in gang warz and only receive 10% the demerits.");
			messageClient(%client,'',"\c32) \c6Gang scoring: GodFather Are +5 or -5 || Mobboss are +3 or -3 || Mobster are +1 or -1");
			messageClient(%client,'',"\c33) \c6");
			messageClient(%client,'',"\c34) \c6");
			messageClient(%client,'',"\c35) \c6");
			inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz", true);
		}
	}
	else
	{
		messageClient(%client,'',"\c3Gang Warz");
		messageClient(%client,'',"\c6You must be a \c3Godfather\c6 or \c3Mobboss\c6 of the gang to toggle gang warz.");
		
		messageClient(%client,'',"\c6Features of gangwarz are:");
		messageClient(%client,'',"\c31) \c6May kill other gangs that are in gang warz and only receive 10% the demerits.");
	}
}

function getTopGang()
{
	%topCount = 0;
	%topClient = 0;
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		//if player is in gang
		if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz"))
		{
			//if gang is top score
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") > %topCount)
			{
				%topCount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score");
				%topClient = %client;
			}
		}
	}
	return getGang(CityRPGData.getData(%topClient.bl_id).valueGangID, "Name");
}


function getTopGangScore()
{
	%topCount = 0;
	%topClient = 0;
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		//if player is in gang
		if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz"))
		{
			//if gang is top score
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") > %topCount)
			{
				%topCount = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score");
				%topClient = %client;
			}
		}
	}
	return getGang(CityRPGData.getData(%topClient.bl_id).valueGangID, "Score");
}

