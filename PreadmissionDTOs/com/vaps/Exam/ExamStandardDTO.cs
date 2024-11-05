using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamStandardDTO:CommonParamDTO
    {
        public int ExmConfig_Id { get; set; }
        public long MI_Id { get; set; }
        public string ExmConfig_RankingMethod { get; set; }
        public bool ExmConfig_PromotionFlag { get; set; }
        public bool ExmConfig_PassFailRankFlag { get; set; }
        public string ExmConfig_Recordsearchtype { get; set; }
        public string ExmConfig_Remarks { get; set; }
        public bool ExmConfig_GraceAplFlg { get; set; }
        public bool ExmConfig_BonusAplFlag { get; set; }
        public bool ExmConfig_MinAttAplFlag { get; set; }
        public bool ExmConfig_MarksMultiply { get; set; }
        public int ExmConfig_NoOfDecimal { get; set; }
        public bool ExmConfig_GroupMarksToResultFlg { get; set; }
        public bool ExmConfig_EnableFractionFlg { get; set; }
        public bool ExmConfig_EntryDateRestFlg { get; set; }
        public bool ExmConfig_AdmnoColumnDisplay { get; set; }
        public bool ExmConfig_RegnoColumnDisplay { get; set; }
        public bool ExmConfig_RollnoColumnDisplay { get; set; }
        public bool returnval { get; set; }
        public Array exm_config { get; set; }
        public bool ExmConfig_RoundoffFlag { get; set; }
        public int ExmConfig_NoOfDecimalValues { get; set; }
        public bool ExmConfig_PerRoundoffFlag { get; set; }
        public bool ExmConfig_HallTicketFlg { get; set; }
        public bool ExmConfig_FailBoldFlg { get; set; }
        public bool ExmConfig_FailItalicFlg { get; set; }
        public bool ExmConfig_FailUnderscoreFlg { get; set; }
        public string ExmConfig_FailColorFlg { get; set; }
        public bool ExmConfig_AllSubjectAbsentFlg { get; set; }
        public bool ExmConfig_FeeDefaulterDisplayFlg { get; set; }
        public string ExmConfig_AdmNoRegNoRollNo_DefaultFlag { get; set; }
        public bool? ExmConfig_FailRankFlg { get; set; }
        public bool? ExmConfig_ClassRankFlg { get; set; }
        public bool? ExmConfig_SecRankFlg { get; set; }
        public bool? ExmConfig_ClassPositionFlg { get; set; }
        public bool? ExmConfig_SectionPositionFlg { get; set; }
        public bool? ExmConfig_GroupwiseMarksFlg { get; set; }
        public bool? ExmConfig_PromSubjRoundoffFlag { get; set; }
        public bool? ExmConfig_SubjectwiseSecRankFlg { get; set; }
        public bool? ExmConfig_SubjectwiseClassRankFlg { get; set; }
    }
}
