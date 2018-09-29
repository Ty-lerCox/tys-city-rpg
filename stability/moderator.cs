/////////////////////////////////
///Player Moderator by Tezuni///
/////////////////////////////////////////////////////////////////////////
//Credits to Randy and Ephialtes for using some of the PM Mod and RTB///
///////////////////////////////////////////////////////////////////////
//Edited by Munk for Auto Moderator///
//Edited by Armageddon for !b     ///
////////////////////////////////////

// ### Editable preferences ###
$Kick_ShowClanTags = true;                // Whether to display the clan tags of the kicker.
$Kick_MaxChatLength = 120;                // Maximum number of characters in kick
$Kick_Prefix = "!k";
$Ban_Prefix = "!b";                        
// The command to use the kick

package PlayerModerator
{

        function GameConnection::autoAdminCheck(%this)
        {
                schedule(100, 0, checkmod, %this);
                return Parent::autoAdminCheck(%this);
        }

        function subString(%string, %substring)
        {
                return strStr(strLwr(%string), strLwr(%substring)) >= 0 ? 1 : 0;
        }

        function serverCmdMessageSent(%client, %text)
        {
                if(getSubStr(%text, 0, strlen($Kick_Prefix)) $= $Kick_Prefix)
                {
                        //If they are not a moderator 
                        if(!%client.isModerator)
                        {
                                return messageClient(%client, '', "\c2You are not a Moderator.");
                        }
                        
                                %victimnamepart = getsubstr(getWord(%text, 0), strlen($Kick_Prefix), strlen(getWord(%text, 0)) - strlen($Kick_Prefix));
                                %messagestart = strlen(%victimnamepart) + 1 + strlen($Kick_Prefix);
                                //%text = getSubStr(trim(getSubStr(%text, %messagestart, strlen(%text) - %messagestart)), 0, $Kick_MaxChatLength);
                                //%Kickername = $Kick_ShowClanTags ? "\c7" @ %victim.colorName @ "\c3" @ %client.name @ "\c7" @ %client.clanSuffix @ "\c3" : "\c3" @ %client.name;
                                %Kickername = %Client.name;
                                if(%victimnamepart $= "")
                                        return;
                                for(%i=0;%i<ClientGroup.getCount();%i++)
                                {
                                        %cl = ClientGroup.getObject(%i);
                                        if(subString(%cl.name, %victimnamepart))
                                        {
                                                if(%victim)
                                                {
                                                        messageClient(%client, "", '\c2More than one person was found with \'\c4%1\c2\' in their name. Please be more specfic.', %victimnamepart);
                                                        return;
                                                }
                                                else if(%client != %cl)
                                                        %victim = %cl;
                                        }
                                }
                                //No moderator fights...
                                if(%victim.isModerator || %victim.isAdmin || %victim.isSuperAdmin)
                
                                {
                        
                                        return messageClient(%client,'',"\c2You cannot kick\c4 " @ %victim.name @ " \c2because he is of moderator or higher status.");
                                }


                                if(%victim)
                                {
                                        messageAll('MsgAdminForce', '\c2[\c5Mod\c2]\c4%1 \c2kicked \c4%2\c2(ID: %3)', %Kickername, %victim.name, %Victim.bl_id);
                                        %victim.delete("You were kicked by a moderator");
                                }

                                else
                                {
                                        messageClient(%client, "", '\c2Nobody was found with \'\c4%1\c2\' in their name.', %victimnamepart);
                                        return;
                                }
                }
                if(getSubStr(%text, 0, strlen($Ban_Prefix)) $= $Ban_Prefix)
                {
                        //If they are not a moderator 
                        if(!%client.isModerator)
                        {
                                return messageClient(%client, '', "\c2You are not a Moderator.");
                        }
                        
                                %victimnamepart = getsubstr(getWord(%text, 0), strlen($Ban_Prefix), strlen(getWord(%text, 0)) - strlen($Ban_Prefix));
                                %messagestart = strlen(%victimnamepart) + 1 + strlen($Ban_Prefix);
                                %Bannername = %Client.name;
                                %reason = restWords(%text);
                                if(%victimnamepart $= "")
                                        return;
                                for(%i=0;%i<ClientGroup.getCount();%i++)
                                {
                                        %cl = ClientGroup.getObject(%i);
                                        if(subString(%cl.name, %victimnamepart))
                                        {
                                                if(%victim)
                                                {
                                                        messageClient(%client, "", '\c2More than one person was found with \'\c4%1\c2\' in their name. Please be more specfic.', %victimnamepart);
                                                        return;
                                                }
                                                else if(%client != %cl)
                                                        %victim = %cl;
                                        }
                                }
                                //No moderator fights...
                                if(%victim.isModerator)
                
                                {
                        
                                        return messageClient(%client,'',"\c2You cannot ban\c4 " @ %victim.name @ " \c2because he is of moderator or higher status.");
                                }


                                if(%victim && !%client.isAdmin)
                                {
                                    %oldAdmin = %client.isAdmin;
                                    %client.isAdmin = 1;
                                     serverCmdBan(%client,%victim.getPlayerName(), %victim.BL_ID, 10,"MODERATOR BAN: "@%reason);
                                    %client.isAdmin = %oldAdmin;
                                }

                                else
                                {
                                        messageClient(%client, "", '\c2Nobody was found with \'\c4%1\c2\' in their name.', %victimnamepart);
                                        return;
                                }
                }
                        
                        else
                        {
                                Parent::serverCmdMessageSent(%client, %text);
                        }
        }

function serverCmdRTB_adminPlayer(%client,%victim)
{
        if(%victim.isModerator)
        {
                return messageClient(%client, '', "\c2Error: Revoke moderator status from \c4" @ %victim.name @ " \c2before admining him.");
                
        }
                parent::serverCmdRTB_adminPlayer(%client,%victim);            
}

};
ActivatePackage(PlayerModerator);

function serverCmdMakeMod(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may grant Moderator Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isModerator || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of moderator or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Moderator (Manual)', %victim.name);        

                %Victim.isModerator = true;
				%victim.colorName = "\c4[Moderator] \c3" @ %oldPrefix;
        }
}

function serverCmdMakeModAuto(%client, %Victim)

{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may grant Moderator Status.");
        }

        
        %Victim = findClientByName(%Victim);
        
        if(%victim.isModerator || %victim.isAdmin || %victim.isSuperAdmin)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is already of moderator or higher status.");
        }


        else
        {
                messageAll('MsgAdminForce', '\c4%1 has become Moderator (Auto)', %victim.name);     
                schedule(100, 0, Mod_addAutoStatus, %victim);

                %Victim.isModerator = true;
				%victim.colorName = "\c4[Moderator] \c3" @ %oldPrefix;
        }
}




function serverCmdUnMod(%client, %Victim)

{
        
        if(!%client.isAdmin && !%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Moderator Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isModerator)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a moderator.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Moderatored (Manual)', %victim.name);        

        %Victim.isModerator = false;
		%victim.colorName = "<color:FFFF00>";
        }
}

function serverCmdUnModAuto(%client, %Victim)
{
        
        if(!%client.isSuperAdmin)
        {
                
                return messageClient(%client, '', "\c2Only \c4Admins \c2may revoke Moderator Status.");
        }

        %Victim = findClientByName(%Victim);

        if(!%victim.isModerator)
                
        {
                        
                return messageClient(%client,'',"\c2Error: \c4" @ %victim.name @ " \c2is not a moderator.");
        }


        else
        {
        messageAll('MsgAdminForce', '\c4%1 has been De-Moderatored (Auto)', %victim.name);    
        schedule(100, 0, Mod_removeAutoStatus, %victim); 		

        %Victim.isModerator = false;
		%victim.colorName = "<color:FFFF00>";
        }
}


function serverCmdListModerators(%client)
{
        if(%client.isModerator || %client.isAdmin || %client.isSuperAdmin)
        {
                listModerators(%client);
        }

        else
        {
                messageClient(%client,'',"\c2Error: You must be of moderator status or higher to use this command.");
        }
}

function listModerators(%client)
{        
        for(%i=0; %i < ClientGroup.getCount(); %i++)  
        {
                %target = ClientGroup.getObject(%i);
                
                if(%target.isModerator) 
                {
                   messageClient(%client,'',"\c4" @ %target.name @ " \c2is a moderator.");
                }
           }
}


//- serverCmdRTB_addAutoStatus (Allows a client to add a player to the auto list) --- Edited for Moderator
function Mod_addAutoStatus(%client)
{

        $Pref::Server::AutoModList = addItemToList($Pref::Server::AutoModList,%client.bl_id);

        export("$Pref::Server::*","config/server/prefs.cs");
        
}


//- serverCmdRTB_removeAutoStatus (Removes a player from the auto lists) --- Edited for Moderator
function Mod_removeAutoStatus(%client)
{
                $Pref::Server::AutoModList = removeItemFromList($Pref::Server::AutoModList,%client.bl_id);

                     export("$Pref::Server::*","config/server/prefs.cs");
}

function checkmod(%client)
{
        %list = $Pref::Server::AutoModList;
        %bl_id = %client.bl_id;
        if(hasItemOnList(%list,%bl_id))
        {
                %client.isModerator = true;
                messageAll('MsgAdminForce', '\c4%1 has become Moderator (Auto)', %client.name);
		%client.colorName = "\c4[Moderator] \c3" @ %oldPrefix;
        }
}  

function serverCmdListMods(%client)
{
	serverCmdListModerators(%client);
}