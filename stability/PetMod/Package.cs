//+--
//Project: PetMod; package.cs
//Description: The functions overwritten in PetMod.
//Authors: Aoki
//+--

package PetMod
{
	function Player::ActivateStuff(%obj)
	{
		Parent::ActivateStuff(%obj);

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType;	
		%eye = %obj.getEyePoint();
		%eyeVec = vectorScale(%obj.getEyeVector(),5);
		%fPoint = vectorAdd(%eye,%eyeVec);

		%target = containerRayCast(%eye, %fPoint, %mask, %obj);
		
		//%name = %obj.client.name @ "'s Pet.";
		//%target.vehicle.SetShapeName(%name);
		
		if(%target.Owner !$= "")
		{
			%rm = getRandom(1, 5);

			switch(%rm)
			{
				case 1:
					%mood = "happy";
				case 2:
					%mood = "upbeat";
				case 3:
					%mood = "energetic";
				case 4:
					%mood = "awesome";
				case 5:
					%mood = "cool";
			}
			
			%obj.client.centerPrint("\c3"@%target.petName@"\c6 is feeling pretty \c4"@%mood@"\c6!", 1);
		}
	}

	function GameConnection::onDrop(%client, %dunno)
	{
		Parent::onDrop(%client, %dunno);

		for(%i = 1; %i <= %client.pets; %i++)
		{
			%client.pet[%i].delete();
			%client.pet[%i] = "";
		}
		%client.pets = 0;
	}
};

activatePackage(PetMod);