using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGRouteSessionStrengthDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGRouteSessionStrengthDTO, CLGRouteSessionStrengthDTO> comml = new CommonDelegate<CLGRouteSessionStrengthDTO, CLGRouteSessionStrengthDTO>();

        public CLGRouteSessionStrengthDTO Getreportdetails(CLGRouteSessionStrengthDTO data)
        {
            return comml.POSTDataTransport(data, "CLGRouteSessionStrengthFacade/Getreportdetails/");
        }
        public CLGRouteSessionStrengthDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "CLGRouteSessionStrengthFacade/getdata/");
        }
    }
}
