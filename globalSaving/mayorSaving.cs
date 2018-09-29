// function serverCmdloadMayor(%client)
// {
	// exec("config/server/CityRPG/Global/Mayor.cs");
	// $Mayor::Loaded = 1;
	// messageClient(%client, '', "Loaded.");
// }

// function serverCmdloadMayorex(%client)
// {
	// exec("config/server/CityRPG/Global/Mayor.cs");
	// $Mayor::Loaded = 0;
	// messageClient(%client, '', "Loaded.");
// }

// function serverCmdsaveMayor(%client)
// {
	// export("$Mayor::*","config/server/CityRPG/Global/Mayor.cs");
	// messageClient(%client, '', "Saved.");
// }

// function serverCmdgetMayor(%client, %id, %dataType)
// {
	// messageClient(%client, '', $Mayor::ID[%id, %dataType]);
// }

// function serverCmdinputMayor(%client, %id, %dataType, %input)
// {
	// $Mayor::ID[%id, %dataType] = %input;
// }

//////////////////////////////////////////////////

function loadMayor()
{
	exec("config/server/CityRPG/Global/Mayor.cs");
	$Mayor::Loaded = 1;
}

function saveMayor()
{
	export("$Mayor::*","config/server/CityRPG/Global/Mayor.cs");
}

//////////////////////////////////////////////////

function getMayor(%id, %dataType)
{
	return $Mayor::ID[%id, %dataType];
}

function inputMayor(%id, %dataType, %input)
{
	$Mayor::ID[%id, %dataType] = %input;
}