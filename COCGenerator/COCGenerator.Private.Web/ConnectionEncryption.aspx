<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConnectionEncryption"
    Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" CodeBehind="ConnectionEncryption.aspx.cs" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">
    Configuration API</asp:Content>

<asp:Content ID="DescriptionContent" ContentPlaceHolderID="Description" Runat="server">
This page uses the configuration API to list pertinent configuration sections and allow them to be encrypted and decrypted.
After encrypting or decrypting a section, click the Refresh button to refresh the view.</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <asp:Button ID="Button1" Runat="server" Width="158px" Text="Refresh" Height="24px"
        OnClick="Button1_Click" />
    <br /><br />
    <asp:GridView ID="GridView1" Runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" BorderWidth="2px" BackColor="White" GridLines="None" CellPadding="3" CellSpacing="1" BorderStyle="Ridge" BorderColor="White">
        <FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
        <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#C6C3C6"></PagerStyle>
        <HeaderStyle ForeColor="#E7E7FF" Font-Bold="True" BackColor="#4A3C8C"></HeaderStyle>
        <Columns>
            <asp:BoundField HeaderText="Section" DataField="Name"></asp:BoundField>
            <asp:BoundField HeaderText="Scope" DataField="Scope"></asp:BoundField>
            <asp:CheckBoxField HeaderText="Declared?" DataField="IsDeclared">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:CheckBoxField>
            <asp:CheckBoxField HeaderText="Protected?" DataField="IsProtected">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:CheckBoxField>
            <asp:TemplateField HeaderText="Action">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" Text='<%# (bool) Eval ("IsProtected") ? "Decrypt" : "Encrypt" %>'
                        CommandName='<%# (bool) Eval ("IsProtected") ? "Decrypt" : "Encrypt" %>'
                        CommandArgument='<%# Eval ("Name") %>' Runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE"></SelectedRowStyle>
        <RowStyle ForeColor="Black" BackColor="#DEDFDE"></RowStyle>
    </asp:GridView></asp:Content>