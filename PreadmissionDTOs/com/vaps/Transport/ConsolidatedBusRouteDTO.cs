using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class ConsolidatedBusRouteDTO
    {
        public long MI_Id { get; set; }
        public Array YearList { get; set; }
        public long ASMAY_Id { get; set; }
        public bool Is_Active { get; set; }

        public long TRMR_Id { get; set; }    
        public long TRMA_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }

        public Array messagelist { get; set; }
        public Array classdata { get; set; }
        public long stud_count { get; set; }
        public Array griddata { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_Id { get; set; }
    }
}
