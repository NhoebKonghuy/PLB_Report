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
    public partial class Guarantee_Agreement : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];
                var GUANRATOR_INFO = @"SELECT
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
	                                        convert_to_khmer_number(CAST(EXTRACT(DAY FROM CC.DATE_OF_BIRTH) AS int))AS DOB_DAY,
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
	                                        convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CC.DATE_OF_BIRTH) AS int))AS DOB_YEAR,
	                                        translate(A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩') as ID_NUMBER,
                                          A.IDENTIFICATION_TYPE,
                                          A.ISSUE_DAY,
	                                        A.ISSUE_MONTH ,
	                                        A.ISSUE_YEAR ,
                                          translate(ACC.COMMUNE_KH, '0123456789', '០១២៣៤៥៦៧៨៩') as COMMUNE_KH,
                                          translate(APC.PROVINCE_KH, '0123456789', '០១២៣៤៥៦៧៨៩')as PROVINCE_KH,
                                          translate(AVC.VILLAGE_KH, '0123456789', '០១២៣៤៥៦៧៨៩')as VILLAGE_KH,
                                          translate(ADC.DISTRICT_KH, '0123456789', '០១២៣៤៥៦៧៨៩')as DISTRICT_KH,
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
                                          INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE='PERSONAL_GUARANTOR'
	                                        INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
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
                                            convert_to_khmer_number(CAST(EXTRACT(DAY FROM CI.ISSUED_DATE) AS int)) AS ISSUE_DAY,
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
	                                        convert_to_khmer_number(CAST(EXTRACT(YEAR FROM CI.ISSUED_DATE) AS int)) AS ISSUE_YEAR
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
	                                    WHERE ap.application_no = '" + applicationNo+"' ) A  WHERE  A.num = 1";
				var BRANCH_NAME = @"SELECT AP.BRANCH_KH AS translate FROM BRANCH_PLB AP
                                    WHERE AP.APPLICATION_NO='" + applicationNo+"'";
				var APPROVAL_DATE = @"SELECT ap.application_no,
                                    to_char(aas.complete_date, 'DD/MM/YYYY')
                                     from app_application ap 
                                    inner join app_application_stage aas on aas.application_id = ap.id and aas.application_stages='APPROVAL'
							    	WHERE ap.application_no = '" + applicationNo + "'";
				var GUANRATOR_INFO_2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
		AP.APPLICATION_NO,
		A.IDENTIFICATION_TYPE,
		CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH as full_name,
		TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) as ID_NUMBER,
		' និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END ||
 ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || convert_to_khmer_number ( CAST ( EXTRACT ( DAY FROM CC.DATE_OF_BIRTH ) AS INT ) ) || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || convert_to_khmer_number ( CAST ( EXTRACT ( YEAR FROM CC.DATE_OF_BIRTH ) AS INT ) )||' សញ្ជាតិ '||CASE
	
	WHEN ANN.NAME = 'Cambodian' THEN
	' ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
		
		WHEN A.IDENTIFICATION_TYPE IS NULL THEN
		'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) IS NULL THEN
	'.........' ELSE TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) 
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_DAY AS INT ) ) 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_YEAR AS INT ) ) || ' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ' ||
CASE
		
		WHEN CCA.STREET_NO = '' THEN
		'..........' ELSE CCA.STREET_NO 
	END || ' ផ្លូវ ' ||
CASE
	
	WHEN CCA.HOUSE_NO = '' THEN
	'..........' ELSE CCA.HOUSE_NO 
	END || ' ភូមិ ' || TRANSLATE ( AVC.VILLAGE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ឃុំ-សង្កាត់ ' || TRANSLATE ( ACC.COMMUNE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ក្រុង-ស្រុក-ខណ្ឌ ' || TRANSLATE ( ADC.DISTRICT_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ខេត្ត-រាជធានី ' || TRANSLATE ( APC.PROVINCE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ព្រះរាជាណាចក្រកម្ពុជា ' 
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
	LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
	LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID --IDENTIFICATION
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
	                                        WHERE  ap.application_no = '" + applicationNo+"'  ) A  WHERE  A.num = 2";
				var GUANRATOR_INFO_3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
		AP.APPLICATION_NO,
		A.IDENTIFICATION_TYPE,
		CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH as full_name,
		TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) as ID_NUMBER,
		' និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END ||
 ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || convert_to_khmer_number ( CAST ( EXTRACT ( DAY FROM CC.DATE_OF_BIRTH ) AS INT ) ) || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || convert_to_khmer_number ( CAST ( EXTRACT ( YEAR FROM CC.DATE_OF_BIRTH ) AS INT ) )||' សញ្ជាតិ '||CASE
	
	WHEN ANN.NAME = 'Cambodian' THEN
	' ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
		
		WHEN A.IDENTIFICATION_TYPE IS NULL THEN
		'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) IS NULL THEN
	'.........' ELSE TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) 
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_DAY AS INT ) ) 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_YEAR AS INT ) ) || ' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ' ||
CASE
		
		WHEN CCA.STREET_NO = '' THEN
		'..........' ELSE CCA.STREET_NO 
	END || ' ផ្លូវ ' ||
CASE
	
	WHEN CCA.HOUSE_NO = '' THEN
	'..........' ELSE CCA.HOUSE_NO 
	END || ' ភូមិ ' || TRANSLATE ( AVC.VILLAGE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ឃុំ-សង្កាត់ ' || TRANSLATE ( ACC.COMMUNE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ក្រុង-ស្រុក-ខណ្ឌ ' || TRANSLATE ( ADC.DISTRICT_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ខេត្ត-រាជធានី ' || TRANSLATE ( APC.PROVINCE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ព្រះរាជាណាចក្រកម្ពុជា ' 
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
	LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
	LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID --IDENTIFICATION
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
	                                        WHERE  ap.application_no = '" + applicationNo + "'  ) A  WHERE  A.num = 3";
                var ACC_NUMBER = @"SELECT
									AP.APPLICATION_NO,
								CASE
										WHEN ACA.ACCOUNT_NO IS NULL THEN
										'.........................' ELSE TRANSLATE ( ACA.ACCOUNT_NO, '0123456789', '០១២៣៤៥៦៧៨៩' ) 
									END AS translate
								FROM
									APP_APPLICATION AP
									LEFT JOIN APP_APPLICATION_DETAIL AAD ON AAD.APPLICATION_ID = AP.
									ID LEFT JOIN app_loan_disbuse_response ACA ON ACA.app_detail_id = AAD.ID 
									AND ACA.STATUS = 't'
                                WHERE AP.APPLICATION_NO='" + applicationNo+"'";
                var GUARANTOR_INFO_4 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY ag.ID ) AS num,
		AP.APPLICATION_NO,
		A.IDENTIFICATION_TYPE,
		CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH as full_name,
		TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) as ID_NUMBER,
		' និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END ||
 ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || convert_to_khmer_number ( CAST ( EXTRACT ( DAY FROM CC.DATE_OF_BIRTH ) AS INT ) ) || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || convert_to_khmer_number ( CAST ( EXTRACT ( YEAR FROM CC.DATE_OF_BIRTH ) AS INT ) )||' សញ្ជាតិ '||CASE
	
	WHEN ANN.NAME = 'Cambodian' THEN
	' ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
		
		WHEN A.IDENTIFICATION_TYPE IS NULL THEN
		'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) IS NULL THEN
	'.........' ELSE TRANSLATE ( A.ID_NUMBER, '0123456789', '០១២៣៤៥៦៧៨៩' ) 
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_DAY AS INT ) ) 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE convert_to_khmer_number ( CAST ( A.ISSUE_YEAR AS INT ) ) || ' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ' ||
CASE
		
		WHEN CCA.STREET_NO = '' THEN
		'..........' ELSE CCA.STREET_NO 
	END || ' ផ្លូវ ' ||
CASE
	
	WHEN CCA.HOUSE_NO = '' THEN
	'..........' ELSE CCA.HOUSE_NO 
	END || ' ភូមិ ' || TRANSLATE ( AVC.VILLAGE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ឃុំ-សង្កាត់ ' || TRANSLATE ( ACC.COMMUNE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ក្រុង-ស្រុក-ខណ្ឌ ' || TRANSLATE ( ADC.DISTRICT_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ខេត្ត-រាជធានី ' || TRANSLATE ( APC.PROVINCE_KH, '0123456789', '០១២៣៤៥៦៧៨៩' ) || ' ព្រះរាជាណាចក្រកម្ពុជា ' 
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
	LEFT JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	LEFT JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	LEFT JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	LEFT JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
	LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID --IDENTIFICATION
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
	                                        WHERE ap.application_no = '" + applicationNo + "'  ) A  WHERE  A.num =4";
				var Current = @"SELECT to_char(CURRENT_DATE, 'DD-MM-YYYY');";


                var dtACC_NUMBER = conn.getPostgreSQLDataTable(ACC_NUMBER);
                var dtGUANRATOR_INFO = conn.getPostgreSQLDataTable(GUANRATOR_INFO);
                var dtBRANCH_NAME = conn.getPostgreSQLDataTable(BRANCH_NAME);
                var dtAPPROVAL_DATE = conn.getPostgreSQLDataTable(APPROVAL_DATE);
                var dtGUANRATOR_INFO_2 = conn.getPostgreSQLDataTable(GUANRATOR_INFO_2);
                var dtGUANRATOR_INFO_3 = conn.getPostgreSQLDataTable(GUANRATOR_INFO_3);
                var dtGUARANTOR_INFO_4 = conn.getPostgreSQLDataTable(GUARANTOR_INFO_4);
				var dtCurrent = conn.getPostgreSQLDataTable(Current);

                var dsACC_NUMBER = new ReportDataSource("ACC_NUMBER", dtACC_NUMBER);
                var dsGUANRATOR_INFO = new ReportDataSource("GUANRANTEE1", dtGUANRATOR_INFO);
                var dsBRANCH_NAME = new ReportDataSource("BRANCH", dtBRANCH_NAME);
                var dsAPPROVAL_DATE = new ReportDataSource("APPROVAL", dtAPPROVAL_DATE);
                var dsGUANRATOR_INFO_2 = new ReportDataSource("GUANRANTEE2", dtGUANRATOR_INFO_2);
                var dsGUANRATOR_INFO_3 = new ReportDataSource("GUANRANTEE3", dtGUANRATOR_INFO_3);
                var dsGUARANTOR_INFO_4 = new ReportDataSource("GUARANTEE4", dtGUARANTOR_INFO_4);
				var dsCurrent = new ReportDataSource("Current",dtCurrent);



                conn.generateReport(ReportViewer1, @"Guarantee_Agreement", null, dsCurrent, dsGUANRATOR_INFO, dsBRANCH_NAME, dsAPPROVAL_DATE, dsGUANRATOR_INFO_2, dsGUANRATOR_INFO_3, dsGUARANTOR_INFO_4,dsACC_NUMBER);


            }
        }
    }
}