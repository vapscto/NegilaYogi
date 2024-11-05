using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
  public  class MasterMess_MessCategoryDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array get_messlistmapping { get; set; }
        public string HLMM_Name { get; set; }
        public string HLMMC_Name { get; set; }
        public long HLMMMC_Id { get; set; }
        public long HLMM_Id { get; set; }
        public long HLMMC_Id { get; set; }
        public bool HLMMC_ActiveFlag { get; set; }
        public DateTime? HLMMC_CreatedDate { get; set; }
        public DateTime? HLMMC_UpdatedDate { get; set; }
        public long HLMMC_CreatedBy { get; set; }
        public long HLMMC_UpdatedBy { get; set; }
        public Array master_mess { get; set; }
        public Array mess_category { get; set; }
        public bool returnval { get; set; }
    }
}
