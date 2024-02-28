using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace LOS_PLB_Report.Reports
{
    public partial class Individual_Customer_Information : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];

                var sqlBusDetail = @"SELECT ID FROM APP_APPLICATION WHERE application_no = '" + applicationNo + "'";

                var dtBusDetail = conn.getPostgreSQLDataTable(sqlBusDetail);

                var dsBusDetail = new ReportDataSource("DS_INFORMATION_APPLICANT", dtBusDetail);

                conn.generateReport(ReportViewer1, @"Individual_Customer_Information", null, dsBusDetail);

            }
        }
    }
}