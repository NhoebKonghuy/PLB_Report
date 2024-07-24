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
    public partial class Colleteral_Receipt_Update : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];
                var OWNERSHIP = @"SELECT
									  AP.application_no,
										AO.NAME 
									FROM
									APP_APPLICATION AP
									INNER JOIN APP_COL_COLLATERAL_ASSET CCA ON CCA.APPLICATION_ID = AP.ID 
									AND CCA.STATUS = 't'
									INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = CCA.ID 
									AND CS.STATUS = 't'
									INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID 
									AND COL.STATUS = 't'
									INNER JOIN ADM_OWNERSHIP AO ON AO.ID = COL.ownership_id WHERE application_no = '" + applicationNo + "'";
                var OWNER_COL_INFO = @"SELECT
								ap.application_no,
								translate(ACI.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') as ID_NUMBER,
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
								END AS type_id,
									convert_to_khmer_number(CAST(EXTRACT(DAY FROM ACI.issued_date) AS int)) AS Day,
							CASE
		
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'JAN' THEN
									'មករា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'FEB' THEN
									'កុម្ភៈ' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'MAR' THEN
									'មីនា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'APR' THEN
									'មេសា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'MAY' THEN
									'ឧសភា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'JUN' THEN
									'មិថុនា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'JUL' THEN
									'កក្កដា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'AUG' THEN
									'សីហា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'SEP' THEN
									'កញ្ញា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'OCT' THEN
									'តុលា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'NOV' THEN
									'វិច្ឆិកា' 
									WHEN TO_CHAR( ACI.issued_date, 'MON' ) = 'DEC' THEN
									'ធ្នូ' 
								END AS MONTH,
								convert_to_khmer_number(CAST(EXTRACT(YEAR FROM ACI.issued_date) AS int)) AS YEAR,
								cc.family_name_kh || ' ' || cc.given_name_kh AS col_full_name
							FROM
								APP_APPLICATION AP
								INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID 
								AND ACCA.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID 
								AND CS.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID 
								AND COL.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID 
								AND CCD.STATUS = 't'
								INNER JOIN cus_customer cc ON col.customer_id = cc.
								ID
								LEFT JOIN app_customer_identification ACI ON ACI.CUSTOMER_ID = CC.
								ID  AND ACI.APPLICATION_ID = AP.ID  
								LEFT JOIN adm_identification_type IT ON IT.ID = ACI.identification_type_ID 
							WHERE ap.application_no ='" + applicationNo + "'";
                var COL_INFO = @"SELECT
								ap.application_no,
								Col.collateral_title,
								TRANSLATE(CCD.collateral_name, '1234567890', '១២៣៤៥៦៧៨៩០') AS collateral_no,
							  convert_to_khmer_number(CAST(EXTRACT(DAY FROM CCD.issued_date) AS int)) AS Day,
							CASE
		
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'JAN' THEN
									'មករា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'FEB' THEN
									'កុម្ភៈ' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'MAR' THEN
									'មីនា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'APR' THEN
									'មេសា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'MAY' THEN
									'ឧសភា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'JUN' THEN
									'មិថុនា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'JUL' THEN
									'កក្កដា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'AUG' THEN
									'សីហា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'SEP' THEN
									'កញ្ញា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'OCT' THEN
									'តុលា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'NOV' THEN
									'វិច្ឆិកា' 
									WHEN TO_CHAR( CCD.issued_date, 'MON' ) = 'DEC' THEN
									'ធ្នូ' 
								END AS MONTH,
								convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CCD.issued_date) AS int)) AS YEAR,
							CASE
		
									WHEN ACCI.NAME = 'VILLAGE' THEN
									'ភូមិ' 
									WHEN ACCI.NAME = 'COMMUNE/SANGKAT' THEN
									'ឃុំ/សង្កាត់' 
									WHEN ACCI.NAME = 'DISTRICT/KHAN' THEN
									'ស្រុក/ខណ្ឌ' 
									WHEN ACCI.NAME = 'PROVINCIAL' THEN
									'ខេត្ត' 
									WHEN ACCI.NAME = 'MINISTRY' THEN
									'ក្រសួង' 
									WHEN ACCI.NAME = 'OTHERS' THEN
									' ផ្សេង' ELSE'.........' 
								END AS issue_by 
							FROM
								APP_APPLICATION AP
								INNER JOIN APP_COL_COLLATERAL_ASSET ACCA ON ACCA.APPLICATION_ID = AP.ID 
								AND ACCA.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_SECURE CS ON CS.APP_COLLATERAL_ASSET_ID = ACCA.ID 
								AND CS.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL COL ON COL.ID = CS.APP_COLLATERAL_ID 
								AND COL.STATUS = 't'
								INNER JOIN APP_COL_COLLATERAL_DETAIL CCD ON CCD.COLLATERAL_ID = COL.ID 
								AND CCD.STATUS = 't'
								INNER JOIN cus_customer cc ON col.customer_id = cc.
								ID LEFT JOIN adm_issue_by ACCI ON ACCI.ID = CCD.issue_by_id
							WHERE ap.application_no = '" + applicationNo + "'";
                var BORROWER_INFO = @"SELECT 
							  AP.APPLICATION_NO,
							  CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH AS COL_NAME,
							CASE

								WHEN CC.GENDER = 'MALE' THEN
								'ប្រុស' ELSE'ស្រី' 
							  END AS GENDER,
							CASE
    
								WHEN ANN.NAME = 'Cambodian' THEN
								'ខ្មែរ' 
							  END AS NATIONALITY,
							  TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
							  A.IDENTIFICATION_TYPE,
							  A.ISSUE_DAY,
								A.ISSUE_MONTH ,
								A.ISSUE_YEAR 
							FROM
							  APP_APPLICATION AP
							  INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.ID
							  INNER JOIN CUS_CUSTOMER CC ON APP.CUSTOMER_ID = CC.id
							  LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID 
							  --IDENTIFICATION
							  LEFT JOIN (
							  SELECT
								CI.CREATED,
								CI.APPLICATION_ID,
								CI.CUSTOMER_ID,
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
								END AS IDENTIFICATION_TYPE,
								CI.ID_NUMBER,

							convert_to_khmer_number(CAST(EXTRACT(DAY FROM CI.issued_date) AS int)) AS  ISSUE_DAY,
							CASE
    
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JAN' THEN
								'មករា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'FEB' THEN
								'កុម្ភៈ' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAR' THEN
								'មីនា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'APR' THEN
								'មេសា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAY' THEN
								'ឧសភា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUN' THEN
								'មិថុនា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUL' THEN
								'កក្កដា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'AUG' THEN
								'សីហា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'SEP' THEN
								'កញ្ញា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'OCT' THEN
								'តុលា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'NOV' THEN
								'វិច្ឆិកា' 
								WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'DEC' THEN
								'ធ្នូ' 
							  END AS ISSUE_MONTH,
								convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CI.issued_date) AS int)) AS  ISSUE_YEAR
							  FROM
								APP_CUSTOMER_IDENTIFICATION CI
								INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = CI.IDENTIFICATION_TYPE_ID
								INNER JOIN (
								SELECT MIN
								  ( CI.ID ) AS CREATED,
								  CI.APPLICATION_ID,
								  CI.CUSTOMER_ID 
								FROM
								  APP_CUSTOMER_IDENTIFICATION CI 
								WHERE
								  CI.STATUS = 't' 
								GROUP BY
								  CI.APPLICATION_ID,
								  CI.CUSTOMER_ID 
								) A ON A.CREATED = CI.ID 
								AND A.APPLICATION_ID = CI.APPLICATION_ID 
								AND A.CUSTOMER_ID = CI.CUSTOMER_ID 
							  WHERE
								CI.STATUS = 't' 
							  ) A ON A.APPLICATION_ID = AP.ID 
							  AND CC.ID = A.CUSTOMER_ID 
							WHERE ap.application_no = '" + applicationNo + "'";
                var CO_BORROWER_1 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
    concat(' និងឈ្មោះ  ',CC.FAMILY_NAME_KH,' ',CC.GIVEN_NAME_KH,' កាន់ ',
    A.IDENTIFICATION_TYPE,' លេខ ',translate(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០')
,' ចុះថ្ងៃទី ',A.ISSUE_DAY,' ខែ ',A.ISSUE_MONTH,' ឆ្នាំ ',A.ISSUE_YEAR) as sub
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID AND ASD.STATUS='t'
  INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
  ID 
  LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID 
  --IDENTIFICATION
  LEFT JOIN (
  SELECT
    CI.CREATED,
    CI.APPLICATION_ID,
    CI.CUSTOMER_ID,
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
    END AS IDENTIFICATION_TYPE,
    CI.ID_NUMBER,
 convert_to_khmer_number(CAST(EXTRACT(DAY FROM CI.issued_date) AS int)) AS ISSUE_DAY,
CASE
    
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'DEC' THEN
    'ធ្នូ' 
  END AS ISSUE_MONTH,
	convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CI.issued_date) AS int)) AS ISSUE_YEAR
  FROM
    APP_CUSTOMER_IDENTIFICATION CI
    INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = CI.IDENTIFICATION_TYPE_ID
    INNER JOIN (
    SELECT MIN
      ( CI.ID ) AS CREATED,
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    FROM
      APP_CUSTOMER_IDENTIFICATION CI 
    WHERE
      CI.STATUS = 't' 
    GROUP BY
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    ) A ON A.CREATED = CI.ID 
    AND A.APPLICATION_ID = CI.APPLICATION_ID 
    AND A.CUSTOMER_ID = CI.CUSTOMER_ID 
  WHERE
    CI.STATUS = 't' 
  ) A ON A.APPLICATION_ID = AP.ID 
  AND CC.ID = A.CUSTOMER_ID  
											WHERE AP.APPLICATION_NO='" + applicationNo+"') A WHERE A.num = 1";
                var CO_BORROWER_2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
    concat(' និងឈ្មោះ  ',CC.FAMILY_NAME_KH,' ',CC.GIVEN_NAME_KH,' កាន់ ',
    A.IDENTIFICATION_TYPE,' លេខ ',translate(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០')
,' ចុះថ្ងៃទី ',A.ISSUE_DAY,' ខែ ',A.ISSUE_MONTH,' ឆ្នាំ ',A.ISSUE_YEAR) as sub
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID AND ASD.STATUS='t'
  INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
  ID 
  LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID 
  --IDENTIFICATION
  LEFT JOIN (
  SELECT
    CI.CREATED,
    CI.APPLICATION_ID,
    CI.CUSTOMER_ID,
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
    END AS IDENTIFICATION_TYPE,
    CI.ID_NUMBER,
 convert_to_khmer_number(CAST(EXTRACT(DAY FROM CI.issued_date) AS int)) AS ISSUE_DAY,
CASE
    
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'DEC' THEN
    'ធ្នូ' 
  END AS ISSUE_MONTH,
	convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CI.issued_date) AS int)) AS ISSUE_YEAR
  FROM
    APP_CUSTOMER_IDENTIFICATION CI
    INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = CI.IDENTIFICATION_TYPE_ID
    INNER JOIN (
    SELECT MIN
      ( CI.ID ) AS CREATED,
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    FROM
      APP_CUSTOMER_IDENTIFICATION CI 
    WHERE
      CI.STATUS = 't' 
    GROUP BY
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    ) A ON A.CREATED = CI.ID 
    AND A.APPLICATION_ID = CI.APPLICATION_ID 
    AND A.CUSTOMER_ID = CI.CUSTOMER_ID 
  WHERE
    CI.STATUS = 't' 
  ) A ON A.APPLICATION_ID = AP.ID 
  AND CC.ID = A.CUSTOMER_ID  
										WHERE AP.APPLICATION_NO='" + applicationNo+"') A WHERE A.num = 2";
                var CO_BORROWER_3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
    concat(' និងឈ្មោះ  ',CC.FAMILY_NAME_KH,' ',CC.GIVEN_NAME_KH,' កាន់ ',
    A.IDENTIFICATION_TYPE,' លេខ ',translate(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០')
,' ចុះថ្ងៃទី ',A.ISSUE_DAY,' ខែ ',A.ISSUE_MONTH,' ឆ្នាំ ',A.ISSUE_YEAR) as sub
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID AND ASD.STATUS='t'
  INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
  ID 
  LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID 
  --IDENTIFICATION
  LEFT JOIN (
  SELECT
    CI.CREATED,
    CI.APPLICATION_ID,
    CI.CUSTOMER_ID,
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
    END AS IDENTIFICATION_TYPE,
    CI.ID_NUMBER,
 convert_to_khmer_number(CAST(EXTRACT(DAY FROM CI.issued_date) AS int)) AS ISSUE_DAY,
CASE
    
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CI.ISSUED_DATE, 'MON' ) = 'DEC' THEN
    'ធ្នូ' 
  END AS ISSUE_MONTH,
	convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CI.issued_date) AS int)) AS ISSUE_YEAR
  FROM
    APP_CUSTOMER_IDENTIFICATION CI
    INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = CI.IDENTIFICATION_TYPE_ID
    INNER JOIN (
    SELECT MIN
      ( CI.ID ) AS CREATED,
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    FROM
      APP_CUSTOMER_IDENTIFICATION CI 
    WHERE
      CI.STATUS = 't' 
    GROUP BY
      CI.APPLICATION_ID,
      CI.CUSTOMER_ID 
    ) A ON A.CREATED = CI.ID 
    AND A.APPLICATION_ID = CI.APPLICATION_ID 
    AND A.CUSTOMER_ID = CI.CUSTOMER_ID 
  WHERE
    CI.STATUS = 't' 
  ) A ON A.APPLICATION_ID = AP.ID 
  AND CC.ID = A.CUSTOMER_ID  
										WHERE AP.APPLICATION_NO='" + applicationNo+"') A WHERE A.num = 3";


                var dtOWNERSHIP = conn.getPostgreSQLDataTable(OWNERSHIP);
                var dtOWNER_COL_INFO = conn.getPostgreSQLDataTable(OWNER_COL_INFO);
                var dtCOL_INFO = conn.getPostgreSQLDataTable(COL_INFO);
                var dtBORROWER_INFO = conn.getPostgreSQLDataTable(BORROWER_INFO);
                var dtCO_BORROWER_1 = conn.getPostgreSQLDataTable(CO_BORROWER_1);
                var dtCO_BORROWER_2 = conn.getPostgreSQLDataTable(CO_BORROWER_2);
                var dtCO_BORROWER_3 = conn.getPostgreSQLDataTable(CO_BORROWER_3);


                var dsOWNERSHIP = new ReportDataSource("OWNERSHIP", dtOWNERSHIP);
                var dsOWNER_COL_INFO = new ReportDataSource("OWNER_COL_INFO", dtOWNER_COL_INFO);
                var dsCOL_INFO = new ReportDataSource("COL_INFO", dtCOL_INFO);
                var dsBORROWER_INFO = new ReportDataSource("BORROWER_INFO", dtBORROWER_INFO);
                var dsCO_BORROWER_1 = new ReportDataSource("CO_BORROWER_1", dtCO_BORROWER_1);
                var dsCO_BORROWER_2 = new ReportDataSource("CO_BORROWER_2", dtCO_BORROWER_2);
                var dsCO_BORROWER_3 = new ReportDataSource("CO_BORROWER_3", dtCO_BORROWER_3);


                conn.generateReport(ReportViewer1, @"Colleteral_Receipt_Update", null, dsOWNERSHIP, dsOWNER_COL_INFO, dsCOL_INFO, dsBORROWER_INFO, dsCO_BORROWER_1, dsCO_BORROWER_2, dsCO_BORROWER_3);
            }
        }
        

    }
}