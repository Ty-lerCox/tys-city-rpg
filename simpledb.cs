//    _____ Mr. Doom's __    ___  ___ 
//   / __(_)_ _  ___  / /__ / _ \/ _ )
//  _\ \/ /  ' \/ _ \/ / -_) // / _  |
// /___/_/_/_/_/ .__/_/\__/____/____/ 
// -----------/_/---------- Version 3
//
// +============================+
// | Mr. Doom's Simple Database |
// +============================+
//
// +===========================================================+
// | VERSION	: 3					       |
// | REVISION	: C					       |
// | RELEASE	: FINAL - Redistributable		       |
// +-----------------------------------------------------------+
// | If you use this. don't change this file and give me       |
// | some damned credit.				       |
// +-----------------------------------------------------------+
// | All other rights reserved by author.		       |
// +===========================================================+
// SimpleDB 3 - Built as a replacement for SASSY in CityRP
// 	      - Became a general purpose DB




// These are custom debugging systems I have added for, of course debugging and performance testing
// Anything starting with an if debug stuff is of course not essential to the script (but they may help you understand what's going on)
$SimpleDB::Debug=0; // (0 - Off, 1 - Warn, 2 - All) Major Errors will always report
$SimpleDB::TimeDebug=0;


//-- DO NOT EDIT BELOW THIS LINE --//
$SimpleDB=3;
//****************************
// CORE SDB FUNCTIONS
//****************************
// To access or change data use DBname.data[%key,%field]

// Key is BL_ID or similar unique DB key
// Field is field of key (row) you want to call


// This function is automatically called when the object is intialized

function SimpleDB::onAdd(%this)
{
	if(%this.getName() $= "")
	{
		error("SimpleDB: ScriptObject for SimpleDB has no name!!! Aborting...");
		%this.schedule(0, "delete");
		return false;
	}
	if($server::lan)
		warn("SimpleDB: Warning Do not use BL_ID\'s as keys while on lan!");
	if(%this.saveFile $= "")
	{
		error("SimpleDB: No SaveFile Specified to SimpleDB instance \"" @ %this.getName() @ "\"!!! Aborting...");
		%this.schedule(0, "delete");
		return false;
	}
	if(isFile(%this.saveFile) && %this.autoLoad)
	{
		if($SimpleDB::Debug >= 2) //echo("SimpleDB: Instance \"" @ %this.getName() @ "\" inital save file load...");
		if(%this.loadData() $= 1)
			%this.hasloaded = 1;
	}
	return true;
}

function SimpleDB::AddUser(%this,%key)
{
	// Checks Sector -- Test Input
	if(%key $= "")
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".AddUser() Key Argument Not Specified! Aborting...");
		return false;
	}
	if(%this.ChkUser(%key))
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".AddUser() User with Key \""@%key@"\" already exists! Aborting...");
		return false;
	}

	// Main Sector -- Add The User To Database
	%this.keys = %this.keys SPC %key;
	for(%i = 0; %i < getWordCount(%this.fields); %i++)
	{
		%field = getword(%this.fields, %i);
		%this.data[%key,%field] = %this.fields[%field];
	}
}
function SimpleDB::ChkUser(%this,%key)
{
	return %this.ExistUser(%key);
}
// New Function to deal with overhead of Loops in BLID Database
// v3 Accuracy has not been found to fail
function SimpleDB::ExistUser(%this,%key)
{
	// Checks Sector -- Test Input
	if(%key $= "")
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".ExistUser() Key Argument Not Specified! Aborting...");
		return 0;
	}
	%data = striPos(%this.keys,%key);
	if(%data >= 0)
		return 1;
	return 0;
}
function SimpleDB::RemUser(%this,%key)
{
	// Checks Sector -- Test Input
	if(%key $= "")
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".RemUser() Key Argument Not Specified! Aborting...");
		return false;
	}
	%data = striPos(%this.keys,%key);
	if(%data < 0)
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".RemUser() User with Key \""@%key@"\" does not exist! Aborting...");
		return false;
	}
	%this.keys = strReplace(" "@%this.keys@" "," "@%key@" "," ");
	return true;
}

function SimpleDB::SaveTick(%this,%time)
{
	if(!%this.autosave)
		return;
	if(!%time)
	{
		%time = 10;
		if($SimpleDB::Debug) warn(%this.getName()@"::SaveTick() - Time not set, defaulting to 10 (minutes)");
	} else if(%time < 0 || !isInteger(%time))
	{
		%time = 10;
		if($SimpleDB::Debug) warn(%this.getName()@"::SaveTick() - Invalid Time, Time must be a non-negative integer! (defaulting to 10 minutes)");
	} else if(%time > 30 && $SimpleDB::Debug >= 2) warn(%this.getName()@"::SaveTick() - WARNING: Time > 30 minutes, data stability is low.");
	if($SimpleDB::Debug >= 2) warn(%this.getName()@"::SaveTick() - Tick!"); 
	%this.SaveData();
	%this.schedule((%time*60000), "SaveTick", %time);
}
function SimpleDB::SaveData(%this)
{
	if($SimpleDB::TimeDebug) $SimpleDB::Time::SaveData=getSimTime();
	if(!isFile(%this.saveFile))
	{
		if($SimpleDB::Debug) warn(%this.getName()@"::SaveData() - Previous saveFile does not exist ("@%this.saveFile@"). will save in SimpleDB mode.");
		return %this.saveSimpleDB();
	}

	%file = new fileObject();
	%file.openForRead(%this.saveFile);
	
	%line = %file.readLine();
	if(getWord(%line, 0) $= "!")
	{
		%file.close();
		%file.delete();
		if($SimpleDB::Debug >= 2) warn(%this.getName()@"::SaveData() - saveFile represents that of a SimpleDB Save, saving in SimpleDB.");
		return %this.saveSimpleDB();
	}
	else if(getWord(%line, 0) $= "values")
	{
		%file.close();
		%file.delete();
		if($SimpleDB::Debug >= 2) warn(%this.getName()@"::SaveData() - saveFile represents that of a Sassy Save, saving in Sassy.");
		return %this.saveSassy();
	}
	else
	{
		error(%this.getName()@"::SaveData() - saveFile does not represent a format SimpleDB can read, Aborting...");
		return 0;
	}
}
function SimpleDB::SaveSimpleDB(%this)
{
	if($SimpleDB::TimeDebug) $SimpleDB::Time::SaveSimpleDB=getSimTime();
	%file = new fileObject();
	%file.openForWrite(%this.saveFile);
	
	%this.fields = trim(%this.fields);
	%this.keys = trim(%this.keys);

	for(%i = 0; %i < getWordCount(%this.fields); %i++)
	{
		%data = %data @ "$" @ getword(%this.fields, %i) @ "=" @ %this.fields[getword(%this.fields, %i)] @ ";";
	}
	%file.writeLine("! INDEX" @ %data);
	%data = "";
	
	for(%l = 0; %l < getWordCount(%this.keys); %l++)
	{
		%key = getWord(%this.keys, %l);
		
			for(%m = 0; %m < getWordCount(%this.fields); %m++)
			{
				%buffer = %buffer @ "$" @ getWord(%this.fields, %m) @ "=" @ %this.data[%key,getWord(%this.fields, %m)] @ ";";
			}

		%file.writeLine("!" SPC %key @ %buffer);
		%buffer = "";
	}
	
	%file.close();
	%file.delete();
	if($SimpleDB::TimeDebug) warn("SaveSimpleDB - TIME:"@(getSimTime()-$SimpleDB::Time::SaveSimpleDB));
	return 1;
}

function SimpleDB::saveSassy(%this)
{
	if($SimpleDB::TimeDebug) $SimpleDB::Time::SaveSassy=getSimTime();
	%file = new fileObject();
	%file.openForWrite(%this.saveFile);
	
	%file.writeLine("values");
	
	for(%a = 0; %a < getWordCount(%this.fields); %a++)
	{
		%file.writeLine(" " @ getWord(%this.fields,%a) SPC %this.fields[getWord(%this.fields,%a)]);
	}
	
	if(getWordCount(%this.keys) > 0)
	{
		%file.writeLine("");
	}
	
	for(%b = 0; %b < getWordCount(%this.keys); %b++)
	{
		if(%this.chkUser(getWord(%this.keys, %b)) < 0)
		{
			continue;
		}
		
		%file.writeLine("ID " @ getWord(%this.keys, %b));
		
		for(%c = 0; %c < getWordCount(%this.fields); %c++)
		{
			%file.writeLine(" " @ getWord(%this.fields, %c) SPC %this.data[getWord(%this.keys, %b),getWord(%this.fields, %c)]);
		}
		
		if(%b < getWordCount(%this.keys))
		{
			%file.writeLine("");
		}
	}
	
	%file.close();
	%file.delete();
	if($SimpleDB::TimeDebug) warn("SaveSassy - TIME:"@(getSimTime()-$SimpleDB::Time::SaveSassy));
	return 1;
}
function SimpleDB::loadData(%this,%force)
{
	if($SimpleDB::TimeDebug) $SimpleDB::Time::LoadData=getSimTime();
	if(!isFile(%this.saveFile))
	{
		error("SimpleDB: Save file '" @ %this.saveFile @ "' could not be found. exiting.");
		return 0;
	}

	if(%this.hasloaded && !%force)
	{

		if($SimpleDB::Debug) warn("SimpleDB: Save File has already been loaded, To force load and overwrite use \""@%this.getName()@".loadData(1)\"");
		return 0;
	}

	if(%this.convert && isFile(%this.sassyFile))
	{
		%this.convert = 0;
		%this.tempsave = %this.saveFile;
		%this.saveFile = %this.sassyFile;
		%this.sassyFile = "";
		if(%this.loadSassy() == 1)
		{
			%this.saveFile = %this.tempsave;
			if(%this.saveSimpleDB() == 0)
				return 0;
		}
		else
		{
			error("SIMPLEDB SASSY CONVERSION FAILURE");
			return 0;
		}
		return 1;
	}

	%file = new fileObject();
	%file.openForRead(%this.saveFile);
	
	%line = %file.readLine();
	if(getWord(%line, 0) $= "!")
	{
		%file.close();
		%file.delete();
		if($SimpleDB::TimeDebug) warn("LoadData - TIME:"@(getSimTime()-$SimpleDB::Time::LoadData));
		return %this.loadSimpleDB();
	}
	else if(getWord(%line, 0) $= "values")
	{
		%file.close();
		%file.delete();
		if($SimpleDB::TimeDebug) warn("LoadData - TIME:"@(getSimTime()-$SimpleDB::Time::LoadData));
		return %this.loadSassy();
	}
	else
	{
		error(%this.getName()@"::SaveData() - saveFile does not represent a format SimpleDB can read, Aborting...");
		return 0;
	}
}
function SimpleDB::loadSimpleDB(%this)
{
	if($SimpleDB::TimeDebug) $SimpleDB::Time::LoadSimpleDB=getSimTime();
	%file = new fileObject();
	%file.openForRead(%this.saveFile);
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		if($SimpleDB::Debug >= 2) warn("LINE:" SPC %line);
		if($SimpleDB::Debug >= 2) warn("GETWORD-LINE-0:" SPC getword(%line, 0));
		if($SimpleDB::Debug >= 2) warn("GETWORD-KEY:" SPC getSubStr(%line,(striPos(%line, "!") + 1),striPos(%line, "$") - 1));
		if(getword(%line, 0) $= "!")
		{
			%key = trim(getSubStr(%line, striPos(%line, "!") + 1, striPos(%line, "$") - 1));
			if($SimpleDB::Debug >= 2) warn("REAL KEY:" SPC %key);
			%line = getSubStr(%line, (striPos(%line, "!") + 1 + strlen(%key)),strLen(%line));

			// INDEX processor
			if(%key $= "INDEX")
			{
				// This SHOULD deal with all fields
				while(strLen(trim(%line)) > 5)
				{
					if(%Overrun <= 150)
						%Overrun++;
					else
					{
						error("SimpleDB: Possible Crash Adverted");
						return 0;
					}
					
					if($SimpleDB::Debug >= 2) warn("While Tick -- INDEX!");
						%fullstring = getSubStr(%line,striPos(%line, "$"),striPos(%line, ";"));
					if($SimpleDB::Debug >= 2) warn("FULLSTRING:" SPC %fullstring);
						%field = getSubStr(%fullstring, striPos(%fullstring, "$") + 1, striPos(%fullstring, "=") - striPos(%fullstring, "$") - 1);
					if($SimpleDB::Debug >= 2) warn("FIELD:" SPC %field);
						%this.fields = trim(%this.fields SPC %field);
					if($SimpleDB::Debug >= 2) warn("THIS.FIELDS:" SPC %this.fields);
						%this.fields[%field] = trim(getSubStr(%fullstring, striPos(%fullstring, "=") + 1, striPos(%fullstring, ";") - striPos(%fullstring, "=") - 1));
					if($SimpleDB::Debug >= 2) warn("VALUE:" SPC %this.fields[%field]);
						%line = strReplace(%line,%fullstring,"");
					if($SimpleDB::Debug >= 2) warn("FINAL-LINE:" SPC %line);
				}
				%Overrun = 0;
			}
			else
			{
				%this.keys = trim(%this.keys SPC %key);

				// This SHOULD deal with all fields for KEY
				while(strLen(trim(%line)) > 5)
				{
					if(%Overrun <= 150)
						%Overrun++;
					else
					{
						error("SimpleDB: Possible Crash Adverted");
						return 0;
					}
				if($SimpleDB::Debug >= 2) warn("While Tick -- DATA! "@%key);
					%fullstring = getSubStr(%line,striPos(%line, "$"),striPos(%line, ";"));
					%field = getSubStr(%fullstring, striPos(%fullstring, "$") + 1, striPos(%fullstring, "=") - striPos(%fullstring, "$") - 1);
					%this.data[%key,%field] = trim(getSubStr(%fullstring, striPos(%fullstring, "=") + 1, striPos(%fullstring, ";") - striPos(%fullstring, "=") - 1));
					%line = strReplace(%line,%fullstring,"");
				}
				%Overrun = 0;
			}
		}
	}
	
	%file.close();
	%file.delete();
	if($SimpleDB::TimeDebug) warn("LoadSimpleDB - TIME:"@(getSimTime()-$SimpleDB::Time::LoadSimpleDB));
	return 1;
}

function SimpleDB::loadSassy(%this)
{	
	if($SimpleDB::TimeDebug) $SimpleDB::Time::LoadSassy=getSimTime();
	%file = new fileObject();
	%file.openForRead(%this.saveFile);
	
	%valueListFound = false;
	%defaultValueListFound = false;
	%this.curkey = 0;
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		
		if(%line $= "")
		{
			%currentState = "";
			
			continue;
		}
		
		if(getSubStr(%line, 0, 1) !$= " ")
		{
			%currentState = getWord(%line, 0);
			
			if(getWord(%line, 0) $= "ID")
			{
				if(%this.chkUser(getWord(%line, 1)) < 0)
				{
					%this.keys = trim(%this.keys SPC getWord(%line, 1));
					%this.curkey = trim(getWord(%line, 1));
				}
			}
		}
		
		if(getSubStr(%line, 0, 1) $= " ")
		{
			if(%currentState $= "values")
			{
				%this.fields = trim(%this.fields SPC getWord(%line, 1));
				%this.fields[getWord(%line, 1)] = getWords(%line, 2, getWordCount(%line) - 1);
			}
			
			if(%currentState $= "ID")
			{
				%this.data[%this.curkey,getWord(%line, 1)] = trim(getWords(%line, 2, getWordCount(%line) - 1));
				continue;
			}
		}
	}
	
	%file.close();
	%file.delete();
	if($SimpleDB::TimeDebug) warn("LoadSassy - TIME:"@(getSimTime()-$SimpleDB::Time::LoadSassy));
	return 1;
}

function SimpleDB::addField(%this, %name, %default)
{
	// Checks Sector -- Test Input
	if(%name $= "")
	{
		if($SimpleDB::Debug) warn(%this.getName()@"::AddField() - Name Argument Not Specified! Aborting...");
		return 0;
	}
	if(%default $= "")
	{
		if($SimpleDB::Debug) warn(%this.getName()@"::AddField() - Default Value Argument Not Specified! Defaulting to 0...");
		%default = "";
	}
	for(%i = 0; getWordCount(%this.fields) > %i; %i++)
	{
		if(getWord(%this.fields, %i) $= %name)
		{
			if($SimpleDB::Debug) warn(%this.getName()@"::AddField() - Field with name \""@%name@"\" already exists! Aborting...");
			return 0;
		}
	}

	// Main Sector -- Actually Add The Field	
	%this.fields = trim(%this.fields SPC %name);
	%this.fields[%name] = trim(%default);

	for(%i = 0; getWordCount(%this.keys) > %i; %i++)
	{
		%key = getWord(%this.keys, %i);
		if(%this.data[%key,%name] $= "")
			%this.data[%key,%name] = trim(%default);
	}
	return 1;
}
// Will be replaced with new "loop free" version soon.
function SimpleDB::remField(%this, %name)
{
	// Checks Sector -- Test Input
	if(%name $= "")
	{
		if($SimpleDB::Debug) warn(%this.getName()@"::RemField() - Name Argument Not Specified! Aborting...");
		return 0;
	}
	%fieldindex = %this.chkField(%name);
	if(%fieldindex < 0)
	{
		if($SimpleDB::Debug) warn(%this.getName()@"::RemField() - Field with name \""@%name@"\" does not exist! Aborting...");
		return 0;
	}

	// Main Sector -- Actually Remove The Field
	%this.fields = trim(strReplace(%this.fields, %name, ""));
	%this.fields[%name] = "";

	for(%i = 0; getWordCount(%this.keys) > %i; %i++)
	{
		%key = getWord(%this.keys, %i);
		if(%this.data[%key,%name] !$= "")
			%this.data[%key,%name] = "";
	}
	return 1;
}

function SimpleDB::ChkField(%this, %name)
{
	// Checks Sector -- Test Input
	if(%name $= "")
	{
		if($SimpleDB::Debug) warn("SimpleDB: Instance \""@%this.getName()@"\".ChkField() Name Argument Not Specified! Aborting...");
		return 0;
	}
	%data = striPos(%this.fields,%name);
	if(%data >= 0)
		return 1;
	return 0;
}

//*******************
// SUPPORT
//*******************
function isInteger(%string)
{
	%search = "- 0 1 2 3 4 5 6 7 8 9";
	for(%i=0;%i<getWordCount(%search);%i++)
	{
		%string = strReplace(%string,getWord(%search,%i),"");
	}
	if(%string $= "")
		return true;
	return false;
}