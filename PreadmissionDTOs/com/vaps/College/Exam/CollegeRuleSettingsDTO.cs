using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class CollegeRuleSettingsDTO
    {
        public long MI_Id { get; set; }
        public long EMRS_Id { get; set; }
        public long ECYS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public string EMRS_MarksPerFlg { get; set; }
        public string message { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACSS_SchmeName { get; set; }
        public string ACST_SchmeType { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string EMGR_GradeName { get; set; }   
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_Order { get; set; }
        public bool returnval { get; set; }
        public bool EMRS_ActiveFlag { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsubjectscheme { get; set; }
        public Array getschemetype { get; set; }
        public Array getexam { get; set; }
        public Array getsubject { get; set; }
        public Array getgrade { get; set; }
        public Array getsaveddetails { get; set; }
        public Array getviewsubjects { get; set; }
        public Array getviewgroupdetails { get; set; }
        public Array getviewexamdetails { get; set; }
        public Array geteditsubjectlist { get; set; }
        public Array geteditgroupdetails { get; set; }
        public Array geteditexamdetails { get; set; }

        public Temp_Subject_ListDTO[] Temp_Subject_ListDTO { get; set; }
        public Temp_Group_ListDTO[] Temp_Group_ListDTO { get; set; }
        public Temp_Subject_Group_ListDTO[] Temp_Subject_Group_ListDTO { get; set; }
        
        public decimal? EMRSS_MaxMarks { get; set; }
        public decimal? EMRSS_MinMarks { get; set; }
        public decimal? EMRSS_ConvertForMarks { get; set; }
        public bool EMRSS_AppToResultFlg { get; set; }
        public bool EMRSS_ActiveFlag { get; set; }
        public long EMRSSG_Id { get; set; }
        public long EMRSS_Id { get; set; }
        public string EMRSSG_GroupName { get; set; }
        public string EMRSSG_DisplayName { get; set; }
        public decimal? EMRSSG_PercentValue { get; set; }
        public decimal? EMRSSG_MarksValue { get; set; }
        public int EMRSSG_MaxOff { get; set; }
        public int EMRSSG_BestOff { get; set; }
        public bool EMRSSG_ActiveFlag { get; set; }
        public long EMRSSGE_Id { get; set; }    
        public bool EMRSSGE_ActiveFlg { get; set; }
    }


    public class Temp_Subject_ListDTO
    {
        public long EMRSS_Id { get; set; }
        public long EMRS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EMRSS_MaxMarks { get; set; }
        public decimal? EMRSS_MinMarks { get; set; }
        public decimal? EMRSS_ConvertForMarks { get; set; }
        public bool EMRSS_AppToResultFlg { get; set; }
    }
    public class Temp_Group_ListDTO
    {
        public long ISMS_Id { get; set; }
        public long EMRSSG_Id { get; set; }
        public long EMRSS_Id { get; set; }
        public string EMRSSG_GroupName { get; set; }
        public string EMRSSG_DisplayName { get; set; }
        public decimal? EMRSSG_PercentValue { get; set; }
        public decimal? EMRSSG_MarksValue { get; set; }
        public int EMRSSG_MaxOff { get; set; }
        public int EMRSSG_BestOff { get; set; }
        public bool EMRSSG_ActiveFlag { get; set; }
        public Temp_Subject_Group_ListDTO[] Exm_M_Prom_Subj_Group_Exams_master { get; set; }
    }
    public class Temp_Subject_Group_ListDTO
    {
        public long EMRSSGE_Id { get; set; }
        public long EMRSSG_Id { get; set; }
        public int EME_Id { get; set; }
        public bool EMRSSGE_ActiveFlg { get; set; }

    }
}
