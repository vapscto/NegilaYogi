using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals
{
    public class Clg_HODEmpSalaryDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int COEME_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }     
        public string ASMAY_Year { get; set; }
        public int month { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public Array studentdetails { get; set; }      
        public Array attendancedetails { get; set; }
        public Array noticeboard { get; set; }
        public Array feedetails { get; set; }
        public Array coereportlist { get; set; }
        public Array calenderlist { get; set; }
        public Array fillabsent { get; set; }
        public Array coedata { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public Array fillfee { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public long HRMLY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long user_id { get; set; }

    }
}
