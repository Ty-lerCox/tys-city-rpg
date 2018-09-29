// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Code for Idiots
// ============================================================

package CityRPG_IdiotPackage
{
	function serverCmdmessageSent(%client, %text)
	{
		//if(getSubStr(%text, 0, 1) $= "^")
			//messageClient(%client, '', "\c5This system does not use Jookia's carets. Please use a standard / infront of all commands.");
		
		//if(strstr(%text, "help") >= 0)
			//messageClient(%client, '', "\c6The <a:cityrpg.site50.net/help.php>First Time Player</a>\c6's guide can be of invaluable assistance to newbies.");
		
		//if(strstr(%text, "how do") >= 0)
			//messageClient(%client, '', "\c6The <a:cityrpg.site50.net/faq.php>FAQ</a>\c6 answers many common questions..");
		
		parent::serverCmdmessageSent(%client, %text);
	}
};
activatePackage(CityRPG_IdiotPackage);