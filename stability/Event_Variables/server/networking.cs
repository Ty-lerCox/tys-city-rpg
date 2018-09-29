//---
//	@package VCE
//	@title Networking
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 4:09 PM 15/03/2011
//---
function serverCmdBlockVCEByID(%client,%BLID)
{
	if(%BLID $= "" || !isInt(%BLID))
	{
		return;
	}
	switch(%client.blockVCE[%BLID])
	{
		case 0:
			%client.blockVCE[%BLID] = true;
			messageClient(%client,'','\c6You are now blocking the ID \c0%1\c6 from using any variable events on you.',%BLID);
		case 1:
			%client.blockVCE[%BLID] = false;
			messageClient(%client,'','\c6You are no longer blocking the ID \c0%1\c6 from using any variable events on you.',%BLID);
	}
}
function serverCmdVCE_onLink(%client,%brick,%index)
{
	if(%client.varLink[%brick] == true && isObject(%brick) && %brick.varLink[%client] !$= "" && getFieldCount(%brick.varLink[%client])-1 >= %index)
	{
		%data = strReplace(getField(%brick.varLink[%client],%index),"=","\t");
		%brick.getGroup().vargroup.setVariable("Client",getField(%data,0),getField(%data,1),%client);
		%brick.varLink[%client] = "";
		%client.varLink[%brick] = "";
		$inputTarget_Self = %brick;
		$inputTarget_Player = %client.player;
		$inputTarget_Bot = %brick.vehicle;
		$inputTarget_Client = %client;
		$inputTarget_Minigame = getMinigameFromObject(%client);
		%brick.processInputEvent("onVariableUpdate",%client);
	}
}
function serverCmdVCE_Handshake(%client)
{
	%client.hasVCE = 1;
}