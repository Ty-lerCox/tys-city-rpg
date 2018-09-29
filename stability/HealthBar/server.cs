//3D Health Bar
//Made by Wrapperup

if(isFile("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	RTB_registerPref("Enabled","3D Health Bar","$Pref::Server::TDHealthBar::Enable","bool","Server_3DHealthBar",1,0,0);
	RTB_registerPref("Stealth Mode","3D Health Bar","$Pref::Server::TDHealthBar::Stealth","bool","Server_3DHealthBar",1,0,0);
}
else
{
	$Pref::Server::TDHealthBar::Enable = true;
	$Pref::Server::TDHealthBar::Stealth = false;
}
	
	
//Uses 1 model with different animation states.
datablock ShapeBaseImageData(HBarA)
{
	shapeFile = "./healthbar.dts";
	emap = true;
	
	mountPoint = 7;
	offset = "0 0 2.8";
	eyeOffset = "0 0 -99999";
	doColorShift = false;
	
	stateName[0] = "Start";
	stateSequence[0] = "aa";
};

datablock ShapeBaseImageData(HBarB : HBarA)
{
	stateSequence[0] = "bb";
};

datablock ShapeBaseImageData(HBarC : HBarA)
{
	stateSequence[0] = "cc";
};

datablock ShapeBaseImageData(HBarD : HBarA)
{
	stateSequence[0] = "dd";
};

datablock ShapeBaseImageData(HBarE : HBarA)
{
	stateSequence[0] = "ee";
};

datablock ShapeBaseImageData(HBarF : HBarA)
{
	stateSequence[0] = "ff";
};

datablock ShapeBaseImageData(HBarG : HBarA)
{
	stateSequence[0] = "gg";
};

datablock ShapeBaseImageData(HBarH : HBarA)
{
	stateSequence[0] = "hh";
};

datablock ShapeBaseImageData(HBarI : HBarA)
{
	stateSequence[0] = "ii";
};

datablock ShapeBaseImageData(HBarJ : HBarA)
{
	stateSequence[0] = "jj";
};

function serverCmdHBar(%client)
{
	%client.player.applyHBar();
}

function Player::applyHBar(%this,%time)
{
	if(!$Pref::Server::TDHealthBar::Enable)
		return;
	%hp = mCeil(%this.getDatablock().maxDamage - %this.getDamageLevel());
	%maxhp = %this.getDatablock().maxDamage;
	%hpdiv = mCeil(%maxhp / 10);
	
	//Sorry if this doesn't look about right. I may optimize in next version if needed.
	if(%hp == %maxhp)
		%this.mountImage(HBarA,3);
	else if(%hp >= %hpdiv*9)
		%this.mountImage(HBarB,3);
	else if(%hp >= %hpdiv*8)
		%this.mountImage(HBarC,3);
	else if(%hp >= %hpdiv*7)
		%this.mountImage(HBarD,3);
	else if(%hp >= %hpdiv*6)
		%this.mountImage(HBarE,3);
	else if(%hp >= %hpdiv*5)
		%this.mountImage(HBarF,3);
	else if(%hp >= %hpdiv*4)
		%this.mountImage(HBarG,3);
	else if(%hp >= %hpdiv*3) 
		%this.mountImage(HBarH,3);
	else if(%hp >= %hpdiv*2)
		%this.mountImage(HBarI,3);
	else if(%hp >= %hpdiv)
		%this.mountImage(HBarJ,3);
	else if(%hp >= 0)
		%this.unmountImage(3);
		
	if(%time !$= "" && %hp != 0)
	{
		while(isEventPending(%this.hbarSche))
			cancel(%this.hbarSche);
			
		%this.hbarSche = %this.schedule(%time,unmountImage,3);
	}
}

function serverCmdHBar(%client)
{
	if($Pref::Server::TDHealthBar::Stealth)
	{
		if(isObject(%client.player))
			%client.player.applyHBar(1000);
		else
			%client.chatMessage("You need to be alive!");
	}
	else
		%client.chatMessage("Stealth Mode is not enabled, as your health should show all the time, this command is not required.");
}

package HbarTD
{
	function Player::setHealth(%this,%health)
	{
		parent::setHealth(%this,%health);
		if($Pref::Server::TDHealthBar::Enable)
		{
			if($Pref::Server::TDHealthBar::Stealth)
				%this.applyHBar(1000);
			else
				%this.applyHBar();
		}
	}
	
	function Player::addHealth(%this,%health)
	{
		parent::addHealth(%this,%health);
		if($Pref::Server::TDHealthBar::Enable)
		{
			if($Pref::Server::TDHealthBar::Stealth)
				%this.applyHBar(1000);
			else
				%this.applyHBar();
		}
	}
	
	function GameConnection::onDeath(%client,%killerPlayer,%killer,%damageType,%damageLoc)
	{
		if(isObject(%client.player) && $Pref::Server::TDHealthBar::Enable)
			%client.player.unmountImage(3);
		
		parent::onDeath(%client,%killerPlayer,%killer,%damageType,%damageLoc);
	}
	
	function Armor::Damage(%this, %obj, %sourceObject, %pos, %am, %damtype)
	{
		parent::Damage(%this, %obj, %sourceObject, %pos, %am, %damtype);
		
		if(isObject(%obj) && $Pref::Server::TDHealthBar::Enable)
		{
			if($Pref::Server::TDHealthBar::Stealth)
				%obj.applyHBar(1000);
			else
				%obj.applyHBar();
		}
	}
	
	function GameConnection::spawnPlayer(%this)
	{
		parent::spawnPlayer(%this);
		
		if($Pref::Server::TDHealthBar::Enable)
		{
			%obj = %this.player;

			if(isObject(%obj))
			{
				if($Pref::Server::TDHealthBar::Stealth)
					%obj.schedule(100,applyHBar,1000);
				else
					%obj.schedule(100,applyHBar);
			}
		}
	}
	
};
activatePackage(HbarTD);