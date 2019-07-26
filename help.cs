//--------------------
// Purpose:	Automated documentation to assist new players.
//--------------------

function Hellen::onAdd(%this)
{
	if(%this.getName() $= "")
	{
		//echo("Hellen::onAdd(): This database has no name! Deleting database..");
		
		%this.schedule(0, "delete");
		
		return false;
	}
	
	%this.loadData();
	
	return true;
}

function Hellen::onRemove(%this)
{
	for(%a = 1; %a <= %this.sectionCount; %a++)
	{
		%this.section[%a].delete();
	}
	
	return true;
}

function Hellen::loadData(%this)
{
	if(!isFile("./help.hlp"))
	{
		//echo(%this.getName() @ "::onAdd(): Help file not found!");
		
		return false;
	}
	
	%file = new fileObject();
	%file.openForRead(findFirstFile("./help.hlp"));
	
	%this.section[(%this.sectionCount = 1)] = new scriptObject();
	%this.sectionName[%this.sectionCount] = "main";
	
	%section = %this.section[%this.sectionCount];
	
	%section.lineCount = 0;
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		
		if(getSubStr(%line, 0, 2) $= "//")
		{
			continue;
		}
		
		if(getSubStr(%line, 0, 1) $= "[")
		{
			for(%a = 1; %a <= %this.sectionCount; %a++)
			{
				if(strLwr(%this.sectionName[%a]) $= strLwr(getSubStr(%line, 1, strLen(%line) - 2)))
				{
					%continue = true;
					
					%section = %this.section[%a];
					
					break;
				}
			}
			
			if(%continue)
			{
				%continue = false;
				
				continue;
			}
			
			%this.section[%this.sectionCount++] = new scriptObject();
			%this.sectionName[%this.sectionCount] = getSubStr(%line, 1, strLen(%line) - 2);
			
			%section = %this.section[%this.sectionCount];
			
			%section.lineCount = 0;
			
			continue;
		}
		
		%section.lineCount++;
		%section.line[%section.lineCount] = %line;
	}
	
	%file.close();
	%file.delete();
	
	return true;
}

function Hellen::displayHelp(%this, %client, %section)
{
	for(%a = 1; %a <= %this.sectionCount; %a++)
	{
		if(strLwr(%this.sectionName[%a]) $= strLwr(%section))
		{
			%section = %this.section[%a];
			
			break;
		}
	}
	
	if(!isObject(%section) || !isObject(%client))
	{
		return false;
	}
	
	for(%b = 1; %b <= %section.lineCount; %b++)
	{
		eval("messageClient(" @ %client @ ", '', \"" @ %section.line[%b] @ "\");");
	}
}