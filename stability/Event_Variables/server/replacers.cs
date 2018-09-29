//---
//	@package VCE
//	@title Replacers
//	@author Clockturn/www.clockler.com
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 4:44 PM 14/04/2011
//---
function filterVariableString(%string,%brick,%client,%player,%vehicle)
{
	if(!isObject(%vehicle))
		%vehicle = %brick.vehicle;
	%offset = 0;
	%its = 0;
	while(1)
	{
		%open = strPos(%string,"<var:",%offset);
		if(%open == -1)
		{
			%open = strpos(%string,"<varlink:",%offset);
			if(%open != -1)
			{
				%close = strpos(%string,">",%open);
				%varstr = getsubstr(%string,%open+1,(%close-%open)-1);
				%varlen = strlen(%varstr)+2;
				%varstr = getsubstr(%varstr,8,strlen(%varstr)-8);
				%colon = strpos(%varstr,":");
				%mode = getsubstr(%varstr,0,%colon);
				%var = getsubstr(%varstr,%colon+1,strlen(%varstr));
				%mode = strReplace(%mode,"_","\t");
				if(%mode !$= "" && %var !$= "")
				{
					if(%brick.varLink[%client] $= "")
						%brick.varLink[%client] = getField(%mode,1) @ "=" @ %var;
					else
						%brick.varLink[%client] = %brick.varLink[%client] TAB getField(%mode,1) @ "=" @ %var;
					%client.varLink[%brick] = true;
					%string = getsubstr(%string,0,%open) @ "<a:varlink-" @ %brick @ "-" @ getFieldCount(%brick.varLink[%client])-1 @ ">" @ getField(%mode,0) @ "</a>" @ getsubstr(%string,%close+1,strlen(%string));
					%offset += strLen(getField(%mode,0));
					%its++;
					if(%its > 15)
						break;
					continue;
				}
			}
			break;
		}
		%close = strpos(%string,">",%open);
		%varstr = getsubstr(%string,%open + 1,(%close-%open) - 1);
		%varlen = strlen(%varstr)+2;
		%varstr = getsubstr(%varstr,4,strLen(%varstr) - 4);
		%colon = strpos(%varstr,":");
		%mode = getsubstr(%varstr,0,%colon);
		%var = getsubstr(%varstr,%colon + 1,strLen(%varstr));
		if(strLen(%mode) > 3)
		{
			if(getSubStr(%mode,0,3) $= "nb_")
			{
				//Only let them get the value of the first brick with the name, it makes no logical sense to try and get the value of all of them
				%nb = nameToID("_" @ getSubStr(%mode,3,strLen(%mode) - 3));
				if(isObject(%nb) && isObject(getBrickGroupFromObject(%nb).varGroup))
				{
					if($VCE::Server::SpecialVar["fxDTSbrick",%var] $= "")
						%variable = %brick.getGroup().vargroup.getVariable("Brick",%var,%nb);
					else 
						%variable = eval("return" SPC strreplace($VCE::Server::SpecialVar["fxDTSbrick",%var],"%this",%nb) @ ";");
					%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
					%offset += strlen(%variable);
					%its++;
					if(%its > 15)
						break;
					continue;
				}
			}
		}
		switch$(%mode)
		{
			case "client":
				if($VCE::Server::SpecialVar["GameConnection",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Client",%var,%client);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["GameConnection",%var],"%this",%client) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "player":
				if($VCE::Server::SpecialVar["Player",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Player",%var,%player);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["Player",%var],"%this",%player) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "brick":
				if($VCE::Server::SpecialVar["fxDTSbrick",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Brick",%var,%brick);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["fxDTSbrick",%var],"%this",%brick) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "vehicle":
				if($VCE::Server::SpecialVar["Vehicle",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Vehicle",%var,%vehicle);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["Vehicle",%var],"%this",%vehicle) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "minigame":
				if($VCE::Server::SpecialVar["MinigameSO",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Minigame",%var,getMinigameFromObject(%brick));
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["MinigameSO",%var],"%this",getMinigameFromObject(%brick)) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "global":
				if($VCE::Server::SpecialVar["GLOBAL",%var] !$= "")
				{
					%variable = eval("return" SPC $VCE::Server::SpecialVar["GLOBAL",%var] @ ";");
					%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
				}
			case "cl":
				if($VCE::Server::SpecialVar["GameConnection",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Client",%var,%client);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["GameConnection",%var],"%this",%client) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "pl":
				if($VCE::Server::SpecialVar["Player",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Player",%var,%player);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["Player",%var],"%this",%player) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "br":
				if($VCE::Server::SpecialVar["fxDTSbrick",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Brick",%var,%brick);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["fxDTSbrick",%var],"%this",%brick) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "ve":
				if($VCE::Server::SpecialVar["Vehicle",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Vehicle",%var,%vehicle);
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["Vehicle",%var],"%this",%vehicle) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "mg":
				if($VCE::Server::SpecialVar["MinigameSO",%var] $= "")
					%variable = %brick.getGroup().vargroup.getVariable("Minigame",%var,getMinigameFromObject(%brick));
				else
					%variable = eval("return" SPC strReplace($VCE::Server::SpecialVar["MinigameSO",%var],"%this",getMinigameFromObject(%brick)) @ ";");
				%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
			case "gl":
				if($VCE::Server::SpecialVar["GLOBAL",%var] !$= "")
				{
					%variable = eval("return" SPC $VCE::Server::SpecialVar["GLOBAL",%var] @ ";");
					%string = getsubstr(%string,0,%open) @ %variable @ getsubstr(%string,%close+1,strlen(%string));
				}
		}
		%offset += strLen(%variable);
		%its++;
		if(%its > 15)
			break;
	}
	return %string;
}
function registerSpecialVar(%classname,%name,%script,%editscript,%arg1,%arg2,%arg3,%arg4)
{
	if($VCE::Server::SpecialVar[%classname,%name] !$= "")
		echo("registerSpecialVar() - Variable" SPC %name SPC "already exists on" SPC %classname @ ". Overwriting...");
	$VCE::Server::SpecialVar[%classname,%name] = %script;
	$VCE::Server::SpecialVarEdit[%classname,%name] = %editscript;
	$VCE::Server::SpecialVarEditArg1[%classname,%name] = %arg1;
	$VCE::Server::SpecialVarEditArg2[%classname,%name] = %arg2;
	$VCE::Server::SpecialVarEditArg3[%classname,%name] = %arg3;
	$VCE::Server::SpecialVarEditArg4[%classname,%name] = %arg4;
}
function isSpecialVar(%classname,%name)
{
	return $VCE::Server::SpecialVar[%classname,%name] !$= "";
}
function unregisterSpecialVar(%classname,%name)
{
	if($VCE::Server::SpecialVar[%classname,%name] $= "")
		return echo("unregisterSpecialVar() - Variable" SPC %name SPC " does not exist on" SPC %classname @ ". Can not un-register.");
	$VCE::Server::SpecialVar[%classname,%name] = "";
	$VCE::Server::SpecialVarEdit[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg1[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg2[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg3[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg4[%classname,%name] = "";
}