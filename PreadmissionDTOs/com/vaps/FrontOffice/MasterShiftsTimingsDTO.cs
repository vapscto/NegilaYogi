using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class MasterShiftsTimingsDTO
    {
        public int FOMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOMS_ShiftName { get; set; }

        public bool FOMS_ActiveFlg { get; set; }
        public int FOMST_Id { get; set; }       
        public int FOHWDT_Id { get; set; }
        public string FOMST_FDWHrMin { get; set; }
        public string FOMST_HDWHrMin { get; set; }
        public string FOMST_IHalfLoginTime { get; set; }
        public string FOMST_IHalfLogoutTime { get; set; }
        public string FOMST_IIHalfLoginTime { get; set; }
        public string FOMST_IIHalfLogoutTime { get; set; }
        public string FOMST_DelayPerShiftHrMin { get; set; }
        public string FOMST_EarlyPerShiftHrMin { get; set; }
        public string FOMST_LunchHoursDuration { get; set; }
        public string FOMST_BlockAttendance { get; set; }
        public string FOMST_FixTimings { get; set; }
        public string returnvalue { get; set; }
        public Array filldata { get; set; }
        public Array filltype { get; set; }
        public string FOHTWD_HolidayWDType { get; set; }
        
    }
}
