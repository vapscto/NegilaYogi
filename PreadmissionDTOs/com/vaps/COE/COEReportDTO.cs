using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.COE
{
    public class COEReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string type { get; set; }
        public int month { get; set; }

        public Array yearlist { get; set; }

        public Array classlist { get; set; }
        public Array coereport { get; set; }
        public int count { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public long year { get; set; }
        public Array ClassList { get; set; }
        public long HRMLY_Id { get; set; }
        public long user_id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public asmalist[] ASMAY_IdList { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }
        public bool categoryflag { get; set; }
    }
    public class asmalist
    {
        public long ASMAY_Id { get; set; }

    }
}
