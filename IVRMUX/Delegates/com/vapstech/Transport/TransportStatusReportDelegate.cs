using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TransportStatusReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TransportStatusReportDTO, TransportStatusReportDTO> comml = new CommonDelegate<TransportStatusReportDTO, TransportStatusReportDTO>();

        public TransportStatusReportDTO Getreportdetails(TransportStatusReportDTO data)
        {
            return comml.POSTDataTransport(data, "TransportStatusReportFacade/Getreportdetails/");
        }
        public TransportStatusReportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "TransportStatusReportFacade/getdata/");
        }

    }
}
