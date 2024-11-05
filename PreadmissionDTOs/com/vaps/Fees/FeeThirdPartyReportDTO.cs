using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeThirdPartyReportDTO
    {
        public long MI_Id { get; set; }
        public Array usersnameslist { get; set; }
        public string NormalizedUserName { get; set; }
      //  public int userId { get; set; }
        public Array ledgerlist { get; set; }
        public string L_Name { get; set; }
       // public int L_Code { get; set; }

        public long acayid { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string flagBorC { get; set; }
        public string stuORotherflag { get; set; }
        public string typeofrptflag{ get; set; }

        public Array reportdatelist { get; set; }
    }
  
}
