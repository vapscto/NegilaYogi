using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HL_Master_Mess_DTO:CommonParamDTO
    {
        public long HLMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMM_Name { get; set; }
        public long HLMMC_Id { get; set; }
        public bool HLMM_VegFlg { get; set; }
        public bool HLMM_NonVegFlg { get; set; }
        public string HLMM_BFSStartTime { get; set; }
        public string HLMM_BFSEndTime { get; set; }
        public string HLMM_LNStartTime { get; set; }
        public string HLMM_LNEndTime { get; set; }
        public string HLMM_LNTSStartTime { get; set; }
        public string HLMM_LNTSEndTime { get; set; }
        public string HLMM_DNSStartTime { get; set; }
        public string HLMM_DNSEndTime { get; set; }
        public bool HLMM_ActiveFlag { get; set; }
        public long UserId { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string HLMMC_Name { get; set; }

        public Array edit_Messlist { get; set; }
        public Array get_messlist { get; set; }
        public Array get_messCategorylist { get; set; }
        //added by sanjeev
        public MasterMess_MessCategoryDTO[] MatserMessArray { get; set; }
        public int count { get; set; }
        public int saverecord { get; set; }
        public long HLMMMC_Id { get; set; }
    }
}
