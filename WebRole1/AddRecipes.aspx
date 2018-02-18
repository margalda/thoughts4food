<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRecipes.aspx.cs" Inherits="WebRole1.AddRecipes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Windows Azure Blob Storage</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <style type="text/css">
        body {
            font-family: Verdana;
            font-size: 12px;
        }

        h1 {
            font-size: x-large;
            font-weight: bold;
        }

        h2 {
            font-size: large;
            font-weight: bold;
        }

        img {
            height: 175px;
            margin: 2em;
            width: 200px;
        }

        li {
            list-style: none;
        }

        ul {
            padding: 1em;
        }

        .form {
            width: 50em;
        }

            .form li span {
                float: left;
                font-weight: bold;
                width: 30%;
            }

            .form li input {
                float: left;
                width: 70%;
            }

            .form input {
                float: right;
            }

        .item {
            font-size: smaller;
            font-weight: bold;
        }

            .item ul li {
                background-color: #ffeecc;
                padding: 0.25em;
            }

                .item ul li span {
                    background-color: #ffffff;
                    display: block;
                    font-style: italic;
                    font-weight: normal;
                    padding: 0.25em;
                }

        .auto-style1 {
            height: 35px;
        }

        .auto-style2 {
            height: 300px;
        }

        .auto-style3 {
            height: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Add a Recipe</h1>
            <div class="form">
                <ul>
                    <li class="auto-style1">
                        <span>Name</span><asp:TextBox ID="recipeName" runat="server" Width="205px" />
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="recipeName"
                            runat="server" />
                    </li>
                    <li class="auto-style1">
                        <span>Preparation Time</span><asp:TextBox ID="timeBox" runat="server" Width="107px" /><asp:DropDownList ID="timeList" runat="server" Width="118px" />
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="timeBox"
                            runat="server" />
                        <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                            ControlToValidate="timeBox" ForeColor="Red" ErrorMessage="Invalid time" />
                    </li>
                    <li class="auto-style1">
                        <span>Kind</span><asp:DropDownList ID="kindList" runat="server" Width="118px" />
                    </li>
                    <li class="auto-style1">
                        <span>Cuisine</span><asp:DropDownList ID="cuisineList" runat="server" Width="118px" />
                    </li>
                    <li class="auto-style2">
                        <div class="auto-style1">
                            <span>Ingredients</span><asp:TextBox ID="name1" runat="server" Width="150px" /><asp:TextBox ID="quantity1" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList1" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name1" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RequiredFieldValidator ErrorMessage="Required at least one" ForeColor="Red" ControlToValidate="name1"
                                runat="server" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity1" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name2" runat="server" Width="150px" /><asp:TextBox ID="quantity2" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList2" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name2" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity2" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name3" runat="server" Width="150px" /><asp:TextBox ID="quantity3" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList3" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name3" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity3" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name4" runat="server" Width="150px" /><asp:TextBox ID="quantity4" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList4" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name4" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity4" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name5" runat="server" Width="150px" /><asp:TextBox ID="quantity5" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList5" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name5" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity5" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name6" runat="server" Width="150px" /><asp:TextBox ID="quantity6" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList6" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name6" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity6" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name7" runat="server" Width="150px" /><asp:TextBox ID="quantity7" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList7" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name7" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity7" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                        <div class="auto-style1">
                            <span></span>
                            <asp:TextBox ID="name8" runat="server" Width="150px" /><asp:TextBox ID="quantity8" runat="server" Width="58px" />
                            <asp:DropDownList ID="measurementList8" runat="server" Width="118px" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                ControlToValidate="name8" ForeColor="Red" ErrorMessage="Invalid name" />
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                ControlToValidate="quantity8" ForeColor="Red" ErrorMessage="Invalid quantity." />
                        </div>
                    </li>
                    <li class="auto-style3">
                        <span>Description</span><asp:TextBox ID="recipeDescription" runat="server" Height="161px" />
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="recipeDescription"
                            runat="server" />
                    </li>
                    <li class="auto-style1">
                        <span>Picture</span><asp:FileUpload ID="imageFile" runat="server" />
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="imageFile"
                            runat="server" />
                    </li>
                </ul>
                <asp:Button ID="upload" runat="server" OnClick="Upload_Click" Text="Upload Recipe" />
            </div>
            <div style="float: left;">
                Status:
            <asp:Label ID="status" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
