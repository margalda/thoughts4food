<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecipePage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="WebFacade.RecipePage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title></title>
    <style type="text/css">
        .Star {
            background-image: url("Resources/Star.gif");
            height: 17px;
            width: 17px;
        }

        .WaitingStar {
            background-image: url("Resources/WaitingStar.gif");
            height: 17px;
            width: 17px;
        }

        .FilledStar {
            background-image: url(Resources/FilledStar.gif);
            height: 17px;
            width: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <body>

    <div style="height: 578px">
        <br/>
        <br/>
        <asp:Label ID="Label1" runat="server" Font-Size="XX-Large" Text="Not Assigned"></asp:Label>
        <br/>
        <hr/>
        <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ScriptManager>
        &nbsp;<asp:Label ID="txtRating" runat="server" Visible="false" Text=""></asp:Label>&nbsp;
        <asp:Label ID="txtNumOfRaters" runat="server" Font-Size="Medium"></asp:Label><asp:Label ID="Label6" runat="server" Text=" raters" Font-Size="Medium"></asp:Label>
        <cc1:Rating ID="Rating1" AutoPostBack="true" OnClick="OnRatingChanged" runat="server"
                    StarCssClass="Star" WaitingStarCssClass="WaitingStar" EmptyStarCssClass="Star"
                    FilledStarCssClass="FilledStar">
        </cc1:Rating>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Image ID="Image1" runat="server" Height="152px" Width="309px"/>
        <br/>
        <br/>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
    </div>

    </body>
</asp:Content>