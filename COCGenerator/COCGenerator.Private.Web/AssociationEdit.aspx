<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AssociationEdit.aspx.cs"
    Inherits="COCGenerator.AssociationEdit" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">Edit Association</span>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
    <div class="con">
        
        <table style="width: 100%;">
            <tr>
                <td width="160px">
                    Association Name:</td>
                <td>
                    <asp:TextBox ID="txtAssociationName" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="160px">
                    Association Logo:</td>
                <td>
                    <asp:FileUpload ID="fulAssociationLogo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="" />
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Save" onclick="btnEdit_Click" />
                    <asp:Button ID="btnBack" runat="server" PostBackUrl="~/AdminHome.aspx" Text="Back" />
                </td>
            </tr>
        </table>
        
    </div>
</asp:Content>
