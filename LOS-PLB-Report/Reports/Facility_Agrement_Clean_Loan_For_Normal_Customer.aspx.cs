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
    public partial class Facility_Agrement_Clean_Loan_For_Normal_Customer : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];
                var BORROWER_INFO = @"SELECT
	AP.APPLICATION_NO,
	CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH AS COL_NAME,
CASE
		
		WHEN CC.GENDER = 'MALE' THEN
		'ប្រុស' ELSE'ស្រី' 
	END AS GENDER,
CASE
		
		WHEN ANN.NAME = 'Cambodian' THEN
		'ខ្មែរ' 
	END AS NATIONALITY,
	TRANSLATE ( TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ), '1234567890', '១២៣៤៥៦៧៨៩០' ) AS DOB_DAY,
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
	TRANSLATE ( TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ), '1234567890', '១២៣៤៥៦៧៨៩០' ) AS DOB_YEAR,
	TRANSLATE ( A.ID_NUMBER, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS ID_NUMBER,
	A.IDENTIFICATION_TYPE,
	TRANSLATE ( A.ISSUE_DAY, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS ISSUE_DAY,
	A.ISSUE_MONTH,
	TRANSLATE ( A.ISSUE_YEAR, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS ISSUE_YEAR,
	TRANSLATE(ACC.COMMUNE_KH,'1234567890','១២៣៤៥៦៧៨៩០') as COMMUNE_KH,
	TRANSLATE(APC.PROVINCE_KH,'1234567890','១២៣៤៥៦៧៨៩០') as PROVINCE_KH,
	TRANSLATE(AVC.VILLAGE_KH,'1234567890','១២៣៤៥៦៧៨៩០') as VILLAGE_KH,
	TRANSLATE(ADC.DISTRICT_KH,'1234567890','១២៣៤៥៦៧៨៩០') as DISTRICT_KH,
CASE
		
		WHEN CCA.STREET_NO = '' THEN
		'..........' ELSE TRANSLATE ( CCA.STREET_NO, '1234567890', '១២៣៤៥៦៧៨៩០' ) 
	END AS STEET_NO,
CASE
		
		WHEN CCA.HOUSE_NO = '' THEN
		'..........' ELSE TRANSLATE ( CCA.HOUSE_NO, '1234567890', '១២៣៤៥៦៧៨៩០' ) 
	END AS HOUSE_NO,
CASE
		
		WHEN CCA.GROUP_NO = '' THEN
		'.........' ELSE TRANSLATE ( CCA.GROUP_NO, '1234567890', '១២៣៤៥៦៧៨៩០' ) 
	END AS GROUP_NO 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.
	ID INNER JOIN CUS_CUSTOMER CC ON APP.CUSTOMER_ID = CC.
	ID LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
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
	AND CC.ID = A.CUSTOMER_ID WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var CO_BORO_1 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
		TRANSLATE(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
		'និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END || ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' សញ្ជាតិ ​' ||
CASE
		
		WHEN ANN.NAME = 'Cambodian' THEN
		'ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
	
	WHEN A.IDENTIFICATION_TYPE IS NULL THEN
	'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN A.ID_NUMBER IS NULL THEN
	'.........' ELSE TRANSLATE(A.ID_NUMBER ,'1234567890','១២៣៤៥៦៧៨៩០')
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_DAY,'1234567890','១២៣៤៥៦៧៨៩០') 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_YEAR,'1234567890','១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '
	||TRANSLATE(CCA.HOUSE_NO,'1234567890','១២៣៤៥៦៧៨៩០')||' ផ្លូវ '||TRANSLATE(CCA.STREET_NO,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ក្រុង-ស្រុក-ខណ្ឌ '||TRANSLATE(ADC.DISTRICT_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||APC.PROVINCE_KH||' ព្រះរាជាណាចក្រកម្ពុជា '
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID 
	AND ASD.STATUS = 't'
	INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
	ID LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
  INNER JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	INNER JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	INNER JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	INNER JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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
                var CO_BORRO_2 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
		TRANSLATE(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
		'និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END || ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' សញ្ជាតិ ​' ||
CASE
		
		WHEN ANN.NAME = 'Cambodian' THEN
		'ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
	
	WHEN A.IDENTIFICATION_TYPE IS NULL THEN
	'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN A.ID_NUMBER IS NULL THEN
	'.........' ELSE TRANSLATE(A.ID_NUMBER ,'1234567890','១២៣៤៥៦៧៨៩០')
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_DAY,'1234567890','១២៣៤៥៦៧៨៩០') 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_YEAR,'1234567890','១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '
	||TRANSLATE(CCA.HOUSE_NO,'1234567890','១២៣៤៥៦៧៨៩០')||' ផ្លូវ '||TRANSLATE(CCA.STREET_NO,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ក្រុង-ស្រុក-ខណ្ឌ '||TRANSLATE(ADC.DISTRICT_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||APC.PROVINCE_KH||' ព្រះរាជាណាចក្រកម្ពុជា '
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID 
	AND ASD.STATUS = 't'
	INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
	ID LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
  INNER JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	INNER JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	INNER JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	INNER JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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
                var PURPOSE = @"SELECT
    AP.APPLICATION_NO,
    'ឥណទានមានកាលកំណត់' AS sub_product,
    AC.currency as currency_en,
	CASE 
	WHEN AC.currency = 'USD'  THEN 'USD '||TRANSLATE(to_char(apd.applied_amount, 'fm999G999G999G999G999G999D00'),'1234567890,.','១២៣៤៥៦៧៨៩០.,')
	WHEN AC.currency = 'KHR' THEN 'KHR ' ||TRANSLATE(to_char(apd.applied_amount, 'fm999G999G999G999G999G999D00'),'1234567890','១២៣៤៥៦៧៨៩០')
	end as applied_amount,
    TRANSLATE(to_char(apd.annual_interest_rate, 'fm999G999G999G999G999G999D00'),'1234567890','១២៣៤៥៦៧៨៩០') as annual_interest_rate,
    TRANSLATE(to_char(APD.tenor, 'fm999G999G999G999G999G999'),'1234567890','១២៣៤៥៦៧៨៩០') as tenor_kh
FROM
    APP_APPLICATION AP
INNER JOIN app_application_detail apd ON apd.application_id = ap.ID
INNER JOIN adm_repayment_type ART ON ART.ID = APD.repayment_type_id
INNER JOIN adm_currency AC ON AC.ID = APD.currency_id
                                WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var PURPOSEDT_1 = @"SELECT * FROM
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
                                            AP.APPLICATION_NO = '" + applicationNo + "' ) A WHERE  A.NUM = 1;";
                var PREFIX_NAME_BOR = @"SELECT 
  AP.application_no,
  CASE
	WHEN CC.GENDER='MALE' THEN concat('លោក ',CC.FAMILY_NAME_KH,' ',CC.GIVEN_NAME_KH)
	WHEN CC.GENDER = 'FEMALE' THEN CONCAT('លោកស្រី  ',CC.FAMILY_NAME_KH,' ',CC.GIVEN_NAME_KH)
	END AS FULL_NAME,
	translate(ACI.id_number, '1234567890', '១២៣៤៥៦៧៨៩០') as id_number,
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
	 FROM app_application AP 
INNER JOIN app_application_detail APD ON APD.application_id = AP.ID
INNER JOIN cus_customer CC ON CC.ID = APD.customer_id
INNER JOIN APP_CUSTOMER_IDENTIFICATION ACI ON CC.ID = ACI.CUSTOMER_ID AND ACI.STATUS = 't'
INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = ACI.IDENTIFICATION_TYPE_ID
                                        WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var PREFIX_SUB_NAME = @"SELECT 
                                            AP.application_no,
                                            CASE
                                                WHEN CC.GENDER = 'MALE' THEN CONCAT('លោក ', CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH)
                                                WHEN CC.GENDER = 'FEMALE' THEN CONCAT('លោកស្រី ', CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH)
                                            END AS FULL_NAME,
		                                        ast.customer_type,
		                                        ALF.CODE
                                        FROM 
                                            app_application AP 
                                        INNER JOIN app_application_detail APD ON APD.application_id = AP.ID
                                        INNER JOIN adm_loan_fund ALF ON ALF.ID = APD.loan_fund_id
                                        INNER JOIN 
                                            app_supplementary AST ON AST.application_id = AP.ID AND AST.status = 't'
		                                        and ast.customer_type = 'CO_BORROWER'
                                        INNER JOIN 
                                            app_supplementary_detail ASD ON ASD.supplementary_id = AST.ID AND ASD.status = 't'
                                        INNER JOIN 
                                            cus_customer CC ON CC.ID = ASD.customer_id
                                        WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var REPATMENT = @"SELECT
	AP.APPLICATION_NO,

	CASE WHEN ART.CODE = 'EMI' THEN 'ត្រូវបង់បណ្តាក់ប្រចាំខែនូវចំនួនប្រាក់ស្មើៗគ្នារួមមានការ
ប្រាក់  និងការបង់រំលោះប្រាក់ដើមដែលចំនួននេះ  នឹងកំណ
ត់ដោយធនាគារហើយចំនួនប្រាក់សងបណ្តាក់ចុងក្រោយនឹង
ត្រូវយកមកកែសម្រួល ឬគណនាទៅតាមវិធីសាស្រ្តមួយដើម្បី
ធ្វើយ៉ាងណា  ឲ្យប្រាក់សងបណ្តាក់ប្រចាំខែអាចសងឥណទាន
មានកាលកំណត់ទាំងស្រុងនៅពេលផុតអំឡុងពេលឥណទាន
។ការសងបណ្តាក់ត្រូវចាប់ផ្តើមមួយខែ      បន្ទាប់ពីឥណទាន
ត្រូវបានបញ្ចេញ    (ចំនួនបង់ប្រចាំខែអាចនឹងប្រែប្រួលទៅ
តាមដំណាក់កាល       និងចំនួនឥណទានដែលបានបញ្ចេញ
យោងតាមតារាងបង់ប្រាក់)។    ធនាគាររក្សាសិទិ្ធក្នុងការកែ
ប្រែចំនួនប្រាក់សងបណ្តាក់ប្រចាំខែ   ដែលនឹងអាចផ្លាស់ប្តូរ
មកពីការសងឥណទានមុនកាលកំណត់ខ្លះ ឬមកពីមូលហេតុ
ដទៃទៀត។'
	 WHEN ART.CODE = 'EMI_GRACE_PERIOD' THEN 'ត្រូវបង់តែការប្រាក់ចំនួន .....ខែនិងត្រូវបង់បណ្តាក់ប្រចាំខែ
នូវចំនួនប្រាក់ស្មើៗគ្នារួមមានការប្រាក់និង
ការបង់រំលោះប្រាក់ដើមដែលចំនួននេះនឹងកំណត់ដោយធនា
គារក្នុងរយៈពេល.....ខែហើយចំនួនប្រាក់សងបណ្តាក់ចុងក្រោ
យនឹងត្រូវយកមកកែសម្រួលឬគណនាទៅតាមវិធីសាស្រ្តមួយ
ដើម្បី ធ្វើយ៉ាងណាឲ្យប្រាក់សងបណ្តាក់ប្រចាំខែអាចសងឥណ
ទានមានកាលកំណត់ទាំងស្រុងនៅពេលផុតអំឡុងពេលឥណ
ទាន។ ការសងបណ្តាក់ត្រូវចាប់ផ្តើមមួយខែបន្ទាប់ពី     ឥណ
ទានត្រូវបានបញ្ចេញ   (ចំនួនបង់ប្រចាំខែអាចនឹងប្រែប្រួល
ទៅតាមដំណាក់កាល   និងចំនួនឥណទានដែលបានបញ្ចេញ
យោងតាមតារាងបង់ប្រាក់)។    ធនាគាររក្សាសិទិ្ធក្នុងការកែ
ប្រែចំនួនប្រាក់សងបណ្តាក់ប្រចាំខែដែលនឹងអាចផ្លាស់ប្តូរម
កពីការសងឥណទានមុនកាលកំណត់ខ្លះ   ឬមកពីមូលហេតុ
ដទៃទៀត។
'
	WHEN ART.CODE = 'DECLINE' THEN 'ត្រូវបង់បណ្តាក់ប្រចាំខែនូវចំនួនទឹកប្រាក់ដែលរួមមានការ
បង់រំលោះប្រាក់ដើមស្មើៗគ្នា  និងការបង់ថយការប្រាក់ប្រចាំ
ខែ ស្របទៅតាមប្រាក់ដើមដែលនៅសល់ដែលចំនួននេះនឹង
កំណត់ដោយធនាគារ       ហើយចំនួនប្រាក់សងបណ្តាក់ចុង
ក្រោយនឹងត្រូវយកមកកែសម្រួល  ឬគណនាទៅតាម វិធីសា
ស្រ្ត  មួយដើម្បីធ្វើយ៉ាងណាឲ្យប្រាក់សងបណ្តាក់ប្រចាំខែអាច
សងឥណទានមានកាលកំណត់ទាំងស្រុងនៅពេលផុតអំឡុង
ពេលឥណទាន ។ការសងបណ្តាក់ត្រូវចាប់ផ្តើមមួយខែបន្ទាប់
ពីឥណទានត្រូវបានបញ្ចេញ។    ធនាគាររក្សាសិទិ្ធក្នុងការកែ
ប្រែចំនួនប្រាក់សងបណ្តាក់ប្រចាំខែ  ដែលនឹងអាចផ្លាស់ប្តូរ
មកពីការសងឥណទានមុនកាលកំណត់ខ្លះ  ឬមកពីមូលហេ
តុដទៃទៀត។
'
   WHEN ART.CODE = 'BALLOON' THEN 'ត្រូវបង់ការប្រាក់ជារៀងរាល់ខែ និងត្រូវបង់ចំនួនប្រាក់ដើម
សរុបនៅចុងគ្រា  ដែលចំនួននេះនឹងកំណត់ដោយធនាគារ
ហើយចំនួនប្រាក់សងបណ្តាក់ចុងក្រោយ  នឹងត្រូវយកមក
កែសម្រួល  ឬគណនាទៅតាមវិធីសាស្រ្តមួយ  ដើម្បីធ្វើយ៉ាង
ណាឲ្យប្រាក់សងបណ្តាក់ប្រចាំខែ   អាចសងឥណទានមាន
កាលកំណត់ទាំង  ស្រុងនៅពេលផុតអំឡុងពេលឥណទាន។
ការសងបណ្តាក់ត្រូវចាប់ផ្តើមមួយខែបន្ទាប់ពីឥណទានត្រូវ
បានបញ្ចេញ។   ធនាគាររក្សាសិទិ្ធក្នុងការកែប្រែចំនួនប្រាក់
សងបណ្តាក់ប្រចាំខែដែល  នឹងអាចផ្លាស់ប្តូរមកពីការសង
ឥណទានមុនកាលកំណត់ខ្លះឬមកពីមូលហេតុដទៃទៀត។
'
 END AS REPAYMENT_TYPE
FROM
	APP_APPLICATION AP
	INNER JOIN app_application_detail apd ON apd.application_id = ap.
	ID INNER JOIN adm_repayment_type ART ON ART.ID = APD.repayment_type_id
                                     WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var PURPOSEDT_2 = @"SELECT * FROM
                                    (
                                        SELECT 
                                            ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
                                            AP.APPLICATION_NO ,
                                            ' , '||APD.PURPOSE_DETAIL as PURPOSEDT
                                        FROM 
                                            APP_APPLICATION AP
                                            INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
                                            INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
                                            INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
                                        WHERE 
                                            AP.APPLICATION_NO = '" + applicationNo + "' ) A WHERE  A.NUM = 2";
                var PURPOSEDT_3 = @"SELECT * FROM
                                    (
                                        SELECT 
                                            ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
                                            AP.APPLICATION_NO ,
                                            ' , '||APD.PURPOSE_DETAIL as PURPOSEDT
                                        FROM 
                                            APP_APPLICATION AP
                                            INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
                                            INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
                                            INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
                                        WHERE 
                                            AP.APPLICATION_NO = '" + applicationNo + "' ) A  WHERE  A.NUM = 3";
                var PURPOSEDT_4 = @"SELECT * FROM
                                    (
                                        SELECT 
                                            ROW_NUMBER() OVER(ORDER BY APD.ID) AS NUM ,
                                            AP.APPLICATION_NO ,
                                            ' , '||APD.PURPOSE_DETAIL as PURPOSEDT
                                        FROM 
                                            APP_APPLICATION AP
                                            INNER JOIN APP_APPLICATION_DETAIL APPD ON APPD.APPLICATION_ID = AP.ID 
                                            INNER JOIN APP_LOAN_PURPOSE LP ON LP.APPLICATION_DETAIL_ID = APPD.ID AND LP.STATUS = 't'
                                            INNER JOIN APP_LOAN_PURPOSE_DETAIL APD ON APD.LOAN_PURPOSE_ID = LP.ID AND APD.STATUS = 't'
                                        WHERE 
                                            AP.APPLICATION_NO = '" + applicationNo + "') A WHERE  A.NUM = 4";
                var TRANCE = @"SELECT ROW_NUMBER
                              ( ) OVER ( ORDER BY DT.ID ) AS NUM,
                              AP.APPLICATION_NO,
                              DT.PRINCIPLE_AMOUNT,
                            CASE
    
                                WHEN CUR.CURRENCY IS NULL THEN
                                '............................' ELSE CUR.CURRENCY 
                              END AS CURRENCY,
                            CASE
    
                                WHEN KH_FUNCTION ( CAST ( DT.PRINCIPLE_AMOUNT AS INTEGER ) ) IS NULL THEN
                                '...........................' ELSE KH_FUNCTION ( CAST ( DT.PRINCIPLE_AMOUNT AS INTEGER ) ) 
                              END AS PRINCIPLE_AMOUNT_KH,
                            CASE
    
                                WHEN CUR.CURRENCY = 'USD' THEN
                                'ដុល្លារអាមេរិក' 
                                WHEN CUR.CURRENCY = 'KHR' THEN
                                'ប្រាក់រៀល' 
                                WHEN CUR.CURRENCY = 'THB' THEN
                                'ប្រាក់បាត' 
                              END AS CURRENCY_KH,
                            CASE
    
                                WHEN DT.ACCOUNT_NUMBER IS NULL THEN
                                '.................................' ELSE DT.ACCOUNT_NUMBER 
                              END AS ACCOUNT_NUMBER 
                            FROM
                              APP_APPLICATION AP
                              INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.
                              ID INNER JOIN APP_DISBURSE_TRANCES DT ON DT.APP_DETAIL_ID = APP.ID 
                              AND DT.STATUS = 't'
                              INNER JOIN ADM_CURRENCY CUR ON CUR.ID = DT.CURRENCY_ID
                             WHERE AP.APPLICATION_NO='" + applicationNo + "'";
                var GUANRANTOR_LOAN = @"SELECT ROW_NUMBER
	                                        ( ) OVER ( ORDER BY GU.ID ) AS num,
	                                        AP.APPLICATION_NO,
	                                        CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH AS COL_NAME,
	                                        A.ID_NUMBER,
	                                        A.ISSUE_DATE 
                                        FROM
	                                        APP_APPLICATION AP
	                                        LEFT JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	                                        AND AST.STATUS = 't' 
	                                        AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	                                        LEFT JOIN APP_GUARANTOR GU ON GU.SUPPLEMENTARY_ID = AST.
	                                        ID LEFT JOIN CUS_CUSTOMER CC ON GU.CUSTOMER_ID = CC.ID --IDENTIFICATION
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
		                                        TO_CHAR( CI.ISSUED_DATE, 'DD/MM/YYYY' ) AS ISSUE_DATE 
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
	                                        ap.application_no = '" + applicationNo + "'";
                var C0_BORRO_3 = @"SELECT
	* 
FROM
	(
	SELECT ROW_NUMBER
		( ) OVER ( ORDER BY asd.ID ) AS num,
		AP.APPLICATION_NO,
		TRANSLATE(A.ID_NUMBER,'1234567890','១២៣៤៥៦៧៨៩០') AS ID_NUMBER,
		'និងឈ្មោះ  ' || CC.FAMILY_NAME_KH || ' ' || CC.GIVEN_NAME_KH || ' ភេទ ' ||
	CASE
			
			WHEN CC.GENDER = 'MALE' THEN
			'ប្រុស' ELSE'ស្រី' 
		END || ' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'DD' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' ខែ ' ||
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
	END || ' ឆ្នាំ ' || TRANSLATE(TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' ),'1234567890','១២៣៤៥៦៧៨៩០') || ' សញ្ជាតិ ​' ||
CASE
		
		WHEN ANN.NAME = 'Cambodian' THEN
		'ខ្មែរ' ELSE'..........' 
	END || ' កាន់ ' ||
CASE
	
	WHEN A.IDENTIFICATION_TYPE IS NULL THEN
	'..........' ELSE A.IDENTIFICATION_TYPE 
	END || ' លេខ ' ||
CASE
	
	WHEN A.ID_NUMBER IS NULL THEN
	'.........' ELSE TRANSLATE(A.ID_NUMBER ,'1234567890','១២៣៤៥៦៧៨៩០')
	END || ' ចុះថ្ងៃទី ' ||
CASE
	
	WHEN A.ISSUE_DAY IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_DAY,'1234567890','១២៣៤៥៦៧៨៩០') 
	END || ' ខែ ' ||
CASE
	
	WHEN A.ISSUE_MONTH IS NULL THEN
	'..........' ELSE A.ISSUE_MONTH 
	END || ' ឆ្នាំ ' ||
CASE
	
	WHEN A.ISSUE_YEAR IS NULL THEN
	'..........' ELSE TRANSLATE(A.ISSUE_YEAR,'1234567890','១២៣៤៥៦៧៨៩០')||' មានអាសយដ្ឋានបច្ចុប្បន្នស្ថិតនៅ ផ្ទះលេខ '
	||TRANSLATE(CCA.HOUSE_NO,'1234567890','១២៣៤៥៦៧៨៩០')||' ផ្លូវ '||TRANSLATE(CCA.STREET_NO,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ភូមិ '||TRANSLATE(AVC.VILLAGE_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ឃុំ-សង្កាត់ '||TRANSLATE(ACC.COMMUNE_KH,'1234567890','១២៣៤៥៦៧៨៩០')
	||' ក្រុង-ស្រុក-ខណ្ឌ '||TRANSLATE(ADC.DISTRICT_KH,'1234567890','១២៣៤៥៦៧៨៩០')||' ខេត្ត-រាជធានី '||APC.PROVINCE_KH||' ព្រះរាជាណាចក្រកម្ពុជា '
	END AS SUB_BORROWER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID 
	AND AST.STATUS = 't' 
	AND AST.CUSTOMER_TYPE = 'CO_BORROWER'
	INNER JOIN APP_SUPPLEMENTARY_DETAIL ASD ON ASD.SUPPLEMENTARY_ID = AST.ID 
	AND ASD.STATUS = 't'
	INNER JOIN CUS_CUSTOMER CC ON ASD.CUSTOMER_ID = CC.
	ID LEFT JOIN ADM_NATIONALITY ANN ON ANN.ID = CC.NATIONALITY_ID
	LEFT JOIN CUS_PERMANENT_ADDRESS CCA ON CCA.CUSTOMER_ID = CC.ID 
	AND CCA.STATUS = 't'
  INNER JOIN ADM_PROVINCE_CBC APC ON APC.ID = CCA.PROVINCE_ID
	INNER JOIN ADM_VILLAGE_CBC AVC ON AVC.ID = CCA.VILLAGE_ID
	INNER JOIN ADM_COMMUNE_CBC ACC ON ACC.ID = CCA.COMMUNE_ID
	INNER JOIN ADM_DISTRICT_CBC ADC ON ADC.ID = CCA.DISTRICT_ID
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
                var PREFIX_SUB_NAME_1 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY asd.ID ) AS num,
		                                        AP.application_no,
		                                        CONCAT ( CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH ) AS FULL_NAME,
		                                        TRANSLATE ( ACI.id_number, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS id_number,
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
		                                        app_application AP
		                                        INNER JOIN app_application_detail APD ON APD.application_id = AP.
		                                        ID INNER JOIN app_supplementary AST ON AST.application_id = AP.ID 
		                                        AND AST.CUSTOMER_TYPE = 'CO_BORROWER' 
		                                        AND AST.status = 't'
		                                        INNER JOIN app_supplementary_detail ASD ON ASD.supplementary_id = AST.ID 
		                                        AND ASD.status = 't'
		                                        INNER JOIN cus_customer CC ON CC.ID = ASD.customer_id
		                                        INNER JOIN CUS_IDENTIFICATION ACI ON CC.ID = ACI.CUSTOMER_ID 
		                                        AND ACI.STATUS = 't'
		                                        INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = ACI.IDENTIFICATION_TYPE_ID 
	                                        WHERE AP.APPLICATION_NO='" + applicationNo + "') A  WHERE A.num = 1";
                var PREFIX_SUB_NAME_2 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY asd.ID ) AS num,
		                                        AP.application_no,
		                                        CONCAT ( CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH ) AS FULL_NAME,
		                                        TRANSLATE ( ACI.id_number, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS id_number,
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
		                                        app_application AP
		                                        INNER JOIN app_application_detail APD ON APD.application_id = AP.
		                                        ID INNER JOIN app_supplementary AST ON AST.application_id = AP.ID 
		                                        AND AST.CUSTOMER_TYPE = 'CO_BORROWER' 
		                                        AND AST.status = 't'
		                                        INNER JOIN app_supplementary_detail ASD ON ASD.supplementary_id = AST.ID 
		                                        AND ASD.status = 't'
		                                        INNER JOIN cus_customer CC ON CC.ID = ASD.customer_id
		                                        INNER JOIN CUS_IDENTIFICATION ACI ON CC.ID = ACI.CUSTOMER_ID 
		                                        AND ACI.STATUS = 't'
		                                        INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = ACI.IDENTIFICATION_TYPE_ID 
	                                        WHERE AP.APPLICATION_NO='" + applicationNo + "') A  WHERE A.num = 2";
                var PREFIX_SUB_NAME_3 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY asd.ID ) AS num,
		                                        AP.application_no,
		                                        CONCAT ( CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH ) AS FULL_NAME,
		                                        TRANSLATE ( ACI.id_number, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS id_number,
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
		                                        app_application AP
		                                        INNER JOIN app_application_detail APD ON APD.application_id = AP.
		                                        ID INNER JOIN app_supplementary AST ON AST.application_id = AP.ID 
		                                        AND AST.CUSTOMER_TYPE = 'CO_BORROWER' 
		                                        AND AST.status = 't'
		                                        INNER JOIN app_supplementary_detail ASD ON ASD.supplementary_id = AST.ID 
		                                        AND ASD.status = 't'
		                                        INNER JOIN cus_customer CC ON CC.ID = ASD.customer_id
		                                        INNER JOIN CUS_IDENTIFICATION ACI ON CC.ID = ACI.CUSTOMER_ID 
		                                        AND ACI.STATUS = 't'
		                                        INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = ACI.IDENTIFICATION_TYPE_ID 
	                                        WHERE AP.APPLICATION_NO='" + applicationNo + "') A  WHERE A.num = 3";
                var PREFIX_SUB_NAME_4 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY asd.ID ) AS num,
		                                        AP.application_no,
		                                        CONCAT ( CC.FAMILY_NAME_KH, ' ', CC.GIVEN_NAME_KH ) AS FULL_NAME,
		                                        TRANSLATE ( ACI.id_number, '1234567890', '១២៣៤៥៦៧៨៩០' ) AS id_number,
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
		                                        app_application AP
		                                        INNER JOIN app_application_detail APD ON APD.application_id = AP.
		                                        ID INNER JOIN app_supplementary AST ON AST.application_id = AP.ID 
		                                        AND AST.CUSTOMER_TYPE = 'CO_BORROWER' 
		                                        AND AST.status = 't'
		                                        INNER JOIN app_supplementary_detail ASD ON ASD.supplementary_id = AST.ID 
		                                        AND ASD.status = 't'
		                                        INNER JOIN cus_customer CC ON CC.ID = ASD.customer_id
		                                        INNER JOIN CUS_IDENTIFICATION ACI ON CC.ID = ACI.CUSTOMER_ID 
		                                        AND ACI.STATUS = 't'
		                                        INNER JOIN ADM_IDENTIFICATION_TYPE IT ON IT.ID = ACI.IDENTIFICATION_TYPE_ID 
	                                        WHERE AP.APPLICATION_NO='" + applicationNo + "') A  WHERE A.num = 4";
                var BRANCH = @"SELECT MB.APPLICATION_NO,
                            MB.BRANCH_KH FROM BRANCH_PLB MB
                            WHERE MB.APPLICATION_NO='" + applicationNo + "'";
                var CBC_FEE = @"SELECT
    AP.application_no,
		CASE WHEN CBC.CBC_FEE IS NULL THEN '............'
		ELSE 
    translate(to_char(CBC.cbc_fee, '99999.99'), '0123456789', '០១២៣៤៥៦៧៨៩') END as cbc_fee,
		' '||CBC.currency as currency
FROM
    APP_APPLICATION AP
INNER JOIN
    APP_CBC CBC ON CBC.application_id = AP.ID
WHERE
    AP.application_no = '" + applicationNo + "'";
                var TICK = @"SELECT
	AP.application_no,
	AST.customer_type,
	ALF.NAME
	FROM
	app_application AP
	LEFT JOIN app_application_detail APD ON APD.application_id = AP.
	ID
	LEFT JOIN app_supplementary AST ON AST.application_id = AP.ID 
	AND AST.status = 't'
	LEFT JOIN app_guarantor ASD ON ASD.supplementary_id = AST.id
	AND ASD.status = 't'
	LEFT JOIN ADM_LOAN_FUND ALF ON ALF.ID = APD.loan_fund_id
                        WHERE AP.APPLICATION_NO='" + applicationNo + "'";

                var dtBORROWER_INFO = conn.getPostgreSQLDataTable(BORROWER_INFO);
                var dtCO_BORO_1 = conn.getPostgreSQLDataTable(CO_BORO_1);
                var dtCO_BORRO_2 = conn.getPostgreSQLDataTable(CO_BORRO_2);
				var dtC0_BORO_3 = conn.getPostgreSQLDataTable(C0_BORRO_3);
                var dtPURPOSE = conn.getPostgreSQLDataTable(PURPOSE);
                var dtPURPOSEDT_1 = conn.getPostgreSQLDataTable(PURPOSEDT_1);
                var dtPREFIX_NAME_BOR = conn.getPostgreSQLDataTable(PREFIX_NAME_BOR);
                var dtPREFIX_SUB_NAME = conn.getPostgreSQLDataTable(PREFIX_SUB_NAME);
                var dtREPATMENT = conn.getPostgreSQLDataTable(REPATMENT);
                var dtPURPOSEDT_2 = conn.getPostgreSQLDataTable(PURPOSEDT_2);
                var dtPURPOSEDT_3 = conn.getPostgreSQLDataTable(PURPOSEDT_3);
                var dtPURPOSEDT_4 = conn.getPostgreSQLDataTable(PURPOSEDT_4);
                var dtTRANCE = conn.getPostgreSQLDataTable(TRANCE);
                var dtGUANRANTOR_LOAN = conn.getPostgreSQLDataTable(GUANRANTOR_LOAN);
                var dtBRANCH = conn.getPostgreSQLDataTable(BRANCH);
                var dtPREFIX_SUB_NAME_2 = conn.getPostgreSQLDataTable(PREFIX_SUB_NAME_2);
                var dtPREFIX_SUB_NAME_3 = conn.getPostgreSQLDataTable(PREFIX_SUB_NAME_3);
                var dtPREFIX_SUB_NAME_1 = conn.getPostgreSQLDataTable(PREFIX_SUB_NAME_1);
                var dtPREFIX_SUB_NAME_4 = conn.getPostgreSQLDataTable(PREFIX_SUB_NAME_4);
				var dtCBC_FEE = conn.getPostgreSQLDataTable(CBC_FEE);
				var dtTICK = conn.getPostgreSQLDataTable(TICK);

                var dsBORROWER_INFO = new ReportDataSource("BORROWER_INFO", dtBORROWER_INFO);
                var dsCO_BORO_1 = new ReportDataSource("CO_BORO_1", dtCO_BORO_1);
                var dsCO_BORRO_2 = new ReportDataSource("C0_BORRO_2", dtCO_BORRO_2);
				var dsCO_BORO_3 = new ReportDataSource("CO_BORO_3", dtC0_BORO_3);
                var dsPURPOSE = new ReportDataSource("PURPOSE", dtPURPOSE);
                var dsPURPOSEDT_1 = new ReportDataSource("PURPOSEDT_1", dtPURPOSEDT_1);
                var dsPREFIX_NAME_BOR = new ReportDataSource("PREFIX_NAME_BOR", dtPREFIX_NAME_BOR);
                var dsPREFIX_SUB_NAME = new ReportDataSource("PREFIX_SUB_NAME", dtPREFIX_SUB_NAME);
                var dsREPATMENT = new ReportDataSource("REPATMENT", dtREPATMENT);
                var dsPURPOSEDT_2 = new ReportDataSource("PURPOSEDT_2", dtPURPOSEDT_2);
                var dsPURPOSEDT_3 = new ReportDataSource("PURPOSEDT_3", dtPURPOSEDT_3);
                var dsPURPOSEDT_4 = new ReportDataSource("PURPOSEDT_4", dtPURPOSEDT_4);
                var dsTRANCE = new ReportDataSource("TRANCE", dtTRANCE);
                var dsGUANRANTOR_LOAN = new ReportDataSource("GUANRANTOR_LOAN", dtGUANRANTOR_LOAN);
                var dsBRANCH = new ReportDataSource("BRANCH", dtBRANCH);
                var dsPREFIX_SUB_NAME_2 = new ReportDataSource("PREFIX_SUB_NAME_2", dtPREFIX_SUB_NAME_2);
                var dsPREFIX_SUB_NAME_3 = new ReportDataSource("PREFIX_SUB_NAME_3", dtPREFIX_SUB_NAME_3);
                var dsPREFIX_SUB_NAME_1 = new ReportDataSource("PREFIX_SUB_NAME_1", dtPREFIX_SUB_NAME_1);
                var dsPREFIX_SUB_NAME_4 = new ReportDataSource("PREFIX_SUB_NAME_4", dtPREFIX_SUB_NAME_4);
				var dsCBC_FEE = new ReportDataSource("CBC_FEE",dtCBC_FEE);
				var dsTICK = new ReportDataSource("TICK",dtTICK);


                conn.generateReport(ReportViewer1, @"Facility_Agrement_Clean_Loan_For_Normal_Customer", null,dsCBC_FEE,dsTICK, dsPREFIX_SUB_NAME_1,dsPREFIX_SUB_NAME_2,dsPREFIX_SUB_NAME_3,dsPREFIX_SUB_NAME_4, dsCO_BORO_3,dsBRANCH, dsGUANRANTOR_LOAN, dsBORROWER_INFO, dsCO_BORO_1, dsCO_BORRO_2, dsPURPOSE, dsPURPOSEDT_1, dsPREFIX_NAME_BOR, dsPREFIX_SUB_NAME, dsREPATMENT, dsPURPOSEDT_2, dsPURPOSEDT_3, dsPURPOSEDT_4, dsTRANCE, dsBORROWER_INFO);


            }
        }
    }
}