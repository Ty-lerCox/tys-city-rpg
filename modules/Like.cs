//RTB prefs

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
  if(!$RTB::RTBR_ServerControl_Hook)
    exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
  RTB_registerPref("Enabled","Reputation","$Reputation::Enabled","bool","Script_Reputation",1,0,1);
  RTB_registerPref("Allow likes","Reputation","$Reputation::Liking","bool","Script_Reputation",1,0,1);
  RTB_registerPref("Allow hates","Reputation","$Reputation::Hating","bool","Script_Reputation",1,0,1);
  RTB_registerPref("Allow normal","Reputation","$Reputation::Normals","bool","Script_Reputation",1,0,1);
  RTB_registerPref("Allow reset","Reputation","$Reputation::Reseting","bool","Script_Reputation",0,0,1);
}
else
{
  $Reputation::Enabled = 1;
  $Reputation::Liking = 1;
  $Reputation::Hating = 1;
  $Reputation::Normals = 1;
  $Reputation::Reseting = 0;
}

//Events

registerOutputEvent("fxDtsBrick","RepCheck","int 0 32 0" TAB "int 0 32 0",1);
registerInputEvent("fxDTSBrick", "onRepFalse",  "Self fxDTSBrick" TAB
                                                 "Player Player" TAB
                                                 "Client GameConnection" TAB
                                                 "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onRepTrue",  "Self fxDTSBrick" TAB
                                                 "Player Player" TAB
                                                 "Client GameConnection" TAB
                                                 "MiniGame MiniGame");

function fxDtsBrick::RepCheck(%brick,%likemax,%hatemax,%client) {
  //echo(%brick SPC %likemax SPC %hatemax SPC %client);
  %likes = %client.Reputation_Likes;
  %hates = %client.Reputation_Hates;
  
  if(%likes >= %likemax && %hates <= %hatemax) {
    //setup targets
    //echo("good");
    %obj = %brick;
    //player = person who destroyed it
    $InputTarget_["Self"]   = %obj;
    $InputTarget_["Player"] = %client.player;
    $InputTarget_["Client"] = %client;

    if($Server::LAN)
    {
        $InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
    }
    else
    {
        if(getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
        else
	$InputTarget_["MiniGame"] = 0;
    }
    //process the event
    %obj.processInputEvent("onRepTrue", %client);
  } else {
    //setup targets
    //echo("bad");
    %obj = %brick;
    //player = person who destroyed it
    $InputTarget_["Self"]   = %obj;
    $InputTarget_["Player"] = %client.player;
    $InputTarget_["Client"] = %client;

    if($Server::LAN)
    {
        $InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
    }
    else
    {
        if(getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
        else
	$InputTarget_["MiniGame"] = 0;
    }
    //process the event
    %obj.processInputEvent("OnRepFalse", %client);
  }
}

//Setting up some functions

function Reputation_Load(%client) {
  if(isFile("config/server/Reputation/"@%client.name@"/likes.cs") || isFile("config/server/Reputation/"@%client.name@"/hates.cs")) {
    //likes
    %file = new FileObject();
    %file.openForRead("config/server/Reputation/"@%client.name@"/likes.cs");
    while(!%file.isEOF) {
      %liker = %file.readLine();
      %client.Reputation_Likes++;
      %client.Reputation_Like[%client.Reputation_Likes] = findClientByBL_ID(%liker);
    }
    %file.close();
    %file.delete();
    
    //hates
    %file = new FileObject();
    %file.openForRead("config/server/Reputation/"@%client.name@"/hates.cs");
    while(!%file.isEOF) {
      %hater = %file.readLine();
      %client.Reputation_Hates++;
      %client.Reputation_Hate[%client.Reputation_Hates] = findClientByBL_ID(%hater);
    }
    %file.close();
    %file.delete();
    //echo("Reputation- Loaded "@%client.name@"'s reputation. "@%client.Reputation_Likes@" likes, "@%client.Reputation_Hates@" haters.");
  } else {
    //echo("Reputation- Tried loading "@%client.name@"'s reputation stats, but he does not have any likes or hates. Resetting");
    %client.Reputation_Likes = 0;
    %client.Reputation_Hates = 0;
  }
}

function Reputation_Save(%client) {
  //likes
  %file = new FileObject();
  %file.openForWrite("config/server/Reputation/"@%client.name@"/likes.cs");
  if(%client.Reputation_Likes != 0) {
    for(%i=1;%i<=%client.Reputation_Likes;%i++) {
      %file.writeLine(%client.Reputation_Like[%i].bl_id);
    }
  }
  %file.close();
  %file.delete();
  
  //hates
  %file = new FileObject();
  %file.openForRead("config/server/Reputation/"@%client.name@"/hates.cs");
  if(%client.Reputation_Hates != 0) {
    for(%i=1;%i<%client.Reputation_Hates;%i++) {
      %file.writeLine(%client.Reputation_Hate[%i].bl_id);
    }
  }
}

function Reputation_Check(%client,%per,%target) {
  if(%per == 1) {
    for(%i=0;%i<=%client.Reputation_Likes;%i++) {
      if(%client.Reputation_Like[%i].name $= %target.name)
        return true;
    }
    return false;
  } else {
    for(%i=0;%i<=%client.Reputation_Hates;%i++) {
      if(%client.Reputation_Hate[%i].name $= %target.name)
        return true;
    }
    return false;
  }
}

//Server commands

function serverCmdlike(%client,%arg1) 
{
	if(!$Reputation::Enabled) 
	{
		messageClient(%client,'',"\c6The reputation mod is disabled");
		return;
	}
	if(!isObject(findClientByname(%arg1))) 
	{
		messageClient(%client,'',"\c6This person doesn't exist.");
		return;
	}
	if(Reputation_Check(findClientByName(%arg1),1,%client)) 
	{
		messageClient(%client,'',"\c6You already like this person.");
		return;
	}
	if(!$Reputation::Liking) {
		messageClient(%client,'',"\c6Liking is not enabled.");
		return;
	}
	if(%arg1 $= "") {
		messageClient(%client,'',"\c6You need to specify a person to like");
		return;
	}
	%target = findClientByName(%arg1);
	%target.Reputation_Likes++;
	%target.Reputation_Like[%target.Reputation_Likes] = %client;
	messageClient(%client,'',"\c6You now like "@%target.name@".");
	messageClient(%target,'',"\c6"@%client.name@" likes you. You've recieved 5 rep.");
}

function serverCmdRepReset(%client,%target) {
  if(%client.isAdmin) {
    if(isObject(findClientByName(%target))) {
      %targ = findClientByName(%target);
      for(%i=0;%i<=%targ.Reputation_Likes;%i++) {
        %targ.Reputation_Like[%i] = "";
      }
      %targ.Reputation_Likes = 0;
      for(%i=0;%i<=%targ.Reputation_Hates;%i++) {
        %targ.Reputation_Hate[%i] = "";
      }
      %targ.Reputation_Hates = 0;
      messageClient(%client,'',"\c6You have reset "@%targ.name@"'s reputation stats");
      messageClient(%targ,'',"\c6"@%client.name@" has reset your reputation stats");
    } else {
      messageClient(%client,'',"\c6That client doesn't exist");
    }
  } else {
    messageClient(%client,'',"\c6You need to be admin to do this");
  }
}

//Setting up the package

if(isPackage(Reputation))
  deactivatePackage(Reputation);

package Reputation {
  function GameConnection::onClientEnterGame(%client) {
    Parent::onClientEnterGame(%client);
    schedule(3000,0,Reputation_Load,%client);
  }
};

activatePackage(Reputation);