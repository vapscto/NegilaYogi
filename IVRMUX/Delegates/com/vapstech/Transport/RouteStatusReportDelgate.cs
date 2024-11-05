using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RouteStatusReportDelgate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<RouteStatusReportDTO, RouteStatusReportDTO> comml = new CommonDelegate<RouteStatusReportDTO, RouteStatusReportDTO>();

        public RouteStatusReportDTO Getreportdetails(RouteStatusReportDTO data)
        {
            return comml.POSTDataTransport(data, "RouteStatusReportFacade/Getreportdetails/");
        }
        public RouteStatusReportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "RouteStatusReportFacade/getdata/");
        }
    }
}
