using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.MobileApp
{
    public class ExamDTO
    {
        public class getStudent
        {
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long ASMAY_Id { get; set; }
            public string ASMAY_Year { get; set; }
            public int ASMAY_Order { get; set; }
            public Array studetiallist { get; set; }
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public bool status { get; set; }
        }
        public class getExamdata
        {
            public long mI_ID { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public bool status { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public string ASMAY_Year { get; set; }
            public Array examlist { get; set; }
            public Array subjectlist { get; set; }
            public string Type { get; set; }
            public long ISMS_Id { get; set; }
            public string ISMS_SubjectName { get; set; }
            public string ISMS_SubjectCode { get; set; }
        }
        public class studentExamDetails
        {
            //studentExamDetails
            public bool status { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long EME_Id { get; set; }
            public string Type { get; set; }
            public Array getexamconfig { get; set; }
            public Array examReportList { get; set; }
            public Array get_eme_id_details { get; set; }
            public long ISMS_Id { get; set; }
            public Array subexamreportexamReportList { get; set; }
        }
        public class getdetails_IT
        {
            public bool status { get; set; }
            public Array getyearlist { get; set; }
            public long mI_ID { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long userid { get; set; }
            public string userName { get; set; }
            public long roleid { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public Array getexam { get; set; }
            public Array getgradelist { get; set; }
            public long EME_Id { get; set; }
            public Array instname { get; set; }
            public Array clstchname { get; set; }
            public long HRME_Id { get; set; }
            public string HRME_EmployeeFirstName { get; set; }

            public Array Present_attendence { get; set; }
            public Array savelisttot { get; set; }
            public Array subjlist { get; set; }
            public int EMGR_Id { get; set; }
            public List<getsProgressReport> savelist { get; set; }
            public string photoname { get; set; }
            public Array getstudentdetails { get; set; }
            public Array examwiseremarks { get; set; }
            public Array grade_details { get; set; }
            public long ISMS_Id { get; set; }
            public int EYCES_SubjectOrder { get; set; }
        }
        public class getsProgressReport
        {
            public decimal ESTMPS_ObtainedMarks { get; set; }
            public string ESTMPS_ObtainedGrade { get; set; }
            public string ESTMPS_PassFailFlg { get; set; }
            public string EME_ExamName { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
            public string AMST_FatherName { get; set; }
            public string AMST_MotherName { get; set; }
            public long AMST_Id { get; set; }
            public string AMST_FirstName { get; set; }
            public string ISMS_SubjectName { get; set; }
            public DateTime AMST_DOB { get; set; }
            public long AMAY_RollNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ISMS_Id { get; set; }
            public decimal? ESTMPS_MaxMarks { get; set; }
            public decimal? ESTMPS_ClassAverage { get; set; }
            public decimal? ESTMPS_ClassHighest { get; set; }
            public decimal? ESTMPS_SectionHighest { get; set; }
            public decimal? ESTMPS_SectionAverage { get; set; }
            public string ISMS_SubjectCode { get; set; }
            public bool EYCES_AplResultFlg { get; set; }
            public decimal? EYCES_MaxMarks { get; set; }
            public decimal? EYCES_MinMarks { get; set; }
            public int EMGR_Id { get; set; }
            public decimal? classheld { get; set; }
            public decimal? classatt { get; set; }
            public string graderemark { get; set; }
            public decimal ESTMP_TotalObtMarks { get; set; }
            public decimal ESTMP_Percentage { get; set; }
            public string ESTMP_TotalGrade { get; set; }
            public int ESTMP_ClassRank { get; set; }
            public int ESTMP_SectionRank { get; set; }
            public string ESTMP_TotalGradeRemark { get; set; }
            public decimal ESTMP_TotalMaxMarks { get; set; }
            public int EYCES_SubjectOrder { get; set; }
            public bool EYCES_MarksDisplayFlg { get; set; }
            public bool EYCES_GradeDisplayFlg { get; set; }
            public string ESTMP_Result { get; set; }
        }

    }
}
