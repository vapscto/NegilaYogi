using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class SchoolStaffperiodtransactionreportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long Userid { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public long LPSSWA_Id { get; set; }
        public DateTime LPSSWA_AllocatedDate { get; set; }
        public DateTime LPSSWA_TakenDate { get; set; }
        public bool LPSSWA_Flag { get; set; }
        public bool LPSSWA_ActiveFlag { get; set; }
        public long LPSSWA_CreatedBy { get; set; }
        public long LPSSWA_UpdatedBy { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string message { get; set; }
        public string employeename { get; set; }
        public string LPMT_TopicName { get; set; }
        public int LPSMTM_Order { get; set; }
        public bool returnval { get; set; }
        public Array masteryear { get; set; }
        public Array employeedetails { get; set; }
        public Array masterclass { get; set; }
        public Array mastersection { get; set; }
        public Array mastersubjects { get; set; }
        public Array getalldates { get; set; }
        public Array gettopicdetails { get; set; }
        public Array getsavedetails { get; set; }
        public Array getreport { get; set; }
        public Array getreportemployee { get; set; }
        public Array getdevationreport { get; set; }
        public Array getdevationreportemployee { get; set; }
    }
}
