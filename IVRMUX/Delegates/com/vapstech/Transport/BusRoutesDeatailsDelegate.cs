using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class BusRoutesDeatailsDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<BusRoutesDetailsDTO, BusRoutesDetailsDTO> comml = new CommonDelegate<BusRoutesDetailsDTO, BusRoutesDetailsDTO>();

        public BusRoutesDetailsDTO Getreportdetails(BusRoutesDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "BusRoutesDetailsFacade/Getreportdetails/");
        }
        public BusRoutesDetailsDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "BusRoutesDetailsFacade/getdata/");
        }
    }
}
