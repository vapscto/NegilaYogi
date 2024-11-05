using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.FeedBack
{
    public class FeedBackReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long Type { get; set; }
        public int FMQE_FQOrder { get; set; }
        public int FMOP_FOOrder { get; set; }
        public string flag { get; set; }
        public Array report { get; set; }
        public Array getreportdetails { get; set; }
        public Array getquestions { get; set; }
        public Array getoptions { get; set; }
        public Array getyear { get; set; }
        public Array feedbacktype { get; set; }
        public Array getcourse { get; set; }
        public string Flagtype { get; set; }
        public string graphtype { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public Array getcoursenew { get; set; }
        public string FMTY_StakeHolderFlag { get; set; }
        public long FMTY_Id { get; set; }
        public long reportnew { get; set; }
        public Array getstudentlist { get; set; }
    }
}
