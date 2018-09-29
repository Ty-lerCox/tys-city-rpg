function serverCmdBuyTickett(%client, %arg1)
{
    if($Lottery::Cost == 0)
        $Lottery::Cost = 15;
    if(%arg1 $= "")
        %arg1 = 1;
	if(%arg1 < 1)
		return;
    %cost = %arg1 * $Lottery::Cost;
    if(CityRPGData.getData(%client.bl_id).valueMoney >= %cost)
    {
        CityRPGData.getData(%client.bl_id).valueTickets = CityRPGData.getData(%client.bl_id).valueTickets + %arg1;
        messageClient(%client,'',"\c6You have bought\c3" SPC %arg1 SPC "\c6ticket(s). You have\c3" SPC CityRPGData.getData(%client.bl_id).valueTickets SPC "\c6ticket(s).");
        messageClient(%client,'',"\c6Type: \c3/scratchticket");
        messageClient(%client,'',"\c6Type: \c3/mytickets");
        CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney - %cost;
    } else {
        messageClient(%client,'',"\c6You need \c3$" @ %cost SPC "\c6to buy\c3" SPC %arg1 SPC"\c6tickets.");
    }
}

function serverCmdmyTickets(%client, %arg1)
{
    messageClient(%client,'',"\c6You have\c3" SPC CityRPGData.getData(%client.bl_id).valueTickets SPC "\c6tickets. Type \c3/ScratchTicket");
}

function serverCmdScratchTicket(%client)
{
    %lockoutchance = getRandom(1,3);
    if(CityRPGData.getData(%client.bl_id).valueTickets > 0)
    {
        %change = getRandom(1,100);
        if(%change >= 97) {
            %cash = getRandom(100,350);
            messageClient(%client,'',"\c6You have won big! You've gotten\c3 $" @ %cash @ "!");
            
        } else if(%change >= 93) {
            %cash = getRandom(20,50);
            messageClient(%client,'',"\c6You've won\c3 $" @ %cash @ "!");
        } else if(%change >= 70) {
            %cash = getRandom(1,20);
            messageClient(%client,'',"\c6You've won\c3 $" @ %cash @ ".");
        } else {
            messageClient(%client,'',"\c6Your ticket didn't win any money.");
        }
        CityRPGData.getData(%client.bl_id).valueTickets--;
        CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney + %cash;
    } else {
        messageClient(%client,'',"\c6You don't have any tickets!");
    }
}

function serverCmdScratchAllTicketss(%client)
{
    %lockoutchance = getRandom(1,3);
    if(CityRPGData.getData(%client.bl_id).valueTickets > 0)
    {
        %loop = CityRPGData.getData(%client.bl_id).valueTickets;
        %totalCash = 0;
        for(%c = 0; %c < %loop; %c++)
	    {
            %change = getRandom(1,100);
            if(%change >= 97) {
                %cash = getRandom(100,350);
                %totalCash = %cash + %totalCash;
            } else if(%change >= 93) {
                %cash = getRandom(20,50);
                %totalCash = %cash + %totalCash;
            } else if(%change >= 70) {
                %cash = getRandom(1,20);
                %totalCash = %cash + %totalCash;
            } else {
                
            }CityRPGData.getData(%client.bl_id).valueTickets--;
            
        }
            CityRPGData.getData(%client.bl_id).valueMoney = CityRPGData.getData(%client.bl_id).valueMoney + %totalCash;
            messageClient(%client,'',"\c6You made:\c3 $" @ %totalCash);
    } else {
        messageClient(%client,'',"\c6You don't have any tickets!");
    }
}

function serverCmdSetTickets(%client, %arg1)
{
    if(%client.isAdmin)
    {
        CityRPGData.getData(%client.bl_id).valueTickets = %arg1;
    }
}