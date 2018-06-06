<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAirplus._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/default.css" rel="stylesheet" />
    <div class="jumbotron">
        <h1>GUEST USER LISTS
        </h1>
        <table style="width:100%;">
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            </table>
        <p >
            <asp:PlaceHolder ID="GuestHolder" runat="server"></asp:PlaceHolder>
            &nbsp;</p>
        <p>

            <asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Text="Update" />
            <asp:Button ID="btn_email" runat="server" OnClick="btn_email_Click" Text="Email" />
        </p>
    </div>

</asp:Content>
