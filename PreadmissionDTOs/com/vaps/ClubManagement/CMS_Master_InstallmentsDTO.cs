using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_Master_InstallmentsDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array MonthArray { get; set; }
        public Array pages { get; set; }
        public Array InstallArray { get; set; }
        public long CMSMINSTTY_Id { get; set; }
        public string CMSMINST_InstallmentName { get; set; }
        public DateTime? CMSMINST_FromDate { get; set; }
        public string CMSMINST_FromMonth { get; set; }
        public DateTime? CMSMINST_ToDate { get; set; }
        public string CMSMINST_ToMonth { get; set; }
        public DateTime? CMSMINST_ApplicableDate { get; set; }
        public string CMSMINST_ApplMonth { get; set; }
        public bool CMSMINST_ActiveFlag { get; set; }
        public long CMSMINST_Id { get; set; }
    }
}
