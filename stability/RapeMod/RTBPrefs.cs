if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
   if(!$RTB::RTBR_ServerControl_Hook) {
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
		RTB_registerPref("Enabled", "Rape Mod", "$Pref::RapeMod::Enabled", "bool", "Script_Rape", 3);
		RTB_registerPref("Mini-Game Only", "Rape Mod", "$Pref::RapeMod::MiniGameOnly", "bool", "Script_Rape", 3);
		RTB_registerPref("Escape / Penetrate Key", "Rape Mod", "$Pref::RapeMod::Key", "list Activate 0 Jump 2", "Script_Rape", 3);
		
}
else 
{
	$Pref::RapeMod::Enabled = 1;
	$Pref::RapeMod::MiniGameOnly = 1;
	$Pref::RapeMod::Key = 2;
}

if($Pref::RapeMod::Enabled != 0 && $Pref::RapeMod::Enabled != 1)
{
	$Pref::RapeMod::Enabled = 1;
}

if($Pref::RapeMod::MiniGameOnly != 0 && $Pref::RapeMod::MiniGameOnly != 1)
{
	$Pref::RapeMod::MiniGameOnly = 1;
}

if($Pref::RapeMod::Key != 0 && $Pref::RapeMod::Key != 2)
{
	$Pref::RapeMod::Key = 2;
}
