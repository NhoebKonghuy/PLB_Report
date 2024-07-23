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
    public partial class Individual_Customer_Information : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var RefNo = Request.QueryString["RefNo"];
                var DS_INFORMATION_USER = @"SELECT
	cc.ref_no,
	cc.resident,
	ap.name as prefix,
	cc.given_name_en,
	cc.given_name_kh,
	cc.family_name_en,
	cc.family_name_kh,
	cc.phone_number,
	cc.home_phone_number,
	cc.email,
	to_char(cc.date_of_birth,'DD/MM/YYYY') as dob,
	'' as fax_number,
	'' as Dual_Citizenships,
	'' AS office_phone_number,
	an.NAME,
	cib.occupation_description as biz_position,
	cie.position as em_position,
	to_char(cie.start_date, 'DD/MM/YYYY') as start_date 
FROM
	cus_customer cc
	INNER JOIN adm_nationality an ON an.ID = cc.nationality_id
    left join adm_prefix ap on ap.id = cc.prefix_id
	left join cus_income_business cib on cib.customer_id = cc.id 
	left join cus_income_employee cie on cie.customer_id = cc.id   
										where cc.ref_no='" + RefNo + "'";
                var DS_ADRESS_CORR = @"SELECT
	cca.street_no,
	cca.group_no,
	apc.province_kh,
	avc.village_kh,
	adc.district_kh,
	acc.commune_kh,
	ac.country_en, 
	cca.house_no,
	ac.country_kh AS country_kh 
FROM
	cus_customer cc
	INNER JOIN cus_corr_address cca on cca.customer_id = cc.id
 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
	INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
	INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
	INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
	INNER JOIN adm_country ac ON ac.ID = cca.country_id
	
											where cc.ref_no='" + RefNo + "'";
                var DS_ADRESS_PER = @"SELECT
    cc.ref_no,
	apc.province_kh,
	avc.village_kh,
	adc.district_kh,
	acc.commune_kh,
	ac.country_en, 
	cca.house_no,
	cca.street_no,
	cca.group_no,
	ac.country_kh AS country_kh 
FROM
	cus_customer cc
	INNER JOIN cus_permanent_address cca on cca.customer_id = cc.id
 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
	INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
	INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
	INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
	INNER JOIN adm_country ac ON ac.ID = cca.country_id
	
										where cc.ref_no='" + RefNo + "'";
                var DS_IDENTIFICATION_1 = @"SELECT * FROM(
SELECT ROW_NUMBER 
( ) OVER ( ORDER BY ci.ID ) AS num,
cc.ref_no,
ci.id_number,
to_char(CI.issued_date, 'DD/MM/YYYY') as issued_date,
to_char(ci.expiry_date, 'DD/MM/YYYY') as expiry_date,
ait.name from cus_customer cc
inner join cus_identification ci on ci.customer_id = cc.id
inner join adm_identification_type ait on ait.id = ci.identification_type_id
WHERE cc.ref_no = '"+RefNo+"')A WHERE A.NUM=1";
                var DS_IDENTIFICATION_2 = @"SELECT * FROM(
SELECT ROW_NUMBER 
( ) OVER ( ORDER BY ci.ID ) AS num,
cc.ref_no,
ci.id_number,
to_char(CI.issued_date, 'DD/MM/YYYY') as issued_date,
to_char(ci.expiry_date, 'DD/MM/YYYY') as expiry_date,
ait.name from cus_customer cc
inner join cus_identification ci on ci.customer_id = cc.id
inner join adm_identification_type ait on ait.id = ci.identification_type_id
WHERE cc.ref_no = '" + RefNo + "')A WHERE A.NUM=2";
                var DS_IDENTIFICATION_3 = @"SELECT * FROM(
SELECT ROW_NUMBER 
( ) OVER ( ORDER BY ci.ID ) AS num,
cc.ref_no,
ci.id_number,
to_char(CI.issued_date, 'DD/MM/YYYY') as issued_date,
to_char(ci.expiry_date, 'DD/MM/YYYY') as expiry_date,
ait.name from cus_customer cc
inner join cus_identification ci on ci.customer_id = cc.id
inner join adm_identification_type ait on ait.id = ci.identification_type_id
WHERE cc.ref_no = '" + RefNo + "')A WHERE A.NUM=3";
                var DS_FOR_BANK = @"SELECT
	acc.NAME CLASSTI,
	act.NAME AS CAT,
	cc.cif_number,
	cc.short_name,
	'' as resident
FROM
	cus_customer cc
	INNER JOIN adm_customer_classification acc ON acc.ID = cc.cus_classification_id
	INNER JOIN adm_customer_category act ON act.id = cc.customer_category_id
									where cc.ref_no='" + RefNo + "'";
                var DS_BORROWER_INFORMATION = @"SELECT
	AP.APPLICATION_NO,
	APP.CUSTOMER_ID,
	CUS.FAMILY_NAME_KH || ' ' || CUS.GIVEN_NAME_KH AS BORROWER_NAME,
	TO_CHAR( CUS.DATE_OF_BIRTH, 'DD/MM/YYYY' ) AS DOB,
CASE
		
		WHEN CUS.GENDER = 'MALE' THEN
		'ប្រុស' ELSE'ស្រី' 
	END AS GENDER 
FROM
	APP_APPLICATION AP
	INNER JOIN APP_APPLICATION_DETAIL APP ON APP.APPLICATION_ID = AP.
	ID INNER JOIN CUS_CUSTOMER CUS ON CUS.ID = APP.CUSTOMER_ID
									where cus.ref_no='" + RefNo + "'";
				var DS_MARITAL_STATUS = @"SELECT ams.name
 from cus_customer cc 
inner join adm_marital_status ams on ams.id = cc.marital_status_id
WHERE CC.REF_NO = '"+RefNo+"'";

                var dtDS_INFORMATION_USER = conn.getPostgreSQLDataTable(DS_INFORMATION_USER);
                var dtDS_ADRESS_CORR = conn.getPostgreSQLDataTable(DS_ADRESS_CORR);
                var dtDS_ADRESS_PER = conn.getPostgreSQLDataTable(DS_ADRESS_PER);
                var dtDS_IDENTIFICATION_1 = conn.getPostgreSQLDataTable(DS_IDENTIFICATION_1);
                var dtDS_IDENTIFICATION_2 = conn.getPostgreSQLDataTable(DS_IDENTIFICATION_2);
                var dtDS_IDENTIFICATION_3 = conn.getPostgreSQLDataTable(DS_IDENTIFICATION_3);
                var dtDS_FOR_BANK = conn.getPostgreSQLDataTable(DS_FOR_BANK);
                var dtDS_BORROWER_INFORMATION = conn.getPostgreSQLDataTable(DS_BORROWER_INFORMATION);
				var dtDS_MARITAL_STATUS = conn.getPostgreSQLDataTable (DS_MARITAL_STATUS);



                var dsDS_INFORMATION_USER = new ReportDataSource("DS_INFORMATION_USER", dtDS_INFORMATION_USER);
				var dsDs_MARITAL_STATUS = new ReportDataSource("DS_MARITAL_STATUS", dtDS_MARITAL_STATUS);
                var dsDS_ADRESS_CORR = new ReportDataSource("DS_ADRESS_CORR", dtDS_ADRESS_CORR);
                var dsDS_ADRESS_PER = new ReportDataSource("DS_ADRESS_PER", dtDS_ADRESS_PER);
                var dsDS_IDENTIFICATION_1 = new ReportDataSource("DS_IDENTIFICATION_1", dtDS_IDENTIFICATION_1);
                var dsDS_IDENTIFICATION_2 = new ReportDataSource("DS_IDENTIFICATION_2", dtDS_IDENTIFICATION_2);
                var dsDS_IDENTIFICATION_3 = new ReportDataSource("DS_IDENTIFICATION_3", dtDS_IDENTIFICATION_3);
                var dsDS_FOR_BANK = new ReportDataSource("DS_FOR_BANK", dtDS_FOR_BANK);
                var dsDS_BORROWER_INFORMATION = new ReportDataSource("DS_BORROWER_INFORMATION", dtDS_BORROWER_INFORMATION);



                conn.generateReport(ReportViewer1, @"Individual_Customer_Information", null, dsDS_INFORMATION_USER, dsDS_ADRESS_CORR, dsDS_ADRESS_PER, dsDS_IDENTIFICATION_1, dsDS_IDENTIFICATION_2,dsDs_MARITAL_STATUS, dsDS_IDENTIFICATION_3, dsDS_FOR_BANK,  dsDS_BORROWER_INFORMATION);


            }
        }
    }
}