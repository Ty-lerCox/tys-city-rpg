package SplitDeath
{
	function clientCmdServerMessage(%tag,%msg,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10)
	{
		%detagged = detag(%tag);
		%det = detag(%msg);
		if(%detagged $= "MsgYourDeath" || %detagged $= "MsgClientKilled")
		{
			return;
		}
		else
			return Parent::clientCmdServerMessage(%tag,%msg,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10);
	}
};
deactivatePackage(SplitDeath);
activatePackage(SplitDeath);