function serverCmdLayout(%client, %arg1)
{
    if(%arg1 $= "Default") {
        messageClient(%client,'',"\c6Your layout has been reset to default.");
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:3C9EFF>";
    } else if(%arg1 $= "DarkBlue") {
        messageClient(%client,'',"\c6Your color has been set to: <color:3F5D7D>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:3F5D7D>";
    } else if(%arg1 $= "Green") {
        messageClient(%client,'',"\c6Your color has been set to: <color:279B61>" @ %arg1);
         CityRPGData.getData(%client.bl_id).valueLayout = "<color:279B61>";
   } else if(%arg1 $= "LightBlue") {
        messageClient(%client,'',"\c6Your color has been set to: <color:008AB8>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:008AB8>";
    } else if(%arg1 $= "LightGreen") {
        messageClient(%client,'',"\c6Your color has been set to: <color:A3E496>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:A3E496>";
    } else if(%arg1 $= "DarkRed") {
        messageClient(%client,'',"\c6Your color has been set to: <color:993333>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:993333>";
    } else if(%arg1 $= "Red") {
        messageClient(%client,'',"\c6Your color has been set to: <color:CC3333>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:CC3333>";
    } else if(%arg1 $= "Yellow") {
        messageClient(%client,'',"\c6Your color has been set to: <color:FFCC33>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:FFCC33>";
    } else if(%arg1 $= "Pink") {
        messageClient(%client,'',"\c6Your color has been set to: <color:CC6699>" @ %arg1);
        CityRPGData.getData(%client.bl_id).valueLayout = "<color:CC6699>";
    } else {
        messageClient(%client,'',"\c6Please choose from the following: <color:3F5D7D>DarkBlue\c6, <color:279B61>Green\c6, <color:008AB8>LightBlue\c6, <color:A3E496>LightGreen\c6, <color:993333>DarkRed\c6, <color:CC3333>Red\c6, <color:FFCC33>Yellow\c6, <color:CC6699>Pink");
    }
	%client.SetInfo();
}

function serverCmdsetFont(%client, %arg1)
{
    
}