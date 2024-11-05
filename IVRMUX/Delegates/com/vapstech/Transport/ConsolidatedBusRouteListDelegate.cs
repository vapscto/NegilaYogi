using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class ConsolidatedBusRouteListDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<ConsolidatedBusRouteDTO, ConsolidatedBusRouteDTO> comml = new CommonDelegate<ConsolidatedBusRouteDTO, ConsolidatedBusRouteDTO>();

        public ConsolidatedBusRouteDTO Getreportdetails(ConsolidatedBusRouteDTO data)
        {
            return comml.POSTDataTransport(data, "ConsolidatedBusRouteListFacade/Getreportdetails/");
        }
        public ConsolidatedBusRouteDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "ConsolidatedBusRouteListFacade/getdata/");
        }
    }
}
