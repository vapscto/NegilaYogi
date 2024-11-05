using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeHeadWisecollectionReportDTO
    {
        public Array fillyear { get; set; }
        public Array fillclass { get; set; }
        public Array fillsec { get; set; }
        public Array fillfeegroup { get; set; }
        public Array fillfeehead { get; set; }           
        public long ASMAY_Id { get; set; }  
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }        
      //  public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }         
        public FeeHeadWisecollectionReportDTO[] tempgroupids { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime  todate { get; set; }
        public DateTime  ondate { get; set; }
        public string columnName { get; set; }
        public string columnID { get; set; }  
        public Array alldatagridreport { get; set; } 
        public string allorindiorcons { get; set; }
        public string dateorbteween { get; set; }
        public string consolidateflag { get; set; }
        public string activeleft { get; set; }
        public string nonconsolidateflag { get; set; }
    }
}
