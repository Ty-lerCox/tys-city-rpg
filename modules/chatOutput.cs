package FamilyMod
{    
    function chatMessageAll(%client,%arg1,%prefix,%name,%suffix,%msg) 
    {
        if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		    %jobName = "Convict";
	    else if(CityRPGData.getData(%client.bl_id).valueStudent)
		    %jobName = "Student" SPC %client.getJobSO().name;
	    else
		    %jobName = %client.getJobSO().name;
        if(%client.newName $= "")
            %client.newName = %name;
        if(CityRPGData.getData(%client.bl_id).valueRep < 0)
            %client.rep = "Rep: \c0" @ CityRPGData.getData(%client.bl_id).valueRep;
        else if(CityRPGData.getData(%client.bl_id).valueRep > 0)
            %client.rep = "Rep: \c2+" @ CityRPGData.getData(%client.bl_id).valueRep;
        else
            %client.rep = "Rep: " @ CityRPGData.getData(%client.bl_id).valueRep;

        Parent::chatMessageAll(%client,%arg1, "" @ $Extras::Font @ "\c7[" @ %client.colorTag @ %client.family @ %client.colorTag @ "\c7-" @ %client.colorTag @ %client.relationship @ %client.colorTag @ "\c7]", %client.colorName @ %client.job @ " " @ %client.newName, "", %client.colorMsg @ %msg SPC "  \c6(<color:eeffff>" @ %jobName @ %client.colorMsg @ "\c6)" SPC "  (" @ %client.rep @ %client.colorMsg @ "\c6)");
    }
};
activatepackage(FamilyMod);