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

        .auto-style4 {
            width: 76.6em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Add a Recipe</h1>
            <div class="auto-style4">
                <ul>
                    <li class="auto-style1">
                        <span>Name</span><asp:TextBox ID="recipeName" runat="server" Width="205px" />
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="recipeName"
                            runat="server" />
                    </li>
                    <li class="auto-style1">
                        <span>Preparation Time</span><asp:TextBox ID="timeBox" runat="server" Width="107px" /><asp:Label ID="timeUnit" runat="server" Width="118px">(minutes)</asp:Label>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ForeColor="Red" ControlToValidate="timeBox"
                            runat="server" />
                        <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9]+(\.?[0-9]+)?$"
                            ControlToValidate="timeBox" ForeColor="Red" ErrorMessage="Invalid time" />
                    </li>
                    <li class="auto-style1">
                        <span>Cuisine</span><asp:DropDownList ID="cuisineList" runat="server" Width="118px" />
                    </li>
                    <li>
                        <div class="auto-style1">
                            <span>Ingredients</span>
                        </div>
                        <asp:GridView ID="Gridview1" runat="server" ShowFooter="true"
                            AutoGenerateColumns="false" OnRowCreated="Gridview1_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="RowNumber" />
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" ValidationGroup="first"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="categoryList" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="CategoryList_SelectedIndexChanged">
                                            <asp:ListItem Value="SaucesAndSpices">SaucesAndSpices</asp:ListItem>
                                            <asp:ListItem Value="MeatAndPasta">MeatAndPasta</asp:ListItem>
                                            <asp:ListItem Value="SnacksAndCookies">SnacksAndCookies</asp:ListItem>
                                            <asp:ListItem Value="Baking">Baking</asp:ListItem>
                                            <asp:ListItem Value="Dairy">Dairy</asp:ListItem>
                                            <asp:ListItem Value="FruitsAndVegetables">FruitsAndVegetables</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" ValidationGroup="first"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="txtUnit" runat="server">(teaspoons)</asp:Label>
                                        <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z\s]+$"
                                            ControlToValidate="txtName" ForeColor="Red" ErrorMessage="Invalid name" />
                                        <asp:RequiredFieldValidator ErrorMessage="Please fill row" ForeColor="Red" ControlToValidate="txtName"
                                            runat="server" />
                                        <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9]+(\.?[0-9]+)?$"
                                            ControlToValidate="txtQuantity" ForeColor="Red" ErrorMessage="Invalid quantity." />
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Button ID="ButtonAdd" runat="server" Text="Add Ingredient"
                                            OnClick="ButtonAdd_Click" ValidationGroup="grid" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ValidationGroup="grid">Remove</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </li>
                    <li class="auto-style3">
                        <span>Description</span><asp:TextBox ID="recipeDescription" runat="server" TextMode="MultiLine" Height="161px" Width="568px" />
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
        </div>
    </form>
</body>
</html>
