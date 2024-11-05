using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
   public class HlMasterRoom_FeeGroupDTO
    {
        public long HLMRFG_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool HLMRFG_ActiveFlag { get; set; }
        public DateTime HLMRFG_CreatedDate { get; set; }
        public long HLMRFG_CreatedBy { get; set; }
        public DateTime HLMRFG_UpdatedDate { get; set; }
        public long HLMRFG_UpdatedBy { get; set; }
        public string FMG_GroupName { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public int UserId { get; set; }
        public Array alldata1 { get; set; }
        public Array roomid { get; set; }
        public Array groupid { get; set; }
        public Array Editlist { get; set; }
        public bool duplicate { get; set; }
        public bool ret { get; set; }
        public string msg { get; set; }
    }
}
