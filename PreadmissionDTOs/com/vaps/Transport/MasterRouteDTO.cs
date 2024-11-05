using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterRouteDTO
    {
        public long TRMR_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMA_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public string TRMA_AreaName { get; set; }
        public bool TRMR_ActiveFlg { get; set; }
        public Array getroutemater { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array geteditdata { get; set; }
        public Array getzonearea { get; set; }
        public int TRMR_order { get; set; }
        public Array areawisedetails { get; set; }
        public Array routedata { get; set; } 
        public Array routearea { get; set; }
        public temp_masterroute[] temp_masterroute { get; set; }
        public arealistarrs[] arealistarr { get; set; }
        // Route Area Mapping 
        public long TRAR_Id { get; set; }
        public bool TRAR_ActiveFlg { get; set; }
    }

    public class temp_masterroute
    {
        public long TRMR_Id { get; set; }
        public long TRMA_Id { get; set; }
        public int TRMR_order { get; set; }
    }
    public class arealistarrs
    {
        public long TRMA_Id { get; set; }
    }
}
