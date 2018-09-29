//---
//	@package VCE
//	@title Package
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 5:30 PM 16/04/2011
//---
package VCE_Main
{
	function fxDtsBrick::onPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		
		return Parent::onPlant(%brick);
	}
	function fxDtsBrick::onLoadPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		
		return Parent::onLoadPlant(%brick);
	}
	function fxDtsBrick::processInputEvent(%brick,%event,%client)
	{
		%client.processingBrick = %brick;
		
		return Parent::processInputEvent(%brick,%event,%client);
	}
	function gameConnection::ChatMessage(%client,%msg)
	{
		return Parent::ChatMessage(%client,filterVariableString(%msg,%client.processingBrick,%client,%client.player));
	}
	function gameConnection::CenterPrint(%client,%msg,%time,%thing)
	{
		return Parent::CenterPrint(%client,filterVariableString(%msg,%client.processingBrick,%client,%client.player),%time,%thing);
	}
	function gameConnection::BottomPrint(%client,%msg,%time,%thing)
	{
		return Parent::BottomPrint(%client,filterVariableString(%msg,%client.processingBrick,%client,%client.player),%time,%thing);
	}
	function miniGameSO::BottomPrintAll(%mini,%msg,%time,%client)
	{
		if(isObject(%client))
			%msg = strReplace(%msg,"%1",%client.getPlayerName());
		for(%i=0;%i<%mini.numMembers;%i++)
			commandtoclient(%mini.member[%i],'bottomPrint',filterVariableString(%msg,%client.processingBrick,%mini.member[%i],%mini.member[%i].player),%time);
	}
	function miniGameSO::CenterPrintAll(%mini,%msg,%time,%client)
	{
		if(isObject(%client))
			%msg = strReplace(%msg,"%1",%client.getPlayerName());
		for(%i=0;%i<%mini.numMembers;%i++)
			commandtoclient(%mini.member[%i],'centerPrint',filterVariableString(%msg,%client.processingBrick,%mini.member[%i],%mini.member[%i].player),%time);
	}
	function miniGameSO::ChatMsgAll(%mini,%msg,%client)
	{
		return;
		//for(%i=0;%i<%mini.numMembers;%i++)
			//messageClient(%mini.member[%i],'',addTaggedString(filterVariableString(%msg,%client.processingBrick,%mini.member[%i],%mini.member[%i].player)),%client.getPlayerName());
	}
	function servercmdMessageSent(%client,%message)
	{
		%client.lastMessage = %message;
		%mini = getMinigameFromObject(%client);
		if(isObject(%mini))
			%mini.lastMessage = %message;
		$VCE::Other::LastMessage = %message;
		return Parent::servercmdMessageSent(%client,%message);
	}
	function servercmdteamMessageSent(%client,%message)
	{
		%client.lastTeamMessage = %message;
		return Parent::servercmdteamMessageSent(%client,%message);
	}
	function Armor::onTrigger(%datablock,%player,%slot,%val)
	{
		Parent::onTrigger(%datablock,%player,%slot,%val);
		switch(%slot)
		{
			case 0:
				%player.fire = %val;
			case 2:
				%player.jump = %val;
				%player.vceSitting = 0;
			case 3:
				%player.crouch = %val;
			case 4:
				%player.jet = %val;
		}
	}
	function serverCmdSit(%client)
	{
		Parent::serverCmdSit(%client);
		if(!isObject(%client.player))
			return;
		switch(%client.player.vceSitting)
		{
			case 1:
				%client.player.vceSitting = 0;
			case 0:
				%client.player.vceSitting = 1;
		}
	}
	function GameConnection::onDeath(%client,%source,%sourceClient,%type,%area)
	{
		%client.vceDeaths++;
		if(%client != %sourceClient && isObject(%sourceClient))
			%sourceClient.vceKills++;
		return Parent::onDeath(%client,%source,%sourceClient,%type,%area);
	}
};
