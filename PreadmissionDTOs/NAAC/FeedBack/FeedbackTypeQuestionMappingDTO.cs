using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
    public class FeedbackTypeQuestionMappingDTO
    {
        public long FMTQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public int FMTQ_TQOrder { get; set; }
        public bool FMTQ_ActiveFlag { get; set; }
        public Array getdetails { get; set; }
        public Array feedbackquestions { get; set; }
        public Array feedbacktype { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public bool FMTY_QuestionwiseOptionFlg { get; set; }
        public Array getquestionsoptions { get; set; }
        public FeedbackTypeQuestionMappingTempDTO[] FeedbackTypeQuestionMappingTempDTO { get; set; }
        public FeedbackTypeQuestionMappingTemporderDTO[] FeedbackTypeQuestionMappingTemporderDTO { get; set; }
        public temp_question_order_TemporderDTO[] temp_question_order_TemporderDTO { get; set; }
        public long FMOP_Id { get; set; }
        public string FMOP_OptionName { get; set; }
        public string FMOP_FeedbackORemarks { get; set; }
        public bool FMTQO_ActiveFlag { get; set; }
        public int FMTQO_TQOOrder { get; set; }
        public long FMTQO_Id { get; set; }
        public Array getoptions { get; set; }
        public temp_question[] temp_question { get; set; }
        public string mappeddetailscount { get; set; }

    }
    public class FeedbackTypeQuestionMappingTempDTO
    {
        public long FMQE_Id { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
    }
    public class FeedbackTypeQuestionMappingTemporderDTO
    {
        public long FMTQ_Id { get; set; }
        public int FMTQ_TQOrder { get; set; }
    }

    public class FeedbackTypeOptionMappingDTO
    {
        public long FMTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMTY_Id { get; set; }
        public long FMOP_Id { get; set; }
        public long userid { get; set; }
        public int FMTO_TQOrder { get; set; }
        public bool FMTO_ActiveFlag { get; set; }
        public long FMTO_CreatedBy { get; set; }
        public long FMTO_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array feedbackoptions { get; set; }
        public Array feedbacktype { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string mappeddetailscount { get; set; }
        public FeedbackTypeOptionMappingTempDTO[] FeedbackTypeOptionMappingTempDTO { get; set; }
        public FeedbackTypeOptionMappingTemporderDTO[] FeedbackTypeOptionMappingTemporderDTO { get; set; }
       
    }

    public class FeedbackTypeOptionMappingTempDTO
    {
        public long FMOP_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
    }
    public class FeedbackTypeOptionMappingTemporderDTO
    {
        public long FMTO_Id { get; set; }
        public int FMTO_TQOrder { get; set; }
    }

    public class temp_question
    {
        public long FMQE_Id { get; set; }
        public string feedbackquestionname { get; set; }
        public long FMTY_Id { get; set; }
        public string feedbacktypename { get; set; }
        public bool FMQE_ManualEntryFlg { get; set; }
        public temp_options[] optiondetails { get; set; }
    }

    public class temp_options
    {
        public long FMOP_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
    }
    public class temp_question_order_TemporderDTO
    {
        public long FMOP_Id { get; set; }
        public long FMTQO_Id { get; set; }
        public int FMTQO_TQOOrder { get; set; }
    }
}
