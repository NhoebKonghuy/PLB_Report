using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using WebGrease.Css.Ast.Selectors;

namespace LOS_PLB_Report.Reports
{
    public partial class Report : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];

                var CO_BORROWER_3 = @"select * from app_application ap where ap.application_no='"+applicationNo+"'";

                var dtCO_BORROWER_3 = conn.getPostgreSQLDataTable(CO_BORROWER_3);


                
                var dsCO_BORROWER_3 = new ReportDataSource("DataSet1", dtCO_BORROWER_3);


                conn.generateReport(ReportViewer1, @"Report", null, dsCO_BORROWER_3);

            }
        }
    }
}