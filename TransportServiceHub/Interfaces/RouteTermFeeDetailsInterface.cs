using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
 public   interface RouteTermFeeDetailsInterface
    {
        RouteTermFeeDetailsDTO getdata(int id);
        RouteTermFeeDetailsDTO Getreportdetails(RouteTermFeeDetailsDTO data);
    }
}
