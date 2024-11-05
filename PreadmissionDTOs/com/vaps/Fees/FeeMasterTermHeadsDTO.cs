using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterTermHeadsDTO:CommonParamDTO
    {
        public long FMTFH_Id { get; set; }
        public long FMT_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMTP_Id { get; set; }
        public string FMT_Year { get; set; }

        public FeeTermHeadTempDTO[] TempararyArrayList { get; set; }
        public FeeTermHeadTempDTO[] TempararyArrayListhd { get; set; }


        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array dataretrive { get; set; }

        public string insname { get; set; }
        public DateTime? fmtfhdD_FromDate { get; set; }
        public DateTime? fmtfhdD_ToDate { get; set; }
        public DateTime? fmtfhdD_ApplicableDate { get; set; }
        public DateTime? fmtfhdD_DueDate { get; set; }

        public List<FeeMasterTermFeeHeadsDueDateDTO> feetfhddd { get; set; }
        public long temyrid { get; set; }
        public bool returnvalue { set; get; }
        public long mainid { get; set; }
        public string status { get; set; }

        public long ASMAY_Id { get; set; }
        public long USER_ID { get; set; }
        public string FMTP_Year { get; set; }
        public string FMTP_FROM_MONTH { get; set; }
        public string FMTP_TO_MONTH { get; set; }
        public string FeeFlag { get; set; }

        
    }
}