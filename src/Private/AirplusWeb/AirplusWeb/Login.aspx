<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AirplusWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="Panel1" runat="server" Height="342px" Width="810px" HorizontalAlign="Center">
                <asp:Table ID="RegisterTable" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label_User" runat="server" Text="User Name"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="TextBox_User" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Label ID="Label_Register" runat="server" Text="Registration Page"></asp:Label>
            </asp:Panel>
        </div>
        <asp:Panel ID="LoginPanel" runat="server">
            <asp:Table ID="LoginTable" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="LoginUserlbl" runat="server" Text="User Name"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="LoginUsertxt" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="LoginPasswordlbl" runat="server" Text="Password"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="LoginPasswordtxt" runat="server" TextMode="Password"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="Loginbtn" runat="server" Text="Submit" OnClick="Loginbtn_click"/>
                    </asp:TableCell>
                </asp:TableRow>
                </asp:Table>
        </asp:Panel>
    </form>
</body>
</html>
