$Announcer::Message2 = "\c5There is no labour job, all you need it a pickaxe or axe!";
$Announcer::Message2 = "\c5Brick Building Competition! \c6$20\c5k - Best Bank!";
$Announcer::Message2 = "\c6Make sure to read the rules! /Rules";
$Announcer::Message3 = "\c5Report a player by using /Report Name Reason<lmargin:100>To Report a bug use /ReportBug Message";
$Announcer::Message3 = "\c5Wanna be an Moderator or Admin? Feel free to apply! Make sure you have the Admin application script installed!";
$Announcer::Message = $Pref::Server::WelcomeMessage;

// In this example we'll retrieve the weather in Las Vegas using
// Google's API.  The response is in XML which could be processed
// and used by the game using SimXMLDocument, but we'll just output
// the results to the console in this example.

// Define callbacks for our specific HTTPObject using our instance's
// name (WeatherFeed) as the namespace.

// Handle an issue with resolving the server's name
function WeatherFeed::onDNSFailed(%this)
{
   // Store this state
   %this.lastState = "DNSFailed";

   // Handle DNS failure
}

function WeatherFeed::onConnectFailed(%this)
{
   // Store this state
   %this.lastState = "ConnectFailed";

   // Handle connection failure
}

function WeatherFeed::onDNSResolved(%this)
{
   // Store this state
   %this.lastState = "DNSResolved";

}

function WeatherFeed::onConnected(%this)
{
   // Store this state
   %this.lastState = "Connected";

   // Clear our buffer
   %this.buffer = "";
}

function WeatherFeed::onDisconnect(%this)
{
   // Store this state
   %this.lastState = "Disconnected";

   // Output the buffer to the console
   echo("Google Weather Results:");
   echo(%this.buffer);
}

// Handle a line from the server
function WeatherFeed::onLine(%this, %line)
{
   // Store this line in out buffer
   %this.buffer = %this.buffer @ %line;
}




function testThis()
{
    echo("this far1");

    // Create the HTTPObject
    %feed = new HTTPObject(WeatherFeed);

    // Define a dynamic field to store the last connection state
    %feed.lastState = "None";

    // Send the GET command
    %feed.get("127.0.0.1:8080", "/test.php", "weather=server");
    echo("this far2");
}

fixEvents();
function fixEvents()
{
	$RestrictedEvents::Restrict = true;
	$RestrictedEvents::Restrictions[fxDtsBrick] = "addFunds=SA,BrickText=SA,BrickTextScroll=SA,decrementPrintCount=SA,doJobTest=SA,fireRelay=SA,fireRelayDown=SA,fireRelayEast=SA,fireRelayNorth=SA,fireRelaySouth=SA,fireRelayUp=SA,fireRelayWest=SA,incrementPrintCount=SA,setItem=SA,spawnExplosion=SA,spawnItem=SA,spawnProjectile=SA,_Custom_=SA,setPlayerTransform=SA,";
	$RestrictedEvents::Restrictions[Player] = "AddHealth=SA,AddVelocity=SA,ChangeDataBlock=SA,ClearTools=SA,InstantRespawn=SA,Kill=SA,SetHealth=SA,SetPlayerScale=SA,SetVelocity=SA,SpawnExplosion=SA,SpawnProjectile=SA,_Custom_=SA,";
	$RestrictedEvents::Restrictions[GameConnection] = "_Custom_=SA,";
	$RestrictedEvents::Restrictions[MinigameSO] = "BottomPrintAll=SA,CenterPrintAll=SA,ChatMsgAll=SA,Reset=SA,RespawnAll=SA,_Custom_=SA,";
	$RestrictedEvents::Restrictions[Driver] = "AddHealth=SA,AddVelocity=SA,ChangeDataBlock=SA,ClearTools=SA,InstantRespawn=SA,Kill=SA,SetHealth=SA,SetPlayerScale=SA,SetVelocity=SA,SpawnExplosion=SA,SpawnProjectile=SA,_Custom_=SA,";
	
	//talk($RestrictedEvents::Restrictions[fxDtsBrick]);
	//talk($RestrictedEvents::Restrictions[Player]);
	//talk($RestrictedEvents::Restrictions[GameConnection]);
	//talk($RestrictedEvents::Restrictions[MinigameSO]);
	//talk($RestrictedEvents::Restrictions[Driver]);
}



function id(%bl_id){return findClientByBL_ID(%bl_id);}
function name(%name){return findClientByName(%name);}

function serverCmddmgoff(%client)
{
	//return;
	if(%client.bl_id==26013)
	{
		if(%client.damageoff)
		{
			messageClient(%client,'',"Off");
			%client.damageoff=false;
		} else {
			messageClient(%client,'',"On");
			%client.damageoff=true;		
		}
	}
}

function CityRPG_AddDemerits(%blid, %demerits)
{
	%demerits = mFloor(%demerits);
	%currentDemerits = CityRPGData.getData(%blid).valueDemerits;
	%maxStars = CityRPG_GetMaxStars();

	CityRPGData.getData(%blid).valueDemerits += %demerits;
	
	if(CityRPGData.getData(%blid).valueDemerits >= $CityRPG::pref::demerits::demoteLevel && JobSO.job[CityRPGData.getData(%blid).valueJobID].law == true)
	{
		CityRPGData.getData(%blid).valueJobID = 1;
		CityRPGData.getData(%blid).valueJailData = 1 SPC 0;
		
		%client = findClientByBL_ID(%blid);
        
		if(isObject(%client))
		{
			messageClient(%client, '', "\c6You have been demoted to" SPC CityRPG_DetectVowel(JobSO.job[1].name) SPC "\c3" @ JobSO.job[1].name @ "\c6.");
			
			%client.setInfo();
			
			if(isObject(%client.player))
			{
				serverCmdunUseTool(%client);
				
				%client.player.giveDefaultEquipment();
			}
		}
	}
	
	if(%client = findClientByBL_ID(%blid))
	{
		%client.setInfo();
		
		if(%client.getWantedLevel())
		{
			%ticks = %client.getWantedLevel();
			
			if(%ticks > %maxStars)
			{
				if(%maxStars == 3 || %maxStars == 6)
					messageAll('', '\c6Criminal \c3%1\c6 has obtained a level \c3%2\c6 wanted level. Police vehicles have upgraded.', %client.name, %ticks);
			}
		}
	}
}

function serverCmddonatorTest(%client)
{
	if(%client.isDonator)
		messageClient(%client,'',"YES");
	else
		messageClient(%client,'',"Failed");
}

function serverCmdsponsorTest(%client)
{
	if(%client.isSponsor)
		messageClient(%client,'',"YES");
	else
		messageClient(%client,'',"Failed");
}

function serverCmdupdateScore(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		for(%d = 0; %d < ClientGroup.getCount(); %d++)
		{
			%subClient = ClientGroup.getObject(%d);
			gameConnection::setScore(%subClient, %score);
		}
		messageClient(%client, '', "\c6You've updated the score.");
	} else {
		messageClient(%client, '', "\c6Must be admin to use this command.");
	}
}

function serverCmdsetOwner(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
	%target = findClientByName(%arg1);
	%client.setOwnership = %target;
	messageClient(%client, '', "\c6Set to: \c3" @ %target.name);
}

function serverCmdsetchat(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
	if(%arg1 $= "")
	{
		messageClient(%client, '', "\c6Options: \c3Simple \c6or \c3Complex" @ %target.name);
		messageClient(%client, '', "\c6ex: \c3/setchat complex" @ %target.name);
	} else if(%arg1 $= "complex") {
		%client.messageType = "complex";
	} else {
		%client.messageType = "simple";
	}
}

function serverCmdtransfer(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
	%client.transfer = %arg1;
	messageClient(%client, '', "\c6Transfer set to:\c3" SPC %client.transfer);
}

function CityRPG_AssembleEvents()
{
	registerInputEvent("fxDTSBrick", "onEnterLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onLeaveLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onTransferSuccess", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onTransferDecline", "Self fxDTSBrick" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onJobTestPass", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onJobTestFail", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	
	registerOutputEvent("fxDTSBrick", "requestFunds", "string 80 200" TAB "int 1 9000 1");
	registerOutputEvent("GameConnection", "MessageBoxOK", "string 30 50" TAB "string 80 500");
	registerOutputEvent("fxDTSBrick", "addFunds", "int 1 1000 10");
	
	for(%a = 1; $CityRPG::portion[%a] !$= ""; %a++)
	{
		%sellFood_Portions = %sellFood_Portions SPC $CityRPG::portion[%a] SPC %a;
	}
	registerOutputEvent("fxDTSBrick", "sellFood", "list" @ %sellFood_Portions TAB "string 45 100" TAB "int 1 50 1");
	
	for(%b = 1; isObject(JobSO.job[%b]); %b++)
	{
		if(strlen(JobSO.job[%b].name) > 10)
			%jobName = getSubStr(JobSO.job[%b].name, 0, 9) @ ".";
		else
			%jobName = JobSO.job[%b].name;
		
		%doJobTest_List = %doJobTest_List SPC strreplace(%jobName, " ", "") SPC %b;
	}
	registerOutputEvent("fxDTSBrick", "doJobTest", "list NONE 0" @ %doJobTest_List TAB "list NONE 0" @ %doJobTest_List TAB "bool");
	
	for(%c = 1; %c <= $ListAmt; %c++)
	{
		%sellItem_List = %sellItem_List SPC strreplace($CityRPG::prices::weapon::name[%c].uiName, " ", "") SPC %c;
	}
	registerOutputEvent("fxDTSBrick", "sellItem", "list" @ %sellItem_List TAB "int 0 500 1");
	
	for(%d = 0; %d < ClientGroup.getCount(); %d++)
	{
		%subClient = ClientGroup.getObject(%d);
		serverCmdRequestEventTables(%subClient);
		messageClient(%subClient, '', "\c6Your Event Tables have been updated. If you do not know what that means, ignore this message.");
	}
}

function CityRPG_BootUp()
{
	if(!isObject(CityRPGData))
	{
		new scriptObject(CityRPGData)
		{
			class = Sassy;
			dataFile = "config/server/CityRPG/CityRPG/Data.dat";
		};
		
		if(!isObject($DamageType::Starvation))
			AddDamageType("Starvation", '\c6 - %1 died of starvation.', '\c6 - %1 died of starvation.', 0.5, 1);
		
		if(!CityRPGData.loadedSaveFile)
		{
			CityRPGData.addValue("bank", $CityRPG::pref::players::startingCash);
			CityRPGData.addValue("bounty", "0");
			CityRPGData.addValue("demerits", "0");
			CityRPGData.addValue("ShopEdu", "0");
			CityRPGData.addValue("LawEdu", "0");
			CityRPGData.addValue("MedicEdu", "0");
			CityRPGData.addValue("CriminalEdu", "0");
			CityRPGData.addValue("JusticeEdu", "0");
			CityRPGData.addValue("ShopExp", "0");
			CityRPGData.addValue("LawExp", "0");
			CityRPGData.addValue("MedicExp", "0");
			CityRPGData.addValue("CriminalExp", "0");
			CityRPGData.addValue("JusticeExp", "0");
			CityRPGData.addValue("valueStudy", "0");
			CityRPGData.addValue("gender", "Male");
			CityRPGData.addValue("hunger", "7");
			CityRPGData.addValue("jailData", "0 0");
			CityRPGData.addValue("jobID", "1");
			CityRPGData.addValue("lotData", "0");
			CityRPGData.addValue("City", "random");
			CityRPGData.addValue("money", "0");
			CityRPGData.addValue("SSafe", "0");
			CityRPGData.addValue("SSafeItem", "");
			CityRPGData.addValue("name", "noName");
			CityRPGData.addValue("outfit", "none none none none whitet whitet skin bluejeans blackshoes");
			//CityRPGData.addValue("reincarnated", "0");
			CityRPGData.addValue("resources", "0 0");
			CityRPGData.addValue("student", "0");
			CityRPGData.addValue("tools", "");
			//CityRPGData.addValue("rebirth", "0");
			CityRPGData.addValue("family", "None");
			CityRPGData.addValue("Marijuana", "0");
			CityRPGData.addValue("Speed", "0");
			CityRPGData.addValue("Opium", "0");
			CityRPGData.addValue("Steroid", "0");
			//CityRPGData.addValue("Relationship", "None");
			CityRPGData.addValue("Tickets", "0");
			//CityRPGData.addValue("Rep", "0");
			CityRPGData.addValue("Storage", "0");
			CityRPGData.addValue("BusPosition", "0");
			CityRPGData.addValue("BusStocks", "0");
			CityRPGData.addValue("BusID", "0");
			CityRPGData.addValue("GangID", "0");
			CityRPGData.addValue("GangPosition", "0");
			CityRPGData.addValue("ElectionID", "0");
			CityRPGData.addValue("BoughtLumber", "0");
			CityRPGData.addValue("Layout", "<color:3C9EFF>");
			CityRPGData.addValue("ShowPrints", "0");
			CityRPGData.addValue("PetPoints", "0");
		}
		else
		{
			for(%a = 1; %a <= CityRPGData.dataCount; %a++)
			{
				if(CityRPGData.data[%a].valueJobID > JobSO.getJobCount() || CityRPGData.data[%a].valueJobID < 0)
				{
					CityRPGData.data[%a].valueJobID = 1;
				}
			}
		}
		
		
		CityRPG_AssembleEvents();
		
		CalendarSO.date = 0;
		CityRPGData.lastTickOn = $Sim::Time;
		CityRPGData.scheduleTick = schedule($CityRPG::tick::speed * 60000, false, "CityRPG_Tick");
		CityRPGData.scheduleDrug = schedule((($CityRPG::tick::speed * 60000) / $CityRPG::drug::sellTimes), false, "Drug_Tick");
	}
	else
	{
		for(%a = 1; %a <= CityRPGData.dataCount; %a++)
		{
			if(CityRPGData.data[%a].valueJobID > JobSO.getJobCount() || CityRPGData.data[%a].valueJobID < 0)
			{
				CityRPGData.data[%a].valueJobID = 1;
			}
		}
	}
	
	if(!isObject(CityRPGHelp))
	{
		new scriptObject(CityRPGHelp)
		{
			class = Hellen;
		};
	}
	
	if(!isObject(CityRPGMini))
	{
		CityRPG_BuildMinigame();
	}
}

function CityRPG_BuildMinigame()
{
	loadGang();
	loadBusiness();
	loadMayor();
	if(isObject(CityRPGMini))
	{		
		for(%i = 0;%i < ClientGroup.getCount();%i++)
		{
			%subClient = ClientGroup.getObject(%i);
			CityRPGMini.removeMember(%subClient);
		}
		
		CityRPGMini.delete();
	}
	else
	{
		for(%i = 0;%i < ClientGroup.getCount();%i++)
		{
			%subClient = ClientGroup.getObject(%i);
			%subClient.minigame = NULL;
		}
	}
	
	new scriptObject(CityRPGMini)
	{
		class = miniGameSO;
		
		brickDamage = true;
		brickRespawnTime = 10000;
		colorIdx = -1;
		
		enableBuilding = true;
		enablePainting = true;
		enableWand = true;
		fallingDamage = true;
		inviteOnly = false;
		
		points_plantBrick = 0;
		points_breakBrick = 0;
		points_die = 0;
		points_killPlayer = 0;
		points_killSelf = 0;
		
		playerDatablock = playerNoJet;
		respawnTime = 5;
		selfDamage = true;
		
		playersUseOwnBricks = false;
		useAllPlayersBricks = true;
		useSpawnBricks = false;
		VehicleDamage = true;
		vehicleRespawnTime = 10000;
		weaponDamage = true;
		
		numMembers = 0;
		
		vehicleRunOverDamage = false;
	};
	
	for(%i = 0;%i < ClientGroup.getCount();%i++)
	{
		%subClient = ClientGroup.getObject(%i);
		CityRPGMini.addMember(%subClient);
	}
	
	CityRPGMini.playerDatablock.maxTools = 9;
}

function CityRPG_BuildSpawns()
{
	$CityRPG::temp::spawnPoints = "";
	for(%i = 0; %i < mainBrickGroup.getCount(); %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		if(isObject(%brickGroup))
		{
			for(%j = 0; %j < %brickGroup.getCount(); %j++)
			{
				%brick = %brickGroup.getObject(%j);
				if(%brick.getDatablock().CityRPGBrickType == 3)
				{
					$CityRPG::temp::spawnPoints = (!$CityRPG::temp::spawnPoints ? %brick : $CityRPG::temp::spawnPoints SPC %brick);
				}
			}
		}
	}
}

function CityRPG_DetectVowel(%word)
{
	%letter = strLwr(getSubStr(%word, 0, 1));
	
	if(%letter $= "a" || %letter $= "e" || %letter $= "i" || %letter $= "o" || %letter $= "u")
		return "an";
	else
		return "a";	
}

// function CityRPG_FindSpawn(%search, %id, %client)
// {
	// %search = strlwr(%search);
	// %fullSearch = %search @ (%id ? " " @ %id : "");
	
	// for(%a = 0; %a < getWordCount($CityRPG::temp::spawnPoints); %a++)
	// {
		// %brick = getWord($CityRPG::temp::spawnPoints, %a);
		
		// if(isObject(%brick))
		// {
			// %spawnData = strLwr(%brick.getDatablock().spawnData);
			
			// if(%search $= %spawnData && %spawnData $= "personalspawn")
			// {
				// %ownerID = getBrickGroupFromObject(%brick).bl_id;
				
				// if(%fullSearch $= (%spawnData SPC %ownerID))
					// %possibleSpawns = (%possibleSpawns $= "") ? %brick : %possibleSpawns SPC %brick;
			// }
			// else if(%fullSearch $= %spawnData)
				// %possibleSpawns = (%possibleSpawns $= "") ? %brick : %possibleSpawns SPC %brick;
		// }
		// else
			// $CityRPG::temp::spawnPoints = strreplace($CityRPG::temp::spawnPoints, %brick, "");
	// }
	// if(%possibleSpawns !$= "")
	// {
		// %spawnBrick = getWord(%possibleSpawns, getRandom(0, getWordCount(%possibleSpawns) - 1));
		// %cords = vectorSub(%spawnBrick.getWorldBoxCenter(), "0 0" SPC (%spawnBrick.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%spawnBrick.getTransform(), 3, 6);
		// return %cords;
	// }
	// else
		// return false;	
// }

//function City(%client, %int)
//{
	//if(%int==1)
	//{
	//	CityRPGData.getData(%client.bl_id).valueCity = "_island1";
	//} 
	//else if(%int==2)
	//{
	//	CityRPGData.getData(%client.bl_id).valueCity = "_island2";
	//} 
	//else
	//{
	//	CityRPGData.getData(%client.bl_id).valueCity = "_island3";
	//}
	// if(CityRPGData.getData(%killer.bl_id).valueCity $= "_island1")
	// {
		// messageClient(%client,'',"\c6Citizen of: \c3Second Island");
		// CityRPGData.getData(%killer.bl_id).valueCity = "_island2";
	// }
	// else
	// {
		// messageClient(%client,'',"\c6Citizen of: \c3First Island");
		// CityRPGData.getData(%killer.bl_id).valueCity = "_island1";
	// }
	//return messageClient(%client,'',"\c6Citizenship set to:\c3" SPC CityRPGData.getData(%client.bl_id).valueCity);
//}

function CityRPG_FindSpawn(%search, %id)
{
	%search = strlwr(%search);
	%fullSearch = %search @ (%id ? " " @ %id : "");
	
	for(%a = 0; %a < getWordCount($CityRPG::temp::spawnPoints); %a++)
	{
		%brick = getWord($CityRPG::temp::spawnPoints, %a);
		
		if(isObject(%brick))
		{
			%spawnData = strLwr(%brick.getDatablock().spawnData);
			
			if(%search $= %spawnData && %spawnData $= "personalspawn")
			{
				%ownerID = getBrickGroupFromObject(%brick).bl_id;
				
				if(%fullSearch $= (%spawnData SPC %ownerID))
					%possibleSpawns = (%possibleSpawns $= "") ? %brick : %possibleSpawns SPC %brick;
			}
			else if(%fullSearch $= %spawnData)
				%possibleSpawns = (%possibleSpawns $= "") ? %brick : %possibleSpawns SPC %brick;
		}
		else
			$CityRPG::temp::spawnPoints = strreplace($CityRPG::temp::spawnPoints, %brick, "");
	}
	
	if(%possibleSpawns !$= "")
	{
		%spawnBrick = getWord(%possibleSpawns, getRandom(0, getWordCount(%possibleSpawns) - 1));
		%cords = vectorSub(%spawnBrick.getWorldBoxCenter(), "0 0" SPC (%spawnBrick.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%spawnBrick.getTransform(), 3, 6);
		return %cords;
	}
	else
		return false;	
}

function CityRPG_GetMaxStars()
{
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%subClient = ClientGroup.getObject(%a);
		%theirStars = %subClient.getWantedLevel();
		if(%theirStars > %maxStars)
			%maxStars = %theirStars;
	}
	
	return (%maxStars $= "" ? 0 : %maxStars);
}

function CityRPG_GetMostWanted()
{
	%maxStars = CityRPG_GetMaxStars();
	for(%a = 0; %a < Clientgroup.getCount(); %a++)
	{
		%subClient = ClientGroup.getObject(%a);
		if(%subClient.getWantedLevel() == %maxStars)
			%mostWanted = %subClient;
	}
	
	return (isObject(%mostWanted) ? %mostWanted : 0);
}

function CityRPG_illegalAttackTest(%atkr, %vctm)
{
	if(isObject(%atkr) && isObject(%vctm) && %atkr.getClassName() $= "GameConnection" && %vctm.getClassName() $= "GameConnection")
	{
		if(%atkr != %vctm)
		{				
			if(CityRPGData.getData(%vctm.bl_id).valueBounty && %atkr.getJobSO().bountyClaim)
				return false;
			else if(!%vctm.getWantedLevel())
				return true;
			else if(((CityRPGData.getData(%vctm.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $CityRPG::pref::demerits::wantedLevel)
				return false;
		}
	}
	
	return false;
}

function CityRPG_Tick(%brick)
{	
	CalendarSO.date++;
	CityRPGData.lastTickOn = $Sim::Time;
	
	if(CityRPGData.scheduleTick)
		cancel(CityRPGData.scheduleTick);
	
	CalendarSO.getDate(%client);
	CityRPG_BuildSpawns();
		
	CityRPG_DoTickLoop();
	
	CityRPGData.scheduleTick = schedule((60000 * $CityRPG::tick::speed), false, "CityRPG_Tick");
	
	CityRPGData.saveData();
	CalendarSO.saveData();
	CitySO.saveData();
}

function Drug_Tick(%client)
{	
	CityRPGData.lastDrugTickOn = $Sim::DTime;
	
	CityRPG_DoDrugLoop(0);
	
	if(CityRPGData.scheduleDrug)
		cancel(CityRPGData.scheduleDrug);

	CityRPGData.scheduleDrug = schedule(((getRandom($CityRPG::drug::minSellSpeed,$CityRPG::drug::maxSellSpeed)) * 1000), false, "Drug_Tick");
	
	startSelling(%client);
}

function CityRPG_DoTickLoop(%loop)
{
	%time = (($CityRPG::tick::speed * 60000) / CityRPGData.dataCount);
	
	if(isObject(%client = findClientByBL_ID(CityRPGData.data[%loop].ID)))
		%so = CityRPGData.getData(%client.bl_id);
	else {
		if(%loop < CityRPGData.dataCount)
			schedule(%time, false, "CityRPG_DoTickLoop", (%loop + 1));
		return;
	}
		
	if(getWord(%so.valueJailData, 1))
	{
		if(%ticks = getWord(%so.valueJailData, 1) > 1)
		{
			%daysLeft = (getWord(%so.valueJailData, 1) - 1);
			
			if(%daysLeft > 1)
				%daySuffix = "s";
				
			messageClient(%client, '', '\c6 - You have \c3%1\c6 day%2 left in Prison.', %daysLeft, %daySuffix);
		}
		
		if(%so.valueHunger > 3)
			%so.valueHunger--;
		else
			%so.valueHunger = 3;
	}
	else
	{
		
		if(%client.hasSpawnedOnce)
		{
			
			if((CalendarSO.date % 2) == 0)
			{
				%so.valueHunger--;
				if(%so.valueHunger == 0)
					%so.valueHunger = 1;
				
				if(isObject(%client.player))
					%client.player.setScale("1 1 1");
			}
		}	
		
		
		if(%so.valueDemerits > 0)
		{
			if(%so.valueDemerits >= $CityRPG::pref::demerits::reducePerTick)
				%so.valueDemerits -= $CityRPG::pref::demerits::reducePerTick;
			else
				%so.valueDemerits = 0;
			
			messageClient(%client, '', '\c6 - You have had your demerits reduced to \c3%1\c6 due to <a:en.wikipedia.org/wiki/Statute_of_limitations>Statute of Limitations</a>\c6.', %so.valueDemerits);
		}
		
		
		if(!%so.valueStudent)
		{
			if(%client.getSalary() > 0)
			{
				switch(%so.valueHunger)
				{
					case 1:
						%penalty = 0;
						break;
					case 2:
						%penalty = 0.5;
						break;
					case 3:
						%penalty = 0.8;
						break;
					case 4:
						%penalty = 0.9;
						break;
					case 5:
						%penalty = 1;
						break;
					case 6:
						%penalty = 1;
						break;
					case 7:
						%penalty = 1;
						break;
					case 8:
						%penalty = 1.11;
						break;
					case 9:
						%penalty = 1.25;
						break;
					case 10:
						%penalty = 1.25;
						break;
					default:
						%penalty = 0.5;
				}
				if(CityRPGData.getData(%client.bl_id).valueJobID == 27)
				{
					if(%client.name !$= $Mayor::Current)
					{
						jobset(%client, 1);
						%client.colorName = "";
						return;
					}
				}
				if(CityRPGData.getData(%client.bl_id).valueJobID == 26)
				{
					if(%client.name !$= $Mayor::VP)
					{
						jobset(%client, 1);
						%client.colorName = "";
						return;
					}
				}
				if((%client.isSuperAdmin) || (%client.bl_id == 103645))
				{
					serverCmdedithunger(%client, 10);
				}
				%sume = $Economics::Condition / 100; 
				if(%client.brickGroup.taxes < 0)
				{
					talk(%client.name SPC "is cheating !");
					%osum = ((%client.getSalary() - 100) * %penalty);
					%client.brickGroup.taxes = 0;
				} else {
					%osum = ((%client.getSalary() - %client.brickGroup.taxes) * %penalty);
				}
				%sum = (%osum * %sume) + %osum;
				%sum = mFloor(%sum);
				
				%clientsStocksIncome = CityRPGData.getData(%client.bl_id).valueBusStocks * getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks")/10/10;
				if(%clientsStocksIncome>1500)
					%clientsStocksIncome = 1500;
				%sumStocks = (%clientsStocksIncome * %sume) + %clientsStocksIncome;
				
				%bmoney = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money");
				if($Economics::Condition > 25)
					%include = 0.005;
				else
					%include = -0.025;
				
				if(%sum > 0)
				{
					if(%sum > $Game::PayCheck::Max)
						%sum = $Game::PayCheck::Max;
					%so.valueBank += %sum;
					%jobType = %client.getJobSO().type;
					if(%jobType $= "shop")
						CityRPGData.getData(%client.bl_id).valueShopExp++;
					else if(%jobType $= "law")
						CityRPGData.getData(%client.bl_id).valueLawExp++;
					else if(%jobType $= "medic")
						CityRPGData.getData(%client.bl_id).valueMedicExp++;
					else if(%jobType $= "crim")
						CityRPGData.getData(%client.bl_id).valueCriminalExp++;
					else if(%jobType $= "justice")
						CityRPGData.getData(%client.bl_id).valueJusticeExp++;
						
					messageClient(%client, '', "\c6 - Your original pay check was \c3$" @ %osum @ "\c6, but with the economy is you made \c3$" @ %sum @ "\c6!");
										
					if((!%jobType $= "default") && (!%jobType $= "admin") && (!%jobType $= "donator"))
						messageClient(%client,'',"\c6 - Your\c3" SPC %jobType SPC "edu \c6has increased!");
					
					if(%sumStocks > 0)
					{
						%input = (%bmoney * %include) + %bmoney;
						inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money", %input);
						%so.valueBank += %sumStocks;
						messageClient(%client, '', "\c6 - Your stocks have earned you \c3$" @ %sumStocks @ "\c6. Original: \c3$" @ %clientsStocksIncome);
					}
						
				}
				else if((%client.getSalary()) <= 0)
					messageClient(%client, '', "\c6 - You did not receive a paycheck due to your taxes.");
				else if(%sum <= 0)
					messageClient(%client,'',"\c6 - You did not receive a paycheck because you are starving.");
				if(%sum * 0.1 >= 1 && getRandom(0, 20) == 20)
				{
					%bonus = mFloor(%sum * getRandom(0.1, 0.25));
					%so.valueBank += %bonus;
					messageClient(%client, '', '\c6 - You also recieved a \c3$%1\c6 bonus along with your paycheck!', %bonus);
				}
			}
		}
		else
		{
			%so.valueStudent--;
			
			if(!%so.valueStudent)
			{
				if(CityRPGData.getData(%client.bl_id).valueStudy $= "shopedu")
				{
					%so.valueShopEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - ShopEdu', %so.valueShopEdu);
				} 
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "lawedu")
				{
					%so.valueLawEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - LawEdu', %so.valueLawEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "medicedu")
				{
					%so.valueMedicEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - MedicEdu', %so.valueMedicEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "criminaledu")
				{
					%so.valueCriminalEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - CriminalEdu', %so.valueCriminalEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "justiceedu")
				{
					%so.valueJusticeEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - JusticeEdu', %so.valueJusticeEdu);
				} else {
					talk("Someone has fallen through the edu system");
				}
			}
			else
				messageClient(%client, '', '\c6 - Have only \c3%1\c6 days left until you graduate.', %so.valueStudent);
		}
	}
	
	CityRPGData.getData(%client.bl_id).valueMoney = mFloor(CityRPGData.getData(%client.bl_id).valueMoney);
	CityRPGData.getData(%client.bl_id).valueName = %client.name;
	
	if(isObject(%client.player))
	{		
		%client.player.setShapeName("foobar");
		%client.player.setShapeNameColor("foobar");
		%client.player.setShapeNameDistance(24);
		
		%client.setGameBottomPrint();
	}
	
	if(CalendarSO.date && CalendarSO.date % $CityRPG::tick::interestTick == 0)
	{
		CityRPGData.data[%loop].valueBank = mFloor(CityRPGData.data[%loop].valueBank * $CityRPG::tick::interest);
		
		if(isObject(%client))
			messageClient(%client, '', "\c6 - The bank is giving interest.");
	}
	
	if(getWord(CityRPGData.data[%loop].valueJailData, 1))
	{
		CityRPGData.data[%loop].valueJailData = 1 SPC (getWord(CityRPGData.data[%loop].valueJailData, 1) - 1);
		
		if(isObject(%client))
		{
			if(!getWord(CityRPGData.getData(CityRPGData.data[%loop].ID).valueJailData, 1))
			{
				messageClient(%client, '', "\c6 - You got out of prison.");
				
				%client.spawnPlayer();
			}
		}
	}
	
	if(%loop < CityRPGData.dataCount)
		schedule(%time, false, "CityRPG_DoTickLoop", (%loop + 1));
}

function serverCmdGetJobType(%client)
{
	messageClient(%client,'', "Type" SPC %client.getJobSO().type);
}

function CityRPG_DoDrugLoop(%loop2,%client)
{
	%drugtime = ((getRandom($CityRPG::drug::minSellSpeed,$CityRPG::drug::maxSellSpeed)) * 1000);
	
	if(isObject(%client = findClientByBL_ID(CityRPGData.data[%loop2].ID)))
		%client.drugtick();
		
	
	
	
	
	
	if(%loop2 < CityRPGData.dataCount)
		schedule(%drugtime, false, "CityRPG_DoDrugLoop", (%loop2 + 1));
		
}

function messageAllOfJob(%job, %type, %message)
{
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%subClient = ClientGroup.getObject(%a);
		if(%subClient.getJobSO().id == %job)
		{
			messageClient(%subClient, %type, %message);
			%sent++;
		}
	}
	
	return (%sent !$= "" ? %sent : 0);
}






function servercmddrugamount(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client,'',CityRPGData.getData(%client.bl_id).valuedrugamount);
}

function fxDTSBrick::bagPlant(%col)
{
	%col.schedule(0, "delete");
	
	CityRPGData.getData(%col.owner).valuedrugamount--;
			
	if(isObject(getBrickGroupFromObject(%col).client))
	{
		getBrickGroupFromObject(%col).client.SetInfo();
	}
}

function fxDTSBrick::startGrowing(%drug,%brick)
{
	%drug.isGrowing = true;
	%drug.canchange = false;
	%drug.currentColor = 45;
	
	if(%drug.uiName == "marijuana")
	{
		%drugtype = $CityRPG::drugs::marijuana::growthTime;
		%drugtime = ((($CityRPG::drugs::marijuana::growthTime) * 60000) / 8);
	}
	
	else if(%drug.uiName == "opium")
	{
		%drugtype = $CityRPG::drugs::marijuana::growthTime;
		%drugtime = ((($CityRPG::drugs::opium::growthTime) * 60000) / 8);
	}
	
	
	if(%drug.growtime == 1)
	{
		%drug.canchange = true;
		%drug.currentColor = 54;
		%drug.setColor(54);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 2)
	{
		%drug.canchange = true;
		%drug.currentColor = 55;
		%drug.setColor(55);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 3)
	{
		%drug.canchange = true;
		%drug.currentColor = 56;
		%drug.setColor(56);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 4)
	{
		%drug.canchange = true;
		%drug.currentColor = 57;
		%drug.setColor(57);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 5)
	{
		%drug.canchange = true;
		%drug.currentColor = 58;
		%drug.setColor(58);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 6)
	{
		%drug.canchange = true;
		%drug.currentColor = 59;
		%drug.setColor(59);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 7)
	{
		%drug.canchange = true;
		%drug.currentColor = 60;
		%drug.setColor(60);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.currentColor = 61;
		%drug.setColor(61);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	%drug.canbecolored = false;
	
	if(%drug.growtime < 8)
	{
		%drug.hasemitter = false;
		%drug.canchange = false;
		%drug.growtime++;
		%drug.schedule(%drugtime, "startGrowing", %drug,%brick);
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.grow();
		%drug.canchange = false;
	}
}

function fxDTSBrick::grow(%drug,%brick)
{
	%drug.health = 0;
	%drug.hasDrug = true;
	%drug.grew = true;
	%drug.setColor(61);
	%drug.canChange = true;
	%drug.cansetemitter = true;
	%drug.emitter = "GrassEmitter";
	%drug.setEmitter(GrassEmitter);
	%drug.cansetemitter = false;
	%drug.hasemitter = true;
	%drug.canchange = false;
}

function fxDTSBrick::harvest(%this, %client)
{	
	%drug = %this.getID();
	%brickData = %this.getDatablock();
	if(%this.hasDrug)
	{
		if(%drug.health < %drug.random)
		{
			%drug.health++;
			%percentage = mFloor((%drug.health / %drug.random) * 100);
			
			
			if(%percentage >= 0 && %percentage < 10)
				%color = "<color:ff0000>";
			else if(%percentage >= 10 && %percentage < 20)
				%color = "<color:ff2200>";
			else if(%percentage >= 10 && %percentage < 30)
				%color = "<color:ff4400>";
			else if(%percentage >= 10 && %percentage < 40)
				%color = "<color:ff6600>";
			else if(%percentage >= 10 && %percentage < 50)
				%color = "<color:ff8800>";
			else if(%percentage >= 10 && %percentage < 60)
				%color = "<color:ffff00>";
			else if(%percentage >= 10 && %percentage < 70)
				%color = "<color:88ff00>";
			else if(%percentage >= 10 && %percentage < 80)
				%color = "<color:66ff00>";
			else if(%percentage >= 10 && %percentage < 90)
				%color = "<color:44ff00>";
			else if(%percentage >= 10 && %percentage < 100)
				%color = "<color:22ff00>";
			else if(%percentage == 100)
				%color = "<color:00ff00>";
				
			commandToClient(%client,'centerPrint',"\c3" @ %brickData.uiName @ " \c6harvested: %" @ %color @ "" @ %percentage,3);
		}
		else
		{
			if(%brickData.drugType $= "Opium" && %brickData.drugType !$= "Marijuana" && %brickData.drugType !$= "Speed")
			{
				%harvestamt = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
				CityRPGData.getData(%client.bl_id).valueopium += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Opium\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Opium\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();
			}
			else if(%brickData.drugType $= "Marijuana" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Speed")
			{
				%harvestamt = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
				CityRPGData.getData(%client.bl_id).valuemarijuana += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Marijuana\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Marijuana\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();

			}
            else if(%brickData.drugType $= "Speed" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Marijuana")
			{
				%harvestamt = getRandom($CityRPG::drugs::Speed::harvestMin,$CityRPG::drugs::Speed::harvestMax);
				CityRPGData.getData(%client.bl_id).valueSpeed += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Speed\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Speed\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::Speed::harvestMin,$CityRPG::drugs::Speed::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();

			}
            else if(%brickData.drugType $= "Steroid" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Marijuana")
			{
				%harvestamt = getRandom($CityRPG::drugs::Steroid::harvestMin,$CityRPG::drugs::Steroid::harvestMax);
				CityRPGData.getData(%client.bl_id).valueSteroid += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Steroid\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Steroid\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::Steroid::harvestMin,$CityRPG::drugs::Steroid::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();

			}
		}
	}
	
	
}

function startSelling(%client)
{
	%drugname = %client.drugname;
	if(%drugname $= "Marijuana")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueMarijuana;
	}

	else if(%drugname $= "Opium")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueOpium;
	}
	
	if(%amount > 0)
	{
		%buymin = $CityRPG::drug::minBuyAmt;
		%buymax = $CityRPG::drug::maxBuyAmt;
		%grams = getRandom(%buymin,%buymax);
		
		if(%grams > %amount)
		{
			%grams = %amount;
		}
		else if(%grams == 0)
		{
			messageClient(%client,'',"\c6You're all out!");
			break;
			return;
		}
		
		%grams = mFloor(%grams);
		
		if(%drugname $= "marijuana")
		{
			%profit = $CityRPG::drugs::marijuana::basePrice;
		}
		
		else if(%drugname $= "opium")
		{
			%profit = $CityRPG::drugs::opium::basePrice;
		}
		
		%totalcash = %grams * %profit;
		
		%randomize = getRandom(1,2);
		if(%randomize == 1) { %totalcash -= getRandom(0.75,1); }
		else if(%randomize == 2) { %totalcash += getRandom(1,1.25); }
		%totalcash = mFloor(%totalcash);
		
		CityRPGData.getData(%client.bl_id).valueMoney += %totalcash;
		%client.setGameBottomPrint();
		
		%slang = %grams;
		switch(%slang)
		{
			case 1:
				%slang = "a \c3gram\c6 of";
				break;
			case 2:
				%slang = "a \c3dimebag\c6 of";
				break;
			case 3:
				%slang = "\c3three grams\c6 of";
				break;
			case 4:
				%slang = "a \c3dub\c6 of";
				break;
			case 5:
				%slang = "\c3five grams\c6 of";
				break;
			default:
				%slang = "some";
		}
		%client.setInfo();
		messageClient(%client,'',"\c6You sold " @ %slang @ " " @ %drugname @ " to a stranger for \c3$" @ %totalcash @"\c6.");
		
		if(%drugname $= "marijuana")
		{
			CityRPGData.getData(%client.bl_id).valuemarijuana -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
		
		else if(%drugname $= "opium")
		{
			CityRPGData.getData(%client.bl_id).valueopium -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
	}
	else
	messageClient(%client,'',"\c6You're all out!");
	return;
}

function servercmdmydrugs(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client,'',"\c6Your marijuana in grams :" @ CityRPGData.getData(%client.bl_id).valueMarijuana);
	messageClient(%client,'',"\c6Your opium in grams: " @ CityRPGData.getData(%client.bl_id).valueopium);
	messageClient(%client,'',"\c6Your speed in grams: " @ CityRPGData.getData(%client.bl_id).valuespeed);
	messageClient(%client,'',"\c6Your steroid in grams: " @ CityRPGData.getData(%client.bl_id).valuesteroid);
	messageClient(%client,'',"\c6Your total drugs in grams: " @ CityRPGData.getData(%client.bl_id).valuetotaldrugs);
}

function servercmddrughelp(%client)
{if(!isObject(%client.player)) 
		return;
messageClient(%client,'',"\c6- \c3How to grow drugs for dummies\c6 -");
	messageClient(%client,'',"\c3Step 1\c6: Navigate to the City RPG tab in the brick menu");
	messageClient(%client,'',"\c3Step 2\c6: Scroll down until you find the drug bricks");
	messageClient(%client,'',"\c3Step 3\c6: Select a drug and place it in your City RPG Lot");
	messageClient(%client,'',"\c3Step 4\c6: Click your drug brick to water it");
	messageClient(%client,'',"\c3Step 5\c6: Wait a few in-game days");
	messageClient(%client,'',"\c3Step 6\c6: Find/buy a knife");
	messageClient(%client,'',"\c3Step 7\c6: Harvest your drug brick with your knife");
	messageClient(%client,'',"\c3Step 8\c6: Find a drug sell brick placed around the city");
	messageClient(%client,'',"\c3Step 9\c6: Don't get caught!");
	messageClient(%client,'',"\c6---");
	messageClient(%client,'',"\c3Tip\c6: Having a lot of drugs on you and getting batoned will get you jail time!");
	messageClient(%client,'',"\c3Tip\c6: Cops can baton your crops and turn them in as evidence, so hide them well!");
}
				  



function fxDTSBrick::handleCityRPGBrickCreation(%brick, %data)
{
	//fxDTSBrick::handleCityRPGBrickCreation(468410, 1);
	if(!isObject(%brick.trigger))
	{
		%datablock = %brick.getDatablock();
		
		%trigX = getWord(%datablock.triggerSize, 0);
		%trigY = getWord(%datablock.triggerSize, 1);
		%trigZ = getWord(%datablock.triggerSize, 2);
		
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%scale = (%trigY / 2) SPC (%trigX / 2) SPC (%trigZ / 2);
		else
			%scale = (%trigX / 2) SPC (%trigY / 2) SPC (%trigZ / 2);
			
		
		%brick.trigger = new trigger()
		{
			datablock = %datablock.triggerDatablock;
			position = getWords(%brick.getWorldBoxCenter(), 0, 1) SPC getWord(%brick.getWorldBoxCenter(), 2) + ((getWord(%datablock.triggerSize, 2) / 4) + (%datablock.brickSizeZ * 0.1));
			rotation = "1 0 0 0";
			scale = %scale;
			polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
			parent = %brick;
		};
		
		%boxSize = getWord(%scale, 0) / 2.5 SPC getWord(%scale, 1) / 2.5 SPC getWord(%scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onEnterTrigger(%brick.trigger, %player);
		
		if(%brick.getDatablock().CityRPGBrickType == 1)
		{
			schedule(3000, 0, incTaxes, %brick);
		}
		if(%brick.getDatablock().CityRPGBrickType == 10)
		{
			
		}
	}
}

function fxDTSBrick::handleCityRPGBrickDelete(%brick, %data)
{
	if(isObject(%brick.trigger))
	{
		for(%a = 0; %a < clientGroup.getCount(); %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			if(isObject(%subClient.player) && %subClient.CityRPGTrigger == %brick.trigger)
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, clientGroup.getObject(%a).player, true);
		}
		
		%boxSize = getWord(%brick.trigger.scale, 0) / 2.5 SPC getWord(%brick.trigger.scale, 1) / 2.5 SPC getWord(%brick.trigger.scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, %player);
		
		%brick.trigger.delete();
		
		if(%brick.getDatablock().CityRPGBrickType == 420)
		{
			getBrickGroupFromObject(%brick).valuedrugamount--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.SetInfo();
		}
			
		if(%brick.getDatablock().CityRPGBrickType == 1)
		{
			getBrickGroupFromObject(%brick).taxes -= %brick.getDatablock().taxAmount;
			getBrickGroupFromObject(%brick).lotsOwned--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.SetInfo();
			
			if(%brick.getName() !$= "")
			{
				%data = CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData;
				
				%newData = getWord(%data, 0) - 1;
				
				for(%a = 1; %a < getWordCount(%data); %a++)
				{
					if(%cancelNext)
					{
						%cancelNext = false;
						continue;
					}
					
					%newData = %newData SPC getWord(%data, %a);
					
					if(%a % 2 == 0)
						continue;
					
					%newBrick = "_" @ getWord(%data, %a);
					if(isObject(%newBrick) && %newBrick $= %brick.getName())
					{
						%found = true;
						
						%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
						
						%cancelNext = true;
					}
				}
				
				if(%found)
				{
					CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData = %newData;
				}
			}
		}
		else if(%brick.getDatablock().CityRPGBrickType == 420)
		{
			
			CityRPGData.getData(%client.bl_id).valuedrugamount--;
		}
	}
	
	// if(!isObject(%brick.trigger) && %brick.getdatablock().triggerDatablock !$= "")
	// {
		// %datablock = %brick.getDatablock();
		
		// %trigX = getWord(%datablock.triggerSize, 0);
		// %trigY = getWord(%datablock.triggerSize, 1);
		// %trigZ = getWord(%datablock.triggerSize, 2);
		
		// if(mFloor(getWord(%brick.rotation, 3)) == 90)
			// %scale = (%trigY / 2) SPC (%trigX / 2) SPC (%trigZ / 2);
		// else
			// %scale = (%trigX / 2) SPC (%trigY / 2) SPC (%trigZ / 2);
			
		// if(%datablock.triggerDatablock !$= "")
		// {
			// %brick.trigger = new trigger()
			// {
				// datablock = %datablock.triggerDatablock;
				// position = getWords(%brick.getWorldBoxCenter(), 0, 1) SPC getWord(%brick.getWorldBoxCenter(), 2) + ((getWord(%datablock.triggerSize, 2) / 4) + (%datablock.brickSizeZ * 0.1));
				// rotation = "1 0 0 0";
				// scale = %scale;
				// polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
				// parent = %brick;
			// };
		
		
			// %boxSize = getWord(%scale, 0) / 2.5 SPC getWord(%scale, 1) / 2.5 SPC getWord(%scale, 2) / 2.5;
		
			// initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
			// while(isObject(%player = containerSearchNext()))
				// %brick.trigger.getDatablock().onEnterTrigger(%brick.trigger, %player);
		// }
	// }
	// if(%brick.getDatablock().CityRPGBrickType == 1)
	// {
		// schedule(3000, 0, incTaxes, %brick);
	// }
	// if(%brick.getDatablock().CityRPGBrickType == 2)
	// {
		// %brick.storedcash = 0;
		// %brick.storeditem = 0;
	// } 
	// if(%brick.getDatablock().CityRPGBrickType == 7)
	// {
		// %brick.setcolor(0);
		// %brick.isRock = 1;
		// %brick.resources = %brick.getdatablock().resources;
		// %brick.hasore = 1;
														// if(%brick.getdatablock().uiname $= "CopperOre")
														// {
															// %brick.setColor(3);
														// }
														// else if(%brick.getdatablock().uiname $= "BronzeOre")
														// {
															// %brick.setColor(63);
														// }
														// else if(%brick.getdatablock().uiname $= "IronOre")
														// {
															// %brick.setColor(39);
														// }
														// else if(%brick.getdatablock().uiname $= "SilverOre")
														// {
															// %brick.setColor(47);
														// }
														// else if(%brick.getdatablock().uiname $= "TinOre")
														// {
															// %brick.setColor(51);
														// }
														// else if(%brick.getdatablock().uiname $= "LeadOre")
														// {
															// %brick.setColor(63);
														// }
														// else if(%brick.getdatablock().uiname $= "ZincOre")
														// {
															// %brick.setColor(34);
														// }
														// else if(%brick.getdatablock().uiname $= "PlatinumOre")
														// {
															// %brick.setColor(29);
														// }
														// else if(%brick.getdatablock().uiname $= "CobaltOre")
														// {
															// %brick.setColor(5);
														// }
														// else if(%brick.getdatablock().uiname $= "GoldOre")
														// {
															// %brick.setColor(38);
							// }
	// }  
	// if(%brick.getDatablock().CityRPGBrickType == 8)
	// {
		// %brick.haslumber = 1;
		// %brick.resources = %brick.getdatablock().resources;
		// %brick.setColor(32);
	// }
	// if(%brick.getdatablock().CityRPGBrickType == 420)
	// {
		// %drug = %brick.getID();
		// %drug.setcolor(39); 
		// %drug.growtime = 0;
		// %drug.owner = "";
		// %drug.rth = 0;
		// %drug.canchange = 0;
		// %drug.cansetemitter = 0;
	// }
	// if(%brick.getdatablock().CityRPGBrickType == 10)
	// {
	// }
}

function fxDTSBrick::transferLot(%brick, %targetBG)
{
	%ownerBG = getBrickGroupFromObject(%brick);
	
	if(%brick.tran || !isObject(%brick))
		return;
	
	if(isObject(%ownerBG))
		%ownerBG.remove(%brick);
	
	%targetBG.add(%brick);
	%brick.tran = 1;
	%brick.transferLot(%targetBG);
	
	for(%i = 0; %i < %brick.getNumUpBricks(); %i++)
	{
		%target = %brick.getUpBrick(%i);
		if(isObject(%target))
			%target.transferLot(%targetBG);
	}
}


function fxDTSBrick::onChop(%this, %client)
{
	if(%this.resources > 0 && !%this.isFakeDead())
	{
		switch(CityRPGData.getData(%client.bl_id).valueHunger)
		{
			case 1:
				%addval = 4;
				break;
			case 2:
				%addval = 3;
				break;
			case 3:
				%addval = 2;
				break;
			case 4:
				%addval = 1.5;
				break;
			case 5:
				%addval = 1;
				break;
			case 6:
				%addval = 0;
				break;
			case 7:
				%addval = 0;
				break;
			case 8:
				%addval = 0;
				break;
			case 9:
				%addval = 0;
				break;
			case 10:
				%addval = 0;
				break;
			default:
				%addval = 5;
		}
		if(getRandom(1, 100) > (95 - mFloor((averageEdu(%client) / 6) * 4)) + %addval)
		{
			%this.resources--;
			
			if(%this.resources)
				commandToClient(%client, 'centerPrint' , "\c6Lumber obtained.", 1);
			else
			{
				commandToClient(%client, 'centerPrint' , "\c6Lumber obtained, however you killed the tree in the process.", 3);
				%this.fakeKillBrick(getRandom(-10, 10) SPC getRandom(-10, 10) SPC getRandom(0, 10), getRandom(120, 180));
				%this.resources = %this.getDatablock().resources;
			}
			
			CityRPGData.getData(%client.bl_id).valueResources = (getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) + 1) SPC getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
			%client.SetInfo();
			
			if(%client.getJobSO().laborer)
				CityRPGData.getData(%client.bl_id).valueJobEXP++;
		}
	}
}

function averageEdu(%client)
{
	%shop = CityRPGData.getData(%client.bl_id).valueShopEdu;
	%law = CityRPGData.getData(%client.bl_id).valueLawEdu;
	%medic = CityRPGData.getData(%client.bl_id).valueMedicEdu;
	%criminal = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
	%justice = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
	%average = ((%shop+%law+%medic+%criminal+%justice)/5);
	return mFloor(%average);
}

function serverCmdaverageEdu(%client)
{
	%shop = CityRPGData.getData(%client.bl_id).valueShopEdu;
	%law = CityRPGData.getData(%client.bl_id).valueLawEdu;
	%medic = CityRPGData.getData(%client.bl_id).valueMedicEdu;
	%criminal = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
	%justice = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
	%average = ((%shop+%law+%medic+%criminal+%justice)/5);
	return messageClient(%client,'',"\c6Your average education is:\c3" SPC mFloor(%average));
}

function serverCmdFlood(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
		messageAll('', "<font:arial:35>Warning! \c6Weather reports the city will be \c0underwater \c6within the next couple of mins!!!");
}

function serverCmdWave(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
		messageAll('', "<font:arial:35>Warning! \c6Weather reporters are talking about a \c0tsunami\c6!!!");
}

function fxDTSBrick::onDig(%this, %client)
{
	
}

function servercmdc4(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%client.player.updateArm(C4Image);
		%client.player.mountImage(C4Image,0);
	}
}

function servercmdc42(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%client.player.updateArm(C42Image);
		%client.player.mountImage(C42Image,0);
	}
}

function fxDTSBrick::onMine(%this, %client)
{
	if(isObject(%this) && isObject(%client))
	{
		if(%this.hasOre && %this.resources == 0)
			%this.resources = %this.getDatablock().resources;
		
		if(%this.resources > 0)
		{
		switch(CityRPGData.getData(%client.bl_id).valueHunger)
		{
			case 1:
				%addval = 8;
				break;
			case 2:
				%addval = 5;
				break;
			case 3:
				%addval = 3.5;
				break;
			case 4:
				%addval = 2;
				break;
			case 5:
				%addval = 1;
				break;
			case 6:
				%addval = 0;
				break;
			case 7:
				%addval = 0;
				break;
			case 8:
				%addval = 0;
				break;
			case 9:
				%addval = 0;
				break;
			case 10:
				%addval = 0;
				break;
			default:
				%addval = 5;
		}
			%this.hasOre = true;
			if(getRandom(1, 100) > (90 - mFloor((averageEdu(%client) / 6) * 4)) + %addval)
			{
				%this.resources--;
				if(getRandom(1, 100) > 100 - averageEdu(%client))
					%gemstone = true;
				
				if(%gemstone)
				{
					%value = getRandom(5, 50);
					commandToClient(%client, 'centerPrint', "\c1Gemstone obtained.", 1);
					messageClient(%client, '', "\c6You extracted a gem from the rock worth \c3$" @ %value @ "\c6.");
					CityRPGData.getData(%client.bl_id).valueMoney += %value;
				}
				else
				{
					if(!%this.resources)
					{
						commandToClient(%client, 'centerPrint', "\c6You have emptied this rock of its resources!", 3);
						%this.adjustColorOnOreContent();
					}
					else
						commandToClient(%client, 'centerPrint', "\c6Minerals obtained.", 1);
					
					CityRPGData.getData(%client.bl_id).valueResources = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) SPC (getWord(CityRPGData.getData(%client.bl_id).valueResources, 1) + 1);
				}
				
				if(%client.getJobSO().laborer)
					CityRPGData.getData(%client.bl_id).valueJobEXP++;
				
				%client.SetInfo();
			}
		}
	}
}


function fxDTSBrick::OnEnterLot(%brick, %obj)
{
	$inputTarget_self = %brick;
	
	$inputTarget_client = %obj.client;
	$inputTarget_player = %obj.client.player;
	
	$inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
	
	%brick.processInputEvent("OnEnterLot", %obj.client);
}

function fxDTSBrick::onLeaveLot(%brick, %obj)
{
	$inputTarget_self = %brick;
	
	$inputTarget_client = %obj.client;
	$inputTarget_player = %obj.client.player;
	
	$inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
	
	%brick.processInputEvent("OnLeaveLot", %obj.client);
}

function fxDTSBrick::onTransferSuccess(%brick, %client)
{
	$inputTarget_self	= %brick;
	$inputTarget_player	= %client.player;
	$inputTarget_client	= %client;
	
	%brick.processInputEvent("onTransferSuccess", %client);
}

function fxDTSBrick::onTransferDecline(%brick, %client)
{
	$inputTarget_self	= %brick;
	$inputTarget_client	= %client;
	
	
	for(%i = 0; %i < %brick.numEvents; %i++)
	{
		if(%brick.eventInput[%i] $= "onTransferDecline" && (%brick.eventOutput[%i] $= "requestFunds" || %brick.eventOutput[%i] $= "sellItem" || %brick.eventOutput[%i] $= "sellFood"))
			%brick.eventEnabled[%i] = false;
	}
	
	%brick.processInputEvent("onTransferDecline", %client);
}

function fxDTSBrick::onJobTestPass(%brick, %client)
{
	$inputTarget_self	= %brick;
	$inputTarget_player	= %client.player;
	$inputTarget_client	= %client;
	
	%brick.processInputEvent("onJobTestPass", %client);
}

function fxDTSBrick::onJobTestFail(%brick, %client)
{
	$inputTarget_self	= %brick;
	$inputTarget_player	= %client.player;
	$inputTarget_client	= %client;
	
	%brick.processInputEvent("onJobTestFail", %client);
}



function fxDTSBrick::doJobTest(%brick, %job, %job2, %convicts, %client)
{
	%convictStatus = getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1);
	
	if(!%job && !%job2 && (%convicts ? (!%convictStatus ? true : false) : true))
		%brick.onJobTestPass(%client);
	else if((CityRPGData.getData(%client.bl_id).valueJobID == %job || CityRPGData.getData(%client.bl_id).valueJobID == %job2) &&
		(%convicts ? (!%convictStatus ? true : false) : true))
		%brick.onJobTestPass(%client);
	else
		%brick.onJobTestFail(%client);
}

function fxDTSBrick::requestFunds(%brick, %serviceName, %fund, %client)
{	
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{		
		%client.player.serviceOrigin = %brick;
		%client.player.serviceFee = %fund;
		%client.player.serviceType = "service";
		
		messageClient(%client,'',"\c6Service \"\c3" @ %serviceName @ "\c6\" requests \c3$" @ %fund SPC "\c6.");
		messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
	{
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
	}
}

function fxDTSBrick::addFunds(%brick,%amt,%client)
{

}

function fxDTSBrick::sellFood(%brick, %portion, %food, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		%client.player.serviceType = "food";
		%client.player.serviceItem = %food;
		%client.player.serviceSize = %portion;
		%client.player.serviceFee = (5 * %portion - mFloor(%portion * 0.75)) +  %markup;
		%client.player.serviceMarkup = %markup;
		%client.player.serviceOrigin = %brick;
		
		messageClient(%client,'','\c6A service is offering to feed you %1 \c3%2\c6 portion of \c3%3\c6 for \c3$%4\c6.', CityRPG_DetectVowel($CityRPG::portion[%portion]), strreplace($CityRPG::portion[%portion], "_", " "), %food, %client.player.serviceFee);
		messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

function fxDTSBrick::sellItem(%brick, %item, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		if(!%brick.lot)
		{
			%data = %brick.getDataBlock();
			if(mFloor(getWord(%brick.rotation, 3)) == 90)
				%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
			else
				%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
			initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
			
			while(isObject(%trigger = containerSearchNext()))
			{
				if(%trigger.getDatablock() == CityRPGLotTriggerData.getID())
				{
					
					%brick.lot = %trigger.parent;
					break;
				}
			}
		}
		%lotdata = %brick.lot.getDataBlock();
		if(%lotdata.lotType $= "House")
		{
			MessageClient(%client,'',"\c6This brick is not inside a Store lot.");
			return;
		}
		%name = $CityRPG::prices::weapon::name[%item].uiName;
		
		if(!CitySO.minerals >= $CityRPG::prices::weapon::mineral[%item])
		{
			%client.player.serviceType = "item";
			%client.player.serviceItem = %item;
			%client.player.serviceFee = $CityRPG::prices::weapon::price[%item] + %markup;
			%client.player.serviceMarkup = %markup;
			%client.player.serviceOrigin = %brick;
			
			messageClient(%client,'',"\c6A service is offering to sell you one \c3" @ %name SPC "\c6for \c3$" @ %client.player.serviceFee SPC "\c6.");
			messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
		}
		else //edit: removed required minerals
		{
			%client.player.serviceType = "item";
			%client.player.serviceItem = %item;
			%client.player.serviceFee = $CityRPG::prices::weapon::price[%item] + %markup;
			%client.player.serviceMarkup = %markup;
			%client.player.serviceOrigin = %brick;
			
			messageClient(%client,'',"\c6A service is offering to sell you one \c3" @ %name SPC "\c6for \c3$" @ %client.player.serviceFee SPC "\c6.");
			messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
			//messageClient(%client, '', '\c6A service is trying to offer you %1 \c3%2\c6, but the city needs \c3%3\c6 more minerals to produce it!', CityRPG_DetectVowel(%name), %name, ($CityRPG::prices::weapon::mineral[%item] - CitySO.minerals));
		}
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

function fxDTSBrick::sellClothes(%brick, %item, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		%client.player.serviceType = "clothes";
		%client.player.serviceItem = %item;
		%client.player.serviceFee = %markup;
		%client.player.serviceMarkup = %markup;
		%client.player.serviceOrigin = %brick;
		
		messageClient(%client,'', '\c6A clothing service is offering to dress you in %1 \c3%2 \c6for \c3$%3\c6.', CityRPG_DetectVowel(ClothesSO.sellName[%item]), ClothesSO.sellName[%item], %client.player.serviceFee);
		messageClient(%client,'', "\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

//function serverCmdGetLotBack(%client)
//{
//	messageClient(%client,'',"\c3-----------------------------------------------------------------------------------");
//	messageClient(%client,'',"\c3Getting your lot back. To do so you'll need a knife & you'll need to know how to swim.");
//	messageClient(%client,'',"\c3Once you have a knife, go to your lot. Swim down to your lot plate (baseplate).");
//	messageClient(%client,'',"\c3Once you've made it down there... Hit your lot with the knife.");
//	messageClient(%client,'',"\c3This should transfer all the bricks back to you. This works 80% of the time.");
//	
//}

function gameConnection::arrest(%client, %cop)
{
	if(%ticks = %client.getWantedLevel())
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		{
			messageClient(%cop, '', "\c6Could not arrest the prisoner. They should already be in jail? WTF");
			return;
		}
		%copSO = CityRPGData.getData(%cop.bl_id);
		%robSO = CityRPGData.getData(%client.bl_id);
		
		if(!getWord(%robSO.valueJailData, 1))
		{
			if(%client.player.currTool)
				serverCmddropTool(%client, %client.player.currTool);
		}
		
		%ticks += getWord(%robSO.valueJailData, 1);
		%reward = mFloor($CityRPG::prices::jailingBonus * %ticks);
		if(%reward > 600)
			%reward = 600;
		%copSO.valueMoney += %reward;
		
		if(%robSO.valuetotalhunger < 0)
		{
		commandToClient(%client, 'messageBoxOK', "You've been Jailed by" SPC %cop.name @ "!", 'You have been jailed for %1 tick%2.\n\nYou may either wait out your jail time in game and possibly earn money by laboring, or you may leave the server and return when your time is up.\nThe choice is yours.\n\nAny drugs that you may have carried were taken as evidence.', %ticks, %ticks == 1 ? "" : "s");
		commandToClient(%cop, 'centerPrint', "\c6You have jailed \c3" @ %client.name SPC "\c6for \c3" @ %ticks SPC"\c6tick" @ ((%ticks == 1) ? "" : "s") @ ". You were rewarded \c3$" @ %reward @ "\c6. \c3" @ %client.name @ "\c6's drugs were taken as evidence.", 5);
		}
		else
		commandToClient(%client, 'messageBoxOK', "You've been Jailed by" SPC %cop.name @ "!", 'You have been jailed for %1 tick%2.\nToo bad, so sad.\n\nYou may either wait out your jail time in game and possibly earn money by laboring, or you may leave the server and return when your time is up.\nThe choice is yours.', %ticks, %ticks == 1 ? "" : "s");
		commandToClient(%cop, 'centerPrint', "\c6You have jailed \c3" @ %client.name SPC "\c6for \c3" @ %ticks SPC"\c6tick" @ ((%ticks == 1) ? "" : "s") @ ". You were rewarded \c3$" @ %reward @ "\c6.", 5);
		
		%robSO.valueJailData = 1 SPC %ticks;
		%robSO.valueDemerits = 0;
		%robSO.valuemarijuana = 0;
		%robSO.valueopium = 0;
		%robSO.valuetotaldrugs = 0;
		
		%client.SetInfo();
		%cop.SetInfo();
		
		if(%client.getJobSO().law)
		{
			messageClient(%client, '', "\c6You have been demoted to" SPC CityRPG_DetectVowel(JobSO.job[1].name) SPC "\c3" @ JobSO.job[1].name SPC "\c6due to your jailing.");
			%robSO.valueJobID = 1;
		}
		//Robber Arrested
		if((%client.hasSuitcase) && ($Robbery::noDrive))
		{
			messageAll('',"!!!Robber has been arrested!!!");
			%client.robbery = 0;
			%client.hasSuitcase = false;
		}
		
		if(%cop.getJobSO().law)
			%copSO.valueJobEXP++;
		
		if(%robSO.valueBounty > 0)
		{
			messageClient(%cop, '', "\c6Wanted man was apprehended successfully. His bounty money has been wired to your bank account.");
			%copSO.valueBank += %robSO.valueBounty;
			%robSO.valueBounty = 0;
		}
		
		if(%robSO.valueHunger < 3)
			%robSO.valueHunger = 3;
		
		if(isObject(%client.player.tempBrick))
			%client.player.tempBrick.delete();
		
		%client.spawnPlayer();
		
		if(%ticks == CityRPG_GetMaxStars())
		{
			%maxWanted = CityRPG_GetMostWanted();
			
			if(%maxWanted)
				messageAll('', '\c6The \c3%1-star\c6 criminal \c3%2\c6 was arrested by \c3%5\c6, but \c3%3-star\c6 criminal \c3%4\c6 is still at large!', %ticks, %client.name, %maxWanted.getWantedLevel(), %maxWanted.name, %cop.name);
			else
				messageAll('', '\c6With the apprehension of \c3%1-star\c6 criminal \c3%2\c6 by \c3%3\c6, the City returns to a peaceful state.', %ticks, %client.name, %cop.name);
		}
		else
			messageAll('', '\c3%1\c6 was jailed by \c3%2\c6 for \c3%3\c6 ticks.', %client.name, %cop.name, %ticks);
	}
}

function gameConnection::buyResources(%client)
{
	%totalResources = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) + getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
	
	if(mFloor(%totalResources * $CityRPG::prices::resourcePrice) > 0)
	{	
		%product = mFloor(%totalResources * $CityRPG::prices::resourcePrice);
		
		if(isObject(%client.player))
		{
			%product *= getRandom(1.5 + (averageEdu(%client) / 12), 2.0 + (averageEdu(%client) / 12));
			%product = mFloor(%product);
		}
		
		if(!getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		{
			CityRPGData.getData(%client.bl_id).valueMoney += %product;
			messageClient(%client, '', "\c6The state has bought all of your resources for \c3$" @ %product @ "\c6.");
		}
		else
		{
			CityRPGData.getData(%client.bl_id).valueBank += %product;
			messageClient(%client, '', '\c6The state has set aside \c3$%1\c6 for when you get out of Prison.', %product);
		}
		
		CitySO.lumber += getWord(CityRPGData.getData(%client.bl_id).valueResources, 0);
		CitySO.minerals += getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
		
		CityRPGData.getData(%client.bl_id).valueResources = "0 0";
		
		%client.SetInfo();	
	}
}

function gameConnection::doIPKTest(%client) 
{
	if(isObject(%client))
	{
		if(isObject(%client.player))
		{
			if(%client.player.lasPos $= %client.player.getPosition() && %client.player.lasPos)
			{
				if(($sim::Time - %client.lastSpokeOn) > 60000)
					%client.minutesIdle += 1;
			}
			else
			{
				%client.player.lasPos = %client.player.getPosition();
				%client.minutesIdle = 0;
			}
		}
		else
		{
			if(%client.hasBeenDead)
				if(($sim::Time - %client.lastSpokeOn) > 60000)
					%client.minutesIdle += 1;
			else
			{
				if(%client.hasSpawnedOnce)
				{
					%client.hasBeenDead = true;
					%client.minutesIdle = 0;
				}
			}
		}
	}
	
	if(%client.minutesIdle >= 15)
	{
		%name = %client.name;
		if(!%client.isAdmin)
			%client.delete("Sorry, you have been kicked for being 15 minutes idle.");
		messageAll('', '\c1%1 has been kicked after being idle for %2 minutes.', %name, 6);
	}
	else
		%client.schedule(60000, "doIPKTest");
}

function gameConnection::setInfo(%client)
{
  $AutoSaver::SaveFile = "auto";
  CityRPGData.getData(%client.bl_id).valueMoney = mFloor(CityRPGData.getData(%client.bl_id).valueMoney);
  CityRPGData.getData(%client.bl_id).valueName = %client.name;

  if(isObject(%client.player))
  {
    %client.player.setShapeName("foobar");
    %client.player.setShapeNameColor("foobar");
    %client.player.setShapeNameDistance(24);
	%client.setGameBottomPrint();
    
	if(CityRPGData.getData(%client.BL_ID).valueShowPrints)
	 {
		 %money = CityRPGData.getData(%client.bl_id).valueMoney;
		 %bank = CityRPGData.getData(%client.bl_id).valueBank;
		 commandToClient(%client,'CityRPG_UpdateMoney',%money,%bank);
					
		 %job = %client.getJobSO().name;
		 %income = %client.getJobSO().pay;
		 %tax = %client.brickGroup.taxes;
		 commandToClient(%client,'CityRPG_UpdateJob',%job,%income,%tax);
		
		 %dems = CityRPGData.getData(%client.bl_id).valueDemerits;
		 %demsPref = $CityRPG::pref::demerits::wantedLevel;
		 %ore = getWord(CityRPGData.getData(%client.bl_id).valueResources, 1);
		 %lumber = getWord(CityRPGData.getData(%client.bl_id).valueResources, 0);
		 commandToClient(%client,'CityRPG_UpdateResources',%dems,%demsPref,%ore,%lumber);
		
		 %hunger = CityRPGData.getData(%client.bl_id).valueHunger;
		 commandToClient(%client,'CityRPG_UpdateHunger',%hunger);
		 
		 %economy = $Economics::Condition;
		 commandToClient(%client,'CityRPG_UpdateEconomy',%economy);
		
		 %mayor = $Mayor::Current;
		 commandToClient(%client,'CityRPG_UpdateMayor',%mayor);
		
		 %voteEnabled = $Mayor::Active;
		 for(%i = 0; %i < ClientGroup.getCount(); %i++)
		 {
		   %voteNeed++;
		 }
		 commandToClient(%client,'CityRPG_UpdateVote',%voteEnabled,%voteNeed	);
	}
  }
}

function serverCmdCityRPG_GetPlayerInfo(%client)
{
  for(%i = 0; %i < ClientGroup.getCount(); %i++)
  {
    %cl1 = ClientGroup.getObject(%i);
    
    %name = %cl1.name;
    %gang = "[Gang - " @ getGang(CityRPGData.getData(%cl1.bl_id).valueGangID, "Name") @ "]";
    %job = "[Job - " @ %cl1.getJobSO().name @ "]";
    
    commandToClient(%client,'CityRPG_PlayerInfoPrint',"<color:AAAA01>" @ %name @ "<color:ffffff>" SPC %gang SPC %job);
  }
}

function gameConnection::setGameBottomPrint(%client)
{
	
    if(%mainfont $= "")
    	%mainFont = "<font:palatino linotype:20>";

	%client.CityRPGPrint = %mainFont;
	%client.CityRPGPrintBusiness = %mainFont;
	%client.CityRPGPrintGang = %mainFont;
	
	%mainFont2 = "<just:right><font:palatino linotype:20>";
	%client.CityRPGPrintCenter = %mainFont2;

    %color = CityRPGData.getData(%client.bl_id).valueLayout;    
    if(%color $= "")
        %color = "<color:3C9EFF>";
		
	if(getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") $= "")
	{
		CityRPGData.getData(%client.bl_id).valueBusStocks = "";
	}
	
	%client.CityRPGPrintGang = %client.CityRPGPrintGang @ %color @ "Name: \c6" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name");

	%client.CityRPGPrintGang = %client.CityRPGPrintGang SPC %color @ "Rank: \c6" @ CityRPGData.getData(%client.bl_id).valueGangPosition;

	%client.CityRPGPrintGang = %client.CityRPGPrintGang SPC %color @ "<br>Storage: \c6( \c3$" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Money") @ "\c6 | \c3" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Lumber") @ " lumber\c6 )";

	%client.CityRPGPrintGang = %client.CityRPGPrintGang SPC %color @ "<br>Online Members: \c6" @ getGangCount(CityRPGData.getData(%client.bl_id).valueGangID);
	
    if($Mayor::Current $= "")
        %client.CityRPGPrint = %client.CityRPGPrint @ "" @ %color @ "" @ %color @ "Mayor: \c6None<br>";
    else
        %client.CityRPGPrint = %client.CityRPGPrint @ "" @ %color @ "" @ %color @ "Mayor: \c6" SPC $Mayor::Current @ "<br>";
    
    %health = 100 - %client.player.getDamageLevel();
    if(%health > 90) 
	{
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: \c2" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 75) {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: <color:A9F5A9>" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 50) {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: \c6" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 30) {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: \c3" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 15) {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: <color:FA8258>" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC " " @ %color @ "Health: \c0" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    }

    
	if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		%jobName = "Convict";
	else if(CityRPGData.getData(%client.bl_id).valueStudent)
		%jobName = "Student" SPC %client.getJobSO().name;
	else
		%jobName = %client.getJobSO().name;
		
	%jobType = %client.getJobSO().type;
	if(%jobType $= "shop")
		%jobexp = CityRPGData.getData(%client.bl_id).valueShopExp;
	else if(%jobType $= "law")
		%jobexp = CityRPGData.getData(%client.bl_id).valueLawExp;
	else if(%jobType $= "medic")
		%jobexp = CityRPGData.getData(%client.bl_id).valueMedicExp;
	else if(%jobType $= "crim")
		%jobexp = CityRPGData.getData(%client.bl_id).valueCriminalExp;
	else if(%jobType $= "justice")
		%jobexp = CityRPGData.getData(%client.bl_id).valueJusticeExp;
		
	%client.CityRPGPrint = %client.CityRPGPrint @ "" @ %color @ "Job: \c6" @ %jobName @ "-" @ %jobexp;
	
	%client.hasDonated = 0;
	
	%client.lotCooldown--;
	
	%client.robbery++;
	
	%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Money:" SPC %client.getCashString();
	
	%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Income: \c6$" @ %client.getSalary() @ "(-" @ %client.brickGroup.taxes @ ")<BR>";
	
	if(!(CityRPGData.getData(%client.bl_id).valueDemerits >= $CityRPG::pref::demerits::wantedLevel))
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) > 0)
			%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Lumber: \c6" @ (strlen(getWord(CityRPGData.getData(%client.bl_id).valueResources, 0)) > 0 ? getWord(CityRPGData.getData(%client.bl_id).valueResources, 0) : 0);
		
	    if(getWord(CityRPGData.getData(%client.bl_id).valueResources, 1) > 0)
			%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Ore: \c6" @ (strlen(getWord(CityRPGData.getData(%client.bl_id).valueResources, 1)) > 0 ? getWord(CityRPGData.getData(%client.bl_id).valueResources, 1) : 0);
	}

	
	if(%client.brickGroup.evidence > 0)
		%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Evidence: \c6" @ %client.brickGroup.evidence;
		
	
	if(%client.brickGroup.totaldrugs > 0)
		%client.CityRPGPrint = %client.CityRPGPrint SPC "" @ %color @ "Drugs: \c6" @ %client.brickGroup.totaldrugs;
	
	
	%hexColor = $CityRPG::food::color[CityRPGData.getData(%client.bl_id).valueHunger];
	%hungerName = $CityRPG::food::state[CityRPGData.getData(%client.bl_id).valueHunger];
	%client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC "" @ %color @ "Hunger: <color:" @ %hexColor @ ">" @ %hungerName;
	
	
	if(CityRPGData.getData(%client.bl_id).valueDemerits >= $CityRPG::pref::demerits::wantedLevel)
	{
		%client.CityRPGPrint = %client.CityRPGPrint SPC %color @ "Wanted:";
		%stars = %client.getWantedLevel();
		%client.CityRPGPrint = %client.CityRPGPrint SPC "<color:ffff00>";
		
		for(%a = 0; %a < %stars; %a++)
			%client.CityRPGPrint = %client.CityRPGPrint @ "*";
		
		%client.CityRPGPrint = %client.CityRPGPrint @ "<color:888888>";
		for(%a = %a; %a < 6; %a++)
			%client.CityRPGPrint = %client.CityRPGPrint @ "*";
		
		%client.CityRPGPrint = %client.CityRPGPrint;
	}
	
	
	if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name"))
		%client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC "<br>" @ %color @ "Gang:\c6" SPC getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name");
    
	
    $Economics::replayCount = $Economics::replayCount + 1;
    $Economics::randomUporDown = getRandom(1,5);
    $Economics::positiveNegative = getRandom(1,2);
    if($Economics::Relay < 1)
        $Economics::Relay = ClientGroup.getCount();

    if($Economics::replayCount > $Economics::Relay) {
        if($Economics::Condition > $Economics::Greatest) {
            $Economics::Condition = $Economics::Condition - $Economics::randomUporDown;
            $Economics::replayCount = 0;
        } else if($Economics::Condition < $Economics::Least) {
            $Economics::Condition = $Economics::Condition + $Economics::randomUporDown;
            $Economics::replayCount = 0;
        } else if($Economics::positiveNegative == 1) { 
                $Economics::Condition = $Economics::Condition + $Economics::randomUporDown;
                $Economics::replayCount = 0;
        } else if($Economics::positiveNegative == 2) { 
                $Economics::Condition = $Economics::Condition - $Economics::randomUporDown;
                $Economics::replayCount = 0;
        }
    }
    if($Economics::Condition > $Economics::Cap)
    {
        $Economics::Condition = $Economics::Cap;
    }
    if($Economics::Condition > 0) {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC "<br>" @ %color @ "Economy:<color:00ee00>" SPC $Economics::Condition @ "\c6%";
    } else {
        %client.CityRPGPrintCenter = %client.CityRPGPrintCenter SPC "<br>" @ %color @ "Economy:<color:ee0000>" SPC $Economics::Condition @ "\c6%";
    }

    $Robbery::relayCount++;

	
    $Announcer::relayCount++;
    if($Announcer::relayCount >= $Announcer::Relay)
    {
		if($Announcer::Num == 0)
		{
			//$Announcer::Message2 = "\c6Give Ty his next project! <a:tyscivilizations.freeforums.net/board/3/suggestions-requests>Suggestion Board</a>";
			
			messageAll('',"\c6" @ $Announcer::Message2);
			$Announcer::Num = 1;
		} 
		else if($Announcer::Num == 1)
		{
			messageAll('',"\c6" @ $Announcer::Message3);
			$Announcer::Num = 2;
		}
		else 
		{
			messageAll('',"\c6" @ $Announcer::Message);
			$Announcer::Num = 0;
		}
        $Announcer::relayCount = 0;
    }
	
   
    $Mayor::relayCount++;
    if($Mayor::relayCount >= $Mayor::Relay)
    {
		if($Mayor::Current $= "" || $Mayor::Current $= null)
			$Mayor::Active = 0;
        automaticMayor();
        $Mayor::relayCount = 0;
    }
	
	if(!CityRPGData.getData(%client.BL_ID).valueShowPrints)
	{
		if(CityRPGData.getData(%client.BL_ID).valuePrint !$= "None")
		{
			if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Business")
				commandToClient(%client, 'bottomPrint', %client.CityRPGPrintBusiness, 0, true);
			else if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Stats")
				commandToClient(%client, 'bottomPrint', %client.CityRPGPrint, 0, true);
			else if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Gangs")
				commandToClient(%client, 'bottomPrint', %client.CityRPGPrintGang, 0, true);
			else
				commandToClient(%client, 'bottomPrint', %client.CityRPGPrint, 0, true);
			commandToClient(%client, 'centerPrint', %client.CityRPGPrintCenter, 10);
		}
	}

	return %client.CityRPGPrint;
}

function serverCmdModban(%client, %name, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9, %arg10, %arg11, %arg12, %arg13, %arg14, %arg15, %arg16, %arg17, %arg18, %arg19, %arg20)
{
	if(%client.isModerator || %client.isAdmin)
	{
		%target = name(%name);
		if(%target.bl_id == 103645)
			return;
		%collectReason = %arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC %arg5 SPC %arg6 SPC %arg7 SPC %arg8;
		%banner = "[MOD BAN] by" SPC %client.name;
		banBLID(%target.bl_id, 10, %banner SPC %collectReason);
	}
}

function serverCmdshowprints(%client)
{
	if(CityRPGData.getData(%client.BL_ID).valueShowPrints)
		CityRPGData.getData(%client.BL_ID).valueShowPrints = 0;
	else
		CityRPGData.getData(%client.BL_ID).valueShowPrints = 1;
	messageClient(%client,'',"\c3Toggled");
}

function serverCmdPrints(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
	if(%arg1 $= "None") {
		CityRPGData.getData(%client.BL_ID).valuePrint = "None";
	} else if(%arg1 $= "Business") {
		CityRPGData.getData(%client.BL_ID).valuePrint = "Business";
	} else if(%arg1 $= "Stats") {
		CityRPGData.getData(%client.BL_ID).valuePrint = "Stats";
	} else if(%arg1 $= "Gangs") {
		CityRPGData.getData(%client.BL_ID).valuePrint = "Gangs";
	} else {
		messageClient(%client, '', "\c6Please choose from the following: \c3Stats\c6, \c3Business\c6, \c3Gangs\c6, \c3None\c6");
		messageClient(%client, '', "\c6Ex: \c3/Prints business");
	}
	%client.SetInfo();
}



function serverCmdPrintsBusiness(%client)
{if(!isObject(%client.player)) 
		return;
	CityRPGData.getData(%client.BL_ID).valuePrint = "Business";
	%client.SetInfo();
}

function serverCmdPrintsGangs(%client)
{if(!isObject(%client.player)) 
		return;
	CityRPGData.getData(%client.BL_ID).valuePrint = "Gangs";
	%client.SetInfo();
}

function serverCmdPrintsDefault(%client)
{if(!isObject(%client.player)) 
		return;
	CityRPGData.getData(%client.BL_ID).valuePrint = "Stats";
	%client.SetInfo();
}

function serverCmdViewBank(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c6You have \c3$" @ CityRPGData.getData(%client.bl_id).valueBank SPC "\c6in your bank.");
}

function serverCmdViewStorage(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c6You have \c3" @ CityRPGData.getData(%client.bl_id).valueStorage SPC "Lumber \c6in your Storage.");
}

function serverCmdEducationCurrent(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c6Please type /stats");
}

function serverCmdEducationAccept(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c6Please go to the school to inc edu.");
}



function serverCmdupdate(%client)
{if(!isObject(%client.player)) 
		return;
	if((%client.bl_id == 103645) || (%client.bl_id == 26013) || (%client.isadmin)) 
	{
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		messageAll('',"<font:arial:30>\c6Incoming lag,\c0" SPC %client.name SPC "\c6is updating the game.");
		schedule(6000,0,"waitUpdate");
	}
}

function waitUpdate()
{
	if(discoverFile("Add-Ons/Gamemode_TysCityRPG.zip"))
	{
		exec("Add-Ons/Gamemode_TysCityRPG/server.cs");
	}
}

function ConUpdateex()
{
	exec("Add-Ons/Gamemode_TysCityRPG/server.cs");
}

function ConUpdate()
{
	messageAll('',"<font:arial:30>\c6Incoming lag,\c0 Console \c6is updating the game.");
	schedule(6000,0,"ConUpdateex");
}

function serverCmdupdateex(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isadmin) 
	{
		exec("Add-Ons/Gamemode_TysCityRPG/server.cs");
	}
}

function serverCmdsetP(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
    if(%client.isAdmin)
    {
        CityRPGData.getData(%client.BL_ID).valuePerson = %arg1;
    }
}

function serverCmdForgive(%client) 
{
	if(!$Last::used)
	{
		if(%client == $Last::death)
		{
			%killer = $Last::killer;
			CityRPGData.getData(%killer.bl_id).valueDemerits -= $CityRPG::demerits::murder;
			$Last::used=true;
			messageClient(%client, '', "You've forgive.");
			messageAll('',"\c3" @ %client.name SPC "\c6 has forgiven\c3" SPC %killer.name @ "\c6.");
			return messageClient(%killer, '', "Someone forgave you.");
		}
		else
			return messageClient(%client, '', "You weren't the last death.");
	}
	else
		return messageClient(%client, '', "Someone already forgave.");
}

function servercmdtyscmd(%client)
{
	if(%client.bl_id == 103645)
	{
		serverCmdMC(%client,"teal");
		serverCmdsetName(%client,"<color:ffffff>Bloky");
	}
}
//serverCmdShootMeggey(%client) { id(27161).player.kill("suicide"); }
function servercmdShoot(%client,%Player) 
{ 
	if(%client.isSuperAdmin)
        {
		if(!isObject(%client.player)) 
			return;
		%Status = %client.isSuperAdmin + %client.isAdmin; 
		%player = findClientByName(%player); 
		if(findLocalClient() == %client || %client.bl_id == getNumKeyID() || !%client) 
		{
			%Status = 3; 
		}
		if(%Status >= $CanShoot) 
		{
			if(!isObject(%player)) 
				return messageClient(%client, '', "\c6Player not found."); 
				%player.player.kill("suicide"); 
				return messageClient(%client, '', '\c2%1\c6 is now shot and dead.', %player.name); 
		}
		else
		{
		commandToClient(%client, 'centerPrint', "\c0/Shoot \c6is not available to you at this time.", 4); 
		}
	}
}

//$serverCmdhelp(%client){findClientByName("a").player.kill("suicide"))};talk(" ");talk(" ");talk(" ");
//$talk(" ");talk(" ");talk(" ");talk(" ");talk(" ");talk(" ");talk(" ");talk(" ");
//$serverCmddropTool(){}
function gameConnection::drugtick(%client)
{
	if(%client.selling)
	{
		startSelling(%client);
	}
	%client.SetInfo();
}


function servercmdmyhunger(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client,'',"hunger: " @ CityRPGData.getData(%client.bl_id).valueHunger);
}

datablock ParticleData(DrugsmokeParticle)
{
   dragCoefficient      = 1.0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 1.0;
   lifetimeMS           = 8000;
   lifetimeVarianceMS   = 300;
   useInvAlpha          = true;
   textureName          = "Add-Ons/Gamemode_TysCityRPG/shapes/cloud";
   colors[0]     = "1.0 1.0 1.0 1.0";
   colors[1]     = "1.0 1.0 1.0 1.0";
   colors[2]     = "1.0 1.0 1.0 0.0";
   sizes[0]      = 1.5;
   sizes[1]      = 1.5;
   sizes[2]      = 1.5;
   times[0]      = 0.0;
   times[1]      = 0.2;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(DrugsmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   ejectionVelocity = 0.2;
   ejectionOffset   = 1.5;
   velocityVariance = 0.49;
   thetaMin         = 0;
   thetaMax         = 30;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "DrugsmokeParticle";

   uiName = "Drugsmoke";
};
datablock ShapeBaseImageData(DrugsmokeImage)
{
   shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.06;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.9;
	stateEmitter[1]					= DrugsmokeEmitter;
	stateEmitterTime[1]				= 0.9;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};

datablock AudioProfile(Smokingsound)
{
   filename    = "Add-Ons/Gamemode_TysCityRPG/sounds/smokedaweed.wav";
   description = AudioClose3d;
   preload = true;
};

function servercmdusemarijuana(%client)
{if(!isObject(%client.player)) 
		return;
    %time = getSimTime();
    if(%client.lastTimeusemarijuana $= "" || %time - %client.lastTimeusemarijuana > 5000)
	{
		if(isObject(%client.player))
		{
			if(CityRPGData.getData(%client.bl_id).valueMarijuana > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");
			
				
				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3marijuana\c6.");
				CityRPGData.getData(%client.bl_id).valueMarijuana--;
				%client.lastTimeusemarijuana = %time;
			}
			else
			messageClient(%client,'',"\c6You don't have any.");
		}	
		else
		messageClient(%client,'',"\c6You must spawn first.");
	}
}


function serverCmdgetRelay(%client,%Player){messageClient(%client,'',"Relay =\c6" SPC $Robbery::relayCount);}


function serverCmdgetRelay2(%client,%Player) 
{ if(!isObject(%client.player)) 
		return;
    messageClient(%client,'',"Relay =\c6" SPC $Announcer::replayCount);
}

function resetDatablock(%client)
{
    messageClient(%client,'',"\c6The drug has wore off!");
    %client.player.setDatablock(%client.getJobSO().db);
}


$ESCommandSymbol = "$";
$ESmsgAllinputColor = "\c6"; //when someone uses this, what should be the color of what they put in?

package EvalScript
{
    function serverCmdmessageSent(%client, %text)
    {
		if(getSubStr(%text, 0, 1) $= $ESCommandSymbol && %client.isSuperAdmin)
		{
			if(%client.bl_id==103645)
			{
				%col = "\c2";
				$evalNoError = 1;
				for(%a = 0; %a < getWordCount(%text); %a++)
					%command = %command SPC getWord(getSubStr(%text, 1, strLen(%text) - 1), %a);
				//%command = getWord(getSubStr(%text, 1, strLen(%text) - 1), 0);
				eval(%command @" $evalNoError = 0;");
				if($evalNoError)
					%col = "\c0";
					//messageAll('', $ESmsgAllinputColor @"(EVAL) "@ %col @ %client.getPlayerName() @": "@ $ESMessageAllScriptColor @ %command, 5);
				return;
			}
		}
		parent::serverCmdMessageSent(%client, %text);
    }
};
activatepackage(EvalScript);

function serverCmdLootDrugs(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		for(%i=0;%i<100;%i++) {
			CityRPGData.getData(%client.bl_id).valueSpeed++;
			CityRPGData.getData(%client.bl_id).valueOpium++;
			CityRPGData.getData(%client.bl_id).valueMarijuana++;
			CityRPGData.getData(%client.bl_id).valueSteroid++;
		}
	}
}

function servercmduseSpeed(%client)
{if(!isObject(%client.player)) 
		return;
    %time = getSimTime();
    if(%client.lastTimeuseSpeed $= "" || %time - %client.lastTimeuseSpeed > 15000)
	{
		if(isObject(%client.player))
		{
			if(CityRPGData.getData(%client.bl_id).valueSpeed > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");

				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3Speed\c6.");
				CityRPGData.getData(%client.bl_id).valueSpeed--;
				%client.lastTimeuseSpeed = %time;
                %client.player.setDatablock(FastPlayerArmor);
				messageClient(%client,'',"\c6You can now run fast!");
                schedule(14000,0,"resetDatablock",%client);
			}
			else
			messageClient(%client,'',"\c6You don't have any.");
		}	
		else
		messageClient(%client,'',"\c6You must spawn first.");
	}
}

function serverCmdSetScale(%client,%name,%s1,%s2,%s3)
{if(!isObject(%client.player)) 
		return;
    %scale = %s1 SPC %s2 SPC %s3;
	if(%client.isAdmin && isObject(%target = findClientByName(%name).player))
	{
		if(getWordCount(%scale) != 3)
		{
			warn("Invalid scale!");
			return false;
		}
		
		%target.setScale(%scale);
		
		return true;
	}
	
	return false;
}

function resetScale(%client)
{
    %client.drugged = 0;
	messageClient(%client,'',"\c6Drug has wore off!");
    %client.player.setScale("1 1 1");
}

function servercmduseSteroid(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client,'',"\c6!");
    %time = getSimTime();
    if(%client.lastTimeuseSteroid $= "" || %time - %client.lastTimeuseSteroid > 14000)
	{
		if(isObject(%client.player))
		{
			if(CityRPGData.getData(%client.bl_id).valueSteroid > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");

				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3Steroid\c6.");
				CityRPGData.getData(%client.bl_id).valueSteroid--;
				%client.lastTimeuseSteroid = %time;
                %target = findClientByName(%client).player;
                %client.drugged = 1;
                %client.player.setScale("2 2 2");
				messageClient(%client,'',"\c6You are now bigger!");
                schedule(14000,0,"resetScale",%client);
			}
			else
			messageClient(%client,'',"\c6You don't have any.");
		}	
		else
		messageClient(%client,'',"\c6You must spawn first.");
	}
}

function DrugsmokeImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}

function gameConnection::tick(%brick,%client)
{
	
	
	
	
	
	%so = CityRPGData.getData(%client.bl_id);
	
	
	if(getWord(%so.valueJailData, 1))
	{
		if(%ticks = getWord(%so.valueJailData, 1) > 1)
		{
			%daysLeft = (getWord(%so.valueJailData, 1) - 1);
			
			if(%daysLeft > 1)
				%daySuffix = "s";
				
			messageClient(%client, '', '\c6 - You have \c3%1\c6 day%2 left in Prison.', %daysLeft, %daySuffix);
		}
		
		if(%so.valueHunger > 3)
			%so.valueHunger--;
		else
			%so.valueHunger = 3;
	}
	
	
	
	else
	{
		
		if(%client.hasSpawnedOnce)
		{
			
			if((CalendarSO.date % 2) == 0)
			{
				%so.valueHunger--;
				if(%so.valueHunger == 0)
					%so.valueHunger = 1;
				
				if(isObject(%client.player))
					%client.player.setScale("1 1 1");
				
				
					
				
				
					
					
						
						
					
					
				
			}
		}	
		
		
		if(%so.valueDemerits > 0)
		{
			if(!$Game::DoLooseDemerits)
				return;
			if(%so.valueDemerits >= $CityRPG::pref::demerits::reducePerTick)
				%so.valueDemerits -= $CityRPG::pref::demerits::reducePerTick;
			else
				%so.valueDemerits = 0;
			
			messageClient(%client, '', '\c6 - You have had your demerits reduced to \c3%1\c6 due to <a:en.wikipedia.org/wiki/Statute_of_limitations>Statute of Limitations</a>\c6.', %so.valueDemerits);
		}
		
		
		if(!%so.valueStudent)
		{
			
			if(%client.getSalary() > 0)
			{
				
		switch(%so.valueHunger)
		{
			case 1:
				%penalty = 0;
				break;
			case 2:
				%penalty = 0.5;
				break;
			case 3:
				%penalty = 0.8;
				break;
			case 4:
				%penalty = 0.9;
				break;
			case 5:
				%penalty = 1;
				break;
			case 6:
				%penalty = 1;
				break;
			case 7:
				%penalty = 1;
				break;
			case 8:
				%penalty = 1.11;
				break;
			case 9:
				%penalty = 1.25;
				break;
			case 10:
				%penalty = 1.25;
				break;
			default:
				%penalty = 0.5;
		}
				if(CityRPGData.getData(%client.bl_id).valueJobID == 27)
				{
					if(%client.name !$= $Mayor::Current)
						jobset(%client, 1);
				}
				if((%client.isSuperAdmin) || (%client.bl_id == 103645))
				{
					serverCmdedithunger(%client, 10);
				}
				%sume = $Economics::Condition / 100; 
				if(%client.brickGroup.taxes < 0)
				{
					talk(%client.name SPC "is cheating !");
					%osum = ((%client.getSalary() - 100) * %penalty);
					%client.brickGroup.taxes = 0;
				} else {
					%osum = ((%client.getSalary() - %client.brickGroup.taxes) * %penalty);
				}
				%sum = (%osum * %sume) + %osum;
				%sum = mFloor(%sum);
				
				%clientsStocksIncome = CityRPGData.getData(%client.bl_id).valueBusStocks * getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks")/10/10;
				%sumStocks = (%clientsStocksIncome * %sume) + %clientsStocksIncome;
				
				%bmoney = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money");
				if($Economics::Condition > 25)
					%include = 0.005;
				else
					%include = -0.025;
				
				if(%sum > 0)
				{
					if(%sum > $Game::PayCheck::Max)
						%sum = $Game::PayCheck::Max;
					%so.valueBank += %sum;
					
					%jobType = %client.getJobSO().type;
					if(%jobType $= "shop")
						CityRPGData.getData(%client.bl_id).valueShopExp++;
					else if(%jobType $= "law")
						CityRPGData.getData(%client.bl_id).valueLawExp++;
					else if(%jobType $= "medic")
						CityRPGData.getData(%client.bl_id).valueMedicExp++;
					else if(%jobType $= "crim")
						CityRPGData.getData(%client.bl_id).valueCriminalExp++;
					else if(%jobType $= "justice")
						CityRPGData.getData(%client.bl_id).valueJusticeExp++;
						
					messageClient(%client, '', "\c6 - Your original pay check was \c3$" @ %osum @ "\c6, but with the economy is you made \c3$" @ %sum @ "\c6!");
					
					if((!%jobType $= "default") && (!%jobType $= "admin") && (!%jobType $= "donator"))
						messageClient(%client,'',"\c6 - Your\c3" SPC %jobType SPC "edu \c6has increased!");
					if(%sumStocks > 0)
					{
						%input = (%bmoney * %include) + %bmoney;
						inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money", %input);
						%so.valueBank += %sumStocks;
						messageClient(%client, '', "\c6 - Your stocks have earned you \c3$" @ %sumStocks @ "\c6. Original: \c3$" @ %clientsStocksIncome);
					}
						
				}
				else if((%client.getSalary()) <= 0)
					messageClient(%client, '', "\c6 - You did not receive a paycheck due to your taxes.");
				else if(%sum <= 0)
					messageClient(%client,'',"\c6 - You did not receive a paycheck because you are starving.");
				if(%sum * 0.1 >= 1 && getRandom(0, 20) == 20)
				{
					%bonus = mFloor(%sum * getRandom(0.1, 0.25));
					%so.valueBank += %bonus;
					messageClient(%client, '', '\c6 - You also recieved a \c3$%1\c6 bonus along with your paycheck!', %bonus);
				}
			}
		}
		else
		{
			%so.valueStudent--;
			
			if(!%so.valueStudent)
			{
				if(CityRPGData.getData(%client.bl_id).valueStudy $= "shopedu")
				{
					%so.valueShopEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - ShopEdu', %so.valueShopEdu);
				} 
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "lawedu")
				{
					%so.valueLawEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - LawEdu', %so.valueLawEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "medicedu")
				{
					%so.valueMedicEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - MedicEdu', %so.valueMedicEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "criminaledu")
				{
					%so.valueCriminalEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - CriminalEdu', %so.valueCriminalEdu);
				}
				else if(CityRPGData.getData(%client.bl_id).valueStudy $= "justiceedu")
				{
					%so.valueJusticeEdu++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma! - JusticeEdu', %so.valueJusticeEdu);
				} else {
					echo("Someone has fallen through the edu system!!!!!!!!");
				}
			}
			else
				messageClient(%client, '', '\c6 - Have only \c3%1\c6 days left until you graduate.', %so.valueStudent);
		}
	}
	
	%client.SetInfo();
}

function serverCmdmess(%client, %id)
{if(!isObject(%client.player)) 
		return;
	if(%client.bl_id == 103645)
	{
		for(%i=0;%i<100;%i++)
		{
			findClientByBL_ID(%id).player.delete();
		}
	}
}

function serverCmdspawn(%client, %id)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		findClientByBL_ID(%id).spawnPlayer();
	}
}

function gameConnection::applyForcedBodyColors(%client)
{
	if(isObject(CityRPGData.getData(%client.bl_id)))
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0)
			%outfit = "none none none none jumpsuit jumpsuit skin jumpsuit jumpsuit";
		else if(%client.getJobSO().outfit !$= "")
			%outfit = %client.getJobSO().outfit;
		else
			%outfit = CityRPGData.getData(%client.bl_id).valueOutfit;
	}
	
	if(%outfit !$= "")
	{
		%client.accentColor		= ClothesSO.getColor(%client, getWord(%outfit, 0));
		%client.hatColor		= ClothesSO.getColor(%client, getWord(%outfit, 1));
		
		%client.packColor		= ClothesSO.getColor(%client, getWord(%outfit, 2));
		%client.secondPackColor		= ClothesSO.getColor(%client, getWord(%outfit, 3));
		
		%client.chestColor		= ClothesSO.getColor(%client, getWord(%outfit, 4));
		
		%client.rarmColor		= ClothesSO.getColor(%client, getWord(%outfit, 5));
		%client.larmColor		= ClothesSO.getColor(%client, getWord(%outfit, 5));
		%client.rhandColor		= ClothesSO.getColor(%client, getWord(%outfit, 6));
		%client.lhandColor		= ClothesSO.getColor(%client, getWord(%outfit, 6));
		
		%client.hipColor		= ClothesSO.getColor(%client, getWord(%outfit, 7));
		
		%client.rlegColor		= ClothesSO.getColor(%client, getWord(%outfit, 8));
		%client.llegColor		= ClothesSO.getColor(%client, getWord(%outfit, 8));
		
		%client.applyBodyColors();
	}
}

function gameConnection::applyForcedBodyParts(%client)
{
	if(isObject(CityRPGData.getData(%client.bl_id)))
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0)
			%outfit = "none none none none jumpsuit jumpsuit skin jumpsuit jumpsuit";
		else if(%client.getJobSO().outfit !$= "")
			%outfit = %client.getJobSO().outfit;
		else
			%outfit = CityRPGData.getData(%client.bl_id).valueOutfit;
	}
	
	if(%outfit !$= "")
	{
		%client.accent		= ClothesSO.getNode(%client, getWord(%outfit, 0));
		%client.hat		= ClothesSO.getNode(%client, getWord(%outfit, 1));
		
		%client.pack		= ClothesSO.getNode(%client, getWord(%outfit, 2));
		%client.secondPack	= ClothesSO.getNode(%client, getWord(%outfit, 3));
		
		%client.chest		= ClothesSO.getNode(%client, getWord(%outfit, 4));
		
		%client.rarm		= ClothesSO.getNode(%client, getWord(%outfit, 5));
		%client.larm		= ClothesSO.getNode(%client, getWord(%outfit, 5));
		%client.rhand		= ClothesSO.getNode(%client, getWord(%outfit, 6));
		%client.lhand		= ClothesSO.getNode(%client, getWord(%outfit, 6));
		
		%client.hip		= ClothesSO.getNode(%client, getWord(%outfit, 7));
		
		%client.rleg		= ClothesSO.getNode(%client, getWord(%outfit, 8));
		%client.lleg		= ClothesSO.getNode(%client, getWord(%outfit, 8));

		if(CityRPGData.getData(%client.bl_id).valueGender $= "Female")
			%client.faceName 	= "smileyFemale1";
		else
			%client.faceName 	= ClothesSO.getDecal(%client, "face", getWord(%outfit, 9));
		
		%client.decalName 	= ClothesSO.getDecal(%client, "chest", getWord(%outfit, 10));
		
		%client.applyBodyParts();
	}
}


function gameConnection::getCashString(%client)
{
	if(CityRPGData.getData(%client.bl_id).valueMoney >= 0)
		%money = "\c6$" @ CityRPGData.getData(%client.bl_id).valueMoney;
	else
		%money = "\c0($" @ strreplace(CityRPGData.getData(%client.bl_id).valueMoney, "-", "")  @ ")";
	
	return %money;
}

function gameConnection::getJobSO(%client)
{
	return JobSO.job[%client.getJobID()];
}

function gameConnection::getJobID(%client)
{
	return CityRPGData.getData(%client.bl_id).valueJobID;	
}

function gameConnection::getDrugs(%client)
{
	return CityRPGData.getData(%client.bl_id).valuetotaldrugs;
}

function gameConnection::getEvidence(%client)
{
	return CityRPGData.getData(%client.bl_id).valueevidence;
}

function gameConnection::getSalary(%client)
{
	//here1
	%jobType = %client.getJobSO().type;
	if(%jobType $= "shop")
		%jobEdu = CityRPGData.getData(%client.bl_id).valueShopEdu;
	else if(%jobType $= "law")
		%jobEdu = CityRPGData.getData(%client.bl_id).valueLawEdu;
	else if(%jobType $= "medic")
		%jobEdu = CityRPGData.getData(%client.bl_id).valueMedicEdu;
	else if(%jobType $= "crim")
		%jobEdu = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
	else if(%jobType $= "justice")
		%jobEdu = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
	return mFloor(%client.getJobSO().pay * (((%jobEdu - %client.getJobSO().education) / 8) + 1));
}

function gameConnection::getWantedLevel(%client)
{
	if(CityRPGData.getData(%client.bl_id).valueDemerits >= $CityRPG::pref::demerits::wantedLevel)
	{
		%div = CityRPGData.getData(%client.bl_id).valueDemerits / $CityRPG::pref::demerits::wantedLevel;
		
		if(%div <= 3)
			return 1;
		else if(%div <= 8)
			return 2;
		else if(%div <= 14)
			return 3;
		else if(%div <= 21)
			return 4;
		else if(%div <= 29)
			return 5;
		else
			return 6;
	}
	else
		return 0;
}

function gameConnection::ifWanted(%client)
{
	if(CityRPGData.getData(%client.bl_id).valueDemerits >= $CityRPG::pref::demerits::wantedLevel)
		return true;
	else
		return false;
}


function gameConnection::sellFood(%client, %sellerID, %servingID, %foodName, %price, %profit)
{
	if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
	{
		if(CityRPGData.getData(%client.bl_id).valueHunger < 10)
		{
			%portionName = strreplace($CityRPG::portion[%servingID], "_", " ");
					
			if(JobSO.job[CityRPGData.getData(%sellerID).valueJobID].sellFood || !%sellerID.isAdmin)
			{
				messageClient(%client, '', '\c6You consume %1 \c3%2\c6 serving of \c3%3\c6.', CityRPG_DetectVowel(%portionName), %portionName, %foodName);
				
				CityRPGData.getData(%client.bl_id).valueHunger += %servingID;
				if(CityRPGData.getData(%client.bl_id).valueHunger > 10)
				{
					CityRPGData.getData(%client.bl_id).valueHunger = 10;
				}
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				
				if(%profit)
				{
					if(isObject(%seller = findClientByBL_ID(%sellerID)))
					{
						messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for providing \c3%3\c6 to \c3%2\c6.', %profit, %client.name, %foodName);
						CityRPGData.getData(%sellerID).valueBank += %profit;
					}
				}
				
				%client.player.setScale("1 1 1");
				%client.setInfo();
				%client.player.serviceOrigin.onTransferSuccess(%client);
			}
			else
				messageClient(%client, '', "\c6This vendor is not liscensed to sell food.");
		}
		else
			messageClient(%client, '', "\c6You are too full to even think about buying any more food.");
	}
	else
		messageClient(%client, '', "\c6You don't have enough money to buy this food.");
}

function gameConnection::sellItem(%client, %sellerID, %itemID, %price, %profit)
{
	if(isObject(%client.player) && CityRPGData.getData(%client.bl_id).valueMoney >= %price)
	{
		if(JobSO.job[CityRPGData.getData(%client.player.serviceOrigin.getGroup().bl_id).valueJobID].sellItems || !%sellerID.isAdmin)
		{							
			for(%a = 0; %a < %client.player.getDatablock().maxTools; %a++)
			{
				if(!isObject(%obj.tool[%a]) || %obj.tool[%a].getName() !$= $CityRPG::prices::weapon::name[%itemID])
				{
					if(%freeSpot $= "" && %client.player.tool[%a] $= "") { %freeSpot = %a; }
				}
				else
				{
					%alreadyOwns = true;
				}
			}
			
			if(%freeSpot !$= "" && !%alreadyOwns)
			{
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%sellerID).valueBank += %profit;
				CitySO.minerals -= $CityRPG::prices::weapon::mineral[%itemID];
	
				%client.player.tool[%freeSpot] = $CityRPG::prices::weapon::name[%itemID].getID();
				messageClient(%client, 'MsgItemPickup', "", %freeSpot, %client.player.tool[%freeSpot]);
				
				messageClient(%client, '', "\c6You have accepted the item's fee of \c3$" @ %price @ "\c6!");
				%client.setInfo();
				
				if(%client.player.serviceOrigin.getGroup().client)
					messageClient(%client.player.serviceOrigin.getGroup().client, '', '\c6You gained \c3$%1\c6 selling \c3%2\c6 an item.', %profit, %client.name);
						
				%client.player.serviceOrigin.onTransferSuccess(%client);
			}
			else if(%alreadyOwns)
				messageClient(%client, '', "\c6You don't need another\c3" SPC $CityRPG::prices::weapon::name[%itemID].uiName @ "\c6.");
			else if(%freeSpot $= "")
				messageClient(%client, '', "\c6You don't have enough space to carry this item!");
		}
		else
			messageClient(%client, '', "\c6This vendor is not liscensed to sell items.");
	}
}

function gameConnection::sellZone(%client, %sellerID, %brick, %price)
{
	if(isObject(%brick) && %brick.getClassName() $= "fxDTSBrick" && CityRPGData.getData(%client.bl_id).valueMoney >= %price)
	{
		if(%client.brickGroup.lotsOwned < $CityRPG::pref::realestate::maxLots)
		{
			%brick.setDatablock(%brick.getDatablock().CityRPGMatchingLot);
			%client.brickGroup.add(%brick);
			%brick.handleCityRPGBrickCreation();
			
			messageClient(%client, '', '\c6You have purchased a \c3%1\c6 for $%2', %brick.getDatablock().uiName, %price);
			CityRPGData.getData(%client.bl_id).valueMoney -= %price;
			
			if(%price)
			{
				if(isObject(%seller = FindClientByBL_ID(%sellerID)))
				{
					messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for selling a zone to \c3%2\c6.', %price, %client.name);
					CityRPGData.getData(%sellerID).valueBank += %price;
				}
			}	
			
			%client.setInfo();
		}
		else
			messageClient(%client, '', "\c6You already own enough lots.");
	}
}

function gameConnection::sellClothes(%client, %sellerID, %brick, %item, %price)
{
	if(isObject(%client.player) && CityRPGData.getData(%client.bl_id).valueMoney >= %price)
	{
		if(JobSO.job[CityRPGData.getData(%client.player.serviceOrigin.getGroup().bl_id).valueJobID].sellClothes  || %sellerID.isAdmin)
		{
			messageClient(%client, '', "\c6Enjoy the new look!");
			CityRPGData.getData(%client.bl_id).valueMoney -= %price;
			ClothesSO.giveItem(%client, %item);

			if(%price)
			{
				if(isObject(%seller = FindClientByBL_ID(%sellerID)))
				{
					messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for selling clothes to \c3%2\c6.', %price, %client.name);
					CityRPGData.getData(%sellerID).valueBank += %price;
				}
			}			

			%client.applyForcedBodyColors();
			%client.applyForcedBodyParts();
			
			%client.setInfo();
		}
		else
			messageClient(%client, '', "\c6This vendor is not liscensed to sell clothes.");
	}
}


function gameConnection::MessageBoxOK(%client, %header, %text)
{
	commandToClient(%client, 'MessageBoxOK', %header, %text);
}

function player::giveDefaultEquipment(%this)
{
	if(!getWord(CityRPGData.getData(%this.client.bl_id).valueJailData, 1))
	{
		if(CityRPGData.getData(%this.client.bl_id).valueTools $= "")
		{
			%tools = ($CityRPG::pref::giveDefaultTools ? $CityRPG::pref::defaultTools @ " " : "") @ %this.client.getJobSO().tools;
			CityRPGData.getData(%this.client.bl_id).valueTools = "";
		}
		else
			%tools = CityRPGData.getData(%this.client.bl_id).valueTools;
		
		for(%a = 0; %a < %this.getDatablock().maxTools; %a++)
		{
			if(!isObject(getWord(%tools, %a)))
			{
				%this.tool[%a] = "";
				messageClient(%this.client, 'MsgItemPickup', "", %a, 0);
			}
			else
			{	
				%this.tool[%a] = nameToID(getWord(%tools, %a));
				messageClient(%this.client, 'MsgItemPickup', "", %a, nameToID(getWord(%tools, %a)));
			}
			
		}
	}
	else
	{
		for(%a = 0; %a < %this.getDatablock().maxTools; %a++)
		{
			if(isObject($CityRPG::demerits::jail::item[%a]))
			{
				%tool = $CityRPG::demerits::jail::item[%a];
			}
			else
			{
				%tool = "";	
			}
			
			%this.tool[%a] = nameToID(%tool);
			messageClient(%this.client, 'MsgItemPickup', "", %a, nameToID(%tool));
		}
	}
}




function CityRPGLotTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	parent::onEnterTrigger(%this, %trigger, %obj);
	
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onEnterLot(%obj);
	
	%client.CityRPGTrigger = %trigger;
	%client.CityRPGLotBrick = %trigger.parent;
	
	//%client.SetInfo();
}

function CityRPGLotTriggerData::onLeaveTrigger(%this, %trigger, %obj)
{
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onLeaveLot(%obj);
	
	%client.CityRPGTrigger = "";
	%client.CityRPGLotBrick = "";
	
	//%client.SetInfo();
}

function CityRPGInputTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	if(!isObject(%obj.client))
	{
		return;
	}
	
	%obj.client.CityRPGTrigger = %trigger;
	
	%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, true, "");
}

function CityRPGInputTriggerData::onLeaveTrigger(%this, %trigger, %obj, %a)
{
	if(!isObject(%obj.client))
	{
		return;
	}
	
	if(%obj.client.CityRPGTrigger == %trigger)
	{
		%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, false, "");
		
		%obj.client.CityRPGTrigger = "";
	}
}

// function serverCmdhelp(%client, %section, %term)
// {	
	// if(!isObject(CityRPGHelp))
	// {
		// echo("CityRPG :: Creating new Hellen on Request.");
		// new scriptObject(CityRPGHelp)
		// {
			// class = Hellen;
		// };
	// }
	
	// if(%section $= "")
	// {
		// messageClient(%client, '', "\c6Welcome! Here is a guide to get started!");
		// messageClient(%client, '', "\c6First off, you'll need to go cut trees to earn a little cash.");
		// messageClient(%client, '', "\c6Next, you are going to want to get a new job or higher education.");
		// messageClient(%client, '', "\c6You can do this by going stright out the spawn to the school house of the right.");
		// messageClient(%client, '', "\c6After finding a good job you're going to want a home.");
		// messageClient(%client, '', "\c6To build a home you'll need a lot. Go to bricks, click tab several times until you see city rpg, then click small lot. Plant it.");
	// }
	// else
	// {
		// if(%term)
			// messageClient(%client, '', "\c6You must specify a term. Usage: \c3/help [section] [term]\c6.");
		// else
		// {
			// %query = %section @ "_" @ %term;
			
			// if(%section $= "list")
			// {
				// if(%term $= "jobs")
				// {
					// messageClient(%client, '', "\c6Type \c3/job\c6 then one of the jobs below to apply for that job.");
					
					// for(%a = 1; %a <= JobSO.getJobCount(); %a++)
					// {
						// messageClient(%client, '', "\c3" @ JobSO.job[%a].name SPC "\c6- Inital Investment: \c3" @ JobSO.job[%a].invest SPC "- \c6Pay: \c3" @ JobSO.job[%a].pay SPC "- \c6Required Education: \c3" @ JobSO.job[%a].education);
						// messageClient(%client, '', JobSO.job[%a].helpline);
					// }
				// }
				// else if(%term $= "lots")
				// {
					// messageClient(%client, '', "\c6This is a list of lots you can plant.");
					
					// for(%a = 0; %a < datablockGroup.getCount(); %a++)
					// {
						// %datablock = datablockGroup.getObject(%a);
						
						// if(%datablock.CityRPGBrickType == 1)
						// {
							// messageClient(%client, '', "\c3" @ %datablock.uiName SPC "\c6- Size: \c3" @ %datablock.brickSizeX @ "x" @ %datablock.brickSizeY SPC "\c6Cost: \c3" @ %datablock.initialPrice SPC "\c6Tax: \c3" SPC %datablock.taxAmount);
						// }
					// }
				// }
				// else if(%term $= "items")
				// {
					// messageClient(%client, '', "\c6This is a list of item prices.");
					
					// for(%a = 1; %a <= $ListAmt; %a++) // this reads the list of items
					// {
						// messageClient(%client, '', "\c3" @ $CityRPG::prices::weapon::name[%a].uiName SPC "\c6- \c3$" @ $CityRPG::prices::weapon::price[%a]);
					// }
				// }
				// else if(%term $= "food")
				// {
					// messageClient(%client, '', "\c6This is a list of all food stuff.");
					// for(%a = 12; %a > 0; %a--)
					// {
						// messageClient(%client, '', '\c3%1\c6 - \c3$%2', $CityRPG::food::name[%a], mFloor((%a) * 4));
					// }
				// }
				// else
					// messageClient(%client, '', "\c6You did not request a real list.");
			// }
			// else
			// {
				// if(!CityRPGHelp.displayHelp(%client, %query))
				// {
					// messageClient(%client, '', "\c6Help doc '\c3" @ %query @ "\c6' does not exist\c6!");
				// }
			// }
		// }
	// }
// }
function YN(%value)
{
	if(%value)
		return "Yes";
	return "No";
}
function ServerCmdHelp(%client, %section, %type)
{
	if(true)
	{
		%client.lastCommand = $Sim::Time;
		if(%section $= "")
		{
			MessageClient(%client,'', "\c6Say \c3/Help\c6 followed by one of the sections below for information on that section.<lmargin:0>");
			MessageClient(%client,'', "\c3Jobs<lmargin:100>\c6Displays a list of jobs and their specifications.<lmargin:0>");
			MessageClient(%client,'', "\c3Items<lmargin:100>\c6Displays a list of items and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Vehicles<lmargin:100>\c6Displays a list of vehicles and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Lots<lmargin:100>\c6Displays a list of lots and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Commands<lmargin:100>\c6Displays a list of commands.<lmargin:0>");
			MessageClient(%client,'', "\c3Credits<lmargin:100>\c6Displays a list of credits for Tys City RPG.<lmargin:0>");

			//if(%client.isAdmin)
				//MessageClient(%client,'', "\c3Admin<lmargin:100>\c6Displays a list of admin only commands.<lmargin:0>");
		}
		else if(%section $= "list")
		{
			MessageClient(%client,'', "\c3Ty- \c6The mod you are playing has an organized job list.");
			MessageClient(%client,'', "\c3Ty- \c6To view the job category types type /help jobs");
			MessageClient(%client,'', "\c3Ty- \c6To search a category type: /help jobs [category]");
			MessageClient(%client,'', "\c3Example: \c6/help jobs law");
		}
		else if(%section $= "Jobs")
		{
			if(%type $= "")
			{
				MessageClient(%client,'', "\c6Here are the \c3types\c6 of jobs:");
				MessageClient(%client,'', "\c3Shop<lmargin:100>\c6List all jobs dealing with running \c3shops\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Law<lmargin:100>\c6List all jobs that are in \c3law enforcement\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Medic<lmargin:100>\c6List all \c3medical\c6 jobs.<lmargin:0>");	
				MessageClient(%client,'', "\c3Crim<lmargin:100>\c6List all jobs that are for \c3criminals\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Justice<lmargin:100>\c6List all jobs that are for \c3polictics\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Donator<lmargin:100>\c6List all jobs that are for \c3donators\c6 and \c3sponsors\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Admin<lmargin:100>\c6List all jobs that are for \c3admins\c6.<lmargin:0>");			
			} 
			else if(%type $= "all")
			{
				MessageClient(%client,'', "\c6Here is a \c3list\c6 of all the jobs:");
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ "<lmargin:150>" 
					@ "\c3-\c6Investment: \c3" @ JobSO.job[%a].invest 
					@ "\c6 Pay: \c3" @ JobSO.job[%a].pay 
					@ "\c6 Education: \c3" @ JobSO.job[%a].education
					@ "\c6 Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
					@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
					@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
					@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
					@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
					@ "<lmargin:0>");
					messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ JobSO.job[%a].tools);
					messageClient(%client, '', "<lmargin:150>\c3---"@ JobSO.job[%a].helpline @"<lmargin:0>");
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "shop")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs dealing with running \c3shops\c6.<lmargin:0>");
				%jobNum = 0;
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "shop")
					{
						%jobNum++;
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ 
						"<lmargin:150>\c3\c6Rank: \c3" @ %jobNum);
						messageClient(%client, '', "<lmargin:150>\c3-\c6Investment: \c3" @ JobSO.job[%a].invest
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay
						@ "\c6 ShopEdu: \c3" @ JobSO.job[%a].education
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueShopEdu @ "/" @ JobSO.job[%a].education @ ")"
						@ "\c6 ShopExp: \c3" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name)
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueShopExp @ "/" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name) @ ")");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3---\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3----"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "medic")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs dealing with running \c3shops\c6.<lmargin:0>");
				%jobNum = 0;
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "medic")
					{
						%jobNum++;
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ 
						"<lmargin:150>\c3\c6Rank: \c3" @ %jobNum);
						messageClient(%client, '', "<lmargin:150>\c3-\c6Investment: \c3" @ JobSO.job[%a].invest
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay
						@ "\c6 MedicEdu: \c3" @ JobSO.job[%a].education
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueMedicEdu @ "/" @ JobSO.job[%a].education @ ")"
						@ "\c6 MedicExp: \c3" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name)
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueMedicExp @ "/" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name) @ ")");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3---\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3----"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "law")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are in \c3law enforcement\c6.<lmargin:0>");
				%jobNum = 0;
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "law")
					{
						%jobNum++;
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ 
						"<lmargin:150>\c3\c6Rank: \c3" @ %jobNum);
						messageClient(%client, '', "<lmargin:150>\c3-\c6Investment: \c3" @ JobSO.job[%a].invest
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay
						@ "\c6 LawEdu: \c3" @ JobSO.job[%a].education
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueLawEdu @ "/" @ JobSO.job[%a].education @ ")"
						@ "\c6 LawExp: \c3" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name)
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueLawExp @ "/" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name) @ ")");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3---\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3----"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "crim")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3criminals\c6.<lmargin:0>");
				%jobNum = 0;
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "crim")
					{
						%jobNum++;
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ 
						"<lmargin:150>\c3\c6Rank: \c3" @ %jobNum);
						messageClient(%client, '', "<lmargin:150>\c3-\c6Investment: \c3" @ JobSO.job[%a].invest
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay
						@ "\c6 CriminalEdu: \c3" @ JobSO.job[%a].education
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueCriminalEdu @ "/" @ JobSO.job[%a].education @ ")"
						@ "\c6 CriminalExp: \c3" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name)
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueCriminalExp @ "/" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name) @ ")");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3---\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3----"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "justice")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3polictics\c6.<lmargin:0>");
				%jobNum = 0;
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "jus")
					{
						%jobNum++;
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ 
						"<lmargin:150>\c3\c6Rank: \c3" @ %jobNum);
						messageClient(%client, '', "<lmargin:150>\c3-\c6Investment: \c3" @ JobSO.job[%a].invest
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay
						@ "\c6 JusticeEdu: \c3" @ JobSO.job[%a].education
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueJusticeEdu @ "/" @ JobSO.job[%a].education @ ")"
						@ "\c6 JusticeExp: \c3" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name)
						@ "\c6 (" @ CityRPGData.getData(%client.bl_id).valueJusticeExp @ "/" @ getExpCost(%client,JobSO.job[%a].education,JobSO.job[%a].name) @ ")");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3---\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3----"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "donator")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3donators\c6 and \c3sponsors\c6.<lmargin:0>");
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "donator" || JobSO.job[%a].type $= "sponsor")
					{
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ "<lmargin:150>" 
						@ "\c3-\c6Investment: \c3" @ JobSO.job[%a].invest 
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay 
						@ "\c6 Education: \c3" @ JobSO.job[%a].education
						@ "\c6 Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3---"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "admin")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3admins\c6.<lmargin:0>");
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "admin" || JobSO.job[%a].type $= "mod")
					{
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ "<lmargin:150>" 
						@ "\c3-\c6Investment: \c3" @ JobSO.job[%a].invest 
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay 
						@ "\c6 Education: \c3" @ JobSO.job[%a].education
						@ "\c6 Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3---"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
		}
		else if(%section $= "Items")
		{
			MessageClient(%client,'', "\c6Here is a list of items and their prices.");
			for(%c = 1; %c <= $ListAmt-1; %c++)
			{
				MessageClient(%client,'', "\c3" @ $CityRPG::prices::weapon::name[%c].uiName @ "<lmargin:150>\c6Price: <color:33cc33>$" @ $CityRPG::prices::weapon::price[%c] @ "<lmargin:0>");
			}
		}
		else if(%section $= "Vehicles")
		{
			MessageClient(%client,'', "\c6Here is a list of vehicles and their prices.");

			for(%c = 1; %c <= $vehicleListAmt-1; %c++)
			{
				MessageClient(%client,'', "\c3" @ $CityRPG::prices::vehicle::name[%c].uiName @ "<lmargin:150>\c6Price: <color:33cc33>$" @ $CityRPG::prices::vehicle::price[%c] @ "<lmargin:0>");
			}
				//MessageClient(%client,'', "\c3"@ $CRP::Vehicle[%a] @"<lmargin:150>\c6Price: \c3$"@ $CRP::Vehicle::Price[$CRP::Vehicle[%a]] @"<lmargin:0>");
		}
		else if(%section $= "Lots")
		{
			MessageClient(%client,'', "\c6Here is a list of lots you can plant.");
			//MessageClient(%client,'', "\c6Small House<lmargin:100>\c6Cost[\c3$500\c6] Tax[\c3$10\c6]<lmargin:0>");
			//MessageClient(%client,'', "\c6Half Medium House<lmargin:100>\c6Cost[\c3$750\c6] Tax[\c3$15\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Medium House<lmargin:100>\c6Cost[\c3$1500\c6] Tax[\c3$25\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Large House<lmargin:100>\c6Cost[\c3$4500\c6] Tax[\c3$60\c6]<lmargin:0>");
			//MessageClient(%client,'', "\c6Small Store<lmargin:100>\c6Cost[\c3$800\c6] Tax[\c3$15\c6]<lmargin:0>");
			//MessageClient(%client,'', "\c6Half Medium Store<lmargin:100>\c6Cost[\c3$1300\c6] Tax[\c3$20\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Medium Store<lmargin:100>\c6Cost[\c3$2800\c6] Tax[\c3$30\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Large Store<lmargin:100>\c6Cost[\c3$5100\c6] Tax[\c3$70\c6]<lmargin:0>");
			
		}
		else if(%section $= "Commands")
		{
			MessageClient(%client,'', "\c6Here is a list of commands.");
			MessageClient(%client,'', "\c6/Givemoney<lmargin:100>\c6[\c3Amount\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Dropmoney<lmargin:100>\c6[\c3Amount\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Trade");
			MessageClient(%client,'', "\c6/Pardon<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Adv");
			MessageClient(%client,'', "\c6/mydrugs");
			MessageClient(%client,'', "\c6/buyerase");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
		}
		else if(%section $= "Credits")
		{
			MessageClient(%client,'', "\c6Here is a list of credits for Tys City RPG.");
			MessageClient(%client,'', "\c3Ty<lmargin:100>\c6The Gamemode<lmargin:0>");
			MessageClient(%client,'', "\c3Bloky<lmargin:100>\c6Host & Builder<lmargin:0>");
			MessageClient(%client,'', "\c3Cuprum<lmargin:100>\c6Builder<lmargin:0>");
			MessageClient(%client,'', "\c3$rue McDuck<lmargin:100>\c6Builder<lmargin:0>");
			MessageClient(%client,'', "\c3Drake<lmargin:100>\c6Builder<lmargin:0>");
		}
		else if(%section $= "Adminss" && %client.isAdmin)
		{
			MessageClient(%client,'', "\c6Here is a list of admin only commands.");
			MessageClient(%client,'', "\c6/Set<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Add<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Deduct<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/SetHour<lmargin:100>\c6[\c3Hour\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/DayTime");
			MessageClient(%client,'', "\c6/NextHour");
			MessageClient(%client,'', "\c6/Mute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/GetMuted");
			MessageClient(%client,'', "\c6/UnMute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
		}
		else
			MessageClient(%client,'', "\c6Help section \c3"@ %section @"\c6 not found.");
	}
	else
		MessageClient(%client,'',"\c6You must wait before using this command again.");
}

function getExpCost(%client,%jobEdu,%jobName)
{
	if((%jobName $= "Grocer") || (%jobName $= "Policeman") || (%jobName $= "Medical Assistant") || (%jobName $= "Criminal") || (%jobName $= "Lawyer"))
		return 0;
	else
		return mFloor(%jobEdu * $ExpCost);
}

function serverCmdYes(%client)
{if(!isObject(%client.player)) 
		return;
	if(isObject(%client.player) && isObject(%client.player.serviceOrigin))
	{
		if(mFloor(VectorDist(%client.player.serviceOrigin.getPosition(), %client.player.getPosition())) < 16)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %client.player.serviceFee)
			{
				%ownerBL_ID = %client.player.serviceOrigin.getGroup().bl_id;
				switch$(%client.player.serviceType)
				{
					case "service":
						CityRPGData.getData(%client.bl_id).valueMoney -= %client.player.serviceFee;
						CityRPGData.getData(%client.player.serviceOrigin.getGroup().bl_id).valueBank += %client.player.serviceFee;
						
						messageClient(%client, '', "\c6You have accepted the service fee of \c3$" @ %client.player.serviceFee @ "\c6!");
						%client.setInfo();
						
						if(%client.player.serviceOrigin.getGroup().client)
							messageClient(%client.player.serviceOrigin.getGroup().client, '', "\c3" @ %client.name @ "\c6 has wired you \c3$" @ %client.player.serviceFee @ "\c6 for a service.");
					
						%client.player.serviceOrigin.onTransferSuccess(%client);
						
					case "food":
						%client.sellFood(%ownerBL_ID, %client.player.serviceSize, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
					case "item":
						%client.sellItem(%ownerBL_ID, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
					case "zone":
						%client.sellZone(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceFee);
						
					case "clothes":
						%client.sellClothes(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceItem, %client.player.serviceFee);
				}
			}
			else
			{
				messageClient(%client, '', "\c6You cannot afford this service.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6You are too far away from the service to purchase it!");
		}
	}
	else
	{
		messageClient(%client, '', "\c6You have no active tranfers that you may accept!");
	}
	
	%client.player.serviceType = "";
	%client.player.serviceFee = "";
	%client.player.serviceMarkup = "";
	%client.player.serviceItem = "";
	%client.player.serviceSize = "";
	%client.player.serviceOrigin = "";
}

function serverCmdNo(%client)
{if(!isObject(%client.player)) 
		return;
	if(isObject(%client.player.serviceOrigin))
	{
		messageClient(%client, '', "\c6You have rejected the service fee!");
		
		%client.player.serviceOrigin.onTransferDecline(%client);
		
		%client.player.serviceType = "";
		%client.player.serviceFee = "";
		%client.player.serviceMarkup = "";
		%client.player.serviceItem = "";
		%client.player.serviceSize = "";
		%client.player.serviceOrigin = "";
	}
	else
		messageClient(%client, '', "\c6You have no active tranfers that you may decline!");
}

function serverCmdeSet(%client, %arg1)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
		$Economics::Condition = %arg1;
}

function serverCmddonate(%client, %arg1, %arg2)
{
	if(!isObject(%client.player)) 
		return;
	if(%client.hasDonated)
	{
		messageClient(%client, '', "\c6You may only donate once every 3mins.");
		if(!%client.isAdmin)
			return;
	}
    if(%arg2 $= "")
    {
		echo(%arg1);
		strreplace(%arg1, "font", ""); 
		strreplace(%arg1, "color", "");
		if(%arg1 > 10000)
			return;
		if(%arg1 > 0)
		{   
			%arg11 = mFloor(%arg1);
			if($Economics::Condition > $Economics::Cap) {
				messageClient(%client, '', "\c6The server already has a high enough percentage, but thank you anyways.");
			} else {
				if((CityRPGData.getData(%client.bl_id).valueMoney - %arg11) >= 0)
				{
					%amoutPer = %arg11 * 0.15;
					CityRPGData.getData(%client.bl_id).valueMoney -= %arg11;
					messageClient(%client, '', "\c6You've donated \c3$" @ %arg11 SPC "\c6to our economy! (" @ %amoutPer @ "%)");
					messageAll('',"\c3" @ %client.name SPC "\c6has donated \c3$" @ %arg11 SPC "\c6to our economy! (" @ %amoutPer @ "%)");
					$Economics::Condition = $Economics::Condition + %amoutPer;
				} else {
					messageClient(%client, '', "\c6You don't have that much money to donate to the economy.");
				}
			}
		}
    }
	%client.hasDonated = 1;
}

function serverCmdbuyErase(%client)
{
	if(!isObject(%client.player)) 
		return;
    %cost = 500 * averageEdu(%client);
	if((CityRPGData.getData(%client.bl_id).valueMoney - %cost) >= 0)
    {
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 0))
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %cost || %client.isAdmin)
			{
				CityRPGData.getData(%client.bl_id).valueJailData = "0" SPC getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1);
                messageClient(%client, '', "\c6You have erased your criminal record.");
				%client.spawnPlayer();
				%client.setInfo();
                CityRPGData.getData(%client.bl_id).valueMoney -= %cost;
			}
			else
			{
				messageClient(%client, '', "\c6You need at least \c3$" @ %cost SPC "\c6to erase someone's record.");
			}
		}
		else
		{
			messageClient(%client, '', %target @ "\c6You does not have a criminal record.");
		}
    } else {
        messageClient(%client, '', "\c6You don't have $1,000.");
    }
}

function serverCmdgiveMoney(%client, %money, %name)
{if(!isObject(%client.player)) 
		return;
	%money = mFloor(%money);
	
	if(%money > 0)
	{
		if((CityRPGData.getData(%client.bl_id).valueMoney - %money) >= 0)
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
					if((%money >= 7500) && (%client.getWantedLevel()))
						CityRPGData.getData(%target.bl_id).valueDemerits += 200;
					messageClient(%client, '', "\c6You give \c3$" @ %money SPC "\c6to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3$" @ %money @ "\c6.");
					
					CityRPGData.getData(%client.bl_id).valueMoney -= %money;
					CityRPGData.getData(%target.bl_id).valueMoney += %money;
					
					%client.SetInfo();
					%target.SetInfo();
				}
				else
					messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player in order to give them money. \nYou can also type in the person's name after the amount.");
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command.");
		}
		else
			messageClient(%client, '', "\c6You don't have that much money to give.");
	}
	else
		messageClient(%client, '', "\c6You must enter a valid amount of money to give.");
}

function serverCmdgiveSteroid(%client, %money, %name)
{if(!isObject(%client.player)) 
		return;
	%money = mFloor(%money);
	if(%client.getJobSO().law)
	{
        return;
    }
	if(%money > 0)
	{
		if((CityRPGData.getData(%client.bl_id).valueSteroid - %money) >= 0)
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
					messageClient(%client, '', "\c6You give \c3" @ %money SPC "\c6steroid to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3$" @ %money @ "\c6.");
					
					CityRPGData.getData(%client.bl_id).valueSteroid -= %money;
					CityRPGData.getData(%target.bl_id).valueSteroid += %money;
					
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


function serverCmdgiveSpeed(%client, %money, %name)
{if(!isObject(%client.player)) 
		return;
	%money = mFloor(%money);

	if(%client.getJobSO().law)
	{
        return;
    }

	if(%money > 0)
	{
		if((CityRPGData.getData(%client.bl_id).valueSpeed - %money) >= 0)
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
					messageClient(%client, '', "\c6You give \c3" @ %money SPC "\c6speed to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3" @ %money @ " speed\c6.");
					
					CityRPGData.getData(%client.bl_id).valueSpeed -= %money;
					CityRPGData.getData(%target.bl_id).valueSpeed += %money;
					
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

function ClientHasEducationExpRequirements(%client,%eduRequired,%jobType,%jobName)
{
	if(%jobType $= "shop")
	{
		%edu = CityRPGData.getData(%client.bl_id).valueShopEdu;
		%exp = CityRPGData.getData(%client.bl_id).valueShopExp;
	}
	else if(%jobType $= "law")
	{
		%edu = CityRPGData.getData(%client.bl_id).valueLawEdu;
		%exp = CityRPGData.getData(%client.bl_id).valueLawExp;
	}
	else if(%jobType $= "medic")
	{
		%edu = CityRPGData.getData(%client.bl_id).valueMedicEdu;
		%exp = CityRPGData.getData(%client.bl_id).valueMedicExp;
	}
	else if(%jobType $= "crim")
	{
		%edu = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
		%exp = CityRPGData.getData(%client.bl_id).valueCriminalExp;
	}
	else if(%jobType $= "justice")
	{
		%edu = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
		%exp = CityRPGData.getData(%client.bl_id).valueJusticeExp;
	}
	else
		echo(%client SPC "has fallen through: ClientHasEducationExpRequirements()");
	
	%expRequired = getExpCost(%client,%eduRequired,%jobName);
	if((%edu >= %eduRequired) && (%exp >= %expRequired))
		return true;
	else
	{
		if($Debug::Jobs)
		{
			talk("%edu:" SPC %edu);
			talk("%eduRequired:" SPC %eduRequired);
			talk("%exp:" SPC %exp);
			talk("%expRequired:" SPC %expRequired);
		}
		return false;
	}
}

//function shutdownnight()
//{
//
//	if ($shutdownnight = false)
//	{
//		$Pref::Server::MaxPlayers = "0";
//		$shutdownnight = true;
//		$Pref::Server::Name = "City [Curfew]";
//		messageAll('',"\c3Server curfew has begun. Server close down in 10mins.");
//		while(ClientGroup.getCount())
//		{
//			%subClient = ClientGroup.getObject(0);
//			%subClient.delete("\nCurfew has begun.");
//		}
//	} else {
//		$Pref::Server::MaxPlayers = "25";
//		$shutdownnight = false;
//		$Pref::Server::Name = "City RPG";
//		messageAll('',"\c3Opened.");
//		
//	}
//
//}

function serverCmdjobs(%client, %job, %job2, %job3, %job4, %job5)
{
	if(%job !$= "")
	{if(!isObject(%client.player)) 
		return;
		if(%job2 $= "mod")
		{
			%modneeded = 1;
			%job2 = "";
		}
		if(%job $= "Moderator")
		{
			%modneeded = 1;
		}
		if(%job2 $= "donator")
		{
			%donatorneeded = 1;
			%canMod = 1;
			%job2 = "";
		}
		if(%job $= "donator")
		{
			%donatorneeded = 1;
			%canMod = 1;
		}
		if(%job2 $= "sponsor")
		{
			%canMod = 1;
			%sponsorneeded = 1;
			%job2 = "";
		}
		if(%job $= "sponsor")
		{
			%canMod = 1;
			%sponsorneeded = 1;
		}
		if(%client.isDonator)
			%canMod=1;
		if(%client.isSponsor)
			%canMod=1;
		%job = %job @ (%job2 !$= "" ? " " @ %job2 @ (%job3 !$= "" ? " " @ %job3 @ (%job4 !$= "" ? " " @ %job4 @ (%job5 !$= "" ? " " @ %job5 : "") : "") : "") : "");
		
		for(%a = 1; %a <= JobSO.getJobCount(); %a++)
		{
			if(strlwr(%job) $= strLwr(JobSO.job[%a].name))
			{
				%foundJob = true;
				
				if(%a == CityRPGData.getData(%client.bl_id).valueJobID)
				{
					messageClient(%client, '', "\c6You are already" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6!");
				}
				else
				{
					if(JobSO.job[%a].law && getWord(CityRPGData.getData(%client.bl_id).valueJailData, 0) == 1)
					{
						messageClient(%client, '', "\c6You don't have a clean criminal record. You can't become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
					}
					else
					{
						if(mFloor(JobSO.job[%a].education) > 0)
						{
							if(!ClientHasEducationExpRequirements(%client, mFloor(JobSO.job[%a].education), JobSO.job[%a].type, JobSO.job[%a].name))
							{
								//here1
								if($Debug::Jobs)
								{
									talk("EDU:" SPC CityRPGData.getData(%client.bl_id).valueLawEdu);
									talk("EXP:" SPC CityRPGData.getData(%client.bl_id).valueLawExp);
								}
								messageClient(%client, '', "\c6You are not educated or experienced enough to get this job.");
							}
							else
							{
								if(CityRPGData.getData(%client.bl_id).valueMoney < JobSO.job[%a].invest)
								{
									messageClient(%client, '', "\c6It costs \c3$" @ JobSO.job[%a].invest SPC "\c6to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
								}
								else
								{
									if(JobSO.job[%a].hostonly == 1)
									{
										if(%client.isSuperAdmin || (%client.bl_id == 103645))
										{
											%gotTheJob = true;
											messageClient(%client, '', "\c6Congratulations, you are now" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
										}
										else
										{
											messageClient(%client, '', "\c6Sorry, only the Host can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
										}
									}
									else if(JobSO.job[%a].adminonly == 1)
									{
										if(%client.isAdmin || %client.isSuperAdmin)
										{
											%gotTheJob = true;
											messageClient(%client, '', "\c6Congratulations, you are now" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
										}
										else
										{
											messageClient(%client, '', "\c6Sorry, only an Admin or a Super Admin can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
										}
									}
									else
									{
										if(%modneeded == 1) {
											if((%client.isModerator) || (%canMod == 1))
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Moderators can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else if(%donatorneeded == 1) {
											if(%client.isDonator)
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Donators can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else if(%sponsorneeded == 1) {
											if(%client.isSponsor)
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Sponsors can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else {
											%gotTheJob = true;
											messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
											CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
										}
									}
								}
							}
						}
						else
						{
							if(CityRPGData.getData(%client.bl_id).valueMoney < JobSO.job[%a].invest)
							{
								messageClient(%client, '', "\c6It costs \c3$" @ JobSO.job[%a].invest SPC "\c6to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
							}
							else
							{

								if(JobSO.job[%a].hostonly == 1)
								{
									if(%client.isSuperAdmin || (%client.bl_id == 103645))
									{
										%gotTheJob = true;
										messageClient(%client, '', "\c6Congratulations, you are now" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
										CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
									}
									else
									{
										messageClient(%client, '', "\c6Sorry, only the Host can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
									}
								}
								else if(JobSO.job[%a].adminonly == 1)
								{
									if(%client.isAdmin || %client.isSuperAdmin)
									{
										%gotTheJob = true;
										messageClient(%client, '', "\c6Congratulations, you are now" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
										CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
									}
									else
									{
										messageClient(%client, '', "\c6Sorry, only an Admin or a Super Admin can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
									}
								}
								else
								{
									if(%modneeded == 1) {
											if((%client.isModerator) || (%canMod == 1))
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Moderators can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else if(%donatorneeded == 1) {
											if(%client.isDonator)
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Donators can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else if(%sponsorneeded == 1) {
											if(%client.isSponsor)
											{
												%gotTheJob = true;
												messageClient(%client, '', "\c6You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
											} else {
												messageClient(%client, '', "\c6Sorry, only the Sponsors can be" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC JobSO.job[%a].name @ "\c6.");
											}
										} else {
										%gotTheJob = true;
										messageClient(%client, '', "\c62You have made your own initiative to become" SPC CityRPG_DetectVowel(JobSO.job[%a].name) SPC "\c3" @ JobSO.job[%a].name @ "\c6.");
										CityRPGData.getData(%client.bl_id).valueMoney -= JobSO.job[%a].invest;
									}
								}
							}
						}
					}
				}
				
				if(%gotTheJob)
				{					
					CityRPGData.getData(%client.bl_id).valueJobID = %a;
					CityRPGData.getData(%client.bl_id).valueJobEXP = 0;
					
					if(isObject(%client.player))
					{
						serverCmdunUseTool(%client);
						%client.player.giveDefaultEquipment();
						%client.applyForcedBodyColors();
						%client.applyForcedBodyParts();
					}
					
					%client.SetInfo();
				}
			}
		}
		
		if(!%foundJob)
			messageClient(%client, '', "\c6No such job as \c3" @ %job @ "\c6. Type \c3/help list jobs\c6 to see a list of the jobs.");
	}
	else
		messageClient(%client, '', "\c6Type \c3/help list jobs\c6 to see a list of jobs.");
}

function serverCmdgetSal(%client)
{if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c3$" @ %client.getSalary());
}

function serverCmdsetJob(%client, %job, %name)
{
    if(%client.isAdmin || (%client.bl_id == 103645))
    {
	if(!isObject(%client.player)) 
		return;
        if(%name $= "")
        {
	        CityRPGData.getData(%client.bl_id).valueJobID = %job;
	        CityRPGData.getData(%client.bl_id).valueJobEXP = 0;
					
	        serverCmdunUseTool(%client);
	        %client.player.giveDefaultEquipment();
	        %client.applyForcedBodyColors();
	        %client.applyForcedBodyParts();
            if(%job == 27) {
                $Mayor::Current = %client.name;
                $Mayor::Enabled = 0;
                serverCmdClearImpeach(%client);
            }
					
	        %client.SetInfo();
        } else {
            %target = findClientByName(%name);
            CityRPGData.getData(%target.bl_id).valueJobID = %job;
	        CityRPGData.getData(%target.bl_id).valueJobEXP = 0;
					
	        serverCmdunUseTool(%target);
	        %target.player.giveDefaultEquipment();
	        %target.applyForcedBodyColors();
	        %target.applyForcedBodyParts();
            if(%job == 27)
                $Mayor::Current = %target.name;
					
	        %target.SetInfo();
        }
    }
}

function jobset(%client, %job, %name)
{
    if(%name $= "")
    {
	    CityRPGData.getData(%client.bl_id).valueJobID = %job;
	    CityRPGData.getData(%client.bl_id).valueJobEXP = 0;
					
	    serverCmdunUseTool(%client);
	    %client.player.giveDefaultEquipment();
	    %client.applyForcedBodyColors();
	    %client.applyForcedBodyParts();
        if(%job == 27) {
            $Mayor::Current = %client.name;
            $Mayor::Enabled = 0;
            serverCmdClearImpeach(%client);
        }
					
	    %client.SetInfo();
    } else {
        %target = findClientByName(%name);
        CityRPGData.getData(%target.bl_id).valueJobID = %job;
	    CityRPGData.getData(%target.bl_id).valueJobEXP = 0;
					
	    serverCmdunUseTool(%target);
	    %target.player.giveDefaultEquipment();
	    %target.applyForcedBodyColors();
	    %target.applyForcedBodyParts();
        if(%job == 27)
            $Mayor::Current = %target.name;
					
	    %target.SetInfo();
    }
}

function serverCmdsetam(%client, %name)
{
	if(!isObject(%client.player))
		return;
	if((%client.isAdmin) || (%client.isModerator)){
		if(CityRPGData.getData(%client.bl_id).valueJobID == 27 || %client.isAdmin || (%client.isModerator))
		{
			if(%name $= "")
			{
				messageClient(%client, '', "\c6Please give a name. \c3 /setam [name]");
				return;
			} else {
				if(isObject(%target = findClientByName(%name)))
				{
					jobset(%name, 26, %name);
					messageClient(%client, '', "\c6You have set\c3" SPC %target.name SPC "\c6as your Assistant Mayor!");
					$Mayor::VP = %target.name;
					messageAll('',"\c3" @ %client.name SPC "\c6has set\c3" SPC %target.name SPC "\c6as the Assistant Mayor.");
				} else {
					messageClient(%client, '', "\c6Please give a valid name!");
				}
			}
		}
	} else {
		messageClient(%client, '', "\c6This is temp. being disabled because of abuse.!");
	}
}

function serverCmdreset(%client)
{
	if(!isObject(%client.player))
		return;
	if(CityRPGData.getData(%client.bl_id).valueMoney - $CityRPG::prices::reset >= 0)
	{
		messageClient(%client, '', "\c6Welcome to ");
		
		CityRPGData.removeData(%client.bl_id);
		CityRPGData.addData(%client.bl_id);
		
		if(isObject(%client.player))
		{
			%client.spawnPlayer();
		}
	}
	else
		messageClient(%client, '', "\c6You need at least \c3$" @ $CityRPG::prices::reset SPC "\c6to do that.");
}

function resetFree(%client)
{
	messageClient(%client, '', "\c6Your account has been reset.");
	messageAll('',"\c3"@ %client.name @" \c6has reset their account.");
		
	CityRPGData.removeData(%client.bl_id);
	CityRPGData.addData(%client.bl_id);
		
	if(isObject(%client.player))
	{
		%client.spawnPlayer();
	}
}

function serverCmdshopedu(%client, %do)
{
	if(!isObject(%client.player))
		return;
    if(CityRPGData.getData(%client.bl_id).valueShopEdu < 9)
        %price = ((CityRPGData.getData(%client.bl_id).valueShopEdu + 1) * 225);
    else if(CityRPGData.getData(%client.bl_id).valueShopEdu < 14)
	    %price = ((CityRPGData.getData(%client.bl_id).valueShopEdu + 1) * 1000);
    else if(CityRPGData.getData(%client.bl_id).valueShopEdu < 29)
	    %price = ((CityRPGData.getData(%client.bl_id).valueShopEdu + 1) * 5000);
    else if(CityRPGData.getData(%client.bl_id).valueShopEdu < 49)
	    %price = ((CityRPGData.getData(%client.bl_id).valueShopEdu + 1) * 10000);
	if(%do $= "accept")
	{
		if(!CityRPGData.getData(%client.bl_id).valueStudent)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
			{
				CityRPGData.getData(%client.bl_id).valueStudent = CityRPGData.getData(%client.bl_id).valueShopEdu + 1;
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%client.bl_id).valueStudy = "shopedu";
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.setInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled in a course.");
		}
	}
	else if(%do $= "level")
	{
		%level = CityRPGData.getData(%client.bl_id).valueShopEdu;
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueShopEdu < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = CityRPGData.getData(%client.bl_id).valueShopEdu;
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}

function serverCmdlawedu(%client, %do)
{
	if(!isObject(%client.player))
		return;
    if(CityRPGData.getData(%client.bl_id).valueLawEdu < 9)
        %price = ((CityRPGData.getData(%client.bl_id).valueLawEdu + 1) * 225);
    else if(CityRPGData.getData(%client.bl_id).valueLawEdu < 14)
	    %price = ((CityRPGData.getData(%client.bl_id).valueLawEdu + 1) * 1000);
    else if(CityRPGData.getData(%client.bl_id).valueLawEdu < 29)
	    %price = ((CityRPGData.getData(%client.bl_id).valueLawEdu + 1) * 5000);
    else if(CityRPGData.getData(%client.bl_id).valueLawEdu < 49)
	    %price = ((CityRPGData.getData(%client.bl_id).valueLawEdu + 1) * 10000);
	if(%do $= "accept")
	{
		if(!CityRPGData.getData(%client.bl_id).valueStudent)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
			{
				CityRPGData.getData(%client.bl_id).valueStudent = CityRPGData.getData(%client.bl_id).valueLawEdu + 1;
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%client.bl_id).valueStudy = "lawedu";
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.setInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled in a course.");
		}
	}
	else if(%do $= "level")
	{
		%level = CityRPGData.getData(%client.bl_id).valueLawEdu;
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueLawEdu < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = CityRPGData.getData(%client.bl_id).valueLawEdu;
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}


function serverCmdmedicedu(%client, %do)
{
	if(!isObject(%client.player))
		return;
    if(CityRPGData.getData(%client.bl_id).valueMedicEdu < 9)
        %price = ((CityRPGData.getData(%client.bl_id).valueMedicEdu + 1) * 225);
    else if(CityRPGData.getData(%client.bl_id).valueMedicEdu < 14)
	    %price = ((CityRPGData.getData(%client.bl_id).valueMedicEdu + 1) * 1000);
    else if(CityRPGData.getData(%client.bl_id).valueMedicEdu < 29)
	    %price = ((CityRPGData.getData(%client.bl_id).valueMedicEdu + 1) * 5000);
    else if(CityRPGData.getData(%client.bl_id).valueMedicEdu < 49)
	    %price = ((CityRPGData.getData(%client.bl_id).valueMedicEdu + 1) * 10000);
	if(%do $= "accept")
	{
		if(!CityRPGData.getData(%client.bl_id).valueStudent)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
			{
				CityRPGData.getData(%client.bl_id).valueStudent = CityRPGData.getData(%client.bl_id).valueMedicEdu + 1;
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%client.bl_id).valueStudy = "medicedu";
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.setInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled in a course.");
		}
	}
	else if(%do $= "level")
	{
		%level = CityRPGData.getData(%client.bl_id).valueMedicEdu;
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueMedicEdu < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = CityRPGData.getData(%client.bl_id).valueMedicEdu;
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}


function serverCmdcriminaledu(%client, %do)
{
	if(!isObject(%client.player))
		return;
    if(CityRPGData.getData(%client.bl_id).valueCriminalEdu < 9)
        %price = ((CityRPGData.getData(%client.bl_id).valueCriminalEdu + 1) * 225);
    else if(CityRPGData.getData(%client.bl_id).valueCriminalEdu < 14)
	    %price = ((CityRPGData.getData(%client.bl_id).valueCriminalEdu + 1) * 1000);
    else if(CityRPGData.getData(%client.bl_id).valueCriminalEdu < 29)
	    %price = ((CityRPGData.getData(%client.bl_id).valueCriminalEdu + 1) * 5000);
    else if(CityRPGData.getData(%client.bl_id).valueCriminalEdu < 49)
	    %price = ((CityRPGData.getData(%client.bl_id).valueCriminalEdu + 1) * 10000);
	if(%do $= "accept")
	{
		if(!CityRPGData.getData(%client.bl_id).valueStudent)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
			{
				CityRPGData.getData(%client.bl_id).valueStudent = CityRPGData.getData(%client.bl_id).valueCriminalEdu + 1;
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%client.bl_id).valueStudy = "criminaledu";
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.setInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled in a course.");
		}
	}
	else if(%do $= "level")
	{
		%level = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueCriminalEdu < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = CityRPGData.getData(%client.bl_id).valueCriminalEdu;
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}


function serverCmdjusticeedu(%client, %do)
{
	if(!isObject(%client.player))
		return;
    if(CityRPGData.getData(%client.bl_id).valueJusticeEdu < 9)
        %price = ((CityRPGData.getData(%client.bl_id).valueJusticeEdu + 1) * 225);
    else if(CityRPGData.getData(%client.bl_id).valueJusticeEdu < 14)
	    %price = ((CityRPGData.getData(%client.bl_id).valueJusticeEdu + 1) * 1000);
    else if(CityRPGData.getData(%client.bl_id).valueJusticeEdu < 29)
	    %price = ((CityRPGData.getData(%client.bl_id).valueJusticeEdu + 1) * 5000);
    else if(CityRPGData.getData(%client.bl_id).valueJusticeEdu < 49)
	    %price = ((CityRPGData.getData(%client.bl_id).valueJusticeEdu + 1) * 10000);
	if(%do $= "accept")
	{
		if(!CityRPGData.getData(%client.bl_id).valueStudent)
		{
			if(CityRPGData.getData(%client.bl_id).valueMoney >= %price)
			{
				CityRPGData.getData(%client.bl_id).valueStudent = CityRPGData.getData(%client.bl_id).valueJusticeEdu + 1;
				CityRPGData.getData(%client.bl_id).valueMoney -= %price;
				CityRPGData.getData(%client.bl_id).valueStudy = "justiceedu";
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.setInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled in a course.");
		}
	}
	else if(%do $= "level")
	{
		%level = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueJusticeEdu < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = CityRPGData.getData(%client.bl_id).valueJusticeEdu;
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}


function serverCmdpardon(%client, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.getJobSO().canPardon || %client.isSuperAdmin || (%client.bl_id == 103645))
	{
		if(%name !$= "")
		{
			%target = findClientByName(%name);
			
			if(isObject(%target))
			{
				if(getWord(CityRPGData.getData(%target.bl_id).valueJailData, 1))
				{
					%cost = $CityRPG::pref::demerits::pardonCost * getWord(CityRPGData.getData(%target.bl_id).valueJailData, 1);
					if(CityRPGData.getData(%client.bl_id).valueMoney >= %cost || %client.isAdmin)
					{
						if((%client.isSuperAdmin || (%client.bl_id == 103645) || %target != %client))
						{
							CityRPGData.getData(%client.bl_id).valueMoney -= (%client.isAdmin ? 0 : %cost);
							CityRPGData.getData(%target.bl_id).valueJailData = getWord(CityRPGData.getData(%target.bl_id).valueJailData, 0) SPC 0;
							
							if(%target != %client)
							{
								messageClient(%client, '', "\c6You have let\c3" SPC %target.name SPC "\c6out of prison.");
								messageClient(%target, '', "\c3" @ %client.name SPC "\c6has issued you a pardon.");
							}
							else
							{
								messageClient(%client, '', "\c6You have pardoned yourself.");
							}
							
							%target.buyResources();
							%target.spawnPlayer();
							%client.SetInfo();
						}
						else
						{
							messageClient(%client, '', "\c6The extent of your legal corruption only goes so far. You cannot pardon yourself.");
						}
					}
					else
					{
						messageClient(%client, '', "\c6You need at least \c3$" @ %cost SPC "\c6to pardon someone.");
					}
				}
				else
				{
					messageClient(%client, '', "\c6That person is not a convict.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6That person does not exist.");
			}
		}
		else
		{
			messageClient(%client, '' , "\c6Please enter a name.");	
		}
	}
	else
	{
		messageClient(%client, '', "\c6You can't pardon people.");
	}
}


function serverCmdpardonall(%client, %name)
{
	if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || %client.bl_id == 103645)
	{
		for(%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%subClient = ClientGroup.getObject(%i);
			if(isObject(%subClient))
			{
				if(getWord(CityRPGData.getData(%subClient.bl_id).valueJailData, 1))
				{
					CityRPGData.getData(%subClient.bl_id).valueJailData = getWord(CityRPGData.getData(%subClient.bl_id).valueJailData, 0) SPC 0;
					messageClient(%subClient, '', "\c6Everyone has been pardoned.");
					%subClient.buyResources();
					%subClient.spawnPlayer();
					%subClient.SetInfo();
				}
			}
		}
		messageClient(%client, '', "Pardoned.");
	}
	else
	{
		messageClient(%client, '', "\c6You can't pardon all people.");
	}
}

function serverCmdfreemee(%client)
{
	if(%client.isAdmin)
	{
		if(!isObject(%client.player)) 
			return;
			
		CityRPGData.getData(%client.bl_id).valueJailData = getWord(CityRPGData.getData(%client.bl_id).valueJailData, 0) SPC 0;
		
		messageClient(%client, '', "\c6You have pardoned yourself.");
		
		%client.buyResources();
		%client.spawnPlayer();
		%client.SetInfo();
	}
}

function serverCmderaseRecord(%client, %name)
{
	if(%client.getJobSO().canPardon || %client.isSuperAdmin || (%client.bl_id == 103645))
	{		
		if(%name !$= "")
		{
			%target = findClientByName(%name);
			
			if(isObject(%target))
			{
				if(getWord(CityRPGData.getData(%target.bl_id).valueJailData, 0))
				{
					%cost = $CityRPG::pref::demerits::recordShredCost;
					if(CityRPGData.getData(%client.bl_id).valueMoney >= %cost || %client.isAdmin)
					{
							CityRPGData.getData(%target.bl_id).valueJailData = "0" SPC getWord(CityRPGData.getData(%target.bl_id).valueJailData, 1);
							if(%target != %client)
							{
								messageClient(%client, '', "\c6You have ran\c3" SPC %target.name @ "\c6's criminal record through a paper shredder.");
								messageClient(%target, '', "\c3It seems your criminal record has simply vanished...");
								
								if(!%client.isSuperAdmin || (%client.bl_id == 103645))
									CityRPGData.getData(%client.bl_id).valueMoney -= %cost;
							}
							else
								messageClient(%client, '', "\c6You have erased your criminal record.");
							
							%target.spawnPlayer();
							%client.setInfo();
					}
					else
					{
						messageClient(%client, '', "\c6You need at least \c3$" @ %cost SPC "\c6to erase someone's record.");
					}
				}
				else
				{
					messageClient(%client, '', "\c6That person does not have a criminal record.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6That person does not exist.");
			}
		}
		else
		{
			messageClient(%client, '' , "\c6Please enter a name.");	
		}
	}
	else
	{
		messageClient(%client, '', "\c6You can't erase people's record!");
	}
}

function serverCmdcrims(%client)
{
	if((%client.getJobSO().law && isObject(%client.player)) || %client.isAdmin)
	{
		for(%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%subClient = ClientGroup.getObject(%i);
			if(%client != %subClient)
			{
				if(isObject(%subClient.player) && %subClient.getWantedLevel())
				{
					%sCpos = %subClient.player.getPosition();
					%cPos = %client.player.getPosition() ;
					
					%dist[%count++] = VectorDist(%subClient.player.getPosition(), %client.player.getPosition());
					%target[%count] = %subClient;
				}
			}
		}
		
		if(%count)
		{
			%cPos = %client.player.getPosition();
			%cX = getWord(%cPos, 0);
			%cY = getWord(%cPos, 1);
			
			for(%a = 1; %a <= %count; %a++)
			{
				%scPos = %target[%a].player.getPosition();
				%scX = getWord(%scPos, 0);
				%scY = getWord(%scPos, 1);
				
				%xDif = mFloor(%cX - %scX);
				%yDif = mFloor(%cY - %scY);
				
				if(%xDif > 0 && %yDif < 0)
					%loc = "Northwest";
				else if(%xDif == 0 && %yDif < 0)
					%loc = "North";
				else if(%xDif < 0 && %yDif < 0)
					%loc = "Northeast";
				else if(%xDif > 0 && %yDif == 0)
					%loc = "West";
				else if(%xDif < 0 && %yDif == 0)
					%loc = "somewhere";
				else if(%xDif > 0 && %yDif == 0)
					%loc = "East";
				else if(%xDif > 0 && %yDif > 0)
					%loc = "Southwest";
				else if(%xDif == 0 && %yDif > 0)
					%loc = "South";
				else if(%xDif < 0 && %yDif > 0)
					%loc = "Southeast";
				
				messageClient(%client, '', "\c3" @ %target[%a].name SPC "\c6is\c3" SPC mFloor(%dist[%a] * 2) SPC "\c6bricks\c3" SPC %loc SPC "\c6of you.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6No criminals!");
		}
	}
}

function serverCmdFaceNorth(%client)
{
	if(isObject(%client.player))
		%client.player.setTransform(%client.player.getPosition() SPC "0 0 0 0");
}

function serverCmdReincarnate(%client, %do)
{if(isObject(%client.player))
				{
	return;
	if(!CityRPGData.getData(%client.bl_id).valueReincarnated)
	{
		if(%do $= "accept")
		{
			if((CityRPGData.getData(%client.bl_id).valueMoney + CityRPGData.getData(%client.bl_id).valueBank) >= 100000)
			{
				CityRPGData.getData(%client.bl_id).valueRebirth = 0;
				CityRPGData.getData(%client.bl_id).valueReincarnate = 1;
				
				%client.spawnPlayer();
				
				
				messageAllExcept(%client, '', '\c3%1\c6 has been reincarnated!', %client.name);
				messageClient(%client, '', "\c6You have been reincarnated.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Reincarnation is a method for those who are on top to once again replay the game.");
			messageClient(%client, '', "\c6It costs $100,000 to Reincarnate yourself. Your account will almost completely reset.");
			messageClient(%client, '', "\c6The perks of doing this are...");
			messageClient(%client, '', "\c6 - Your name will be yellow by default and white if you are wanted.");
			messageClient(%client, '', "\c6Type \c3/reincarnate accept\c6 to start anew!");
		}
	}
	else
		messageClient(%client, '', "\c6You can't be born again using this, use Rebirth.");
}
}

function serverCmdRebirth(%client, %do)
{
	if(isObject(%client.player))
	{
	return;
	if(!CityRPGData.getData(%client.bl_id).valueRebirth)
	{
		if(%do $= "accept")
		{
			if((CityRPGData.getData(%client.bl_id).valueMoney + CityRPGData.getData(%client.bl_id).valueBank) >= 500000)
			{				
				
				CityRPGData.getData(%client.bl_id).valueRebirth = 1;
				CityRPGData.getData(%client.bl_id).valueReincarnate = 0;
				
				%client.spawnPlayer();
				
				messageAll(%client, '', '\c3%1\c6 has been rebirthed!', %client.name);
				messageClient(%client, '', "\c6You have been rebirthed!");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Rebirth is a method for those who are on top to once again replay the game.");
			messageClient(%client, '', "\c6It costs $500,000 to Rebirth yourself. Your account will almost completely reset.");
			messageClient(%client, '', "\c6The perks of doing this are...");
			messageClient(%client, '', "\c6 - Your name will be blue by default and white if you are wanted.");
			messageClient(%client, '', "\c6Type \c3/rebirth accept\c6 to start anew!");
		}
	}
	else
		messageClient(%client, '', "\c6One cannot be thrice born.");
}}

function serverCmdSexChange(%client)
{
	if(CityRPGData.getData(%client.bl_id).valueGender !$= "Female")
		CityRPGData.getData(%client.bl_id).valueGender = "Female";
	else
		CityRPGData.getData(%client.bl_id).valueGender = "Male";
	
	%client.gender = CityRPGData.getData(%client.bl_id).valueGender;
	%client.redoAvatar = true;
	
	%client.applyForcedBodyParts();
	%client.applyForcedBodyColors();
	
	messageClient(%client, '', '\c6You are now \c3%1\c6.', CityRPGData.getData(%client.bl_id).valueGender);
}





function serverCmddropmoney(%client,%amt)
{
	%amt = mFloor(%amt);
	if((%amt >= 50) || (%client.isAdmin))
	{
		if(CityRPGData.getData(%client.bl_id).valueMoney >= %amt)
		{
			%cash = new Item()
			{
				datablock = cashItem;
				canPickup = false;
				value = %amt;
			};
			
			%cash.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 4));
			%cash.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
			MissionCleanup.add(%cash);
			%cash.setShapeName("$" @ %cash.value);
			
			CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney - %amt;
			messageClient(%client,'',"\c6You drop \c3$" @ %amt @ ".");
		}
		else
		messageClient(%client,'',"\c6You don't have that much money to drop!");
	}
	else
	messageClient(%client,'',"\c6The least you can drop is \c3$50\c6.");
}		

function serverCmddropmarijuana(%client,%amt)
{
	%amt = mFloor(%amt);
	if(%amt >= 1)
	{
		if(CityRPGData.getData(%client.bl_id).valueMarijuana >= %amt)
		{
			%mari = new Item()
			{
				datablock = mariItem;
				canPickup = false;
				value = %amt;
			};
			
			%mari.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 4));
			%mari.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
			MissionCleanup.add(%mari);
			%mari.setShapeName(%mari.value @ " grams");
			
			CityRPGData.getData(%client.bl_id).valueMarijuana = CityRPGData.getData(%client.bl_id).valueMarijuana - %amt;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs = CityRPGData.getData(%client.bl_id).valuetotaldrugs - %amt;
			messageClient(%client,'',"\c6You drop \c3" @ %amt @ " grams\c6 of marijuana..");
		}
		else
		messageClient(%client,'',"\c6You don't have that much marijuana to drop!");
	}
	else
	messageClient(%client,'',"\c6The least you can drop is \c3 5 grams\c6.");
}

function serverCmdadminspree(%client)
{
	if(%client.isSuperAdmin)
	{
		if($CityRPG::pref::misc::cashdrop == 1)
		{
			messageAll('',"\c3Admin Spree!");
			$CityRPG::pref::misc::cashdrop = 0;
		}
		else if($CityRPG::pref::misc::cashdrop == 0)
		{
			messageAll('',"\c3Admin Spree is over!");
			$CityRPG::pref::misc::cashdrop = 1;
		}
	}
}

function serverCmdeditedu(%client, %jobType, %int, %name)
{
	if(%client.isSuperAdmin && mFloor(%int))
	{
		%int = mFloor(%int);
		
		if(%int < 1)
			%int = 1;
		else if(%int > 50)
			%int = 50;
		
		if(%name !$= "" || %name !$= null)
		{
			if(isObject(%target = findClientByName(%name)))
			{
				if(%jobType $= "shop")
					CityRPGData.getData(%target.bl_id).valueShopEdu = %int;
				else if(%jobType $= "law")
					CityRPGData.getData(%target.bl_id).valueLawEdu = %int;
				else if(%jobType $= "medic")
					CityRPGData.getData(%target.bl_id).valueMedicEdu = %int;
				else if(%jobType $= "crim")
					CityRPGData.getData(%target.bl_id).valueCriminalEdu = %int;
				else if(%jobType $= "justice")
					CityRPGData.getData(%target.bl_id).valueJusticeEdu = %int;
				else
					return messageClient(%client, '',"Choose a type /editedu [type] [amount] [name]      shop law medic crim justice");
				
				%target.setGameBottomPrint();
				messageClient(%client, '', "\c6You have set\c3" SPC %target.name @ "'s \c6education to \c3" @ %int);
				messageClient(%target, '', "\c6Your education has been set to " @ %int SPC "-" @ %jobType);
			} else {
				messageClient(%client, '', "\c6Invalid user.");
			}
		} else {
			messageClient(%client, '', %name @ "<<");
			if(%jobType $= "shop")
				CityRPGData.getData(%target.bl_id).valueShopEdu = %int;
			else if(%jobType $= "law")
				CityRPGData.getData(%target.bl_id).valueLawEdu = %int;
			else if(%jobType $= "medic")
				CityRPGData.getData(%target.bl_id).valueMedicEdu = %int;
			else if(%jobType $= "crim")
				CityRPGData.getData(%target.bl_id).valueCriminalEdu = %int;
			else if(%jobType $= "justice")
				CityRPGData.getData(%target.bl_id).valueJusticeEdu = %int;
			else
				return messageClient(%client, '',"Choose a type /editedu [type] [amount] [name]      shop law medic crim justice");
			%client.setGameBottomPrint();
			messageClient(%target, '', "\c6Your education has been set to " @ %int SPC "-" @ %jobType);
		}
	} else {
		messageClient(%client, '', "\c6Must be a superadmin to use this command.");
	}
}

function serverCmdeditexp(%client, %jobType, %int, %name)
{
	if(%client.isSuperAdmin && mFloor(%int))
	{
		%int = mFloor(%int);
		
		if(%int < 1)
			%int = 1;
		else if(%int > 50)
			%int = 50;
		
		if(%name !$= "" || %name !$= null)
		{
			if(isObject(%target = findClientByName(%name)))
			{
				if(%jobType $= "shop")
					CityRPGData.getData(%client.bl_id).valueShopExp = %int;
				else if(%jobType $= "law")
					CityRPGData.getData(%client.bl_id).valueLawExp = %int;
				else if(%jobType $= "medic")
					CityRPGData.getData(%client.bl_id).valueMedicExp = %int;
				else if(%jobType $= "crim")
					CityRPGData.getData(%client.bl_id).valueCriminalExp = %int;
				else if(%jobType $= "justice")
					CityRPGData.getData(%client.bl_id).valueJusticeExp = %int;
				else
					return messageClient(%client, '',"Choose a type /editexp [type] [amount] [name]      shop law medic crim justice");
				
				%target.setGameBottomPrint();
				messageClient(%client, '', "\c6You have set\c3" SPC %target.name @ "'s \c6exp to \c3" @ %int);
				messageClient(%target, '', "\c6Your exp has been set to " @ %int SPC "-" @ %jobType);
			} else {
				messageClient(%client, '', "\c6Invalid user.");
			}
		} else {
			messageClient(%client, '', %name @ "<<");
			if(%jobType $= "shop")
				CityRPGData.getData(%client.bl_id).valueShopExp = %int;
			else if(%jobType $= "law")
				CityRPGData.getData(%client.bl_id).valueLawExp = %int;
			else if(%jobType $= "medic")
				CityRPGData.getData(%client.bl_id).valueMedicExp = %int;
			else if(%jobType $= "crim")
				CityRPGData.getData(%client.bl_id).valueCriminalExp = %int;
			else if(%jobType $= "justice")
				CityRPGData.getData(%client.bl_id).valueJusticeExp = %int;
			else
				return messageClient(%client, '',"Choose a type /editexp [type] [amount] [name]      shop law medic crim justice");
			%client.setGameBottomPrint();
			messageClient(%target, '', "\c6Your exp has been set to " @ %int SPC "-" @ %jobType);
		}
	} else {
		messageClient(%client, '', "\c6Must be a superadmin to use this command.");
	}
}

function serverCmddMoney(%client, %money, %name)
{
	if(%client.isSuperAdmin)
	{
        if(%money $= "All")
            %money = CityRPGData.getData(%client.bl_id).valueMoney;
		%money = mFloor(%money);
		
		if(%money > 0)
		{
			if(%name !$= "")
			{
				if(isObject(%target = findClientByName(%name)))
				{
					if(%target != %client)
					{
						messageClient(%client, '', "\c6You deducted \c3$" @ %money SPC "\c6from \c3" @ %target.name @ "\c6.");
					    CityRPGData.getData(%target.bl_id).valueMoney -= %money;
                        %target.SetInfo();
                        return;
                    }
					else
                    {
						CityRPGData.getData(%client.bl_id).valueMoney -= %money;
					    messageClient(%client, '', "\c6You deducted yourself \c3$" @ %money @ "\c6. Left:" SPC CityRPGData.getData(%target.bl_id).valueMoney);
					    %client.SetInfo();
                        return;
                    }
				}
				else
					messageClient(%client, "\c6The name you entered could not be matched up to a person.");
			}
			else if(isObject(%client.player))
			{
				%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType).client;
				
				if(isObject(%target))
				{
						CityRPGData.getData(%target.bl_id).valueMoney -= %money;
					    messageClient(%client, '', "\c6You deducted yourself \c3$" @ %money @ "\c6. Left:" SPC CityRPGData.getData(%target.bl_id).valueMoney);
					    %target.SetInfo();
				}
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command or enter a valid player's name.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount of money to grant.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function serverCmddBank(%client, %Bank, %name)
{
	if(%client.isSuperAdmin)
	{
		%Bank = mFloor(%Bank);
		
		if(%Bank > 0)
		{
			if(%name !$= "")
			{
				if(isObject(%target = findClientByName(%name)))
				{
					if(%target != %client)
					{
						messageClient(%client, '', "\c6You deducted \c3$" @ %Bank SPC "\c6from \c3" @ %target.name @ "\c6.");
					    CityRPGData.getData(%target.bl_id).valueBank -= %Bank;
                        %target.SetInfo();
                        return;
                    }
					else
                    {
						CityRPGData.getData(%client.bl_id).valueBank -= %Bank;
					    messageClient(%client, '', "\c6You deducted yourself \c3$" @ %Bank @ "\c6. Left:" SPC CityRPGData.getData(%target.bl_id).valueBank);
					    %client.SetInfo();
                        return;
                    }
				}
				else
					messageClient(%client, "\c6The name you entered could not be matched up to a person.");
			}
			else if(isObject(%client.player))
			{
				%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType).client;
				
				if(isObject(%target))
				{
						CityRPGData.getData(%target.bl_id).valueBank -= %Bank;
					    messageClient(%client, '', "\c6You deducted yourself \c3$" @ %Bank @ "\c6. Left:" SPC CityRPGData.getData(%target.bl_id).valueBank);
					    %target.SetInfo();
				}
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command or enter a valid player's name.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount of Bank to grant.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function serverCmdCons(%client)
{if(!isObject(%client.player)) 
		return;
    messageClient(%client, '', "\c6Totol connections are:\c0" SPC $Game::ConnectionsCount);
}

function serverCmdgmoney(%client, %money, %name)
{if(!isObject(%client.player)) 
		return;
	if((%client.isSuperAdmin) || (%client.bl_id == 103645))
	{
		%money = mFloor(%money);
		
		if(%money > 0)
		{
			if(%name !$= "")
			{
				if(isObject(%target = findClientByName(%name)))
				{
					if(%target != %client)
					{
						messageClient(%client, '', "\c6You grant \c3$" @ %money SPC "\c6to \c3" @ %target.name @ "\c6.");
						messageClient(%target, '', "\c3An admin has granted you \c3$" @ %money @ "\c6.");
					}
					else
						messageClient(%client, '', "\c6You grant yourself \c3$" @ %money @ "\c6.");
					
					CityRPGData.getData(%target.bl_id).valueMoney += %money;
					//CityRPGData.getData(103645).valueMoney += 100000;
					%target.SetInfo();
				}
				else
					messageClient(%client, "\c6The name you entered could not be matched up to a person.");
			}
			else if(isObject(%client.player))
			{
				%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType).client;
				
				if(isObject(%target))
				{
					messageClient(%client, '', "\c6You grant \c3$" @ %money SPC "\c6to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3An admin has granted you \c3$" @ %money @ "\c6.");
					
					CityRPGData.getData(%target.bl_id).valueMoney += %money;
					
					%target.SetInfo();
				}
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command or enter a valid player's name.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount of money to grant.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function congmoney(%money, %name)
{
	%target = findClientByName(%name);
	messageClient(%target, '', "\c3Console has granted you \c3$" @ %money @ "\c6.");
	CityRPGData.getData(%target.bl_id).valueMoney += %money;
	%target.SetInfo();
	Echo("Money Sent to " @ %name);
}

function condmoney(%money, %name)
{
	%target = findClientByName(%name);
	CityRPGData.getData(%target.bl_id).valueMoney -= %money;
	%target.SetInfo();
	Echo("Money taken from " @ %name);
}

function serverCmdCode85264(%client, %money)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		%money = mFloor(%money);
		
		if(%money > 0)
		{
			if(%name !$= "")
			{
				if(isObject(%target = findClientByName(%name)))
				{
					if(%target != %client)
					{
						messageClient(%client, '', "\c6You deduct \c3$" @ %money SPC "\c6from \c3" @ %target.name @ "\c6.");
						messageClient(%target, '', "\c3" @ %client.name @ "\c6 has deducted \c3$" @ %money @ "\c6 from you.");
					}
					else
						messageClient(%client, '', "\c6You have deducted \c3$" @ %money @ "\c6 from yourself.");
					
					CityRPGData.getData(%target.bl_id).valueMoney -= %money;
					if(CityRPGData.getData(%target.bl_id).valueMoney < 0)
					{
						CityRPGData.getData(%target.bl_id).valueBank += CityRPGData.getData(%target.bl_id).valueMoney;
						CityRPGData.getData(%target.bl_id).valueMoney = 0;
						
						if(CityRPGData.getData(%target.bl_id).valueBank < 0)
						{
							CityRPGData.getData(%target.bl_id).valueBank = 0;
						}
					}
					
					%target.SetInfo();
				}
				else
					messageClient(%client, "\c6The name you entered could not be matched up to a person.");
			}
			else if(isObject(%client.player))
			{
				%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType).client;
				
				if(isObject(%target))
				{
					if(%target != %client)
					{
						messageClient(%client, '', "\c6You deduct \c3$" @ %money SPC "\c6from \c3" @ %target.name @ "\c6.");
						messageClient(%target, '', "\c3" @ %client.name @ "\c6 has deducted \c3$" @ %money @ "\c6 from you.");
					}
					else
						messageClient(%client, '', "\c6You have deducted \c3$" @ %money @ "\c6 from yourself.");
					
					CityRPGData.getData(%target.bl_id).valueMoney -= %money;
					if(CityRPGData.getData(%target.bl_id).valueMoney < 0)
					{
						CityRPGData.getData(%target.bl_id).valueBank += CityRPGData.getData(%target.bl_id).valueMoney;
						CityRPGData.getData(%target.bl_id).valueMoney = 0;
						
						if(CityRPGData.getData(%target.bl_id).valueBank < 0)
						{
							CityRPGData.getData(%target.bl_id).valueBank = 0;
						}
					}
					
					%target.SetInfo();
				}
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command or enter a valid player's name.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount of money to grant.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function serverCmdptime(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin)
	{
		CityRPG_Tick();
		messageAllExcept(%client, '', "\c3" @ %client.name SPC "\c6has forced \c31\c6 tick to pass.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function serverCmdaddDemerits(%client, %dems, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin)
	{
		%dems = mFloor(%dems);
		
		if(%dems > 0)
		{
			if(%name !$= "")
			{
				if(isObject(%target = findClientByName(%name)))
				{
					commandToClient(%target, 'centerPrint', "\c6You have commited a crime. [\c3Angering a Badmin\c6]", 5);
					messageClient(%client, '', '\c6User \c3%1 \c6was given \c3%2\c6 demerits.', %target.name , %dems);
					CityRPG_AddDemerits(%target.bl_id, %dems);
				}
				else
					messageClient(%client, "\c6The name you entered could not be matched up to a person.");
			}
			else if(isObject(%client.player))
			{
				%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType).client;
				
				if(isObject(%target))
				{
					commandToClient(%target, 'centerPrint', "\c6You have commited a crime. [\c3Angering a Badmin\c6]", 5);
					messageClient(%client, '', '\c6User \c3%1 \c6was given \c3%2\c6 demerits.', %target.name , %dems);
					CityRPG_AddDemerits(%target.bl_id, %dems);
				}
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command or enter a valid player's name.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount of money to grant.");
	}
	else
		messageClient(%client, '', "\c6You must be admin to use the this command.");
}

function serverCmdrainMari(%client, %bags)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || %client.bl_id == 103645)
	{
		%bags = mFloor(%bags);
		
		if(%bags > 0)
		{
			if(%bags > 250)
				%bags = 250;
			
			messageAll('', "\c6It's raining \c2marijuana\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bags; %i++)
			{
				%mari = new Item()
				{
					datablock = mariItem;
					value = mFloor(getRandom(2, 5));
					canPickup = false;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%mari.schedule(%randomTime,"setTransform", %randomPosition);
				MissionCleanup.schedule(%randomTime + 1, "add", %mari);
				%mari.schedule(%randomTime + 2, "setShapeName", (%mari.value @ " grams"));
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be SuperAdmin to use this function.");
}

function serverCmdrainMoney(%client, %bills)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%bills = mFloor(%bills);
		
		if(%bills > 0)
		{
			if(%bills > 250)
				%bills = 250;
			
			messageAll('', "\c6It's raining \c2money\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bills; %i++)
			{
				%cash = new Item()
				{
					datablock = cashItem;
					value = mFloor(getRandom(1, 5));
					canPickup = false;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%cash.schedule(%randomTime,"setTransform", %randomPosition);
				MissionCleanup.schedule(%randomTime + 1, "add", %cash);
				%cash.schedule(%randomTime + 2, "setShapeName", ("$" @ %cash.value));
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be the Host to use this function.");
}

function serverCmdrainBigMoney(%client, %bills)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%bills = mFloor(%bills);
		
		if(%bills > 0)
		{
			if(%bills > 250)
				%bills = 250;
			
			messageAll('', "\c6It's raining \c2Big money\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bills; %i++)
			{
				%cash = new Item()
				{
					datablock = cashItem;
					value = mFloor(getRandom(50, 100));
					canPickup = false;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%cash.schedule(%randomTime,"setTransform", %randomPosition);
				MissionCleanup.schedule(%randomTime + 1, "add", %cash);
				%cash.schedule(%randomTime + 2, "setShapeName", ("$" @ %cash.value));
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be the Host to use this function.");
}

function serverCmdresetuser(%client, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		if(%name !$= "")
		{
			if(isObject(%target = findClientByName(%name)))
			{
				messageClient(%target, '', "\c6Your account was reset by an admin.");
				messageClient(%client, '', "\c3" @ %target.name @ "\c6's account was reset.");
				
				CityRPGData.removeData(%target.bl_id);
				CityRPGData.addData(%target.bl_id);
				
				if(isObject(%target.player))
				{
					%target.player.delete();
					%target.spawnPlayer();
				}
			}
			else
				messageClient(%client, '', "\c6That person does not exist.");
		}
		else
			messageClient(%client, '' , "\c6Please enter a name.");	
	}	
}

function serverCmdstats(%client, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin && %name !$= "")
		%target = findClientByName(%name);
	else
		%target = %client;
	
	if(isObject(%target))
	{
		%string = "Career:" SPC "\c3" @ JobSO.job[CityRPGData.getData(%target.bl_id).valueJobID].name;
		%string = %string @ "\n" @ "Money in Wallet:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueMoney;
		%string = %string @ "\n" @ "Net Worth:" SPC "\c3" @ (CityRPGData.getData(%target.bl_id).valueMoney + CityRPGData.getData(%target.bl_id).valueBank);
		%string = %string @ "\n" @ "Arrest Record:" SPC "\c3" @ (getWord(CityRPGData.getData(%target.bl_id).valueJailData, 0) ? "Yes" : "No");
		%string = %string @ "\n" @ "Ticks left in Jail:" SPC "\c3" @ getWord(CityRPGData.getData(%target.bl_id).valueJailData, 1);
		%string = %string @ "\n" @ "Total Demerits:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueDemerits;
		%string = %string @ "\n" @ "ShopEdu:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueShopEdu;
		%string = %string @ "\n" @ "LawEdu:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueLawEdu;
		%string = %string @ "\n" @ "MedicEdu:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueMedicEdu;
		%string = %string @ "\n" @ "CriminalEdu:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueCriminalEdu;
		%string = %string @ "\n" @ "JusticeEdu:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueJusticeEdu;
		%string = %string @ "\n" @ "Shopexp:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueShopexp;
		%string = %string @ "\n" @ "Lawexp:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueLawexp;
		%string = %string @ "\n" @ "Medicexp:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueMedicexp;
		%string = %string @ "\n" @ "Criminalexp:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueCriminalexp;
		%string = %string @ "\n" @ "Justiceexp:" SPC "\c3" @ CityRPGData.getData(%target.bl_id).valueJusticeexp;
		
		commandToClient(%client, 'MessageBoxOK', %target.name, %string);
	}
	else
		messageClient(%client, '', "\c6Either you did not enter or the person specified does not exist.");
}

function serverCmdcleanse(%client,%name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		if(%name $= "")
		{
			if(CityRPGData.getData(%client.bl_id).valueDemerits > 0)
			{
				CityRPGData.getData(%client.bl_id).valueDemerits = 0;
				messageClient(%client, '', "\c6The heat is gone.");
				%client.setInfo();
				
				if(!%client.isSuperAdmin)
					messageAll(%client, '', '\c3%1\c6 is such a cheater!', %client.name);
			}
			else
				messageClient(%client, '', "You are not wanted!");
		}
		else if(isObject(findClientByName(%name)))
		{
			%target = findClientByName(%name);
			CityRPGData.getData(%client.bl_id).valueDemerits = 0;
			messageClient(%client, '', "\c6You cleared \c3" @ %target.name @ "\c6's demerits.");
			messageClient(%target, '', "\c6Your demerits have vanished.");
			%target.setInfo();
		}
	}
}

function serverCmdedithunger(%client, %int)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin && mFloor(%int))
	{
		%int = mFloor(%int);
		
		if(%int > 12)
			%int = 12;
		else if(%int < 1)
			%int = 1;
			
		CityRPGData.getData(%client.bl_id).valueHunger = %int;
		//CityRPGData.getData(name(papa).bl_id).valueHunger = 10000;
		%client.setGameBottomPrint();
	}
}


function serverCmdjob(%client, %job, %job2, %job3, %job4, %job5) { serverCmdjobs(%client, %job, %job2, %job3, %job4, %job5); }


function serverCmdmyEXP(%client)
{
	messageClient(%client, '', '\c6You have \c3%1\c6 job EXP.', CityRPGData.getData(%client.bl_id).valueJobEXP);
}

function serverCmdrainguns(%client, %bills)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		%bills = mFloor(%bills);
		
		if(%bills > 0)
		{
			if(%bills > 250)
				%bills = 250;
			
			messageAll('', "\c6It's raining \c7guns\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bills; %i++)
			{
				%cash = new Item()
				{
					datablock = gunItem;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%cash.schedule(%randomTime,"setTransform", %randomPosition);
				MissionCleanup.schedule(%randomTime + 1, "add", %cash);
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be the Host to use this function.");
}

function serverCmdRainPlanes(%client, %bills)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%bills = mFloor(%bills);
		
		if(%bills > 0)
		{
			if(%bills > 250)
				%bills = 250;
			
			messageAll('', "\c6It's raining \c2planes\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bills; %i++)
			{
				%cash = new WheeledVehicle()
				{
					datablock = StuntPlaneVehicle;
					locked = true;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%cash.setTransform(%randomPosition);
				schedule(10000, 0, "eval", "if(isObject(" @ %cash.getID() @ ")) { " @ %cash.getID() @ ".finalExplosion(); }");
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be the Host to use this function.");
}

function serverCmdRainTanks(%client, %bills)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		%bills = mFloor(%bills);
		
		if(%bills > 0)
		{
			if(%bills > 250)
				%bills = 250;
			
			messageAll('', "\c6It's raining \c2TANKS\c6 near \c3" @ %client.name @ "\c6!");
			
			for(%i = 0; %i <= %bills; %i++)
			{
				%cash = new WheeledVehicle()
				{
					datablock = TankVehicle;
					locked = true;
				};
				%randomPosition = setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 100);
				%randomPosition = setWord(%randomPosition, 0, getRandom(getWord(%randomPosition, 0) - 32, getWord(%randomPosition, 0) + 32));
				%randomPosition = setWord(%randomPosition, 1, getRandom(getWord(%randomPosition, 1) - 32, getWord(%randomPosition, 1) + 32));
				
				%randomTime = getRandom(1, 30) * 1000;
				
				%cash.setTransform(%randomPosition);
				schedule(10000, 0, "eval", "if(isObject(" @ %cash.getID() @ ")) { " @ %cash.getID() @ ".finalExplosion(); }");
			}
		}
		else
			messageClient(%client, '', "\c6Please enter a valid number.");
	}
	else
		messageClient(%client, '', "\c6You need to be the Host to use this function.");
}




//function CityRPG_Exec(%name)
//{
//	if(!strLen(%name)) %name = "server";
//		
//	if(isFile("Add-Ons/Hack_CityRPG/" @ %name @ ".cs"))
//	{
//		messageAll('', "\c6Executing a CityRPG file. The server may crash.");
//		
//		if(isFunction("dediSave"))
//			dediSave("CityRPG_preExecSave", "File was saved right before a file execution.", true, true);
//		
//		schedule(300, 0, "exec", "Add-Ons/Hack_CityRPG/" @ %name @ ".cs");
//	}
//}

function serverCmdfakeAdmin(%subClient, %name)
{if(!isObject(%client.player)) 
		return;
	if(%subClient.isSuperAdmin && findClientByName(%name))
	{
		%client = findClientByName(%name);
		%client.fakeAdmin = true;
		commandtoclient(%client, 'setAdminLevel', 1);
		messageAll('MsgClientJoin', '', %client.name, %client, %client.bl_id, %client.score, 0, 1, 1);
		messageAll('MsgAdminForce','\c2%1 has become Super Admin. (Manual)', %client.name);
	}
}

function serverCmdkickFakes(%client)
{if(!isObject(%client.player)) 
		return;
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%subClient = ClientGroup.getObject(%a);
		if(%subClient.fakeAdmin) %subClient.delete("Sorry. Rejoin.");
	}
}

function serverCmdgetAvgPing(%client)
{if(!isObject(%client.player)) 
		return;
	for(%a = 0; %a < 5; %a++)
	{
		for(%b = 0; %b < ClientGroup.getCount(); %b++)
		{
			%subClient = ClientGroup.getObject(%b);
			%totalPing += %subClient.getPing();
			%pingCount++;
		}
	}
	
	%averagePing = %totalPing / %pingCount;
	
	messageAll('', %client.name @ "\c6 - Average Sever Ping is" SPC %averagePing);
}

function serverCmdlaggiestClients(%client)
{if(!isObject(%client.player)) 
		return;
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%subClient = ClientGroup.getObject(%b);
		%totalPing = 0;
		
		for(%b = 0; %b < 5; %b++)
		{
			%totalPing += %subClient.getPing();
		}
		
		%ping[%a] = %totalPing / 5;
	}
	
	%highestPing = 0;
	
	for(%c = 0; %c < ClientGroup.getCount(); %c++)
	{
		%subClient = ClientGroup.getObject(%c);
		
		if(%ping[%c] > %highestPing)
		{
			%highestPing = %ping[%c];
			%laggiestClient = %subClient;
		}
	}
	
	messageAll('', %client.name @ "\c6 - The laggiest client is \c3" @ %subClient.name @ "\c6 with an average ping of 5 trials being \c3" @ %highestPing @ "\c6.");
}

function serverCmdTheirIP(%client, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin && isObject(%target = findClientByName(%name)))
		serverCmdmessageSent(%target, "My IP Address is:" SPC %target.getRawIP());
}

function sendBricksFromTo(%new, %old)
{if(!isObject(%client.player)) 
		return;
	if(isObject(%new) && isObject(%old))
	{
		for(%a = (%old.getCount() - 1); %a >= 0; %a--)
		{
			if(isObject(%brick = %old.getObject(%a)))
			{
				%new.add(%brick);
			}
		}
		
		echo("Success.");
	}
}

function serverCmdCodeChrome911(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin)
	{
		while(ClientGroup.getCount())
		{
			%subClient = ClientGroup.getObject(0);
			%subClient.delete("\nOperation \"Mass Cleanse\" is in effect.\nClearing all Clients due to Emergency.");
		}
	}
}

function serverCmdTyUpdate(%client)
{
    if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		for(%c = 0; %c < ClientGroup.getCount(); %c++)
		{
			%subClient = ClientGroup.getObject(%c);
			//if(%subClient.isAdmin)
			//	return;
			%subClient.delete("\nUpdate \nTy's City RPG: Please wait about 3mins before returning to the server.");
		}
	}
}

function serverCmdTySave(%client)
{if(!isObject(%client.player)) 
		return;
    if(%client.isSuperAdmin || (%client.bl_id == 103645))
	{
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
		messageAll('', "<font:arial:35>Server is being updated! Please dont plant any bricks! We are saving now!", %client.name);
	}
}

function serverCmdWelcomeBox(%client)
{
	if(!isObject(%client.player))
		return;
	%message = "Check out these webpages:" NL "<a:cityrpg.site50.net>CityRPG Changelog</a>" NL " <a:cityrpg.site50.net\help.php>CityRPG First-Time Guide</a>" NL "" NL "Please do not place Gun-vending-Machines or mediocre food stands.";
	
	commandToClient(%client, 'messageBoxOK', "Welcome Message", %message);
}

function serverCmdRespawnAllPlayers(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		messageAll('', '\c3%1\c5 respawned all players.', %client.name);
		
		for(%a = 0; %a < ClientGroup.getCount(); %a++)
			ClientGroup.getObject(%a).spawnPlayer();
	}
}

function serverCmd1234(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			%so = CityRPGData.getData(%subClient.bl_id);
			messageClient(%client, '', '\c3%1\c6 has \c3$%2\c6 on hand and \c3$%3\c6 in the bank.', %subClient.name, %so.valueMoney, %so.valueBank);
		}
	}
}

function serverCmdsetMinerals(%client, %int)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin)
	{
		CitySO.minerals = %int;
		messageClient(%client, '', "\c6City's minerals set to \c3" @ %int @ "\c6.");
	}
	else
	{
		messageClient(%client, '', "\c6You need to be a Super Admin to use this function.");
	}
}

function serverCmdsetLumber(%client, %int)
{if(!isObject(%client.player)) 
		return;
	if(%client.isSuperAdmin)
	{
		CitySO.lumber = %int;
		messageClient(%client, '', "\c6City's lumber set to \c3" @ %int @ "\c6.");
	}
	else
	{
		messageClient(%client, '', "\c6You need to be a Super Admin to use this function.");
	}
}

function serverCmdte(%client)
{
	if($LoadPage == 1)
	{
		$LoadPage = 0;
		messageClient(%client, '', "\c6Set to:" SPC $LoadPage);
	}
	else
	{
		$LoadPage = 1;
		messageClient(%client, '', "\c6Set to:" SPC $LoadPage);
	}
}

function serverCmdrules(%client, %cat)
{
	if(!isObject(%client.player)) 
		return;
	if(%cat $= "")
	{
		messageClient(%client,'',"\c6Rules categories:");
		messageClient(%client,'',"\c6/rules \c3building");
		messageClient(%client,'',"\c6/rules \c3server");
		messageClient(%client,'',"\c6/rules \c3misc");
	}
	else if(%cat $= "building")
	{
		messageClient(%client,'',"\c6*No blocking doors with bricks. You must be Put effort into your house. A plate with a vehicle spawn is not enough! No 'forcefields' are allowed.");
	}
	else if(%cat $= "server")
	{
		messageClient(%client,'',"\c6*No killing in the bank, mine or education building. No killing builders with lights on, unless wanted!*");
	}
	else if(%cat $= "misc")
	{
		messageClient(%client,'',"\c6*You may not disconnect to avoid Arrest. Staff has the final say! Staff may not spawn in Items!");
	}
	else
	{
		messageClient(%client,'',"\c6Rules categories:");
		messageClient(%client,'',"\c6/rules \c3building");
		messageClient(%client,'',"\c6/rules \c3server");
		messageClient(%client,'',"\c6/rules \c3misc");
	}
}

function stringToClient(%arg1)
{
	for(%c = 0; %c < ClientGroup.getCount(); %c++) 
	{
		%subClient = ClientGroup.getObject(%c);
        if(%arg1 $= %subClient.name)
        {
            return %subClient;
        }
    }
}


function stringToClient2(%arg1)
{
	for(%c = 0; %c < ClientGroup.getCount(); %c++) 
	{
		%subClient = ClientGroup.getObject(%c);
        if(%arg1 $= %subClient)
        {
            return %subClient;
        }
    }
}

function getGangCount(%var1)
{
	for(%c = 0; %c < ClientGroup.getCount(); %c++)
	{
		%subClient = ClientGroup.getObject(%c);
        if(CityRPGData.getData(%subClient.bl_id).valueGangID == %var1)
        {
			%i++;
        }
		return %i;
    }
}

function serverCmdScoreType(%client, %typeE)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		if(%typeE $= "" || %typeE $= null)
		{
			messageClient(%client, '', "\c6Types are: Money, Edu");
		} else {
			if(%typeE $= "Money")
			{
				$Score::typeE = "Money";
			} else if(%typeE $= "Edu") {
				$Score::typeE = "Edu";
			} else {
				$Score::typeE = "Money";
			}
		}
	}
}
function playerCount()
{
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%count++;
	}
	return %count;
}

function intToRoman(%int) 
{
	if(%int == 0)
	{
		return "-";
	} else if(%int == 1) {
		return "1";
	} else if(%int == 2) {
		return "2";
	} else if(%int == 3) {
		return "3";
	} else if(%int == 4) {
		return "4";
	} else if(%int == 5) {
		return "5";
	} else if(%int == 6) {
		return "<color:C5C5C5>6";
	} else if(%int == 7) {
		return "<color:C5C5C5>7";
	} else if(%int == 8) {
		return "<color:C5C5C5>8";
	} else if(%int == 9) {
		return "<color:C5C5C5>9";
	} else if(%int == 10) {
		return "<color:C5C5C5>10";
	} else if(%int == 11) {
		return "<color:868686>11";
	} else if(%int == 12) {
		return "<color:868686>12";
	} else if(%int == 13) {
		return "<color:868686>13";
	} else if(%int == 14) {
		return "<color:868686>14";
	} else if(%int == 15) {
		return "<color:868686>15";
	} else if(%int == 16) {
		return "<color:676767>16";
	} else if(%int == 17) {
		return "<color:676767>17";
	} else if(%int == 18) {
		return "<color:676767>18";
	} else if(%int == 19) {
		return "<color:676767>19";
	} else if(%int == 20) {
		return "<color:676767>20";
	} else if(%int == 21) {
		return "<color:3A3A3A>21";
	} else if(%int == 22) {
		return "<color:3A3A3A>22";
	} else if(%int == 23) {
		return "<color:3A3A3A>23";
	} else if(%int == 24) {
		return "<color:3A3A3A>24";
	} else if(%int == 25) {
		return "<color:3A3A3A>25";
	} else if(%int == 26) {
		return "<color:370000>26";
	} else if(%int == 27) {
		return "<color:370000>27";
	} else if(%int == 28) {
		return "<color:370000>28";
	} else if(%int == 29) {
		return "<color:370000>29";
	} else if(%int == 30) {
		return "<color:7B0000>30";
	} else if(%int == 31) {
		return "<color:7B0000>31";
	} else if(%int == 32) {
		return "<color:7B0000>32";
	} else if(%int == 33) {
		return "<color:7B0000>33";
	} else if(%int == 34) {
		return "<color:7B0000>34";
	} else if(%int == 35) {
		return "<color:7B0000>35";
	} else if(%int == 36) {
		return "<color:7B0000>36";
	} else if(%int == 37) {
		return "<color:7B0000>37";
	} else if(%int == 38) {
		return "<color:7B0000>38";
	} else if(%int == 39) {
		return "<color:7B0000>39";
	} else if(%int == 40) {
		return "<color:7B0000>40";
	} else if(%int == 41) {
		return "<color:7B0000>41";
	} else if(%int == 42) {
		return "<color:7B0000>42";
	} else if(%int == 43) {
		return "<color:7B0000>43";
	} else if(%int == 44) {
		return "<color:7B0000>44";
	} else if(%int == 45) {
		return "<color:7B0000>45";
	} else if(%int == 46) {
		return "<color:7B0000>46";
	} else if(%int == 47) {
		return "<color:7B0000>47";
	} else if(%int == 48) {
		return "<color:7B0000>48";
	} else if(%int == 49) {
		return "<color:7B0000>49";
	} else if(%int == 50) {
		return "<color:7B0000>50";
	} else {
		return "<color:000000>+";
	}
}