using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class MasterTimeSettingDTO
    {
        public int FOMTS_Id { get; set; }
        public string FOMTS_FDWHrMin { get; set; }
        public string FOMTS_HDWHrMin { get; set; }

        public string FOMTS_IHalfLoginTime { get; set; }
        public string FOMTS_IhalfLogoutTime { get; set; }
        public string FOMTS_IIHalfLoginTime { get; set; }
        public string FOMTS_IIHalfLogoutTime { get; set; }
        public string FOMTS_DelayPerShiftHrMin { get; set; }
        public string FOMTS_EarlyPerShiftHrMin { get; set; }
        public string FOMTS_LunchHoursDuration { get; set; }
        public string FOMTS_BlockAttendance { get; set; }
        public string FOMTS_FixTimings { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
        public long MI_Id { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array periodlistedit { get; set; }

        public Array getlist { get; set; }
        public int count { get; set; }
    }
}
