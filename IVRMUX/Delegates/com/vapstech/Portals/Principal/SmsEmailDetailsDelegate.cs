using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Principal
{
    public class SmsEmailDetailsDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SmsEmailDetailsDTO, SmsEmailDetailsDTO> COMMM = new CommonDelegate<SmsEmailDetailsDTO, SmsEmailDetailsDTO>();
        public SmsEmailDetailsDTO Getreportdetails(SmsEmailDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "SmsEmailDetailsFacade/Getreportdetails/");
        }
        public SmsEmailDetailsDTO Getreportdetails1(SmsEmailDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "SmsEmailDetailsFacade/Getreportdetails1/");
        }
        public SmsEmailDetailsDTO getdata(SmsEmailDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "SmsEmailDetailsFacade/getdata/");
        }
   
    }
}
