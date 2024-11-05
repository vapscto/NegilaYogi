using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class GroupwiseSubListReportDTO
    {
        public long MI_Id { get; set; }
        public int EMG_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array examlist { get; set; }
        public Array group { get; set; }
        public int MyProperty { get; set; }
        public Array groupname { get; set; }
        public Array subjectname { get; set; }
        public Array institution { get; set; }
        public Array applicable_flag { get; set; }
        public Array getyear { get; set; }
        public string report_type { get; set; }
        public string examwiseorwithout { get; set; }
        public string masteryearly { get; set; }
        //public Array grp_subjects { get; set; }
    }
}
