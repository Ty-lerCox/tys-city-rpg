//////////////////////////////////////////////////////////////////////////////////////////////////
//				     Support_SpecialKills.cs     				//
//Creator: Space Guy [ID 130]									//
//Allows you to create special extra messages added to damage types in minigames		//
//												//
//addSpecialDamageMsg(string name,		- unique name for the message			//
//			string murderMessage,	- message displayed on kill			//
//			string suicideMessage,	- message displayed on self-kill		//
//												//
//string isSpecialKill_name(%this,%sourceObject,%sourceClient,%mini)				//
// return "0" for no special message								//
// return "1" for special message								//
// return "2" TAB [value] for extra values							//
//												//
//In the messages:										//
// %1 is the killed player and any messages after the name					//
// %2 is the killer's name and any messages before the name					//
// %3 is any text between the names (e.g. CI bitmap)						//
// %4+ are special strings sent by returning "2" TAB [value] TAB [...] from the function	//
//////////////////////////////////////////////////////////////////////////////////////////////////

if($SpaceMods::Server::SpecialKillsVersion > 3)
   return;

$SpaceMods::Server::SpecialKillsVersion = 3;

function addSpecialDamageMsg(%name,%murderMsg,%suicideMsg)
{
   if($SpecialDamage_NumTypes $= "")
      $SpecialDamage_NumTypes = -1;
   
   if($SpecialDamage_TypeID[%name] $= "")
      %id = $SpecialDamage_NumTypes++;
   else
      %id = $SpecialDamage_TypeID[%name];
   
   $SpecialDamage_TypeID[%name] = %id;
   $SpecialDamage_TypeName[%id] = %name;
   $SpecialDamage_MurderMessage[%id] = %murderMsg;
   $SpecialDamage_SuicideMessage[%id] = %suicideMsg;
}

package SpecialKills
{
   function GameConnection::onDeath(%this,%sourceObject,%sourceClient,%damageType,%damageArea)
   {
      %mini = (isObject(%this.minigame) ? %this.minigame : 0);
      %kill = (isObject(%sourceClient) && %sourceClient != %this);
     
      %curmsg = "%2%3%1";
      for(%i=0;$SpecialDamage_TypeName[%i] !$= "";%i++)
      {
         %f = "isSpecialKill_" @ $SpecialDamage_TypeName[%i];
         if(!isFunction(%f))
            continue;
         
         %val = call(%f,%this,%sourceObject,%sourceClient,%mini);
         %num = getField(%val,0);
         
         if(!%num)
            continue;
         
         if(%num == 2)
         {
            %extra = getField(%val,1);
            %extra2 = getField(%val,2);
            %extra3 = getField(%val,3);
         }
         else
         {
            %extra = "";
            %extra2 = "";
            %extra3 = "";
         }
         
         if(%kill)
         {
            %msg = $SpecialDamage_MurderMessage[%i];
           
            %pos1 = strPos(%msg,"%2");
            %pos2 = strPos(%msg,"%1");
           
            //echo("SPECIAL KILL TYPE = " @ $SpecialDamage_TypeName[%i]);
           
            if(%pos1 < 0)
            {
               %killerString = "";
               %pos1 = -2;
            }
            else
               %killerString = getSubStr(%msg,0,%pos1+2);
           
            if(%pos2 < 0)
            {
               %killedString = "";
               %pos2 = 0;
            }
            else
               %killedString = getSubStr(%msg,%pos2,100);
           
            %ciString = getSubStr(%msg,%pos1+2,%pos2-%pos1-2);
         }
         else
         {
            %msg = $SpecialDamage_SuicideMessage[%i];
           
            %pos1 = strPos(%msg,"%1");
            %killerString = "";
           
            //echo("SPECIAL SUICIDE TYPE = " @ $SpecialDamage_TypeName[%i]);
           
            if(%pos1 < 0)
            {
               %killedString = "";
               %pos1 = 0;
            }
            else
               %killedString = getSubStr(%msg,%pos1,100);
           
            %ciString = getSubStr(%msg,0,%pos1);
         }
         
         %curmsg = strReplace(%curmsg,"%1",%killedString);
         %curmsg = strReplace(%curmsg,"%2",%killerString);
         %curmsg = strReplace(%curmsg,"%3",%ciString);
         %curmsg = strReplace(%curmsg,"%4",%extra);
         %curmsg = strReplace(%curmsg,"%5",%extra2);
         %curmsg = strReplace(%curmsg,"%6",%extra3);
         
         %specialMessage = 1;
      }
     
      if(%specialMessage)
      {
         if(%kill)
         {
            %msg = getTaggedString($DeathMessage_Murder[%damageType]);
           
            %pos1 = strPos(%msg,"%2");
            %pos2 = strPos(%msg,"%1");
           
            //echo("NORMAL KILL");
           
            if(%pos1 < 0)
            {
               %killerString = "";
               %pos1 = -2;
            }
            else
               %killerString = getSubStr(%msg,0,%pos1+2);
           
            if(%pos2 < 0)
            {
               %killedString = "";
               %pos2 = 0;
            }
            else
               %killedString = getSubStr(%msg,%pos2,100);
           
            %ciString = getSubStr(%msg,%pos1+2,%pos2-%pos1-2);
         }
         else
         {
            %msg = getTaggedString($DeathMessage_Suicide[%damageType]);
            %pos1 = strPos(%msg,"%1");
            %killerString = "";
           
            //echo("NORMAL SUICIDE");
           
            if(%pos1 < 0)
            {
               %killedString = "";
               %pos1 = 0;
            }
            else
               %killedString = getSubStr(%msg,%pos1,100);
           
            %ciString = getSubStr(%msg,0,%pos1);
         }
         
         %msg = strReplace(%curmsg,"%1",%killedString);
         %msg = strReplace(%curmsg,"%2",%killerString);
         %msg = strReplace(%curmsg,"%3",%ciString);
         
         %msg = strReplace(%msg,"%1",%this.getPlayerName());
         if(%kill)
            %msg = strReplace(%msg,"%2",%sourceClient.getPlayerName());
         
         if(isObject(%mini))
            %mini.messageAll('',%msg);
         else
            messageAll('',%msg);
         
         %damageType = 0;
         %this.player.lastDirectDamageType = 0;
      }
     
      Parent::onDeath(%this,%sourceObject,%sourceClient,%damageType,%damageArea);
   }
};activatePackage(SpecialKills);