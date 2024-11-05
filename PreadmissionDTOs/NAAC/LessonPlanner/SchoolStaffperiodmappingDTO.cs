using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class SchoolStaffperiodmappingDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long Userid { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public long LPLP_Id { get; set; }
        public DateTime LPLP_LPDate { get; set; }
        public DateTime LPLP_CTDate { get; set; }
        public bool LPLP_ClassTakenFlg { get; set; }
        public bool LPLP_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }
        public long roleId { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string message { get; set; }
        public string employeename { get; set; }
        public string LPMT_TopicName { get; set; }
        public string username { get; set; }
        public string rolename { get; set; }
        public int LPMMT_Order { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public bool returnval { get; set; }
        public Array masteryear { get; set; }
        public Array employeedetails { get; set; }
        public Array masterclass { get; set; }
        public Array mastersection { get; set; }
        public Array mastersubjects { get; set; }
        public Array getalldates { get; set; }
        public Array gettopicdetails { get; set; }
        public Array getsavedetails { get; set; }
        public SchoolStaffPeriodMappingTempDTO[] SchoolStaffPeriodMappingTempDTO { get; set; }
        public SchoolStaffPeriodMappingsavingTempDTO[] SchoolStaffPeriodMappingsavingTempDTO { get; set; }
    }
    public class SchoolStaffPeriodMappingTempDTO
    {
        public string LPLP_LPDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPLP_Id { get; set; }
    }
    public class SchoolStaffPeriodMappingsavingTempDTO
    {
        public string LPLP_LPDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPLP_Id { get; set; }
        public string LPLP_CTDate { get; set; }

    }
}
