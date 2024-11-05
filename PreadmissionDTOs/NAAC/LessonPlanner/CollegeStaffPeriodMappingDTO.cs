using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner
{
    public class CollegeStaffPeriodMappingDTO
    {
        public long LPLPC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public DateTime LPLPC_LPDate { get; set; }
        public DateTime LPLPC_CTDate { get; set; }
        public bool LPCSWA_Flag { get; set; }
        public bool LPCSWA_ActiveFlag { get; set; }
        public long LPCSWA_CreatedBy { get; set; }
        public long LPCSWA_UpdatedBy { get; set; }
        public long Userid { get; set; }
        public Array masteryear { get; set; }
        public Array mastercourse { get; set; }
        public Array masterbranch { get; set; }
        public Array mastersemester { get; set; }
        public Array mastersection { get; set; }
        public Array mastersubjects { get; set; }
        public Array employeedetails { get; set; }
        public string employeename { get; set; }
        public Array getalldates { get; set; }
        public Array gettopicdetails { get; set; }
        public string LPMT_TopicName { get; set; }
        public int LPMT_Topicorder { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public bool returnval { get; set; }
        public bool LPLPC_ClassTakenFlg { get; set; }
        public CollegeStaffPeriodMappingTempDTO[] CollegeStaffPeriodMappingTempDTO { get; set; }
        public Array getsavedetails { get; set; }
        public string rolename { get; set; }
        public string username { get; set; }
        public long roleId { get; set; }
        public long Emp_Code { get; set; }
        public CollegeStaffPeriodMappingsavingTempDTO[] CollegeStaffPeriodMappingsavingTempDTO { get; set; }
        public temp_section_id[] temp_section_id { get; set; }
        public temp_branch_id[] temp_branch_id { get; set; }

    }
    public class CollegeStaffPeriodMappingTempDTO
    {
        public string LPLPC_LPDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPLPC_Id { get; set; }
    }
    public class CollegeStaffPeriodMappingsavingTempDTO
    {
        public string LPLPC_LPDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPLPC_Id { get; set; }
        public string LPLPC_CTDate { get; set; }
    }

    public class temp_branch_id
    {
        public long AMB_Id { get; set; }
    }
    public class temp_section_id
    {
        public long ACMS_Id { get; set; }
    }
}
