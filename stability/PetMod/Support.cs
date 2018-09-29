//+--
//Project: PetMod; Support.cs
//Description: The support that backs the rest of the mod.
//Authors: halcynthis, Aoki, Fooly Cooly
//+--

datablock ParticleData(Skin)
{
	textureName = "./Shapes/base.blank.png";
};

datablock ParticleData(MonkeyHand)
{
	textureName = "./Shapes/yellow.blank.jpg";
};

datablock PlayerData(PetArmor : HorseArmor)
{
	maxSideCrouchSpeed = 6;
	maxSideSpeed = 6;
	jumpForce = 17 * 100;
	uiname = "";
	numMountPoints = 0;
};

datablock ShapeBaseImageData(DogImage)
{
	shapeFile = "./Shapes/Dog.dts";
	emap = false;
	doColorShift = true;
	colorshiftcolor = "0.4 0.3 0.2 1";
	mountPoint = $BackSlot;
	offset = "0 0 -0.35";
	eyeOffset = "-1 -1 -1";
	rotation = "0 0 1 180";
	className = "ItemImage";
};	

datablock ShapeBaseImageData(MonkeyImage)
{
	shapeFile = "./Shapes/Monkey.dts";
	emap = false;
	doColorShift = True;
	colorshiftcolor = "0.4 0.3 0.2 1";
	mountPoint = $BackSlot;
	offset = "0 0 -1.6";
	eyeOffset = "-1 -1 -1";
	rotation = "0 0 1 180";
	className = "ItemImage";
};

datablock ShapeBaseImageData(SkeleBodyImage)
{
	shapeFile = "./Shapes/SkeleBody.dts";
	emap = false;
	doColorShift = True;
	colorshiftcolor = "1 1 1 1";
	mountPoint = 2;
	offset = "0 .02 -1.18";
	eyeOffset = "-1 -1 -1";
};

datablock ShapeBaseImageData(SkeleHeadImage)
{
	shapeFile = "./Shapes/SkeleHead.dts";
	emap = false;
	doColorShift = True;
	colorshiftcolor = "1 1 1 1";
	mountPoint = 5;
	offset = "0 0 -.4";
	eyeOffset = "-1 -1 -1";
};

function HideHeadNodes(%Player)
{
	%Player.hideNode("headSkin");
	%Player.unMountImage(2);
	for(%i=0;$accent[%i]	!$= "";%i++) %Player.hideNode($accent[%i]);
	for(%i=0;$hat[%i]	!$= "";%i++) %Player.hideNode($hat[%i]);
}

function ClearAllPlayerNodes(%Player)
{
	switch(isObject(%Player))
	{
		case 1:
			HideHeadNodes(%Player);
			%Player.hideNode("LSki");
			%Player.hideNode("RSki");
			%Player.hideNode("skirtTrimLeft");
			%Player.hideNode("skirtTrimRight");
			for(%i=0;%i		<5    ;%i++) %Player.unMountImage(%i);
			for(%i=0;$chest[%i]	!$= "";%i++) %Player.hideNode($chest[%i]);
			for(%i=0;$hip[%i]	!$= "";%i++) %Player.hideNode($hip[%i]);
			for(%i=0;$LArm[%i]	!$= "";%i++) %Player.hideNode($LArm[%i]);
			for(%i=0;$LHand[%i]	!$= "";%i++) %Player.hideNode($LHand[%i]);
			for(%i=0;$LLeg[%i]	!$= "";%i++) %Player.hideNode($LLeg[%i]);
			for(%i=0;$pack[%i]	!$= "";%i++) %Player.hideNode($pack[%i]);
			for(%i=0;$RArm[%i]	!$= "";%i++) %Player.hideNode($RArm[%i]);
			for(%i=0;$RHand[%i]	!$= "";%i++) %Player.hideNode($RHand[%i]);
			for(%i=0;$RLeg[%i]	!$= "";%i++) %Player.hideNode($RLeg[%i]);
			for(%i=0;$secondPack[%i]!$= "";%i++) %Player.hideNode($secondPack[%i]);
	}
}

function unHideHeadNodes(%Client)
{
	%Player = %Client.player;
	%Player.unHideNode("headSkin");
	%Player.setNodeColor("headSkin", %client.headColor);
	%Player.unHideNode($hat[%client.hat]);
	%Player.setNodeColor($hat[%client.hat], %client.hatColor);
	%Player.unHideNode("HeadSkin");
	%Player.unHideNode($Hat[%Client.hat]);
	if($accent[%Client.accent] !$= "none" && %Client.accent)
	{
		if(%Client.hat == 1){%Player.unHideNode($accent[4]);}
		else if(%Client.hat > 0){%Player.unHideNode($accent[%client.accent]);}
		%Player.setNodeColor($accent[%client.accent],%client.accentColor);
	}
}
