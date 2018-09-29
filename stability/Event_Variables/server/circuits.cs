//discontinued
function fxDtsBrick::VCE_startCircuit(%brick,%delay,%client)
{
	cancel(%brick.circuitSchedule);
	%brick.circuitSchedule = %brick.schedule(%delay,"VCE_processCircuit",%brick,%delay,%client);
}
function fxDtsBrick::VCE_stopCircuit(%brick,%client)
{
	cancel(%brick.circuitSchedule);
}
function fxDtsBrick::VCE_processCircuit(%brick,%ignore,%delay,%client)
{
	%start = %brick.getTransform();
	%end = vectorAdd(%start,"0 0 " @ (%brick.getDatablock().brickSizeZ * 0.2) + 0.1);
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
	%end = vectorAdd(%start,"0 0 -" @ (%brick.getDatablock().brickSizeZ * 0.2) - 0.1);
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
	%end = vectorAdd(%start,"0 " @ (%brick.getDatablock().brickSizeY / 2) + 0.1 @ " 0");
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
	%end = vectorAdd(%start,"0 -" @ (%brick.getDatablock().brickSizeY / 2) + 0.1 @ " 0");
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
	%end = vectorAdd(%start,(%brick.getDatablock().brickSizeX / 2) + 0.1 @ " 0 0");
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
	%end = vectorAdd(%start,-(%brick.getDatablock().brickSizeX / 2) - 0.1 @ " 0 0");
	%target = firstWord(containerRaycast(%start,%end,$TypeMasks::FxBrickObjectType,%brick));
	if(isObject(%target) && %target != %ignore && getTrustLevel(%brick.getGroup(),%target.getGroup()) >= 2)
		%target.onVariableCircuit(%brick,%delay,%client);
}
function fxDtsBrick::onVariableCircuit(%brick,%ignore,%delay,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Minigame = getMinigameFromObject(%client);
	%brick.processInputEvent("onVariableCircuit",%client);
	%brick.circuitSchedule = %brick.schedule(%delay,"VCE_processCircuit",%ignore,%delay,%client);
}