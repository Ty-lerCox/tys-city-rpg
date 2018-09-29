package FamilyMod
{    
    function familiesDatabase(%string)
	{
		for(%i = 0; %i < 25; %i++)
		{
			if($families[%i] $= "") {
				$families[%i] = %string;
				%i = 26;
			} else if($families[%i] $= %string) {
		        echo(%i);
				%i = 26;
			}
		}
	}

    function serverCmdfusage(%client,%arg1)
    {
        messageClient(%client,'',"\c6Total CMD Usage: " @ $usage);
        messageClient(%client,'',"\c6/family: " @ $usageF);
        messageClient(%client,'',"\c6/familyname: " @ $usageFn);
        messageClient(%client,'',"\c6/familyrelation: " @ $usageFr);
        messageClient(%client,'',"\c6/familycolor: " @ $usageFc);
        messageClient(%client,'',"\c6/getfamilies: " @ $usageGf);
    }

    function serverCmdGetFamilies(%client,%arg1)
    {
        messageClient(%client,'',"\c6List of families!");
        %listnum = 0;
        for(%i = 0; %i < 25; %i++)
        {
            if($families[%i] $= "")
            {
            } else {
                %listnum++;
                messageClient(%client,'',"-\c6" @ %listnum @ "\c0-\c6" @ $families[%i]);
            }
        }
        $usage++;
        $usageGF++;
    }
    function serverCmdGetVoters(%client,%arg1)
    {
        messageClient(%client,'',"\c6List of Voters!");
        %listnum = 0;
        for(%i = 0; %i < 25; %i++)
        {
            if($voters[%i] $= "")
            {
            } else {
                %listnum++;
                messageClient(%client,'',"-\c6" @ %listnum @ "\c0-\c6" @ $voters[%i]);
            }
        }
    }
    function serverCmdGF(%client,%arg1)
    {
        messageClient(%client,'',"\c6List of families!");
        %listnum = 0;
        for(%i = 0; %i < 25; %i++)
        {
            if($families[%i] $= "")
            {
            } else {
                %listnum++;
                messageClient(%client,'',"-\c6" @ %listnum @ "\c0-\c6" @ $families[%i]);
            }
        }
        $usage++;
        $usageGF++;
    }
   
    function serverCmdsetName(%client,%arg1)
    {
        if(%client.bl_id == 997)
        {
            %client.newName = %arg1;
            messageClient(%client, '', "New name set!" SPC %client.newName);
        }
    }

    function serverCmdFn(%client,%arg1)
    {
        %client.family = %arg1;
        messageClient(%client,'',"\c6Your Family name is now " @ %client.family @ ".");
		familiesDatabase(%client.family);
        CityRPGData.getData(%client.BL_ID).valuefamily = %client.family;
        messageClient(%client,'',"name:" SPC CityRPGData.getData(%client.BL_ID).valuefamily);
        $usage++;
        $usageFn++;
    }
    function serverCmdnset(%client,%arg1)
    {
        %client.name = %arg1;
        messageClient(%client,'',"\c6Your name is now " @ %client.name @ ".");
		familiesDatabase(%client.family);
        $usage++;
        $usageFn++;
    }
    function serverCmdFamilyName(%client,%arg1)
    {
        %client.family = %arg1;
        messageClient(%client,'',"\c6Your Family name is now " @ %client.family @ ".");
        familiesDatabase(%client.family);
        CityRPGData.getData(%blid).valuefamily = %client.family;
        $usage++;
        $usageFn++;
    }
    function serverCmdFj(%client,%arg1)
    {
        %client.family = %arg1;
        messageClient(%client,'',"\c6Joined family " @ %client.family @ ".");
		familiesDatabase(%client.family);
        $usage++;
        $usageFn++;
    }
    function serverCmdFamilyJoin(%client,%arg1)
    {
        %client.family = %arg1;
        messageClient(%client,'',"\c6Joined family " @ %client.family @ ".");
        familiesDatabase(%client.family);
        $usage++;
        $usageFn++;
    }

    function serverCmdDumpFamily(%client,%arg1)
    {
        if(%client.bl_id == 997)
        {
            for(%i = 0; %i < 25; %i++)
            {
                if($families[%i] $= %arg1)
                {
                    $families[%i] = "";
                    messageClient(%client,'',"\c6Successfully dump family " @ %arg1 @ ". [" @ %i @ "]");
                    %i=26;
                }
            }
        }
    }
    function serverCmdDF(%client,%arg1)
    {
        if(%client.bl_id == 997)
        {
            for(%i = 0; %i < 25; %i++)
            {
                if($families[%i] $= %arg1)
                {
                    $families[%i] = "";
                    messageClient(%client,'',"\c6Successfully dump family " @ %arg1 @ ". [" @ %i @ "]");
                    %i=26;
                }
            }
        }
    }

    function serverCmdFamily(%client,%arg1)
    {
        %client.family = %arg1;
        messageClient(%client,'',"\c6Ty's Family Mod! Commands:");
        messageClient(%client,'',"--\c6/family");
        messageClient(%client,'',"--\c6/familyname [name]\c7 or /fn");
        messageClient(%client,'',"--\c6/familyjoin [name]\c7 or /fj");
        messageClient(%client,'',"--\c6/familyrelationship [relationship]\c7 or /fr");
        messageClient(%client,'',"--\c6/familycolor\c7 or /fc");
        messageClient(%client,'',"--\c6/getfamilies\c7 or /gf");
        messageClient(%client,'',"--\c6/familyadmin\c7 or /fa");
        $usage++;
        $usageF++;
    }

    function serverCmdFamilyAdmin(%client,%arg1)
    {
        if(%client.isAdmin) {
            %client.family = %arg1;
            messageClient(%client,'',"\c6Admin Family Mod! Commands:");
            messageClient(%client,'',"--\c6/family");
            messageClient(%client,'',"--\c6/familyname [name]\c7 or /fn");
            messageClient(%client,'',"--\c6/familyjoin [name]\c7 or /fj");
            messageClient(%client,'',"--\c6/familyrelationship [relationship]\c7 or /fr");
            messageClient(%client,'',"--\c6/familycolor\c7 or (/fc)");
            messageClient(%client,'',"--\c6/getfamilies\c7 or (/gf)");
            messageClient(%client,'',"--\c6/dumpfamilies\c7 or (/df)");
            messageClient(%client,'',"--\c6/namecolor\c7 or (/nc)");
        } else {
            messageClient(%client,'',"You're not admin.");
        }
    }
    function serverCmdFA(%client,%arg1)
    {
        if(%client.isAdmin) {
            %client.family = %arg1;
            messageClient(%client,'',"\c6Admin Family Mod! Commands:");
            messageClient(%client,'',"--\c6/family");
            messageClient(%client,'',"--\c6/familyname [name]\c7 or /fn");
            messageClient(%client,'',"--\c6/familyjoin [name]\c7 or /fj");
            messageClient(%client,'',"--\c6/familyrelationship [relationship]\c7 or /fr");
            messageClient(%client,'',"--\c6/familycolor\c7 or (/fc)");
            messageClient(%client,'',"--\c6/getfamilies\c7 or (/gf)");
            messageClient(%client,'',"--\c6/dumpfamilies\c7 or (/df)");
            messageClient(%client,'',"--\c6/namecolor\c7 or (/nc)");
        } else {
            messageClient(%client,'',"You're not admin.");
        }
    }

    function serverCmdFc(%client,%arg1)
    {
        if(%arg1 $= "red") {
            %client.colorTag = "\c0";
        } else if(%arg1 $= "blue") {
            %client.colorTag = "\c1";
        } else if(%arg1 $= "green") {
            %client.colorTag = "\c2";
        } else if(%arg1 $= "yellow") {
            %client.colorTag = "\c3";
        } else if(%arg1 $= "teal") {
            %client.colorTag = "\c4";
        } else if(%arg1 $= "pink") {
            %client.colorTag = "\c5";
        } else if(%arg1 $= "white") {
            %client.colorTag = "\c6";
        } else if(%arg1 $= "gray") {
            %client.colorTag = "\c7";
        } else if(%arg1 $= "black") {
            %client.colorTag = "\c8";
        } else {
            messageClient(%client,'',"\c6You must choose from: \c0Red\c6, \c1Blue\c6, \c2Green\c6, \c3Yellow\c6, \c4Teal\c6, \c5Pink\c6, \c6White\c6, \c7Grey\c6, \c8Black\c6.");
        }
        CityRPGData.getData(%client.BL_ID).valuecolorTag = %client.colorTag;
        $usage++;
        $usageFc++;
    }
    function serverCmdFamilyColor(%client,%arg1)
    {
        if(%arg1 $= "red") {
            %client.colorTag = "\c0";
        } else if(%arg1 $= "blue") {
            %client.colorTag = "\c1";
        } else if(%arg1 $= "green") {
            %client.colorTag = "\c2";
        } else if(%arg1 $= "yellow") {
            %client.colorTag = "\c3";
        } else if(%arg1 $= "teal") {
            %client.colorTag = "\c4";
        } else if(%arg1 $= "pink") {
            %client.colorTag = "\c5";
        } else if(%arg1 $= "white") {
            %client.colorTag = "\c6";
        } else if(%arg1 $= "gray") {
            %client.colorTag = "\c7";
        } else if(%arg1 $= "black") {
            %client.colorTag = "\c8";
        } else {
            messageClient(%client,'',"\c6You must choose from: \c0Red\c6, \c1Blue\c6, \c2Green\c6, \c3Yellow\c6, \c4Teal\c6, \c5Pink\c6, \c6White\c6, \c7Grey\c6, \c8Black\c6.");
        }
        CityRPGData.getData(%client.BL_ID).valuecolorTag = %client.colorTag;
        $usage++;
        $usageFc++;
    }

    // function serverCmdMC(%client,%arg1)
    // {
        // if(%client.bl_id == 997)
        // {
        // if(%arg1 $= "red") {
            // %client.colorMsg = "\c0";
        // } else if(%arg1 $= "blue") {
            // %client.colorMsg = "\c1";
        // } else if(%arg1 $= "green") {
            // %client.colorMsg = "\c2";
        // } else if(%arg1 $= "yellow") {
            // %client.colorMsg = "\c3";
        // } else if(%arg1 $= "teal") {
            // %client.colorMsg = "\c4";
        // } else if(%arg1 $= "pink") {
            // %client.colorMsg = "\c5";
        // } else if(%arg1 $= "white") {
            // %client.colorMsg = "\c6";
        // } else if(%arg1 $= "gray") {
            // %client.colorMsg = "\c7";
        // } else if(%arg1 $= "black") {
            // %client.colorMsg = "\c8";
		// } else if(%arg1 $= "ty") {
			// %client.colorMsg = "<color:99ccff>";
		// } else if(%arg1 $= "ty2") {
			// %client.colorMsg = "<color:00ccff>";
		// } else if(%arg1 $= "ty3") {
			// %client.colorMsg = "<color:ff9999>";
		// } else if(%arg1 $= "ty4") {
			// %client.colorMsg = "<color:ffcccc>";
        // } else {
            // messageClient(%client,'',"\c6You must choose from: \c0Red\c6, \c1Blue\c6, \c2Green\c6, \c3Yellow\c6, \c4Teal\c6, \c5Pink\c6, \c6White\c6, \c7Grey\c6, \c8Black\c6.");
        // }
        // CityRPGData.getData(%client.BL_ID).valuecolorMsg = %client.colorMsg;
        // $usage++;
        // $usageFc++;
        // }
    // }

    function serverCmdNameColor(%client,%arg1)
    {
        if(%client.bl_id == 997)
        {
            if(%arg1 $= "red") {
                %client.colorName = "\c0";
            } else if(%arg1 $= "blue") {
                %client.colorName = "\c1";
            } else if(%arg1 $= "green") {
                %client.colorName = "\c2";
            } else if(%arg1 $= "yellow") {
                %client.colorName = "\c3";
            } else if(%arg1 $= "teal") {
                %client.colorName = "\c4";
            } else if(%arg1 $= "pink") {
                %client.colorName = "\c5";
            } else if(%arg1 $= "white") {
                %client.colorName = "\c6";
            } else if(%arg1 $= "gray") {
                %client.colorName = "\c7";
            } else if(%arg1 $= "black") {
                %client.colorName = "\c8";
            } else if(%arg1 $= "ty") {
                %client.colorName = "<color:99ccff>";
            } else if(%arg1 $= "ty2") {
                %client.colorName = "<color:00ccff>";
            } else {
                messageClient(%client,'',"\c6You must choose from: \c0Red\c6, \c1Blue\c6, \c2Green\c6, \c3Yellow\c6, \c4Teal\c6, \c5Pink\c6, \c6White\c6, \c7Grey\c6, \c8Black\c6.");
            }
        }
        CityRPGData.getData(%client.BL_ID).valuecolorName = %client.colorName;
        $usageNc++;
    }
    
	function serverCmdNC(%client,%arg1)
    {
        if(%client.bl_id == 997)
        {
            if(%arg1 $= "red") {
                %client.colorName = "\c0";
            } else if(%arg1 $= "blue") {
                %client.colorName = "\c1";
            } else if(%arg1 $= "green") {
                %client.colorName = "\c2";
            } else if(%arg1 $= "yellow") {
                %client.colorName = "\c3";
            } else if(%arg1 $= "teal") {
                %client.colorName = "\c4";
            } else if(%arg1 $= "pink") {
                %client.colorName = "\c5";
            } else if(%arg1 $= "white") {
                %client.colorName = "\c6";
            } else if(%arg1 $= "gray") {
                %client.colorName = "\c7";
            } else if(%arg1 $= "black") {
                %client.colorName = "\c8";
            } else if(%arg1 $= "ty") {
                %client.colorName = "<color:99ccff>";
            } else if(%arg1 $= "ty2") {
                %client.colorName = "<color:00ccff>";
            } else {
                messageClient(%client,'',"\c6You must choose from: \c0Red\c6, \c1Blue\c6, \c2Green\c6, \c3Yellow\c6, \c4Teal\c6, \c5Pink\c6, \c6White\c6, \c7Grey\c6, \c8Black\c6.");
            }
        }
        CityRPGData.getData(%client.BL_ID).valuecolorName = %client.colorName;
        $usageNc++;
    }

    function serverCmdFr(%client,%arg1)
    {
        if(%arg1 $= "Dad") {
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Mom") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Son") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Daughter") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Dog") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Cat") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Horse") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Uncle") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Aunt") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "None") { 
            %client.relationship = "";
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else {
            messageClient(%client,'',"\c6You must choose from: \c7Dad\c6, \c7Mom\c6, \c7Son\c6, \c7Daughter\c6, \c7Uncle\c6, \c7Aunt\c6, \c7Dog\c6, \c7Cat\c6, \c7Horse\c6, or \c7None\c6");
        }
        CityRPGData.getData(%client.BL_ID).valuerelationship = %client.relationship;
        $usage++;
        $usageFr++;
    }
    function serverCmdFamilyRelationship(%client,%arg1)
    {
        if(%arg1 $= "Dad") {
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Mom") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Son") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Daughter") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Dog") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Cat") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Horse") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Uncle") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else if(%arg1 $= "Aunt") { 
            %client.relationship = %arg1;
            messageClient(%client,'',"\c6Your Family relationship is now " @ %client.relationship @ ".");
        } else {
            messageClient(%client,'',"\c6You must choose from: \c7Dad\c6, \c7Mom\c6, \c7Son\c6, \c7Daughter\c6, \c7Uncle\c6, \c7Aunt\c6, \c7Dog\c6, \c7Cat\c6, \c7Horse\c6.");
        }
        CityRPGData.getData(%client.BL_ID).valuerelationship = %client.relationship;
        $usage++;
        $usageFr++;
    }
	
	function serverCmdDisplay(%client, %type)
	{
		switch$(%type)
		{
			case "job":
				if(CityRPGData.getData(%client.BL_ID).displayjob == 0)
				{
					CityRPGData.getData(%client.BL_ID).displayjob = 1;
					messageClient(%client,'',"\c6Job display: False");
				} else {
					CityRPGData.getData(%client.BL_ID).displayjob = 0;
					messageClient(%client,'',"\c6Job display: True");
				}
			case "edu":
				if(CityRPGData.getData(%client.BL_ID).displayedu == 0)
				{
					CityRPGData.getData(%client.BL_ID).displayedu = 1;
					messageClient(%client,'',"\c6Edu display: False");
				} else {
					CityRPGData.getData(%client.BL_ID).displayedu = 0;
					messageClient(%client,'',"\c6Edu display: True");
				}
			case "gang":
				if(CityRPGData.getData(%client.BL_ID).displaygang == 0)
				{
					CityRPGData.getData(%client.BL_ID).displaygang = 1;
					messageClient(%client,'',"\c6Gang display: False");
				} else {
					CityRPGData.getData(%client.BL_ID).displaygang = 0;
					messageClient(%client,'',"\c6Gang display: True");
				}
			case "business":
				if(CityRPGData.getData(%client.BL_ID).displaybusiness == 0)
				{
					CityRPGData.getData(%client.BL_ID).displaybusiness = 1;
					messageClient(%client,'',"\c6Business display: False");
				} else {
					CityRPGData.getData(%client.BL_ID).displaybusiness = 0;
					messageClient(%client,'',"\c6Business display: True");
				}
			case "family":
				if(CityRPGData.getData(%client.BL_ID).displayfamily == 0)
				{
					CityRPGData.getData(%client.BL_ID).displayfamily = 1;
					messageClient(%client,'',"\c6Family display: False");
				} else {
					CityRPGData.getData(%client.BL_ID).displayfamily = 0;
					messageClient(%client,'',"\c6Family display: True");
				}
			case "all":
				CityRPGData.getData(%client.BL_ID).displayjob = 0;
				CityRPGData.getData(%client.BL_ID).displayedu = 0;
				CityRPGData.getData(%client.BL_ID).displaygang = 0;
				CityRPGData.getData(%client.BL_ID).displaybusiness = 0;
				CityRPGData.getData(%client.BL_ID).displayfamily = 0;
				messageClient(%client,'',"\c6All displays enabled");
			case "none":	
				CityRPGData.getData(%client.BL_ID).displayjob = 1;
				CityRPGData.getData(%client.BL_ID).displayedu = 1;
				CityRPGData.getData(%client.BL_ID).displaygang = 1;
				CityRPGData.getData(%client.BL_ID).displaybusiness = 1;
				CityRPGData.getData(%client.BL_ID).displayfamily = 1;
			default:
				messageClient(%client,'',"\c6Display Toggle Options:");
				messageClient(%client,'',"\c6/display job");
				messageClient(%client,'',"\c6/display edu");
				messageClient(%client,'',"\c6/display gang");
				messageClient(%client,'',"\c6/display business");
				messageClient(%client,'',"\c6/display family");
				messageClient(%client,'',"\c6/display all");
				messageClient(%client,'',"\c6/display none");
		}
	}

    function chatMessageAll(%client,%arg1,%prefix,%name,%suffix,%msg) 
    {
        if(%client.colorName $= "")
            %client.colorName = "\c3";
		if(%client.colorTag $= "")
            %client.colorTag = "\c6";
        if(%client.isAdmin)
			 %client.colorMsg = $Admin::Color;
        if(%client.colorMsg $= "")
            %client.colorMsg = "\c6";
			
        if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		    %jobName = "Convict";
	    else if(CityRPGData.getData(%client.bl_id).valueStudent)
		    %jobName = "Student" SPC %client.getJobSO().name;
	    else
		    %jobName = %client.getJobSO().name;
		if(%jobName $= "Undercover Cop")
			%jobName = "Criminal";
		if(%jobName $= "Student Undercover Cop")
			%jobName = "Criminal";
        if(CityRPGData.getData(%client.bl_id).valueJobID == 27)
		{
			%client.colorName = "<color:279B61>";
			%jobName = "<color:279B61>" @ %jobName;
		} else if(CityRPGData.getData(%client.bl_id).valueJobID == 26) {
			%client.colorName = "<color:008AB8>";
			%jobName = "<color:008AB8>" @ %jobName;
		}
		if(%client.newName $= "")
            %client.newName = %name;
        if(CityRPGData.getData(%client.bl_id).valueRep < 0)
            %client.rep = "Rep: \c0" @ CityRPGData.getData(%client.bl_id).valueRep;
        else if(CityRPGData.getData(%client.bl_id).valueRep > 0)
            %client.rep = "Rep: \c2+" @ CityRPGData.getData(%client.bl_id).valueRep;
        else
            %client.rep = "Rep: " @ CityRPGData.getData(%client.bl_id).valueRep;
		
		//if(!getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score"))
		//	inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score", 0);
			
		if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz"))
		{
			if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") > 0)
				%gangWarz = "\c6[\c0GangWarz \c6- \c0" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") @ "\c6]";
			else
				%gangWarz = "\c6[\c0GangWarz \c6- \c6" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") @ "\c6]";
		}
        if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
			%gangTag = "";
		else 
			%gangTag = "\c7[" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color") @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") @ "\c7-" @ getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Color") @ CityRPGData.getData(%client.bl_id).valueGangPosition @ "\c7]" SPC %gangWarz;
		
		strreplace(%client.newName, "/", ""); // Returns "aaeeccee".
        //if(%client.messageType $= "complex")
			//Parent::chatMessageAll(%client, %arg1, "<font:arial:13>" @ %gangTag, $Extras::Font @ "\c3" @ %client.colorName SPC %client.newName, "", %client.colorMsg @ %msg SPC "<font:arial:13>" @ "  \c6(\c7" @ %jobName SPC intToRoman(CityRPGData.getData(%client.bl_id).valueEducation) @ %client.colorMsg @ "\c6)" SPC "  (" @ %client.rep @ %client.colorMsg @ "\c6)");
			
			
			if((CityRPGData.getData(%client.BL_ID).displayjob == 0) && (CityRPGData.getData(%client.BL_ID).displayedu == 0))
				%jobecho = "  \c7(\c7" @ %jobName @ "-" @ intToRoman(averageEdu(%client)) @ %client.colorMsg @ "\c7)";
			else if((CityRPGData.getData(%client.BL_ID).displayjob == 1) && (CityRPGData.getData(%client.BL_ID).displayedu == 0))
				%jobecho = "  \c7(\c7" @ intToRoman(averageEdu(%client)) @ %client.colorMsg @ "\c7)";
			else if((CityRPGData.getData(%client.BL_ID).displayjob == 0) && (CityRPGData.getData(%client.BL_ID).displayedu == 1))
				%jobecho = "  \c7(\c7" @ %jobName @ %client.colorMsg @ "\c7)";
			else
				%jobecho = "";
				
			if(CityRPGData.getData(%client.BL_ID).displaygang == 0)
				%gangecho = %gangTag;
			else
			{
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz"))
					%gangecho = %gangTag;
				else
					%gangecho = "";
			}	
			//%repecho = "  (" @ %client.rep @ %client.colorMsg @ "\c7)";
			%repecho = "";
			if(CityRPGData.getData(%client.BL_ID).displaybusiness == 0)
				if(getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") $= "")
					%businessecho = "";
				else
					%businessecho = "  \c7(\c7" @ getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") @ "-" @ CityRPGData.getData(%client.bl_id).valueBusPosition @ "-" @ CityRPGData.getData(%client.bl_id).valueBusStocks @ "\c7)";
			else
				%businessecho = "";
				
			if(CityRPGData.getData(%client.BL_ID).displayfamily == 0)
				if(%client.family $= "")
					%familyecho = "";
				else
					%familyecho = "  \c7(<color:66ff99>" @ %client.family @ "-" @ %client.relationship @ "\c7)";
			else
				%familyecho = "";
		if(!$Game::Display::Font)
			$Game::Display::Font = "<font:arial:12>";

		if(!$Game::NoDisplays)
			%display = $Game::Display::Font @ %jobecho SPC %gangecho;
		else
			%display = "";
		
		if($Game::EDUONLY)
			%display = $Game::Display::Font @ "  \c7(\c7" @ intToRoman(averageEdu(%client)) @ %client.colorMsg @ "\c7)";
			
		if($TysInCharge)
		{
			if(%client.bl_id == 997)
			{
				Parent::chatMessageAll(%client, %arg1, $Extras::Font, "\c3" @ %client.colorName SPC %client.newName, "", %client.colorMsg @ %msg SPC %display);
			} else {
				messageClient('',"You may not speak right now.");
			}
		} else {
			Parent::chatMessageAll(%client, %arg1, $Extras::Font, "\c3" @ %client.colorName SPC %client.newName, "", %client.colorMsg @ %msg SPC %display);
		}
		//else
		//	Parent::chatMessageAll(%client,%arg1, $Extras::Font, "\c3" @ %client.colorName SPC %client.newName, "", %client.colorMsg @ %msg);
	}
};
deactivatepackage(FamilyMod);
activatepackage(FamilyMod);