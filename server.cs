// ============================================================ 
// Project				:	CityRPG
// Author				:	Iban & Jookia & Ty(ID997)
// Description			:	Code used by all walks of life.
// ============================================================
// Table of Contents
// 1. Idiot Test
// 2. LAN Test
// 3. File Execution
// 4. Required Add-on loading
// ============================================================

// ============================================================
// Section 1 : Idiot Test
// ============================================================

if(0 == 1)
{
	//echo("=========================");
	//echo("CityRPG is a splinter project of CityRP after difficulties between Jookia and myself.");
	//echo("The intention of this mod was to provide players with the ability to see a block city from block eyes.");
	//echo("Of course CityRPG fails on this promise and is merely a really, really fucked up GameMode for blockland.");
	//echo("Although not the best handy-work of code to ever exist this is pretty much my biggest feat that has held together.");
	//echo("I enjoyed working with it but can now honestly say I am done.");
	//echo("This mod is provided as-is without warranty of functionality.");
	//echo("By circumventing this message in server.cs you declare that you are taking this mod for your own self interests and cant blame Iban for how bad it is.");
	//echo("=========================");
	return;
}

// ============================================================
// Section 2 : LAN Test
// ============================================================

if($server::lan)
{
	error("CityRPG cannot support LAN servers because of conflicts within the Saving system.\nThis will most likely never be added due to the severity of this incompatibility/nTerminating...");
	return;
}

// ============================================================
// Section 3 : File Execution
// ============================================================

//Core Files
exec("./prefs.cs");
exec("./bricks.cs");
exec("./scriptobject.cs");
exec("./common.cs");
exec("./package.cs");
exec("./saving.cs");
exec("./help.cs");
exec("./weather.cs");

// Tools
exec("./items/tools/axe.cs");
exec("./items/tools/shovel.cs");
exec("./items/tools/pickaxe.cs");
exec("./items/tools/knife.cs");

// Family Mod
exec("./family/familymod.cs");

// Weapons
exec("./items/weapons/taser.cs");
exec("./items/weapons/baton.cs");
exec("./items/weapons/playerbaton.cs");
exec("./items/weapons/brickbaton.cs");
exec("./items/weapons/adminLot.cs");
exec("./items/weapons/limitedbaton.cs");
exec("./items/weapons/lockpick.cs");

// Modules
exec("./modules/cash.cs");
exec("./modules/spacecasts.cs");
exec("./modules/idiots.cs");
//exec("./modules/voteBan.cs");
exec("./modules/voteImpeach.cs");
exec("./modules/setLayout.cs");
exec("./modules/waterDamage.cs");
exec("./modules/lottery.cs");
exec("./modules/rep.cs");
exec("./modules/lumberintobricks.cs");
//exec("./modules/trade.cs");
//exec("./modules/business.cs");
exec("./modules/hire.cs");
exec("./modules/gangs.cs");
exec("./modules/insurance.cs");
exec("./modules/gangChat.cs");
exec("./modules/mayor.cs");
exec("./modules/RandomColor.cs");
//exec("./modules/Like.cs");
exec("./modules/citizenship.cs");
//exec("./modules/advertise.cs");
exec("./modules/purge.cs");
exec("./modules/robbery.cs");
exec("./modules/gangWarz.cs");
//exec("./modules/coordinatesystem.cs");
//exec("./modules/Witness.cs");
exec("./modules/warrent.cs");
exec("./modules/RemoveKills.cs");

//stabilityAddons
exec("./stability/duplicator.cs");
exec("./stability/report.cs");
exec("./stability/CellPhones/server.cs");
//exec("./stability/RapeMod/server.cs");
//exec("./stability/throwmod.cs");
exec("./stability/moderator.cs");
//exec("./stability/FishingRod/server.cs");
exec("./stability/restrictevents.cs");
exec("./stability/donator.cs");
exec("./stability/sponsor.cs");
exec("./stability/HealthBar/server.cs");
//exec("./stability/VoteKick.cs");
exec("./stability/PetMod/server.cs");
exec("./stability/AdminChat/server.cs");
exec("./stability/ModChat/server.cs");

//playerTypes
exec("./playerTypes/Player_Fast.cs");
exec("./playerTypes/Multislot/server.cs");

//globalSaving
exec("./globalSaving/businessSaving.cs");
exec("./globalSaving/gangsSaving.cs");
exec("./globalSaving/mayorSaving.cs");


CityRPG_BootUp();

// ============================================================
// Section 2 : Required Add-on loading
// ============================================================

//This section has only one point.  Because the ForceRequiredAddon doesn't 
//fix the problem, this is only here to tell the server host that he needs 
//to get certain Add-ons.

//Item_Key

//%error = ForceRequiredAddOn("Item_Key");

//if(%error == $Error::AddOn_NotFound)
//{
   //we don't have the item, so we're screwed
//   error("ERROR: Gamemode_TysCityRPG - required add-on Item_Key not found");
//}


//Weapon_Guns_Akimbo (The Guns Akimbo forces the Gun to load on its own.
//Therefore, we can load the Guns Akimbo only.)

//%error = ForceRequiredAddOn("Weapon_Guns_Akimbo");

//if(%error == $Error::AddOn_NotFound)
//{
   //we don't have the item, so we're screwed
//   error("ERROR: Gamemode_TysCityRPG - required add-on Weapon_Guns_Akimbo not found");
//}


//Weapon_Shotgun

//%error = ForceRequiredAddOn("Weapon_Shotgun");

//if(%error == $Error::AddOn_NotFound)
//{
   //we don't have the item, so we're screwed
//   error("ERROR: Gamemode_TysCityRPG - required add-on Weapon_Shotgun not found");
//}


//Weapon_Rocket_Launcher

//%error = ForceRequiredAddOn("Weapon_Rocket_Launcher");

//if(%error == $Error::AddOn_NotFound)
//{
   //we don't have the item, so we're screwed
//   error("ERROR: Gamemode_TysCityRPG - required add-on Weapon_Rocket_Launcher not found");
//}


//Weapon_Sniper_Rifle

//%error = ForceRequiredAddOn("Weapon_Sniper_Rifle");

//if(%error == $Error::AddOn_NotFound)
//{
   //we don't have the item, so we're screwed
//   error("ERROR: Gamemode_TysCityRPG - required add-on Weapon_Sniper_Rifle not found");
//}
//forceRequiredAddon("knifeItem");
//forceRequiredAddon("assaultRifleItem");
//forceRequiredAddon("blackKnifeItem");
//forceRequiredAddon("dualG36CsItem");
//forceRequiredAddon("as50Item");
//forceRequiredAddon("piAK47Item");
//forceRequiredAddon("p90Item");