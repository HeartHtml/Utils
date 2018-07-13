<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RRALauncher._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="jumbotron">
        <h1>RRALauncher App</h1>
        <p><asp:LinkButton runat="server" CssClass="btn btn-primary btn-lg" ID="btnLaunchApp" OnClick="btnLaunchApp_OnClick" Text="Refresh Random File Playlist"></asp:LinkButton></p>
    </div>

</asp:Content>
