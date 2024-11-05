using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TrnsMonthEndReportDTO
    {
        public long TRMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public bool TRMA_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool retrval { get; set; }
        public Array getmasterarea { get; set; }
        public Array geteditdataarea { get; set; }
        public Array Acdlist { get; set; }
        public Array monthlist { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public Array griddata { get; set; }
        public string year { get; set; }
        public string type { get; set; }
        public string IVRM_Month_Id { get; set; }
    }
}
