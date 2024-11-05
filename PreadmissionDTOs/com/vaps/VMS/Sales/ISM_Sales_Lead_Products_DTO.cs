using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_Products_DTO
    {
        public long ISMSLEPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public bool ISMSLEPR_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long ISMSLEPR_CreatedBy { get; set; }
        public long ISMSLEPR_UpdatedBy { get; set; }

    }
}
