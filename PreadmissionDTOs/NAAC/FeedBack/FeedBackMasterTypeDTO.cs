using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
    public class FeedBackMasterTypeDTO
    {
        public long MI_Id { get; set; }
        public long FMTY_Id { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string FMTY_FeedbackTypeRemarks { get; set; }
        public int FMTY_FTOrder { get; set; }
        public string FMTY_StakeHolderFlag { get; set; }
        public bool FMTY_SubjectSpecificFlag { get; set; }
        public bool FMTY_QuestionwiseOptionFlg { get; set; }
        public bool FMTY_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public Array getdetails { get; set; }
        public long userid { get; set; }
        public string message { get; set; }
        public Array editdetails { get; set; }
        public Type_Master_TempDTO[] Type_Master_TempDTO { get; set; }
        public bool FMTY_StudentFlag { get; set; }
        public long FMTY_NOFPerYearByStudent { get; set; }
        public bool FMTY_StaffFlag { get; set; }
        public long FMTY_NOFPerYearByStaff { get; set; }
        public bool FMTY_ParentFlag { get; set; }
        public long FMTY_NOFPerYearByParent { get; set; }
        public bool FMTY_AlumniFlag { get; set; }
        public long FMTY_NOFPerYearByAlumni { get; set; }
        public long nooftimesfeedback { get; set; }
    }
    public class Type_Master_TempDTO
    {
        public long FMTY_Id { get; set; }
        public int FMTY_FTOrder { get; set; }
    }
    public class Feedback_Master_QuestionDTO
    {
        public long MI_Id { get; set; }
        public bool returnval { get; set; }
        public Array getdetails { get; set; }
        public long userid { get; set; }
        public string message { get; set; }
        public Array editdetails { get; set; }
        public long FMQE_Id { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public string FMQE_FeedbackQRemarks { get; set; }
        public int FMQE_FQOrder { get; set; }
        public bool FMQE_ActiveFlag { get; set; }
        public bool FMQE_ManualEntryFlg { get; set; }
        public Questions_Master_TempDTO[] Questions_Master_TempDTO { get; set; }
    }
    public class Questions_Master_TempDTO
    {
        public long FMQE_Id { get; set; }
        public int FMQE_FQOrder { get; set; }
    }
    public class Feedback_Master_OptionDTO
    {
        public long MI_Id { get; set; }
        public bool returnval { get; set; }
        public Array getdetails { get; set; }
        public long userid { get; set; }
        public string message { get; set; }
        public Array editdetails { get; set; }
        public long FMOP_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public int FMOP_OptionsValue { get; set; }
        public string FMOP_FeedbackORemarks { get; set; }
        public int FMOP_FOOrder { get; set; }
        public bool FMOP_ActiveFlag { get; set; }
        public Option_Master_TempDTO[] Option_Master_TempDTO { get; set; }
    }
    public class Option_Master_TempDTO
    {
        public long FMOP_Id { get; set; }
        public int FMOP_FOOrder { get; set; }
    }
}
