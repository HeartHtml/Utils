<%@ Page Title="Process" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnitValueAllocationProcess.aspx.cs" Inherits="UnitValueAllocationReport.Reports.UnitValueAllocationProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>UVA Report generated successfully.
    </h3>

    <div class="table-responsive">
        <table class="table">
            <thead>
                <tr>
                    <th>Download</th>
                    <th>Report Name</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <asp:Button runat="server" class="btn btn-default btn-sm" Text="Download" ID="btnDownloadReport" OnClick="btnDownloadReport_OnClick" />
                    </td>
                    <td>2015.06.30 NWS UnitValueAllocation.xls</td>
                </tr>
            </tbody>
        </table>
    </div>

    <fieldset class="form-group">
        <legend>Save Report</legend>
        <div class="form-group">
            <asp:PlaceHolder runat="server" ID="plhPersistToLedger">
                <asp:Label runat="server" ID="lblPersistToLedger" Text="Click below to persist this report. This can only be done once." AssociatedControlID="btnPersistToLedger" CssClass="control-label"></asp:Label>
                <br/>
                <asp:Button runat="server" ID="btnPersistToLedger" CssClass="btn btn-danger" Text="Persist to General Ledger" OnClick="btnPersistToLedger_OnClick" />
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plhAlreadyPersisted" Visible="False">
                <div class="alert alert-success">
                    <strong>Success!</strong>
                    <asp:Literal ID="ltAlreadyPersisted" runat="server" />
                </div>
            </asp:PlaceHolder>
        </div>
    </fieldset>

    <asp:Button runat="server" ID="btnStartOver" CssClass="btn btn-success" Text="Go Back to the UVA Dashboard" OnClick="btnStartOver_OnClick" />

</asp:Content>
