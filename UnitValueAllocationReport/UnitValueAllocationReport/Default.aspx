<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UnitValueAllocationReport._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>NWS Reporting Dashboard</h1>
        <p class="lead">This tool facilitates the creation of accounting reports in the New World Symphony Organization.</p>
        <p>
            <asp:LoginView runat="server" ViewStateMode="Disabled">
                <AnonymousTemplate>
                    <asp:Button runat="server" ID="btnNotLoggedIn" PostBackUrl="~/Account/Login.aspx" CssClass="btn btn-primary" Text="Log In"/>
                </AnonymousTemplate>
                <LoggedInTemplate>
                    <asp:Button runat="server" ID="btnNotLoggedIn" PostBackUrl="~/Reports/UnitValueAllocationReport.aspx" CssClass="btn btn-primary" Text="Go to Reports"/>
                </LoggedInTemplate>
            </asp:LoginView>
        </p>
    </div>

</asp:Content>
