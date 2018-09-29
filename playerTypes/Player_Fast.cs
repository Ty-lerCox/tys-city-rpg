//Why are you looking at this file? Planning to steal it?
//By: Boomerangdog
datablock PlayerData(FastPlayerArmor : PlayerStandardArmor)
{
   runForce = 50 * 15;
   runEnergyDrain = 0;
   minRunEnergy = 0;
   maxForwardSpeed = 60;
   maxBackwardSpeed = 100;
   maxSideSpeed = 40;

   maxForwardCrouchSpeed = 30;
   maxBackwardCrouchSpeed = 50;
   maxSideCrouchSpeed = 80;
 
   maxDamage = 200;

   jumpEnergyDrain = 0;
   minJumpEnergy = 0;
   jumpDelay = 0;

	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

	uiName = "Fast Player";
	showEnergyBar = false;

   runSurfaceAngle  = 55;
   jumpSurfaceAngle = 55;
};
