using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SMSReportDTO
    {
        public int mid { get; set; }
        public int yearid { get; set; }
        public Array yeardropDown { get; set; }
        public long MI_Id { get; set; }
        public long user_id { get; set; }
        public long asmayid { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public Array yearfromTo { get; set; }  
        public SMSReportDTO[] sms_listarray { get; set; }
        public SMSReportDTO[] mial_listarray { get; set; }
        public Array sms_mial_listarray { get; set; }
        public Array meritlistdata { get; set; }


        public string studentFName { get; set; }
        public string studentMName { get; set; }
        public string studentLName { get; set; }
        public long pasr_id { get; set; }
        public decimal obtainedmarks { get; set; }
        public string passfail_flag { get; set; }

        public string Name { get; set; }
        public string smscount { get; set; }
        public string emailcount { get; set; }



    }
}
