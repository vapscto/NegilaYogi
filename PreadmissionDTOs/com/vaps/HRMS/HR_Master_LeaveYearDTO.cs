using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_LeaveYearDTO:CommonParamDTO
    {
        public long HRMLY_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public DateTime HRMLY_FromDate { get; set; }
        public DateTime HRMLY_ToDate { get; set; }
        public bool HRMLY_ActiveFlag { get; set; }
        public Array leaveYearList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public long HRMLY_LeaveYearOrder { get; set; }
        public Array yeardetailList { get; set; }
        public HR_Master_LeaveYearDTO[] LeaveorderDTO { get; set; }
    }
}
