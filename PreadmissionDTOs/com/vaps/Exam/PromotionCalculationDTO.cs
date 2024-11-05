using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PromotionCalculationDTO
    {
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public Array yearlist { get; set; }
        public Array ctlist { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }
        public string EMP_MarksPerFlg { get; set; }
        public long AMST_Id { get; set; }
        public long userid { get; set; }
        public long countcalculated { get; set; }
    }
    public class TEmp_GroupWise_Marks
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public decimal? ESTMPPSG_GroupMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarks { get; set; }

        //New Added
        public decimal? ESTMPPSG_GroupTotalMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarksOutOfGroupTotal { get; set; }
        public decimal? ESTMPPSG_ObtMarksOutOfSubjectMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupPercentage { get; set; } 
        public string ESTMPPSG_GroupMarksGrade { get; set; }
    }
    public class TEmp_GroupWise_Exam_Marks
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public int? EMGD_Id { get; set; }
        public decimal? ESTMPPSGE_ExamActualMarks { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedMarks { get; set; }
        public string ESTMPPSGE_ExamConvertedGrade { get; set; }
        public string ESTMPPSGE_ExamPassFailFlag { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedPoints { get; set; }
        public decimal? ESTMPPSGE_ExamActualMaxMarks { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedMaxMarks { get; set; }
    }
}


