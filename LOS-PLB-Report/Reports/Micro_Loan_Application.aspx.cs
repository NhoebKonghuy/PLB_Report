using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace LOS_PLB_Report.Reports
{
    public partial class Micro_Loan_Application : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                var applicationNo = Request.QueryString["application_no"];
                var sqlCustomer = @"SELECT
                    app.application_no,
                    cc.family_name_kh || ' ' || cc.given_name_kh AS fullname,
                    aci.id_number,
                    ait.name,
                    cc.phone_number 
                FROM
                    app_application app
                    INNER JOIN app_application_detail apd ON apd.application_id = app.ID 
                    INNER JOIN cus_customer cc ON cc.ID = apd.customer_id
                    INNER JOIN app_customer_identification aci ON aci.customer_id = cc.ID 
                    AND aci.application_id = app.ID
                    INNER JOIN adm_identification_type ait ON ait.ID = aci.identification_type_id  WHERE app.application_no = '" + applicationNo + "'";
                var sqlSub1 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asd.ID ) AS num,
									ap.application_no,
									asd.full_name_kh,
									ait.name,
									ai.id_number,
									cc.phone_number
		
		
								FROM
									app_application ap
									 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'CO_BORROWER'
									inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
									AND asd.status = 't'
									inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
									inner join app_customer_identification ai on ai.customer_id = cc.id and ai.application_id = ap.id
									INNER JOIN adm_identification_type ait on ait.id = ai.identification_type_id
		
								WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 1";
                var sqlSub2 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asd.ID ) AS num,
									ap.application_no,
									asd.full_name_kh,
									ait.name,
									ai.id_number,
									cc.phone_number
		
		
								FROM
									app_application ap
									 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'CO_BORROWER'
									inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
									AND asd.status = 't'
									inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
									inner join app_customer_identification ai on ai.customer_id = cc.id and ai.application_id = ap.id
									INNER JOIN adm_identification_type ait on ait.id = ai.identification_type_id
		
								WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 2";
                var sqlSub3 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asd.ID ) AS num,
									ap.application_no,
									asd.full_name_kh,
									ait.name,
									ai.id_number,
									cc.phone_number
		
		
								FROM
									app_application ap
									 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'CO_BORROWER'
									inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
									AND asd.status = 't'
									inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
									inner join app_customer_identification ai on ai.customer_id = cc.id and ai.application_id = ap.id
									INNER JOIN adm_identification_type ait on ait.id = ai.identification_type_id
		
								WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 3";
                var sqlEmployee = @"SELECT
									ap.application_no,
									cir.company_name,
									cir.length_of_business,
									cir.primary_income,
									cir.position,
									cir.other_benefit 
								FROM
									app_application ap
									INNER JOIN app_application_detail apd ON apd.application_id = ap.
									ID INNER JOIN cus_customer cc ON apd.customer_id = cc.
									ID INNER JOIN app_customer_income_employee cir ON cir.application_id = ap.ID and cc.id = cir.customer_id WHERE ap.application_no = '" + applicationNo + "'";
                var sqlSubEmp1 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.company_name,
										cir.length_of_business,
										cir.primary_income,
										cir.position,
										cir.other_benefit 
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_employee cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									    WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 1";
                var sqlSubEmp2 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.company_name,
										cir.length_of_business,
										cir.primary_income,
										cir.position,
										cir.other_benefit 
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_employee cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									    WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 2"; ;
                var sqlSubEmp3 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.company_name,
										cir.length_of_business,
										cir.primary_income,
										cir.position,
										cir.other_benefit 
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_employee cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									    WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 3";
                var sqlSubBus1 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.career,
										cir.length_of_business,
										cir.primary_income,
										'' as other_income
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_business cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 1";
                var sqlSubBus2 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.career,
										cir.length_of_business,
										cir.primary_income,
										'' as other_income
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_business cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 2";
                var sqlSubBus3 = @"SELECT
									* 
								FROM
									(
									SELECT ROW_NUMBER
										( ) OVER ( ORDER BY asd.ID ) AS num,
										ap.application_no,
										cir.career,
										cir.length_of_business,
										cir.primary_income,
										'' as other_income
									FROM
										app_application ap
										 inner JOIN app_supplementary asu ON ap.ID = asu.application_id 
										AND asu.status = 't' 
										AND asu.customer_type = 'CO_BORROWER'
										inner JOIN app_supplementary_detail asd ON asd.supplementary_id = asu.ID 
										AND asd.status = 't'
										inner JOIN cus_customer cc ON cc.ID = asd.customer_id 
										INNER JOIN app_customer_income_business cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
		
									WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 3";
                var sqlOwnBus = @"SELECT
								ap.application_no,
									cir.career,
									cir.length_of_business,
									cir.primary_income,
									'' as other_income 
							FROM
								app_application ap
								INNER JOIN app_application_detail apd ON apd.application_id = ap.
								ID INNER JOIN cus_customer cc ON apd.customer_id = cc.
								ID INNER JOIN app_customer_income_business cir ON cir.application_id = ap.ID and cc.id = cir.customer_id 
								WHERE ap.application_no = '" + applicationNo + "'";
                var sqlGuarantor1 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asu.ID ) AS num,
									ap.application_no,
									ag.full_name_kh,
									ait.NAME,
									ai.id_number,
									cc.phone_number,
									cir.career,
									cir.length_of_business,
									cir.primary_income,
									'' AS other_income 
								FROM
									app_application ap
									INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'PERSONAL_GUARANTOR'
									INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
									ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
									INNER JOIN app_customer_identification ai ON ai.customer_id = cc.ID 
									AND ai.application_id = ap.
									ID INNER JOIN adm_identification_type ait ON ait.ID = ai.identification_type_id
									LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
									AND cc.ID = cir.customer_id 
								   WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 1";
                var sqlGuarantor2 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asu.ID ) AS num,
									ap.application_no,
									ag.full_name_kh,
									ait.NAME,
									ai.id_number,
									cc.phone_number,
									cir.career,
									cir.length_of_business,
									cir.primary_income,
									'' AS other_income 
								FROM
									app_application ap
									INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'PERSONAL_GUARANTOR'
									INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
									ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
									INNER JOIN app_customer_identification ai ON ai.customer_id = cc.ID 
									AND ai.application_id = ap.
									ID INNER JOIN adm_identification_type ait ON ait.ID = ai.identification_type_id
									LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
									AND cc.ID = cir.customer_id 
								   WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 2";
                var sqlGuarantor3 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asu.ID ) AS num,
									ap.application_no,
									ag.full_name_kh,
									ait.NAME,
									ai.id_number,
									cc.phone_number,
									cir.career,
									cir.length_of_business,
									cir.primary_income,
									'' AS other_income 
								FROM
									app_application ap
									INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'PERSONAL_GUARANTOR'
									INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
									ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
									INNER JOIN app_customer_identification ai ON ai.customer_id = cc.ID 
									AND ai.application_id = ap.
									ID INNER JOIN adm_identification_type ait ON ait.ID = ai.identification_type_id
									LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
									AND cc.ID = cir.customer_id 
								   WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 3";
                var sqlGuarantor4 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY asu.ID ) AS num,
									ap.application_no,
									ag.full_name_kh,
									ait.NAME,
									ai.id_number,
									cc.phone_number,
									cir.career,
									cir.length_of_business,
									cir.primary_income,
									'' AS other_income 
								FROM
									app_application ap
									INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
									AND asu.status = 't' 
									AND asu.customer_type = 'PERSONAL_GUARANTOR'
									INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
									ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
									INNER JOIN app_customer_identification ai ON ai.customer_id = cc.ID 
									AND ai.application_id = ap.
									ID INNER JOIN adm_identification_type ait ON ait.ID = ai.identification_type_id
									LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
									AND cc.ID = cir.customer_id 
								   WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 4";
                var sqlLoan = @"SELECT
							ap.application_no,
							ac.currency,
							apd.applied_amount,
							apd.tenor,
							apd.annual_interest_rate,
							art.name
	
						FROM
							app_application ap
							INNER JOIN app_application_detail apd ON apd.application_id = apd.
							ID INNER JOIN adm_currency ac ON ac.ID = apd.currency_id
							INNER JOIN adm_repayment_type art on art.id = apd.repayment_type_id
						WHERE ap.application_no = '" + applicationNo + "'";
				var sqlCol = @"SELECT 

								AP.APPLICATION_NO ,

								COL.DOCUMENT_TITLE ,

								COL.COLLATERAL_NO ,

								COL.OWNER_NAME ,

								SSU.NAME AS ISSUE_BY ,

								CCD.MARKET_VALUE

								FROM APP_APPLICATION AP 
								INNER JOIN APP_COL_COLLATERAL_ASSET CCA ON CCA.APPLICATION_ID = AP.ID AND CCA.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = CCA.ID AND CS.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
								LEFT JOIN ADM_ISSUE_BY SSU ON SSU.ID = CCD.ISSUE_BY_ID
								WHERE AP.application_no = 'B-0002-A88322'";
                var sqlBusDetail = @"SELECT ID FROM APP_APPLICATION WHERE application_no = '" + applicationNo + "'";

                var dtBusDetail = conn.getPostgreSQLDataTable(sqlBusDetail);
                var dtCustomer = conn.getPostgreSQLDataTable(sqlCustomer);
                var dtSub1 = conn.getPostgreSQLDataTable(sqlSub1);
                var dtSub2 = conn.getPostgreSQLDataTable(sqlSub2);
                var dtSub3 = conn.getPostgreSQLDataTable(sqlSub3);
                var dtEmployee = conn.getPostgreSQLDataTable(sqlEmployee);
                var dtSubEmp1 = conn.getPostgreSQLDataTable(sqlSubEmp1);
                var dtSubEmp2 = conn.getPostgreSQLDataTable(sqlSubEmp2);
                var dtSubEmp3 = conn.getPostgreSQLDataTable(sqlSubEmp3);
                var dtOwnBus = conn.getPostgreSQLDataTable(sqlOwnBus);
                var dtSubBus1 = conn.getPostgreSQLDataTable(sqlSubBus1);
                var dtSubBus2 = conn.getPostgreSQLDataTable(sqlSubBus2);
                var dtSubBus3 = conn.getPostgreSQLDataTable(sqlSubBus3);
                var dtGuarantor1 = conn.getPostgreSQLDataTable(sqlGuarantor1);
                var dtGuarantor2 = conn.getPostgreSQLDataTable(sqlGuarantor2);
                var dtGuarantor3 = conn.getPostgreSQLDataTable(sqlGuarantor3);
                var dtGuarantor4 = conn.getPostgreSQLDataTable(sqlGuarantor4);
                var dtLoan = conn.getPostgreSQLDataTable(sqlLoan);
				var dtCol = conn.getPostgreSQLDataTable(sqlCol);

                var dsCustomer = new ReportDataSource("customer", dtCustomer);
                var dsSub1 = new ReportDataSource("sub1", dtSub1);
                var dsSub2 = new ReportDataSource("sub2", dtSub2);
                var dsSub3 = new ReportDataSource("sub3", dtSub3);
                var dsEmployee = new ReportDataSource("employee_work", dtEmployee);
                var dsSubEmp1 = new ReportDataSource("sub_employee1", dtSubEmp1);
                var dsSubEmp2 = new ReportDataSource("sub_employee2", dtSubEmp2);
                var dsSubEmp3 = new ReportDataSource("sub_employee3", dtSubEmp3);
                var dsOwnBus = new ReportDataSource("ownbusiness", dtOwnBus);
                var dsSubBus1 = new ReportDataSource("sub_business1", dtSubBus1);
                var dsSubBus2 = new ReportDataSource("sub_business2", dtSubBus2);
                var dsSubBus3 = new ReportDataSource("sub_business3", dtSubBus3);
                var dsGuarantor1 = new ReportDataSource("guarantor_1", dtGuarantor1);
                var dsGuarantor2 = new ReportDataSource("Guarantor2", dtGuarantor2);
                var dsGuarantor3 = new ReportDataSource("guarantor3", dtGuarantor3);
                var dsGuarantor4 = new ReportDataSource("guanrator_4", dtGuarantor4);
                var dsloan = new ReportDataSource("loan_request1", dtLoan);
				var dsCol = new ReportDataSource("COL_INFO", dtCol);
                var dsBusDetail = new ReportDataSource("DS_INFORMATION_APPLICANT", dtBusDetail);

                conn.generateReport(ReportViewer1, @"Micro_Loan_Application", null, dsCustomer, dsSub1, dsSub2, dsSub3, dsEmployee, dsSubEmp1, dsSubEmp2, dsSubEmp3, dsOwnBus, dsSubBus1, dsSubBus2, dsSubBus3, dsGuarantor1, dsGuarantor2, dsGuarantor3, dsGuarantor4, dsloan, dsCol,dsBusDetail);

            }
        }
    }
}