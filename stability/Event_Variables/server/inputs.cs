function fxDtsBrick::onVariableUpdate(%brick,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Minigame = getMinigameFromObject(%client);
	%brick.processInputEvent("onVariableUpdate",%client);
}
function fxDtsBrick::onVariableTrue(%brick,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Minigame = getMinigameFromObject(%client);
	%brick.processInputEvent("onVariableTrue",%client);
}
function fxDtsBrick::onVariableFalse(%brick,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Minigame = getMinigameFromObject(%client);
	%brick.processInputEvent("onVariableFalse",%client);
}
function fxDtsBrick::onVariableFunction(%brick,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Minigame = getMinigameFromObject(%client);
	%brick.processInputEvent("onVariableFunction",%client);
}