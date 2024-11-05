using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePortalStudentReportCardDTO
    {
        public class Input
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long AMST_Id { get; set; }
            public long EME_Id { get; set; }
           // public Array subjlist { get; set; }

        }
        public class Output
        {
            public int EMGR_Id { get; set; }
            public Array clstchname { get; set; }
            public Array savelist { get; set; }
            public Array subjlist { get; set; }
            public Array grade_details { get; set; }


        }
        public class OutputClassTeacherName
        {
            public long HRME_Id { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
        }
        public class OutputSaveList
        {
            public decimal? ESTMPS_ObtainedMarks { get; set; }
            public string ESTMPS_ObtainedGrade { get; set; }
            public string ESTMPS_PassFailFlg { get; set; }
            public string EME_ExamName { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
            public long AMST_Id { get; set; }
            public string AMST_FirstName { get; set; }
            public DateTime AMST_DOB { get; set; }
            public long AMAY_RollNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ISMS_Id { get; set; }
            public string ISMS_SubjectName { get; set; }
            public decimal? ESTMPS_MaxMarks { get; set; }
        }
        public class OutputsubjectList
        {
            public long ISMS_Id { get; set; }
            public string ISMS_SubjectName { get; set; }
            public string ISMS_SubjectCode { get; set; }
            public bool EYCES_AplResultFlg { get; set; }
            public decimal? EYCES_MaxMarks { get; set; }
            public decimal? EYCES_MinMarks { get; set; }
            public int EMGR_Id { get; set; }
        }
        public Array EmployeePortalStudentReportCard { get; set; }
    }
}
