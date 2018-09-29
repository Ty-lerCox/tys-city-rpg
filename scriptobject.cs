//============================================================
//Project				:	CityRPG
//Author				:	Iban  & /Ty(ID997) (Edited by Gadgethm)
//Description				:	Your new ScriptObject overlords.
//============================================================
//Table of Contents
//1. JobSO
//2. CitySO
//3. CalendarSO
//4. ClothesSO
//5. WeatherSO
//============================================================

//============================================================
//Section 1 : JobsSO
//============================================================
function JobSO::populateJobs(%so)
{
	for(%a = 1; isObject(%so.job[%a]); %a++)
	{
		%so.job[%a].delete();
		%so.job[%a] = "";
	}
	
	//NOTE: Order is incredibly important. Jobs are referenced by ID, which is determined by order.
	//Mixing up the order of these professions will cause save data to reference the wrong job.
	%so.addJobFromFile("hobo");//1
	%so.addJobFromFile("grocer");//2
	%so.addJobFromFile("armsdealer");//3
	%so.addJobFromFile("shopkeeper");//4
	%so.addJobFromFile("shopowner");//5
	%so.addJobFromFile("shopceo");//6
	%so.addJobFromFile("medicalassistant");//7
	%so.addJobFromFile("medicalinturn");//8
	%so.addJobFromFile("doctor");//9
	%so.addJobFromFile("surgeon");//10
	%so.addJobFromFile("policeman");//11 
	%so.addJobFromFile("policelieutenant");//12 
	%so.addJobFromFile("policechief");//13
	%so.addJobFromFile("undercovercop");//15
	%so.addJobFromFile("private");//15
	%so.addJobFromFile("mastersergeant");//16
	%so.addJobFromFile("armylieutenant");//17
	%so.addJobFromFile("deaagent");//18
	%so.addJobFromFile("deaofficer");//19
	%so.addJobFromFile("criminal");//20
	%so.addJobFromFile("mobster");//21
	%so.addJobFromFile("mobboss");//22
	%so.addJobFromFile("godfather");//23
	%so.addJobFromFile("hitman");//24
	%so.addJobFromFile("bodyguard");//25
	%so.addJobFromFile("assistantmayor");//26
	%so.addJobFromFile("mayor");//27
	%so.addJobFromFile("moderatorcop");//28
	%so.addJobFromFile("moderatorcriminal");//29
	%so.addJobFromFile("donatorcop");//30
	%so.addJobFromFile("donatorcriminal");//31
	%so.addJobFromFile("sponsorcop");//32
	%so.addJobFromFile("sponsorcriminal");//33
	%so.addJobFromFile("administrator");//34
	%so.addJobFromFile("lawyer");//35
	%so.addJobFromFile("Bureaucrat");//36
	%so.addJobFromFile("SWATOperator");//37
}		

function JobSO::addJobFromFile(%so, %file)
{
	if(isFile("./jobs/" @ %file @ ".cs"))
	{
		%jobID = %so.getJobCount() + 1;
		exec("./jobs/" @ %file @ ".cs");
		%so.job[%jobID] = new scriptObject()
		{
			id		= %jobID;
			
			name		= $CityRPG::jobs::name;
			type		= $CityRPG::jobs::type;
			invest		= $CityRPG::jobs::initialInvestment;
			pay		= $CityRPG::jobs::pay;
			tools		= $CityRPG::jobs::tools;
			education	= $CityRPG::jobs::education;
			db		= $CityRPG::jobs::datablock;
			hostonly	= $CityRPG::jobs::hostonly;
			adminonly	= $CityRPG::jobs::adminonly;
			usepolicecars	= $CityRPG::jobs::usepolicecars;
			usecrimecars	= $CityRPG::jobs::usecrimecars;
			useparacars		= $CityRPG::jobs::useparacars;
			outfit		= $CityRPG::jobs::outfit;
			
			shopExp = $CityRPG::jobs::shopExp;
			lawExp = $CityRPG::jobs::lawExp;
			medicExp = $CityRPG::jobs::medicExp;
			crimExp = $CityRPG::jobs::crimExp;
			justiceExp = $CityRPG::jobs::justiceExp;
			
			sellItems	= $CityRPG::jobs::sellItems;
			sellFood	= $CityRPG::jobs::sellFood;
			sellServices 	= $CityRPG::jobs::sellServices; //Unused.
			sellClothes 	= $CityRPG::jobs::sellClothes;
			
			law		= $CityRPG::jobs::law;
			canPardon	= $CityRPG::jobs::canPardon;
			
			thief		= $CityRPG::jobs::thief;
			hideJobName	= $CityRPG::jobs::hideJobName;
			
			bountyOffer	= $CityRPG::jobs::offerer;
			bountyClaim	= $CityRPG::jobs::claimer;
			
			laborer		= $CityRPG::jobs::labor;
			
			tmHexColor	= $CityRPG::jobs::tmHexColor;
			helpline	= $CityRPG::jobs::helpline;
		};
		
		if(!isObject("CityRPGJob" @ %jobID @ "SpawnBrickData"))
		{
			datablock fxDtsBrickData(CityRPGSpawnBrickData : brickSpawnPointData)
			{
				category = "CityRPG";
				subCategory = "CityRPG Spawns";
				
				uiName = %so.job[%jobID].name SPC "Spawn";
				
				specialBrickType = "";
				
				CityRPGBrickType = 3;
				CityRPGBrickAdmin = true;
				
				spawnData = "jobSpawn" SPC %jobID;
			};
			
			CityRPGSpawnBrickData.setName("CityRPGJob" @ %jobID @ "SpawnBrickData");
		}
		
		deleteVariables("$CityRPG::jobs::*");
	}
}

function JobSO::getAnAlias(%so)
{
	%jobCount = 0;
	
	for(%a = 1; isObject(%so.job[%a]); %a++)
	{
		if(!%so.job[%a].hideJobName)
		{
			%jobCount++;
			%jobName[%jobCount] = %so.job[%a].name;
		}
	}
	
	if(%jobCount)
		return %jobName[getRandom(1, %jobCount)];
	else
		return "Gadgethm";
}

function JobSO::getJobCount(%so)
{
	for(%a = 0; isObject(%so.job[%a + 1]); %a++) { }
	return %a;
}

if(!isObject(JobSO))
{
	new scriptObject(JobSO) { };
	JobSO.populateJobs();
}

//============================================================
//Section 2 : CitySO
//============================================================
function CitySO::loadData(%so)
{
	if(isFile("config/server/CityRPG/CityRPG/City.cs"))
	{
		exec("config/server/CityRPG/CityRPG/City.cs");
		%so.minerals		= $CityRPG::temp::citydata::datumminerals;
		%so.lumber			= $CityRPG::temp::citydata::datumlumber;
		%so.economy			= $Economics::Condition;
		
	}
	else
	{
		%so.value["minerals"] = 0;
		%so.value["lumber"] = 0;
		%so.value["economy"] = 0;
	}
}

function CitySO::saveData(%so)
{
	$CityRPG::temp::citydata::datum["minerals"]		= %so.minerals;
	$CityRPG::temp::citydata::datum["lumber"]		= %so.lumber;
	export("$CityRPG::temp::citydata::*", "config/server/CityRPG/CityRPG/City.cs");
}

if(!isObject(CitySO))
{
	new scriptObject(CitySO) { };
	CitySO.loadData();
}

//============================================================
//Section 3 : CalendarSO
//============================================================
function CalendarSO::loadCalendar(%so)
{
	//Counters
	%so.numOfMonths = 12;
	%so.zbNumMonths = %so.numOfMonths - 1;
	
	//Names
	%so.nameOfMonth[0] = "January";
	%so.nameOfMonth[1] = "February";
	%so.nameOfMonth[2] = "March";
	%so.nameOfMonth[3] = "April";
	%so.nameOfMonth[4] = "May";
	%so.nameOfMonth[5] = "June";
	%so.nameOfMonth[6] = "July";
	%so.nameOfMonth[7] = "August";
	%so.nameOfMonth[8] = "September";
	%so.nameOfMonth[9] = "October";
	%so.nameOfMonth[10] = "November";
	%so.nameOfMonth[11] = "December";
	
	//Days
	%so.daysInMonth[0] = 31;
	%so.daysInMonth[1] = 28;
	%so.daysInMonth[2] = 31;
	%so.daysInMonth[3] = 30;
	%so.daysInMonth[4] = 31;
	%so.daysInMonth[5] = 30;
	%so.daysInMonth[6] = 31;
	%so.daysInMonth[7] = 31;
	%so.daysInMonth[8] = 30;
	%so.daysInMonth[9] = 31;
	%so.daysInMonth[10] = 30;
	%so.daysInMonth[11] = 31;
	
	//Holidays
	%so.holiday[1] = "\c2Happy New Year!";
	%so.holiday[8] = "\c6It's <color:FF9900>Ty's \c6birthday today!";
	%so.holiday[91] = "\c2A\c1p\c2r\c1i\c2l \c0F\c3o\c0o\c3l\c0s \c7D\c6a\c7y\c6!";
	%so.holiday[110] = "\c6It's \c24/20 \c6today.";
	%so.holiday[265] = "\c1It's \c5Random\c3 Day\c4!\c2!";
	%so.holiday[350] = "\c0H\c3a\c2p\c1p\c5y\c6 Holidays\c7!";
}

function CalendarSO::getDate(%so, %client)
{
	%ticks = %so.date;
	
	for(%a = 0; %ticks > %so.daysInMonth[%a % %so.numOfMonths]; %a++)
	{
		%ticks -= %so.daysInMonth[%a % %so.numOfMonths];
	}
	
	%year = mFloor(%a / %so.numOfMonths);
	
	//If the second number from last is a "1" (e.g. 12 or 516), the suffix will always be "th"
	if(strlen(%ticks) > 1 && getSubStr(%ticks, (strlen(%ticks) - 2), 1) $= "1")
	{
		%suffix = "th";
	}
	//If not, it can either be "st," "nd," "rd," or "th," depending on the last numeral.
	else
	{
		switch(getSubStr(%ticks, (strlen(%ticks) - 1), 1))
		{
			case 1: %suffix = "st";
			case 2: %suffix = "nd";
			case 3: %suffix = "rd";
			default: %suffix = "th";
		}
	}
	
	messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 of year \c3%3\c6, ...', %so.nameOfMonth[%so.getMonth()], %ticks, %year, %suffix);
	
	if(%so.holiday[%so.getCurrentDay()] !$= "")
		messageAll('', "\c6 -" SPC %so.holiday[%so.getCurrentDay()]);
		
	if(isObject(WeatherSO))
	{
		WeatherSO.todaysForecast();
	}
}

function CalendarSO::getMonth(%so)
{
	%ticks = %so.date;
	
	for(%a = 0; %ticks > %so.daysInMonth[%a % %so.numOfMonths]; %a++)
		%ticks -= %so.daysInMonth[%a % %so.numOfMonths];
	
	%month = %a % %so.numOfMonths;
	return %month;
}

function CalendarSO::dumpCalendar(%so)
{
	for(%a = 0; %so.daysInMonth[%a] !$= ""; %a++)
	{
		echo(%so.nameOfMonth[%a] SPC "has" SPC %so.daysInMonth[%a] SPC "days.");
	}
}

function CalendarSO::getYearLength(%so)
{
	for(%a = 0; %so.daysInMonth[%a] > 0; %a++)
	{
		%totalLength += %so.daysInMonth[%a];
	}
	
	return %totalLength;
}

function CalendarSO::getCurrentDay(%so)
{
	return (%so.date % %so.getYearLength());
}

function CalendarSO::loadData(%so)
{
	if(isFile("config/server/CityRPG/CityRPG/Calendar.cs"))
	{
		exec("config/server/CityRPG/CityRPG/Calendar.cs");
		%so.date = $CityRPG::temp::calendar::datumdate;
	}
	else
	{
		%so.date = 0;
	}
	
	%so.loadCalendar();
}

function CalendarSO::saveData(%so)
{
	$CityRPG::temp::calendar::datum["date"]	= %so.date;
	export("$CityRPG::temp::calendar::*", "config/server/CityRPG/CityRPG/Calendar.cs");
}

if(!isObject(CalendarSO))
{
	new scriptObject(CalendarSO) { };
	CalendarSO.schedule(1, "loadData");
}

//============================================================
//Section 4 : ClothesSO
//============================================================
function ClothesSO::loadClothes(%so)
{
	//Clothing Data
	%so.color["none"]		= "1 1 1 1";
	%so.node["none"]		= "0";
	
	//Outfits
	//Outfits use index instead of names.
	//Do not repeat indexes.
	//This is the order they appear in the GUI.
	%so.str[1]	= "none none none none whitet whitet skin bluejeans blackshoes default default";
	%so.uiName[1]	= "Default";
	%so.sellName[1]	= "Default Suit";

	%so.str[2]	= "none brownhat keep keep greenshirt greenshirt keep greyPants blackshoes default default";
	%so.uiName[2]	= "Basic";
	%so.sellName[2]	= "Basic Outfit";
	
	%so.str[3]	= "keep skullcap keep keep blackshirt blackshirt blackgloves blackPants blackshoes default default";
	%so.uiName[3]	= "Gimp";
	%so.sellName[3]	= "Gimp Suit";
	
	%so.str[4]	= "none none none none whitet redsleeve keep brightbluePants blueshoes default default";
	%so.uiName[4]	= "Blockhead";
	%so.sellName[4]	= "Blockhead Clothes";

	%so.str[5]	= "keep keep keep keep greenshirt greenshirt keep brownPants blackshoes default worm-sweater";
	%so.uiName[5]	= "Nerd";
	%so.sellName[5]	= "Nerd Suit";

	%so.str[6]	= "keep keep keep keep blackshirt blackshirt keep blackPants blackshoes default Mod-Suit";
	%so.uiName[6]	= "Business";
	%so.sellName[6]	= "Business Suit";

	%so.str[7]	= "keep keep keep keep blueshirt blueshirt keep bluePants blackshoes default Mod-Suit";
	%so.uiName[7]	= "Council";
	%so.sellName[7]	= "Council Suit";

	%so.str[8]	= "keep keep keep keep skingen skingen skingen skingen skingen default default";
	%so.uiName[8]	= "Naked";
	%so.sellName[8]	= "B-Day Suit";
	
	%so.str[9]	= "keep keep keep keep blackshirt blackshirt skingen blackpants blackshoes default Mod-Suit";
	%so.uiName[9]	= "Suit";
	%so.sellName[9]	= "Suit & Tie";
	
	%so.str[10]	= "DrKleiner DrKleiner DrKleiner DrKleiner whitet whitet brightbluegloves whitet blackshoes DrKleiner DrKleiner";
	%so.uiName[10]	= "Doctor";
	%so.sellName[10]	= "Doctor";
	
	//Hats
	%so.color["brownhat"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownhat"]	= "4";
	%so.str["brownhat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["piratehat"]	= "0.078 0.078 0.078 1";
	%so.node["piratehat"]	= "5";
	%so.str["piratehat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["copHat"]	= "0 0.141176 0.333333 1";
	%so.node["copHat"]	= "6";
	%so.str["copHat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["skullcap"]	= "0.200 0.200 0.200 1.000";
	%so.node["skullcap"]	= "7";
	%so.str["skullcap"]	= "keep this keep keep keep keep keep keep keep";

        %so.color["copHat2"]	= "0 0.000 0.500 0.250 1.000";
	%so.node["copHat2"]	= "8";
	%so.str["copHat2"]	= "keep this keep keep keep keep keep keep keep";
	
	//Gloves
	%so.color["blackgloves"] = "0.200 0.200 0.200 1.000";
	%so.node["blackgloves"]	= "0";
	%so.str["blackgloves"]	= "keep keep keep keep keep keep this keep keep";
	
	%so.color["brightbluegloves"] = "0.500 0.400 0.800 1.000";
	%so.node["brightbluegloves"]	= "0";
	%so.str["brightbluegloves"]	= "keep keep keep keep keep keep this keep keep";
	
	//Shirts
	%so.color["pinkt"]	= "1 0.75 0.79 1";
	%so.node["pinkt"]	= "gender";
	%so.str["pinkt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["greyShirt"]	= "0.000 0.000 0.000 1.000";
	%so.node["greyShirt"]	= "gender";
	%so.str["greyShirt"]	= "keep keep keep keep this skingen keep keep keep";
	
	%so.color["whitet"]	= "1 1 1 1";
	%so.node["whitet"]	= "gender";
	%so.str["whitet"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["copShirt"]	= "0 0.141176 0.333333 1";
	%so.node["copShirt"]	= "gender";
	%so.str["copShirt"]	= "keep keep keep keep this this keep keep keep";

	%so.color["jumpsuit"]	= "1 0.617 0 1";
	%so.node["jumpsuit"]	= "gender";
	%so.str["jumpsuit"]	= "keep keep keep keep this this keep this this";
	
	%so.color["blackshirt"]	= "0.200 0.200 0.200 1.000";
	%so.node["blackshirt"]	= "gender";
	%so.str["blackshirt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["brownshirt"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownshirt"]	= "gender";
	%so.str["brownshirt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["greenshirt"]	= "0.00 0.262 0.00 1";
	%so.node["greenshirt"]	= "gender";
	%so.str["greenshirt"]	= "keep keep keep keep this this keep keep keep";

	%so.color["blueshirt"]	= "0.0 0.141 0.333 1";
	%so.node["blueshirt"]	= "gender";
	%so.str["blueshirt"]	= "keep keep keep keep this this keep keep keep";

	//Pants
	%so.color["bluejeans"]	= "0 0.141 0.333 1";
	%so.node["bluejeans"]	= "0";
	%so.str["bluejeans"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["blackPants"] = "0.200 0.200 0.200 1.000";
	%so.node["blackPants"]	= "0";
	%so.str["blackPants"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["brownPants"] = "0.329 0.196 0.000 1.000";
	%so.node["brownPants"]	= "0";
	%so.str["brownPants"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["greyPants"] = "0.000 0.000 0.000 1.000";
	%so.node["greyPants"]	= "0";
	%so.str["greyPants"]	= "keep keep keep keep keep keep keep this keep";

	%so.color["brightbluePants"] = "0.200 0.000 0.800 1.000";
	%so.node["brightbluePants"]	= "0";
	%so.str["brightbluePants"]	= "keep keep keep keep keep keep keep this keep";

	%so.color["bluePants"] = "0.0 0.141 0.333 1";
	%so.node["bluePants"]	= "0";
	%so.str["bluePants"]	= "keep keep keep keep keep keep keep this keep";

	//Shoes
	%so.color["blackshoes"]	= "0.200 0.200 0.200 1.000";
	%so.node["blackshoes"]	= "0";
	%so.str["blackshoes"]	= "keep keep keep keep keep keep keep keep this";

	%so.color["brownshoes"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownshoes"]	= "0";
	%so.str["brownshoes"]	= "keep keep keep keep keep keep keep keep this";

	%so.color["blueshoes"]	= "0.000 0.000 0.000 1.000";
	%so.node["blueshoes"]	= "0";
	%so.str["bluehoes"]	= "keep keep keep keep keep keep keep keep this";

	//Misc
	%so.color["redsleeve"] = "0.900 0.220 0.000 1.000";
	%so.node["redsleeve"] = "gender";
	%so.color["redsleeve"] = "keep keep keep keep this this keep keep keep";
}

function ClothesSO::postEvents(%so)
{
	%str = "list";
	
	for(%a = 1; %so.str[%a] !$= ""; %a++)
		%str = %str SPC %so.uiName[%a] SPC %a;
	
	if(%str !$= "")
	{
		registerOutputEvent("fxDTSBrick", "sellClothes", %str TAB "int 0 500 1");
		
		for(%b = 0; %b < ClientGroup.getCount(); %b++)
		{
			%subClient = ClientGroup.getObject(%b);
			serverCmdRequestEventTables(%subClient);
			messageClient(%subClient, '', "\c6Your Event Tables have been updated. If you do not know what that means, ignore this message.");
		}
	}
}

function ClothesSO::getColor(%so, %client, %item)
{
	if(%item $= "skin" || %item $= "skingen")
		return %client.headColor;
	else
	{
		%color = %so.color[%item];
		return %color;
	}
}

function ClothesSO::getNode(%so, %client, %item)
{
	if(%item $= "skin")
		return 0;
	else if(%item $= "skingen")
		return (%client.gender $= "Male" ? 0 : 1);
	else
	{
		%node = %so.node[%item];
	
		if(%node $= "gender" || %node $= "skingen")
			return (%client.gender $= "Male" ? 0 : 1);
		else
			return %node;
	}
}

function ClothesSO::getDecal(%so, %client, %segment, %item)
{
	if(%item $= "" || %item $= "default")
	{
		if(%segment $= "face")
			return "smiley";
		else if(%segment $= "chest")
			return "AAA-none";
	}
	else
		return %item;
}

function ClothesSO::giveItem(%so, %client, %item)
{
	if(strLen(%so.str[%item]) && isObject(%client))
	{
		%outfit = CityRPGData.getData(%client.bl_id).valueOutfit;
		
		for(%a = 0; %a < getWordCount(%outfit); %a++)
		{
			if(getWord(%so.str[%item], %a) $= "keep")
				%newOutfit = (%newOutfit $= "" ? getWord(%outfit, %a) : %newOutfit SPC getWord(%outfit, %a));
			else if(getWord(%so.str[%item], %a) $= "this")
				%newOutfit = (%newOutfit $= "" ? %item : %newOutfit SPC %item);
			else
				%newOutfit = (%newOutfit $= "" ? getWord(%so.str[%item], %a) : %newOutfit SPC getWord(%so.str[%item], %a));
		}
		
		CityRPGData.getData(%client.bl_id).valueOutfit = %newOutfit;
		%client.applyBodyParts();
		%client.applyBodyColors();
	}
}

if(!isObject(ClothesSO))
{
	new scriptObject(ClothesSO) { };
	ClothesSO.schedule(1, "loadClothes");
	ClothesSO.schedule(1, "postEvents");
}

//============================================================
//Section 5 : WeatherSO
//============================================================
function WeatherSO::loadForecast(%so)
{
	//Rain Probability
	%so.rainProb[0] = 5; //"January";
	%so.rainProb[1] = 10; //"February";
	%so.rainProb[2] = 15; //"March";
	%so.rainProb[3] = 20; //"April";
	%so.rainProb[4] = 30; //"May";
	%so.rainProb[5] = 40; //"June";
	%so.rainProb[6] = 50; //"July";
	%so.rainProb[7] = 40; //"August";
	%so.rainProb[8] = 30; //"September";
	%so.rainProb[9] = 20; //"October";
	%so.rainProb[10] = 15; //"November";
	%so.rainProb[11] = 10; //"December";
	
	//Snow Probability
	%so.snowProb[0] = 30; //"January";
	%so.snowProb[1] = 15; //"February";
	%so.snowProb[2] = 5; //"March";
	%so.snowProb[3] = 0; //"April";
	%so.snowProb[4] = 0; //"May";
	%so.snowProb[5] = 0; //"June";
	%so.snowProb[6] = 0; //"July";
	%so.snowProb[7] = 0; //"August";
	%so.snowProb[8] = 0; //"September";
	%so.snowProb[9] = 5; //"October";
	%so.snowProb[10] = 15; //"November";
	%so.snowProb[11] = 30; //"December";
}

function WeatherSO::todaysForecast(%so)
{
	%month = CalendarSO.getMonth();
	
	%snowProb = %so.snowProb[%month];
	%rainProb = %so.rainProb[%month];
	%clearProb = 100 - (%so.snowProb[%month] + %so.rainProb[%month]);
	
	%snowPow = getRandom(0, %snowProb);
	%rainPow = getRandom(0, %rainProb);
	if(%clearProb > 0)
		%clearPow = getRandom(0, %clearProb);
	
	if(%snowPow > %rainPow && %snowPow > %clearPow)
		%forecast = "snow";
	else if(%rainPow > %snowPow && %rainPow > %clearPow)
		%forecast = "rain";
	else
		%forecast = "";
	
	if(%forecast !$= "")
	{
		%intensity = getRandom(0, 15);
		
		if(%forecast $= %so.currWeather)
			%verb = "remains";
		else
		{
			%so.currWeather = %forecast;
			%verb = "is";
		}
		
		%intensity = getRandom(5, 15);
		
		switch$(%forecast)
		{
			case "snow":
				%so.generateWeather("snow", %intensity);
				messageAll('', '\c6 - Today\'s forecast %1 Snowy.', %verb);
			case "rain":
				%so.generateWeather("rain", %intensity);
				messageAll('', '\c6 - Today\'s forecast %1 Rainy.', %verb);
			default:
				%so.generateWeather("", 0);
				messageAll('', '\c6 - Today\'s forecast %1 \c0INVALID\c6.', %verb);
		}
	}
	else
	{
		messageAll('', "\c6 - Today's forecast is Clear.");
		%so.generateWeather("", 0);
	}
}

function WeatherSO::generateWeather(%so, %weather, %intensity)
{
	warn("WEATHER ADJUSTING TO:" SPC "[" @ %weather @ "] WITH INTENSITY OF [" @ %intensity @ "]");
	
	if(isObject(Precipitation))
		Precipitation.delete();
	
	$CityRPG_Temp::CurrWeather = %weather;
	
	if(%weather !$= "" && (%intensity = mFloor(%intensity)) > 0)
	{		
		switch$(%weather)
		{
			case "snow":
				%db = "SnowA";
				%mutliplier = 0.15;
			case "rain":
				%db = "CityRPG_Rain";
				%mutliplier = 0.5;
			default:
				%db = "SnowA";
				%mutliplier = 0.15;
		}
		
		%intensity = %intensity * %mutliplier;
		//Default Properties
		%numDrops = 500 * %intensity;
		%minSpeed = 0.5 * %intensity;
		%maxSpeed = 1 * %intensity;
		%minMass = 0.75 * %intensity;
		%maxMass = 0.85 * %intensity;
		%maxTurbulence = 0.1 * %intensity;
		%turbulenceSpeed = 0.5 * %intensity;
		
		new Precipitation(Precipitation)
		{
			dataBlock = %db;
			numDrops = %numDrops;
			
			position = "191.844 621.068 566.484";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			doCollision = "1";
			
			boxWidth = "100";
			boxHeight = "100";
			
			minSpeed = %minSpeed;
			maxSpeed = %maxSpeed;
			
			minMass = %minMass;
			maxMass = %maxMass;
			
			useTurbulence = "false";
			maxTurbulence = %maxTurbulence;
			turbulenceSpeed = %turbulenceSpeed;
			rotateWithCamVel = "false";
		};
		
		Sky.setName("Sky_Old");
		switch$(%weather)
		{
			case "rain":
				new Sky(Sky)
				{
					materialList = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Rain/resource.dml";
					position = "336 136 0";
					rotation = "1 0 0 0";
					scale = "1 1 1";
					cloudHeightPer[0] = "0.6";
					cloudHeightPer[1] = "0.3";
					cloudHeightPer[2] = "0.4";
					cloudSpeed1 = "0.003";
					cloudSpeed2 = "0.001";
					cloudSpeed3 = "0.0001";
					visibleDistance = "140";
					fogDistance = "40";
					fogColor = "0.250000 0.250000 0.300000 1.000000";
					fogStorm1 = "0";
					fogStorm2 = "0";
					fogStorm3 = "0";
					fogVolume1 = "0 0 0";
					fogVolume2 = "0 0 0";
					fogVolume3 = "0 0 0";
					fogVolumeColor1 = "128.000000 128.000000 128.000000 -222768174765569860000000000000000000000.000000";
					fogVolumeColor2 = "128.000000 128.000000 128.000000 0.000000";
					fogVolumeColor3 = "128.000000 128.000000 128.000000 -170698929442160050000000000000000000000.000000";
					windVelocity = "0.5 0 0";
					windEffectPrecipitation = "1";
					SkySolidColor = "0.000000 0.000000 0.100000 1.000000";
					useSkyTextures = "1";
					renderBottomTexture = "0";
					noRenderBans = "0";
					locked = "true";
				};
			case "snow":
				new Sky(Sky)
				{
					position = "312 96 0";
					rotation = "1 0 0 0";
					scale = "1 1 1";
					materialList = "base/data/skies/Sky_Spooky3/resource.dml";
					cloudHeightPer[0] = "0.349971";
					cloudHeightPer[1] = "0.3";
					cloudHeightPer[2] = "0.199973";
					cloudSpeed1 = "0.0005";
					cloudSpeed2 = "0.001";
					cloudSpeed3 = "0.0003";
					visibleDistance = "1500";
					fogDistance = "1000";
					fogColor = "0.900000 0.900000 0.900000 1.000000";
					fogStorm1 = "0";
					fogStorm2 = "0";
					fogStorm3 = "0";
					fogVolume1 = "0 0 0";
					fogVolume2 = "0 0 0";
					fogVolume3 = "0 0 0";
					fogVolumeColor1 = "0.000000 0.000000 0.000000 1.000000";
					fogVolumeColor2 = "0.000000 0.000000 0.000000 1.000000";
					fogVolumeColor3 = "0.000000 0.000000 0.000000 1.000000";
					windVelocity = "0.25 0.25 0";
					windEffectPrecipitation = "1";
					SkySolidColor = "0.600000 0.600000 0.600000 1.000000";
					useSkyTextures = "1";
					renderBottomTexture = "0";
					noRenderBans = "0";
				};
			
		}
		Sky_Old.delete();
	}
	else
	{
		Sky.setName("Sky_Old");
		new Sky(Sky)
		{
			position = "336 136 0";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			materialList = "Add-Ons/Gamemode_TysCityRPG/shapes/Skies/Clear/resource.dml";
			cloudHeightPer[0] = "0.349971";
			cloudHeightPer[1] = "0.3";
			cloudHeightPer[2] = "0.199973";
			cloudSpeed1 = "0.0005";
			cloudSpeed2 = "0.001";
			cloudSpeed3 = "0.0003";
			visibleDistance = "5000";
			fogDistance = "800";
			fogColor = "0.900000 0.900000 1.000000 1.000000";
			fogStorm1 = "0";
			fogStorm2 = "0";
			fogStorm3 = "0";
			fogVolume1 = "0 0 0";
			fogVolume2 = "0 0 0";
			fogVolume3 = "0 0 0";
			fogVolumeColor1 = "128.000000 128.000000 128.000000 -222768174765569860000000000000000000000.000000";
			fogVolumeColor2 = "128.000000 128.000000 128.000000 0.000000";
			fogVolumeColor3 = "128.000000 128.000000 128.000000 -170698929442160050000000000000000000000.000000";
			windVelocity = "0 0 0";
			windEffectPrecipitation = "0";
			SkySolidColor = "0.000000 0.000000 0.100000 1.000000";
			useSkyTextures = "1";
			renderBottomTexture = "0";
			noRenderBans = "0";
			locked = "true";
		};	
		Sky_Old.delete();		
	}
}

if(!isObject(WeatherSO))
{
	//new scriptObject(WeatherSO) { };
	//WeatherSO.loadForecast();
}