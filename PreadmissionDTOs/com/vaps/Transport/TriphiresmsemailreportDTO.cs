using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TriphiresmsemailreportDTO : CommonParamDTO
    {
        //TR_Trip.
        public long MI_Id { get; set; }
    
         public DateTime frmdate { get; set; }
        public DateTime todate { get; set; }
        public string type { get; set; }
        public string templete { get; set; }
        public Array griddata { get; set; }
      
     
    }
}
