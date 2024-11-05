using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterRouteScheduleDTO
    {
        public long TRRSC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TRRSC_Date { get; set; }
        public string TRRSC_ScheduleName { get; set; }
        public bool TRRSC_ActiveFlag { get; set; }
        public long TRMR_Id { get; set; }
        public Array getroute { get; set; }
        public Array getdata { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string TRMR_RouteName { get; set; }
        public Array geteditdata { get; set; }

    }
}
