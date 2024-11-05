
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_Demo_DTO
    {
        public long? ISMSLEDM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMSLEDM_DemoType { get; set; }
        public DateTime ISMSLEDM_DemoDate { get; set; }
        public string ISMSLEDM_ContactPerson { get; set; }
        public string ISMSLEDM_DemoAddress { get; set; }
        public string ISMSLEDM_Remarks { get; set; }
        public bool ISMSLEDM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLEDM_CreatedBy { get; set; }
        public long ISMSLEDM_UpdatedBy { get; set; }
        public long user_id { get; set; }
        public string employeename { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_LeadCode { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public long? ISMSMST_Id { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public long ISMSLEDMPR_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public long ISMSLEDMPR_DiscussionPoints { get; set; }
        public bool ISMSLEDMPR_ActiveFlag { get; set; }
        public string return_status { get; set; }
        public long? ISMSLEDM_Status_Flg { get; set; }
        public long viewall { get; set; }
        public string ISMSLEDMPR_Remarks { get; set; }
        public Array lead_list_dd { get; set; }
        public Array hrme_list_dd { get; set; }
        public Array product_list_dd { get; set; }
        public Array demo_list { get; set; }
        public Array Edit_details_lead_demo { get; set; }
        public Array Lead_Demo_Products_list { get; set; }
        public Array product_dd_s { get; set; }
        public Array view_lead_demo { get; set; }
        public Array demo_response_list { get; set; }
        public Array demo_response_details { get; set; }
        public Array status_demo_master { get; set; }
        public product_demo_master_tempA[] product_demo_master_temp1 { get; set; }
        public product_demo_master_temp[] product_demo_master_temp2 { get; set; }

        public class product_demo_master_temp
        {
            public long ISMSLEDMPR_Id { get; set; }
            public long MI_Id { get; set; }
            public long ISMSLEDM_Id { get; set; }
            public long ISMSMPR_Id { get; set; }
            public long ISMSLEDMPR_DiscussionPoints { get; set; }
            public long ISMSMST_Id { get; set; }
            public string ISMSLEDMPR_Remarks { get; set; }
            public bool ISMSLEDMPR_ActiveFlag { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public long ISMSLEDMPR_CreatedBy { get; set; }
            public long ISMSLEDMPR_UpdatedBy { get; set; }
        }
        public class product_demo_master_tempA
        {
            public long ISMSMPR_Id { get; set; }
            public long ISMSLEDMPR_DiscussionPoints { get; set; }
        }
    }
}