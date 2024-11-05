using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class Stu_FeedbackDTO
    {
        public long MI_Id { get; set; }

        public string ASMAY_Year { get; set; }
        public DateTime ASMAY_From_Date { get; set; }
        public DateTime ASMAY_To_Date { get; set; }
        public DateTime ASMAY_PreAdm_F_Date { get; set; }
        public DateTime ASMAY_PreAdm_T_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMAY_ActiveFlag { get; set; }
        public int ASMAY_Pre_ActiveFlag { get; set; }
        public DateTime ASMAY_Cut_Of_Date { get; set; }
        public bool Is_Active { get; set; }

      //  public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_Order { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMAY_RollNo { get; set; }

      
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMC_ActiveFlag { get; set; }
        public int ASMC_MaxCapacity { get; set; }


        public long ASYST_Id { get; set; }
      
        public int? AMAY_PassFailFlag { get; set; }
        public long? LoginId { get; set; }
        public DateTime? AMAY_DateTime { get; set; }
        public int AMAY_ActiveFlag { get; set; }


        public int ESTMPS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public string ESTMPS_ObtainedGrade { get; set; }

        public string ESTMPS_PassFailFlg { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }

        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }

        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_ExamFlag { get; set; }
        public long ISMS_PreadmFlag { get; set; }
        public long ISMS_SubjectFlag { get; set; }
        public long ISMS_BatchAppl { get; set; }
        public long ISMS_ActiveFlag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }
        public int ISMS_LanguageFlg { get; set; }
        public int ISMS_AtExtraFeeFlg { get; set; }

        public Array stuyearlist { get; set; }
        public Array studetiallist { get; set; }
        public Array subjectlist { get; set; }
        public Array examlist { get; set; }
        public Array examgradelist { get; set; }
        public Array examsubjdetails { get; set; }
        public decimal percentage { get; set; }

        public Array instname { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }

        public long adm_fid { get; set; }

        public string adm_name { get; set; }
        public string adm_emailid { get; set; }
        public long adm_contactno { get; set; }

        public string adm_comment { get; set; }
        public DateTime adm_date { get; set; }

        public  bool submsg { get; set; }





    }
}