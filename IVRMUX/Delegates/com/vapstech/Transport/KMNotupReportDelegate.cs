using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class KMNotupReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TripReportDTO, TripReportDTO> comml = new CommonDelegate<TripReportDTO, TripReportDTO>();

        public TripReportDTO Getreportdetails(TripReportDTO data)
        {
            return comml.POSTDataTransport(data, "KMNotupReportFacade/Getreportdetails/");
        }
        public TripReportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "KMNotupReportFacade/getdata/");
        }
    }
}
