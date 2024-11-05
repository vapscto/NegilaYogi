using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
    public class FeedbackTransactionDTO
    {
        public long MI_Id { get; set; }
        public string Flag { get; set; }
        public long userid { get; set; }
        public long FMTY_Id { get; set; }
        public long FMOP_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public long? roleId { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int FMTQ_TQOrder { get; set; }
        public int FMTO_TOOrder { get; set; }
        public long count { get; set; }
        public long ISMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public Array mappedquestiondeetails { get; set; }
        public Array mappedoptiondeetails { get; set; }
        public Array typelist { get; set; }
        public Array typelistload { get; set; }
        public Array questionlist { get; set; }
        public Array optionlist { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getstaffdetails { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string staffname { get; set; }
        public string rolename { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public bool FMQE_ManualEntryFlg { get; set; }
        public string baseurl { get; set; }
        public long IVRM_MI_Id { get; set; }
        public string Institute_Name { get; set; }
        public string Institute_code { get; set; }
        public FeedbackTransactionTempDTO[] temp { get; set; }
        public Array getstudentdetails { get; set; }
        public savemodulefeedbackDTO[] savemodulefeedback { get; set; }

        // Newly Added 

        public long FMOP_FOOrder { get; set; }
        public int FMOP_OptionsValue { get; set; }
        public DateTime FSSTR_FeedbackDate { get; set; }
        public long FMQE_FQOrder { get; set; }
        public string FMQE_FeedbackQRemarks { get; set; }
        public Array feedbackquestion { get; set; }
        public Array feedbackoption { get; set; }
        

    }

    public class FeedbackTransactionTempDTO
    {
        public string FMTY_FeedbackTypeName { get; set; }
        public long FMTY_Id { get; set; }
        public FeedbackTransactionquestionTempDTO[] ques { get; set; }

    }

    public class FeedbackTransactionquestionTempDTO
    {
        public string FMQE_FeedbackQuestions { get; set; }
        public long FMQE_Id { get; set; }
        public bool manualflg { get; set; }
        public string name { get; set; }
        public FeedbackTransactionoptionTempDTO[] opt { get; set; }
    }
    public class FeedbackTransactionoptionTempDTO
    {
        public string FMOP_FeedbackOptions { get; set; }
        public long FMOP_Id { get; set; }
    }
    public class savemodulefeedbackDTO
    {
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public string name { get; set; }
        public string FSTTR_FeedBack { get; set; }
        public int FMOP_OptionsValue { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public string FMQE_FeedbackQRemarks { get; set; }


    }
}
