function Player::attemptGrab(%this)
{
	//if(%this.client.isAdmin){
    %scale = getWord(%this.getScale(),2);
    %start = %this.getEyePoint();
    %vec   = %this.getEyeVector();
    %end   = vectorAdd(%start,vectorScale(%vec,2 * %scale));
    
    %ray = containerRaycast(%start,%end,$TypeMasks::PlayerObjectType,%this);
    %col = firstWord(%ray);
    
    //if(!isObject(%col) || %col.isHeld || %col.isHolding)
    if(!isObject(%col) || %cl.unholdable)
        return 0;
    
    %client = %this.client;
    %scale2 = getWord(%col.getScale(),2);
    %model  = fileName(%col.getDatablock().shapeFile);
    %proper = (%col.client.miniGame.weaponDamage $= 1 || %col.spawnBrick.client == %client);
    
    if(%scale2 > %scale || %model !$= "m.dts" || !%proper)
        return 0;
    
    %this.isHolding  = 1;
    %this.oldDatablock = %this.getDatablock();
    %this.changeDatablock(playerNoJet);
    %col.isHeld      = 1;
    %col.canDismount = 0;
    %col.setScale("0.6 0.6 0.6");
    
    %col.setLookLimits(0.5,0.5);
    %col.unMountImage(0);
    
    if(isObject(%them = %col.client))
    {
        %them.camera.setMode("Grabbed");
        %them.setControlObject(%them.camera);
        %them.camera.setorbitmode(%this,0,5,10,5,0);
    }
    
    %this.mountObject(%col,1);
    %col.playThread(2,death1);
    
    if(%scale2 >= %scale / 2)
        %this.playThread(2,armreadyboth);
    else
        %this.playThread(2,armreadyleft);
    
    %pos = %col.getPosition();
    %col.setTransform(%pos SPC "0 0 -0.85 90");
    
    return 1;
    %client = %this.client;
    centerPrint(%client,"\c6Your are now holding \c3" @ %col.client.name @ "\c6. Click to throw.");
	//}
}

function Player::attemptThrow(%this)
{
    if(!%this.isHolding)
        return 0;
    
    %held = %this.getMountedObject(0);
    %vec  = %this.getEyeVector();
    
    %this.unMountObject(%held);
    %this.playThread(2,root);
    %held.playThread(2,root);
    %held.setLookLimits(1,0);
    
    %held.addVelocity(vectorScale(%vec,50));
    
    if(isObject(%them = %held.client))
        %them.setControlObject(%held);
    
    %this.isHolding   = 0;
    %this.setDatablock(%this.oldDatablock);
    %held.isHeld      = 0;
    %held.canDismount = 1;
    %held.setScale("1 1 1");
    
    return 1;
}

package ThrowMod
{
    function Player::activateStuff(%this)
    {
        if(!%this.attemptThrow())
        {
            if(!%this.attemptGrab())
                return Parent::activateStuff(%this);
        }
    }
};
activatePackage(ThrowMod);

