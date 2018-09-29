//---
//	@package VCE
//	@title Output Events
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 4:39 PM 15/03/2011
//---
$VCE::Server::ImmuneTime = 5000;
function fxDtsBrick::VCE_modVariable(%brick,%type,%name,%logic,%value,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		switch(%type)
		{
			case 0:
				%category = "Brick";
				%target = %brick;
				%class = "fxDtsBrick";
			case 1:
				%category = "Player";
				%target = %client.player;
				%class = "Player";
			case 2:
				%category = "Client";
				%target = %client;
				%class = "GameConnection";
			case 3:
				%category = "Minigame";
				%target = getMinigameFromObject(%brick);
				%class = "MinigameSO";
			case 4:
				%category = "Vehicle";
				%target = %brick.vehicle;
				%class = "Vehicle";
		}
		%name = filterVariableString(%name,%brick,%client,%client.player);
		%oldvalue = %brick.getGroup().vargroup.getVariable(%category,%name,%target);
		%newvalue = filterVariableString(%value,%brick,%client,%client.player);
		if($VCE::Server::SpecialVarEdit[%class,%name] !$= "" && isObject(%target))
		{
			%oldvalue = eval("return" SPC strreplace($VCE::Server::SpecialVar[%class,%var],"%this",%target) @ ";");
			switch(%logic)
			{
				case 1:
					%newvalue = %oldvalue + %newvalue;
				case 2:
					%newvalue = %oldvalue - %newvalue;
				case 3:
					%newvalue = %oldvalue * %newvalue;
				case 4:
					%newvalue = %oldvalue / %newvalue;
				case 5:
					%newvalue = mFloor(%oldvalue);
				case 6:
					%newvalue = mCeil(%oldvalue);
				case 7:
					%newvalue = mPow(%oldvalue,%newvalue);
				case 8:
					if(%newvalue $= "")
						%root = 1 / 2;
					else
						%root = 1 / %newvalue;
					%newvalue = mPow(%oldvalue,%root);
				case 9:
					%newvalue = mPercent(%oldvalue,%newvalue);
				case 10:
					if(isInt(%oldvalue) && isInt(%newvalue))
						%newvalue = getRandom(%oldvalue,%newvalue);
					else
						%newvalue = getRandom();
				case 11:
					if(%newvalue $= "")
						%newvalue = "0";
					if(getWordCount(%newvalue) == 2)
						%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
					else
						%newvalue = getWord(%oldvalue,%newvalue + 1);
				case 12:
					%newvalue = strLwr(%oldvalue);
				case 13:
					%newvalue = strUpr(%oldvalue);
				case 14:
					%newvalue = strChr(%oldvalue,%newvalue);
				case 15:
					%newvalue = strLen(%oldvalue);
			}
			%f = "VCE_" @ %category @ "_" @ $VCE::Server::SpecialVarEdit[%class,%name];
			if(isFunction(%f))
			{
				call(%f,%target,%newvalue,$VCE::Server::SpecialVarEditArg1[%class,%name],$VCE::Server::SpecialVarEditArg2[%class,%name],$VCE::Server::SpecialVarEditArg3[%class,%name],$VCE::Server::SpecialVarEditArg4[%class,%name]);
				return;
			}
		}
		switch(%logic)
		{
			case 1:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue + %newvalue,%target);
			case 2:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue - %newvalue,%target);
			case 3:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue * %newvalue,%target);
			case 4:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue / %newvalue,%target);
			case 5:
				%brick.getGroup().vargroup.setVariable(%category,%name,mFloor(%oldvalue),%target);
			case 6:
				%brick.getGroup().vargroup.setVariable(%category,%name,mCeil(%oldvalue),%target);
			case 0:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 7:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPow(%oldvalue,%newvalue),%target);
			case 8:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue $= "" ? mPow(%oldvalue,1 / 2) : mPow(%oldvalue,1 / %newvalue),%target);
			case 9:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPercent(%oldvalue,%newvalue),%target);
			case 10:
				if(isInt(%oldvalue) && isInt(%newvalue))
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(%oldvalue,%newvalue),%target);
				else
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(),%target);
			case 11:
				if(%newvalue $= "")
					%newvalue = "0";
				if(getWordCount(%newvalue) == 2)
					%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
				else
					%newvalue = getWord(%oldvalue,%newvalue + 1);
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 12:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLwr(%oldvalue),%target);
			case 13:
				%brick.getGroup().vargroup.setVariable(%category,%name,strUpr(%oldvalue),%target);
			case 14:
				%brick.getGroup().vargroup.setVariable(%category,%name,strChr(%oldvalue,%newvalue),%target);
			case 15:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLen(%oldvalue),%target);
		}
	}
	%brick.onVariableUpdate(%client);
}
function MinigameSO::VCE_modVariable(%mini,%name,%logic,%value,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%category = "Minigame";
		%target = %mini;
		%class = "MinigameSO";
		%name = filterVariableString(%name,%brick,%client,%client.player);
		%oldvalue = %brick.getGroup().vargroup.getVariable(%category,%name,%target);
		%newvalue = filterVariableString(%value,%brick,%client,%client.player);
		if($VCE::Server::SpecialVarEdit[%class,%name] !$= "" && isObject(%target))
		{
			%oldvalue = eval("return" SPC strreplace($VCE::Server::SpecialVar[%class,%var],"%this",%target) @ ";");
			switch(%logic)
			{
				case 1:
					%newvalue = %oldvalue + %newvalue;
				case 2:
					%newvalue = %oldvalue - %newvalue;
				case 3:
					%newvalue = %oldvalue * %newvalue;
				case 4:
					%newvalue = %oldvalue / %newvalue;
				case 5:
					%newvalue = mFloor(%oldvalue);
				case 6:
					%newvalue = mCeil(%oldvalue);
				case 7:
					%newvalue = mPow(%oldvalue,%newvalue);
				case 8:
					if(%newvalue $= "")
						%root = 1 / 2;
					else
						%root = 1 / %newvalue;
					%newvalue = mPow(%oldvalue,%root);
				case 9:
					%newvalue = mPercent(%oldvalue,%newvalue);
				case 10:
					if(isInt(%oldvalue) && isInt(%newvalue))
						%newvalue = getRandom(%oldvalue,%newvalue);
					else
						%newvalue = getRandom();
				case 11:
					if(%newvalue $= "")
						%newvalue = "0";
					if(getWordCount(%newvalue) == 2)
						%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
					else
						%newvalue = getWord(%oldvalue,%newvalue + 1);
				case 12:
					%newvalue = strLwr(%oldvalue);
				case 13:
					%newvalue = strUpr(%oldvalue);
				case 14:
					%newvalue = strChr(%oldvalue,%newvalue);
				case 15:
					%newvalue = strLen(%oldvalue);
			}
			%f = "VCE_" @ %category @ "_" @ $VCE::Server::SpecialVarEdit[%class,%name];
			if(isFunction(%f))
			{
				call(%f,%target,%newvalue,$VCE::Server::SpecialVarEditArg1[%class,%name],$VCE::Server::SpecialVarEditArg2[%class,%name],$VCE::Server::SpecialVarEditArg3[%class,%name],$VCE::Server::SpecialVarEditArg4[%class,%name]);
				return;
			}
		}
		switch(%logic)
		{
			case 1:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue + %newvalue,%target);
			case 2:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue - %newvalue,%target);
			case 3:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue * %newvalue,%target);
			case 4:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue / %newvalue,%target);
			case 5:
				%brick.getGroup().vargroup.setVariable(%category,%name,mFloor(%oldvalue),%target);
			case 6:
				%brick.getGroup().vargroup.setVariable(%category,%name,mCeil(%oldvalue),%target);
			case 0:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 7:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPow(%oldvalue,%newvalue),%target);
			case 8:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue $= "" ? mPow(%oldvalue,1 / 2) : mPow(%oldvalue,1 / %newvalue),%target);
			case 9:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPercent(%oldvalue,%newvalue),%target);
			case 10:
				if(isInt(%oldvalue) && isInt(%newvalue))
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(%oldvalue,%newvalue),%target);
				else
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(),%target);
			case 11:
 				if(%newvalue $= "")
					%newvalue = "0";
				if(getWordCount(%newvalue) == 2)
					%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
				else
					%newvalue = getWord(%oldvalue,%newvalue + 1);
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 12:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLwr(%oldvalue),%target);
			case 13:
				%brick.getGroup().vargroup.setVariable(%category,%name,strUpr(%oldvalue),%target);
			case 14:
				%brick.getGroup().vargroup.setVariable(%category,%name,strChr(%oldvalue,%newvalue),%target);
			case 15:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLen(%oldvalue),%target);
		}
	}
}
function Vehicle::VCE_modVariable(%vehicle,%name,%logic,%value,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%category = "Vehicle";
		%target = %vehicle;
		%class = "Vehicle";
		%name = filterVariableString(%name,%brick,%client,%client.player,%vehicle);
		%oldvalue = %brick.getGroup().vargroup.getVariable(%category,%name,%target);
		%newvalue = filterVariableString(%value,%brick,%client,%client.player,%vehicle);
		if($VCE::Server::SpecialVarEdit[%class,%name] !$= "" && isObject(%target))
		{
			%oldvalue = eval("return" SPC strreplace($VCE::Server::SpecialVar[%class,%var],"%this",%target) @ ";");
			switch(%logic)
			{
				case 1:
					%newvalue = %oldvalue + %newvalue;
				case 2:
					%newvalue = %oldvalue - %newvalue;
				case 3:
					%newvalue = %oldvalue * %newvalue;
				case 4:
					%newvalue = %oldvalue / %newvalue;
				case 5:
					%newvalue = mFloor(%oldvalue);
				case 6:
					%newvalue = mCeil(%oldvalue);
				case 7:
					%newvalue = mPow(%oldvalue,%newvalue);
				case 8:
					if(!isInt(%newvalue))
						%root = 1 / 2;
					else
						%root = 1 / %newvalue;
					%newvalue = mPow(%oldvalue,%root);
				case 9:
					%newvalue = mPercent(%oldvalue,%newvalue);
				case 10:
					if(isInt(%oldvalue) && isInt(%newvalue))
						%newvalue = getRandom(%oldvalue,%newvalue);
					else
						%newvalue = getRandom();
				case 11:
					if(%newvalue $= "")
						%newvalue = "0";
					if(getWordCount(%newvalue) == 2)
						%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
					else
						%newvalue = getWord(%oldvalue,%newvalue + 1);
				case 12:
					%newvalue = strLwr(%oldvalue);
				case 13:
					%newvalue = strUpr(%oldvalue);
				case 14:
					%newvalue = strChr(%oldvalue,%newvalue);
				case 15:
					%newvalue = strLen(%oldvalue);
			}
			%f = "VCE_" @ %category @ "_" @ $VCE::Server::SpecialVarEdit[%class,%name];
			if(isFunction(%f))
			{
				call(%f,%target,%newvalue,$VCE::Server::SpecialVarEditArg1[%class,%name],$VCE::Server::SpecialVarEditArg2[%class,%name],$VCE::Server::SpecialVarEditArg3[%class,%name],$VCE::Server::SpecialVarEditArg4[%class,%name]);
				return;
			}
		}
		switch(%logic)
		{
			case 1:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue + %newvalue,%target);
			case 2:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue - %newvalue,%target);
			case 3:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue * %newvalue,%target);
			case 4:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue / %newvalue,%target);
			case 5:
				%brick.getGroup().vargroup.setVariable(%category,%name,mFloor(%oldvalue),%target);
			case 6:
				%brick.getGroup().vargroup.setVariable(%category,%name,mCeil(%oldvalue),%target);
			case 0:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 7:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPow(%oldvalue,%newvalue),%target);
			case 8:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue $= "" ? mPow(%oldvalue,1 / 2) : mPow(%oldvalue,1 / %newvalue),%target);
			case 9:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPercent(%oldvalue,%newvalue),%target);
			case 10:
				if(isInt(%oldvalue) && isInt(%newvalue))
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(%oldvalue,%newvalue),%target);
				else
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(),%target);
			case 11:
				if(%newvalue $= "")
					%newvalue = "0";
				if(getWordCount(%newvalue) == 2)
					%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
				else
					%newvalue = getWord(%oldvalue,%newvalue + 1);
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 12:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLwr(%oldvalue),%target);
			case 13:
				%brick.getGroup().vargroup.setVariable(%category,%name,strUpr(%oldvalue),%target);
			case 14:
				%brick.getGroup().vargroup.setVariable(%category,%name,strChr(%oldvalue,%newvalue),%target);
			case 15:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLen(%oldvalue),%target);
		}
	}
}
function Player::VCE_modVariable(%player,%name,%logic,%value,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%category = "Player";
		%target = %player;
		%class = "Player";
		%name = filterVariableString(%name,%brick,%client,%player);
		%oldvalue = %brick.getGroup().vargroup.getVariable(%category,%name,%target);
		%newvalue = filterVariableString(%value,%brick,%client,%player);
		if($VCE::Server::SpecialVarEdit[%class,%name] !$= "" && isObject(%target))
		{
			%oldvalue = eval("return" SPC strreplace($VCE::Server::SpecialVar[%class,%var],"%this",%target) @ ";");
			switch(%logic)
			{
				case 1:
					%newvalue = %oldvalue + %newvalue;
				case 2:
					%newvalue = %oldvalue - %newvalue;
				case 3:
					%newvalue = %oldvalue * %newvalue;
				case 4:
					%newvalue = %oldvalue / %newvalue;
				case 5:
					%newvalue = mFloor(%oldvalue);
				case 6:
					%newvalue = mCeil(%oldvalue);
				case 7:
					%newvalue = mPow(%oldvalue,%newvalue);
				case 8:
					if(%newvalue $= "")
						%root = 1 / 2;
					else
						%root = 1 / %newvalue;
					%newvalue = mPow(%oldvalue,%root);
				case 9:
					%newvalue = mPercent(%oldvalue,%newvalue);
				case 10:
					if(isInt(%oldvalue) && isInt(%newvalue))
						%newvalue = getRandom(%oldvalue,%newvalue);
					else
						%newvalue = getRandom();
				case 11:
					if(%newvalue $= "")
						%newvalue = "0";
					if(getWordCount(%newvalue) == 2)
						%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
					else
						%newvalue = getWord(%oldvalue,%newvalue + 1);
				case 12:
					%newvalue = strLwr(%oldvalue);
				case 13:
					%newvalue = strUpr(%oldvalue);
				case 14:
					%newvalue = strChr(%oldvalue,%newvalue);
				case 15:
					%newvalue = strLen(%oldvalue);
			}
			%f = "VCE_" @ %category @ "_" @ $VCE::Server::SpecialVarEdit[%class,%name];
			if(isFunction(%f))
			{
				call(%f,%target,%newvalue,$VCE::Server::SpecialVarEditArg1[%class,%name],$VCE::Server::SpecialVarEditArg2[%class,%name],$VCE::Server::SpecialVarEditArg3[%class,%name],$VCE::Server::SpecialVarEditArg4[%class,%name]);
				return;
			}
		}
		switch(%logic)
		{
			case 1:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue + %newvalue,%target);
			case 2:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue - %newvalue,%target);
			case 3:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue * %newvalue,%target);
			case 4:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue / %newvalue,%target);
			case 5:
				%brick.getGroup().vargroup.setVariable(%category,%name,mFloor(%oldvalue),%target);
			case 6:
				%brick.getGroup().vargroup.setVariable(%category,%name,mCeil(%oldvalue),%target);
			case 0:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 7:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPow(%oldvalue,%newvalue),%target);
			case 8:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue $= "" ? mPow(%oldvalue,1 / 2) : mPow(%oldvalue,1 / %newvalue),%target);
			case 9:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPercent(%oldvalue,%newvalue),%target);
			case 10:
				if(isInt(%oldvalue) && isInt(%newvalue))
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(%oldvalue,%newvalue),%target);
				else
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(),%target);
			case 11:
				if(%newvalue $= "")
					%newvalue = "0";
				if(getWordCount(%newvalue) == 2)
					%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
				else
					%newvalue = getWord(%oldvalue,%newvalue + 1);
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 12:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLwr(%oldvalue),%target);
			case 13:
				%brick.getGroup().vargroup.setVariable(%category,%name,strUpr(%oldvalue),%target);
			case 14:
				%brick.getGroup().vargroup.setVariable(%category,%name,strChr(%oldvalue,%newvalue),%target);
			case 15:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLen(%oldvalue),%target);
		}
	}
}
function GameConnection::VCE_modVariable(%client,%name,%logic,%value,%sourceClient)
{
	%brick = %sourceClient.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%category = "Client";
		%target = %client;
		%class = "GameConnection";
		%name = filterVariableString(%name,%brick,%client,%client.player);
		%oldvalue = %brick.getGroup().vargroup.getVariable(%category,%name,%target);
		%newvalue = filterVariableString(%value,%brick,%client,%client.player);
		if($VCE::Server::SpecialVarEdit[%class,%name] !$= "" && isObject(%target))
		{
			%oldvalue = eval("return" SPC strReplace($VCE::Server::SpecialVar[%class,%var],"%this",%target) @ ";");
			switch(%logic)
			{
				case 1:
					%newvalue = %oldvalue + %newvalue;
				case 2:
					%newvalue = %oldvalue - %newvalue;
				case 3:
					%newvalue = %oldvalue * %newvalue;
				case 4:
					%newvalue = %oldvalue / %newvalue;
				case 5:
					%newvalue = mFloor(%oldvalue);
				case 6:
					%newvalue = mCeil(%oldvalue);
				case 7:
					%newvalue = mPow(%oldvalue,%newvalue);
				case 8:
					if(%newvalue $= "")
						%root = 1 / 2;
					else
						%root = 1 / %newvalue;
					%newvalue = mPow(%oldvalue,%root);
				case 9:
					%newvalue = mPercent(%oldvalue,%newvalue);
				case 10:
					if(isInt(%oldvalue) && isInt(%newvalue))
						%newvalue = getRandom(%oldvalue,%newvalue);
					else
						%newvalue = getRandom();
				case 11:
					if(%newvalue $= "")
						%newvalue = "0";
					if(getWordCount(%newvalue) == 2)
						%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
					else
						%newvalue = getWord(%oldvalue,%newvalue + 1);
				case 12:
					%newvalue = strLwr(%oldvalue);
				case 13:
					%newvalue = strUpr(%oldvalue);
				case 14:
					%newvalue = strChr(%oldvalue,%newvalue);
				case 15:
					%newvalue = strLen(%oldvalue);
			}
			%f = "VCE_" @ %category @ "_" @ $VCE::Server::SpecialVarEdit[%class,%name];
			if(isFunction(%f))
			{
				call(%f,%target,%newvalue,$VCE::Server::SpecialVarEditArg1[%class,%name],$VCE::Server::SpecialVarEditArg2[%class,%name],$VCE::Server::SpecialVarEditArg3[%class,%name],$VCE::Server::SpecialVarEditArg4[%class,%name]);
				return;
			}
		}
		switch(%logic)
		{
			case 1:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue + %newvalue,%target);
			case 2:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue - %newvalue,%target);
			case 3:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue * %newvalue,%target);
			case 4:
				%brick.getGroup().vargroup.setVariable(%category,%name,%oldvalue / %newvalue,%target);
			case 5:
				%brick.getGroup().vargroup.setVariable(%category,%name,mFloor(%oldvalue),%target);
			case 6:
				%brick.getGroup().vargroup.setVariable(%category,%name,mCeil(%oldvalue),%target);
			case 0:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 7:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPow(%oldvalue,%newvalue),%target);
			case 8:
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue $= "" ? mPow(%oldvalue,1 / 2) : mPow(%oldvalue,1 / %newvalue),%target);
			case 9:
				%brick.getGroup().vargroup.setVariable(%category,%name,mPercent(%oldvalue,%newvalue),%target);
			case 10:
				if(isInt(%oldvalue) && isInt(%newvalue))
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(%oldvalue,%newvalue),%target);
				else
					%brick.getGroup().vargroup.setVariable(%category,%name,getRandom(),%target);
			case 11:
				if(%newvalue $= "")
					%newvalue = "0";
				if(getWordCount(%newvalue) == 2)
					%newvalue = getWords(%oldvalue,getWord(%newvalue,0) + 1,getWord(%newvalue,1) + 1);
				else
					%newvalue = getWord(%oldvalue,%newvalue + 1);
				%brick.getGroup().vargroup.setVariable(%category,%name,%newvalue,%target);
			case 12:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLwr(%oldvalue),%target);
			case 13:
				%brick.getGroup().vargroup.setVariable(%category,%name,strUpr(%oldvalue),%target);
			case 14:
				%brick.getGroup().vargroup.setVariable(%category,%name,strChr(%oldvalue,%newvalue),%target);
			case 15:
				%brick.getGroup().vargroup.setVariable(%category,%name,strLen(%oldvalue),%target);
		}
	}
}
function MinigameSO::VCE_ifVariable(%mini,%var,%logic,%valb,%subdata,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%var = filterVariableString(%var,%brick,%client,%client.player);
		%vala = %brick.getGroup().vargroup.getVariable("Minigame",%var,%mini);
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Minigame",%next,%mini);
								%last = %brick.getGroup().vargroup.getVariable("Minigame",%last,%mini);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function Vehicle::VCE_ifVariable(%vehicle,%var,%logic,%valb,%subdata,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%var = filterVariableString(%var,%brick,%client,%client.player,%vehicle);
		%vala = %brick.getGroup().vargroup.getVariable("Vehicle",%var,%vehicle);
		%valb = filterVariableString(%valb,%brick,%client,%client.player,%vehicle);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Vehicle",%next,%vehicle);
								%last = %brick.getGroup().vargroup.getVariable("Vehicle",%last,%vehicle);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function Player::VCE_ifVariable(%player,%var,%logic,%valb,%subdata,%client)
{
	%brick = %client.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%var = filterVariableString(%var,%brick,%client,%player);
		%vala = %brick.getGroup().vargroup.getVariable("Player",%var,%player);
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Player",%next,%player);
								%last = %brick.getGroup().vargroup.getVariable("Player",%last,%player);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function GameConnection::VCE_ifVariable(%client,%var,%logic,%valb,%subdata,%sourceClient)
{
	%brick = %sourceClient.processingBrick;
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%var = filterVariableString(%var,%brick,%client,%client.player);
		%vala = %brick.getGroup().vargroup.getVariable("Client",%var,%client);
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Client",%next,%Client);
								%last = %brick.getGroup().vargroup.getVariable("Client",%last,%Client);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function fxDtsBrick::VCE_ifVariable(%brick,%var,%logic,%valb,%subdata,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%var = filterVariableString(%var,%brick,%client,%client.player);
		%vala = %brick.getGroup().vargroup.getVariable("Brick",%var,%brick);
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%var) > 1 && (strPos(%var,"&") > -1 || strPos(%var,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%var);%w++)	
					{
						%word = getWord(%var,%w);
						%next = getWord(%var,%w+1);
						%last = getWord(%var,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else 
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function fxDtsBrick::VCE_ifValue(%brick,%vala,%logic,%valb,%subdata,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%vala = filterVariableString(%vala,%brick,%client,%client.player);
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%logic)
		{
			case 0:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((%last $= %valb) && (%next $= %valb));
								else	
									%test = ((%last $= %valb) && (%next $= %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last $= %valb) || (%next $= %valb));
								else	
									%test = ((%last $= %valb) || (%next $= %valb));
						}
					}
				}
				else
					%test = %vala $= %valb;
			case 1:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) && (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) && (%next !$= %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last !$= %valb) || (%next !$= %valb));
								else	
									%test = ((%last !$= %valb) || (%next !$= %valb));
						}
					}
				}
				else
					%test = %vala !$= %valb;
			case 2:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((%last > %valb) && (%next > %valb));
								else	
									%test = ((%last > %valb) && (%next > %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last > %valb) || (%next > %valb));
								else	
									%test = ((%last > %valb) || (%next > %valb));
						}
					}
				}
				else
					%test = %vala > %valb;
			case 3:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((%last < %valb) && (%next < %valb));
								else	
									%test = ((%last < %valb) && (%next < %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last < %valb) || (%next < %valb));
								else	
									%test = ((%last < %valb) || (%next < %valb));
						}
					}
				}
				else
					%test = %vala < %valb;
			case 4:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((%last >= %valb) && (%next >= %valb));
								else	
									%test = ((%last >= %valb) && (%next >= %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last >= %valb) || (%next >= %valb));
								else	
									%test = ((%last >= %valb) || (%next >= %valb));
						}
					}
				}
				else
					%test = %vala >= %valb;
			case 5:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								%next = %brick.getGroup().vargroup.getVariable("Brick",%next,%Brick);
								%last = %brick.getGroup().vargroup.getVariable("Brick",%last,%Brick);
								if(%test !$= "")
									%test = %test && ((%last <= %valb) && (%next <= %valb));
								else	
									%test = ((%last <= %valb) && (%next <= %valb));
							case "|":
								if(%test !$= "")
									%test = %test && ((%last <= %valb) || (%next <= %valb));
								else	
									%test = ((%last <= %valb) || (%next <= %valb));
						}
					}
				}
				else
					%test = %vala <= %valb;
			case 6:
				if(getWordCount(%vala) > 1 && (strPos(%vala,"&") > -1 || strPos(%vala,"|") > -1))
				{
					for(%w=0;%w<getWordCount(%vala);%w++)	
					{
						%word = getWord(%vala,%w);
						%next = getWord(%vala,%w+1);
						%last = getWord(%vala,%w-1);
						if(%next $= "" || %last $= "")
							continue;
						switch$(%word)
						{
							case "&":
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) && (strPos(%next,%valb) > -1));
							case "|":
								if(%test !$= "")
									%test = %test && ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
								else	
									%test = ((strPos(%last,%valb) > -1) || (strPos(%next,%valb) > -1));
						}
					}
				}
				else
					%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else 
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function fxDtsBrick::VCE_retroCheck(%brick,%vala,%logic,%valb,%subdata,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime()-%client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		//ifPlayerName 0 ifPlayerID 1 ifAdmin 2 ifPlayerEnergy 3 ifPlayerDamage 4 ifPlayerScore 5 ifLastPlayerMsg 6 ifBrickName 7 ifRandomDice 8
		%valb = filterVariableString(%valb,%brick,%client,%client.player);
		switch(%vala)
		{
			case 0:
				%vala = %client.getPlayerName();
			case 1:
				%vala = %client.BL_ID;
			case 2:
				%vala = %client.isAdmin;
				%valb = %client.isAdmin == 1 ? 1 : -1;
			case 3:
				%vala = 0;
				if(isObject(%client.player))
					%vala = %client.player.getEnergyLevel();
			case 4:
				%vala = 0;
				if(isObject(%client.player))
					%vala = %client.player.getDamageLevel();
			case 5:
				%vala = %client.score;
			case 6:
				%vala = %client.lastMessage;
			case 7:
				if(strLen(%brick.getName()) >= 1)
					%vala = getSubStr(%brick.getName(),1,strLen(%brick.getName()) - 1);
			case 8:
				%vala = getRandom(1,6);
		}
		switch(%logic)
		{
			case 0:
				%test = %vala $= %valb;
			case 1:
				%test = %vala !$= %valb;
			case 2:
				%test = %vala > %valb;
			case 3:
				%test = %vala < %valb;
			case 4:
				%test = %vala >= %valb;
			case 5:
				%test = %vala <= %valb;
			case 6:
				%test = striPos(%vala,%valb) > -1;
		}
		if(getWordCount(%subdata) != 2)
		{
			if(%test)
				%brick.onVariableTrue(%client);
			else 
				%brick.onVariableFalse(%client);
			return;
		}
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				case "~=":
					%sublike = %subend;
				case "!=":
					%subignore = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
			{
				if(%brick.eventInput[%i] $= "onVariableTrue" || %brick.eventInput[%i] $= "onVariableFalse")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		if(%test)
			%brick.onVariableTrue(%client);
		else
			%brick.onVariableFalse(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function fxDtsBrick::VCE_stateFunction(%brick,%name,%subdata,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		if(getWordCount(%subdata) != 2)
			return;
		%name = filterVariableString(%name,%brick,%client,%client.player);
		%substart = getWord(%subdata,0);
		%subend = getWord(%subdata,1);
		if(!isInt(%substart) && isInt(%subend))
		{
			switch$(%substart)
			{
				case "<":
					%substart = 0;
					%subend = %subend - 1;
				case "<=":
					%substart = 0;
				case ">":
					%substart = %subend + 1;
					%subend = %brick.numEvents;
				case ">=":
					%substart = %subend;
					%subend = %brick.numEvents;
				case "==":
					%substart = %subend;
				default:
					%substart = 0;
					%subend = %brick.numEvents;
			}
		}
		%substart = %substart < -1 ? 0 : %substart;
		%subend = %subend > %brick.numEvents ? %brick.numEvents : %subend;
		%brick.vceFunction[%name] = %substart SPC %subend;
	}
}
function fxDtsBrick::VCE_callFunction(%brick,%name,%args,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		%name = filterVariableString(%name,%brick,%client,%client.player);
		if(getWordCount(%brick.vceFunction[%name]) < 2)
		{
			%brick.onVariableFunction(%client);
			return;
		}
		%args = filterVariableString(%args,%brick,%client,%client.player);
		%args = strReplace(%args,"|","\t");
		%fc = getFieldCount(%args);
		for(%i=0;%i<%fc;%i++)
		{
			%arg[%i] = getField(%args,%i);
			%brick.getGroup().vargroup.setVariable("Brick","arg" @ %i,%arg[%i],%brick);
		}
		%brick.getGroup().vargroup.setVariable("Brick","argcount",getFieldCount(%args),%brick);
		%substart = getWord(%brick.vceFunction[%name],0);
		%subend = getWord(%brick.vceFunction[%name],1);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(!(%i >= %substart && %i <= %subend))
			{
				if(%brick.eventInput[%i] $= "onVariableFunction")
				{
					%oldEnabled[%i] = %brick.eventEnabled[%i];
					%brick.eventEnabled[%i] = 0;
				}
			}
		}
		%brick.onVariableFunction(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%oldEnabled[%i] !$= "")
				%brick.eventEnabled[%i] = %oldEnabled[%i];
		}
	}
}
function fxDtsBrick::VCE_relayCallFunction(%brick,%direction,%name,%args,%client)
{
	%start = posFromTransform(%brick.getTransform());
	%db = %brick.getDatablock();
	%angle = %brick.getAngleID();
	if(%angle == 1 || %angle == 3)
	{
		%addX = %db.brickSizeY * 0.55;
		%addY = %db.brickSizeX * 0.55;
	}
	else
	{
		%addX = %db.brickSizeX * 0.55;
		%addY = %db.brickSizeY * 0.55;
	}
	%addZ = %db.brickSizeZ * 0.22;
	//Up Down North East South West
	switch(%direction)
	{
		case 0:
			%add = 0 SPC 0 SPC %addZ;
		case 1:
			%add = 0 SPC 0 SPC -%addZ;
		case 2:
			%add = 0 SPC %addY SPC 0;
		case 3:
			%add = %addX SPC 0 SPC 0;
		case 4:
			%add = 0 SPC -%addY SPC 0;
		case 5:
			%add = -%addX SPC 0 SPC 0;
	}
	%end = vectorAdd(%start,%add);
	%ray = containerRaycast(%start,%end,$TypeMasks::FxBrickAlwaysObjectType,%brick);
	%col = firstWord(%ray);
	if(isObject(%col))
	{
		if(getTrustLevel(%col,%brick) < 2)
			return;
		%col.VCE_callFunction(%name,%args,%client);
	}
}
function fxDtsBrick::VCE_castRelay(%brick,%direction,%range,%client)
{
	%start = posFromTransform(%brick.getTransform());
	//Up Down North East South West
	switch(%direction)
	{
		case 0:
			%add = 0 SPC 0 SPC %range;
		case 1:
			%add = 0 SPC 0 SPC -%range;
		case 2:
			%add = 0 SPC %range SPC 0;
		case 3:
			%add = %range SPC 0 SPC 0;
		case 4:
			%add = 0 SPC -%range SPC 0;
		case 5:
			%add = -%range SPC 0 SPC 0;
	}
	%end = vectorAdd(%start,%add);
	%ray = containerRaycast(%start,%end,$TypeMasks::FxBrickAlwaysObjectType,%brick);
	%col = firstWord(%ray);
	if(isObject(%col))
	{
		if(getTrustLevel(%col,%brick) < 2)
			return;
		%col.processInputEvent("onRelay",%client);
	}
}
function fxDtsBrick::VCE_saveVariable(%brick,%type,%vars,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		switch(%type)
		{
			case 0:
				%category = "Client";
				%target = %client;
			case 1:
				if(isObject(%client.player))
				{
					%category = "Player";
					%target = %client.player;
				}
				else
					return;
		}
		%vargroup = %brick.getGroup().vargroup;
		%vars = strReplace(%vars,", ","\t");
		%vars = strReplace(%vars,",","\t");
		%count = getFieldCount(%vars);
		for(%i=0;%i<%count;%i++)
			%vargroup.saveVariable(%category,getField(%vars,%i),%target);
	}
}
function fxDtsBrick::VCE_loadVariable(%brick,%type,%vars,%client)
{
	if(%client.blockVCE[getBrickGroupFromObject(%brick).bl_id] || ((isObject(%client.player) && getSimTime() - %client.player.spawnTime < $VCE::Server::ImmuneTime)))
		return;
	if(isObject(%brick.getGroup().vargroup))
	{
		switch(%type)
		{
			case 0:
				%category = "Client";
				%target = %client;
			case 1:
				if(isObject(%client.player))
				{
					%category = "Player";
					%target = %client.player;
				}
				else
					return;
		}
		%vargroup = %brick.getGroup().vargroup;
		%vars = strReplace(%vars,", ","\t");
		%vars = strReplace(%vars,",","\t");
		%count = getFieldCount(%vars);
		for(%i=0;%i<%count;%i++)
			%vargroup.loadVariable(%category,getField(%vars,%i),%target);
	}
}
