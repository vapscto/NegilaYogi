using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
   public class PromotionReportDetailsDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long Emp_Code { get; set; }
        public long Userid { get; set; }
        public int EME_Id { get; set; }
        public int? EME_ExamOrder { get; set; }
        public string EME_ExamName { get; set; }
        public string EMER_Remarks { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string allorindi { get; set; }
        public Array allclasslist { get; set; }
        public Array allAcademicYear { get; set; }
        public Array allsectionlist { get; set; }
        public Array reportdata { get; set; }
        public Array getexamwiseavgmarkspromotion { get; set; }
        public Array reportdata1 { get; set; }
        public string Flag { get; set; }
        public string Flagone { get; set; }
        public string reporttype { get; set; }
        public Array getpromotiondetails { get; set; }
        public Array studentlist { get; set; }
        public Array studentlistdetails { get; set; }
        public Array studentwise_individualexamremarks { get; set; }       
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_SOL { get; set; }
        public long AMAY_RollNo { get; set; }
        public DateTime AMST_DOB { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long? subjorder { get; set; }
        public decimal? marksorper { get; set; }
        public decimal? maxmarks { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getsubjectgrouplist { get; set; }
        public bool? apptoresult { get; set; }
        public string groupname { get; set; }
        public string displayname { get; set; }
        public long EMPSG_Id { get; set; }
        public int? grouporder { get; set; }
        public Array configuration { get; set; }
        public Array masterinstitution { get; set; }
        public Array studentwisetotal { get; set; }
        public Array studenwise_remarks { get; set; }
        public Array subjectwisetotal { get; set; }
        public decimal? ESTMPPS_MaxMarks { get; set; }
        public decimal? ESTMPPS_ObtainedMarks { get; set; }
        public string ESTMPPS_ObtainedGrade { get; set; }
        public string ESTMPPS_PassFailFlg { get; set; }
        public string ESTMPPS_Remarks { get; set; }
        public decimal? ESTMPPS_GradePoints { get; set; }
        public Array getexamlist { get; set; }
        public long[] AMST_Ids { get; set; }
        public int? ESTMPPS_ClassRank { get; set; }
        public int? ESTMPPS_SectionRank { get; set; }
        public bool? Deactive_Flag { get; set; }
        public bool? Left_Flag { get; set; }
    }
}
