using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class ScheduleReportDTO : CommonParamDTO
    {

        public Array fillyear { get; set; }
        public Array fillclass { get; set; }

        public string asmayid { get; set; }
        public long asmclid { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string flagows { get; set; }
        public Array allreports { get; set; }
        public Array schedulelist { get; set; }
        public Array remarkschedulelist { get; set; }
        public string oralwrittenscheduleflag { get; set; }
        public int  mid { get; set; }
        public Array writentestlist { get; set; }
        public long asmay_id { get; set; }
        public Array orallist { get; set; }
        public int yearid { get; set; }
        public long disid { get; set; }
        public string disname { get; set; }
        public string yearorbtwndates { get; set; }
        public long schids { get; set; }
        public ScheduleReportDTO[] SmsMailStudentDetailst { get; set; }

        public string name { get; set; }

        public string ScheduleDate { get; set; }

        public string ScheduleTime { get; set; }


        public string ScheduleTimeTo { get; set; }

        public Array studetaarray { get; set; }

        public string regno { get; set; }

        public int stype { get; set; }

        public long PAOTS_Id { get; set; }

    }
}
