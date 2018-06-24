<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="History.aspx.cs" Inherits="WebAirplus.History" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <link href="Content/default.css" rel="stylesheet" />
        <div class="jumbotron">
        <h1>GUEST USER LISTS HISTORY
        </h1>
            <p >
            <asp:PlaceHolder ID="GuestHolder" runat="server"></asp:PlaceHolder>
            &nbsp;</p>
        <p>

            <asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Text="Update" />
            <asp:Button ID="btn_email" runat="server" OnClick="btn_email_Click" Text="Email" />           
            <asp:Button ID="btn_previous" runat="server" Text="Previous" OnClick="btn_previous_Click" />
            <asp:Button ID="btn_next" runat="server" OnClick="btn_next_Click" Text="Next" />
        </p>
        </div>
</asp:Content>
