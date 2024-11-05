using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamCategoryReportDTO
    {
        public int EMG_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMG_TotSubjects { get; set; }
        public int EMG_MaxAplSubjects { get; set; }
        public int EMG_MinAplSubjects { get; set; }
        public int EMG_BestOff { get; set; }       
        public bool EMG_ActiveFlag { get; set; }
        public bool EMG_ElectiveFlg { get; set; }
        public int EYCE_Id { get; set; }      
        public int EYC_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMGR_Id { get; set; }
        public DateTime? EYCE_AttendanceFromDate { get; set; }
        public DateTime? EYCE_AttendanceToDate { get; set; }
        public bool EYCE_SubExamFlg { get; set; }
        public bool EYCE_SubSubjectFlg { get; set; }
        public bool EYCE_ActiveFlg { get; set; }
        public int EMCA_Id { get; set; }  
        public string EMCA_CategoryName { get; set; }
        public bool EMCA_ActiveFlag { get; set; }
        public long ISMS_Id { get; set; }
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

        public Array yearlist { get; set; }
        public Array grlist { get; set; }
        public Array groupname { get; set; }
        public Array categoryname { get; set; }
        public Array subjectname { get; set; }
        public Array studentAttendanceList { get; set; }
        public Array masterinstitution { get; set; }
        
        public string type { get; set; }
    }
}
