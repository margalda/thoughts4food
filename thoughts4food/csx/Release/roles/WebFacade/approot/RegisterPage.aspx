<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterPage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="WebFacade.RegisterPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        input { width: 200px; }

        table { border: 1px solid #ccc; }

        table th {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }

        table th, table td {
            padding: 5px;
            border-color: #ccc;
        }

        .auto-style2 { width: 186px; }

        .auto-style3 {
            width: 520px;
            margin-left: 272px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <body>

    <table class="auto-style3">
        <tr>
            <th colspan="3">
                Registration
            </th>
        </tr>
        <tr>
            <td>
                Username
            </td>
            <td>
                <asp:TextBox ID="txtUsername" runat="server"/>
            </td>
            <td class="auto-style2">
                <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtUsername"
                                            runat="server"/>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]+$"
                                                ControlToValidate="txtUsername" ForeColor="Red" ErrorMessage="Invalid Username."/>
            </td>
        </tr>
        <tr>
            <td>
                Password
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"/>
            </td>
            <td class="auto-style2">
                <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtPassword"
                                            runat="server"/>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]+$"
                                                ControlToValidate="txtPassword" ForeColor="Red" ErrorMessage="Invalid Password."/>
            </td>
        </tr>
        <tr>
            <td>
                Confirm Password
            </td>
            <td>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"/>
            </td>
            <td class="auto-style2">
                <asp:CompareValidator ErrorMessage="Passwords do not match." ForeColor="Red" ControlToCompare="txtPassword"
                                      ControlToValidate="txtConfirmPassword" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                Email
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"/>
            </td>
            <td class="auto-style2">
                <asp:RequiredFieldValidator ErrorMessage="Required" Display="Dynamic" ForeColor="Red"
                                            ControlToValidate="txtEmail" runat="server"/>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid email address."/>
            </td>
        </tr>
        <tr>
            <td>
                Age
            </td>
            <td>
                <asp:TextBox ID="txtAge" runat="server"/>
            </td>
            <td class="auto-style2">
                <asp:RequiredFieldValidator ErrorMessage="Required" Display="Dynamic" ForeColor="Red"
                                            ControlToValidate="txtAge" runat="server"/>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                                ControlToValidate="txtAge" ForeColor="Red" ErrorMessage="Invalid age."/>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button Text="Sign up" runat="server" OnClick="Register_Click"/>
            </td>
            <td class="auto-style2"></td>
        </tr>
    </table>

    </body>
</asp:Content>