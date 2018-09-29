// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Weather
// ============================================================
// Table of Contents
// 1. WeatherBoxes and Misc. Forced Downloads
// 2. Functions
// ============================================================

// ============================================================
// Section 1 : WeatherBoxes and Misc. Forced Downloads
// ============================================================

// Rain
if(!isObject(CityRPG_Rain))
{
	datablock PrecipitationData(CityRPG_Rain)
	{
	   //soundProfile = "RainSound";
	   dropTexture = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Rain/droplet";
	   splashTexture = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Rain/splash";
	   dropSize = 0.50;
	   splashSize = 0.2;
	   useTrueBillboards = false;
	   splashMS = 250;
	};
}

if(!isObject(CityRPG_Rain_Texture))
{
	datablock decalData(CityRPG_Rain_Texture)
	{
		textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Rain/droplet";
		preload = true;
	};
}

if(!isObject(CityRPG_Splash_Texture))
{
	datablock decalData(CityRPG_Splash_Texture)
	{
		textureName = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Rain/splash";
		preload = true;
	};
}

return;

// WeatherBoxes
if(!isObject(WeatherBox_Clear))
{
	new InteriorInstance(WeatherBox_Clear)
	{
		position = "0 0 0";
		rotation = "1 0 0 0";
		scale = "1 1 1";
		interiorFile = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Clear/Sky_ClearSky.dif";
		useGLLighting = "0";
		showTerrainInside = "0";
	};
}