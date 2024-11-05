using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RouteSessionTotalStrengthDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<RouteSessionTotalStrengthDTO, RouteSessionTotalStrengthDTO> comml = new CommonDelegate<RouteSessionTotalStrengthDTO, RouteSessionTotalStrengthDTO>();

        public RouteSessionTotalStrengthDTO Getreportdetails(RouteSessionTotalStrengthDTO data)
        {
            return comml.POSTDataTransport(data, "RouteSessionTotalStrengthFacade/Getreportdetails/");
        }
        public RouteSessionTotalStrengthDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "RouteSessionTotalStrengthFacade/getdata/");
        }
    }
}
