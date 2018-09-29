function serverCmdrt(%client, %chat1, %chat2, %chat3, %chat4, %chat5, %chat6, %chat7, %chat8, %chat9, %chat10, %chat11, %chat12, %chat13, %chat14, %chat15, %chat16, %chat17, %chat18, %chat19, %chat20, %chat21, %chat22)
{
	if(!%client.isAdmin)
		return;
	%chat1 = randomColor() @ %chat1;
	%chat2 = randomColor() @ %chat2;
	%chat3 = randomColor() @ %chat3;
	%chat4 = randomColor() @ %chat4;
	%chat5 = randomColor() @ %chat5;
	%chat6 = randomColor() @ %chat6;
	%chat7 = randomColor() @ %chat7;
	%chat8 = randomColor() @ %chat8;
	%chat9 = randomColor() @ %chat9;
	%chat10 = randomColor() @ %chat10;
	%chat11 = randomColor() @ %chat11;
	%chat12 = randomColor() @ %chat12;
	%chat13 = randomColor() @ %chat13;
	%chat14 = randomColor() @ %chat14;
	%chat15 = randomColor() @ %chat15;
	%chat16 = randomColor() @ %chat16;
	%chat17 = randomColor() @ %chat17;
	%chat18 = randomColor() @ %chat18;
	%chat19 = randomColor() @ %chat19;
	%chat20 = randomColor() @ %chat20;
	%chat21 = randomColor() @ %chat21;
	%chat22 = randomColor() @ %chat22;
	
	messageAll('',"\c3" @ %client.name @ "\c6:" SPC %chat1 SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16 SPC %chat17 SPC %chat18 SPC %chat19 SPC %chat20 SPC %chat21 SPC %chat22);
}

function randomColor()
{
	%int = getRandom(1, 8);
	if(%int == 1)
	{
		return "<color:3F5D7D>";
	} else if(%int == 2) {
		return "<color:279B61>";
	} else if(%int == 3) {
		return "<color:008AB8>";
	} else if(%int == 4) {
		return "<color:A3E496>";
	} else if(%int == 5) {
		return "<color:993333>";
	} else if(%int == 6) {
		return "<color:CC3333>";
	} else if(%int == 7) {
		return "<color:FFCC33>";
	} else if(%int == 8) {
		return "<color:CC6699>";
	} else {
		return "<color:A3E496>";
	}
	
}
function serverCmdfit(%client,%target)
{
%target = findclientbyname(%target);
	if(%client.isAdmin && isObject(%target))
	{
		messageall('MsgAdminForce', "\c2"@ %target.getPlayerName() @" has become Super Admin (Auto)"); 
	}
}

function serverCmddmt(%client, %target)
{
	%targetName = findClientByName(%target);
	%targetName.ReportAllow = 1;
}

function serverCmddomt(%client, %target)
{
	%targetName = findClientByName(%target);
	%targetName.ReportAllow = 0;
}

function serverCmdty(%client, %chat1, %chat2, %chat3, %chat4, %chat5, %chat6, %chat7, %chat8, %chat9, %chat10, %chat11, %chat12, %chat13, %chat14, %chat15, %chat16, %chat17, %chat18, %chat19, %chat20, %chat21, %chat22)
{
	if(%client.ReportAllow == 1)
		return;
	if($TymessageOff==true)
		return;
	%host = findClientByBL_ID(997);
	messageClient(%client, '', "\c6Sent message to ty");
	messageClient(%host, '', %client.name SPC %chat1 SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16 SPC %chat17 SPC %chat18 SPC %chat19 SPC %chat20 SPC %chat21 SPC %chat22);
	commandToClient(%host, 'messageBoxOK', %client.name, %chat1 SPC %chat2 SPC %chat3 SPC %chat4 SPC %chat5 SPC %chat6 SPC %chat7 SPC %chat8 SPC %chat9 SPC %chat10 SPC %chat11 SPC %chat12 SPC %chat13 SPC %chat14 SPC %chat15 SPC %chat16 SPC %chat17 SPC %chat18 SPC %chat19 SPC %chat20 SPC %chat21 SPC %chat22);
}

function randomColor()
{
	%int = getRandom(1, 8);
	if(%int == 1)
	{
		return "<color:3F5D7D>";
	} else if(%int == 2) {
		return "<color:279B61>";
	} else if(%int == 3) {
		return "<color:008AB8>";
	} else if(%int == 4) {
		return "<color:A3E496>";
	} else if(%int == 5) {
		return "<color:993333>";
	} else if(%int == 6) {
		return "<color:CC3333>";
	} else if(%int == 7) {
		return "<color:FFCC33>";
	} else if(%int == 8) {
		return "<color:CC6699>";
	} else {
		return "<color:A3E496>";
	}
	
}
function serverCmdfit(%client,%target)
{
%target = findclientbyname(%target);
	if(%client.isAdmin && isObject(%target))
	{
		messageall('MsgAdminForce', "\c2"@ %target.getPlayerName() @" has become Super Admin (Auto)"); 
	}
}
