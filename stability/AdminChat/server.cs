RTB_registerPref("Color code 1", "Admin Chat", "$AC::Color1", "string 100", "Server_AdminChat", "<color:26FF00>", 0, 0, "");
RTB_registerPref("Color code 2", "Admin Chat", "$AC::Color2", "string 100", "Server_AdminChat", "<color:00FFEA>", 0, 0, "");
RTB_registerPref("Color code 3", "Admin Chat", "$AC::Color3", "string 100", "Server_AdminChat", "<color:FFFB00>", 0, 0, "");

function servercmdac(%client, %Chat, %Chat2, %Chat3, %Chat4, %Chat5, %Chat6, %Chat7, %Chat8, %Chat9, %Chat10, %Chat11, %Chat12, %Chat13, %Chat14, %Chat15, %Chat16)
{
	if(%client.isadmin)
	{
		%count = ClientGroup.getCount();
		for(%cl = 0; %cl < %count; %cl++)
		{
			%clientB = ClientGroup.getObject(%cl);
			if(%clientB.isadmin)
			{
				Messageclient(%clientb, '', $AC::Color1 @ "AdminChat(" @ $AC::Color2 @ %client.name @ $AC::Color1 @ ")" @ $AC::Color3 @": "@ %chat SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16);
			}
		}
	}
	else
	{
		messageclient(%client, '', "\c3You aren't admin!");
	}
}