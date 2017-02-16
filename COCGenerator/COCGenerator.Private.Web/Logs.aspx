<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Logs.aspx.cs"
    Inherits="COCGenerator.Logs" Title="Arthur J. Gallagher & Co (Aus) Limited - Certificate" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadbg">
        <span class="headertext">Logs</span>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
    <div class="con">
        <span class="SubHead">Filter options: </span>
        <asp:RadioButtonList ID="rblFilterOption" runat="server" RepeatColumns="4" AutoPostBack="True">
            <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
            <asp:ListItem Value="1">Yesterday</asp:ListItem>
            <asp:ListItem Value="2">This Week</asp:ListItem>
            <asp:ListItem Value="3">Last Week</asp:ListItem>
            <asp:ListItem Value="4">This Month</asp:ListItem>
            <asp:ListItem Value="5">Last Month</asp:ListItem>
            <asp:ListItem Value="6">All</asp:ListItem>
        </asp:RadioButtonList>
        <asp:DataList ID="dalLogs" runat="server" DataKeyField="ExceptionLogId" 
            DataSourceID="ObjectDataSource1">
            <ItemTemplate>
                <table style="width:100%; border-color:#6e8d82; border-width: 1px; border-style: solid" >
                    <tr>
                        <td width="120px" valign="top">
                            Short Description:</td>
                        <td colspan="3">
                            <asp:Label ID="lblShortDescription" Text='<%# Eval("ShortDescription") %>' runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Long Description:</td>
                        <td colspan="3">
                            <asp:Label ID="lblLongDescription" Text='<%# Eval("LongDescription") %>' runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Created Date:
                        </td>
                        <td width="300px">
                            <asp:Label ID="lblCreatedDate" Text='<%# Eval("CreatedDate") %>' runat="server"></asp:Label>
                        </td>
                        <td width="120px">
                            Created By:</td>
                        <td width="300px">
                            <asp:Label ID="lblCreatedBy" Text='<%# Eval("CreatedBy") %>' runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ListLogs" 
            TypeName="COCGenerator.BusinesObjects.bizLog">
            <SelectParameters>
                <asp:ControlParameter ControlID="rblFilterOption" Name="Filter" PropertyName="SelectedValue" 
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <div>
        <asp:Button ID="btnBack" PostBackUrl="~/AdminHome.aspx" runat="server" Text="Back" />
    </div>
</asp:Content>
