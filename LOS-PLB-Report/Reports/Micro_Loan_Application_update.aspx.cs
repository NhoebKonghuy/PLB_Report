using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace LOS_PLB_Report.Reports
{
    public partial class Micro_Loan_Application_update : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                var applicationNo = Request.QueryString["application_no"];
                var sqlsub1 = @"SELECT
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
		
								WHERE
									ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var sqlsub2 = @"SELECT
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
		
									WHERE 
										ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var sqlsub3 = @"SELECT
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
		
									WHERE 
										ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var sqlcustomer = @"SELECT
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
										INNER JOIN adm_identification_type ait ON ait.ID = aci.identification_type_id
									WHERE app.application_no= '" + applicationNo + "'";
                var sqlemployee_work = @"SELECT
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
								ID INNER JOIN app_customer_income_employee cir ON cir.application_id = ap.ID and cc.id = cir.customer_id
							WHERE ap.application_no= '" + applicationNo + "'";
                var sqlsub_employee1 = @"SELECT
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
		
								WHERE
									ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var sqlsub_employee2 = @"SELECT
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
		
											WHERE
												ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var sqlsub_employee3 = @"SELECT
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
		
											WHERE
												ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 3";
                var sqlsub_business1 = @"SELECT
											* 
										FROM
											(
											SELECT ROW_NUMBER
												( ) OVER ( ORDER BY asd.ID ) AS num,
												ap.application_no,
												CAR.NAME AS career,
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
												LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
		
											WHERE
												ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var sqlsub_business2 = @"SELECT
											* 
										FROM
											(
											SELECT ROW_NUMBER
												( ) OVER ( ORDER BY asd.ID ) AS num,
												ap.application_no,
												CAR.NAME AS career,
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
												LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
											WHERE
												ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var sqlsub_business3 = @"SELECT
											* 
										FROM
											(
											SELECT ROW_NUMBER
												( ) OVER ( ORDER BY asd.ID ) AS num,
												ap.application_no,
												CAR.NAME AS career,
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
												LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
											WHERE
												ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 3";
                var sqlownbusiness = @"SELECT
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
									CAR.NAME AS career,
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
									LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
								   WHERE ap.application_no = '" + applicationNo + "') A WHERE A.num = 2";
                var sqlguarantor_1 = @"SELECT
											* 
										FROM
											(
											SELECT ROW_NUMBER
												( ) OVER ( ORDER BY asu.ID ) AS num,
												ap.application_no,
												ag.full_name_kh,
												Ci.id_number,
												cc.phone_number,
												CAR.NAME AS career,
												cir.length_of_business||' ខែ' as length_of_bus,
												'' AS other_income,
												asu.customer_type,
												CASE
											  WHEN IT.ID_TYPE = 'F' THEN
											  'សៀវភៅគ្រួសារ' 
											  WHEN IT.ID_TYPE = 'CD' THEN
											  'លិខិតបញ្ជាក់អត្តសញ្ញាណ' 
											  WHEN IT.ID_TYPE = 'N' THEN
											  'អត្តសញ្ញាណប័ណ្ណ' 
											  WHEN IT.ID_TYPE = 'P' THEN
											  'លិខិតឆ្លងដែន' 
											  WHEN IT.ID_TYPE = 'MI' THEN
											  'ឆាយា' 
											  WHEN IT.ID_TYPE = 'R' THEN
											  'សៀវភៅស្នាក់នៅ' 
											  WHEN IT.ID_TYPE = 'B' THEN
											  'សំបុត្រកំណើត' 
											END AS IDENTIFICATION_TYPE
											FROM
												app_application ap
												INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
												AND asu.status = 't' 
												AND asu.customer_type IN ('PERSONAL_GUARANTOR','MORTGAGOR')
												INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
												ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
												left JOIN app_customer_identification Ci ON Ci.customer_id = cc.ID 
												AND Ci.application_id = ap.
												ID left JOIN adm_identification_type it ON it.ID = Ci.identification_type_id
												LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
												AND cc.ID = cir.customer_id 
												LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
											WHERE
											ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var sqlGuarantor2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asu.ID ) AS num,
		ap.application_no,
		ag.full_name_kh,
		Ci.id_number,
		cc.phone_number,
		CAR.NAME AS career,
		cir.length_of_business||' ខែ' as length_of_bus,
		'' AS other_income,
		asu.customer_type,
		CASE
      WHEN IT.ID_TYPE = 'F' THEN
      'សៀវភៅគ្រួសារ' 
      WHEN IT.ID_TYPE = 'CD' THEN
      'លិខិតបញ្ជាក់អត្តសញ្ញាណ' 
      WHEN IT.ID_TYPE = 'N' THEN
      'អត្តសញ្ញាណប័ណ្ណ' 
      WHEN IT.ID_TYPE = 'P' THEN
      'លិខិតឆ្លងដែន' 
      WHEN IT.ID_TYPE = 'MI' THEN
      'ឆាយា' 
      WHEN IT.ID_TYPE = 'R' THEN
      'សៀវភៅស្នាក់នៅ' 
      WHEN IT.ID_TYPE = 'B' THEN
      'សំបុត្រកំណើត' 
    END AS IDENTIFICATION_TYPE
	FROM
		app_application ap
		INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
		AND asu.status = 't' 
		AND asu.customer_type IN ('PERSONAL_GUARANTOR','MORTGAGOR')
		INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
		ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
		left JOIN app_customer_identification Ci ON Ci.customer_id = cc.ID 
		AND Ci.application_id = ap.
		ID left JOIN adm_identification_type it ON it.ID = Ci.identification_type_id
		LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
		AND cc.ID = cir.customer_id 
		LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
		
	WHERE
											ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var sqlguarantor3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asu.ID ) AS num,
		ap.application_no,
		ag.full_name_kh,
		Ci.id_number,
		cc.phone_number,
		CAR.NAME AS career,
		cir.length_of_business||' ខែ' as length_of_bus,
		'' AS other_income,
		asu.customer_type,
		CASE
      WHEN IT.ID_TYPE = 'F' THEN
      'សៀវភៅគ្រួសារ' 
      WHEN IT.ID_TYPE = 'CD' THEN
      'លិខិតបញ្ជាក់អត្តសញ្ញាណ' 
      WHEN IT.ID_TYPE = 'N' THEN
      'អត្តសញ្ញាណប័ណ្ណ' 
      WHEN IT.ID_TYPE = 'P' THEN
      'លិខិតឆ្លងដែន' 
      WHEN IT.ID_TYPE = 'MI' THEN
      'ឆាយា' 
      WHEN IT.ID_TYPE = 'R' THEN
      'សៀវភៅស្នាក់នៅ' 
      WHEN IT.ID_TYPE = 'B' THEN
      'សំបុត្រកំណើត' 
    END AS IDENTIFICATION_TYPE
	FROM
		app_application ap
		INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
		AND asu.status = 't' 
		AND asu.customer_type IN ('PERSONAL_GUARANTOR','MORTGAGOR')
		INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
		ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
		left JOIN app_customer_identification Ci ON Ci.customer_id = cc.ID 
		AND Ci.application_id = ap.
		ID left JOIN adm_identification_type it ON it.ID = Ci.identification_type_id
		LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
		AND cc.ID = cir.customer_id 
		LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
	WHERE
											ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 3";
                var sqlguanrator_4 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asu.ID ) AS num,
		ap.application_no,
		ag.full_name_kh,
		Ci.id_number,
		cc.phone_number,
		CAR.NAME AS career,
		cir.length_of_business||' ខែ' as length_of_bus,
		'' AS other_income,
		asu.customer_type,
		CASE
      WHEN IT.ID_TYPE = 'F' THEN
      'សៀវភៅគ្រួសារ' 
      WHEN IT.ID_TYPE = 'CD' THEN
      'លិខិតបញ្ជាក់អត្តសញ្ញាណ' 
      WHEN IT.ID_TYPE = 'N' THEN
      'អត្តសញ្ញាណប័ណ្ណ' 
      WHEN IT.ID_TYPE = 'P' THEN
      'លិខិតឆ្លងដែន' 
      WHEN IT.ID_TYPE = 'MI' THEN
      'ឆាយា' 
      WHEN IT.ID_TYPE = 'R' THEN
      'សៀវភៅស្នាក់នៅ' 
      WHEN IT.ID_TYPE = 'B' THEN
      'សំបុត្រកំណើត' 
    END AS IDENTIFICATION_TYPE
	FROM
		app_application ap
		INNER JOIN app_supplementary asu ON ap.ID = asu.application_id 
		AND asu.status = 't' 
		AND asu.customer_type IN ('PERSONAL_GUARANTOR','MORTGAGOR')
		INNER JOIN app_guarantor ag ON ag.supplementary_id = asu.
		ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
		left JOIN app_customer_identification Ci ON Ci.customer_id = cc.ID 
		AND Ci.application_id = ap.
		ID left JOIN adm_identification_type it ON it.ID = Ci.identification_type_id
		LEFT JOIN app_customer_income_business cir ON cir.application_id = ap.ID 
		AND cc.ID = cir.customer_id 
		LEFT JOIN ADM_CAREER CAR ON CAR.ID = CIR.CAREER_ID
	WHERE
											ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 4";
                var sqlloan_request1 = @"SELECT
										ap.application_no,
										ac.currency,
										apd.applied_amount,
										apd.tenor,
										apd.annual_interest_rate,
										art.name,
										apd.pay_principle_every_month,
										apd.grace_period
									FROM
										app_application ap
										INNER JOIN app_application_detail apd ON apd.application_id = apd.
										ID INNER JOIN adm_currency ac ON ac.ID = apd.currency_id
										INNER JOIN adm_repayment_type art on art.id = apd.repayment_type_id
									WHERE ap.application_no = '" + applicationNo + "'";
                var sqlCOL_INFO = @"SELECT 

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
									WHERE AP.application_no = '" + applicationNo + "'";
                var sqlIV_INFO = @"SELECT
										ap.application_no,
										cc.family_name_kh || ' ' || cc.given_name_kh AS NAME,
										adi.disbursement_amount,
										adi.monthly_existing_debt_commitment 
									FROM
										app_application ap
										INNER JOIN app_application_detail aad ON aad.application_id = ap.
										ID INNER JOIN cus_customer cc ON cc.ID = aad.customer_id
										INNER JOIN app_customer_debt_info adi ON adi.customer_id = cc.ID 
									WHERE
										AP.application_no = '" + applicationNo + "' ORDER BY adi.ID";
                var sqlStatus = @"SELECT ap.application_no ,
								adi.status
								from app_application ap 
								inner join app_debt_information adi on adi.application_id = ap.id
								where AP.application_no = '" + applicationNo + "'";
				var sqlPURPOSE_1 = @"SELECT * FROM
									(
										SELECT 
											ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
											AP.APPLICATION_NO ,
											APD.PURPOSE_DETAIL as PURPOSEDT
										FROM 
											APP_APPLICATION AP
											INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
											INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
											INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
										WHERE 
											AP.APPLICATION_NO = '"+applicationNo+"') A WHERE A.NUM = 1";
				var sqlPURPOSE_2 = @"SELECT * FROM
									(
										SELECT 
											ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
											AP.APPLICATION_NO ,
											APD.PURPOSE_DETAIL as PURPOSEDT
										FROM 
											APP_APPLICATION AP
											INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
											INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
											INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
										WHERE 
											AP.APPLICATION_NO = '"+applicationNo+"') A WHERE A.NUM = 2";
				var sqlPURPOSE_3 = @"SELECT * FROM
									(
										SELECT 
											ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
											AP.APPLICATION_NO ,
											APD.PURPOSE_DETAIL as PURPOSEDT
										FROM 
											APP_APPLICATION AP
											INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
											INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
											INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
										WHERE 
											AP.APPLICATION_NO = '"+applicationNo+"') A WHERE A.NUM = 3";
				var sqlPURPOSE_4 = @"SELECT * FROM
									(
										SELECT 
											ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
											AP.APPLICATION_NO ,
											APD.PURPOSE_DETAIL as PURPOSEDT
										FROM 
											APP_APPLICATION AP
											INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
											INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
											INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
										WHERE 
											AP.APPLICATION_NO = '"+applicationNo+"') A WHERE A.NUM = 4";



				var dtPURPOSE_1 = conn.getPostgreSQLDataTable(sqlPURPOSE_1);
				var dtPURPOSE_2 = conn.getPostgreSQLDataTable(sqlPURPOSE_2);
				var dtPURPOSE_3 = conn.getPostgreSQLDataTable(sqlPURPOSE_3);
				var dtPURPOSE_4 = conn.getPostgreSQLDataTable(sqlPURPOSE_4);
                var dtsub1 = conn.getPostgreSQLDataTable(sqlsub1);
                var dtsub2 = conn.getPostgreSQLDataTable(sqlsub2);
                var dtsub3 = conn.getPostgreSQLDataTable(sqlsub3);
                var dtcustomer = conn.getPostgreSQLDataTable(sqlcustomer);
                var dtemployee_work = conn.getPostgreSQLDataTable(sqlemployee_work);
                var dtsub_employee1 = conn.getPostgreSQLDataTable(sqlsub_employee1);
                var dtsub_employee2 = conn.getPostgreSQLDataTable(sqlsub_employee2);
                var dtsub_employee3 = conn.getPostgreSQLDataTable(sqlsub_employee3);
                var dtownbusiness = conn.getPostgreSQLDataTable(sqlownbusiness);
                var dtsub_business1 = conn.getPostgreSQLDataTable(sqlsub_business1);
                var dtsub_business2 = conn.getPostgreSQLDataTable(sqlsub_business2);
                var dtsub_business3 = conn.getPostgreSQLDataTable(sqlsub_business3);
                var dtguarantor_1 = conn.getPostgreSQLDataTable(sqlguarantor_1);
                var dtGuarantor2 = conn.getPostgreSQLDataTable(sqlGuarantor2);
                var dtguarantor3 = conn.getPostgreSQLDataTable(sqlguarantor3);
                var dtguanrator_4 = conn.getPostgreSQLDataTable(sqlguanrator_4);
                var dtloan_request1 = conn.getPostgreSQLDataTable(sqlloan_request1);
                var dtCOL_INFO = conn.getPostgreSQLDataTable(sqlCOL_INFO);
                var dtIV_INFO = conn.getPostgreSQLDataTable(sqlIV_INFO);
                var dtStatus = conn.getPostgreSQLDataTable(sqlStatus);





                var dssub1 = new ReportDataSource("sub1", dtsub1);
                var dssub2 = new ReportDataSource("sub2", dtsub2);
                var dssub3 = new ReportDataSource("sub3", dtsub3);
                var dscustomer = new ReportDataSource("customer", dtcustomer);
                var dsemployee_work = new ReportDataSource("employee_work", dtemployee_work);
                var dssub_employee1 = new ReportDataSource("sub_employee1", dtsub_employee1);
                var dssub_employee2 = new ReportDataSource("sub_employee2", dtsub_employee2);
                var dssub_employee3 = new ReportDataSource("sub_employee3", dtsub_employee3);
                var dsownbusiness = new ReportDataSource("ownbusiness", dtownbusiness);
                var dssub_business1 = new ReportDataSource("sub_business1", dtsub_business1);
                var dsDtsubBusiness2 = new ReportDataSource("sub_business2", dtsub_business2);
                var dsDtsubBusiness3 = new ReportDataSource("sub_business3", dtsub_business3);
                var dsDtguarantor1 = new ReportDataSource("guarantor_1", dtguarantor_1);
                var dsDtGuarantor2 = new ReportDataSource("Guarantor2", dtGuarantor2);
                var dsDtguarantor3 = new ReportDataSource("guarantor3", dtguarantor3);
                var dsDtguanrator4 = new ReportDataSource("guanrator_4", dtguanrator_4);
                var dsDtloanRequest1 = new ReportDataSource("loan_request1", dtloan_request1);
                var dsDtCOLINFO = new ReportDataSource("COL_INFO", dtCOL_INFO);
                var dsDtIVINFO = new ReportDataSource("IV_INFO", dtIV_INFO);
                var dsDtStatus = new ReportDataSource("Status", dtStatus);
				var dsPURPOSE_1 = new ReportDataSource("PURPOSE_1", dtPURPOSE_1);
				var dsPURPOSE_2 = new ReportDataSource("PURPOSE_2", dtPURPOSE_2);
				var dsPURPOSE_3 = new ReportDataSource("PURPOSE_3", dtPURPOSE_3);
				var dsPURPOSE_4 = new ReportDataSource("PURPOSE_4", dtPURPOSE_4);
                conn.generateReport(ReportViewer1, @"Micro_Loan_Application_update", null, dscustomer, dssub1, dssub2, dssub3, dsemployee_work, dssub_employee1, dssub_employee2, dssub_employee3, dsownbusiness, dssub_business1, dsDtsubBusiness2, dsDtsubBusiness3, dsDtguarantor1, dsDtGuarantor2, dsDtguarantor3, dsDtguanrator4, dsDtloanRequest1, dsDtCOLINFO, dsDtIVINFO, dsDtStatus, dsPURPOSE_1, dsPURPOSE_2, dsPURPOSE_3, dsPURPOSE_4);

            }
        }
    }
}