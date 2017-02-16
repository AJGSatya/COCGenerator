<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InsuredEdit.aspx.cs"
    Inherits="COCGenerator.InsuredEdit" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="COCGenerator.Private.Web" namespace="WebControls.NumericBox" tagprefix="pwc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">Edit Insured</span>
    </div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
    <div class="con">  
        <table style="width: 100%;">
            <tr>
                <td>
                    Insured Name:
                </td>
                <td width="691px">
                    <asp:TextBox ID="txtInsuredName" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Association:</td>
                <td>
                    <asp:DropDownList ID="ddlAssociations" runat="server" DataSourceID="ObjectDataSource1" DataTextField="AssociationName"
                        DataValueField="AssociationID">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ListAssociations" TypeName="COCGenerator.BusinesObjects.bizAssociation">
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    Account Executive:
                </td>
                <td>
                    <asp:TextBox ID="txtAccountExecutive" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    OAMPS Email:
                </td>
                <td>
                    <asp:TextBox ID="txtOAMPSEmail" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Insurer:
                </td>
                <td>
                    <asp:TextBox ID="txtInsurer" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Period of Insurance From:
                </td>
                <td>
                    <asp:TextBox ID="txtStart" runat="server" MaxLength="10" Width="72px"></asp:TextBox>
                    <asp:DropDownList ID="ddlStartHour" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStartAMPM" runat="server">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                    <cc1:CalendarExtender ID="cexStart" runat="server" TargetControlID="txtStart" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    To:
                </td>
                <td>
                    <asp:TextBox ID="txtEnd" runat="server" MaxLength="10" Width="72px"></asp:TextBox>
                    <asp:DropDownList ID="ddlEndHour" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEndAMPM" runat="server">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                    <cc1:CalendarExtender ID="cexEnd" runat="server" TargetControlID="txtEnd" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Time Zone:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTimeZone" runat="server" DataSourceID="ObjectDataSource2" DataTextField="TimeZoneName"
                        DataValueField="TimeZoneName">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="ListTimeZones" TypeName="COCGenerator.BusinesObjects.bizTimeZone">
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    Class:
                </td>
                <td>
                    <asp:TextBox ID="txtClass" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Policy Number:
                </td>
                <td>
                    <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Limit of Indemnity ($):
                </td>
                <td>
                    <pwc:NumberBox ID="txtIndemnityLimit" runat="server" MaxAmount="1000000000" MinAmount="0" 
                        Width="100px"></pwc:NumberBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Business description and policy notes:</td>
                <td valign="top">
                    <asp:TextBox ID="txtBusinessDescription" runat="server" Height="128px" MaxLength="1000" 
                        TextMode="MultiLine" Width="600px" CssClass="multiline"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Save" onclick="btnEdit_Click" />
                    <asp:Button ID="btnBack" runat="server" PostBackUrl="~/AssociationInsureds.aspx" Text="Back" />
                </td>
            </tr>
        </table> 
    </div>
</asp:Content>
