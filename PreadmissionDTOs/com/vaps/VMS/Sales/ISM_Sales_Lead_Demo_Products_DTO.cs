using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_Demo_Products_DTO
    {
        public long ISMSLEDMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLEDM_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public long ISMSLEDMPR_DiscussionPoints { get; set; }
        public long ISMSMST_Id { get; set; }
        public string ISMSLEDMPR_Remarks { get; set; }
        public bool ISMSLEDMPR_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long ISMSLEDMPR_CreatedBy { get; set; }
        public long ISMSLEDMPR_UpdatedBy { get; set; }
    }
}
