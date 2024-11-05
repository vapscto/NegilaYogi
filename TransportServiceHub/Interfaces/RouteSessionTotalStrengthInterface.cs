using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
public    interface RouteSessionTotalStrengthInterface
    {
        RouteSessionTotalStrengthDTO getdata(int id);
        RouteSessionTotalStrengthDTO Getreportdetails(RouteSessionTotalStrengthDTO data);
    }
}
