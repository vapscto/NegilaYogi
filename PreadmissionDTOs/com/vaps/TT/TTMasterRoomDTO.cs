using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTMasterRoomDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        
        public long User_Id { get; set; }
        public long TTMFA_Id { get; set; }
        public long TTMRMFA_Id { get; set; }
        public long TTMRM_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMRM_RoomName { get; set; }
        public string TTMRM_RoomDetails { get; set; }
        public bool TTMRM_ActiveFlg { get; set; }
        public bool TTMRMFA_ActiveFlg { get; set; }
        public long TTMRM_CreatedBy { get; set; }
        public long TTMRM_UpdatedBy { get; set; }
        public DateTime TTMRM_CreatedDate { get; set; }
        public DateTime TTMRM_UpdatedDate { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array facilitylistall { get; set; }
        public Array editfaclist { get; set; }
        public Array viewdata { get; set; }
         public string TTMFA_FacilityName { get; set; }
         public string TTMFA_FacilityDesc { get; set; }

        public facids[] flist { get; set; }
    }

    public class facids
    {
        public long TTMFA_Id { get; set; }

    }
}
