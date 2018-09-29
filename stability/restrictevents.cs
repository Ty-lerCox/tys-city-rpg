if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	RTB_registerPref("Restrict events","Restricted Events","$RestrictedEvents::Restrict","bool","Server_RestrictedEvents",0,0,0);
	%file = new FileObject();
	if(!isFile("config/server/restrictedEventsClasses.txt"))
	{
		%file.openForWrite("config/server/restrictedEventsClasses.txt");
		%file.writeLine("fxDtsBrick Brick");
		%file.writeLine("Player Player");
		%file.writeLine("GameConnection Client");
		%file.writeLine("MinigameSO Minigame");
		%file.writeLine("Vehicle Vehicle");
		%file.close();
		
	}
	%file.openForRead("config/server/restrictedEventsClasses.txt");
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		%class = firstWord(%line);
		%read = restWords(%line);
		RTB_registerPref("Restricted events (" @ %read @ ")","Restricted Events","$RestrictedEvents::Restrictions[" @ %class @ "]","string 255","Server_RestrictedEvents","",0,0);
	}
	%file.close();
	%file.delete();
}
else
{
	error("Server_RestrictedEvents: Required mod System_ReturnToBlockland not found!");
	return;
}
package Server_RestrictedEvents
{
	function serverCmdAddEvent(%client,%delay,%a,%input,%target,%b,%output,%para1,%para2,%para3,%para4)
	{
		if($RestrictedEvents::Restrict)
		{
			
			%targetClass = inputEvent_GetTargetClass("fxDtsBrick",%input,%target);
			if($RestrictedEvents::Restrictions[%targetClass] $= "")
			{
				Parent::serverCmdAddEvent(%client,%delay,%a,%input,%target,%b,%output,%para1,%para2,%para3,%para4);
				return;
			}
			%outputName = outputEvent_GetOutputName(%targetClass,%output);
			%restrictedList = strReplace($RestrictedEvents::Restrictions[%targetClass],",","\t");
			for(%i=0;%i<getFieldCount(%restrictedList);%i++)
			{
				%field = getField(%restrictedList,%i);
				%field = strReplace(%field,"=","\t");
				%restrictedTo = getField(%field,1);
				%restrictedEvent = getField(%field,0);
				if(%outputName $= %restrictedEvent)
				{
					cancel(%client.warnRestrictedEvent);
					if(%client.warnRestrictedEventMessage $= "")
					{
						%client.warnRestrictedEventMessage = "The following events are restricted to you:\n";
					}
					%client.warnRestrictedEventMessage = %client.warnRestrictedEventMessage @ %outputName @ "\n";
					%client.warnRestrictedEvent = %client.schedule(1000,"warnRestrictedEvent");
					switch$(%restrictedTo)
					{
						case "A":
							if(!%client.isAdmin)
							{
								return;
							}
						case "SA":
							if(!%client.isSuperAdmin)
							{
								return;
							}
						case "H":
							if(!%client.BL_ID == getNumKeyID())
							{
								return;
							}
					}
					cancel(%client.warnRestrictedEvent);
					%client.warnRestrictedEventMessage = "";
				}
			}
		}
		Parent::serverCmdAddEvent(%client,%delay,%a,%input,%target,%b,%output,%para1,%para2,%para3,%para4);
	}
	function GameConnection::warnRestrictedEvent(%client)
	{
		commandToClient(%client,'messageBoxOk',"Restricted Events",%client.warnRestrictedEventMessage);
		%client.warnRestrictedEventMessage = "";
		%client.warnRestrictedEvent = "";
	}
};
activatePackage(Server_RestrictedEvents);