using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TriphiresmsemailreportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TriphiresmsemailreportDTO, TriphiresmsemailreportDTO> comml = new CommonDelegate<TriphiresmsemailreportDTO, TriphiresmsemailreportDTO>();

        public TriphiresmsemailreportDTO Getreportdetails(TriphiresmsemailreportDTO data)
        {
            return comml.POSTDataTransport(data, "TriphiresmsemailreportFacade/Getreportdetails/");
        }
        public TriphiresmsemailreportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "TriphiresmsemailreportFacade/getdata/");
        }
    }
}
