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
											case when an.name='Cambodian' THEN 'ខ្មែរ​' else '' end as nationality
										FROM
											cus_customer cc
											INNER JOIN adm_nationality an ON an.ID = cc.nationality_id
										  inner join adm_prefix ap on ap.id = cc.prefix_id   
										where cc.ref_no='" + RefNo + "'";
                var DS_ADRESS_CORR = @"SELECT
											apc.province_kh,
											avc.village_kh,
											adc.district_kh,
											cca.house_no,
											acc.commune_kh,
											cca.street_no,
											cca.group_no,
											cc.ref_no,
											CASE WHEN ac.country_en='CAMBODIA' then 'កម្ពុជា' END AS country_kh 
										FROM
											cus_customer cc
											INNER JOIn cus_corr_address cca on cca.customer_id = cc.id
										 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
											INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
											INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
											INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
											INNER JOIN adm_country ac ON ac.ID = cca.country_id
	
											where cc.ref_no='" + RefNo + "'";
                var DS_ADRESS_PER = @"SELECT
										apc.province_kh,
										avc.village_kh,
										adc.district_kh,
										acc.commune_kh,
										cca.house_no,
										cca.street_no,
										cca.group_no,
										cc.ref_no,
										CASE WHEN ac.country_en='CAMBODIA' then 'កម្ពុជា' END AS country_kh 
									FROM
										cus_customer cc
										INNER JOIn cus_permanent_address cca on cca.customer_id = cc.id
									 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
										INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
										INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
										INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
										INNER JOIN adm_country ac ON ac.ID = cca.country_id
	
										where cc.ref_no='" + RefNo + "'";
                var DS_IDENTIFICATION = @"SELECT ci.id_number,
											to_char(ci.issued_date, 'DD/MM/YYYY') as issued_date,
											to_char(ci.expiry_date, 'DD/MM/YYYY') as expiry_date,
											cc.ref_no,
											 CASE
      
												  WHEN AIT.ID_TYPE = 'F' THEN
												  'សៀវភៅគ្រួសារ' 
												  WHEN AIT.ID_TYPE = 'CD' THEN
												  'លិខិតបញ្ជាក់អត្តសញ្ញាណ' 
												  WHEN AIT.ID_TYPE = 'N' THEN
												  'អត្តសញ្ញាណប័ណ្ណ' 
												  WHEN AIT.ID_TYPE = 'P' THEN
												  'លិខិតឆ្លងដែន' 
												  WHEN AIT.ID_TYPE = 'MI' THEN
												  'ឆាយា' 
												  WHEN aIT.ID_TYPE = 'R' THEN
												  'សៀវភៅស្នាក់នៅ' 
												  WHEN aIT.ID_TYPE = 'B' THEN
												  'សំបុត្រកំណើត' 
												END AS IDENTIFICATION_TYPE
											from cus_customer cc
											inner join cus_identification ci on ci.customer_id = cc.id
											inner join adm_identification_type ait on ait.id = ci.identification_type_id
											where cc.ref_no='" + RefNo + "'";
                var DS_MARITAL_STATUS = @"SELECT ams.name,
											cie.company_name,
											 to_char(cie.start_date, 'DD/MM/YYYY') as start_date,
											 cie.position,
											cc.ref_no
											 from cus_customer cc 
											inner join adm_marital_status ams on ams.id = cc.marital_status_id
											inner join cus_income_employee cie on cie.customer_id = cc.id
											Where cc.ref_no ='" + RefNo + "'";
                var DS_FOR_BANK = @"SELECT
										acc.NAME CLASSTI,
										act.NAME AS CAT,
										cc.cif_number,
										cc.short_name,
										'' as resident,
										cc.ref_no
									FROM
										cus_customer cc
										INNER JOIN adm_customer_classification acc ON acc.ID = cc.cus_classification_id
										INNER JOIN adm_customer_category act ON act.id = cc.customer_category_id
									where cc.ref_no='" + RefNo + "'";
                var BRANCH_NAME = @"SELECT cc.ref_no,
											mb.name
											FROM
											cus_customer cc
											inner join mas_branch mb on cc.branch_id = mb.id
									where cc.ref_no='" + RefNo + "'";
                var DS_BORROWER_INFORMATION = @"SELECT
										Cc.REF_NO,
										Cc.FAMILY_NAME_KH || ' ' || Cc.GIVEN_NAME_KH AS BORROWER_NAME,
										TO_CHAR( Cc.DATE_OF_BIRTH, 'DD/MM/YYYY' ) AS DOB,
									CASE
		
											WHEN Cc.GENDER = 'MALE' THEN
											'ប្រុស' ELSE'ស្រី' 
										END AS GENDER 
									FROM
										cus_customer Cc 
									where cc.ref_no='" + RefNo + "'";

                var dtDS_INFORMATION_USER = conn.getPostgreSQLDataTable(DS_INFORMATION_USER);
                var dtDS_ADRESS_CORR = conn.getPostgreSQLDataTable(DS_ADRESS_CORR);
                var dtDS_ADRESS_PER = conn.getPostgreSQLDataTable(DS_ADRESS_PER);
                var dtDS_IDENTIFICATION = conn.getPostgreSQLDataTable(DS_IDENTIFICATION);
                var dtDS_MARITAL_STATUS = conn.getPostgreSQLDataTable(DS_MARITAL_STATUS);
                var dtDS_FOR_BANK = conn.getPostgreSQLDataTable(DS_FOR_BANK);
                var dtBRANCH_NAME = conn.getPostgreSQLDataTable(BRANCH_NAME);
                var dtDS_BORROWER_INFORMATION = conn.getPostgreSQLDataTable(DS_BORROWER_INFORMATION);



                var dsDS_INFORMATION_USER = new ReportDataSource("DS_INFORMATION_USER", dtDS_INFORMATION_USER);
                var dsDS_ADRESS_CORR = new ReportDataSource("DS_ADRESS_CORR", dtDS_ADRESS_CORR);
                var dsDS_ADRESS_PER = new ReportDataSource("DS_ADRESS_PER", dtDS_ADRESS_PER);
                var dsDS_IDENTIFICATION = new ReportDataSource("DS_IDENTIFICATION", dtDS_IDENTIFICATION);
                var dsDS_MARITAL_STATUS = new ReportDataSource("DS_MARITAL_STATUS", dtDS_MARITAL_STATUS);
                var dsDS_FOR_BANK = new ReportDataSource("DS_FOR_BANK", dtDS_FOR_BANK);
                var dsBRANCH_NAME = new ReportDataSource("BRANCH_NAME", dtBRANCH_NAME);
                var dsDS_BORROWER_INFORMATION = new ReportDataSource("DS_BORROWER_INFORMATION", dtDS_BORROWER_INFORMATION);



                conn.generateReport(ReportViewer1, @"Individual_Customer_Information", null, dsDS_INFORMATION_USER, dsDS_ADRESS_CORR, dsDS_ADRESS_PER, dsDS_IDENTIFICATION, dsDS_MARITAL_STATUS, dsDS_FOR_BANK, dsBRANCH_NAME, dsDS_BORROWER_INFORMATION);


            }
        }
    }
}