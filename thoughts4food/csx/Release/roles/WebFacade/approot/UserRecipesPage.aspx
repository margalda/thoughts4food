<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRecipesPage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="WebFacade.UserRecipesPage" %>

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
        <h1>User Recipes</h1>
        <asp:ListView ID="images" runat="server"
                      OnItemDataBound="OnBlobDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"/>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <h2>No Recipes Available</h2>
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
                            <asp:HyperLink NavigateUrl='<%# $"RecipePage.aspx?username={Request.QueryString["username"]}&cuisine={Eval("Container.Name")}&name={Eval("Name")}" %>' runat="server">See full description</asp:HyperLink>
                        </li>
                    </ul>
                    <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float: left"/>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>

    </body>
</asp:Content>