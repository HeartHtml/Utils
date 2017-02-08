using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UnitValueAllocationReport.Reports
{
    public partial class UnitValueAllocationProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnStartOver_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/UnitValueAllocationReport.aspx");
        }

        protected void btnPersistToLedger_OnClick(object sender, EventArgs e)
        {
            plhPersistToLedger.Visible = false;

            plhAlreadyPersisted.Visible = true;

            ltAlreadyPersisted.Text = string.Format("UVA Report persisted to ledger on {0}",
                DateTime.Now);
        }

        protected void btnDownloadReport_OnClick(object sender, EventArgs e)
        {
            FileInfo info = new FileInfo(Server.MapPath("~/Reports/2015.6.30 NWS UnitValueAllocation.xls"));
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + info.Name);
            Response.AddHeader("Content-Length", info.Length.ToString(CultureInfo.InvariantCulture));
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(info.FullName);
            Response.End();
        }
    }
}