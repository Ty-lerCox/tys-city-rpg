//Thanks to Kalphiter for the writeToFileLine and %getTime code.
//Thanks to MR.DOOM for help with the ReportSchedule coding.

package Report
{
    	function writeToFileLine(%filepath, %text)
    	{
		%file = new FileObject();
		%file.openForAppend(%filepath);
		%file.writeLine(%text);
		%file.close();
		%file.delete();
    	}

	function serverCmdReport(%client, %target, %w1, %w2, %w3, %w4, %w5, %w6, %w7, %w8, %w9, %w10, %w11, %w12, %w13, %w14, %w15, %w16, %w17, %w18, %w19, %w20, %w21, %w22, %w23, %w24, %w25, %w26)
	{
		%victim = FindClientByName(%target);

		if(%client.bl_id == %victim.bl_id)
		{
			messageClient(%client,"","<color:FFFFFF>You cannot report yourself! That doesn't make sense!");
			return;
		}

		if(%victim.bl_id == getNumKeyID())
		{
			messageClient(%client,"","<color:FFFFFF>You cannot report the host, that won't do any good!");
			return;
		}

		if(!isEventPending(%client.ReportSchedule))
		{
			if(isObject(%victim) && strLen(%w1) > 0)
			{
				%reason = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5 SPC %w6 SPC %w7 SPC %w8 SPC %w9 SPC %w10 SPC %w11 SPC %w12 SPC %w13 SPC %w14 SPC %w15 SPC %w16 SPC %w17 SPC %w18 SPC %w19 SPC %w20 SPC %w21 SPC %w22 SPC %w23 SPC %w24 SPC %w25 SPC %w26);
				%getTime = firstWord(getDateTime()) SPC restWords(getDateTime());
				%client.ReportSchedule = schedule(60000,0,"");

				if(%victim.isSuperAdmin)
				{
					writeToFileLine("config/server/Logs/Reports.txt","["@ %getTime @"]: "@ %client.name @" ("@ %client.bl_id @") reported [SUPER ADMIN] "@ %victim.name @" ("@ %victim.bl_id @") for *"@ %reason @"*");
					messageClient(%client,"","<color:FFFFFF>Thank-you, your report has been successfully sent!");

					for(%i=0;%i<ClientGroup.GetCount();%i++)
					{
						%check = clientgroup.getobject(%i);

						if(%check.bl_id == getNumKeyID())
						{
							messageClient(%check,"","<color:FF0000>REPORT: <color:FFFF00>"@ %client.name @" ("@ %client.bl_id @") <color:FF0000>reported <color:0000FF>[SUPER ADMIN] <color:FFFF00>"@ %victim.name @" ("@ %victim.bl_id @") <color:FF0000>for *"@ %reason @"*");
						}
					}
				}
				else if(%victim.isAdmin)
				{
					writeToFileLine("config/server/Logs/Reports.txt","["@ %getTime @"]: "@ %client.name @" ("@ %client.bl_id @") reported [ADMIN] "@ %victim.name @" ("@ %victim.bl_id @") for *"@ %reason @"*");
					messageClient(%client,"","<color:FFFFFF>Thank-you, your report has been successfully sent!");

					for(%i=0;%i<ClientGroup.GetCount();%i++)
					{
						%check = clientgroup.getobject(%i);

						if(%check.isSuperAdmin)
						{
							messageClient(%check,"","<color:FF0000>REPORT: <color:FFFF00>"@ %client.name @" ("@ %client.bl_id @") <color:FF0000>reported <color:0000FF>[ADMIN] <color:FFFF00>"@ %victim.name @" ("@ %victim.bl_id @") <color:FF0000>for *"@ %reason @"*");
						}
					}
				}
				else
				{
					writeToFileLine("config/server/Logs/Reports.txt","["@ %getTime @"]: "@ %client.name @" ("@ %client.bl_id @") reported "@ %victim.name @" ("@ %victim.bl_id @") for *"@ %reason @"*");
					messageClient(%client,"","<color:FFFFFF>Thank-you, your report has been successfully sent!");

					for(%i=0;%i<ClientGroup.GetCount();%i++)
					{
						%check = clientgroup.getobject(%i);

						if(%check.isAdmin || %check.isSuperAdmin)
						{
							messageClient(%check,"","<color:FF0000>REPORT: <color:FFFF00>"@ %client.name @" ("@ %client.bl_id @") <color:FF0000>reported <color:FFFF00>"@ %victim.name @" ("@ %victim.bl_id @") <color:FF0000>for *"@ %reason @"*");
						}
					}
				}
			}
			else if(!isObject(%victim))
			{
				messageClient(%client,"","<color:FFFFFF>Target not found!");
				return;
			}
			else if(strLen(%w1) <= 0)
			{
				messageClient(%client,"","<color:FFFFFF>Don't forget to put a reason for your report!");
				return;
			}
		}
		else if(isEventPending(%client.ReportSchedule))
		{
			messageClient(%client,"","<color:FFFFFF>You must wait 60 seconds before reporting again!");
			return;
		}
	}
};
activatePackage(Report);