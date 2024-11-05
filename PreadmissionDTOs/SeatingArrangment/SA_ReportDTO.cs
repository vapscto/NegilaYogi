using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class SA_ReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public DateTime? ESAEDATE_ExamDate { get; set; }
        public Array Getyearlist { get; set; }
        public Array Getexamlisst { get; set; }
        public Array Getuniversityexamlist { get; set; }
        public Array Getcourselist { get; set; }
        public Array Getslotlist { get; set; }
        public Array GetReportList { get; set; }
    }
}