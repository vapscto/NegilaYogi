using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TransportServiceHub.Interfaces
{
   public  interface CLGRouteStatusReportInterface
    {
        RouteStatusReportDTO getdata(int id);
        RouteStatusReportDTO Getreportdetails(RouteStatusReportDTO data);
    }
}
