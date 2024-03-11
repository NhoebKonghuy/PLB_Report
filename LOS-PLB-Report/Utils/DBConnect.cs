using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
using System.Data;
using System.Web;
using Npgsql;

namespace LOS_PLB_Report
{
    public class DBConnect
    {
        private NpgsqlConnection conn;
        public DBConnect()
        {
            posgresIntailize();
        }

        private void posgresIntailize()
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["plb"].ConnectionString;
            conn = new NpgsqlConnection(connStr);
            openConnection();
        }

        private bool openConnection()
        {
            conn.Open();
            return true;
        }

        private bool closeConnection()
        {
            conn.Close();
            return true;
        }
        public DataTable getPostgreSQLDataTable(string sql)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public void generateReport(ReportViewer reportViewer, string reportName, ReportParameter[] parameters, params ReportDataSource[] reportDataSources)
        {
            reportViewer.SizeToReportContent = true;
            reportViewer.LocalReport.ReportPath = HttpContext.Current.Server.MapPath(String.Format("~/Reports/{0}.rdlc", reportName));
            reportViewer.LocalReport.DataSources.Clear();

            foreach (var item in reportDataSources)
            {
                System.Diagnostics.Debug.WriteLine("item name: " + item.Name);
                reportViewer.LocalReport.DataSources.Add(item);
            }

            reportViewer.LocalReport.Refresh();
        }

        public void subReportDataSource(SubreportProcessingEventArgs e, params ReportDataSource[] reportDataSource)
        {
            foreach (var item in reportDataSource)
            {
                if (item != null)
                {
                    e.DataSources.Add(item);
                }
            }
        }

      
    }
}