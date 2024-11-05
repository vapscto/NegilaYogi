using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class CollegeCumulativeAvgBestReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long roletype { get; set; }
        public long HRME_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getexam { get; set; }
        public Array getsubject { get; set; }
        public Array subjectshemalist { get; set; }
        public Array schmetypelist { get; set; }
        public Array configuration { get; set; }
        public Array reportdata { get; set; }
        public Array reportinddata { get; set; }
        public Array instname { get; set; }
        public temp_cumulativereportavgbest[] examids { get; set; }
        public string Flag { get; set; }
        public int bestcount { get; set; }
    }
    public class temp_cumulativereportavgbest
    {
        public long EME_Id { get; set; }
        public string EME_ExamName { get; set; }

    }
}
