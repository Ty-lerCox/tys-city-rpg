//////////////////////////////////////////////////

function loadGang()
{
	exec("config/server/CityRPG/Global/Gang.cs");
	$Gangs::Loaded = 1;
}

function saveGang()
{
	export("$Gangs::*","config/server/CityRPG/Global/Gang.cs");
}

//////////////////////////////////////////////////

function getGang(%id, %dataType)
{
	return $Gangs::ID[%id, %dataType];
}

function inputGang(%id, %dataType, %input)
{
	$Gangs::ID[%id, %dataType] = %input;
}