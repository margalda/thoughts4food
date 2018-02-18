<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recipes.aspx.cs" Inherits="WebRole1.Recipes" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Recipes</h1>
            <div style="float: left;">
                Status:
                <asp:Label ID="status" runat="server" />
            </div>
            <asp:ListView ID="images" runat="server"
                OnItemDataBound="OnBlobDataBound">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
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
                                        <li><%# Eval("Name") %><span><%# Eval("Value") %></span></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </li>
                            <li>
                                <asp:LinkButton ID="deleteBlob"
                                    OnClientClick="return confirm('Delete image?');"
                                    CommandName="Delete"
                                    CommandArgument='<%# Eval("Name") %>'
                                    runat="server" Text="Delete" OnCommand="OnDeleteImage" />
                                <asp:LinkButton ID="CopyBlob"
                                    OnClientClick="return confirm('Copy image?');"
                                    CommandName="Copy"
                                    CommandArgument='<%# Eval("Name") %>'
                                    runat="server" Text="Copy" OnCommand="OnCopyImage" />
                                <a href='<%# Eval("uri") %>'>THUMB</a>
                            </li>
                        </ul>
                        <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float: left" />
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
