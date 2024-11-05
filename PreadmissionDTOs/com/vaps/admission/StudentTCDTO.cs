using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentTCDTO
    {
        public long ASTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long IMC_Id { get; set; }
        public long ASTC_ASMCL_Id { get; set; }
        public string ASTC_TCNO { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASTC_LeavingReason { get; set; }
        public DateTime? ASTC_TCDate { get; set; }
        public DateTime? ASTC_LastAttendedDate { get; set; }
        public long? ASTC_WorkingDays { get; set; }
        public long? ASTC_AttendedDays { get; set; }
        public DateTime? ASTC_PromotionDate { get; set; }
        public string ASTC_MediumOfINStruction { get; set; }
        public string ASTC_LanguageStudied { get; set; }
        public string ASTC_ElectivesStudied { get; set; }
        public string ASTC_Scholarship { get; set; }
        public string ASTC_MedicallyExam { get; set; }
        public string ASTC_NCCDetails { get; set; }
        public string ASTC_ExtraActivities { get; set; }
        public string ASTC_Conduct { get; set; }
        public string ASTC_Remarks { get; set; }
        public string ASTC_FeePaid { get; set; }
        public string ASTC_FeeConcession { get; set; }
        public string ASTC_LastExamDetails { get; set; }
        public string ASTC_Result { get; set; }
        public string ASTC_ResultDetails { get; set; }
        public long ASTC_TemporaryFlag { get; set; }
        public string ASTC_Qual_Class { get; set; }
        public string ASTC_Qual_PromotionFlag { get; set; }
        public string ASTC_ActiveFlag { get; set; }
        public DateTime? ASTC_TCApplicationDate { get; set; }
        public DateTime? ASTC_TCIssueDate { get; set; }
        public string StudentName { get; set; }
        public DateTime? AMST_Date { get; set; }
        public string AMST_Sex { get; set; }
        public DateTime? AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public string AMST_StuBankIFSC_Code { get; set; }
        public string AMST_BirthPlace { get; set; }
        public string AMST_BirthCertNO { get; set; }
        public string AMST_StuCasteCertiNo { get; set; }
        public int PASR_Age { get; set; }
        public string AMST_MotherTongue { get; set; }
        public string AMST_SOL { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMST_Nationality { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_AadharNo { get; set; }
        public string AMST_BloodGroup { get; set; }
        public string AMST_StuBankAccNo { get; set; }
        public string AMST_BPLCardNo { get; set; }
        public string AMC_Name { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ClassStudying { get; set; }
        public object ClassStudied { get; set; }
        public string IC_CasteName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }        
        public string RegistrationNo { get; set; }
        public string AdmNo { get; set; }
        public string imageIndex { get; set; }
        public string StudPhoto { get; set; }
        public string flag { get; set; }
        public Array studentlist { get; set; }
        public Array studentListById { get; set; }
        public Array classstudiedlist { get; set; }
        public Array castelist { get; set; }
        //public DateTime? ASMAY_From_Date { get; set; }
        public DateTime Admission_Date { get; set; }
        public int class_order { get; set; }
        public string previous_section { get; set; }
        public int section_order { get; set; }
        public bool returnval { get; set; }
        public string adm_num_flag { get; set; }
        public string Status_flag { get; set; }

        //ADDED ON 04-02-2017
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        // public DateTime Admission_Date { get; set; }
        public string Last_Class_Studied { get; set; }
        public string Caste { get; set; }
        public long Pre_Class_Id { get; set; }
        public long Section_Id { get; set; }
        public string tcflagexists { get; set; }
        public long feetobepaid { get; set; }
        public string student_first_nme;
        public string fathername { get; set; }
        public string stsno { get; set; }
        public Array admissioncongigurationList { get; set; }
        public Array academicList { get; set; }
        public Array currentYear { get; set; }
        public Array getdeletedtcdetails { get; set; }
        public Array getstudenttcdetails { get; set; }
        public Array classlist { get; set; }
        public Array SectionList { get; set; }
        public string Email_flag { get; set; }

        //--------tcno
        public Array admTransNumSetting { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public string allorindividual { get; set; }
        public long?  countclass { get; set; }
        public long? attclasscount { get; set; }
        public string flagstaus { get; set; }
        public DateTime? todateatt { get; set; }

        //added for search 
        public Array studentlistsearch { get; set; }
        public string searchfilter { get; set; }
        public Array tcpermanentpayment { get; set; }
        public Array classlistnew { get; set; }
        public Array get_elective_subjects { get; set; }
        public string subjectname { get; set; }
        public Array get_elective_subjects_language { get; set; }
        public Array get_elective_subjects_common { get; set; }
        public Array count_issuebooks { get; set; }
        public Array pdadata { get; set; }
        public long order { get; set; }
        public string ASTC_SubjectsStudied { get; set; }
        public Array getexamdetails { get; set; }
        public Array getconcession { get; set; }
        public Array viewstudentfeedetails { get; set; }
        public Array getjoineddetails { get; set; }
        public Array getapprovalmasterdetails { get; set; }
        public Array getstudentapplieddetails { get; set; }
        public Array getapprovalresultdetails { get; set; }
        public Array gettcdetailsbystudentid { get; set; }
        public string tcgeneratedornot { get; set; }
        public string joinedclassname { get; set; }
        public string joinedyearname { get; set; }
        public string leftclassname { get; set; }
        public string leftyearname { get; set; }
        public string leftsectionname { get; set; }
        public string studentphotopath { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string TC_CancelReason { get; set; }
        public Array studenttcdetails { get; set; }
        public Array Qualifiedclass { get; set; }
        public Array getadm_m_student_details { get; set; }

        public Array studentDetails { get; set; }
        public long UserId { get; set; }
        
              public Array accyear { get; set; }
        public classlsttwo1[] classlsttwo { get; set; }
    }
    public class classlsttwo1
    {
        public long ASMCL_Id { get; set; }

    }
}
