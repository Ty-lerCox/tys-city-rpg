function serverCmdBusiness(%client, %Do, %Cost, %YesNo, %Name)
{
	%Bus::Money = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money");
	%Bus::Name = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name");
	%Bus::OriginalStocks = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks");
	%Bus::StocksWorth = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks")/10;
	%Bus::IncomePerStock = %Bus::StocksWorth/10;
	%Your::Income = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "valueYourStocks") * %Bus::IncomePerStock;
	%Your::Stocks = CityRPGData.getData(%client.bl_id).valueBusStocks;
	%Your::Position = CityRPGData.getData(%client.bl_id).valueBusPosition;
	if(%Do $= "Start")
    {
        if(!$Business::StartCost)
            $Business::StartCost = 10000;
        //if player has license
        if(CityRPGData.getData(%client.bl_id).valueMoney >= $Business::StartCost)
        {
            //if business name doesn't exist
            if(!(%Cost $= ""))
			{
				if(%Cost >= $Business::StartCost)
				{
					if(%Cost >= 25001)
					{
						messageClient(%client, '', "Businesses are still in it's beta, so we are limiting it to 25k as max. Thanks for high interest tho. <3 Ty");
						return;
					}
					%Cost -= (%cost)*(0.5);
					messageClient(%client, '', "\c6Are you sure this is correct?");
					messageClient(%client, '', "-\c6Starting Account: \c3$" @ %Cost);
					messageClient(%client, '', "-\c6Starting Stocks: \c3" SPC (%Cost/50));
					messageClient(%client, '', "-\c6Starting Cost Per Stock: \c3$" @ ((%Cost/50)/10));
					messageClient(%client, '', "-\c6Starting Income Per Stock: \c3$" @ (((%Cost/50)/10)/10));
					if(%YesNo $= "Yes")
					{
						if(!(%Name $= ""))
						{
							CityRPGData.getData(%client.bl_id).valueBusID = %client.bl_id;
							messageClient(%client, '', "\c6You have started your own business!");
							CityRPGData.getData(%client.bl_id).valueMoney -= %cost;
							inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name", %Name);
							inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money", %cost);
							inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks", %Cost/50);
							CityRPGData.getData(%client.bl_id).valueBusPosition = "CEO";
							CityRPGData.getData(%client.bl_id).valueBusStocks = getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks");
							messageClient(%client, '', "\c6Business Name: \c6" @ getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name"));
							%client.SetInfo();
							saveBusiness();
						} else {
							messageClient(%client, '', "\c6Please give your Business a name!");
						}
					} else if(%YesNo $= "No") {
						messageClient(%client, '', "\c6You have decided not to start your own business.");
					} else {
						messageClient(%client, '', "\c6Please type \c3yes \c6or \c3no.");
					}
				} else {
					messageClient(%client, '', "\c6The amount you've enter is lower than the required amount to start a business.");
				}
			} else {
				messageClient(%client, '', "-\c6Please enter the amount of money you'd like to start your business for with.");
				messageClient(%client, '', "-Note! \c6The more you put into your business the more the stocks will make.");
				messageClient(%client, '', "-\c6Amount must be over (\c3$10000\c6)");
			}
        } else {
			messageClient(%client, '', "\c6You don't have the min amount need to start a business.");
		}
    } else if(%Do $= "BusinessAccount") {
		messageClient(%client, '', "\c6The business account has \c3$" @ %Bus::Money);
		messageClient(%client, '', "\c6The stocks are worth \c3$" @ %Bus::StocksWorth SPC "each\c6.");
		messageClient(%client, '', "\c6The stocks make \c3$" @ %Bus::IncomePerStock @ "per stock\c6.");
	} else if(%Do $= "YourAccount") {
		messageClient(%client, '', "\c6You have\c3" SPC %Your::Stocks SPC "Stocks\c6.");
		messageClient(%client, '', "\c6Your stocks are worth \c3$" @ %Bus::StocksWorth SPC "each\c6.");
		messageClient(%client, '', "\c6The total income from all stocks: \c3$" @ %Your::Income);
	} else if(%Do $= "Manage") {
		messageClient(%client, '', "\c6Hiring:");
		messageClient(%client, '', "--\c6Type /hire [name] [position]");
		messageClient(%client, '', "--\c6Ex: /hire Ty Manager");
		messageClient(%client, '', "--\c6This will set player Ty as one of ur managers.");
		messageClient(%client, '', "--\c6Managers are allowed to hire stock holders or buy stock.");
		messageClient(%client, '', "--\c6Stock holders are players that are able to buy stock from you or any other player within ur company with stock.");
		messageClient(%client, '', "--\c6To hire a stock holder type /hire [name] stockholder");
		messageClient(%client, '', "--\c6Ex: /hire ty stockholder");
	} else if(%Do $= "Destroy") {
		if((!(getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") $= "")) || (!(getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") $= null)))
		{
			if(CityRPGData.getData(%client.bl_id).valueBusPosition $= "CEO")
			{
				if(%YesNo $= "Yes")
				{
					CityRPGData.getData(%client.bl_id).valueMoney += getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money");
					inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name", "");
					inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Money", "0");
					inputBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks", "0");
					CityRPGData.getData(%client.bl_id).valueBusPosition = "";
					CityRPGData.getData(%client.bl_id).valueBusStocks = 0;
					messageClient(%client, '', "\c6You have \c0destroyed \c6your business!");
					CityRPGData.getData(%client.bl_id).valueBusID = 0;
					%client.SetInfo();
					saveBusiness();
				} else {
					messageClient(%client, '', "\c6Are you sure you want to destroy your business? Type \c3yes \c6or \c3no");
				}
			} else {
				messageClient(%client, '', "\c6You are not the CEO of your company.");
			}
		} else {
			messageClient(%client, '', "\c6You're not in a business.");
		}
	} else if(%Do $= "ClearStock") {
		messageClient(%client, '', "Deleted stocks");
		CityRPGData.getData(%client.bl_id).valueBusPosition = "";
		CityRPGData.getData(%client.bl_id).valueBusStocks = 0;
		CityRPGData.getData(%client.bl_id).valueBusID = 0;
	} else {
        messageClient(%client, '', "\c6Use: \c3/business start");
    }
}

function tradeStock(%client, %arg1, %arg2, %arg3)
{
	if(%client.tradeID > 0)
	{
		messageClient(%client, '', "\c6You are currently in a trade. Type \c3/trade clear");
		return;
	}
	if(!(%arg2 $= "") && !(%arg3 $= ""))
	{
		%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
		if(isObject(%target))
		{
			if(CityRPGData.getData(%client.bl_id).valueBusStocks >= %arg2)
			{
				%getTradeID = getRandom(1,100);
				messageClient(%client, '', "\c3.Trade.");
				messageClient(%target, '', "\c3.Trade.");
				messageClient(%client, '', "\c6You have began a trade with\c3" SPC %target.name SPC "\c6. Selling him/her\c3" SPC %arg2 SPC "Stocks \c6for \c3$" @ %arg3 @"\c6.");
				messageClient(%target, '', "\c3" @ %client.name SPC "\c6has began a trade with you. Selling you\c3" SPC %arg2 SPC "Stocks \c6for \c3$" @ %arg3 @ "\c6.");
				messageClient(%target, '', "\c6Would you like to accept this trade? \c3/yestradestocks \c6or \c3/notradestocks\c6.");
				%client.tradeID = %getTradeID;
				%target.tradeID = %getTradeID;
				%client.asking = %arg3;
				%client.has = %arg2;
				%client.SetInfo();
				saveBusiness();
			} else {
				messageClient(%client, '', "\c3.Trade.");
				messageClient(%client, '', "\c6You don't have\c3" SPC %arg2 SPC "Stocks\c6.");
			}
		} else {
			messageClient(%client, '', "\c3.Trade.");
			messageClient(%client, '', "\c6The player must be infront of you to trade him.");
		}
	} else {
		messageClient(%client, '', "\c3.Trade.");
		messageClient(%client, '', "\c6-Fill out the command correctly:");
		messageClient(%client, '', "\c6-/trade Stocks [\c3amount\c6] [\c3cost each\c6]");
		messageClient(%client, '', "\c6-Ex: /trade Stocks 10 50");
	}
}

function serverCmdyestradestocks(%client)
{
    %target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
    if(isObject(%target))
	{
        if(%target.tradeID > 0)
        {
            if(%target.tradeID == %client.tradeID)
            {
				if(getBusiness(CityRPGData.getData(%target.bl_id).valueBusID, "Name") $= getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name"))
				{
					if(CityRPGData.getData(%client.bl_id).valueMoney >= %target.asking)
					{
						messageClient(%client, '', "\c6You have bought\c3" SPC %target.has SPC "Stocks \c6for \c3$" @ %target.asking @ "\c6.");
						messageClient(%target, '', "\c6You have sold\c3" SPC %target.has SPC "Stocks \c6for \c3$" @ %target.asking @ "\c6.");
						CityRPGData.getData(%target.bl_id).valueBusStocks -= %target.has;
						CityRPGData.getData(%client.bl_id).valueBusStocks += %target.has;
						CityRPGData.getData(%target.bl_id).valueMoney += %target.asking;
						CityRPGData.getData(%client.bl_id).valueMoney -= %target.asking;
						%client.SetInfo();
						saveBusiness();
					} else {
						messageClient(%client, '', "You don't have enought money to do this transaction.");
						messageClient(%target, '', %client.name SPC "don't have enought money to do this transaction.");
					}
					clearTrade(%client, %target);
				} else {
					if(getBusiness(CityRPGData.getData(%client.bl_id).valueBusID, "Name") $= "")
					{
						if(CityRPGData.getData(%client.bl_id).valueMoney >= %target.asking)
						{
							serverCmdJoinBusiness(%client, CityRPGData.getData(%target.bl_id).valueBusID);
							messageClient(%client, '', "\c6You have bought\c3" SPC %target.has SPC "Stocks \c6for \c3$" @ %target.asking @ "\c6.");
							messageClient(%target, '', "\c6You have sold\c3" SPC %target.has SPC "Stocks \c6for \c3$" @ %target.asking @ "\c6.");
							CityRPGData.getData(%target.bl_id).valueBusStocks -= %target.has;
							CityRPGData.getData(%client.bl_id).valueBusStocks += %target.has;
							CityRPGData.getData(%target.bl_id).valueMoney += %target.asking;
							CityRPGData.getData(%client.bl_id).valueMoney -= %target.asking;
							%client.SetInfo();
							saveBusiness();
						} else {
							messageClient(%client, '', "You don't have enought money to do this transaction.");
							messageClient(%target, '', %client.name SPC "don't have enought money to do this transaction.");
						}
						clearTrade(%client, %target);
					}
					messageClient(%client, '', "\c6This player isn't part of the business. Type \c3/hire");
				}
            }
        } else {
            messageClient(%client, '', "This player isn't trading");
        }
    } else {
        messageClient(%client, '', "\c3.Trade.");
        messageClient(%client, '', "\c6The player must be infront of you to trade him.");
    }
}

function serverCmdnotradestocks(%client)
{
    clearTrade(%client, %target);
	%client.SetInfo();
	saveBusiness();
}

function serverCmdJoinBusiness(%client, %arg1)
{
	CityRPGData.getData(%client.bl_id).valueBusID = %arg1;
	CityRPGData.getData(%client.bl_id).valueBusPosition = "StockHolder";
	CityRPGData.getData(%client.bl_id).valueBusStocks = 0;
	%client.setInfo();
}

function getMembers(%id)
{
	
}

function togglePrints(%client)
{
	if(CityRPGData.getData(%client.BL_ID).valuePrint $= "Stats")
	{
		CityRPGData.getData(%client.BL_ID).valuePrint = "Business";
		messageClient(%client, '', "\c6Prints set to business prints.");
	} else {
		CityRPGData.getData(%client.BL_ID).valuePrint = "Stats";
		messageClient(%client, '', "\c6Prints set to starts prints.");
	}
	%client.setInfo();
}