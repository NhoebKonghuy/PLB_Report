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
    public partial class Micro_Guarantee_Agreement : System.Web.UI.Page
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
                                      A.ISSUE_DAY,
	                                    A.ISSUE_MONTH ,
	                                    A.ISSUE_YEAR ,
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
		                                    ap.application_no = '"+applicationNo+"'  ) A WHERE  A.num = 1";
				var BRANCH_NAME = @"SELECT ap.application_no,
                                    mb.name
                                     FROM
                                    app_application ap 
                                    inner join app_application_detail apd on apd.application_id = ap.id
                                    inner join mas_branch mb on mb.id = apd.branch_id
                                    WHERE AP.APPLICATION_NO='"+applicationNo+"'";
				var APPROVAL_DATE = @"SELECT to_char(aas.complete_date, 'DD/MM/YYYY')
                                     from app_application ap 
                                    inner join app_application_stage aas on aas.application_id = ap.id and aas.application_stages='APPROVAL'
							    	WHERE
									ap.application_no = '" + applicationNo + "'";
				var GUANRATOR_INFO_2 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY ag.ID ) AS num,
                                          AP.APPLICATION_NO,
                                          ' អ្នករួមធានាឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
                                            CASE
                                            WHEN CC.GENDER = 'MALE' THEN
                                            'ប្រុស' ELSE'ស្រី' END
                                        ||' សញ្ជាតិ ​'||
	                                        CASE
                                            WHEN ANN.NAME = 'Cambodian' THEN
                                            'ខ្មែរ' ELSE '..........' END
                                        ||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
                                          TO_CHAR( CC.DATE_OF_BIRTH, 'DD' )||' ខែ'||
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

                                          TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' )


                                        ||' ឯកសារសម្គាល់អត្តសញ្ញាណ '||
                                        CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	                                        CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE A.ID_NUMBER END||' ចុះថ្ងៃទី '||
                                          CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE A.ISSUE_DAY END ||' ខែ '||
	                                        CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	                                        CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE A.ISSUE_YEAR END AS SUB_BORROWER
                                        FROM
                                          APP_APPLICATION AP
                                          INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	                                        INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	                                        ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id  
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
		                                        ap.application_no = '"+applicationNo+"'  ) A  WHERE  A.num = 2";
				var GUANRATOR_INFO_3 = @"SELECT
	                                        * 
                                        FROM
	                                        (
	                                        SELECT ROW_NUMBER
		                                        ( ) OVER ( ORDER BY ag.ID ) AS num,
                                          AP.APPLICATION_NO,
                                          ' អ្នករួមធានាឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
                                            CASE
                                            WHEN CC.GENDER = 'MALE' THEN
                                            'ប្រុស' ELSE'ស្រី' END
                                        ||' សញ្ជាតិ ​'||
	                                        CASE
                                            WHEN ANN.NAME = 'Cambodian' THEN
                                            'ខ្មែរ' ELSE '..........' END
                                        ||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
                                          TO_CHAR( CC.DATE_OF_BIRTH, 'DD' )||' ខែ'||
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

                                          TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' )


                                        ||' ឯកសារសម្គាល់អត្តសញ្ញាណ '||
                                        CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	                                        CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE A.ID_NUMBER END||' ចុះថ្ងៃទី '||
                                          CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE A.ISSUE_DAY END ||' ខែ '||
	                                        CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	                                        CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE A.ISSUE_YEAR END AS SUB_BORROWER
                                        FROM
                                          APP_APPLICATION AP
                                          INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	                                        INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	                                        ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id  
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
		                                        ap.application_no = '"+applicationNo+"' ) A WHERE A.num = 3";

                var GUARANTOR_INFO_4 = @"SELECT
	                                * 
                                FROM
	                                (
	                                SELECT ROW_NUMBER
		                                ( ) OVER ( ORDER BY ag.ID ) AS num,
                                  AP.APPLICATION_NO,
                                  ' អ្នករួមធានាឈ្មោះ  '||CC.FAMILY_NAME_KH|| ' ' || CC.GIVEN_NAME_KH ||' ភេទ '||
                                    CASE
                                    WHEN CC.GENDER = 'MALE' THEN
                                    'ប្រុស' ELSE'ស្រី' END
                                ||' សញ្ជាតិ ​'||
	                                CASE
                                    WHEN ANN.NAME = 'Cambodian' THEN
                                    'ខ្មែរ' ELSE '..........' END
                                ||' ថ្ងៃខែឆ្នាំកំណើត​​ ថ្ងៃទី '||
                                  TO_CHAR( CC.DATE_OF_BIRTH, 'DD' )||' ខែ'||
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

                                  TO_CHAR( CC.DATE_OF_BIRTH, 'YYYY' )


                                ||' ឯកសារសម្គាល់អត្តសញ្ញាណ '||
                                CASE WHEN A.IDENTIFICATION_TYPE IS NULL THEN '..........'ELSE A.IDENTIFICATION_TYPE END ||' លេខ '||
	                                CASE WHEN A.ID_NUMBER IS NULL THEN '.........' ELSE A.ID_NUMBER END||' ចុះថ្ងៃទី '||
                                  CASE WHEN A.ISSUE_DAY IS NULL THEN '..........' ELSE A.ISSUE_DAY END ||' ខែ '||
	                                CASE WHEN A.ISSUE_MONTH IS NULL THEN '..........' ELSE A.ISSUE_MONTH END||' ឆ្នាំ '||
	                                CASE WHEN A.ISSUE_YEAR IS NULL THEN '..........' ELSE A.ISSUE_YEAR END AS SUB_BORROWER
                                FROM
                                  APP_APPLICATION AP
                                  INNER JOIN APP_SUPPLEMENTARY AST ON AST.APPLICATION_ID = AP.ID AND AST.STATUS='t' AND AST.CUSTOMER_TYPE = 'PERSONAL_GUARANTOR'
	                                INNER JOIN app_guarantor ag ON ag.supplementary_id = AST.
	                                ID INNER JOIN cus_customer cc ON cc.ID = ag.customer_id  
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
		                                ap.application_no = '"+applicationNo+"'  ) A WHERE  A.num = 4";



                var dtGUANRATOR_INFO = conn.getPostgreSQLDataTable(GUANRATOR_INFO);
                var dtBRANCH_NAME = conn.getPostgreSQLDataTable(BRANCH_NAME);
                var dtAPPROVAL_DATE = conn.getPostgreSQLDataTable(APPROVAL_DATE);
                var dtGUANRATOR_INFO_2 = conn.getPostgreSQLDataTable(GUANRATOR_INFO_2);
                var dtGUANRATOR_INFO_3 = conn.getPostgreSQLDataTable(GUANRATOR_INFO_3);
                var dtGUARANTOR_INFO_4 = conn.getPostgreSQLDataTable(GUARANTOR_INFO_4);


                var dsGUANRATOR_INFO = new ReportDataSource("GUANRATOR_INFO", dtGUANRATOR_INFO);
                var dsBRANCH_NAME = new ReportDataSource("BRANCH_NAME", dtBRANCH_NAME);
                var dsAPPROVAL_DATE = new ReportDataSource("APPROVAL_DATE", dtAPPROVAL_DATE);
                var dsGUANRATOR_INFO_2 = new ReportDataSource("GUANRATOR_INFO_2", dtGUANRATOR_INFO_2);
                var dsGUANRATOR_INFO_3 = new ReportDataSource("GUANRATOR_INFO_3", dtGUANRATOR_INFO_3);
                var dsGUARANTOR_INFO_4 = new ReportDataSource("GUARANTOR_INFO_4", dtGUARANTOR_INFO_4);



                conn.generateReport(ReportViewer1, @"Micro_Guarantee_Agreement", null,  dsGUANRATOR_INFO, dsBRANCH_NAME, dsAPPROVAL_DATE, dsGUANRATOR_INFO_2, dsGUANRATOR_INFO_3, dsGUARANTOR_INFO_4);


            }
        }
    }
}