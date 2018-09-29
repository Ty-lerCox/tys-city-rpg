package WaterDamage
{
	function Armor::damage(%data, %this, %proj, %hitPos, %damage, %type)
	{		
        if((%type == $DamageType::Lava) && ((%this.getClassName() $= "Player") || (%this.getClassName() $= "aiPlayer")))
		{
            %damage = 0;
            //$keep = 1;
        }
        Parent::damage(%data, %this, %proj, %hitPos, %damage, %type);
	}
};
activatePackage("WaterDamage");

function serverCmdgetKeep(%client)
{
    messageClient(%client,'',"..." @ $keep);
}