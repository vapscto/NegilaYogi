using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
    public class FeedbackSchoolGeneralTransactionDTO
    {
        public long MI_Id { get; set; }
        public string Flag { get; set; }
        public long userid { get; set; }
        public long FMTY_Id { get; set; }
        public long FMOP_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? roleId { get; set; }
        public long ASMS_Id { get; set; }       
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
        public FeedbackTransactionschoolTempDTO[] temp { get; set; }
        public Array getstudentdetails { get; set; }
    }

    public class FeedbackTransactionschoolTempDTO
    {
        public string FMTY_FeedbackTypeName { get; set; }
        public long FMTY_Id { get; set; }
        public FeedbackTransactionquestionschoolTempDTO[] ques { get; set; }

    }

    public class FeedbackTransactionquestionschoolTempDTO
    {
        public string FMQE_FeedbackQuestions { get; set; }
        public long FMQE_Id { get; set; }
        public string name { get; set; }
        public bool manualflg { get; set; }
        public FeedbackTransactionoptionschoolTempDTO[] opt { get; set; }
    }
    public class FeedbackTransactionoptionschoolTempDTO
    {
        public string FMOP_FeedbackOptions { get; set; }
        public long FMOP_Id { get; set; }
    }
}
