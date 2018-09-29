//+--
//Project: PetMod; Script_PetMod.cs
//Description: The main .cs file that houses all the important functions.
//Authors: halcynthis, Aoki, Fooly Cooly
//+--

//Section One: Server Commands
$Pets::Dog::Cost = 15;
$Pets::Monkey::Cost = 30;
$Pets::Horse::Cost = 5;
$Pets::Skeleton::Cost = 100;
$Pets::Point::Cost = 1000;

function BuyPetPoints(%client, %amount)
{
	%amountOfPoints = mFloor(%amount);
	
	%cost = %amountOfPoints * $Pets::Point::Cost;
	
	if(%cost < 1)
		return messageClient(%client, '', "Error.");
		
	if(CityRPGData.getData(%client.bl_id).valueMoney - %cost < 0)
		return messageClient(%client, '', "Error. Need more money.");
	
	CityRPGData.getData(%client.bl_id).valueMoney -= %cost;
	CityRPGData.getData(%client.bl_id).valuePetPoints += %amountOfPoints;
	//
	%client.SetInfo();
}

function serverCmdgpetpoints(%client, %arg1)
{
	if(%client.isAdmin)
		CityRPGData.getData(%client.bl_id).valuePetPoints += %arg1;
	else
		return;
	messageClient(%client,'',"Granted." SPC CityRPGData.getData(%client.bl_id).valuePetPoints);
}

function AvaliablePoints(%client)
{
	%pointsAllowed = CityRPGData.getData(%client.bl_id).valuePetPoints;
	for(%i = 1; %i <= 5; %i++)
	{
		if(%client.pet[%i].petType !$= "")
		{
			if(%client.pet[%i].petType $= "Dog")
				%pointsUsed += $Pets::Dog::Cost;
			else if(%client.pet[%i].petType $= "Monkey")
				%pointsUsed += $Pets::Monkey::Cost;
			else if(%client.pet[%i].petType $= "Horse")
				%pointsUsed += $Pets::Horse::Cost;
			else if(%client.pet[%i].petType $= "Skeleton")
				%pointsUsed += $Pets::Skeleton::Cost;
			else
				talk("PetMod - AvaliablePoints(): Line 31-ish");
		}
	}
	%avaliablePoints = %pointsAllowed - %pointsUsed;
	return %avaliablePoints;
}

function UsedPoints(%client)
{
	%pointsAllowed = CityRPGData.getData(%client.bl_id).valuePetPoints;
	for(%i = 1; %i <= 5; %i++)
	{
		if(%client.pet[%i].petType !$= "")
		{
			if(%client.pet[%i].petType $= "Dog")
				%pointsUsed += $Pets::Dog::Cost;
			else if(%client.pet[%i].petType $= "Monkey")
				%pointsUsed += $Pets::Monkey::Cost;
			else if(%client.pet[%i].petType $= "Horse")
				%pointsUsed += $Pets::Horse::Cost;
			else if(%client.pet[%i].petType $= "Skeleton")
				%pointsUsed += $Pets::Skeleton::Cost;
			else
				talk("PetMod - PointsUsed(): Line 55-ish");
		}
	}
	if(%pointsUsed < 1)
		%pointsUsed=0;
	return %pointsUsed;
}

function petRecall(%client)
{
	talk("1" SPC %client.pet[1].petType);
	talk("2" SPC %client.pet[2].petType);
	talk("3" SPC %client.pet[3].petType);
	talk("4" SPC %client.pet[4].petType);
	talk("5" SPC %client.pet[5].petType);
	talk("6" SPC %client.pet[6].petType);
	talk("7" SPC %client.pet[7].petType);
	talk("8" SPC %client.pet[8].petType);
}

function removeAnimal(%datablockID)
{
	messageAll('',%datablockID.owner @ "'s pet (" @ %datablockID.petType @ ") was removed from the game.");
	%datablockID.delete();
}

function serverCmdgivepetpoint(%client, %money, %name)
{if(!isObject(%client.player)) 
		return;
	%money = mFloor(%money);
	
	if(%money > 0)
	{
		if((CityRPGData.getData(%client.bl_id).valuePetPoints - %money) >= 0)
		{
			if(isObject(%client.player))
			{
				if(%name != "")
				{
					%target = findclientbyname(%name);
				}
				else
                {
					%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
				}
				if(isObject(%target))
				{
					messageClient(%client, '', "\c6You give \c3" @ %money SPC "pet points \c6to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3" @ %money @ " pet points\c6.");
					
					CityRPGData.getData(%client.bl_id).valuePetPoints -= %money;
					CityRPGData.getData(%target.bl_id).valuePetPoints += %money;
					
					%client.SetInfo();
					%target.SetInfo();
				}
				else
					messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player in order to give them pet points. \nYou can also type in the person's name after the amount.");
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command.");
		}
		else
			messageClient(%client, '', "\c6You don't have that much pet points to give.");
	}
	else
		messageClient(%client, '', "\c6You must enter a valid amount of pet points to give.");
}

function CanBuyAnimal(%client, %petName, %petType)
{
	if(%petType $= "Dog")
		%cost = $Pets::Dog::Cost;
	else if(%petType $= "Monkey")
		%cost = $Pets::Monkey::Cost;
	else if(%petType $= "Horse")
		%cost = $Pets::Horse::Cost;
	else if(%petType $= "Skeleton")
		%cost = $Pets::Skeleton::Cost;
	
	%avaliablePoints = AvaliablePoints(%client);
	if(%avaliablePoints - %cost >= 0)
		return true;
	else
		return false;
}
//getDog(id(997),"tan");
function GetDog(%client, %petName)
{
	
	if(%petName.Owner !$= "")
	{
		messageClient(%client, '', "\c6There is already a pet named\c3 "@%petName);
		return;
	}

	if(%client.pets >= 5)
	{
		messageClient(%client, '', "\c6You may only have five pets.");
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(!CanBuyAnimal(%client,%petName,"Dog"))
		return messageClient(%client,'',"\c6You must have the required amount of pet points to buy this animal");
		
	messageClient(%client, '', "\c6You adopted a pet and named him \c3"@%petName@"\c6.");

	new AIPlayer(%petName)
	{
		datablock = "PetArmor";
		position = %client.player.getPosition();
	};
	
	%petName.Owner = %client.netName;
	%petName.petName = %petName;
	%petName.setPlayerScale("0.4 0.4 0.4");
	%petName.setMoveObject(%client.player);
	%petName.mountimage(DogImage, 1);
	%petName.hideNode(body);
	%petName.hideNode(head);
	%petName.isPet = 1;
	%petName.petType =  "Dog";
	%client.pets += 1;
	%client.pet[%client.pets] = %petName;
}

function GetMonkey(%client, %petName)
{
	if(%petName.Owner !$= "")
	{
		messageClient(%client, '', "\c6There is already a pet named\c3 "@%petName);
		return;
	}

	if(%client.pets >= 5)
	{
		messageClient(%client, '', "\c6You may only have five pets.");
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(!CanBuyAnimal(%client,%petName,"Monkey"))
		return messageClient(%client,'',"\c6You must have the required amount of pet points to buy this animal");
		
	messageClient(%client, '', "\c6You adopted a pet and named him \c3"@%petName@"\c6.");

	new AIPlayer(%petName)
	{
		datablock = "PetArmor";
		position = %client.player.getPosition();
	};

	%petName.Owner = %client.netName;
	%petName.petName = %petName;
	%petName.setPlayerScale("0.5 0.5 0.5");
	%petName.setMoveObject(%client.player);
	%petName.mountimage(MonkeyImage, 1);
	%petName.hideNode(body);
	%petName.hideNode(head);
	%petName.petType =  "Monkey";
	%client.pets += 1;
	%client.pet[%client.pets] = %petName;
}

function GetHorse(%client, %petName)
{
	if(%petName.Owner !$= "")
	{
		messageClient(%client, '', "\c6There is already a pet named\c3 "@%petName);
		return;
	}

	if(%client.pets >= 5)
	{
		messageClient(%client, '', "\c6You may only have five pets.");
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}
	
	if(!CanBuyAnimal(%client,%petName,"Horse"))
		return messageClient(%client,'',"\c6You must have the required amount of pet points to buy this animal");

	messageClient(%client, '', "\c6You adopted a pet and named him \c3"@%petName@"\c6.");

	new AIPlayer(%petName)
	{
		datablock = "HorseArmor";
		position = %client.player.getPosition();
	};

	%petName.Owner = %client.netName;
	%petName.petName = %petName;
	%petName.setPlayerScale("1 1 1");
	%petName.setMoveObject(%client.player);
	%petName.petType =  "Horse";
	%client.pets += 1;
	%client.pet[%client.pets] = %petName;
}

function GetChild(%client, %petName)
{
	if(%petName.Owner !$= "")
	{
		messageClient(%client, '', "\c6There is already a child named\c3 "@%petName);
		return;
	}

	if(%client.pets >= 5)
	{
		messageClient(%client, '', "\c6You may only have five pets or children.");
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a child name.");
		return;
	}
	
	if(!CanBuyAnimal(%client,%petName,"Horse"))
		return messageClient(%client,'',"\c6You must have the required amount of pet points to buy this animal");
		
	messageClient(%client, '', "\c6You adopted a child and named him \c3"@%petName@"\c6.");

	new AIPlayer(%petName)
	{
		datablock = "PlayerStandardArmor";
		position = %client.player.getPosition();
	};

	%petName.Owner = %client.netName;
	%petName.petName = %petName;
	%petName.setPlayerScale("0.6 0.6 0.6");
	%petName.setMoveObject(%client.player);
	%petName.petType =  "Child";
	%client.pets += 1;
	%client.pet[%client.pets] = %petName;
}

function GetSkeleton(%client, %petName)
{
	if(%petName.Owner !$= "")
	{
		messageClient(%client, '', "\c6There is already a pet named\c3 "@%petName);
		return;
	}

	if(%client.pets >= 5)
	{
		messageClient(%client, '', "\c6You may only have five pets.");
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(!CanBuyAnimal(%client,%petName,"Skeleton"))
		return messageClient(%client,'',"\c6You must have the required amount of pet points to buy this animal");
		
	messageClient(%client, '', "\c6You adopted a pet and named him \c3"@%petName@"\c6.");

	new AIPlayer(%petName)
	{
		datablock = "PlayerStandardArmor";
		position = %client.player.getPosition();
	};

	%petName.Owner = %client.netName;
	%petName.petName = %petName;
	%petName.setPlayerScale("0.6 0.6 0.6");
	%petName.setMoveObject(%client.player);
	%petName.petType =  "Skeleton";

	ClearAllPlayerNodes(%petName);

	%petName.unhideNode(RHand);
	%petName.unhideNode(LHand);
	%petName.unhideNode(RArmSlim);
	%petName.unhideNode(LArmSlim);
	%petName.unhideNode(RShoe);
	%petName.unhideNode(LShoe);
	%petName.larmColor = "1 1 1 1";
	%petName.llegColor = "1 1 1 1";
	%petName.rarmColor = "1 1 1 1";
	%petName.rlegColor = "1 1 1 1";
	%petName.rhandColor = "1 1 1 1";
	%petName.lhandColor = "1 1 1 1";
	%petName.setNodeColor(RHand, "1 1 1 1");
	%petName.setNodeColor(LHand, "1 1 1 1");
	%petName.setNodeColor(RArmSlim, "1 1 1 1");
	%petName.setNodeColor(LArmSlim, "1 1 1 1");
	%petName.setNodeColor(RShoe, "1 1 1 1");
	%petName.setNodeColor(LShoe, "1 1 1 1");
	%petName.mountImage(SkeleHeadImage, 2);
	%petName.mountImage(SkeleBodyImage, 1);

	%client.pets += 1;
	%client.pet[%client.pets] = %petName;
}

function servercmdKillPet(%client, %petName)
{
	if(%petName $= "All")
	{
		for(%i = 1; %i <= %client.pets; %i++)
		{
			%client.pet[%i].delete();
			%client.pet[%i] = "";
		}
		%client.pets = 0;
		return;
	}

	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3"@%petName@"\c6 is not your pet.");
		return;
	}

	for(%i = 1; %i <= 5; %i++)
	{
		if(%client.pet[%i] $= %petName)
		{
			%client.pet[%i].delete();
			%client.pet[%i] = "";
		}
	}

	%client.pets -= 1;
}

function servercmdStay(%client, %petName)
{
	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3"@%petName@"\c6 is not your pet.");
		return;
	}

	messageClient(%client, '', "\c6You asked your pet\c3 "@%petName@"\c6 to stay!");
	%petName.setMoveObject("");
}

function servercmdFollow(%client, %petName)
{
	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3"@%petName@"\c6 is not your pet.");
		return;
	}

	messageClient(%client, '', "\c6You asked your pet\c3 "@%petName@"\c6 to follow!");
	%petName.setMoveObject(%client.player);
}

function servercmdListPets(%client)
{
	if(%client.pet[1] $= "" && %client.pet[2] $= "" && %client.pet[3] $= "" && %client.pet[4] $= "" && %client.pet[5] $= "")
	{
		messageClient(%client, '', "\c6You have no pets.");
		return;
	}

	messageClient(%client, '', "\c6Here is a list of all your pets.");

	for(%i = 1; %i <= 5; %i++)
	{
		messageClient(%client, '', "\c3" @ %client.pet[%i]);
	}
}

function servercmdFetchPet(%client, %petName)
{
	if(%petName $= "")
	{
		messageClient(%client, '', "\c6Please enter a pet name.");
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3" @ %petName @ "\c6 is not your pet.");
		return;
	}

	messageClient(%client, '', "\c6You fetched your pal \c3" @ %petName @ "\c6.");
	%petName.setTransform(%client.player.getPosition());
}

function servercmdControl(%client, %petName)
{
	if(%petName $= "")
	{
		%client.setControlObject(%client.player);
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3"@%petName@"\c6 is not your pet.");
		return;
	}

	messageClient(%client, '', "\c6You took control of \c3"@%petName@"\c6.");
	%client.setControlObject(%petName);
}

function servercmdunControl(%client, %petName)
{
	%client.setControlObject(%client.player);
}

function servercmdResize(%client, %petName, %x, %y, %z)
{
	if(%petName $= "")
	{
		%client.setControlObject(%client.player);
		return;
	}

	if(%petName.Owner !$= %client.netName)
	{
		messageClient(%client, '', "\c3"@%petName@"\c6 is not your pet.");
		return;
	}

	if(%x >= 1)
		%x = 0.7;
	if(%y >= 1)
		%x = 0.7;
	if(%z >= 1)
		%z = 0.7;

	if(%x < 0.3)
		%x = 0.3;
	if(%y < 0.3)
		%y = 0.3;
	if(%z < 0.3)
		%z = 0.3;

	messageClient(%client, '', "\c6You resized your pet\c3" SPC %petName @ "\c6.");
	%petName.setPlayerScale(%x, %y, %z);
}

function servercmdPetHelp(%client)
{
	messageClient(%client, '', "\c6Welcome to PetMod!\c3 Here are all the server commands you are able to use.");
	//messageClient(%client, '', "\c3/getDog\c2 NAME\c6 - Gives you a pet Dog.");
	//messageClient(%client, '', "\c3/getMonkey\c2 NAME\c6 - Gives you a pet Monkey.");
	//messageClient(%client, '', "\c3/getHorse\c2 NAME\c6 - Gives you a pet Horse.");
	//messageClient(%client, '', "\c3/getSkeleton\c2 NAME\c6 - Gives you a pet Skeleton.");
	//messageClient(%client, '', "\c3/getChild\c2 NAME\c6 - Gives you a small child.");
	messageClient(%client, '', "\c0/killPet\c2 NAME \c6 - Deletes any given pet. (say \"ALL\" to delete all of them)");
	messageClient(%client, '', "\c3/stay\c2 NAME \c6 - Makes the pet stop following you.");
	messageClient(%client, '', "\c3/follow\c2 NAME \c6 - makes the pet start following you.");
	messageClient(%client, '', "\c3/listPets\c6 - Lists all your current pets.");
	messageClient(%client, '', "\c3/fetchPet\c2 NAME \c6 - transports your pet to your current location.");
	messageClient(%client, '', "\c3/control & /uncontrol\c2 NAME \c6 - Controls your pet, leave the name blank to control yourself.");
	//messageClient(%client, '', "\c3/resize \c2 NAME, X, Y, Z \c6 - Resizes your pet. (only factors between 0.3 and 0.7 work)");
	messageClient(%client, '', "\c7END OF HELP");
}


