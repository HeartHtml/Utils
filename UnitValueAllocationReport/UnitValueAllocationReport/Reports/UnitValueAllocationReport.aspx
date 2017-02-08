<%@ Page Title="UVA Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnitValueAllocationReport.aspx.cs" Inherits="UnitValueAllocationReport.Reports.UnitValueAllocationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            $("#" + '<%= txtReportDate.ClientID %>').datepicker({
                format: "mm-yyyy",
                viewMode: "months",
                minViewMode: "months"
            });

            $("#" + '<%= txtReportDate.ClientID %>').datepicker('update', new Date());

            $("#" + '<%= txtReportDate.ClientID %>').on('changeDate', function (ev) {
                $(this).datepicker('hide');
                if ($("#" + '<%= txtReportDate.ClientID %>').val().length > 0) {
                    $("form").submit();
                }
            });

        });


    </script>

    <h2>Unit Value Allocation Report
    </h2>
    <p>
        Select a month to generate the UVA report for that month. All fields are required.
    </p>
    <fieldset class="form-group">
        <legend>Date for Report</legend>
        <div class="form-group">
            <asp:Label runat="server" ID="lblDateForReport" Text="The month and year to run the UVA report for" AssociatedControlID="txtReportDate" CssClass="control-label"></asp:Label>
            <asp:TextBox runat="server" ID="txtReportDate" data-date-format="mm-yyyy" CssClass="form-control" OnTextChanged="txtReportDate_OnTextChanged"></asp:TextBox>
        </div>
    </fieldset>
    <fieldset class="form-group">
        <legend>Last Month's Unit Value</legend>
        <div class="form-group">
            <asp:Label runat="server" ID="lblPreviousUnitValue" Text="Previous Month Unit Value" AssociatedControlID="txtPreviousUnitValue" CssClass="control-label"></asp:Label>
            <asp:TextBox runat="server" ID="txtPreviousUnitValue" CssClass="form-control" ReadOnly="True" Text="$218.00"></asp:TextBox>
        </div>
    </fieldset>
    <fieldset class="form-group">
        <legend>Expense Cash Flow</legend>
        <div class="form-group">
            <asp:Label runat="server" ID="lblAllocatedExpenseCashFlowFoundation" Text="Allocated Expense Cash Flow - Foundation" AssociatedControlID="txtAllocatedExpenseCashFlowFoundation" CssClass="control-label"></asp:Label>
            <div class="input-group">
                <span class="input-group-addon">$</span>
                <asp:TextBox runat="server" ID="txtAllocatedExpenseCashFlowFoundation" CssClass="form-control" Text="-626962.60"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="lblAllocatedExpenseCashFlowUnrestricted" Text="Allocated Expense Cash Flow - Unrestricted" AssociatedControlID="txtAllocatedExpenseCashFlowUnrestricted" CssClass="control-label"></asp:Label>
            <div class="input-group">
                <span class="input-group-addon">$</span>
                <asp:TextBox runat="server" ID="txtAllocatedExpenseCashFlowUnrestricted" CssClass="form-control" Text="-278042.33"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="lblAllocatedExpenseCashFlowInitiative" Text="Allocated Expense Cash Flow - Talented Students in the Arts Initiative" AssociatedControlID="txtAllocatedExpenseCashFlowInitiative" CssClass="control-label"></asp:Label>
            <div class="input-group">
                <span class="input-group-addon">$</span>
                <asp:TextBox runat="server" ID="txtAllocatedExpenseCashFlowInitiative" CssClass="form-control" Text="-43132.00"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="lblAllocatedExpenseCashFlowNewCampusCampaign" Text="Allocated Expense Cash Flow - New Campus Campaign" AssociatedControlID="txtAllocatedExpenseCashFlowNewCampusCampaign" CssClass="control-label"></asp:Label>
            <div class="input-group">
                <span class="input-group-addon">$</span>
                <asp:TextBox runat="server" ID="txtAllocatedExpenseCashFlowNewCampusCampaign" CssClass="form-control" Text="-317514.67"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="lblAllocatedExpenseCashFlowCulturalEndowment" Text="Allocated Expense Cash Flow - Cultural Endowment" AssociatedControlID="txtAllocatedExpenseCashFlowCulturalEndowment" CssClass="control-label"></asp:Label>
            <div class="input-group">
                <span class="input-group-addon">$</span>
                <asp:TextBox runat="server" ID="txtAllocatedExpenseCashFlowCulturalEndowment" CssClass="form-control" Text="-28961.33"></asp:TextBox>
            </div>
        </div>
    </fieldset>

    <fieldset class="form-group">
        <legend>Non Allocated Cash Flow</legend>
        <div class="form-group col-xs-6">
            <asp:Label runat="server" ID="lblNonAllocatedCashFlowUpload" Text="Upload non-allocated cash flow metrics by endowment to include in the report" AssociatedControlID="fuplCashFlow" CssClass="control-label"></asp:Label>
            <asp:FileUpload runat="server" ID="fuplCashFlow" CssClass="form-control" />
        </div>
    </fieldset>

    <asp:Button runat="server" ID="btnGenerateReport" CssClass="btn btn-primary" Text="Generate Report" OnClick="btnGenerateReport_OnClick" />
    <asp:Button runat="server" ID="btnStartOver" CssClass="btn btn-default" Text="Start Over" OnClick="btnStartOver_OnClick" />

</asp:Content>
