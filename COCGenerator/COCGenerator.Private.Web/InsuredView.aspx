<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InsuredView.aspx.cs"
    Inherits="COCGenerator.InsuredView" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">View Insured Details</span>
    </div>
    <div class="con">
        <table style="width: 100%;">
            <tr>
                <td colspan="2">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Insured Name:
                </td>
                <td width="690px">
                    <asp:Label ID="lblInsuredName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Account Executive:
                </td>
                <td>
                    <asp:Label ID="lblAccountExecutive" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    OAMPS Email:
                </td>
                <td>
                    <asp:Label ID="lblOAMPSEmail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Association:
                </td>
                <td>
                    <asp:Label ID="lblAssociation" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Insurer:
                </td>
                <td>
                    <asp:Label ID="lblInsurer" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Period of Insurance From:
                </td>
                <td>
                    <asp:Label ID="lblInsurancePeriodStart" runat="server"></asp:Label>
                    &nbsp;<asp:Label ID="lblTimeZoneStart" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    To:
                </td>
                <td>
                    <asp:Label ID="lblInsurancePeriodEnd" runat="server"></asp:Label>
                    &nbsp;<asp:Label ID="lblTimeZoneEnd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Class:
                </td>
                <td>
                    <asp:Label ID="lblClass" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Policy Number:
                </td>
                <td>
                    <asp:Label ID="lblPolicyNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Limit of Indemnity:
                </td>
                <td>
                    <asp:Label ID="lblIndemnityLimit" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Business description and policy notes:
                </td>
                <td valign="top">
                    <asp:Label ID="lblBusinessDescription" runat="server" Width="600px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnBack" runat="server" PostBackUrl="~/AssociationInsureds.aspx" Text="Back" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
