using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTMasterFacilitiesDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        
        public long User_Id { get; set; }
        public long TTMFA_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMFA_FacilityName { get; set; }
        public string TTMFA_FacilityDesc { get; set; }
        public bool TTMFA_ActiveFlg { get; set; }
        public long TTMFA_CreatedBy { get; set; }
        public long TTMFA_UpdatedBy { get; set; }
        public DateTime TTMFA_CreatedDate { get; set; }
        public DateTime TTMFA_UpdatedDate { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }



    }
}
