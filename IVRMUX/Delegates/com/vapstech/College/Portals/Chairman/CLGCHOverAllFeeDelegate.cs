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
    public class CLGCHOverAllFeeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGGRPHeadFeeDetailsDTO, CLGGRPHeadFeeDetailsDTO> COMMM = new CommonDelegate<CLGGRPHeadFeeDetailsDTO, CLGGRPHeadFeeDetailsDTO>();
        public CLGGRPHeadFeeDetailsDTO Getdetails(CLGGRPHeadFeeDetailsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CLGCHOverAllFeeFacade/Getdetails/");
        }      
    }
}
