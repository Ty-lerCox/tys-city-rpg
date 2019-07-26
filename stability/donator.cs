/////////////////////////////////
///Player Donator by Tezuni///
/////////////////////////////////////////////////////////////////////////
//Credits to Randy and Ephialtes for using some of the PM Donator and RTB///
///////////////////////////////////////////////////////////////////////
//Edited by Munk for Auto Donator///
/////////////////////////////////////

// ### Editable preferences ###
$Kick_ShowClanTags = true;                // Whether to display the clan tags of the kicker.
$Kick_MaxChatLength = 120;                // Maximum number of characters in kick
$Kick_Prefix = "!k";                        // The command to use the kick

package PlayerDonator
{

	function GameConnection::autoAdminCheck(%this)
	{
		schedule(100, 0, checkDonator, %this);
		return Parent::autoAdminCheck(%this);
	}

        function subString(%string, %substring)
        {
                return strStr(strLwr(%string), strLwr(%substring)) >= 0 ? 1 : 0;
        }

		function serverCmdRTB_adminPlayer(%client,%victim)
		{
				if(%victim.isDonator)
				{
						return messageClient(%client, '', "\c2Error: Revoke Donator status from \c4" @ %victim.name @ " \c2before admining him.");
						
				}
						parent::serverCmdRTB_adminPlayer(%client,%victim);            
		}

		};
ActivatePackage(PlayerDonator);

function serverCmdMakeDonator(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                return messageClient(%client, '', "\c2Only \c4SuperAdmins \c2may grant Donator Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isDonator || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of Donator or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Donator (Manual)', %victim.name);        

                %Victim.isDonator = true;
        }
}

function serverCmdMakeDonatorAuto(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may grant Donator Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isDonator || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of Donator or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Donator (Auto)', %victim.name);     
                schedule(100, 0, Donator_addAutoStatus, %victim);

                %Victim.isDonator = true;
        }
}



function MakeDonatorAuto(%Victim)

{        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isDonator || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of Donator or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Donator (Auto)', %victim.name);     
				//echo(%victim.name @ " has become Donator (Auto)");
                schedule(100, 0, Donator_addAutoStatus, %victim);

                %Victim.isDonator = true;
        }
}




function serverCmdUnDonator(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Donator Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isDonator)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a Donator.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Donatored (Manual)', %victim.name);        

        %Victim.isDonator = false;
        }
}

function serverCmdUnDonatorAuto(%client, %Victim)
{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Donator Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isDonator)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a Donator.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Donatored (Auto)', %victim.name);    
	schedule(100, 0, Donator_removeAutoStatus, %victim);    

        %Victim.isDonator = false;
        }
}


function UnDonatorAuto(%client, %Victim)
{

        %Victim = findClientByName(%Victim);

        if(!%victim.isDonator)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a Donator.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Donatored (Auto)', %victim.name);    
		//echo(%victim.name @ " has been de-donatored");
	schedule(100, 0, Donator_removeAutoStatus, %victim);    

        %Victim.isDonator = false;
        }
}


function serverCmdListDonators(%client)
{
        if(%client.isDonator || %client.isAdmin || %client.isSuperAdmin)
        {
                listDonators(%client);
        }

        else
        {
                messageClient(%client,'',"\c2Error: You must be of Donator status or higher to use this command.");
        }
}

function listDonators(%client)
{        
        for(%i=0; %i < ClientGroup.getCount(); %i++)  
        {
                %target = ClientGroup.getObject(%i);
                
                if(%target.isDonator) 
                {
                   messageClient(%client,'',"\c4" @ %target.name @ " \c2is a Donator.");
                }
           }
}


//- serverCmdRTB_addAutoStatus (Allows a client to add a player to the auto list) --- Edited for Donator
function Donator_addAutoStatus(%client)
{

	$Pref::Server::AutoDonatorList = addItemToList($Pref::Server::AutoDonatorList,%client.bl_id);

	export("$Pref::Server::*","config/server/prefs.cs");
	
}


//- serverCmdRTB_removeAutoStatus (Removes a player from the auto lists) --- Edited for Donator
function Donator_removeAutoStatus(%client)
{
		$Pref::Server::AutoDonatorList = removeItemFromList($Pref::Server::AutoDonatorList,%client.bl_id);

     		export("$Pref::Server::*","config/server/prefs.cs");
}

function checkDonator(%client)
{
	%list = $Pref::Server::AutoDonatorList;
	%bl_id = %client.bl_id;
	if(hasItemOnList(%list,%bl_id))
	{
		%client.isDonator = true;
		messageAll('MsgAdminForce', '\c4%1 has become Donator (Auto)', %client.name);
	}
}  
