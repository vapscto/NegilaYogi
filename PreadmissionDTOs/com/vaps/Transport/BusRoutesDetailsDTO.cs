using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class BusRoutesDetailsDTO
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

        public string flag { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }

        public secid[] secidlist { get; set; }
        public clsid[] clsidlist { get; set; }
        public string type { get; set; }

        public Array studentgriddata { get; set; }
    }

    public class secid
        {
        public long ASMS_Id { get; set; }
    }
    public class clsid
    {
        public long ASMCL_Id { get; set; }
    }
}
