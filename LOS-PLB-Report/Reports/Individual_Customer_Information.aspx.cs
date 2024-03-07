using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace LOS_PLB_Report.Reports
{
    public partial class Individual_Customer_Information : System.Web.UI.Page
    {
        DBConnect conn = new DBConnect();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var applicationNo = Request.QueryString["application_no"];
                var sqlUserdetial = @"SELECT
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
	                                    an.NAME 
                                    FROM
	                                    cus_customer cc
	                                    INNER JOIN adm_nationality an ON an.ID = cc.nationality_id
                                        inner join adm_prefix ap on ap.id = cc.prefix_id";
				var sqlAdressCorre = @"SELECT
										cca.street_no,
										cca.group_no,
										apc.province_kh,
										avc.village_kh,
										adc.district_kh,
										acc.commune_kh,
										ac.country_en, 
										cca.house_no,
										CASE WHEN ac.country_en='CAMBODIA' then 'កម្ពុជា' END AS country_kh 
									FROM
										cus_customer cc
										INNER JOIN cus_corr_address cca on cca.customer_id = cc.id
									 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
										INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
										INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
										INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
										INNER JOIN adm_country ac ON ac.ID = cca.country_id";
				var sqlAdressPerma = @"SELECT
											apc.province_kh,
											avc.village_kh,
											adc.district_kh,
											acc.commune_kh,
											ac.country_en, 
											cca.house_no,
											CASE WHEN ac.country_en='CAMBODIA' then 'កម្ពុជា' END AS country_kh 
										FROM
											cus_customer cc
											INNER JOIN cus_permanent_address cca on cca.customer_id = cc.id
										 INNER JOIN adm_province_cbc apc ON apc.ID = cca.province_id
											INNER JOIN adm_village_cbc avc ON avc.ID = cca.village_id
											INNER JOIN adm_commune_cbc acc ON acc.ID = cca.commune_id
											INNER JOIN adm_district_cbc adc ON adc.ID = cca.district_id
											INNER JOIN adm_country ac ON ac.ID = cca.country_id";
				var sqlIdentity = @"SELECT ci.id_number,
									ci.issued_date,
									ci.expiry_date,
									ait.name from cus_customer cc
									inner join cus_identification ci on ci.customer_id = cc.id
									inner join adm_identification_type ait on ait.id = ci.identification_type_id";
				var sqlMarital = @"SELECT ams.name
									from cus_customer cc 
									inner join adm_marital_status ams on ams.id = cc.marital_status_id";
				var sqlForBank = @"SELECT
										acc.NAME CLASSTI,
										act.NAME AS CAT,
										cc.cif_number,
										cc.short_name,
										'' as resident
									FROM
										cus_customer cc
										INNER JOIN adm_customer_classification acc ON acc.ID = cc.cus_classification_id
										INNER JOIN adm_customer_category act ON act.id = cc.customer_category_id
										";
                var sqlBusDetail = @"SELECT ID FROM APP_APPLICATION WHERE application_no = '" + applicationNo + "'";


                var dtUserdetial = conn.getPostgreSQLDataTable(sqlUserdetial);
				var dtAdressCorre = conn.getPostgreSQLDataTable(sqlAdressCorre);
				var dtAdressPerma = conn.getPostgreSQLDataTable(sqlAdressPerma);
				var dtIdentity = conn.getPostgreSQLDataTable(sqlIdentity);
				var dtMarital = conn.getPostgreSQLDataTable(sqlMarital);
				var dtForBank = conn.getPostgreSQLDataTable(sqlForBank);

                var dtBusDetail = conn.getPostgreSQLDataTable(sqlBusDetail);

                var dsBusDetail = new ReportDataSource("DS_INFORMATION_APPLICANT", dtBusDetail);
                var dsUserdetial = new ReportDataSource("DS_INFORMATION_USER", dtUserdetial);
                var dsAdressCorre = new ReportDataSource("DS_ADRESS_CORR", dtAdressCorre);
                var dsAdressPerma = new ReportDataSource("DS_ADRESS_PER", dtAdressPerma);
                var dsIdentity = new ReportDataSource("DS_IDENTIFICATION", dtIdentity);
                var dsMarital = new ReportDataSource("DS_MARITAL_STATUS", dtMarital);
                var dsForBank = new ReportDataSource("DS_FOR_BANK", dtForBank);


                conn.generateReport(ReportViewer1, @"Individual_Customer_Information", null, dsBusDetail, dsUserdetial, dsAdressCorre, dsAdressPerma, dsIdentity, dsMarital, dsForBank);

            }
        }
    }
}