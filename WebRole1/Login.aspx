<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebRole1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        input {
            width: 200px;
        }

        table {
            border: 1px solid #ccc;
        }

            table th {
                background-color: #F7F7F7;
                color: #333;
                font-weight: bold;
            }

            table th, table td {
                padding: 5px;
                border-color: #ccc;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <th colspan="3">Login
                </th>
            </tr>
            <tr>
                <td>Username
                </td>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtUsername"
                        runat="server" />
                    <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]+$"
                                                    ControlToValidate="txtUsername" ForeColor="Red" ErrorMessage="Invalid Username." />
                </td>
            </tr>
            <tr>
                <td>Password
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtPassword"
                        runat="server" />
                    <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]+$"
                                                    ControlToValidate="txtPassword" ForeColor="Red" ErrorMessage="Invalid Password." />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button Text="Sign in" runat="server" OnClick="Login_Click" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label runat="server">Not a member yet? </asp:Label><asp:HyperLink NavigateUrl="Register.aspx" runat="server">Register</asp:HyperLink>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
