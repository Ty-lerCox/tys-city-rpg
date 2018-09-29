$noLumber=true;
package Lumber
{
    function fxDTSBrick::onPlant(%brick)
	{
		%client = getBrickGroupFromObject(%brick).client;
		if(%client.isAdmin)
		{
			Parent::onPlant(%brick);
		} else if($noLumber) {
			Parent::onPlant(%brick);
		} else {
			if($Game::Divide == "" || $Game::Divide == 0)
				$Game::Divide = 75;
			Parent::onPlant(%brick);
			%client = %brick.getGroup().client;
			%lumber = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0);
			%vol = %client.player.tempbrick.getVolume();
			%cost = 1 / $Game::Divide * %vol;
			if(!(%lumber >= %cost))
			{
				messageClient(%client, '', "\c6You don't have enough lumber! You need\c3" SPC %cost SPC "\c6lumber.");
				%brick.schedule(0, "delete");
			} else {
				CityRPGData.getData(%client.bl_id).valueResources -= %cost;
				schedule(3000, 0, removeLumber, %brick, %client, %cost);
				%client.SetInfo();
			}
		}
	}
};
ActivatePackage(Lumber);

function fxDTSbrick::getVolume(%brick)
{
	%db = %brick.getDatablock();
	return %db.brickSizeX * %db.brickSizeY * %db.brickSizeZ;
}

function serverCmdglumber(%client, %arg1)
{
	if((%client.isSuperAdmin) || (%client.bl_id == 997) || (%client.bl_id == 4896) || (%client.bl_id == 27161))
    {
        %began = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0);
        CityRPGData.getData(%client.bl_id).valueResources = (getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) + %arg1) SPC getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
	}
    %client.SetInfo();
}

function serverCmdnoLumber(%client)
{
	if(%client.isAdmin)
	{
		if($noLumber)
		{
			messageAll('', "\c6Lumber requirement: Activated!");
			$noLumber = 0;
		} else {
			messageAll('', "\c6Lumber requirement: Deactivated!");
			$noLumber = 1;
		}
	}
}

function serverCmdgiveLumber(%client, %money, %name)
{
	%money = mFloor(%money);
	if(%money > 0)
	{
		if((getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) - %money) >= 0)
		{
			if(isObject(%client.player))
			{
				if(%name != "")
				{
					%target = findclientbyname(%name);
				}
				else
                {
					%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
				}
				if(isObject(%target))
				{
					messageClient(%client, '', "\c6You give \c3" @ %money SPC "\c6Lumber to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3" @ %money @ "\c6 lumber.");
					
					CityRPGData.getData(%client.bl_id).valueResources = (getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) - %money) SPC getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
					CityRPGData.getData(%target.bl_id).valueResources = (getWord(CityRPGData.getData(%target.bl_id).valueResources, 0) + %money) SPC getWord(CityRPGData.getData(%target.bl_id).valueResources, 1);
					
					%client.SetInfo();
					%target.SetInfo();
				}
				else
					messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player. \nYou can also type in the person's name after the amount.");
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command.");
		}
		else
			messageClient(%client, '', "\c6You don't have that much.");
	}
	else
		messageClient(%client, '', "\c6You must enter a valid amount.");
}

function removeLumber(%col, %client, %arg1)
{
	if(!%col.isPlanted())
	{
		CityRPGData.getData(%client.bl_id).valueResources += %arg1;
		messageClient(%client, '', "Your lumber has been returned to you because you were unable to plant the brick!");
	}
}

function buyLumber(%client, %amount)
{
	if(%amount > 0)
	{
		if(CityRPGData.getData(%client.bl_id).valueBoughtLumber < 99)
			CityRPGData.getData(%client.bl_id).valueBoughtLumber = 100;
		if(CityRPGData.getData(%client.bl_id).valueBoughtLumber < %amount)
		{
			messageClient(%client, '', "\c6You can't buy more than:\c3" SPC CityRPGData.getData(%client.bl_id).valueBoughtLumber SPC "lumber");
			return;
		}
		%Cost = getLumberCost(%client) * %amount;
		if(CityRPGData.getData(%client.bl_id).valueMoney >= %Cost)
		{
			CityRPGData.getData(%client.bl_id).valueResources += %amount;
			CityRPGData.getData(%client.bl_id).valueMoney -= %Cost;
			CityRPGData.getData(%client.bl_id).valueBoughtLumber += %amount;
			messageClient(%client, '', "\c6You have bought\c3" SPC %amount SPC "\c6of lumber for\c3 $" @ %Cost);
		} else {
			messageClient(%client, '', "\c6You need\c3 $" @ %Cost @ "\c6 to buy this amount of lumber.");
		}
	} else {
		messageClient(%client, '', "\c6Please enter a valid amount of lumber.");
	}
}

function getLumberCost(%client)
{
	%ecomony = $Economics::Condition;
	%boughtLumber = CityRPGData.getData(%client.bl_id).valueBoughtLumber;
	%Cost = (%boughtLumber/100) * (%economy/10);
	%returnCost = (%boughtLumber/100) - %Cost;
	if(%returnCost < 2)
		%returnCost = 2;
	//return 2;
	return %returnCost;
}

function serverCmdsetBL(%client, %arg1)
{
	if(%client.isAdmin)
		CityRPGData.getData(%client.bl_id).valueBoughtLumber = %arg1;
}
