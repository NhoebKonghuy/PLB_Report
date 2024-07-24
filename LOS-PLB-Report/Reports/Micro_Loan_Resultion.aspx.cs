using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using WebGrease.Css.Ast.Selectors;

namespace LOS_PLB_Report.Reports
{
    public partial class Micro_Loan_Resultion : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];

				var FAC_REQUEST_INFO = @"SELECT AP.APPLICATION_NO,
										 APP.total_exposure_amount as total_exposure_amount,
										 APP.monthly_existing_debt_commitment,
										 AAD.total_ang_monthly_commitment AS COMMIT_OF_LOAN_REQUEST,
										 LP.NAME LOAN_PRODUCT,
										 APP.request_loan_amount AS LOAN_AMOUNT,
										 AC.currency as currency,
										 APP.annual_interest_rate||'%' as annual_interest_rate,
										 APP.monthly_interest_rate||'%' as monthly_interest_rate,
										 APP.loan_fee,
										 APP.cbc_fee AS cbc_fee,
										 APP.TENOR,
										 App.period,
										 ART.NAME AS REPAYMENT,
										 AAD.pay_principle_every_month,
										 APP.total_average,
										 APP.current_dscr AS CURENT_DSCR,
										 APP.ltv_ratio AS LTV_RATIO
			 
							FROM APP_APPLICATION AP 
							INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
							LEFT JOIN app_application_pre_approval APP ON APP.application_detail_id = AAD.ID
							INNER JOIN CUS_CUSTOMER CC ON CC.ID = AAD.customer_id 
							INNER JOIN mas_loan_product LP ON LP.ID = AAD.loan_product_id
							INNER JOIN adm_currency AC ON AC.ID = APP.currency_id
							INNER JOIN adm_repayment_type ART ON ART.ID = APP.repayment_type_id
							WHERE AP.APPLICATION_NO='" + applicationNo + "'";
				var SqlMBLS = @"SELECT 
				AP.APPLICATION_NO ,
				AAR.COMMENTS AS COMMENT ,
				SA.application_status,
				DE.FIRST_NAME_KH||' '||DE.LAST_NAME_KH AS USER_NAME,
				TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
				FROM APP_APPLICATION AP 
				INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
				INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
				INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
				INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
				INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID AND ROL.ROLE = 'MBLS'
				LEFT JOIN app_application_recommendation AAR ON AAR.created_by_id = US.ID 
				WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var APPROVAL_FAC_REQUEST_INFO = @"SELECT AP.APPLICATION_NO,
			 APP.total_exposure_amount as total_exposure_amount,
			 APP.monthly_existing_debt_commitment,
			 APP.total_average - APP.monthly_existing_debt_commitment AS COMMIT_OF_LOAN_REQUEST,
			 APP.approval_request_loan_amount AS LOAN_AMOUNT,
			 AC.currency as currency,
			 APP.approval_annual_interest_rate||'%' as anual_inter_rate,
			 APP.approval_monthly_interest_rate||'%' as mon_inter_rate,
			 APP.approval_loan_fee,
			 APP.cbc_fee AS cbc_fee,
			 APP.TENOR,
			 App.approval_period,
			 ART.NAME AS REPAYMENT,
			 APP.approval_pay_principle_every_month,
			 APP.approval_total_average,
			 APP.approval_current_dscr AS CURENT_DSCR,
			 APP.approval_ltv_ratio AS LTV_RATIO
FROM APP_APPLICATION AP 
INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
LEFT JOIN app_application_pre_approval APP ON APP.application_detail_id = AAD.ID
INNER JOIN CUS_CUSTOMER CC ON CC.ID = AAD.customer_id 
INNER JOIN adm_currency AC ON AC.ID = APP.currency_id
LEFT JOIN adm_repayment_type ART ON ART.ID = APP.approval_repayment_type_id 
									WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var BM = @"SELECT 
AP.APPLICATION_NO ,
AAR.COMMENTS AS COMMENT ,
SA.application_status,
DE.LAST_NAME_KH||' '||DE.FIRST_NAME_KH AS USER_NAME,
TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
FROM APP_APPLICATION AP 
INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
LEFT JOIN app_application_recommendation AAR ON AAR.created_by_id = US.ID
							WHERE ROL.ROLE = 'BM' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var HEAD_OF_MICRO = @"SELECT 
									AP.APPLICATION_NO ,
									SA.NOTE AS COMMENT ,
									SA.application_status,
									DE.LAST_NAME_KH||' '||DE.FIRST_NAME_KH AS USER_NAME,
									TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
									FROM APP_APPLICATION AP 
									INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
									INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
									INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
									INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
									INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
									WHERE ROL.ROLE = 'Head of Micro' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var SqlMBLO = @"SELECT 
AP.APPLICATION_NO ,
AAR.COMMENTS AS COMMENT ,
SA.application_status,
DE.LAST_NAME_KH||' '||DE.FIRST_NAME_KH AS USER_NAME,
TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
FROM APP_APPLICATION AP 
INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
LEFT JOIN app_application_recommendation AAR ON AAR.created_by_id = US.ID
			WHERE
				ROL.ROLE = 'MBLO' 
				AND AP.APPLICATION_NO = '" + applicationNo+"'";
				var ANALYST = @"SELECT 
AP.APPLICATION_NO ,
AAR.COMMENTS AS COMMENT ,
SA.application_status,
DE.LAST_NAME_KH||' '||DE.FIRST_NAME_KH AS USER_NAME,
TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
FROM APP_APPLICATION AP 
INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
LEFT JOIN app_application_recommendation AAR ON AAR.created_by_id = US.ID
							WHERE ROL.ROLE = 'Analyst' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var SENOIR = @"SELECT 
AP.APPLICATION_NO ,
AAR.COMMENTS AS COMMENT ,
SA.application_status,
DE.LAST_NAME_KH||' '||DE.FIRST_NAME_KH AS USER_NAME,
TO_CHAR(SA.COMPLETE_DATE ,'DD/MM/YYYY') AS COMPLETED_DATE
FROM APP_APPLICATION AP 
INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't'
INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
LEFT JOIN app_application_recommendation AAR ON AAR.created_by_id = US.ID
							WHERE ROL.ROLE = 'Senior/Micro Loan Evaluation Manager' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var GENERA1 = @"SELECT AP.APPLICATION_NO,
									 MU.NAME AS MBLO_NAME,
									 CC.family_name_kh||' '||CC.given_name_kh AS FULL_NAME,
									 MB.NAME AS BRANH_NAME,
									 to_char(ap.created, 'DD/MM/YYYY')
						FROM APP_APPLICATION AP 
						INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
						INNER JOIN CUS_CUSTOMER CC ON CC.ID = AAD.customer_id 
						INNER JOIN mas_branch MB ON MB.ID = AAD.branch_id
						INNER JOIN MAS_USERS MU ON MU.ID = AP.created_by_id
						WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var EXPOSURE = @"SELECT AP.APPLICATION_NO,
								 AET.NAME AS EXPOSURE_TYPE
					FROM APP_APPLICATION AP 
					INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
					LEFT JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
					LEFT JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID AND CS.STATUS = 't'
					LEFT JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
					LEFT JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
					LEFT JOIN app_col_value_evaluation ACVE ON ACVE.app_collateral_detail_ID = CCD.ID
					LEFT JOIN adm_exposure_type AET ON AET.ID = ACVE.exposure_type_id
					WHERE ap.application_no ='"+applicationNo+"'";
				var PURPOSE = @"SELECT  AP.application_no,
								 ALPD.purpose_detail
					FROM APP_APPLICATION AP 
					INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
					INNER JOIN app_loan_purpose ALP ON ALP.application_detail_id = AAD.ID AND ALP.status='t'
					LEFT JOIN app_loan_purpose_detail ALPD ON ALPD.loan_purpose_id = ALP.ID AND ALPD.status='t'
					WHERE AP.application_no='"+applicationNo+"'";
				var CODE1 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY ALPD.ID ) AS num,
									ap.application_no,
										ADP.code
							FROM APP_APPLICATION AP 
							INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
							INNER JOIN app_loan_purpose ALP ON ALP.application_detail_id = AAD.ID 
							LEFT JOIN app_loan_purpose_detail ALPD ON ALPD.loan_purpose_id = ALP.ID
							INNER JOIN adm_purpose ADP ON ADP.ID = ALPD.purpose_iD
							WHERE ap.application_no ='"+applicationNo+ "')A WHERE A.num = 1";
                var CODE2 = @"SELECT
								* 
							FROM
								(
								SELECT ROW_NUMBER
									( ) OVER ( ORDER BY ALPD.ID ) AS num,
									ap.application_no,
										ADP.code
							FROM APP_APPLICATION AP 
							INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
							INNER JOIN app_loan_purpose ALP ON ALP.application_detail_id = AAD.ID 
							LEFT JOIN app_loan_purpose_detail ALPD ON ALPD.loan_purpose_id = ALP.ID
							INNER JOIN adm_purpose ADP ON ADP.ID = ALPD.purpose_iD
							WHERE ap.application_no ='" + applicationNo + "')A WHERE A.num = 2";
				var SCORE = @"SELECT AP.APPLICATION_NO,
						   ACR.grade AS CRR_Grade,
						   ACR.category AS CRR_CATEGORY,
						   MCRL.NAME AS CSC_GRADE,
						   AAD.final_score AS CSC_SCORE
					FROM APP_APPLICATION AP 
					LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID
					LEFT JOIN app_application_credit_rating ACR ON ACR.application_detail_id = AAD.ID
					LEFT JOIN mas_cs_risk_level MCRL ON MCRL.ID = AAD.risk_level_id
					WHERE AP.application_no='" + applicationNo+"'";
                var CBC = @"SELECT AP.APPLICATION_NO,
								 case when cci.result= '0' then 'ACCEPT'
								 WHEN cci.result= '1' then 'REVIEW'
								 ELSE 'REJECT' END AS CBC
					FROM APP_APPLICATION AP 
					INNER JOIN app_cbc AC ON AC.application_id = AP.ID 
					INNER JOIN app_cbc_consumer_info CCI ON CCI.cbc_id = AC.ID
					WHERE AP.application_no='" + applicationNo+"'";
                var AML = @"SELECT AP.APPLICATION_NO,
									 AML.aml_status
							FROM APP_APPLICATION AP 
							LEFT JOIN APP_AML AML ON AML.application_id = AP.ID 
							WHERE ap.application_no ='" + applicationNo + "'";
                var APPROVAL = @"SELECT 
					AP.APPLICATION_NO,
					ROL.ROLE 
					FROM APP_APPLICATION AP 
					INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.application_stages ='APPROVAL'
					INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't'
					INNER JOIN MAS_USERS US ON US.ID = MU.USER_ID
					INNER JOIN MAS_USER_DETAIL DE ON DE.USER_ID = US.ID 
					INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
					WHERE AP.application_no='" + applicationNo + "'";
				var SUB_PURPOSE = @"SELECT
									AP.APPLICATION_NO,
									ALP.NAME 
								FROM
									APP_APPLICATION AP
									INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.
									ID INNER JOIN mas_loan_sub_product ALP ON ALP.ID = AAD.LOAN_SUB_PRODUCT_ID
									AND ALP.STATUS = 't' 
									WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var DEVIATION = @"SELECT
							  AP.APPLICATION_NO,
							  A.CONDITION_TYPE,
							  A.JUSTIFICATION 
							FROM
							  APP_APPLICATION AP
							  LEFT JOIN (
							  SELECT
								AP.APPLICATION_NO,
								APP.APPLICATION_ID,
								JSONB_ARRAY_ELEMENTS ( APP.DEVIATION_REQUEST ) ->> 'conditionType' AS CONDITION_TYPE,
								JSONB_ARRAY_ELEMENTS ( APP.DEVIATION_REQUEST ) ->> 'justification' AS JUSTIFICATION 
							  FROM
								APP_APPLICATION AP
								LEFT JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.ID 
								AND APP.STATUS = 't' 
							  ) A ON A.APPLICATION_ID = AP.ID 
								WHERE
								  AP.APPLICATION_NO = '" + applicationNo+"'";
				var CONDITION = @"SELECT AP.APPLICATION_NO,
									AIC.CONDITION FROM APP_APPLICATION AP 
									LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
									LEFT JOIN APP_IMPOSE_CONDITION AIC ON AIC.APP_DETAIL_ID = AAD.ID
									WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var ACC_NUMBER = @"SELECT
									AP.APPLICATION_NO,
                                    ACA.ACCOUNT_NO AS account_number					
                                    FROM
									APP_APPLICATION AP
									LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.
									ID LEFT JOIN app_loan_disbuse_response ACA ON ACA.app_detail_id = AAD.ID 
									AND ACA.STATUS = 't'
									WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var BRANCH = @"SELECT * FROM BRANCH_PLB
							 WHERE APPLICATION_NO='"+applicationNo+"'";
				var MONTHLY = @"SELECT ap.application_no,
			 aad.total_ang_monthly_commitment,
			 acd.monthly_existing_debt_commitment 
from app_application ap 
inner join app_application_detail aad on aad.application_id = ap.id 
inner join app_customer_debt_info acd on acd.application_id = ap.id
WHERE AP.APPLICATION_NO='" + applicationNo+"'";

				var dtCONDITION = conn.getPostgreSQLDataTable(CONDITION);
                var dtFAC_REQUEST_INFO = conn.getPostgreSQLDataTable(FAC_REQUEST_INFO);
				var dtMBLS = conn.getPostgreSQLDataTable(SqlMBLS);
				var dtAPPROVAL_FAC_REQUEST_INFO = conn.getPostgreSQLDataTable(APPROVAL_FAC_REQUEST_INFO);
				var dtBM = conn.getPostgreSQLDataTable(BM);
				var dtHEAD_OF_MICRO = conn.getPostgreSQLDataTable(HEAD_OF_MICRO);
				var dttMBLO = conn.getPostgreSQLDataTable(SqlMBLO);
				var dtANALYST = conn.getPostgreSQLDataTable(ANALYST);
				var dtSENOIR = conn.getPostgreSQLDataTable(SENOIR);
				var dtGENERA1 = conn.getPostgreSQLDataTable(GENERA1);
                var dtEXPOSURE = conn.getPostgreSQLDataTable(EXPOSURE);
                var dtPURPOSE = conn.getPostgreSQLDataTable(PURPOSE);
                var dtCODE1 = conn.getPostgreSQLDataTable(CODE1);
                var dtCODE2 = conn.getPostgreSQLDataTable(CODE2);
                var dtSCORE = conn.getPostgreSQLDataTable(SCORE);
                var dtCBC = conn.getPostgreSQLDataTable(CBC);
                var dtAML = conn.getPostgreSQLDataTable(AML);
                var dtAPPROVAL = conn.getPostgreSQLDataTable(APPROVAL);
				var dtSUB_PURPOSE = conn.getPostgreSQLDataTable(SUB_PURPOSE);
				var dtDEVIATION = conn.getPostgreSQLDataTable(DEVIATION);
                var dtACC_NUMBER = conn.getPostgreSQLDataTable(ACC_NUMBER);
				var dtBRANCH = conn.getPostgreSQLDataTable(BRANCH);
				var dtMONTHLY = conn.getPostgreSQLDataTable(MONTHLY);

                var dsFAC_REQUEST_INFO = new ReportDataSource("FAC_REQUEST_INFO", dtFAC_REQUEST_INFO);
                var dsMBLS = new ReportDataSource("MBLS", dtMBLS);
                var dsAPPROVAL_FAC_REQUEST_INFO = new ReportDataSource("APPROVAL_FAC_REQUEST_INFO", dtAPPROVAL_FAC_REQUEST_INFO);
                var dsBM = new ReportDataSource("BM", dtBM);
                var dsHEAD_OF_MICRO = new ReportDataSource("HEAD_OF_MICRO", dtHEAD_OF_MICRO);
                var dssMBLO = new ReportDataSource("MBLO", dttMBLO);
                var dsANALYST = new ReportDataSource("ANALYST", dtANALYST);
				var dsSENOIR = new ReportDataSource("SENOIR", dtSENOIR);
				var dsGENERA1 = new ReportDataSource("GENERA1", dtGENERA1);
                var dsEXPOSURE = new ReportDataSource("EXPOSURE", dtEXPOSURE);
                var dsPURPOSE = new ReportDataSource("PURPOSE", dtPURPOSE);
				var dsCODE1 = new ReportDataSource("CODE1", dtCODE1);
                var dsCODE2 = new ReportDataSource("CODE2", dtCODE2);
                var dsSCORE = new ReportDataSource("SCORE", dtSCORE);
                var dsCBC = new ReportDataSource("CBC", dtCBC);
                var dsAML = new ReportDataSource("AML", dtAML);
                var dsAPPROVAL = new ReportDataSource("APPROVAL", dtAPPROVAL);
				var dsSUB_PURPOSE = new ReportDataSource("SUB_PURPOSE", dtSUB_PURPOSE);
				var dsDEVIATION = new ReportDataSource("DEVIATION", dtDEVIATION);
				var dsCONDITION = new ReportDataSource("CONDITION", dtCONDITION);
				var dsACC_NUMBER = new ReportDataSource("ACC_NUMBER", dtACC_NUMBER);
				var dsBRANCH = new ReportDataSource("BRANCH", dtBRANCH);
				var dsMONTHLY = new ReportDataSource("MONTHLY", dtMONTHLY);
				

                conn.generateReport(ReportViewer1, @"Micro_Loan_Resultion", null ,dsACC_NUMBER,dsMONTHLY, dsCONDITION,dsBRANCH, dsMBLS, dsFAC_REQUEST_INFO,dsDEVIATION, dsAPPROVAL_FAC_REQUEST_INFO, dsBM, dsHEAD_OF_MICRO, dssMBLO, dsANALYST, dsSENOIR, dsGENERA1, dsEXPOSURE, dsPURPOSE, dsCODE1, dsCODE2, dsSCORE, dsCBC, dsAML, dsAPPROVAL,dsSUB_PURPOSE);

            }
        }
    }
}