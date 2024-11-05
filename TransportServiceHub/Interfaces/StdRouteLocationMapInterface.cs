using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
 public   interface StdRouteLocationMapInterface
    {
        StdRouteLocationMapDTO getdata(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO get_sections(StdRouteLocationMapDTO data);

        StdRouteLocationMapDTO get_cls_secs(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO Getreportdetails(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO getreport(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO on_pic_route_change(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO getreportedit(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO savedata(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO check_feegroup(StdRouteLocationMapDTO data);
        StdRouteLocationMapDTO deactivate(StdRouteLocationMapDTO data);

        StdRouteLocationMapDTO get_data(StdRouteLocationMapDTO data);


    }
}
