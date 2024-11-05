using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSubjectAllMDTO : CommonParamDTO
    {
        public bool already_cnt { get; set; }

        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_IVRSSubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long? ISMS_ExamFlag { get; set; }
        public long? ISMS_PreadmFlag { get; set; }
        public long? ISMS_SubjectFlag { get; set; }
        public long? ISMS_BatchAppl { get; set; }
        public long? ISMS_ActiveFlag { get; set; }
        public long? ISMS_OrderFlag { get; set; }
        public bool? ISMS_TTFlag { get; set; }
        public int userId { get; set; }
        public bool? ISMS_AttendanceFlag { get; set; }
        public int? ISMS_LanguageFlg { get; set; }
        public int? ISMS_AtExtraFeeFlg { get; set; }
        public string IMS_SubjectTypeFlag { get; set; }
        public Array subject_m_list { get; set; }
        public Array subject_m_list_new { get; set; }
        public Array subject_m_listOrder { get; set; }
        public string returnduplicatestatus { get; set; }
        public string returnvaluetype { get; set; }
        public bool returnval { get; set; }
        public Array edit_m_subject { get; set; }

        public long PAMS_Id { get; set; }

        public string PAMS_SubjectName { get; set; }
        public string PAMS_SubjectCode { get; set; }
        public decimal PAMS_MaxMarks { get; set; }
        public decimal PAMS_MinMarks { get; set; }
        public string PAMS_SubjectFlag { get; set; }
        public int PAMS_ActiveFlag { get; set; }
        public Array MasterSubjectData { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public subject_orderDTO[] subjectDTO { get; set; }
        public string retrunMsg { get; set; }


        public long? ISMS_IntroYear { get; set; }
        public string ISMS_FileName { get; set; }
        public string ISMS_FilePath { get; set; }
        public bool? ISMS_DiscontinuedFlg { get; set; }
        public long? ISMS_DiscontinuedYear { get; set; }
        public string ISMS_DiscontinuedReason { get; set; }

        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public Array courst_list { get; set; }
        public Array branch_list { get; set; }
        public Array sub_list { get; set; }
        public Array year_list { get; set; }
        public string yearname { get; set; }
        public long yearid { get; set; }

        public Array yearOfintro { get; set; }
        public Array mappinglistdata { get; set; }
        public bool duplicate { get; set; }

        public string ISMS_SubjectNameNew { get; set; }

        public MasterSubjectAllMDTO[] branch_list_data { get; set; }
    }
    public class subject_orderDTO : CommonParamDTO
    {
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
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
    }

}
