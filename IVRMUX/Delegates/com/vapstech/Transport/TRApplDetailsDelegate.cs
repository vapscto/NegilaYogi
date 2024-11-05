using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TRApplDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TRApplDetailsDTO, TRApplDetailsDTO> comml = new CommonDelegate<TRApplDetailsDTO, TRApplDetailsDTO>();

        public TRApplDetailsDTO Getreportdetails(TRApplDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "TRApplDetailsFacade/Getreportdetails/");
        }
        public TRApplDetailsDTO sendmsg(TRApplDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "TRApplDetailsFacade/sendmsg/");
        }
        public TRApplDetailsDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "TRApplDetailsFacade/getdata/");
        }
    }
}
