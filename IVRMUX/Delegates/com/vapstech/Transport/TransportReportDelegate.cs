using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TransportReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TransportReportDTO, TransportReportDTO> comml = new CommonDelegate<TransportReportDTO, TransportReportDTO>();

        public TransportReportDTO Getreportdetails(TransportReportDTO data)
        {
            return comml.POSTDataTransport(data, "TransportReportFacade/Getreportdetails/");
        }
    }
}

