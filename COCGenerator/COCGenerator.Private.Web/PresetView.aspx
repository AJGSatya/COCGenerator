<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PresetView.aspx.cs"
    Inherits="COCGenerator.PresetView" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="COCGenerator.Private.Web" Namespace="WebControls.NumericBox" TagPrefix="pwc" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">View Preset Details</span>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
    <div class="con">
        <div class="con">
            <table style="width: 100%;">
                <tr>
                    <td width="145px">
                        Preset ID:
                    </td>
                    <td>
                        <asp:Label ID="lblPresetID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="145px">
                        Association:</td>
                    <td>
                        <asp:Label ID="lblAssociation" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Document Type:
                    </td>
                    <td>
                        <asp:Label ID="lblDocumentType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Template Name:
                    </td>
                    <td>
                        <asp:Label ID="lblTemplateName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Panel ID="thumbnails" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" PostBackUrl="~/AssociationPresets.aspx" Text="Back" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
