datablock ParticleData(CityRPG_PainParticle)
{
	dragCoefficient      = 5.0;
	gravityCoefficient   = -0.2;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 500;
	useInvAlpha          = false;
	textureName          = "Add-Ons/Gamemode_TysCityRPG/shapes/decals/Pain";
	colors[0]     = "1.0 0.0 0.0 1.0";
	colors[1] = "";
	sizes[0]      = 3;
	sizes[1] = "";
};

datablock ParticleEmitterData(CityRPG_PainEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 0.5;
	ejectionOffset   = 1.0;
	velocityVariance = 0.49;
	thetaMin         = 0;
	thetaMax         = 120;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "CityRPG_PainParticle";

	uiName = "Emote - BlockPain";
};

datablock ShapeBaseImageData(CityRPG_PainImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.350;
	stateEmitter[1]					= CityRPG_PainEmitter;
	stateEmitterTime[1]				= 0.350;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};

function CityRPG_PainImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}