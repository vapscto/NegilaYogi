using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeStudenttctransactionDTO
    {
        public long ACSTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ASMAY_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public long? AMB_Id { get; set; }
        public long? AMSE_Id { get; set; }
        public long? ACMS_Id { get; set; }
        public long? AMCST_Id { get; set; }        
        public long? IMC_Id { get; set; }
        public long? ACSTC_WorkingDays { get; set; }
        public long? ACSTC_AttendedDays { get; set; }
        public long? ACSTC_TemporaryFlag { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getssection { get; set; }
        public Array getstudentlist { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getstudentattendancedetails { get; set; }
        public Array getstudentfeedetails { get; set; }
        public Array getstudentelectivesubjects { get; set; }
        public Array getstudentlanguagesubjects { get; set; }
        public Array getconfigurationdetails { get; set; }
        public Array admTransNumSetting { get; set; }
        public string message { get; set; }
        public string AMCST_SOL { get; set; }
        public string adm_num_flag { get; set; }
        public string ACSTC_TCNO { get; set; }
        public string ACSTC_Scholarship { get; set; }
        public string ACSTC_LanguageStudied { get; set; }
        public string ACSTC_MediumOfINStruction { get; set; }
        public string ACSTC_ElectivesStudied { get; set; }
        public string ACSTC_MedicallyExam { get; set; }
        public string ACSTC_Qual_PromotionFlag { get; set; }
        public string ACSTC_Qual_Course { get; set; }
        public string ACSTC_FeePaid { get; set; }
        public string ACSTC_FeeConcession { get; set; }
        public string ACSTC_Conduct { get; set; }
        public string ACSTC_LeavingReason { get; set; }
        public string ACSTC_Result { get; set; }
        public string ACSTC_ResultDetails { get; set; }
        public string ACSTC_NCCDetails { get; set; }
        public string ACSTC_LastExamDetails { get; set; }
        public string ACSTC_ExtraActivities { get; set; }
        public string ACSTC_Remarks { get; set; }
        public string ACSTC_ActiveFlag { get; set; }
        public string Last_Course_Studied { get; set; }
        public string Caste { get; set; }
        public string searchfilter { get; set; }
        public string allorindividual { get; set; }
        public string studentName { get; set; }
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        public DateTime? Admission_Date { get; set; }
        public DateTime? ACSTC_TCApplicationDate { get; set; }
        public DateTime? ACSTC_TCDate { get; set; }
        public DateTime? ACSTC_TCIssueDate { get; set; }
        public DateTime? ACSTC_LastAttendedDate { get; set; }
        public DateTime? ACSTC_PromotionDate { get; set; }
        public bool returnval { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }

    }
}

