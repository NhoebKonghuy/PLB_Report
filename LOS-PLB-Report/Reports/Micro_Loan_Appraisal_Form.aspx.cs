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
    public partial class Micro_Loan_Appraisal_Form : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];
				var LOAN_REQUEST = @"SELECT ap.application_no,
										 mlsp.name as sub_product,
										 ac.currency,
										 aad.applied_amount,
										 aad.annual_interest_rate,
										 aad.tenor,
										 aad.loan_fee,
										 art.name as repayment_type,
 								case when art.name= 'Decline' then aad.grace_period
 										 when art.name= 'EMI' then aad.grace_period
 										 when art.name= 'Flexible Principle' then null 
	  								 when art.name= 'Semi-Balloon' then null
 										 when art.name= 'Balloon' then null
										 end as repayment_month,
										 aad.total_ang_monthly_commitment
							from app_application ap 
							left join app_application_detail  aad on aad.application_id = ap.id
							left join mas_loan_sub_product mlsp on mlsp.id = aad.loan_sub_product_id
							left join adm_currency ac on ac.id = aad.currency_id
							left join adm_repayment_type art on art.id = aad.repayment_type_id
							WHERE AP.APPLICATION_NO='" + applicationNo + "'";
				var MBLS = @"SELECT 
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
							WHERE ROL.ROLE = 'MBLS' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var COL_ASSET = @"SELECT ap.application_no,
											 ast.name as sector,
											 aps.name as sector_priority
								 from app_application ap 
								inner join app_col_collateral_asset cca on cca.application_id = ap.id
								inner join adm_priority_sector aps on aps.id = cca.priority_sector_id
								inner join adm_sector_detail ast on ast.id = cca.sector_detail_id
								WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var BM = @"SELECT 
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
							WHERE ROL.ROLE = 'BM' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var LOAN_PURPOSE = @"SELECT ap.application_no,
											 apd.purpose_detail,
											 apd.finance_amount,
											 apd.utilized_amount,
											 ac.currency
								 from app_application ap 
								inner join app_application_detail aad on aad.application_id = ap.id
								INNER JOIN APP_LOAN_PURPOSE ALP ON ALP.APPLICATION_DETAIL_ID = AAD.ID AND ALP.STATUS = 't'
								INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = ALP.ID AND APD.STATUS = 't'
								LEFT JOIN adm_currency AC ON AC.ID = AAD.currency_id
									WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var MBLO = @"SELECT 
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
								WHERE ROL.ROLE = 'MBLO' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var ANALYST = @"SELECT 
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
							WHERE ROL.ROLE = 'Analyst' AND AP.APPLICATION_NO='" + applicationNo+"'";
				var SUB_PURPOSE = @"SELECT ap.application_no,
									 AAD.applied_amount,
									 ASLP.NAME AS sub_loan_purpose,
									 ALP.sub_loan_finance_amount,
									 alp.margin_of_finance,
									 apd.turn_over_of_business_purpose,
									 apd.total_asset_of_business_purpose,
									 case when APD.registration_of_business_purpose='t' then 'YES'
												else 'NO' end as registration
						 from app_application ap 
						inner join app_application_detail aad on aad.application_id = ap.id
						INNER JOIN APP_LOAN_PURPOSE ALP ON ALP.APPLICATION_DETAIL_ID = AAD.ID AND ALP.STATUS = 't'
						INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = ALP.ID AND APD.STATUS = 't'
						INNER JOIN adm_sub_loan_purpose ASLP ON ASLP.ID = ALP.sub_loan_purpose_id
						WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var FACILITY_INFO = @"SELECT
									ap.application_no,
									ALN.NAME AS LENDER_NAME,
									AC.currency,
									 AFT.NAME AS FACILITY_TYPE,
									 to_char(adi.disbursement_date, 'DD/MM/YYYY') AS disbursement_date,
									 adi.disbursement_amount,
									 adi.tenure,
									 adi.annual_interest_rate,
									 CASE WHEN adi.refinance='f' THEN 'NO'
									 WHEN adi.refinance='t' then 'YES'
									 else '' end as refinance,
									 adi.outstanding_balance,
									 aCT.NAME AS COLLATERAL_TYPE,
									 ADI.monthly_existing_debt_commitment
		 
									FROM APP_APPLICATION AP 
									INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
									INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id
									LEFT JOIN app_customer_debt_info ADI ON ADI.application_id = AP.ID and cc.id = ADI.customer_id
									LEFT JOIN adm_facility_type AFT ON AFT.ID = ADI.facility_type_id
									LEFT JOIN adm_currency AC ON AC.ID = ADI.currency_id
								  LEFT JOIN adm_collateral_type act on act.id = ADI.collateral_type_id
									LEFT JOIN adm_lender_name ALN ON ALN.ID = ADI.leader_name_id
					WHERE ap.application_no ='" + applicationNo+"'";
				var BORO_INFO = @"SELECT AP.application_no,
								 cc.cif_number,
								 AAD.full_name_en,
								 AAD.full_name_kh,
								 CC.AGE,
								 CASE
									  WHEN CC.GENDER = 'MALE' THEN
									  'ប្រុស' ELSE'ស្រី' end as gender,			
								 'MAIN BORROWER' AS BORROWER_TYPE,
								 CASE 
										WHEN AMS.NAME = 'Married' THEN 'រៀបការ'
										WHEN AMS.NAME = 'Divorced' THEN 'លែងលះ'
										WHEN AMS.NAME = 'Single' THEN 'នៅលីវ'
										WHEN AMS.NAME = 'Widower' THEN 'មេម៉ាយ'
										WHEN AMS.NAME = 'Separated' THEN 'លែងលះ'
										END AS MARITAL,
										acci.k_score,
										acci.score_index
					FROM app_application AP 
					INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
					INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id
					LEFT JOIN adm_marital_status AMS ON AMS.ID = CC.marital_status_id
					LEFT JOIN app_cbc AC ON AC.application_id = AP.ID
					LEFT JOIN app_cbc_consumer_info acci on acci.cbc_id = ac.id
					WHERE AP.application_no='" + applicationNo+"'";
				var GUAN_CO_BOR = @"SELECT *
									FROM (
										SELECT AP.application_no,
												 AAD.full_name_en,
												 AAD.full_name_kh,
												 CC.AGE,
												 cc.cif_number,
												 CASE
													  WHEN CC.GENDER = 'MALE' THEN
													  'ប្រុស' ELSE'ស្រី' end as gender,			
												 asp.customer_type,
												 art.name AS relatioship,
												 CASE 
														WHEN AMS.NAME = 'Married' THEN 'រៀបការ'
														WHEN AMS.NAME = 'Divorced' THEN 'លែងលះ'
														WHEN AMS.NAME = 'Single' THEN 'នៅលីវ'
														WHEN AMS.NAME = 'Widower' THEN 'មេម៉ាយ'
														WHEN AMS.NAME = 'Separated' THEN 'លែងលះ'
														END AS MARITAL,
														acci.k_score,
														acci.score_index
									FROM app_application AP 
									INNER JOIN app_supplementary ASP ON ASP.application_id = AP.ID and aSP.status ='t' and ASP.customer_type in ('PERSONAL_GUARANTOR','MORTGAGOR')
									INNER JOIN app_guarantor AAD ON AAD.supplementary_id = ASP.ID 
									INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id
									INNER JOIN adm_relationship_type ART ON ART.ID = AAD.relationship_type_id
									INNER JOIN adm_marital_status AMS ON AMS.ID = CC.marital_status_id
									left join app_cbc ac on ac.application_id = ap.id
									left JOIN app_cbc_consumer_info acci on acci.cbc_id = ac.id and acci.customer_id= cc.id and  acci.status='t'
									UNION
									SELECT AP.application_no,
												 AAD.full_name_en,
												 AAD.full_name_kh,
												 CC.AGE,
												 cc.cif_number,
												 CASE
													  WHEN CC.GENDER = 'MALE' THEN
													  'ប្រុស' ELSE'ស្រី' end as gender,			
												 asp.customer_type,
												 art.name AS relatioship,
												 CASE 
														WHEN AMS.NAME = 'Married' THEN 'រៀបការ'
														WHEN AMS.NAME = 'Divorced' THEN 'លែងលះ'
														WHEN AMS.NAME = 'Single' THEN 'នៅលីវ'
														WHEN AMS.NAME = 'Widower' THEN 'មេម៉ាយ'
														WHEN AMS.NAME = 'Separated' THEN 'លែងលះ'
														END AS MARITAL,
														acci.k_score,
														acci.score_index
									FROM app_application AP 
									INNER JOIN app_supplementary ASP ON ASP.application_id = AP.ID and aSP.status ='t' and ASP.customer_type = 'CO_BORROWER'
									INNER JOIN app_supplementary_detail AAD ON AAD.supplementary_id = ASP.ID 
									INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id
									INNER JOIN adm_relationship_type ART ON ART.ID = AAD.relationship_type_id
									INNER JOIN adm_marital_status AMS ON AMS.ID = CC.marital_status_id
									left join app_cbc ac on ac.application_id = ap.id
									left JOIN app_cbc_consumer_info acci on acci.cbc_id = ac.id and acci.customer_id= cc.id and  acci.status='t'
									) AS subquery
									WHERE subquery.application_no = '" + applicationNo+"'";
                var COLLATERAL = @"SELECT ap.application_no,
										 ACT.NAME 
							from app_application ap
							INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID AND CS.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
							INNER JOIN adm_collateral_type ACT ON ACT.ID = CCD.collateral_type_id
							WHERE ap.application_no ='" + applicationNo + "'";
				var INCOME = @"SELECT *
								FROM (
									SELECT ap.application_no,
										   CIB.occupation_description AS SOURCE_INCOME,
										   CIB.business_income AS income,
										   to_char(CIB.start_date, 'yyyy') AS YEAR,
										   CIB.net_income AS NET_INCOME,
										   CIB.net_income/CIB.business_income AS MARGIN,
										   CIB.total_expense AS expense,
													 CC.family_name_kh||' '||CC.given_name_kh as name,
										   CIB.business_status AS BUS
									FROM app_application AP 
									INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
										INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id 
									INNER JOIN app_customer_income_business CIB ON CIB.application_id = AP.ID AND CC.ID = CIB.customer_id AND CIB.STATUS= 't'
									UNION 
									SELECT ap.application_no,
										   CIB.POSITION AS SOURCE_INCOME,
										   (CIB.base_salary + CIB.other_benefit) AS income,
										   to_char(CIB.start_date, 'yyyy') AS YEAR,
										   CIB.net_income AS NET_INCOME,
										   CIB.net_income / (CIB.base_salary + CIB.other_benefit) AS MARGIN,
										   CIB.expense AS expense,
										   CC.family_name_kh||' '||CC.given_name_kh as name,
										   CIB.business_status AS BUS
									FROM app_application AP 
									INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
										INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id 
									INNER JOIN app_customer_income_employee CIB ON CIB.application_id = AP.ID AND CC.ID = CIB.customer_id AND CIB.STATUS= 't'
									UNION
									SELECT ap.application_no,
										   CIB.property_rental_description AS SOURCE_INCOME,
										   CIB.income_from_property_rental AS income,
										   to_char(CIB.started_date, 'yyyy') AS YEAR,
										   CIB.net_income AS NET_INCOME,
										   CIB.net_income/CIB.income_from_property_rental AS MARGIN,
										   CIB.miscellaneous_expenses AS expense,
										   CC.family_name_kh||' '||CC.given_name_kh as name,
										   CIB.business_status AS BUS
									FROM app_application AP 
									INNER JOIN app_application_detail AAD ON AAD.application_id = AP.ID 
										INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id 
									INNER JOIN app_cus_income_property_rental CIB ON CIB.application_id = AP.ID AND CIB.customer_id = CC.ID AND CIB.STATUS= 't'
								) AS subquery
								WHERE subquery.application_no = '" + applicationNo+"'";
                var LOAN_FUND = @"SELECT ap.application_no,
								alf.name 
								from app_application ap
								inner join app_application_detail aad on aad.id = ap.id
								inner join adm_loan_fund alf on alf.id = aad.loan_fund_id
					WHERE AP.application_no='" + applicationNo+"'";
                var COLLATERAL_INFO = @"SELECT AP.APPLICATION_NO,
								   CCD.collateral_name,
								   COL.collateral_NO,
								   COL.owner_name,
								   CT.NAME AS COL_TYPE,
								   CCU.NAME AS PROPERTY,
								   CVE.max_percent_ltv || '%' AS STD_LTV,
								   HR.NAME AS HYPO,
								   CASE WHEN CVE.fair_market_value IS NULL THEN 0 ELSE CVE.fair_market_value END AS FMV,
								   CVE.force_sale_value AS FSV,
								  CASE WHEN (CVE.max_percent_ltv * CVE.fair_market_value) / 100 IS NULL THEN 0 ELSE (CVE.max_percent_ltv * CVE.fair_market_value) / 100 END AS Max_eli,
								   AC.currency
							FROM APP_APPLICATION AP 
							INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID AND CS.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
							INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
							INNER JOIN cus_customer CC ON CC.ID = COL.customer_id
							LEFT JOIN app_col_value_evaluation CVE ON CVE.app_collateral_detail_id = CCD.ID
							INNER JOIN adm_currency AC ON AC.ID = COL.currency_id
							LEFT JOIN adm_collateral_type CT ON CT.ID = CCD.collateral_type_id
							LEFT JOIN adm_col_current_using_land CCU ON CCU.ID = CCD.current_using_land_id
							LEFT JOIN adm_hypothec_registeration HR ON HR.ID = CVE.hypothec_registration_id
							WHERE ap.application_no ='" + applicationNo + "'";
                var COL_INFO = @"SELECT ap.application_no,
								 MCT.NAME AS MAIN_COL,
								 GTP.NAME AS GARANTEE
					FROM APP_APPLICATION AP 
					INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
					LEFT JOIN adm_main_collateral_type MCT ON MCT.ID = ACCA.main_collateral_type_id 
					LEFT JOIN adm_guarantee_third_party GTP ON GTP.ID = ACCA.guarantee_by_third_party_id
					WHERE AP.application_no='" + applicationNo + "'";
				var GENERAL_INFO = @"SELECT AP.APPLICATION_NO,
									   MU.NAME MBLO_NAME,
											 MB.NAME AS BRANCH_NAME,
											 TO_CHAR(AP.created,'DD/MM/YYYY') AS REQUEST_DATE,
											 AET.NAME AS EXPOSURE_TYPE,
											 AAS.application_stages
								FROM APP_APPLICATION AP
								INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
								INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
								LEFT JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID AND CS.STATUS = 't'
								LEFT JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
								LEFT JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
								LEFT JOIN app_col_value_evaluation ACVE ON ACVE.app_collateral_detail_ID = CCD.ID
								LEFT JOIN adm_exposure_type AET ON AET.ID = ACVE.exposure_type_id
								INNER JOIN app_application_stage AAS ON AAS.application_id = AP.ID
								INNER JOIN mas_users MU ON MU.ID = AP.created_by_id
								INNER JOIN MAS_BRANCH MB ON MB.ID = AP.branch_id
								WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var HYP_COL = @"SELECT AP.APPLICATION_NO,
							   CCD.collateral_name,
							   COL.collateral_NO,
							   COL.owner_name,
							   CT.NAME AS COL_TYPE,
							   CCU.NAME AS PROPERTY,
							   CVE.max_percent_ltv || '%' AS STD_LTV,
							   HR.NAME AS HYPO,
							   CASE WHEN CVE.fair_market_value IS NULL THEN 0 ELSE CVE.fair_market_value END AS FMV,
							   CVE.force_sale_value AS FSV,
							   CASE WHEN (cve.max_percent_ltv * cve.fair_market_value) / 100 IS NULL THEN 0 ELSE (cve.max_percent_ltv * cve.fair_market_value) / 100 END AS Max_eli,
							   AC.currency
						FROM APP_APPLICATION AP 
						INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID AND ACCA.STATUS = 't'
						INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID AND CS.STATUS = 't'
						INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID AND COL.STATUS = 't'
						INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID AND CCD.STATUS = 't'
						INNER JOIN cus_customer CC ON CC.ID = COL.customer_id
						LEFT JOIN app_col_value_evaluation CVE ON CVE.app_collateral_detail_id = CCD.ID 
						INNER JOIN adm_currency AC ON AC.ID = COL.currency_id
						INNER JOIN adm_collateral_type CT ON CT.ID = CCD.collateral_type_id
						INNER JOIN adm_col_current_using_land CCU ON CCU.ID = CCD.current_using_land_id
						INNER JOIN adm_hypothec_registeration HR ON HR.ID = CVE.hypothec_registration_id AND HR.name != 'Un-Register Hypothecation'
						WHERE AP.APPLICATION_NO = '" + applicationNo+"' ";
				var lOAN_FMV = @"SELECT AP.APPLICATION_NO,
										 CDI.outstanding_balance,
										 aad.applied_amount 
							FROM app_application AP 
							LEFT JOIN app_application_detail AAD ON AAD.applicaTION_id = AP.ID 
							LEFT JOIN app_customer_debt_info CDI ON CDI.application_id = AP.ID AND CDI.overlap_collateral_with_plb = 't' 
							WHERE AP.application_no= '"+applicationNo+"'";
				var APPROVAL = @"SELECT AP.APPLICATION_NO,
					   ROL.ROLE
				FROM APP_APPLICATION AP 
				INNER JOIN APP_APPLICATION_STAGE SA ON SA.APPLICATION_ID = AP.ID AND SA.STATUS = 't' AND SA.application_stages='APPROVAL'
				INNER JOIN MAS_ALLOCATE_USER MU ON MU.APPLICATION_STAGE_ID = SA.ID AND MU.STATUS = 't' 
				INNER JOIN MAS_ROLE ROL ON ROL.ID = MU.ROLE_ID
				WHERE AP.APPLICATION_NO = '"+applicationNo+"' ";
				var PURPOSE_CODE = @"SELECT AP.APPLICATION_NO,
										 ASI.CODE
							 FROM APP_APPLICATION AP
							INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
							INNER JOIN CUS_CUSTOMER CC ON CC.ID = AAD.CUSTOMER_ID 
							INNER JOIN APP_CUSTOMER_INCOME_BUSINESS CIB ON CIB.CUSTOMER_ID = CC.ID 
							LEFT JOIN ADM_SOURCE_INCOME ASI ON ASI.ID = CIB.SOURCE_INCOME_ID
							WHERE ap.application_no ='" + applicationNo + "'";
				var SCORE = @"SELECT AP.APPLICATION_NO,
								   ACR.final_score AS CRR_SCORE,
										 ACR.category AS CRR_CATEGORY,
										 MCRL.NAME AS CSC_GRADE,
										 AAD.final_score AS CSC_SCORE
							FROM APP_APPLICATION AP 
							LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID
							LEFT JOIN app_application_credit_rating ACR ON ACR.application_detail_id = AAD.ID
							LEFT JOIN mas_cs_risk_level MCRL ON MCRL.ID = AAD.risk_level_id
							WHERE AP.application_no='"+applicationNo+"'";
				var NET_INCOME = @"SELECT AP.APPLICATION_NO,
									 ACP.net_income_after_personal_expense
						FROM app_application AP 
						INNER JOIN app_application_customer_personal_expense ACP ON ACP.APPLICATION_ID = AP.ID 
						WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var DEBT = @"SELECT AP.APPLICATION_NO,
									 APP.current_dscr AS CURENT_DSCR,
									 APP.monthly_existing_debt_commitment,
									 AAD.total_ang_monthly_commitment
						FROM APP_APPLICATION AP 
						INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID
						LEFT JOIN app_application_pre_approval APP ON APP.application_detail_id = AAD.ID AND APP.status ='t'
						LEFT JOIN adm_currency AC ON AC.ID = AAD.currency_id
						WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var CBC = @"SELECT AP.APPLICATION_NO,
								 case when cci.result= '0' then 'ACCEPT'
								 WHEN cci.result= '1' then 'REVIEW'
								 ELSE 'REJECT' END AS CBC
					FROM APP_APPLICATION AP 
					INNER JOIN app_application_detail AAD ON AAD.APPLICATION_ID = AP.ID 
					INNER JOIN app_cbc AC ON AC.application_id = AP.ID 
					INNER JOIN app_cbc_consumer_info CCI ON CCI.cbc_id = AC.ID
					WHERE AP.APPLICATION_NO = '"+applicationNo+"' ";
				var EXIST_DEBT = @"SELECT
											AP.APPLICATION_NO,
											ADI.OUTSTANDING_BALANCE
								FROM APP_APPLICATION AP
								INNER JOIN APP_CUSTOMER_DEBT_INFO ADI ON ADI.APPLICATION_ID = AP.ID
					WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var NUM_ACCOUNT = @"SELECT AP.APPLICATION_NO,
									 ACA.ACCOUNT_NUMBER
						FROM APP_APPLICATION AP 
						INNER JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.ID 
						INNER JOIN CUS_CUSTOMER CC ON CC.ID = AAD.CUSTOMER_ID
						INNER JOIN APP_CUS_ACCOUNT ACA ON ACA.CUSTOMER_ID = CC.ID AND ACA.app_detail_id=AAD.ID AND ACA.STATUS='t' 
						WHERE AP.APPLICATION_NO ='"+applicationNo+"'";
				var DEVIATE_REQUEST = @"SELECT 
					  AP.APPLICATION_NO ,
					  JSONB_ARRAY_ELEMENTS(APP.DEVIATION_REQUEST) ->> 'conditionType' AS CONDITION_TYPE,
					  JSONB_ARRAY_ELEMENTS(APP.DEVIATION_REQUEST) ->> 'justification' AS JUSTIFICATION 
					FROM
					  APP_APPLICATION AP
					  INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.ID 
					  AND APP.STATUS = 't'
					WHERE AP.APPLICATION_NO = '"+applicationNo+"'";
				var NUM_EMPLOYEE = @"SELECT ap.application_no,
								 CIB.number_full_time_employee
					 from app_application ap 
					inner join app_application_detail aad on aad.application_id = ap.id
					INNER JOIN cus_customer CC ON CC.ID = AAD.customer_id 
					INNER JOIN app_customer_income_business CIB ON CIB.customer_id = CC.ID
					where ap.application_no ='"+applicationNo+"'";


				var dtEXIST_DEBT = conn.getPostgreSQLDataTable(EXIST_DEBT);
				var dtNUM_ACCOUNT = conn.getPostgreSQLDataTable(NUM_ACCOUNT);
				var dtDEVIATE_REQUEST = conn.getPostgreSQLDataTable(DEVIATE_REQUEST);
				var dtNUM_EMPLOYEE = conn.getPostgreSQLDataTable(NUM_EMPLOYEE);
                var dtLOAN_REQUEST = conn.getPostgreSQLDataTable(LOAN_REQUEST);
				var dtMBLS = conn.getPostgreSQLDataTable(MBLS);
				var dtCOL_ASSET = conn.getPostgreSQLDataTable(COL_ASSET);
				var dtBM = conn.getPostgreSQLDataTable(BM);
				var dtLOAN_PURPOSE = conn.getPostgreSQLDataTable(LOAN_PURPOSE);
				var dtMBLO = conn.getPostgreSQLDataTable(MBLO);
				var dtANALYST = conn.getPostgreSQLDataTable(ANALYST);
				var dtSUB_PURPOSE = conn.getPostgreSQLDataTable(SUB_PURPOSE);
                var dtFACILITY_INFO = conn.getPostgreSQLDataTable(FACILITY_INFO);
                var dtBORO_INFO = conn.getPostgreSQLDataTable(BORO_INFO);
                var dtGUAN_CO_BOR = conn.getPostgreSQLDataTable(GUAN_CO_BOR);
                var dtCOLLATERAL = conn.getPostgreSQLDataTable(COLLATERAL);
                var dtCOLLATERAL_INFO = conn.getPostgreSQLDataTable(COLLATERAL_INFO);
                var dtINCOME = conn.getPostgreSQLDataTable(INCOME);
                var dtCBC = conn.getPostgreSQLDataTable(CBC);
                var dtLOAN_FUND = conn.getPostgreSQLDataTable(LOAN_FUND);
                var dtAPPROVAL = conn.getPostgreSQLDataTable(APPROVAL);
				var dtSCORE = conn.getPostgreSQLDataTable(SCORE);
				var dtCOL_INFO = conn.getPostgreSQLDataTable(COL_INFO);
				var dtGENERAL_INFO = conn.getPostgreSQLDataTable(GENERAL_INFO);
				var dtHYP_COL = conn.getPostgreSQLDataTable(HYP_COL);
				var dtlOAN_FMV = conn.getPostgreSQLDataTable(lOAN_FMV);
				var dtPURPOSE_CODE = conn.getPostgreSQLDataTable(PURPOSE_CODE);
				var dtNET_INCOME = conn.getPostgreSQLDataTable(NET_INCOME);
				var dtDEBT = conn.getPostgreSQLDataTable(DEBT);


				
                var dsFAC_REQUEST_INFO = new ReportDataSource("LOAN_REQUEST", dtLOAN_REQUEST);
				var dsEXIST_DEBT = new ReportDataSource("EXIST_DEBT", dtEXIST_DEBT);
				var dsNUM_ACCOUNT = new ReportDataSource("ACC_NUMBER", dtNUM_ACCOUNT);
				var dsDEVIATE_REQUEST = new ReportDataSource("DEVIATE_REQUEST", dtDEVIATE_REQUEST);
				var dsNUM_EMPLOYEE = new ReportDataSource("NUM_EMPLOYEE", dtNUM_EMPLOYEE);
                var dsMBLS = new ReportDataSource("MBLS", dtMBLS);
                var dsCOL_ASSET = new ReportDataSource("COL_ASSET", dtCOL_ASSET);
                var dsBM = new ReportDataSource("MB", dtBM);
                var dsLOAN_PURPOSE = new ReportDataSource("LOAN_PURPOSE", dtLOAN_PURPOSE);
                var dsMBLO = new ReportDataSource("MBLO", dtMBLO);
                var dsANALYST = new ReportDataSource("ANALYST", dtANALYST);
				var dsSUB_PURPOSE = new ReportDataSource("SUB_PURPOSE", dtSUB_PURPOSE);
				var dsBORO_INFO = new ReportDataSource("BORO_INFO", dtBORO_INFO);
                var dsCOLLATERAL_INFO = new ReportDataSource("COLLATERAL_INFO", dtCOLLATERAL_INFO);
                var dsCOLLATERAL = new ReportDataSource("COLLATERAL", dtCOLLATERAL);
				var dsGUAN_CO_BOR = new ReportDataSource("GUAN_CO_BOR", dtGUAN_CO_BOR);
                var dsINCOME = new ReportDataSource("INCOME", dtINCOME);
                var dsSCORE = new ReportDataSource("SCORE", dtSCORE);
                var dsCBC = new ReportDataSource("CBC", dtCBC);
                var dsAPPROVAL = new ReportDataSource("APPROVAL", dtAPPROVAL);
				var dsCOL_INFO = new ReportDataSource("COL_INFO", dtCOL_INFO);
				var dsGENERAL_INFO= new ReportDataSource("GENERAL_INFO", dtGENERAL_INFO);
				var dsLOAN_FUND = new ReportDataSource("LOAN_FUND", dtLOAN_FUND);
				var dsHYP_COL = new ReportDataSource("HYP_COL", dtHYP_COL);
                var dslOAN_FMV = new ReportDataSource("lOAN_FMV", dtlOAN_FMV);
				var dsPURPOSE_CODE = new ReportDataSource("PURPOSE_CODE", dtPURPOSE_CODE);
				var dsNET_INCOME = new ReportDataSource("NET_INCOME", dtNET_INCOME);
				var dsDEBT = new ReportDataSource("DEBT", dtDEBT);
				var dsFACILITY_INFO = new ReportDataSource("FACILITY_INFO", dtFACILITY_INFO);




                conn.generateReport(ReportViewer1, @"Micro_Loan_Appraisal_Form", null, dsFACILITY_INFO, dsMBLS, dsFAC_REQUEST_INFO, dsCOL_ASSET, dsBM, dsLOAN_PURPOSE, dsMBLO, dsANALYST, dsGUAN_CO_BOR, dsBORO_INFO, dsCOLLATERAL_INFO, dsSUB_PURPOSE, dsCOLLATERAL, dsINCOME, dsSCORE, dsCBC, dsINCOME, dsAPPROVAL, dsCOL_INFO, dsGENERAL_INFO, dsLOAN_FUND, dsHYP_COL, dslOAN_FMV, dsPURPOSE_CODE, dsNET_INCOME, dsDEBT, dsEXIST_DEBT, dsNUM_ACCOUNT, dsDEVIATE_REQUEST, dsNUM_EMPLOYEE);

            }
        }
    }
}