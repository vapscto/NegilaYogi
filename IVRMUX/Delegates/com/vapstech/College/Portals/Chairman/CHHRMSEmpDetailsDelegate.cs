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
    public class CHHRMSEmpDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CHHRMSEmpSalaryDTO, CHHRMSEmpSalaryDTO> COMMM = new CommonDelegate<CHHRMSEmpSalaryDTO, CHHRMSEmpSalaryDTO>();
        public CHHRMSEmpSalaryDTO Getdetails(CHHRMSEmpSalaryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CHHRMSEmpDetailsFacade/Getdetails/");
        }      
    }
}
