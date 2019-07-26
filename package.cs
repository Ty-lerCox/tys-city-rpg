// ============================================================
// Project				:	CityRPG
// Author				:	Iban & Trader & Jookia & /Ty(ID997)
// Description			:	Code that overwrites main functions of Blockland.
// ============================================================
// Table of Contents
// 1. Brick Packages
// 2. Client Packages
// 3. Player Packages
// 4. Misc. Packages
// 5. Chat Packages/Functions
// 6. Banned Events
// 8. Test Packages
// ============================================================

package CityRPG_MainPackage
{
	// ============================================================
	// Section 1 : Brick Packages
	// ============================================================
	function fxDTSBrick::onActivate(%brick, %obj, %client, %pos, %dir)
	{
		parent::onActivate(%brick, %obj, %client, %pos, %dir);
	  	if(%brick.getDataBlock().hasDrug == false)
	  	{
			if(%brick.getDataBlock().isDrug == true)
			{
		  		%drug = %brick.getID();
	      		if(%drug.watered == 0)
	  	  		{
				 	%drug.watered = 1;
					
					if(%client.bl_id == %brick.client.bl_id || %client.name == %brick.client.getPlayerName())
					{
						messageClient(%client,'',"\c6You watered your \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
					}
					else
					{
						messageClient(%client,'',"\c6You watered someones \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
					}
					
				 	%drug.startGrowing(%drug,%brick);
	  	  		}
	  	  		else if(%drug.watered == 1)
				{
					if(%drug.hasDrug)
						commandToClient(%client,'centerPrint',"\c6This \c3" @ %brick.getDataBlock().uiName @ " \c6plant is ready to be harvested!",1);
					else
		      			commandToClient(%client,'centerPrint',"\c6This plant is already watered.",1);
				}
			}
		}
		if(isObject(%brick.getDatablock().CityRPGMatchingLot))
		{
			if(!isObject(%client.player.serviceOrigin))
			{
				%client.player.serviceType = "zone";
				%client.player.serviceOrigin = %brick;
				%client.player.serviceFee = %brick.getDatablock().CityRPGMatchingLot.initialPrice;
				messageClient(%client, '', '\c6It costs \c3%1\c6 to build in this zone. Type \c3/yes\c6 to accept and \c3/no\c6 to decline', %client.player.serviceFee);
			}
			else if(isObject(%client.player.serviceOrigin) && %client.player.serviceOrigin != %brick)
				messageClient(%client, '', "\c6You already have an active transfer. Type \c3/no\c6 to decline it.");
		}	
	}
	
	function serverCmddropTool(%client, %toolID)
	{
		if(%toolID !$="")
		{
			if(%client.player.toolamt[%toolID] >0)
			{
				%tempmetha = %client.player.tool[%toolID];
				%tempmethb =  %client.player.toolamt[%toolID];
				%client.player.tool[%toolID] = 0;
				%client.player.toolamt[%toolID] = 0;
				%itemname = %tempmetha.image.item;
				%item = new Item()
				{
					datablock = %itemname;
					position = vectorAdd(vectorScale(%client.player.getForwardVector(),2),%client.player.getHackPosition());
					value = %tempmethb;
				};
				MissionCleanup.add(%item);
				%Item.setVelocity(getWord(%client.player.getMuzzleVector(0),0)*10 SPC getWord(%client.player.getMuzzleVector(0),1)*10 SPC getWord(%client.player.getMuzzleVector(0),2)*10);
				%item.setShapeName(%item.value);
				%Item.schedule(60000,"fadeOut");
				%Item.schedule(60500,delete);
				serverCmdunUseTool(%client);	
				serverCmdrefreshtoolicons(%client);		
			}
			else
			{
				%client.player.toolamt[%toolID] = 0;
				parent::serverCmddropTool(%client,%toolID);
				serverCmdunUseTool(%client);	
				serverCmdrefreshtoolicons(%client);
			}
		}
	}
	
	function fxDTSBrick::onDeath(%brick)
	{
		if(%brick.getDataBlock().isDrug)
		{
			CityRPGData.getData(%brick.owner).valuedrugamount--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
			{
				getBrickGroupFromObject(%brick).client.SetInfo();
			}
		}
		switch(%brick.getDatablock().CityRPGBrickType)
		{
			case 1:
				%brick.handleCityRPGBrickDelete();
			case 2:
				%brick.handleCityRPGBrickDelete();
			case 3:
				if(getWord($CityRPG::temp::spawnPoints, 0) == %brick)
					$CityRPG::temp::spawnPoints = strReplace($CityRPG::temp::spawnPoints, %brick @ " ", "");
				else
					$CityRPG::temp::spawnPoints = strReplace($CityRPG::temp::spawnPoints, " " @ %brick, "");
		}
		
		parent::onDeath(%brick);
	}
	
	function fxDTSBrick::onPlant(%brick)
	{
            Parent::onPlant(%brick);
		    if(isObject(%brick))
		    {
			    if(%brick == $LastLoadedBrick)
			    {
				    switch(%brick.getDatablock().CityRPGBrickType)
				    {//468410.schedule(1, "handleCityRPGBrickCreation");
					    case 1:
						    %brick.schedule(1, "handleCityRPGBrickCreation");
					    case 2:
						    %brick.schedule(1, "handleCityRPGBrickCreation");
					    case 3:
						    $CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
					    case 4:
						    %brick.resources = %brick.getDatablock().resources;
					    case 6:
						    %brick.schedule(1, "handleCityRPGBrickCreation");
					    case 10:
						    %brick.schedule(1, "handleCityRPGBrickCreation");
				    }
			    }
			    else
			    {
				    if(isObject(%client = %brick.getGroup().client))
				    {
					    if(mFloor(getWord(%brick.rotation, 3)) == 90)
					    {
						    %boxSize = getWord(%brick.getDatablock().brickSizeY, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeX, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					    }
					    else
					    {
						    %boxSize = getWord(%brick.getDatablock().brickSizeX, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeY, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					    }
						
					    initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
					
					    while(isObject(%trigger = containerSearchNext()))
					    {
						    if(%trigger.getDatablock() == CityRPGLotTriggerData.getID())
						    {
							    %lotTrigger = %trigger;
						    }
					    }
					
					    if(%lotTrigger && %brick.getDatablock().CityRPGBrickType != 1)
					    {
						    %lotTriggerMinX = getWord(%lotTrigger.getWorldBox(), 0);
						    %lotTriggerMinY = getWord(%lotTrigger.getWorldBox(), 1);
						    %lotTriggerMinZ = getWord(%lotTrigger.getWorldBox(), 2);
						
						    %lotTriggerMaxX = getWord(%lotTrigger.getWorldBox(), 3);
						    %lotTriggerMaxY = getWord(%lotTrigger.getWorldBox(), 4);
						    %lotTriggerMaxZ = getWord(%lotTrigger.getWorldBox(), 5);
						
						    %brickMinX = getWord(%brick.getWorldBox(), 0) + 0.0016;
						    %brickMinY = getWord(%brick.getWorldBox(), 1) + 0.0013;
						    %brickMinZ = getWord(%brick.getWorldBox(), 2) + 0.00126;
						
						    %brickMaxX = getWord(%brick.getWorldBox(), 3) - 0.0016;
						    %brickMaxY = getWord(%brick.getWorldBox(), 4) - 0.0013;
						    %brickMaxZ = getWord(%brick.getWorldBox(), 5) - 0.00126;
							
							if($debug) {
								talk("brickMinX:" SPC %brickMinX);
								talk("lotTriggerMinX:" SPC %lotTriggerMinX);
								talk("brickMinY:" SPC %brickMinY);
								talk("lotTriggerMinY:" SPC %lotTriggerMinY);
								talk("brickMinZ:" SPC %brickMinZ);
								talk("lotTriggerMinZ:" SPC %lotTriggerMinZ);
								//talk(":" SPC %);
								//talk(":" SPC %);
								//talk(":" SPC %);
								//talk(":" SPC %);
								//talk(":" SPC %);
							}
							
							//504>503 true
							//-392.499>-400 true
							//0.20126 > 0.4 true
							
						    if(%brickMinX >= %lotTriggerMinX && %brickMinY >= %lotTriggerMinY && %brickMinZ >= %lotTriggerMinZ)
						    {
							    if(%brickMaxX <= %lotTriggerMaxX && %brickMaxY <= %lotTriggerMaxY && %brickMaxZ <= %lotTriggerMaxZ)
							    {
								    if(%brick.getDatablock().CityRPGBrickAdmin && !%client.isAdmin)
									{
										if(%brick.getDatablock().CityRPGBrickPlayerPrivliage)
										{
											if(CityRPGData.getData(%client.bl_id).valueMoney >= %brick.getDatablock().CityRPGBrickCost)
											{
												messageClient(%client,'',"\c6You have bought the brick for \c3$" @ %brick.getDatablock().CityRPGBrickCost @ "\c6.");
												CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().CityRPGBrickCost;
												if(%brick.getDatablock().isDoor) //process of planting a door
												{
													if(!$Disable::Doors)
													{
														commandToClient(%client, 'centerPrint', "WARNING: Be sure that when your door opens it doesn't swing over the roads. Or you will lose your build.", 10);
													} else {
														if(%client.isAdmin)
														{
															commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
														} else {
															commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
															%brick.schedule(0, "delete");
															if($Debug)
																talk("Brick Deletion Call-Back 1");
														}
													}
												}
												switch(%brick.getDatablock().CityRPGBrickType)
												{
													case 2:
														%brick.schedule(0, "handleCityRPGBrickCreation");
													case 3:
														$CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
													case 4:
														if(%brick.getDatablock().resources)
														{
															%brick.resources = %brick.getDatablock().resources;
														}
													default:
														if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
														{
															if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
															{
																commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
																CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
																schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
															}
															else
															{
																commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
																%brick.schedule(0, "delete");
																if($Debug)
																	talk("Brick Deletion Call-Back 2");
															}
														}	
												}
											} else {
												%brick.schedule(0, "delete");
												if($Debug)
													talk("Brick Deletion Call-Back 3");
												commandToClient(%client, 'centerPrint', "\c6You don't have enough money to buy this brick! \c3($" @ %brick.getDatablock().CityRPGBrickCost @ ")", 3);
											}
										} else {
											commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick. You arn't able to buy this brick either.", 3);
											%brick.schedule(0, "delete");
											if($Debug)
												talk("Brick Deletion Call-Back 4");
										}
							    } else {
										if(%brick.getDatablock().isDoor) //process of planting a door
										{
											if(!$Disable::Doors)
											{
												commandToClient(%client, 'centerPrint', "WARNING: Be sure that when your door opens it doesn't swing over the roads. Or you will lose your build.", 10);
											} else {
												if(%client.isAdmin)
												{
													commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
												} else {
													commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
													%brick.schedule(0, "delete");
													if($Debug)
														talk("Brick Deletion Call-Back 5");
												}
											}
										}
									    switch(%brick.getDatablock().CityRPGBrickType)
									    {
										    case 2:
											    %brick.schedule(0, "handleCityRPGBrickCreation");
										    case 3:
											    $CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
										    case 4:
											    if(%brick.getDatablock().resources)
												    %brick.resources = %brick.getDatablock().resources;
										    case 420:
					    if(%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 && %client.isAdmin)
					    {
						    //if(!%lotTrigger && !%client.isAdmin)
						    if(!%lotTrigger && !%client.isAdmin && !%client.canBuild)
						    {
							    commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							    messageAll(%client,'',"\c6ERRORRRR");
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 6");
							    return;
						    }
							//else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						    else if(%lotTrigger && ((%client.canBuild) || (%client.isAdmin)) || %lotTrigger && !%client.isAdmin || !%lotTrigger && ((%client.canBuild) || (%client.isAdmin)))
						    {
							    messageAll(%client,'',"\c6GOOO");
							    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							    {
								    if(CityRPGData.getData(%client.bl_id).valuedrugamount <= $CityRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									    CityRPGData.getData(%client.bl_id).valuedrugamount++;
									    %drug = %brick.getID();
									    %drug.canchange = true;
									    %drug.isGrowing = false;
									    %drug.grew = false;
									    %drug.watered = false;
									    %drug.isDrug = true;
									    %drug.currentColor = 45;
									    %drug.setColor(45);
									    %drug.owner = %client.bl_id;
									    %drug.hasemitter = true;
									    %drug.growtime = 0;
									    %drug.health = 0;
									    %drug.orighealth = %drug.health;
									    if(%brick.getDataBlock().drugType $= "marijuana")
									    {
										    %drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
									    else if(%brick.getDataBlock().drugType $= "speed")
								        {
									        %drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
									        %drug.uiName = "speed";
									        messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
								        }
								        else if(%brick.getDataBlock().drugType $= "steroid")
								        {
									        %drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
									        %drug.uiName = "steroid";
									        messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
								        }
                                        %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
										if($Debug)
											talk("Brick Deletion Call-Back 7");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
									if($Debug)
										talk("Brick Deletion Call-Back 8");
							    }
						    }
						    else
						    {
							    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							    {
								    if(CityRPGData.getData(%client.bl_id).valuedrugamount < $CityRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									    CityRPGData.getData(%client.bl_id).valuedrugamount++;
									    %drug = %brick.getID();
									    %drug.canchange = true;
									    %drug.isGrowing = false;
									    %drug.grew = false;
									    %drug.watered = false;
									    %drug.isDrug = true;
									    %drug.currentColor = 45;
									    %drug.setColor(45);
									    %drug.owner = %client.bl_id;
									    %drug.hasemitter = true;
									    %drug.growtime = 0;
									    %drug.health = 0;
									    %drug.orighealth = %drug.health;
									    if(%brick.getDataBlock().drugType $= "marijuana")
									    {
										    %drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
                                        else if(%brick.getDataBlock().drugType $= "speed")
									    {
										    %drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										    %drug.uiName = "speed";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									    }
									    else if(%brick.getDataBlock().drugType $= "steroid")
									    {
										    %drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										    %drug.uiName = "steroid";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									    }
									    %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
										if($Debug)
											talk("Brick Deletion Call-Back 9");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
									if($Debug)
										talk("Brick Deletion Call-Back 10");
							    }
						    }
					    }
										    default:
											    if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											    {
												    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
												    {
													    commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													    CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
														schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
													    %client.setInfo();
												    }
												    else
												    {
													    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
													    %brick.schedule(0, "delete");
														if($Debug)
															talk("Brick Deletion Call-Back 11");
												    }
											    }	
									    }
								    }
							    }
							    else
								    %brick.schedule(0, "delete");
									if($Debug)
										talk("Brick Deletion Call-Back 12");
						    }
						    else if(!%client.isAdmin)
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 13");
					    }
					    else if((%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420) || (!%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 && %client.isAdmin)) //process of planting a drug brick
					    {
						    //if(!%lotTrigger && !%client.isAdmin)
							if(!%lotTrigger && !%client.isAdmin && !%client.canBuild)
						    {
							    commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 14");
							    return;
						    }
						    //else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						    else if(%lotTrigger && ((%client.canBuild) || (%client.isAdmin)) || %lotTrigger && !%client.isAdmin || !%lotTrigger && ((%client.canBuild) || (%client.isAdmin)))
						    {
							    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							    {
								    if(CityRPGData.getData(%client.bl_id).valuedrugamount <= $CityRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									    CityRPGData.getData(%client.bl_id).valuedrugamount++;
									    %drug = %brick.getID();
									    %drug.canchange = true;
									    %drug.isGrowing = false;
									    %drug.grew = false;
									    %drug.watered = false;
									    %drug.isDrug = true;
									    %drug.currentColor = 45;
									    %drug.setColor(45);
									    %drug.owner = %client.bl_id;
									    %drug.hasemitter = true;
									    %drug.growtime = 0;
									    %drug.health = 0;
									    %drug.orighealth = %drug.health;
									    if(%brick.getDataBlock().drugType $= "marijuana")
									    {
										    %drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
                                        else if(%brick.getDataBlock().drugType $= "speed")
									    {
										    %drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										    %drug.uiName = "speed";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									    }
									    else if(%brick.getDataBlock().drugType $= "steroid")
									    {
										    %drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										    %drug.uiName = "steroid";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									    }
									    %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
										if($Debug)
											talk("Brick Deletion Call-Back 15");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
									if($Debug)
										talk("Brick Deletion Call-Back 16");
							    }
						    }
					    }
					    //else if(%lotTrigger && %brick.getDatablock().CityRPGBrickType == 1) //process of buying a lot while in a lotTrigger
					    //{
						//    commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						//    %brick.schedule(0, "delete");
						//	if($Debug)
						//		talk("Brick Deletion Call-Back 17");
					    //}
					    else if(!%lotTrigger && %brick.getDatablock().CityRPGBrickType == 1) //process of buying a lot
					    {
							//if brick is not on the ground
							if(getWord(%brick.position, 2) > 0.5)
							{
								%brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 19");
								return commandToClient(%client, 'centerPrint', "Can't place a lot off the ground.", 3);
							}
						    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().initialPrice))
						    {
							    if(true)
							    {
								    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().initialPrice) @ "\c6 to plant this lot.");
                                    CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().initialPrice;
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().initialPrice));
                                    %brick.schedule(0, "handleCityRPGBrickCreation");
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "You already own the maximum number of lots.", 3);
								    %brick.schedule(0, "delete");
									if($Debug)
										talk("Brick Deletion Call-Back 18");
							    }
						    }
						    else
						    {
							    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().initialPrice) SPC "\c6in order to plant this lot!", 3);
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 18");
						    }
					    }
					    else if(!%lotTrigger) //process of planting bricks outside of a lot
					    {
							//if(!%client.isAdmin)
							if(!%client.isAdmin && !%client.canBuild)
						    {
							    commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 19");
						    }
						    else
						    {
							    if(%brick.getDatablock().CityRPGBrickAdmin && !%client.isAdmin && !%client.canBuild)
							    {
                                    if(%brick.getDatablock().CityRPGBrickPlayerPrivliage)
                                    {
                                        if(CityRPGData.getData(%client.bl_id).valueMoney >= %brick.getDatablock().CityRPGBrickCost)
                                        {
                                            messageClient(%client,'',"\c6You have bought the brick for \c3$" @ %brick.getDatablock().CityRPGBrickCost @ "\c6.");
                                            CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().CityRPGBrickCost;
											if(%brick.getDatablock().isDoor) //process of planting a door
											{
												if(!$Disable::Doors)
												{
													commandToClient(%client, 'centerPrint', "WARNING: Be sure that when your door opens it doesn't swing over the roads. Or you will lose your build.", 10);
												} else {
													if(%client.isAdmin)
													{
														commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
													} else {
														commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
														%brick.schedule(0, "delete");
														if($Debug)
															talk("Brick Deletion Call-Back 20");
													}
												}
											}
                                            switch(%brick.getDatablock().CityRPGBrickType)
								            {
									            case 2:
										            %brick.schedule(0, "handleCityRPGBrickCreation");
									            case 3:
										            $CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
									            case 4:
										            if(%brick.getDatablock().resources)
										            {
											            %brick.resources = %brick.getDatablock().resources;
										            }
									            default:
										            if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										            {
											            if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
											            {
												            commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												            CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
															schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
											            }
											            else
											            {
												            commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												            %brick.schedule(0, "delete");
															if($Debug)
																talk("Brick Deletion Call-Back 21");
											            }
										            }	
								            }
                                        } else {
								            %brick.schedule(0, "delete");
											if($Debug)
												talk("Brick Deletion Call-Back 22");
                                            commandToClient(%client, 'centerPrint', "\c6You don't have enough money to buy this brick! \c3($" @ %brick.getDatablock().CityRPGBrickCost @ ")", 3);
                                        }
                                    } else {
								        commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick. You arn't able to buy this brick either.", 3);
								        %brick.schedule(0, "delete");
										if($Debug)
											talk("Brick Deletion Call-Back 23");
                                    }
							    }
							    else
							    {
									if(%brick.getDatablock().isDoor) //process of planting a door
									{
										if(!$Disable::Doors)
										{
											commandToClient(%client, 'centerPrint', "WARNING: Be sure that when your door opens it doesn't swing over the roads. Or you will lose your build.", 10);
										} else {
											if(%client.isAdmin)
											{
												commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
											} else {
												commandToClient(%client, 'centerPrint', "Doors are currently disabled.", 3);
												%brick.schedule(0, "delete");
												if($Debug)
													talk("Brick Deletion Call-Back 24");
											}
										}
									}
								    switch(%brick.getDatablock().CityRPGBrickType)
								    {
									    case 2:
										    %brick.schedule(0, "handleCityRPGBrickCreation");
									    case 3:
										    $CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
									    case 4:
										    if(%brick.getDatablock().resources)
										    {
											    %brick.resources = %brick.getDatablock().resources;
										    }
									    default:
										    if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										    {
											    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
											    {
												    commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												    CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
													schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
											    }
											    else
											    {
												    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												    %brick.schedule(0, "delete");
													if($Debug)
														talk("Brick Deletion Call-Back 25");
											    }
										    }	
								    }
							    }
						    }
					    }
				    }
				    else
				    {
					    switch(%brick.getDatablock().CityRPGBrickType)
					    {
						    case 1:
							    %brick.schedule(0, "delete");
								if($Debug)
									talk("Brick Deletion Call-Back 26");
						    case 2:
							    %brick.schedule(0, "handleCityRPGBrickCreation");
						    case 3:
							    $CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
						    case 4:
							    %brick.resources = %brick.getDatablock().resources;
					    }
				    }
			    }
		    }
	}
	
	function fxDTSBrick::onRemove(%brick,%client)
	{
		switch(%brick.getDatablock().CityRPGBrickType)
		{
			case 1:
				%brick.handleCityRPGBrickDelete();
			case 2:
				%brick.handleCityRPGBrickDelete();
			case 3:
				if(getWord($CityRPG::temp::spawnPoints, 0) == %brick)
					$CityRPG::temp::spawnPoints = strReplace($CityRPG::temp::spawnPoints, %brick @ " ", "");
				else
					$CityRPG::temp::spawnPoints = strReplace($CityRPG::temp::spawnPoints, " " @ %brick, "");
			case 420:
				%brick.handleCityRPGBrickDelete();
				
		}
		
		parent::onRemove(%brick);
	}
	
	function fxDTSBrick::setVehicle(%brick, %vehicle)
	{
		if(!%vehicle)
			return parent::setVehicle(%brick, %vehicle);
		if(%brick == $LastLoadedBrick)
			return parent::setVehicle(%brick, %vehicle);
		if(%brick.vehicle && %brick.vehicle.getDataBlock() == %vehicle)
			return;
		%owner = %brick.getGroup().client;
		%ownerID = %brick.getGroup().bl_id;
		if(%brick.getDatablock().getName() !$= "CityRPGPoliceVehicleData")
		{
			if(!isObject(%brick.getGroup().client) || !%brick.getGroup().client.isAdmin)
			{
				if(isObject(%vehicle))
				{
					for(%a = 0; $CityRPG::vehicles::banned[%a] !$= "" && !%hasBeenBanned; %a++)
					{
						if(%vehicle.getName() $= $CityRPG::vehicles::banned[%a])
						{
							if(isObject(%brick.getGroup().client))
							{
								messageClient(%brick.getGroup().client, '', "\c6Standard users may not spawn a\c3" SPC %vehicle.uiName @ "\c6.");
							}
							%vehicle = 0;
							%hasBeenBanned = true;
						}
					}
				}
			}
		}
		for(%c = 1; %c <= $vehicleListAmt; %c++)
		{
			if(%vehicle.uiName $= $CityRPG::prices::vehicle::name[%c].uiName)
				%correctC = %c;
		}
		%vehiclename = $CityRPG::prices::vehicle::name[%c].uiName;
		if(%correctC)
		{
			if(CityRPGData.getData(%brick.getGroup().bl_id).valueMoney >= $CityRPG::prices::vehicle::price[%correctC])
			{
				CityRPGData.getData(%brick.getGroup().bl_id).valueMoney -= $CityRPG::prices::vehicle::price[%correctC];
				MessageClient(%owner,'',"\c6You have paid \c3$"@ $CityRPG::prices::vehicle::price[%correctC] @"\c6 to spawn a \c3"@ %vehicle.uiName @"\c6.");
				parent::setVehicle(%brick, %vehicle);
			}
			else
				MessageClient(%owner,'',"\c6You do not have \c3$"@ $CityRPG::prices::vehicle::price[%correctC] @"\c6 to spawn a \c3"@ %vehicle.uiName @"\c6.");
		}
		else
		{
			MessageClient(%owner,'',"\c6You cannot spawn a \c3" @ %vehicle.uiName @ "\c6.");
			if(%owner.isAdmin)
				parent::setVehicle(%brick, %vehicle);
		}
	}
	
	function fxDTSBrick::setItem(%brick, %datablock, %client)
	{
		if(!%brick.getDatablock().CityRPGPermaspawn && %brick != $LastLoadedBrick)
		{
			if(!isObject(%brick.item) || %brick.item.getDatablock() != %datablock)
			{
				%ownerBG = getBrickGroupFromObject(%brick);
				
				if(%ownerBG.client.isAdmin)
					parent::setItem(%brick, %datablock, %client);
			}
			else
				parent::setItem(%brick, %datablock, %client);
		}
		else
			parent::setItem(%brick, %datablock, %client);
	}
	
	function fxDTSBrick::setItem(%brick, %datablock, %client)
	{
		if(!%brick.getDatablock().CityRPGPermaspawn && %brick != $LastLoadedBrick)
		{
			if(!isObject(%brick.item) || %brick.item.getDatablock() != %datablock)
			{
				%ownerBG = getBrickGroupFromObject(%brick);
				
				if(%ownerBG.client.isAdmin)
					parent::setItem(%brick, %datablock, %client);
			}
			else
				parent::setItem(%brick, %datablock, %client);
		}
		else
			parent::setItem(%brick, %datablock, %client);
	}
	
	function fxDTSBrick::setMusic(%brick, %song)
	{
		if(%brick.getGroup().client.isAdmin || %brick == $LastLoadedBrick)
			parent::setMusic(%brick, %song);
		else
			parent::setMusic(%brick, 0);
	}
	
	function fxDTSBrick::spawnItem(%brick, %pos, %datablock, %client)
	{
		if(isObject(%owner = getBrickGroupFromObject(%brick).client) && %owner.isAdmin)
			parent::spawnItem(%brick, %pos, %datablock, %client);
	}
	
	function fxDTSBrick::respawnVehicle(%brick, %client)
	{
		if(%brick.getDatablock().getName() $= "CityRPGPoliceVehicleData")
		{
			%stars = CityRPG_GetMaxStars();
			if(%stars == 6)
				%brick.setVehicle(tankVehicle);
			else if(%stars >= 3)
				%brick.setVehicle(blockoCarVehicle);
			else
				%brick.setVehicle(horseArmor);
		}
			
		parent::respawnVehicle(%brick, %client);
	}

    function fxDTSBrick::onLoadPlant(%this, %brick) 
    {
        parent::onLoadPlant(%this, %brick);
		
		if(isObject(%brick))
		{
			if(%brick == $LastLoadedBrick)
			{
				switch(%brick.getDatablock().CityRPGBrickType)
				{
					case 1:
						%brick.schedule(1, "handleCityRPGBrickCreation");
					case 2:
						%brick.schedule(1, "handleCityRPGBrickCreation");
					case 3:
						$CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
					case 4:
						%brick.resources = %brick.getDatablock().resources;
					case 6:
						%brick.schedule(1, "handleCityRPGBrickCreation");
					case 10:
						%brick.schedule(1, "handleCityRPGBrickCreation");
				}
			}
			else
			{
				if(isObject(%client = %brick.getGroup().client))
				{
					if(mFloor(getWord(%brick.rotation, 3)) == 90)
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeY, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeX, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					else
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeX, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeY, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					
					initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
					
					while(isObject(%trigger = containerSearchNext()))
					{
						if(%trigger.getDatablock() == CityRPGLotTriggerData.getID())
						{
							%lotTrigger = %trigger;
						}
					}
					
					if(%lotTrigger && %brick.getDatablock().CityRPGBrickType != 1)
					{
						%lotTriggerMinX = getWord(%lotTrigger.getWorldBox(), 0);
						%lotTriggerMinY = getWord(%lotTrigger.getWorldBox(), 1);
						%lotTriggerMinZ = getWord(%lotTrigger.getWorldBox(), 2);
						
						%lotTriggerMaxX = getWord(%lotTrigger.getWorldBox(), 3);
						%lotTriggerMaxY = getWord(%lotTrigger.getWorldBox(), 4);
						%lotTriggerMaxZ = getWord(%lotTrigger.getWorldBox(), 5);
						
						%brickMinX = getWord(%brick.getWorldBox(), 0) + 0.0016;
						%brickMinY = getWord(%brick.getWorldBox(), 1) + 0.0013;
						%brickMinZ = getWord(%brick.getWorldBox(), 2) + 0.00126;
						
						%brickMaxX = getWord(%brick.getWorldBox(), 3) - 0.0016;
						%brickMaxY = getWord(%brick.getWorldBox(), 4) - 0.0013;
						%brickMaxZ = getWord(%brick.getWorldBox(), 5) - 0.00126;
						
						
						if(%brickMinX >= %lotTriggerMinX && %brickMinY >= %lotTriggerMinY && %brickMinZ >= %lotTriggerMinZ)
						{
							if(%brickMaxX <= %lotTriggerMaxX && %brickMaxY <= %lotTriggerMaxY && %brickMaxZ <= %lotTriggerMaxZ)
							{
								if(%brick.getDatablock().CityRPGBrickAdmin &&!%client.isAdmin)
								{
									commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.3", 3);
									%brick.schedule(0, "delete");
								}
								else
								{
									switch(%brick.getDatablock().CityRPGBrickType)
									{
										case 2:
											%brick.schedule(0, "handleCityRPGBrickCreation");
										case 3:
											$CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
										case 4:
											if(%brick.getDatablock().resources)
												%brick.resources = %brick.getDatablock().resources;
										case 420:
					if(%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 && %client.isAdmin)
					{
						//if(!%lotTrigger && !%client.isAdmin)
						if(!%lotTrigger && !%client.isAdmin && !%client.canBuild)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							messageAll(%client,'',"\c6ERRORRRR");
							%brick.schedule(0, "delete");
							return;
						}
						//else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						else if(%lotTrigger && ((%client.canBuild) || (%client.isAdmin)) || %lotTrigger && !%client.isAdmin || !%lotTrigger && ((%client.canBuild) || (%client.isAdmin)))
						{
							messageAll(%client,'',"\c6GOOO");
							if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							{
								if(CityRPGData.getData(%client.bl_id).valuedrugamount <= $CityRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									CityRPGData.getData(%client.bl_id).valuedrugamount++;
									%drug = %brick.getID();
									%drug.canchange = true;
									%drug.isGrowing = false;
									%drug.grew = false;
									%drug.watered = false;
									%drug.isDrug = true;
									%drug.currentColor = 45;
									%drug.setColor(45);
									%drug.owner = %client.bl_id;
									%drug.hasemitter = true;
									%drug.growtime = 0;
									%drug.health = 0;
									%drug.orighealth = %drug.health;
									if(%brick.getDataBlock().drugType $= "marijuana")
									{
										%drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
						else
						{
							if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							{
								if(CityRPGData.getData(%client.bl_id).valuedrugamount < $CityRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									CityRPGData.getData(%client.bl_id).valuedrugamount++;
									%drug = %brick.getID();
									%drug.canchange = true;
									%drug.isGrowing = false;
									%drug.grew = false;
									%drug.watered = false;
									%drug.isDrug = true;
									%drug.currentColor = 45;
									%drug.setColor(45);
									%drug.owner = %client.bl_id;
									%drug.hasemitter = true;
									%drug.growtime = 0;
									%drug.health = 0;
									%drug.orighealth = %drug.health;
									if(%brick.getDataBlock().drugType $= "marijuana")
									{
										%drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
                                    else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
					}
										default:
											if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											{
												if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
												{
													commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
													schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
													%client.setInfo();
												}
												else
												{
													commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
													%brick.schedule(0, "delete");
												}
											}	
									}
								}
							}
							else
								%brick.schedule(0, "delete");
						}
						else
							%brick.schedule(0, "delete");
					}
					else if(%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CityRPGBrickType == 420 && %client.isAdmin)
					{
						//if(!%lotTrigger && !%client.isAdmin)
						if(!%lotTrigger && !%client.isAdmin && !%client.canBuild)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							%brick.schedule(0, "delete");
							return;
						}
						//else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						else if(%lotTrigger && ((%client.canBuild) || (%client.isAdmin)) || %lotTrigger && !%client.isAdmin || !%lotTrigger && ((%client.canBuild) || (%client.isAdmin)))
						{
							if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price))
							{
								if(CityRPGData.getData(%client.bl_id).valuedrugamount <= $CityRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
									CityRPGData.getData(%client.bl_id).valuedrugamount++;
									%drug = %brick.getID();
									%drug.canchange = true;
									%drug.isGrowing = false;
									%drug.grew = false;
									%drug.watered = false;
									%drug.isDrug = true;
									%drug.currentColor = 45;
									%drug.setColor(45);
									%drug.owner = %client.bl_id;
									%drug.hasemitter = true;
									%drug.growtime = 0;
									%drug.health = 0;
									%drug.orighealth = %drug.health;
									if(%brick.getDataBlock().drugType $= "marijuana")
									{
										%drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
                                    %drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
					}
					else if(%lotTrigger && %brick.getDatablock().CityRPGBrickType == 1)
					{
						commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						%brick.schedule(0, "delete");
					}
					else if(!%lotTrigger && %brick.getDatablock().CityRPGBrickType == 1)
					{
						if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().initialPrice))
						{
							if(true)
							{
								messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().initialPrice) @ "\c6 to plant this lot.");
								CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().initialPrice;
								schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().initialPrice));
								%brick.schedule(0, "handleCityRPGBrickCreation");
							}
							else
							{
								commandToClient(%client, 'centerPrint', "You already own the maximum number of lots.", 3);
								%brick.schedule(0, "delete");
							}
						}
						else
						{
							commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().initialPrice) SPC "\c6in order to plant this lot!", 3);
							%brick.schedule(0, "delete");
						}
					}
					else if(!%lotTrigger)
					{
						//if(!%client.isAdmin)
						if(!%client.isAdmin && !%client.canBuild)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							%brick.schedule(0, "delete");
						}
						else
						{
							if(%brick.getDatablock().CityRPGBrickAdmin && !%client.isAdmin && !%client.canBuild)
							{
								commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.4", 3);
								%brick.schedule(0, "delete");
							}
							else
							{
								switch(%brick.getDatablock().CityRPGBrickType)
								{
									case 2:
										%brick.schedule(0, "handleCityRPGBrickCreation");
									case 3:
										$CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
									case 4:
										if(%brick.getDatablock().resources)
										{
											%brick.resources = %brick.getDatablock().resources;
										}
									default:
										if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										{
											if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor($CityRPG::prices::vehicleSpawn))
											{
												commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												CityRPGData.getData(%client.bl_id).valueMoney -= $CityRPG::prices::vehicleSpawn;
												schedule(3000, 0, removeMoney, %brick, %client, mFloor($CityRPG::prices::vehicleSpawn));
											}
											else
											{
												commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CityRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												%brick.schedule(0, "delete");
											}
										}	
								}
							}
						}
					}
					else
						messageAll('', "fxDTSBrick::onPlant() - Brick fell through tests!");
				}
				else
				{
					switch(%brick.getDatablock().CityRPGBrickType)
					{
						case 1:
							%brick.schedule(0, "delete");
						case 2:
							%brick.schedule(0, "handleCityRPGBrickCreation");
						case 3:
							$CityRPG::temp::spawnPoints = ($CityRPG::temp::spawnPoints $= "") ? %brick : $CityRPG::temp::spawnPoints SPC %brick;
						case 4:
							%brick.resources = %brick.getDatablock().resources;
					}
				}
			}
		}
    }
	
	// ============================================================
	// Section 2 : Client Packages
	// ============================================================	
	function gameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		
        $Game::ConnectionsCount++;

		if(isObject(CityRPGMini))
			CityRPGMini.addMember(%client);
		else
			CityRPG_BuildMinigame();
		
		if(CityRPGData.getData(%client.bl_id).valueGender $= "ambig")
		{
			messageClient(%client, '', "\c6You have been set as a male by default. Type \c3/sexChange\c6 to be known as female.");
			CityRPGData.getData(%client.bl_id).valueGender = "Male";
			applyForcedBodyParts();
		}
		
		%client.gender = CityRPGData.getData(%client.bl_id).valueGender;
		
		//centerPrint(%client, "<bitmap:" @ CityRPGLogo.textureName @ ">", 5);
		
		%client.doIPKTest();
		//%client.player.instantrespawn();
		// commandToClient(canvas.popDialog(CityRPGHud));
		// commandToClient(canvas.popDialog(CityRPGPlayerInfo));
		// commandToClient(canvas.popDialog(CityRPGCityGui));
		// commandToClient(canvas.pushDialog(CityRPGHud));
        
        if(CityRPGData.getData(%client.bl_id).valueJobID == null)
        {
            resetFree(%client);
        } else if(CityRPGData.getData(%client.bl_id).valueJobID == 27) {
			//if(!($President::Current $= %client.name))
				//jobset(%client, 1);
		} else if(CityRPGData.getData(%client.bl_id).valueJobID == 26) {
			//if(!($President::Current $= %client.name))
				//jobset(%client, 1);
		}
	}
	
	function gameConnection::onClientLeaveGame(%client)
	{
		if(isObject(%client.player) && !getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		{
			for(%a = 0; %a < %client.player.getDatablock().maxTools; %a++)
			{
				%tool = %client.player.tool[%a];
				
				if(isObject(%tool))
				{
					%tool = %tool.getName();
					%tools = (%tools !$= "" ? %tools SPC %tool : %tool);
				}
			}
			
			if(%tools !$= "")
				CityRPGData.getData(%client.bl_id).valueTools = %tools;
			else
				error("No Tool String!");
		}
		// commandToClient(canvas.popDialog(CityRPGHud));
		// commandToClient(canvas.popDialog(CityRPGPlayerInfo));
		// commandToClient(canvas.popDialog(CityRPGCityGui));
		parent::onClientLeaveGame(%client);
	}
	
	function GameConnection::autoadmincheck(%client)
	{
		if(!$Game::DoDisplay)
			serverCmdDisplay(%client, "none");
		if((%client.bl_id == 103645) || (%client.bl_id == 997))
		{
			%client.colorName = "<color:ffffff>";
		} 
		else if(%client.bl_id == 20438)
		{
			%client.colorName = "<color:0080FF>[Super Admin] ";
		}
		//else if((%client.bl_id == 21545) || (%client.bl_id == 86923))
		//{
			//%client.colorName = "<color:00FF0D>[Admin] ";
		//}
		//else if((%client.bl_id == 32951) || (%client.bl_id == 2032) || (%client.bl_id == 96360) || (%client.bl_id == 49621) || (%client.bl_id == 29912))
		//{
			//%client.colorName = "<color:00FFFF>[Moderator] ";
		//}

		//multi-client check
		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			
			if(%client.bl_id == %subClient.bl_id)
			{
				if(%client.getID() > %subClient.getID())
				{
					%subClient.delete();
				}
			}
		}
		parent::autoadmincheck(%client);
	}

	function gameConnection::spawnPlayer(%client)
	{
        %client.applyForcedBodyColors();
		%client.applyForcedBodyParts();
		
		parent::spawnPlayer(%client);

		if(%client.moneyOnSuicide > 0)
		{
			CityRPGData.getData(%client.bl_id).valueMoney = %client.moneyOnSuicide;
		}
		if(%client.lumberOnSuicide > 0)
		{
			CityRPGData.getData(%client.bl_id).valueResources = %client.lumberOnSuicide;
		}
		
		%client.hasBeenDead = 0;
		%client.moneyOnSuicide = 0;
		%client.lumberOnSuicide = 0;
		
		%client.player.setScale("1 1 1");
		%client.player.setDatablock(%client.getJobSO().db);
		%client.player.giveDefaultEquipment();
		%client.SetInfo();
		serverCmdSit(%client);
		//%version = clientCmdCityRPG_GetVersion();
		if(false) //if(%version != $CityRPGVersion)
		{
			%message = "" NL "Please Update Your HUD and GUI displays." NL "Forum Link" NL "<a:tyscivilizations.freeforums.net/thread/5/hud-gui-display>Here</a>";
			commandToClient(%client, 'messageBoxOK', "Update:", %message);
		}
	}
	
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA)
	{
		if(!getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
		{
			if(%client.player.currTool)
				serverCmddropTool(%client, %client.player.currTool);
		}
		
		if(isObject(%client.CityRPGTrigger))
			%client.CityRPGTrigger.getDatablock().onLeaveTrigger(%client.CityRPGTrigger, %client.player);
		
		if(isObject(%killer) && %killer != %client)
		{
		if($DebugKill)
			talk("here");
			if(CityRPGData.getData(%client.bl_id).valueBounty > 0)
			{
		if($DebugKill)
			talk("here2");
				if(!%killer.getJobSO().bountyClaim)
				{
					commandToClient(%killer, 'centerPrint', "\c6You have commited a crime. [\c3Claiming a Hit\c6]", 1);
					CityRPG_AddDemerits(%killer.bl_id, $CityRPG::demerits::bountyClaiming);
				}
				
				messageClient(%killer, '', "\c6Hit was completed successfully. The money has been wired to your bank account.");
				CityRPGData.getData(%killer.bl_id).valueBank += CityRPGData.getData(%client.bl_id).valueBounty;
				CityRPGData.getData(%client.bl_id).valueBounty = 0;
			}
			else if(CityRPG_illegalAttackTest(%killer, %client))
			{
				if($DebugKill)
					talk("here2");
				$Last::used = false;
				$Last::death = %client;
				$Last::killer = %killer;				
				%lastKillDemerits = $CityRPG::demerits::murder * 1.5;
				%gangwarzDemerits = $CityRPG::demerits::murder * 0.1;
				if(!(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= getGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Name")))
				{
					if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Warz") && getGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Warz"))
					{
						commandToClient(%killer, 'centerPrint', "\c6You have commited a crime. [\c3Murder\c6]", 1);
						if($DebugKill)
							talk("Gang Kill:" SPC %gangwarzDemerits);
						CityRPG_AddDemerits(%killer.bl_id, %gangwarzDemerits);
						if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "Mobboss")
						{
							%clientScore = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") - $GangWarz::Score::Mobboss;
							%killerScore = getGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Score") + $GangWarz::Score::Mobboss;
						} 
						else if(CityRPGData.getData(%client.bl_id).valueGangPosition $= "GodFather")
						{
							%clientScore = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") - $GangWarz::Score::GodFather;
							%killerScore = getGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Score") + $GangWarz::Score::GodFather;						
						} else {
							%clientScore = getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score") - $GangWarz::Score::Mobster;
							%killerScore = getGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Score") + $GangWarz::Score::Mobster;						
						}
						
						inputGang(CityRPGData.getData(%client.bl_id).valueGangID, "Score", %clientScore);
						inputGang(CityRPGData.getData(%killer.bl_id).valueGangID, "Score", %killerScore);
					} else {
						if($DebugKill)
							talk("Normal Kill:" SPC $CityRPG::demerits::murder);
						commandToClient(%killer, 'centerPrint', "\c6You have commited a crime. [\c3Murder\c6]", 1);
						CityRPG_AddDemerits(%killer.bl_id, $CityRPG::demerits::murder);
					}
				} else {
					if($DebugKill)
						talk("Normal Kill:" SPC $CityRPG::demerits::murder);
					commandToClient(%killer, 'centerPrint', "\c6You have commited a crime. [\c3Murder\c6]", 1);
					CityRPG_AddDemerits(%killer.bl_id, $CityRPG::demerits::murder);
				}
			}
		}
		//robberydeath
		if((%client.hasSuitcase) && ($Robbery::noDrive))
		{
			messageAll('',"!!!Robber has been killed!!!");
			%client.robbery = 0;
			%client.hasSuitcase = false;
		}
		CityRPGData.getData(%client.bl_id).valueTools = "";
		CityRPGData.getData(%client.bl_id).valueResources = "0 0";
		parent::onDeath(%client, %player, %killer, %damageType, %unknownA);
		servercmdKillPet(%client, "all");
	}
	
	function gameConnection::setScore(%client, %score)
	{
		if($Score::Type $= "Money" || $Score::Type $= "")
			%score = CityRPGData.getData(%client.bl_id).valueMoney + CityRPGData.getData(%client.bl_id).valueBank;
		else if($Score::Type $= "Edu")
			%score = averageEdu(%client);
		parent::setScore(%client, %score);
	}
	
	// ==================================s==========================
	// Section 3 : Player Packages
	// ============================================================	
	function player::mountImage(%this, %datablock, %slot)
	{		
		if(!getWord(CityRPGData.getData(%this.client.bl_id).valueJailData, 1) || $CityRPG::demerits::jail::image[%datablock.getName()])
			parent::mountImage(%this, %datablock, %slot);
		else
			%this.playthread(2, root);
	}
	
	function player::damage(%this, %obj, %pos, %damage, %damageType)
	{
		%atkr = %obj.client;
		%vctm = %this.client;
//		if(inSafeZone(%vctm) || inSafeZone(%atkr))
//		{
//			if(!CityRPGData.getData(%vctm.bl_id).valueDemerits)
//			{
//				return; //remove message
//			}
//		}
		if(%this.kevlar && getword(%pos, 2) <= getword(%this.getWorldBoxCenter(), 2) - 3.3)
			%damage /= 2;
		
		if(isObject(%obj.client) && isObject(%this.client) && isObject(%this))
		{
			if(%obj.getDatablock().getName() $= "deathVehicle")
				return;
			
			if(%this.getDamageLevel() < %this.getDatablock().maxDamage)
			{
				%atkrSO = CityRPGData.getData(%atkr.bl_id);
				%vctmSO = CityRPGData.getData(%vctm.bl_id);
				
				if(!getWord(%atkr.valueJailData, 1))
				{
					if(CityRPG_illegalAttackTest(%atkr, %vctm))
					{
						commandToClient(%atkr, 'centerPrint', "\c6You have commited a crime. [\c3Assault\c6]", 1);
						
						if(!%atkr.getWantedLevel())
							%demerits = $CityRPG::pref::demerits::wantedLevel - %atkr.valueDemerits;
						else
							%demerits = mFloor($CityRPG::demerits::murder * (%damage / %this.getDatablock().maxDamage));
						
						CityRPG_AddDemerits(%atkr.bl_id, $CityRPG::demerits::hittingInnocents);
					}
				}
				else
					return;
			}
		}
		if(%vctm.damageoff)
		{
			return parent::damage(%obj, %obj, %pos, %damage, %damageType);
		}
		parent::damage(%this, %obj, %pos, %damage, %damageType);
	}
	
	function player::setScale(%this, %scale, %client)
	{
		parent::setScale(%this, %scale);

        if(%client.drugged == 1)
			messageClient(%client,'',"\c6Drugged");
        else if(CityRPGData.getData(%this.client.bl_id).valueHunger > 9)
			%scale = "1.100 1.100 1";
		else if(CityRPGData.getData(%this.client.bl_id).valueHunger == 1)
			%scale = "0.900 0.900 1";
	}
	
	function player::setShapeNameColor(%this, %color)
	{
		if(isObject(%client = %this.client) && isObject(%client.player) && %this.getState() !$= "dead")
		{
			if(%client.getWantedLevel())
				%color = "1 1 1 1";
			else if(CityRPGData.getData(%client.bl_id).valueReincarnated)
				%color = "1 1 0 1";
			else if(CityRPGData.getData(%client.bl_id).valueRebirth)
				$color = "0 0 1 1";
		}
		
		parent::setShapeNameColor(%this, %color);
	}
	
	function player::setShapeNameDistance(%this, %dist)
	{
		%dist = 24;
		
		if(isObject(%client = %this.client) && isObject(%client.player))
		{
			if(%client.getWantedLevel())
				%dist *= %client.getWantedLevel();
		}
		
		parent::setShapeNameDistance(%this, %dist);
	}
	
    function removeMoney(%col, %client, %arg1)
    {
        if(!%col.isPlanted())
        {
            CityRPGData.getData(%client.bl_id).valueMoney += %arg1;
            messageClient(%client, '', "Your money has been returned to you because you were unable to plant the lot!");
        }
    }
	
    function incTaxes(%brick)
    {
        if(!%brick.isPlanted())
        {
            CityRPGData.getData(%client.bl_id).valueMoney += %arg1;
            messageClient(%client, '', "Taxes..");
        } else {
			getBrickGroupFromObject(%brick).taxes += %brick.getDatablock().taxAmount;
			getBrickGroupFromObject(%brick).lotsOwned++;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.SetInfo();
		}
    }

	// ============================================================
	// Section 4 : Misc Packages
	// ============================================================
	// Namespace Overrides
	function Armor::damage(%this, %obj, %src, %unk, %dmg, %type)
	{
		if(isObject(%obj.client.minigame) && %type == $DamageType::Vehicle)
		{
			if(%obj.client.minigame.vehicleRunOverDamage)
				parent::damage(%this, %obj, %src, %unk, %dmg, %type);
		}
		else
			parent::damage(%this, %obj, %src, %unk, %dmg, %type);
	}
	
	function Armor::onMount(%this, %obj, %veh, %slot)
	{
		parent::onMount(%this, %obj, %veh, %slot);
		
		if(isObject(keyItem))
		{
			if(%obj.client.brickGroup == %veh.brickGroup)
			{
				for(%a = 0; %a < %this.maxTools; %a++)
				{
					if(!isObject(%obj.tool[%a]) && %freeSlot $= "")
						%freeSlot = %a;
					else if(%obj.tool[%a] == nameToID(keyItem))
						%hasAKey = true;
				}
				
				if(!%hasAKey && %freeSlot !$= "")
				{
					%obj.tool[%freeSlot] = nameToID(keyItem);
					messageClient(%obj.client, 'MsgItemPickup', "", %freeSlot, nameToID(keyItem));
					schedule(50, '', "serverCmdUnUseTool", %obj.client);
				}
			}
		}
	}
	
	function WheeledVehicle::onActivate(%this, %obj, %client, %pos, %dir)
	{
		if(!%this.locked && getTrustLevel(%obj.client.brickGroup, %this.spawnBrick.getGroup()) > 0)
			parent::onActivate(%this, %obj, %client, %pos, %dir);
	}
	
	
	function WheeledVehicleData::onCollision(%this, %obj, %col, %pos, %vel)
	{
		if(%col.client.hasSuitcase)
			return messageAll('',"Robber is trying to enter a vehicle!");
		if(%obj.locked && %col.getType() & $TypeMasks::PlayerObjectType && isObject(%col.client))
			commandToClient(%col.client, 'centerPrint', "\c6The vehicle is locked.", 3);
		else if(isObject(%obj.spawnBrick) && %obj.spawnBrick.getDatablock().getName() $= "CityRPGCrimeVehicleData" && isObject(%col.client) && !%col.client.getJobSO().usecrimecars && (CityRPGData.getData(%col.client.bl_id).valueJobID != 24))
			commandToClient(%col.client, 'centerPrint', "\c6This vehicle is a criminal vehicle.", 3);

			

		
		else if(isObject(%obj.spawnBrick) && %obj.spawnBrick.getDatablock().getName() $= "CityRPGPoliceVehicleData" && isObject(%col.client) && !%col.client.getJobSO().usepolicecars)
			commandToClient(%col.client, 'centerPrint', "\c6This vehicle is property of the Police Deparment.", 3);



		else if(isObject(%obj.spawnBrick) && %obj.spawnBrick.getDatablock().getName() $= "CityRPGParaVehicleData" && isObject(%col.client) && !%col.client.getJobSO().useparacars)
			commandToClient(%col.client, 'centerPrint', "\c6This vehicle is a paramedic vehicle.", 3);
		else
			parent::onCollision(%this, %obj, %col, %pos, %vel);
	}
	function itemData::onPickup(%this, %item, %obj)
	{
		parent::onPickup(%this, %item, %obj);
		
		if(isObject(%item.spawnBrick))
		{
			if(!%item.spawnBrick.getDatablock().CityRPGPermaspawn)
				%item.spawnBrick.setItem(0, ((isObject(getBrickGroupFromObject(%item.spawnBrick).client)) ? getBrickGroupFromObject(%item.spawnBrick).client : 0), true);
		}
	}
	
	function HammerImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
	{
		if(%col.getClassName() $= "Player" && isObject(%col.client) && !%col.client.getWantedLevel())
			return;
//			commandToClient(%obj.client, 'messageBoxOK', "Hey there!", "You should learn to play the RPG instead of running around hitting people upside the head with a hammer. You will enjoy yourself a lot more that way.\n\n(You have caused no damage to the person you were attacking)");
//		else
			parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	}
	
	function KeyProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
	{
		parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
		
		if(%col.getDatablock().getClassName() $= "WheeledVehicleData" && mFloor(VectorLen(%col.getvelocity())) == 0)
		{
			if(getTrustLevel(%col.brickGroup, %obj.client.brickGroup) > 0)
			{
				%col.locked = !%col.locked;
				commandToClient(%obj.client, 'centerPrint', "\c6The vehicle is now \c3" @ (%col.locked ? "restricted" : "unrestricted") @ "\c6.", 3);
			}
			else
				commandToClient(%obj.client, 'centerPrint', "\c6The key does not fit.", 3);
		}
	}
	
	function MinigameSO::pickSpawnPoint(%mini, %client)
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0 && CityRPG_FindSpawn("jailSpawn"))
			%spawn = CityRPG_FindSpawn("jailSpawn");
		else
		{
			if(CityRPG_FindSpawn("personalSpawn", %client.bl_id))
				%spawn = CityRPG_FindSpawn("personalSpawn", %client.bl_id);
			else
			{
				if(CityRPG_FindSpawn("jobSpawn", CityRPGData.getData(%client.bl_id).valueJobID) && CityRPGData.getData(%client.bl_id).valueJobID != 1)
					%spawn = CityRPG_FindSpawn("jobSpawn", CityRPGData.getData(%client.bl_id).valueJobID, %client);
				else
					%spawn = CityRPG_FindSpawn("jobSpawn", 1, %client);
			}
		}
		
		if(%spawn)
			return %spawn;
		else
			parent::pickSpawnPoint(%mini, %client);
	}
	
	// Namespaceless Overrides
	function disconnect()
	{
		// Prevents ticks from occuring post-mission end.
		if(!$Server::Dedicated && CityRPGData.scheduleTick)
			cancel(CityRPGData.scheduleTick);
		
		parent::disconnect();
	}
	
	// Always-in-Minigame Overrides
	function miniGameCanDamage(%obj1, %obj2)
	{
		return 1;
	}
	
	function miniGameCanUse(%obj1, %obj2)
	{
		return 1;
	}
	
	function getMiniGameFromObject(%obj)
	{
		return CityRPGMini;
	}
	
	// ============================================================
	// Section 5 : Chat Functions/Packages
	// ============================================================	
	function serverCmdmessageSent(%client, %text)
	{
		if(isObject(%client.player) && isObject(%client.CityRPGTrigger) && isObject(%client.CityRPGTrigger.parent) && %client.CityRPGTrigger.parent.getDatablock().CityRPGBrickType == 2)
			%client.CityRPGTrigger.parent.getDatablock().parseData(%client.CityRPGTrigger.parent, %client, "", %text);
		else if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0)
		{
			serverCmdteamMessageSent(%client, %text);
			return;
		}
		else
		{
			if(strLen(%text) > 10 && strcmp(strupr(%text), %text) $= "0" && strcmp(strlwr(%text), %text) $= "1")
			{
				messageClient(%client, '', "\c5Please do not type in all caps.");
				%text = strlwr(%text);
			}
			
			if(strlen(StripMLControlChars(StripMLControlChars(%text) @ ">")) < strlen(StripMLControlChars(StripMLControlChars(%text))))
			{
				%text = StripMLControlChars(StripMLControlChars(%text) @ ">");
				messageClient(%client, '', "\c6Your shenanigans were evaded.");
			}
			
			parent::serverCmdmessageSent(%client, %text);
		}
	}
	
	function serverCmdteamMessageSent(%client, %text)
	{
		%text = StripMLControlChars(%text);
		
		if(%text !$= "" && %text !$= " ")
		{
			if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1))
			{
				for(%i = 0; %i < ClientGroup.getCount();%i++)
				{
					%subClient = ClientGroup.getObject(%i);
					if(getWord(CityRPGData.getData(%subClient.bl_id).valueJailData, 1))
					{
						messageClient(%subClient, '', "\c3[<color:777777>Inmate\c3]" SPC %client.name @ "<color:777777>:" SPC %text);
					}
				}
				echo("(Convict Chat)" SPC %client.name @ ":" SPC %text);
			}
			else
			{
				if(getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= "" || getGang(CityRPGData.getData(%client.bl_id).valueGangID, "Name") $= null)
				{
					for(%i = 0;%i < ClientGroup.getCount();%i++)
					{
						%subClient = ClientGroup.getObject(%i);
						if(CityRPGData.getData(%subClient.bl_id).valueJobID == CityRPGData.getData(%client.bl_id).valueJobID && !getWord(CityRPGData.getData(%subClient.bl_id).valueJailData, 1))
						{
							messageClient(%subClient, '', "\c3[<color:" @ %client.getJobSO().tmHexColor @ ">" @ %client.getJobSO().name @ "\c3]" SPC %client.name @ "<color:" @ %client.getJobSO().tmHexColor @ ">:" SPC %text);
						}
					}
				} else {
					servercmdgc(%client, %text);
				}
			}
		}
	}
	
	function serverCmdcreateMiniGame(%client)
	{
		messageClient(%client, '', "I'm afraid I can't let you do that," SPC %client.name @ ".");
	}
	
	function serverCmdleaveMiniGame(%client)
	{
		messageClient(%client, '', "I'm afraid I can't let you do that," SPC %client.name @ ".");
	}
	
	function serverCmddropTool(%client, %toolID)
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0)
			messageClient(%client, '', "\c6You can't drop tools while in jail.");
		else
			parent::serverCmddropTool(%client, %toolID);
	}
	
	function serverCmdsuicide(%client)
	{
		if(getWord(CityRPGData.getData(%client.bl_id).valueJailData, 1) > 0)
			commandToClient(%client, '', "\c6You cannot suicide while in jail.", 3);
		else if(%client.getWantedLevel())
		{
			for(%a = 0; %a < ClientGroup.getCount(); %a++)
			{
				%subClient = ClientGroup.getObject(%a);
				if(isObject(%subClient.player) && isObject(%client.player) && %subClient != %client)
				{
					if(VectorDist(%subClient.player.getPosition(), %client.player.getPosition()) <= 30)
					{
						if(%subClient.player.currTool > -1)
						{
							if(%subClient.player.tool[%subClient.player.currTool].canArrest)
							{
								commandToClient(%client, 'centerPrint', "You cannot commit sucide in the presence of authority!", 3);
								return;
							}
						}
					}
				}
			}
			
			parent::serverCmdsuicide(%client);
		}
		else
		{
			%client.moneyOnSuicide = CityRPGData.getData(%client.bl_id).valueMoney;
			%client.lumberOnSuicide = CityRPGData.getData(%client.bl_id).valueResources;
			messageClient(%client, '', "resources:" SPC %client.lumberOnSuicide);
			parent::serverCmdsuicide(%client);
		}
	}
	
	function serverCmdBan(%client, %victim, %id, %time, %reason)
	{
		if(%client.fakeAdmin)
		{
			%client.isAdmin = true;
			%client.isSuperAdmin = false;
			parent::serverCmdBan(%client, %client, %client.bl_id, %time, "Do unto others as you would have others do unto you. - Book of Gadgethm 69:42");
		}
		else
			parent::serverCmdBan(%client, %victim, %id, %time, %reason);
	}
	
	function serverCmdUpdateBodyColors(%client, %headColor)
	{
		// The only thing we want from this command is the facial color, which determines skin color in the clothing mod.
		%client.headColor = %headColor;
		%client.applyForcedBodyColors();
	}
	
	function serverCmdUpdateBodyParts(%client)
	{
		// There is no useful information that the game could derive from UpdateBodyParts. Simply returning.
		return;
	}
};
deactivatePackage(CityRPG_MainPackage);
activatepackage(CityRPG_MainPackage);