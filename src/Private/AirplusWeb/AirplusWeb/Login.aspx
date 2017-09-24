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
                <asp:Table>
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
        <asp:Login ID="Login1" runat="server" BackColor="#F7F7DE" BorderColor="#CCCC99" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="10pt">
            <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
        </asp:Login>
    </form>
</body>
</html>
