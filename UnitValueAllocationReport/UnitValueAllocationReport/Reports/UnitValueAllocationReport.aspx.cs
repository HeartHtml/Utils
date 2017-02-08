using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UnitValueAllocationReport.Reports
{
    public partial class UnitValueAllocationReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtReportDate_OnTextChanged(object sender, EventArgs e)
        {
        }

        protected void btnStartOver_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/UnitValueAllocationReport.aspx");
        }

        protected void btnGenerateReport_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/UnitValueAllocationProcess.aspx");
        }
    }
}