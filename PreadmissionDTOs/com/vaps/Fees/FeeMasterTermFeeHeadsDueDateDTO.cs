using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterTermFeeHeadsDueDateDTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long FMTFHDD_Id { get; set; }
        public long FMTFH_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FMTFHDD_FromDate { get; set; }
        public DateTime FMTFHDD_ToDate { get; set; }
        public DateTime FMTFHDD_ApplicableDate { get; set; }
        public DateTime FMTFHDD_DueDate { get; set; }

        public Array insarray { set; get; }
        public Array arrduadates { set; get; }

        public bool returnval { get; set; }
        public Array duadateget { set; get; }
        public Array masterdit { get; set; }
    }
}
