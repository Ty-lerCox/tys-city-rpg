RTB_registerPref("Color code 1", "Mod Chat", "$MC::Color1", "string 100", "Server_ModChat", "<color:477689>", 0, 0, "");
RTB_registerPref("Color code 2", "Mod Chat", "$MC::Color2", "string 100", "Server_ModChat", "<color:ffffff>", 0, 0, "");
RTB_registerPref("Color code 3", "Mod Chat", "$MC::Color3", "string 100", "Server_ModChat", "<color:ffffff>", 0, 0, "");
$MC::Color1 = "<color:477689>";
$MC::Color2 = "<color:ffffff>";
$MC::Color3 = "<color:ffffff>";

function servercmdmodc(%client, %Chat, %Chat2, %Chat3, %Chat4, %Chat5, %Chat6, %Chat7, %Chat8, %Chat9, %Chat10, %Chat11, %Chat12, %Chat13, %Chat14, %Chat15, %Chat16)
{
	if((%client.isModerator) || (%client.isAdmin))
	{
		%count = ClientGroup.getCount();
		for(%cl = 0; %cl < %count; %cl++)
		{
			%clientB = ClientGroup.getObject(%cl);
			if((%clientB.isModerator) || (%clientB.isAdmin))
			{
				Messageclient(%clientb, '', $MC::Color1 @ "ModChat(" @ $MC::Color2 @ %client.name @ $MC::Color1 @ ")" @ $MC::Color3 @": "@ %chat SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16);
			}
		}
	}
	else
	{
		messageclient(%client, '', "\c3You aren't Mod!");
	}
}