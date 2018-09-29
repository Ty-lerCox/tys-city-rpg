//---
//	@package VCE
//	@title Groups
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 4:23 PM 15/03/2011
//---
function getVariableGroupFromObject(%obj)
{
	switch$(%obj.getClassName())
	{
		case "Player":
			%vargroup = nameToID("VariableGroup_"@%obj.client.BL_ID);
		case "fxDtsBrick":
			%vargroup = nameToID("VariableGroup_"@%obj.getGroup().BL_ID);
		case "GameConnection":
			%vargroup = nameToID("VariableGroup_"@%obj.BL_ID);
		case "MinigameSO":
			%vargroup = nameToID("VariableGroup_"@%obj.owner.BL_ID);
		case "Vehicle":
			%vargroup = nameToID("VariableGroup_"@%obj.brickGroup.BL_ID);
	}
	if(isObject(%vargroup))
		return %vargroup;
	return -1;
}
function VCE_createVariableGroup(%brick)
{
	%brickgroup = getBrickGroupFromObject(%brick);
	
	if(isObject(%brickgroup) && !isObject(%brickgroup.vargroup) && %brickgroup.bl_id !$= "")
	{
		%brickgroup.vargroup = new ScriptObject("VariableGroup_" @ %brickgroup.bl_id)
		{
			class = "VariableGroup";
		};
		%brickgroup.vargroup.bl_id = %brickgroup.bl_id;
		%brickgroup.vargroup.name = %brickgroup.name;
		%brickgroup.vargroup.client = %brickgroup.client;
	}
}
function VariableGroup::setVariable(%group,%type,%name,%value,%obj)
{
	if(!isObject(%group) || $VCE::Server::SpecialVar[%obj.getClassName(),%name] !$= "")
		return;
	%group.value[%type,%obj,%name] = %value;
}
function VariableGroup::getVariable(%group,%type,%name,%obj)
{
	if(!isObject(%group) || $VCE::Server::SpecialVar[%obj.getClassName(),%name] !$= "")
		return;
	return %group.value[%type,%obj,%name];
}
function VariableGroup::saveVariable(%group,%type,%name,%obj)
{
	if(!isObject(%group) || $VCE::Server::SpecialVar[%obj.getClassName(),%name] !$= "")
		return;
	else
	{
		if(%group.value[%type,%obj,%name] $= "")
			return;
		switch$(%obj.getClassname())
		{
			case "Player":
				%value = %group.value[%type,%obj,%name];
				%id = %obj.client.BL_ID;
			case "GameConnection":
				%value = %group.value[%type,%obj,%name];
				%id = %obj.BL_ID;
			default:
				warn("VariableGroup::saveVariable - Unable to save "@%obj@" because it is not an accepted class.");
				return;
		}
		%line = VCE_getSaveLine(%group.bl_id,%id,%type,%name);
		if(%line <= 0)
			$VCE::Server::SaveLine[$VCE::Server::SaveLineCount++] = %group.BL_ID TAB %id TAB %type TAB %name TAB %value;
		else
			$VCE::Server::SaveLine[%line] = %group.BL_ID TAB %id TAB %type TAB %name TAB %value;
		if(isEventPending($VCE::Server::SaveSchedule))
			cancel($VCE::Server::SaveSchedule);
		$VCE::Server::SaveSchedule = %group.schedule(300,"saveAllVariables",$VCE::Server::SavePath);
	}
}
function VariableGroup::saveAllVariables(%group,%path)
{
	%file = new FileObject();
	%file.openForWrite(%path);
	%file.writeLine("VCE SAVE FILE (CONTAINS "@$VCE::Server::SaveLineCount@" VALUES)");
	for(%i=1;%i<=$VCE::Server::SaveLineCount;%i++)
		%file.writeLine($VCE::Server::SaveLine[%i]);
	%file.close();
	%file.delete();
}
function VariableGroup::loadVariable(%group,%type,%name,%obj)
{
	if(!isObject(%group) || $VCE::Server::SpecialVar[%obj.getClassName(),%name] !$= "")
		return;
	else
	{
		switch$(%obj.getClassname())
		{
			case "Player":
				%value = %group.value[%type,%obj,%name];
				%id = %obj.client.BL_ID;
			case "GameConnection":
				%value = %group.value[%type,%obj,%name];
				%id = %obj.BL_ID;
			default:
				warn("VariableGroup::loadVariable - Unable to load " @ %obj @ " because it is not an accepted class.");
				return;
		}
		%line = VCE_getSaveLine(%group.BL_ID,%id,%type,%name);
		if(%line == 0)
			return;
		%group.value[%type,%obj,%name] = getField($VCE::Server::SaveLine[%line],4);
	}
}
function VCE_getSaveLine(%groupid,%id,%type,%name)
{
	if($VCE::Server::SaveLineCount <= 0)
		return 0;
	for(%i=1;%i<=$VCE::Server::SaveLineCount;%i++)
	{
		%line = $VCE::Server::SaveLine[%i];
		if(getField(%line,0) == %groupid && getField(%line,1) == %id && getField(%line,2) $= %type && getField(%line,3) $= %name)
			return %i;
	}
	return 0;
}
function VCE_updateSaveFile()
{
	$VCE::Server::SaveLineCount = 0;
	%file = new FileObject();
	if(!isFile($VCE::Server::SavePath))
	{
		%file.openForWrite($VCE::Server::SavePath);
		%file.writeLine("VCE SAVE FILE (CONTAINS 0 VALUES)");
		%file.close();
		%file.delete();
		return;
	}
	%file.openForRead($VCE::Server::SavePath);
	%file.readLine();
	while(!%file.isEOF())
		$VCE::Server::SaveLine[$VCE::Server::SaveLineCount++] = %file.readLine();
	%file.close();
	%file.delete();
}
