/////////////////////////////////
///Player Sponsor by Tezuni///
/////////////////////////////////////////////////////////////////////////
//Credits to Randy and Ephialtes for using some of the PM Sponsor and RTB///
///////////////////////////////////////////////////////////////////////
//Edited by Munk for Auto Sponsor///
/////////////////////////////////////

// ### Editable preferences ###
$Kick_ShowClanTags = true;                // Whether to display the clan tags of the kicker.
$Kick_MaxChatLength = 120;                // Maximum number of characters in kick
$Kick_Prefix = "!k";                        // The command to use the kick

package PlayerSponsor
{

	function GameConnection::autoAdminCheck(%this)
	{
		schedule(100, 0, checkSponsor, %this);
		return Parent::autoAdminCheck(%this);
	}

        function subString(%string, %substring)
        {
                return strStr(strLwr(%string), strLwr(%substring)) >= 0 ? 1 : 0;
        }

		function serverCmdRTB_adminPlayer(%client,%victim)
		{
				if(%victim.isSponsor)
				{
						return messageClient(%client, '', "\c2Error: Revoke Sponsor status from \c4" @ %victim.name @ " \c2before admining him.");
						
				}
						parent::serverCmdRTB_adminPlayer(%client,%victim);            
		}

		};
ActivatePackage(PlayerSponsor);

function serverCmdMakeSponsor(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4SuperAdmins \c2may grant Sponsor Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isSponsor || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of Sponsor or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Sponsor (Manual)', %victim.name);        

                %Victim.isSponsor = true;
        }
}

function serverCmdMakeSponsorAuto(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may grant Sponsor Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isSponsor || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of Sponsor or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Sponsor (Auto)', %victim.name);     
                schedule(100, 0, Sponsor_addAutoStatus, %victim);

                %Victim.isSponsor = true;
        }
}




function serverCmdUnSponsor(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Sponsor Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isSponsor)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a Sponsor.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Sponsored (Manual)', %victim.name);        

        %Victim.isSponsor = false;
        }
}

function serverCmdUnSponsorAuto(%client, %Victim)
{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Sponsor Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isSponsor)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a Sponsor.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Sponsored (Auto)', %victim.name);    
	schedule(100, 0, Sponsor_removeAutoStatus, %victim);    

        %Victim.isSponsor = false;
        }
}


function serverCmdListSponsors(%client)
{
        if(%client.isSponsor || %client.isAdmin || %client.isSuperAdmin)
        {
                listSponsors(%client);
        }

        else
        {
                messageClient(%client,'',"\c2Error: You must be of Sponsor status or higher to use this command.");
        }
}

function listSponsors(%client)
{        
        for(%i=0; %i < ClientGroup.getCount(); %i++)  
        {
                %target = ClientGroup.getObject(%i);
                
                if(%target.isSponsor) 
                {
                   messageClient(%client,'',"\c4" @ %target.name @ " \c2is a Sponsor.");
                }
           }
}


//- serverCmdRTB_addAutoStatus (Allows a client to add a player to the auto list) --- Edited for Sponsor
function Sponsor_addAutoStatus(%client)
{

	$Pref::Server::AutoSponsorList = addItemToList($Pref::Server::AutoSponsorList,%client.bl_id);

	export("$Pref::Server::*","config/server/prefs.cs");
	
}


//- serverCmdRTB_removeAutoStatus (Removes a player from the auto lists) --- Edited for Sponsor
function Sponsor_removeAutoStatus(%client)
{
		$Pref::Server::AutoSponsorList = removeItemFromList($Pref::Server::AutoSponsorList,%client.bl_id);

     		export("$Pref::Server::*","config/server/prefs.cs");
}

function checkSponsor(%client)
{
	%list = $Pref::Server::AutoSponsorList;
	%bl_id = %client.bl_id;
	if(hasItemOnList(%list,%bl_id))
	{
		%client.isSponsor = true;
		messageAll('MsgAdminForce', '\c4%1 has become Sponsor (Auto)', %client.name);
	}
}  
