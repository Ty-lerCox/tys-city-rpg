//////////////////////////////////////////////////

function loadBusiness()
{
	exec("config/server/CityRPG/Global/Business.cs");
	$Business::Loaded = 1;
}

function saveBusiness()
{
	export("$Business::*","config/server/CityRPG/Global/Business.cs");
}

//////////////////////////////////////////////////

function getBusiness(%id, %dataType)
{
	return $Business::ID[%id, %dataType];
}

function inputBusiness(%id, %dataType, %input)
{
	$Business::ID[%id, %dataType] = %input;
}