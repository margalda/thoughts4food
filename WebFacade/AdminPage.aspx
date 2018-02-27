<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" MasterPageFile="~/SiteWithoutTabs.Master" Inherits="WebFacade.AdminPage" %>

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
            height: 175px;
            margin: 2em;
            width: 200px;
        }

        li { list-style: none; }

        ul { padding: 1em; }

        .form { width: 50em; }

        .form li span {
            float: left;
            font-weight: bold;
            width: 30%;
        }

        .form li input {
            float: left;
            width: 70%;
        }

        .form input { float: right; }

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <body>
    <div>
        <h1>Admin Page</h1>
        <div style="float: left;">
            <h2>Statistics:</h2>
            Number of Users:
            <asp:Label ID="txtUsers" runat="server" Text="0"></asp:Label>
            <br/>
            <br/>
            Number of Recipes:
            <asp:Label ID="txtRecipes" runat="server" Text="0"></asp:Label>
            <br/>
            <br/>
            Number of Ingridients:
            <asp:Label ID="txtIngridients" runat="server" Text="0"></asp:Label>
        </div>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        Update minimum rating :&nbsp;&nbsp;
        <td>
            <asp:TextBox ID="ratingBox" runat="server"/>
        </td>
        <td>
            <asp:RequiredFieldValidator ErrorMessage="Required" Display="Dynamic" ForeColor="Red"
                                        ControlToValidate="ratingBox" runat="server"/>
            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^([1-4](\.[0-9]+)?|5|5.0)$"
                                            ControlToValidate="ratingBox" ForeColor="Red" ErrorMessage="Invalid rating."/>
        </td>
        <br/>
        Update minimum number of raters:&nbsp;&nbsp;
        <td>
            <asp:TextBox ID="numOfRatersBox" runat="server"/>
        </td>
        <td>
            <asp:RequiredFieldValidator ErrorMessage="Required" Display="Dynamic" ForeColor="Red"
                                        ControlToValidate="numOfRatersBox" runat="server"/>
            <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationExpression="^\d+$"
                                            ControlToValidate="numOfRatersBox" ForeColor="Red" ErrorMessage="Invalid number of raters."/>
        </td>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="save" runat="server" Height="24px" OnClick="Save_Click" Text="Save" Width="44px"/>
        <br/>
        <br/>
        <br/>
        <asp:ListView ID="images" runat="server"
                      OnItemDataBound="OnBlobDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"/>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <h2>No Data Available</h2>
            </EmptyDataTemplate>
            <ItemTemplate>
                <div class="item">
                    <ul style="width: 40em; float: left; clear: left">
                        <li>
                            <asp:Repeater ID="blobMetadata" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <%# Eval("Name") %><span><%# Eval("Value") %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </li>
                        <li>
                            <asp:LinkButton ID="deleteBlob"
                                            OnClientClick="return confirm('Delete recipe?');"
                                            CommandName="Delete"
                                            CommandArgument='<%#Eval("Container.Name") + "," + Eval("Name") %>'
                                            runat="server" Text="Delete" OnCommand="OnDeleteImage"/>
                        </li>
                    </ul>
                    <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float: left"/>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </body>
</asp:Content>