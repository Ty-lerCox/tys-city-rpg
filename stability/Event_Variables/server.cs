//------
//Variable/Conditional Events
//Creator: Zack0Wack0
//Helpers: Clockturn, Truce, Boom, Lilboarder32 and Chrono
//------
//Major: 6
//Minor: 1
//Patch: 2
//Total: 6.12
$VCE::Server::Version = "6.12";
//---
// Support
//---
//eeee
//---
// Main
//---
package VCE_Other
{
	function GameConnection::autoAdminCheck(%client)
	{
		%v = Parent::autoAdminCheck(%client);
 	
		schedule(500,0,"commandToClient",%client,'VCE_Handshake',$VCE::Server::Version);
		
		return %v;
	}
};
activatePackage(VCE_Other);
function VCE_initServer()
{
	activatePackage(VCE_Main);
	//Register all events and special vars
	registerOutputEvent(fxDtsBrick,"VCE_modVariable","list Brick 0 Player 1 Client 2 Minigame 3 Vehicle 4\tstring 32 100\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Floor 5 Ceil 6 Power 7 Radical 8 Percent 9 Random 10 Words 11 Lowercase 12 Uppercase 13 Character 14 Length 15\tstring 32 255",1);
	registerOutputEvent(fxDtsBrick,"VCE_ifValue","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_retroCheck","list ifPlayerName 0 ifPlayerID 1 ifAdmin 2 ifPlayerEnergy 3 ifPlayerDamage 4 ifPlayerScore 5 ifLastPlayerMsg 6 ifBrickName 7 ifRandomDice 8\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 50 50\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_stateFunction","string 32 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_callFunction","string 32 100\tstring 64 100",1);
	registerOutputEvent(fxDtsBrick,"VCE_relayCallFunction","list Up 0 Down 1 North 2 East 3 South 4 West 5\tstring 32 100\tstring 64 100",1);
	if($Pref::Server::ExperimentalVCE)
	{
		registerOutputEvent(fxDtsBrick,"VCE_castRelay","list Up 0 Down 1 North 2 East 3 South 4 West 5\tint 1 96 2",1);
		registerSpecialVar(Player,"pos","posFromTransform(%this.getTransform())","setPosition");
		registerSpecialVar(Vehicle,"pos","posFromTransform(%this.getTransform())","setPosition");
	}
 	registerOutputEvent(fxDtsBrick,"VCE_saveVariable","list Client 0 Player 1\tstring 200 255",1);
 	registerOutputEvent(fxDtsBrick,"VCE_loadVariable","list Client 0 Player 1\tstring 200 255",1);
	registerInputEvent(fxDtsBrick,"onVariableTrue","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableFalse","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableFunction","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableUpdate","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerOutputEvent(Player,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 50 50\tstring 8 30",1);
	registerOutputEvent(Player,"VCE_modVariable","string 32 100\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Floor 5 Ceil 6 Power 7 Radical 8 Percent 9 Random 10 Words 11 Lowercase 12 Uppercase 13 Character 14 Length 15\tstring 32 255",1);
	registerOutputEvent(GameConnection,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 50 50\tstring 8 30",1);
	registerOutputEvent(GameConnection,"VCE_modVariable","string 32 100\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Floor 5 Ceil 6 Power 7 Radical 8 Percent 9 Random 10 Words 11 Lowercase 12 Uppercase 13 Character 14 Length 15\tstring 32 255",1);
	registerOutputEvent(Minigame,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 50 50\tstring 8 30",1);
	registerOutputEvent(Minigame,"VCE_modVariable","string 32 100\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Floor 5 Ceil 6 Power 7 Radical 8 Percent 9 Random 10 Words 11 Lowercase 12 Uppercase 13 Character 14 Length 15\tstring 32 255",1);
	registerOutputEvent(Vehicle,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 50 50\tstring 8 30",1);
	registerOutputEvent(Vehicle,"VCE_modVariable","string 32 100\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Floor 5 Ceil 6 Power 7 Radical 8 Percent 9 Random 10 Words 11 Lowercase 12 Uppercase 13 Character 14 Length 15\tstring 32 255",1);
	if(!$VCE::Server)
	{
		registerSpecialVar(GameConnection,"brickcount","%this.brickGroup.getCount()");
		registerSpecialVar(GameConnection,"bl_id","%this.bl_id");
		registerSpecialVar(GameConnection,"name","%this.getPlayerName()");
		registerSpecialVar(GameConnection,"kdratio","(isInt(%client.vceKills / %client.vceDeaths)) ? (%client.vceKills / %client.vceDeaths) : 0");
		registerSpecialVar(GameConnection,"clanPrefix","%this.clanPrefix","setClanPrefix");
		registerSpecialVar(GameConnection,"clanSuffix","%this.clanSuffix","setClanSuffix");
		registerSpecialVar(GameConnection,"score","%this.score","setScore");
		registerSpecialVar(GameConnection,"hat","$hat[%this.hat]");
		registerSpecialVar(GameConnection,"accent","$accent[%this.accent]");
		registerSpecialVar(GameConnection,"pack","$pack[%this.pack]");
		registerSpecialVar(GameConnection,"secondPack","$secondpack[%this.secondpack]");
		registerSpecialVar(GameConnection,"hip","$hip[%this.hip]");
		registerSpecialVar(GameConnection,"rleg","$rleg[%this.rleg]");
		registerSpecialVar(GameConnection,"rarm","$rarm[%this.rarm]");
		registerSpecialVar(GameConnection,"lleg","$lleg[%this.lleg]");
		registerSpecialVar(GameConnection,"larm","$larm[%this.larm]");
		registerSpecialVar(GameConnection,"chest","$chest[%this.chest]");
		registerSpecialVar(GameConnection,"decal","%this.decalName","setDecalName");
		registerSpecialVar(GameConnection,"face","%this.faceName","setFaceName");
		registerSpecialVar(GameConnection,"lastmsg","%this.lastMessage");
		registerSpecialVar(GameConnection,"lastteammsg","%this.lastTeamMessage");
		registerSpecialVar(GameConnection,"isAdmin","%this.isAdmin");
		registerSpecialVar(GameConnection,"isSuperAdmin","%this.isSuperAdmin");
		registerSpecialVar(GameConnection,"isSuperAdmin","(%this.bl_id == getNumKeyID() || %this.isLocalConnection())");
		registerSpecialVar(Player,"damage","%this.getDamageLevel()","setDamage");
		registerSpecialVar(Player,"energy","%this.getEnergyLevel()","setEnergy");
		registerSpecialVar(Player,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()","setHealth");
		registerSpecialVar(Player,"maxhealth","%this.getDatablock().maxDamage");
		registerSpecialVar(Player,"velx","getWord(%this.getVelocity(),0)");
		registerSpecialVar(Player,"vely","getWord(%this.getVelocity(),1)");
		registerSpecialVar(Player,"velz","getWord(%this.getVelocity(),2)");
		registerSpecialVar(Player,"vel","getWords(%this.getVelocity(),0,2)","setVelocity");
		registerSpecialVar(Player,"speed","vectorDist(%this.getVelocity(),\"0 0 0\")");
		registerSpecialVar(Player,"crouching","%this.crouch");
		registerSpecialVar(Player,"jumping","%this.jump");
		registerSpecialVar(Player,"jetting","%this.jet");
		registerSpecialVar(Player,"firing","%this.fire");
		registerSpecialVar(Player,"sitting","%this.vcesitting");
		registerSpecialVar(Player,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(Player,"currentitem","%this.tool[%this.currTool].uiName","setCurrentItem");
		registerSpecialVar(Player,"item1","%this.tool[0].uiName","setItem",0);
		registerSpecialVar(Player,"item2","%this.tool[1].uiName","setItem",1);
		registerSpecialVar(Player,"item3","%this.tool[2].uiName","setItem",2);
		registerSpecialVar(Player,"item4","%this.tool[3].uiName","setItem",3);
		registerSpecialVar(Player,"item5","%this.tool[4].uiName","setItem",4);
		registerSpecialVar(Player,"posx","getWord(%this.getTransform(),0)");
		registerSpecialVar(Player,"posy","getWord(%this.getTransform(),1)");
		registerSpecialVar(Player,"posz","getWord(%this.getTransform(),2)");
		registerSpecialVar(fxDTSbrick,"ownername","%this.getGroup().name");
		registerSpecialVar(fxDTSbrick,"ownerbl_id","%this.getGroup().bl_id");
		registerSpecialVar(fxDTSbrick,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(fxDTSbrick,"colorid","%this.getColorID()","setColor");
		registerSpecialVar(fxDTSbrick,"printcount","%this.printcount","setPrintCount");
		registerSpecialVar(fxDTSbrick,"printname","%this.getPrintName()","setPrintName");
		registerSpecialVar(fxDTSbrick,"name","%this.getBrickName()","setBrickName");
		registerSpecialVar(fxDTSbrick,"colorfxid","%this.getColorFXID()","setColorFX");
		registerSpecialVar(fxDTSbrick,"printid","%this.printid","setPrint");
		registerSpecialVar(fxDTSbrick,"shapefxid","%this.getShapeFXID()","setShapeFX");
		registerSpecialVar(fxDTSbrick,"posx","getWord(%this.getTransform(),0)");
		registerSpecialVar(fxDTSbrick,"posy","getWord(%this.getTransform(),1)");
		registerSpecialVar(fxDTSbrick,"posz","getWord(%this.getTransform(),2)");
		registerSpecialVar(Vehicle,"drivername","%this.getDriverName()");
		registerSpecialVar(Vehicle,"driverbl_id","%this.getDriverBL_ID()");
		registerSpecialVar(Vehicle,"damage","%this.getDamageLevel()","setDamage");
		registerSpecialVar(Vehicle,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()","setHealth");
		registerSpecialVar(Vehicle,"maxhealth","%this.getDatablock().maxDamage");
		registerSpecialVar(Vehicle,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(Vehicle,"velx","getWord(%this.getVelocity(),0)");
		registerSpecialVar(Vehicle,"vely","getWord(%this.getVelocity(),1)");
		registerSpecialVar(Vehicle,"velz","getWord(%this.getVelocity(),2)");
		registerSpecialVar(Vehicle,"vel","%this.getVelocity()","setVelocity");
		registerSpecialVar(Vehicle,"speed","vectorDist(%this.getVelocity(),\"0 0 0\")");
		registerSpecialVar(Vehicle,"posx","getWord(%this.getTransform(),0)");
		registerSpecialVar(Vehicle,"posy","getWord(%this.getTransform(),1)");
		registerSpecialVar(Vehicle,"posz","getWord(%this.getTransform(),2)");
		registerSpecialVar(MinigameSO,"lastmsg","%this.lastMessage");
		registerSpecialVar(MinigameSO,"membercount","%this.numMembers");
		registerSpecialVar("GLOBAL","date","getDate()");
		registerSpecialVar("GLOBAL","lastmsg","$VCE::Other::LastMessage");
		registerSpecialVar("GLOBAL","brickcount","$Server::BrickCount");
		registerSpecialVar("GLOBAL","time","getTime()");
		registerSpecialVar("GLOBAL","hour","getField(strReplace(getTime(),\":\",\"\t\"),0)");
		registerSpecialVar("GLOBAL","minute","getField(strReplace(getTime(),\":\",\"\t\"),1)");
		registerSpecialVar("GLOBAL","second","getField(strReplace(getTime(),\":\",\"\t\"),2)");
		registerSpecialVar("GLOBAL","simtime","getSimTime()");
		registerSpecialVar("GLOBAL","macintosh","isMacintosh()");
		registerSpecialVar("GLOBAL","windows","isWindows()");
		registerSpecialVar("GLOBAL","servername","$Pref::Server::Name");
		registerSpecialVar("GLOBAL","port","$Pref::Server::Port");
		registerSpecialVar("GLOBAL","maxplayercount","$Pref::Server::MaxPlayers");
		registerSpecialVar("GLOBAL","playercount","$server::playercount");
		registerSpecialVar("GLOBAL","pi","3.14159265");
	}
	//Set up prefs and various stuff
	$VCE::Server = 1;
	$VCE::Server::SavePath = "config/server/VCE/saves.txt";
	VCE_updateSaveFile();
}
//---
// Package
//---
exec("./server/package.cs");
//---
//Misc
//---
exec("./server/misc.cs");
//---
// Networking
//---
exec("./server/networking.cs");
//---
// Replacers
//---
exec("./server/replacers.cs");
//---
// Groups
//---
// Introduced in v5, variables are now stored in brick groups.
// Basically if you are working with variables, all variables that you modify will only exist in your brickgroup, so now one else can modify them.
//---
exec("./server/groups.cs");
//---
// Events
//---
//exec("./server/circuits.cs"); discontinued
exec("./server/inputs.cs");
exec("./server/outputs.cs");
//-
$VCE::InitSchedule = schedule(2000,0,"VCE_initServer");
