<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UnitValueAllocationReport._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>NWS Reporting Dashboard</h1>
        <p class="lead">This tool facilitates the creation of accounting reports in the New World Symphony Organization.</p>
        <p><a href="<%= ResolveClientUrl("~/Account/Login.aspx") %>" class="btn btn-primary btn-lg">Log In</a></p>
    </div>

</asp:Content>
