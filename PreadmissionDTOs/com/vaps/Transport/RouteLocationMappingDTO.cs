using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class RouteLocationMappingDTO
    {
        public long TRMRL_Id { get; set; }
        public int TRMR_order { get; set; }
        public long MI_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRML_Id { get; set; }
        public int TRMRL_Order { get; set; }
        public string routename { get; set; }
        public string locationname { get; set; }
        public Array getdetails { get; set; }
        public Array getdetails1 { get; set; }
        public Array routedetails { get; set; }
        public Array routedetailsarea { get; set; }
        public Array locationdetails { get; set; }
        public string message { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public bool TRMRL_ActiveFlag { get; set; }
        public long TRMA_Id { get; set; }
        public string trmR_RouteName { get; set; }
        public selected_locations[] selectedlocations { get; set; }
        public Array selectedroutes { get; set; }
        public selected_route[] selectedroute { get; set; }
        public selected_order[] selectedorder { get; set; }
    }
    public class selected_locations
    {
        public long TRML_Id { get; set; }
        public string locationname { get; set; }
    }
    public class selected_route
    {
        public long TRML_Id { get; set; }
        public string routename { get; set; }
    }
    public class selected_order
    {
        public long TRMRL_Id { get; set; }
        public int TRMRL_Order { get; set; }
    }
}
