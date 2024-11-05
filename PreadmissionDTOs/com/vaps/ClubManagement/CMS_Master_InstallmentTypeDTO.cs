using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_Master_InstallmentTypeDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array MonthArray { get; set; }
        public Array pages { get; set; }
        public long CMSMINSTTY_Id { get; set; }
        public string CMSMINSTTY_InstallmentType { get; set; }
        public string CMSMINSTTY_InstallmentTypeFlg { get; set; }
        public string CMSMINSTTY_DurationFlg { get; set; }
        public bool CMSMINSTTY_ActiveFlag { get; set; }
        public long CMSMINSTTY_Duration { get; set; }
    }
}
