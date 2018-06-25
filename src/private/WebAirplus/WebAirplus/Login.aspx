<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebAirplus.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table style="width:100%;">
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="User_lbl" runat="server" Text="UserID :" ></asp:Label></td>
                    <td>
                        <asp:TextBox ID="User_Txt" runat="server"></asp:TextBox></td>
                    <td></td>
                </tr>                
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="Pass_lbl" runat="server" Text="Password"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Pass_Txt" runat="server" TextMode="Password"></asp:TextBox></td>
                    <td></td>
                </tr>                
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="Submit_btn" runat="server" Text="Login" OnClick="Submit_btn_Click" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Label ID="error_lbl" runat="server" Text=""></asp:Label></td>
                    <td></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>