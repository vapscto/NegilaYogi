using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamPassFailConditionDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public int EYCE_Id { get; set; }
        public int EYC_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMGR_Id { get; set; }

        public int EMCA_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public bool EYCES_SubSubjectFlg { get; set; }
        public string EYCES_MarksGradeEntryFlg { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public bool EYCES_ActiveFlg { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public int EYCESSE_SubExamOrder { get; set; }
        public int EYCESSS_SubSubjectOrder { get; set; }
        public int EPFRC_From { get; set; }
        public int EPFRC_To { get; set; }
        public int ECM_Id { get; set; }
        public string ECM_ConditionName { get; set; }
        public string ECM_ConditionFlag { get; set; }
        public string EPFRC_PassFailFlag { get; set; }
        public int EPFRC_RankFlag { get; set; }
        public string EPFRC_Condition { get; set; }
        public string EPFRC_ExamFlag { get; set; }
        public string ASMAY_Year { get; set; }
        public string EME_ExamName { get; set; }
        public int EPFRC_Id { get; set; }
        public decimal? EPFRC_Percentage { get; set; }
        public bool EPFRC_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array examlist { get; set; }
        public Array subexamlist { get; set; }
        public Array subjectlist { get; set; }
        public Array examconditionlist { get; set; }
        public Array passfailrank_list { get; set; }
        public Array conditiontype { get; set; }
        public Array editlist { get; set; }
        public Array edclasslist { get; set; }
        public decimal? EPFRC_OverallPercentage { get; set; }
    }

}
