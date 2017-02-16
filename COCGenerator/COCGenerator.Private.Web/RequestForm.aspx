<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestForm.aspx.cs" Inherits="COCGenerator.RequestForm"
    Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta id="MetaDescription" name="DESCRIPTION" content="Arthur J. Gallagher & Co (Aus) Limited works to understand the needs and provide solutions to all shapes and sizes of businesses. We get closer to clients and closer to communities.  Contact your local broker today." />
    <meta id="MetaKeywords" name="KEYWORDS" content="insurance, brokers, business insurance, finance, superannuation, australia, agriculture, mining, hopitality, sport, aviation, marine, Insurance, Broker, Broking, Business, Liability, Risk, Commercial, Finance, Wesfarmers, Quote, Clients, Communities" />
    <meta id="MetaCopyright" name="COPYRIGHT" content="© Copyright Arthur J. Gallagher & Co (Aus) Limited 2014" />
    <meta id="MetaAuthor" name="AUTHOR" content="Arthur J. Gallagher & Co (Aus) Limited" />
    <meta name="RESOURCE-TYPE" content="DOCUMENT" />
    <meta name="DISTRIBUTION" content="GLOBAL" />
    <meta name="ROBOTS" content="INDEX, FOLLOW" />
    <meta name="REVISIT-AFTER" content="1 DAYS" />
    <meta name="RATING" content="GENERAL" />
    <meta http-equiv="PAGE-ENTER" content="RevealTrans(Duration=0,Transition=1)" />
    <style id="StylePlaceholder" type="text/css">
        </style>
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/default.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/skin.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/home.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/sub.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/container.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/Menu.css" />
    <link runat="server" rel="stylesheet" type="text/css" href="~/Content/Shared.css" />
    
    <link runat="server" rel="SHORTCUT ICON" href="~/Content/media/favicon.png" />
    <link rel="SHORTCUT ICON" href="media/favicon.png" />
    <title>Arthur J. Gallagher & Co (Aus) Limited - Home Page </title>
    
    <script type="text/javascript">
    var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
    document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <script type="text/javascript">
    var pageTracker = _gat._getTracker("UA-3474314-1");
    pageTracker._initData();
    pageTracker._trackPageview();
    </script>
    
    <script type="text/javascript">
    <!--
    function filtery(pattern, list, listbak)
    {
        match = new Array();
        for (n=0;n<listbak.length;n++)
        {
            if(listbak[n].text.toLowerCase().indexOf(pattern.toLowerCase()) == 0)
            {
                match[match.length] = new Array(listbak[n].value, listbak[n].text);
            }
        }

        list.options.length = 0;
        for (n=0;n<match.length;n++)
        {
            list[n] = new Option(match[n][1], match[n][0]);
        }

        if (list.length != 0)
        {
            list.selectedIndex = 0;
        }
    }
    //-->
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <table width="843" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="144px">
                <asp:Image ID="imgOAMPSLogo" runat="server" ImageUrl="~/Content/media/gallagher-nrl_logo.jpg" />
            </td>
            <td class="logomiddle">
                <asp:Label ID="lblHeaderTitle" runat="server"></asp:Label></td>
            <td width="144px">
                <asp:Image ID="imgAssociationLogo" runat="server" ImageUrl="" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="skinmaster" border="0" align="center" cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top" width="843">
                            <div>
                                <div class="breadbg">
                                    &nbsp;
                                    <asp:Label ID="lblFormTitle" runat="server" CssClass="headertext"></asp:Label>
                                </div>
                                <div class="con">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                                Please enter the affiliate name or select from the list below:</td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                Insured (search):
                                            </td>
                                            <td width="650px">
                                               <input type="text" id="txtSearch" 
                                                    onkeyup="filtery(this.value,this.form.lstInsured,this.form.ddlInsured)" 
                                                    style="width: 350px" runat="server" />
                                                <br />
                                                <asp:ListBox ID="lstInsured" runat="server" Width="350px"></asp:ListBox>
                                                <asp:DropDownList ID="ddlInsured" runat="server" Height="1px" Width="1px" Enabled="False"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Interested Party (Council):
                                            </td>
                                            <td>
                                                <table style="width:100%;" cellpadding=0 cellspacing=0>
                                                    <tr>
                                                        <td width="100px" valign="baseline">
                                                            <asp:RadioButtonList ID="rblInterestedParty" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                                                OnSelectedIndexChanged="rblInterestedParty_SelectedIndexChanged">
                                                                <asp:ListItem Value="Certificate of Currency 3rd Party">Yes</asp:ListItem>
                                                                <asp:ListItem Value="Certificate of Currency" Selected="True">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td valign="baseline">
                                                            <asp:Image ID="imgInterestedParty" runat="server" ImageUrl="~/Content/media/info16x16.jpg" 
                                                                Title="Does anybody else need to be listed on the document? e.g. a local council or business partner." />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="hidActualPreset" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblInterestedPartyNameLabel" runat="server" Text="Insert Full Council Name Only:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInterestedPartyName" runat="server" MaxLength="100" Width="350px" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Requested By:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRequestedBy" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Email:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Position at Insured:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPositionAtInsured" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
                                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear Data" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="footsl">
                            &nbsp;
                        </td>
                        <td align="left" valign="top" class="footst">
                            <span class="UserHolder">
                                <a href="http://www.oamps.com.au/Default.aspx?tabid=450" class="User">Terms and Conditions</a> |&nbsp; 
                                <a href="http://www.oamps.com.au/Default.aspx?tabid=340" class="User">Privacy Policy</a> </span>
                        </td>
                        <td class="footsr">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center" style="margin: 0; padding: 0">
                  <span class="disclaimer">&copy 2014 Copyright Arthur J. Gallagher & Co (Aus) Limited, 289 Wellington Parade South, East Melbourne
                    Victoria 3002, A.B.N. 34 005 543 920. Any advice does not take into account your personal needs and
                    financial circumstances and you should consider whether it is appropriate for you.</span> 
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
