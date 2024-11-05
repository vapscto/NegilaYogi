using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Portals;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals
{
    public class CLGCHSubjectDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGCHSubjectDetailsDTO, CLGCHSubjectDetailsDTO> COMMM = new CommonDelegate<CLGCHSubjectDetailsDTO, CLGCHSubjectDetailsDTO>();
        public CLGCHSubjectDetailsDTO Getdetails(CLGCHSubjectDetailsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CLGCHSubjectDetailsFacade/Getdetails/");
        }
        public CLGCHSubjectDetailsDTO Getdetails1(CLGCHSubjectDetailsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CLGCHSubjectDetailsFacade/Getdetails1/");
        }
    }
}
