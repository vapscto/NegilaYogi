using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RouteTermFeeDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<RouteTermFeeDetailsDTO, RouteTermFeeDetailsDTO> comml = new CommonDelegate<RouteTermFeeDetailsDTO, RouteTermFeeDetailsDTO>();

        public RouteTermFeeDetailsDTO Getreportdetails(RouteTermFeeDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "RouteTermFeeDetailsFacade/Getreportdetails/");
        }
        public RouteTermFeeDetailsDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "RouteTermFeeDetailsFacade/getdata/");
        }
    }
}
