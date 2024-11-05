using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_DTO
    {
        public long ISMSLE_Id { get; set; }
        public long ISMSLEPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSMCA_Id { get; set; }
        public long ISMSMSO_Id { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_LeadCode { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_LeadAddress1 { get; set; }
        public string ISMSLE_LeadAddress2 { get; set; }
        public string ISMSLE_LeadAddress3 { get; set; }
        public DateTime? ISMSLE_VisitedDate  { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public string ISMSLE_TypeFlg { get; set; }
        public long ISMSMST_Id { get; set; }
        public string ISMSLE_ContactDesignation { get; set; }
        public string ISMSLE_Reference { get; set; }
        public bool sendemail { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long ISMSLE_StudentStrength { get; set; }
        public long ISMSLE_StaffStrength { get; set; }
        public long ISMSLE_NoOfInstitutions { get; set; }
        public string ISMSLE_Remarks { get; set; }
        public bool ISMSLE_OrderConfirmedFlg { get; set; }
        public string ISMSLE_ClosureRemarks { get; set; }
        public bool ISMSLE_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? FROMDATE { get; set; }
        public DateTime? TODATE { get; set; }
        public long ISMSLE_CreatedBy { get; set; }
        public long ISMSLE_UpdatedBy { get; set; }
        public string return_value { get; set; }
        public bool returnvalue { get; set; }
        public long user_id { get; set; }
        public string ISMSMCA_CategoryName { get; set; }
        public string ISMSMSO_SourceName { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public long ISMSMPR_Id { get; set; }
        public bool ISMSLEPR_ActiveFlag { get; set; }
        public string IVRMMS_Name { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public string ISMSMPR_Remarks { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public bool retbool { get; set; }
        public long HRME_Id { get; set; }
        public string employeename { get; set; }
        public string R_flag { get; set; }
        public long ISMSLE_Pincode { get; set; }
        public Array lead_list { get; set; }
        public Array category_dd { get; set; }
        public Array source_dd { get; set; }
        public Array product_dd { get; set; }
        public Array state_dd { get; set; }
        public Array country_dd { get; set; }
        public Array status_dd { get; set; }
        public Array sales_lead_edit { get; set; }
        public Array sales_lead_edit_product_dd { get; set; }
        public Array sales_lead_edit_product_dd_product { get; set; }
        public Array lead_create_report_list { get; set; }
        public product_list_save1[] product_list_save { get; set; }
        public class product_list_save1
        {
            public long ISMSMPR_Id { get; set; }
        }
    }
}