function servercmdgc(%client, %Chat, %Chat2, %Chat3, %Chat4, %Chat5, %Chat6, %Chat7, %Chat8, %Chat9, %Chat10, %Chat11, %Chat12, %Chat13, %Chat14, %Chat15, %Chat16)
{
	if(CityRPGData.getData(%client.bl_id).valueGangID)
	{
		if((!(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color") $= "")) || (!(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color") $= null))) {
			%gangColor = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color");
		} else {
			%gangColor = "\c7";
		}
		if(%gangColor $= "")
			%gangColor = "\c7";
		%count = ClientGroup.getCount();
		for(%cl = 0; %cl < %count; %cl++)
		{
			%clientB = ClientGroup.getObject(%cl);
			if(CityRPGData.getData(%clientB.bl_id).valueGangID == CityRPGData.getData(%client.bl_id).valueGangID)
			{
				Messageclient(%clientb, '', %gangColor @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") @ "(\c3" @ %client.name @ %gangColor @ ") \c3:\c6 "@ %chat SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16);
			}
		}
	}
	else
	{
		messageclient(%client, '', "\c3You arn't in a gang!");
	}
}