<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnitValueAllocationReport.aspx.cs" Inherits="UnitValueAllocationReport.Reports.UnitValueAllocationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        $("#" + '<%= txtReportDate.ClientID %>').datepicker({
            format: "mm-yyyy",
            viewMode: "months",
            minViewMode: "months"
        });
    </script>

    <h2>Unit Value Allocation Report
    </h2>
    <div class="form-group">
        <asp:Label runat="server" ID="lblDateForReport" Text="Date for Report" AssociatedControlID="txtReportDate" CssClass="control-label"></asp:Label>
        <asp:TextBox runat="server" ID="txtReportDate" data-date-format="mm-yyyy" CssClass="form-control"></asp:TextBox>
    </div>
     <button type="submit" class="btn btn-primary">Generate Report</button>
</asp:Content>
