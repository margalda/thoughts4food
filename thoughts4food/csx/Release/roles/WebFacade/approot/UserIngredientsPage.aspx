<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserIngredientsPage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="WebFacade.UserIngredientsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Windows Azure Blob Storage</title>
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
            width: 200px;
            height: 175px;
            margin: 2em;
        }

        li { list-style: none; }

        ul { padding: 1em; }

        .form { width: 50em; }

        .form li span {
            width: 30%;
            float: left;
            font-weight: bold;
        }

        .form li input {
            width: 70%;
            float: left;
        }

        .form input { float: right; }

        .item {
            font-size: smaller;
            font-weight: bold;
        }

        .item ul li {
            padding: 0.25em;
            background-color: #ffeecc;
        }

        .item ul li span {
            padding: 0.25em;
            background-color: #ffffff;
            display: block;
            font-style: italic;
            font-weight: normal;
        }

        .auto-style1 {
            width: 55.6em;
            height: 303px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <body>

    <div>
        <h1>Users Ingredients</h1>
        <asp:GridView ID="GridView2" runat="server" OnRowDeleting="GridView2_OnRowDeleting">
            <EmptyDataTemplate>
                <h2>No Ingredients Available</h2>
            </EmptyDataTemplate>
            <Columns>
                <asp:CommandField ShowDeleteButton="true"/>
            </Columns>
        </asp:GridView>
        <h1>Add Ingedients</h1>
        <div class="auto-style1">
            <asp:GridView ID="Gridview1" runat="server" ShowFooter="true"
                          AutoGenerateColumns="false" OnRowCreated="Gridview1_RowCreated">
                <Columns>
                    <asp:BoundField DataField="RowNumber"/>
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
                                                            ControlToValidate="txtName" ForeColor="Red" ErrorMessage="Invalid name"/>
                            <asp:RequiredFieldValidator ErrorMessage="Please fill row" ForeColor="Red" ControlToValidate="txtName"
                                                        runat="server"/>
                            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^[0-9]+(\.?[0-9]+)?$"
                                                            ControlToValidate="txtQuantity" ForeColor="Red" ErrorMessage="Invalid quantity."/>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                        <FooterTemplate>
                            <asp:Button ID="ButtonAdd" runat="server" Text="Add Ingredient"
                                        OnClick="ButtonAdd_Click" ValidationGroup="grid"/>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ValidationGroup="grid">Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="upload" runat="server" Text="Upload Ingredients" OnClick="Upload_Click"/>
        </div>
    </div>

    </body>
</asp:Content>