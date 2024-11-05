using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HL_Master_MessCategory_DTO
    {

        public long HLMMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMMC_Name { get; set; }
        public bool HLMMC_ActiveFlag { get; set; }
        public long HLMMC_CreatedBy { get; set; }
        public long HLMMC_UpdatedBy { get; set; }

        public long UserId { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array get_messCategorylist { get; set; }

    }
}
