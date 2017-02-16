<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PresetEdit.aspx.cs"
    Inherits="COCGenerator.PresetEdit" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register Assembly="COCGenerator.Private.Web" Namespace="WebControls.NumericBox" TagPrefix="pwc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">Edit Preset</span>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
    <div class="con">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table style="width: 100%;">
            <tr>
                <td width="160px">
                    Association:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssociations" runat="server" DataSourceID="ObjectDataSource1" DataTextField="AssociationName"
                        DataValueField="AssociationID">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ListAssociations" TypeName="COCGenerator.BusinesObjects.bizAssociation">
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td width="160px">
                    Document Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlDocumentType" runat="server">
                        <asp:ListItem>Certificate of Currency</asp:ListItem>
                        <asp:ListItem>Certificate of Currency 3rd Party</asp:ListItem>
                        <asp:ListItem>Confirmation of Insurance</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="160px">
                    Template:
                </td>
                <td>
                    <asp:Label ID="lblTemplateName" runat="server"></asp:Label>
                    &nbsp;<asp:FileUpload ID="fulTemplate" runat="server" />
                    <asp:Button ID="btnThumbnail" runat="server" OnClick="btnThumbnail_Click" Text="Refresh" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Panel ID="thumbnails" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Save" />
                    <asp:Button ID="btnBack" runat="server" PostBackUrl="~/PresetView.aspx" Text="Back" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
