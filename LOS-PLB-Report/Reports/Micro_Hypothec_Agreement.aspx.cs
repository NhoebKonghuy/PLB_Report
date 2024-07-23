using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace LOS_PLB_Report.Reports
{
    public partial class Micro_Hypothec_Agreement : System.Web.UI.Page
    {
        string applicationNo = "";
        DBConnect conn = new DBConnect();
        //private ReportViewer ReportViewer1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];

                var COL_OWNER = @"SELECT
								  AP.APPLICATION_NO,
								  AV.VILLAGE_CODE 
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
								  LEFT JOIN ADM_VILLAGE_CBC AV ON AV.ID = CCD.VILLAGE_ID 
								WHERE
								  AP.APPLICATION_NO = '"+applicationNo+"' GROUP BY AP.APPLICATION_NO, AV.VILLAGE_CODE";
				var VILLAGE = @"SELECT
								  AP.APPLICATION_NO,
								  AV.VILLAGE_CODE 
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
								  LEFT JOIN ADM_VILLAGE_CBC AV ON AV.ID = CCD.VILLAGE_ID 
								WHERE
								  AP.APPLICATION_NO = '"+applicationNo+"' GROUP BY AP.APPLICATION_NO , AV.VILLAGE_CODE HAVING COUNT(AV.VILLAGE_CODE) = 1";

                var BORROWER = @"SELECT 
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
                      TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០') AS DOB_DAY,
                    CASE
    
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
                        'មករា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
                        'កុម្ភៈ' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
                        'មីនា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
                        'មេសា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
                        'ឧសភា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
                        'មិថុនា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
                        'កក្កដា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
                        'សីហា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
                        'កញ្ញា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
                        'តុលា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
                        'វិច្ឆិកា' 
                        WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
                        'ធ្នូ' 
                      END AS DOB_MONTH,

                      TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០') AS DOB_YEAR,
                      TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
                      A.IDENTIFICATION_TYPE,
                      TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') AS ISSUE_DAY,
	                    A.ISSUE_MONTH ,
	                    TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០') AS ISSUE_YEAR,
	                    TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS COMMUNE_KH,
                      TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS PROVINCE_KH,
	                    TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS VILLAGE_KH,
	                    TRANSLATE(ADC.DISTRICT_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS DISTRICT_KH,
                    CASE
    
                        WHEN CCA.STREET_NO = '' THEN
                        '..........' ELSE TRANSLATE(CCA.STREET_NO, '1234567890', '១២៣៤៥៦៧៨៩០')
                      END AS STEET_NO,
                    CASE
    
                        WHEN CCA.HOUSE_NO = '' THEN
                        '..........' ELSE TRANSLATE(CCA.HOUSE_NO, '1234567890', '១២៣៤៥៦៧៨៩០')
                      END AS HOUSE_NO,
                    CASE
    
                        WHEN CCA.GROUP_NO = '' THEN
                        '.........' ELSE TRANSLATE(CCA.GROUP_NO, '1234567890', '១២៣៤៥៦៧៨៩០') 
                      END AS GROUP_NO 
                    FROM
                      APP_APPLICATION AP
                      INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.ID
                      INNER JOIN CUS_CUSTOMER CC ON APP.CUSTOMER_ID = CC.
                      ID LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
                      ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
                      LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
                      LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
                      LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

                    TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	                    TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
					where ap.application_no='" + applicationNo + "'";
                var CO_BORROWER = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY ag.ID ) AS num,
                                          AP.APPLICATION_NO,
                                          'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
                                            CASE
                                            WHEN CC.GENDER = 'MALE' THEN
                                            'ប្រុស' ELSE'ស្រី' END
                                        ||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
                                          TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
                                        CASE
    
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
                                            'មករា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
                                            'កុម្ភៈ' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
                                            'មីនា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
                                            'មេសា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
                                            'ឧសភា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
                                            'មិថុនា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
                                            'កក្កដា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
                                            'សីហា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
                                            'កញ្ញា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
                                            'តុលា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
                                            'វិច្ឆិកា' 
                                            WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
                                            'ធ្នូ' END
                                        ||' ឆ្នាំ '||

                                          TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

                                        ||' សញ្ជាតិ ​'||
	                                        CASE
                                            WHEN ANN.NAME = 'Cambodian' THEN
                                            'ខ្មែរ' ELSE '..........' END
                                        ||' កាន់ '||
                                        CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	                                        CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
                                          CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	                                        CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	                                        CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
                                            WHEN CCA.HOUSE_NO = '' THEN
                                            '..........' ELSE CCA.HOUSE_NO 
                                          END||' ផ្លូវ '||CASE
    
                                            WHEN CCA.STREET_NO = '' THEN
                                            '..........' ELSE CCA.STREET_NO 
                                          END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
                                        FROM
                                          APP_APPLICATION AP
                                          INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	                                        INNER JOIN APP_SUPPLEMENTARY_DETAIL ag ON ag.supplementary_id = ast.
	                                        ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	                                        LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
                                          ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
                                          LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
                                          LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
                                          LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

                                        TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	                                        TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
										WHERE ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var CO_BORROWER_2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var CO_BORROWER_3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 3";
                var CO_BORROWER_4 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 4";
                var GUARANTOR_1 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY AG.ID ) AS num,
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
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០') AS DOB_DAY,
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' 
  END AS DOB_MONTH,

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០') AS DOB_YEAR,
	AST.CUSTOMER_TYPE,
  TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
  A.IDENTIFICATION_TYPE,
  TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') AS ISSUE_DAY,
	A.ISSUE_MONTH ,
	TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០') AS ISSUE_YEAR,
  TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS COMMUNE_KH,
  TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS PROVINCE_KH,
	TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS VILLAGE_KH,
	TRANSLATE(ADC.DISTRICT_KH, '1234567890', '១២៣៤៥៦៧៨៩០') AS DISTRICT_KH,
CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END AS STEET_NO,
CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END AS HOUSE_NO,
CASE
    
    WHEN CCA.GROUP_NO = '' THEN
    '.........' ELSE CCA.GROUP_NO 
  END AS GROUP_NO 
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE ='MORTGAGOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = asT.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 1";
                var GUARANTOR_2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||'ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'MORTGAGOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 2";
                var GUARANTOR_3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||'ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'MORTGAGOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	WHERE
		ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 3";
                var GUARANTOR_4 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
  AP.APPLICATION_NO,
  'និងឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||'ភេទ '||
    CASE
    WHEN CC.GENDER = 'MALE' THEN
    'ប្រុស' ELSE'ស្រី' END
||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០')||' ខែ'||
CASE
    
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
    'មករា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
    'កុម្ភៈ' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
    'មីនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
    'មេសា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
    'ឧសភា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
    'មិថុនា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
    'កក្កដា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
    'សីហា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
    'កញ្ញា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
    'តុលា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
    'វិច្ឆិកា' 
    WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
    'ធ្នូ' END
||' ឆ្នាំ '||

  TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០')

||' សញ្ជាតិ ​'||
	CASE
    WHEN ANN.NAME = 'Cambodian' THEN
    'ខ្មែរ' ELSE '..........' END
||' កាន់ '||
CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE TRANSLATE(A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០') END||' ចុះថ្ងៃទី '||
  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០') END ||' ខែ '||
	CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE TRANSLATE(A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '||CASE
    
    WHEN CCA.HOUSE_NO = '' THEN
    '..........' ELSE CCA.HOUSE_NO 
  END||' ផ្លូវ '||CASE
    
    WHEN CCA.STREET_NO = '' THEN
    '..........' ELSE CCA.STREET_NO 
  END||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ក្រុង-ស្រុក-ខណ្ឌ  '||TRANSLATE(ACC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||TRANSLATE(APC.PROVINCE_KH, '1234567890', '១២៣៤៥៦៧៨៩០') END AS SUB_BORROWER
FROM
  APP_APPLICATION AP
  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'MORTGAGOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = ast.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
  ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
  LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
  LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
  LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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

TO_CHAR( CI.ISSUED_DATE, 'DD' ) AS ISSUE_DAY,
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
	TO_CHAR( CI.ISSUED_DATE, 'YYYY' ) AS ISSUE_YEAR
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
	            WHERE
		            ap.application_no = '" + applicationNo + "' ) A WHERE A.num = 4";
                var APPROVE_AMOUNT = @"SELECT ap.application_no,
TRANSLATE(to_char(apd.approved_amount, 'fm999G999G999G999G999G999D00'),'1234567890,.','១២៣៤៥៦៧៨៩០.,') as approved_amount,
kh_function(cast(apd.approved_amount as integer)) as approved_amount_kh,
NUM_TO_WORDS(CAST(annual_interest_rate AS numeric)) as annual_interest_rate_kh,
TRANSLATE(to_char(apd.annual_interest_rate, 'fm999G999G999G999G999G999D00'),'1234567890,.','១២៣៤៥៦៧៨៩០.,') as annual_interest_rate,
ac.currency,
case when ac.currency = 'USD' then 'ដុល្លារ'
when ac.currency = 'THB' then 'បាត'
when ac.currency = 'KHR' then 'រៀល'
end as currncy_kh
from app_application ap 
inner join app_application_detail apd on apd.application_id = ap.id 
inner join adm_currency ac on ac.id = apd.currency_id   
                                     where ap.application_no='" + applicationNo + "'";
                var ACC_NUMBER = @"SELECT

                            AP.APPLICATION_NO,
	                        TRANSLATE(ACA.ACCOUNT_NO, '1234567890', '១២៣៤៥៦៧៨៩០') AS account_number
                        FROM
                            APP_APPLICATION AP
                            LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.

                            ID LEFT JOIN app_loan_disbuse_response ACA ON ACA.app_detail_id = AAD.ID

                            AND ACA.STATUS = 't'
                        WHERE AP.APPLICATION_NO= '" + applicationNo + "'";




                var dtCOL_OWNER = conn.getPostgreSQLDataTable(COL_OWNER);
				var dtVILLAGE = conn.getPostgreSQLDataTable(VILLAGE);
				var dtBORROWER = conn.getPostgreSQLDataTable(BORROWER);
                var dtCO_BORROWER = conn.getPostgreSQLDataTable(CO_BORROWER);
				var dtCO_BORROWER_2 = conn.getPostgreSQLDataTable(CO_BORROWER_2);
				var dtCO_BORROWER_3 = conn.getPostgreSQLDataTable(CO_BORROWER_3);
                var dtCO_BORROWER_4 = conn.getPostgreSQLDataTable(CO_BORROWER_4);
				var dtGUARANTOR_1 = conn.getPostgreSQLDataTable(GUARANTOR_1);
                var dtGUARANTOR_2 = conn.getPostgreSQLDataTable(GUARANTOR_2);
				var dtGUARANTOR_3 = conn.getPostgreSQLDataTable(GUARANTOR_3);
				var dtGUARANTOR_4 = conn.getPostgreSQLDataTable(GUARANTOR_4);
                var dtAPPROVE_AMOUNT = conn.getPostgreSQLDataTable(APPROVE_AMOUNT);
                var dtACC_NUMBER = conn.getPostgreSQLDataTable(ACC_NUMBER);




                var dsCOL_OWNER = new ReportDataSource("COL_OWNER", dtCOL_OWNER);
                var dsVILLAGE = new ReportDataSource("VILLAGE", dtVILLAGE);
                var dsBORROWER = new ReportDataSource("BORROWER", dtBORROWER);
                var dsCO_BORROWER = new ReportDataSource("CO_BORROWER", dtCO_BORROWER);
                var dsCO_BORROWER_2 = new ReportDataSource("CO_BORROWER_2", dtCO_BORROWER_2);
                var dsCO_BORROWER_3 = new ReportDataSource("C0_BORROWER_3", dtCO_BORROWER_3);
                var dsCO_BORROWER_4 = new ReportDataSource("CO_BORROWER_4", dtCO_BORROWER_4);
                var dsGUARANTOR_1 = new ReportDataSource("GUARANTOR_1", dtGUARANTOR_1);
                var dsGUARANTOR_2 = new ReportDataSource("GUARANTOR_2", dtGUARANTOR_2);
                var dsGUARANTOR_3 = new ReportDataSource("GUARANTOR_3", dtGUARANTOR_3);
                var dsGUARANTOR_4 = new ReportDataSource("GUARANTOR_4", dtGUARANTOR_4);
                var dsAPPROVE_AMOUNT = new ReportDataSource("APPROVE_AMOUNT", dtAPPROVE_AMOUNT);
                var dsACC_NUMBER = new ReportDataSource("ACC_NUMBER", dtACC_NUMBER);



                generateReportWithSubReport(
									ReportViewer1,
                                    @"Micro_Hypothec_Agreement",
									null,
									dsCOL_OWNER,
									dsVILLAGE,
									dsBORROWER,
									dsCO_BORROWER,
									dsCO_BORROWER_2,
									dsCO_BORROWER_3,
									dsGUARANTOR_1,
									dsGUARANTOR_2,
									dsGUARANTOR_3,
									dsGUARANTOR_4,
									dsAPPROVE_AMOUNT,
                                    dsACC_NUMBER,
                                    dsCO_BORROWER_4
				);

            }
        }

        private void generateReportWithSubReport(ReportViewer reportViewer, string reportName, ReportParameter[] parameters, params ReportDataSource[] reportDataSources)
        {
            reportViewer.SizeToReportContent = true;
            reportViewer.LocalReport.ReportPath = HttpContext.Current.Server.MapPath(String.Format("~/Reports/{0}.rdlc", reportName));
            reportViewer.LocalReport.DataSources.Clear();

            foreach (var item in reportDataSources)
            {
                System.Diagnostics.Debug.WriteLine("item name: " + item.Name);
                reportViewer.LocalReport.DataSources.Add(item);
            }
            // Add new
            var param = new[] { new ReportParameter("param1", "RDLC Sub Report") };
            reportViewer.LocalReport.SetParameters(param);

            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(getSubReport);
            reportViewer.LocalReport.Refresh();
        }

        private void getSubReport(object sender, SubreportProcessingEventArgs e)
        {
            // Add new
            string applicationNo = e.Parameters["ApplicationNo"].Values[0].ToString();
            //string customerId = e.Parameters["CustomerID"].Values[0].ToString();
            //int customerId = int.Parse(e.Parameters["CustomerID"].Values[0].ToString());
            string villageCode = e.Parameters["VillageCode"].Values[0].ToString();

            var DS_OWNER_INFORMATION = @"SELECT DISTINCT
            A.APPLICATION_NO ,
            A.VILLAGE_CODE ,
            A.COL_NAME ,
            A.GENDER ,
            A.NATIONALITY,
            A.DOB_DAY ,
            A.DOB_MONTH ,
            A.DOB_YEAR ,
            A.ID_NUMBER ,
            A.IDENTIFICATION_TYPE,
            A.ISSUE_DATE ,
            A.COMMUNE_KH ,
            A.VILLAGE_KH,
            A.DISTRICT_KH ,
            A.PROVINCE_KH ,
            A.STEET_NO ,
            A.HOUSE_NO ,
            A.GROUP_NO
            FROM 
            ( 
	            SELECT ROW_NUMBER
              ( ) OVER ( ORDER BY COL.ID , COO.ID) AS NUM,
              AP.APPLICATION_NO,
	            COL.ID ,
	            '' AS LAND,
	            AV.VILLAGE_CODE ,
              CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH AS COL_NAME,
            CASE
    
                WHEN CC.GENDER = 'MALE' THEN
                'ប្រុស' ELSE'ស្រី' 
              END AS GENDER,
            CASE
    
                WHEN ANN.NAME = 'Cambodian' THEN
                'ខ្មែរ' 
              END AS NATIONALITY,
              TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ) AS DOB_DAY,
            CASE
    
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JAN' THEN
                'មករា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'FEB' THEN
                'កុម្ភៈ' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAR' THEN
                'មីនា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'APR' THEN
                'មេសា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'MAY' THEN
                'ឧសភា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUN' THEN
                'មិថុនា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'JUL' THEN
                'កក្កដា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'AUG' THEN
                'សីហា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'SEP' THEN
                'កញ្ញា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'OCT' THEN
                'តុលា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'NOV' THEN
                'វិច្ឆិកា' 
                WHEN TO_CHAR( CC.DATE_OF_BIRTH, 'MON' ) = 'DEC' THEN
                'ធ្នូ' 
              END AS DOB_MONTH,
              TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ) AS DOB_YEAR,
              A.ID_NUMBER,
              A.IDENTIFICATION_TYPE,
              A.ISSUE_DATE,
              ACC.COMMUNE_KH,
              APC.PROVINCE_KH,
              AVC.VILLAGE_KH,
              ADC.DISTRICT_KH,
            CASE
    
                WHEN CCA.STREET_NO = '' THEN
                '..........' ELSE CCA.STREET_NO 
              END AS STEET_NO,
            CASE
    
                WHEN CCA.HOUSE_NO = '' THEN
                '..........' ELSE CCA.HOUSE_NO 
              END AS HOUSE_NO,
            CASE
    
                WHEN CCA.GROUP_NO = '' THEN
                '.........' ELSE CCA.GROUP_NO 
              END AS GROUP_NO 
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
	            LEFT JOIN ADM_VILLAGE_CBC AV ON AV.ID = CCD.VILLAGE_ID
	            INNER JOIN APP_COL_COLLATERAL_CO_OWNER COO ON COO.APPLICATION_COLLATERAL_ID = COL.ID AND COO.STATUS = 't'
              INNER JOIN CUS_CUSTOMER CC ON COO.CUSTOMER_ID = CC.
              ID LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.
              ID AND CCA.STATUS = 't' LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
              LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
              LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
              LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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
                TO_CHAR( ISSUED_DATE, 'DD/MM/YYYY' ) AS ISSUE_DATE 
              FROM
                APP_CUSTOMER_IDENTIFICATION CI
                INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = CI.IDENTIFICATION_TYPE_ID
                INNER JOIN (
                  
                SELECT MIN ( CI.ID ) AS CREATED,
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
	            ) A WHERE A.APPLICATION_NO = '" + applicationNo + "' AND A.VILLAGE_CODE = '" + villageCode + "'";
            var DS_CollateralInformation = @"SELECT ROW_NUMBER
	( ) OVER ( ORDER BY COL.ID ) AS NUM,
	AV.VILLAGE_CODE,
	AP.APPLICATION_NO,
CASE
		
		WHEN TY.CODE IN ( '00102', '00103' ) THEN
		RIGHT ( CAST ( CCD.COLLATERAL_NAME AS VARCHAR ), 4 ) ELSE'...................................................................................................................................................................................' 
	END AS HEAD_LAND_TITLE,
	COL.COLLATERAL_TITLE,
	CASE WHEN CCD.COLLATERAL_NAME IS NULL THEN '...............' ELSE
	TRANSLATE ( CCD.COLLATERAL_NAME, '1234567890', '១២៣៤៥៦៧៨៩០' ) END as COLLATERAL_NO,
	CASE WHEN COL.OWNER_NAME IS NULL THEN '.............'
	ELSE COL.OWNER_NAME END AS OWNER_NAME,
	TRANSLATE ( TO_CHAR( CCD.ISSUED_DATE, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០' ) AS ISSUED_DATE_DAY,
CASE
		
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'JAN' THEN
		'មករា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'FEB' THEN
		'កុម្ភៈ' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'MAR' THEN
		'មីនា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'APR' THEN
		'មេសា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'MAY' THEN
		'ឧសភា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'JUN' THEN
		'មិថុនា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'JUL' THEN
		'កក្កដា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'AUG' THEN
		'សីហា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'SEP' THEN
		'កញ្ញា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'OCT' THEN
		'តុលា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'NOV' THEN
		'វិច្ឆិកា' 
		WHEN TO_CHAR( CCD.ISSUED_DATE, 'MON' ) = 'DEC' THEN
		'ធ្នូ' 
	END AS ISSUED_DATE_MONTH,
	TRANSLATE ( TO_CHAR( CCD.ISSUED_DATE, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០' ) AS ISSUED_DATE_YEAR,
	TRANSLATE ( AV.VILLAGE_KH, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS VILLAGE_KH,
	TRANSLATE ( AC.COMMUNE_KH, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS COMMUNE_KH,
	TRANSLATE ( AD.DISTRICT_KH, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS DISTRICT_KH,
	APR.PROVINCE_KH 
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
	LEFT JOIN ADM_COLLATERAL_TYPE TY ON TY.ID = CCD.COLLATERAL_TYPE_ID
	LEFT JOIN ADM_VILLAGE_CBC AV ON AV.ID = CCD.VILLAGE_ID
	LEFT JOIN ADM_COMMUNE_CBC AC ON AC.ID = CCD.COMMUNE_ID
	LEFT JOIN ADM_DISTRICT_CBC AD ON AD.ID = CCD.DISTRICT_ID
	LEFT JOIN ADM_PROVINCE_CBC APR ON APR.ID = CCD.PROVINCE_ID 
                      WHERE AP.APPLICATION_NO = '" + applicationNo + "' AND AV.VILLAGE_CODE = '" + villageCode + "'";

            var dtDS_OWNER_INFORMATION = conn.getPostgreSQLDataTable(DS_OWNER_INFORMATION);
            var dtDS_CollateralInformation = conn.getPostgreSQLDataTable(DS_CollateralInformation);

            var dsDS_OWNER_INFORMATION = new ReportDataSource("DS_OWNER_INFORMATION_2", dtDS_OWNER_INFORMATION);
            var dsDS_CollateralInformation = new ReportDataSource("DS_COLLATERAL_INFORMATION", dtDS_CollateralInformation);
            conn.subReportDataSource(e, dsDS_CollateralInformation, dsDS_OWNER_INFORMATION);

        }
    }
}