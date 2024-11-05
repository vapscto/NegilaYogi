using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeTermDTO:CommonParamDTO
    {
        public long FMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMT_Name { get; set; }

        public bool FMT_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array feetermsarray { get; set; }
        public Array feetermsarrayDrop { get; set; }
        public Array hdnames { get; set; }
        public Array insnames { get; set; }
        public Array dataretrive { get; set; }
        public Array academicdrp { get; set; }
        public long ASMAY_ID { get; set; }

        // second tab 
        public string termnaem { get; set; }
        public string headname { get; set; }
        public string installmentname { get; set; }
        public long  idforedt { get; set; }
        //third tab
        public Array duadateget { get; set; }

        public long fmthddid { get; set; }
        public string termname { get; set; }
        public string headnamed { get; set; }
        public string yearname { get; set; }
        public DateTime fdate1 { get; set; }
        public DateTime tdate1 { get; set; }
        public DateTime aplc1 { get; set; }
        public DateTime ddate1 { get; set; }
        public Array arrduadates { get; set; }

        public long fmt { get; set; }
        public long fmh { get; set; }
        public long asyid { get; set; }
        public long  fti { get; set; }

        public long FMTFHNew { get; set; }


        public long FTI_ID { get; set; }
        public string FTI_NAME { get; set; }
        public Array feetermsarray1 { get; set; }

        public string status { get; set; }

       
        public string ToMonth { get; set; }

       
        public long USER_ID { get; set; }
        public string FMTP_TO_MONTH { get; set; }
        public string FromMonth { get; set; }
        public string fmtToMonth { get; set; }
        public string FMTP_Year { get; set; }
        public string FeeFlag { get; set; }
        public long FMTP_Id { get; set; }
        public Array masteperiodarray { get; set; }

        public bool FMT_IncludeArrearFeeFlg { get; set; }
    }
}
