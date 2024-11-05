using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class MasterLeaveDTO : CommonParamDTO
    {


       

        /// <summary>
        /// ///////////
        /// </summary>
        public Array edit_m_event { get; set; }
        public Array leaveData { get; set; }
        public bool dupr { get; set; }
        public Array selected_master_event { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval_Update { get; set; }
        public bool returnval_add { get; set; }
        public string retrunMsg { get; set; }
        public bool returnval { get; set; }
        public long HRML_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public Array master_eventlist { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public string HRML_LeaveType { get; set; }
        public string message { get; set; }
        public bool HRML_LateDeductFlag { get; set; }
        public Array GridviewDetails { get; set; }
        public Array leavelistedit { get; set; }
        public int HRML_LateDeductOrder { get; set; }
        public Array SelectedRowDetails { get; set; }
        public MasterLeaveDTO[] MasterLeaveDTOO { get; set; }
        public Array leaveorderlist { get; set; }

    }
}
