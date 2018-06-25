<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="WebAirplus.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder ID="SettingHolder" runat="server"></asp:PlaceHolder>
    <br />
    <asp:Button ID="EditBtn" runat="server" Text="Edit" OnClick="EditBtn_Click" />
    <asp:Button ID="UpdateBtn" runat="server" Text="Update" Enabled="False" OnClick="UpdateBtn_Click" Visible="False" />
    <asp:Button ID="CancelBtn" runat="server" Text="Cancel" Enabled="False" OnClick="CancelBtn_Click" Visible="False" />
</asp:Content>
