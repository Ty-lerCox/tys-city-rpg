//------
//Variable Conditional Events Client
//Creator: Zack0Wack0
//------
if($Pref::Client::ShowVCEHandshake $= "")
	$Pref::Client::ShowVCEHandshake = 0;
$VCE::Client = 1;
$VCE::Client::Manual = 1;
if(!$VCE::Client::AddedBinds)
{
	$remapDivision[$remapCount] = "Variable/Conditional Events";
	$remapName[$remapCount] = "Toggle VCE Client";
	$remapCmd[$remapCount] = "VCEClient_toggle";
	$remapCount++;
	$VCE::Client::AddedBinds = 1;
}
if(!isObject(VCEClient_TextProfile))
{
	new GuiControlProfile(VCEClient_TextProfile)
	{
 		fontColor = "30 30 30 255";
		fontType = "Palatino Linotype";
		fontSize = "18";
		justify = "Left";
		fontColors[1] = "100 100 100";
		fontColors[2] = "0 255 0";  
		fontColors[3] = "0 0 255"; 
		fontColors[4] = "255 255 0"; 
		fontColorLink = "60 60 60 255";
		fontColorLinkHL = "0 0 0 255";
	};
}
if(!isObject(VCEClient_ContentBorderProfile))
{
	new GuiControlProfile(VCEClient_ContentBorderProfile : GuiBitmapBorderProfile)
	{
 		bitmap = "Add-Ons/Event_Variables/images/contentBorder.png";
	};
}
if(!isObject(VCEClient_ScrollProfile))
{
	new GuiControlProfile(VCEClient_ScrollProfile : GuiScrollProfile)
	{
		hasBitmapArray = true;
		bitmap = "Add-Ons/Event_Variables/images/scrollBar.png";
	};
}
if(!isObject(VCEClient))
{
	exec("Add-Ons/Event_Variables/VCEClient.gui");
}
if(!isObject(VCEClient_Manual_PopUp))
{
	%f = new GuiBitmapButtonCtrl(VCEClient_Manual_PopUp) {
		profile = "BlockButtonProfile";
		horizSizing = "right";
		vertSizing = "bottom";
		position = "593 31";
		extent = "80 18";
		minExtent = "8 2";
		visible = "1";
		command = "canvas.pushDialog(VCEClient);";
		text = "VCE Client";
		groupNum = "-1";
		buttonType = "PushButton";
		bitmap = "base/client/ui/button2";
		lockAspectRatio = "0";
		alignLeft = "0";
		overflowImage = "0";
		mKeepCached = "0";
		mColor = "255 255 255 255";
	};
	WrenchEvents_Window.add(%f);
}
function VCEClient_Content::setTab(%this,%tab)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%obj = %this.getObject(%i);
		if(strPos(%obj.getName(),"VCEClient_") > -1)
			%obj.setVisible(0);
	}
	%tab.setVisible(1);
	%tab.onTabWake();
}
function VCEClient_toggle(%val)
{
	if(%val)
	{
		switch(VCEClient.isAwake())
		{
			case 1:
				canvas.popDialog(VCEClient);
			case 0:
				canvas.pushDialog(VCEClient);
		}
	}
}
//HTTP shit
if(!isObject(VCEClientHTTP))
{
	new HTTPObject(VCEClientHTTP)
	{
		isConnected = false;
		debug = false;
	};
}
function VCEClientHTTP::onConnected(%this)
{
	%this.isConnected = true;
	VCEClient_Status_TextControl.setText("<just:center>" @ %this.connectedMsg);
}
function VCEClientHTTP::onDisconnect(%this)
{
	%this.isConnected = false;
}
function VCEClientHttp::onDNSFailed(%this)
{
	%this.isConnected = false;
	VCEClient_Status_TextControl.setText("<just:center>Failed to connect due to DNS errors.");
}
function VCEClientHttp::onConnectFailed(%this)
{
	%this.isConnected = false;
	VCEClient_Status_TextControl.setText("<just:center>Failed to connect due to internet errors.<br>Check your connection");
}
function VCEClientHTTP::onLine(%this,%line)
{
	if(%this.debug)
		//echo(%line);
	switch$(%this.cmd)
	{
		case "FETCHMANUALCACHE":
			if(firstWord(%line) $= "CACHE:")
			{
				%cache = restWords(%line);
				if(%cache > $pref::Client::lastVCEManualCache)
				{
					$pref::Client::lastVCEManualCache = %cache;
					%this.cmd = "FETCHMANUAL";
					%this.connectedMsg = "Connected, new manual found - caching";
					%this.get("blockland.zack0wack0.com:80","/vce/manual.php?t=1");
					return;
				}
				else
				{
					VCEClient_fetchManualCache();
					return;
				}
				%this.disconnect();
			}
			else
				return;
		case "FETCHMANUAL":
			if(firstWord(%line) $= "ERROR:")
			{
				VCEClient_Status_TextControl.setText("<just:center>There was an error fetching the manual:<br>" @ VCEClient_parseIcons(restWords(%line)));
				return;
			}
			if(firstWord(%line) !$= "MANUAL:")
			{
				return;
			}
			if(!isObject(%this.manualCacher))
			{
				%this.manualCacher = new FileObject();
				%this.manualCacher.openForWrite("config/client/VCE/manual.txt");
			}
			if(isEventPending(%this.manualCacher.delete))
				cancel(%this.manualCacher.delete);
			%this.manualCacher.writeLine(%line);
			%icon = getField(%line,0);
			%icon = getWord(%icon,1);
			%name = getField(%line,1);
			%content = VCEClient_parseIcons(getField(%line,2));
			VCEClient_Manual_ListControl.addRow(VCEClient_Manual_ListControl.rowCount(),"      " @ %name TAB %content);
			%rows = VCEClient_Manual_ListControl.rowCount()-1;
			if(isFile("./images/icons/"@%icon))
			{
				%bit = new GuiBitmapCtrl()
				{
					position = 0 SPC ((%rows * 16) + 2) + %rows * 4;
					extent = "16 16";
					bitmap = "./images/icons/"@%icon;
				};
				VCEClient_Manual_ListIcons.add(%bit);
			}
			VCEClient_Manual_ListControl.setSelectedRow(0);
			VCEClient_Content.setTab(VCEClient_Manual);
			%this.manualCacher.delete = %this.manualCacher.schedule(1500,"delete");
	}
}
//Manual
function VCEClientHTTP::fetchManual(%this)
{
	VCEClient_Status_TextControl.setText("<just:center>Connecting...");
	VCEClient_Content.setTab(VCEClient_Status);
	%this.disconnect();
	VCEClient_Manual_TextControl.setText("");
	VCEClient_Manual_ListControl.clear();
	VCEClient_Manual_ListIcons.clear();
	%this.cmd = "FETCHMANUALCACHE";
	%this.connectedMsg = "Connected, fetching cache details...";
	%this.get("blockland.zack0wack0.com:80","/vce/manual.php?t=0");
}
function VCEClient_fetchManualCache()
{
	if(isFile("config/client/VCE/manual.txt"))
	{
		%r = new FileObject();
		%r.openForRead("config/client/VCE/manual.txt");
		while(!%r.isEOF())
		{
			%line = %r.readLine();
			%icon = getField(%line,0);
			%icon = getWord(%icon,1);
			%name = getField(%line,1);
			%content = VCEClient_parseIcons(getField(%line,2));
			VCEClient_Manual_ListControl.addRow(VCEClient_Manual_ListControl.rowCount(),"      " @ %name TAB %content);
			%rows = VCEClient_Manual_ListControl.rowCount() - 1;
			if(isFile("./images/icons/"@%icon))
			{
				%bit = new GuiBitmapCtrl()
				{
					position = 0 SPC ((%rows*16)+2)+%rows*4;
					extent = "16 16";
					bitmap = "./images/icons/"@%icon;
				};
				VCEClient_Manual_ListIcons.add(%bit);
			}
		}
		%r.close();
		%r.delete();
		VCEClient_Manual_ListControl.setSelectedRow(0);
		VCEClient_Content.setTab(VCEClient_Manual);
	}
}
function VCEClient_Manual_ListControl::onSelect(%this,%row,%text)
{
	%text = getField(%text,1);
	if(%text !$= "")
		VCEClient_Manual_TextControl.setText(VCEClient_ParseIcons(%text));
}
function VCEClient_Manual::onTabWake(%this)
{
	return;
}
//Changelog
function VCEClient_Changelog::onTabWake(%this)
{
	VCEClient_ChangeLog_TextControl.setText("");
	%fr = new fileobject();
	%fr.openForRead("Add-Ons/Event_Variables/changelog.txt");
	while(!%fr.isEOF())
	{
		%line = VCEClient_ParseIcons(%fr.readLine());
		%text = VCEClient_ChangeLog_TextControl.getText() $= "" ? %line : VCEClient_ChangeLog_TextControl.getText() NL %line;
		VCEClient_ChangeLog_TextControl.setText(%text);
	}
	%fr.close();
	%fr.delete();
}
//Online
function VCEClient_Online::onTabWake(%this)
{
	return;
}
//Status
function VCEClient_Status::onTabWake(%this)
{
	return;
}
//Icon Parsing
function VCEClient_parseIcons(%string)
{
	%string = strReplace(%string," <3","<bitmap:Add-Ons/Event_Variables/images/emotes/heart.png>");
	%string = strReplace(%string," ;D","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_wink.png>");
	%string = strReplace(%string," ;)","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_wink.png>");
	%string = strReplace(%string," :3","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_waii.png>");
	%string = strReplace(%string," :(","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_unhappy.png>");
	%string = strReplace(%string," D:","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_unhappy.png>");
	%string = strReplace(%string," :P","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_tongue.png>");
	%string = strReplace(%string," :o","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_surprised.png>");
	%string = strReplace(%string," :O","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_surprised.png>");
	%string = strReplace(%string," :)","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_smile.png>");
	%string = strReplace(%string," ^^","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_happy.png>");
	//hahha no, only idiots use this emote - %string = strReplace(%string," XD","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_evilgrin.png>");
	%string = strReplace(%string," :D","<bitmap:Add-Ons/Event_Variables/images/emotes/emoticon_grin.png>");
	%string = strReplace(%string," <li>","<bitmap:Add-Ons/Event_Variables/images/icons/bullet_black.png>");
	%string = strReplace(%string," <subli>","<bitmap:Add-Ons/Event_Variables/images/icons/bullet_arrow_down.png>");
	return %string;
}
package VCE_Client
{
	function WrenchEventsDlg::onWake(%this)
	{
		Parent::onWake(%this);
		if(!$Pref::Client::ShownVCEManual)
		{
			$Pref::Client::ShownVCEManual = 1;
			canvas.pushDialog(VCEClient);
		}
	}
	function GuiMLTextCtrl::onUrl(%this,%url)
	{
		%explode = strReplace(%url,"-","\t");
		if(getField(%explode,0) $= "varlink")
			commandToServer('VCE_onLink',getField(%explode,1),getField(%explode,2));
		else
			Parent::onUrl(%this,%url);
	}
};
activatePackage(VCE_Client);
function clientCmdVCE_Handshake(%version)
{
	//echo("VCEClient: Server is running VCE " @ %version @ ".");
	if($Pref::Client::ShowVCEHandshake)
		NewChatSO.addLine("<font:Palatino Linotype:24>\c6This server is running \c2VCE \c0" @ %version @ "\c6.");
	commandToServer('VCE_Handshake');
}
VCEClient_Content.setTab(VCEClient_Manual);
VCEClientHTTP.fetchManual();