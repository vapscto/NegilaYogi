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
    public class CLGCHStudentStrengthDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGCHStudentStrengthDTO, CLGCHStudentStrengthDTO> COMMM = new CommonDelegate<CLGCHStudentStrengthDTO, CLGCHStudentStrengthDTO>();
        public CLGCHStudentStrengthDTO Getdetails(CLGCHStudentStrengthDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CLGCHStudentStrengthFacade/Getdetails/");
        }
        public CLGCHStudentStrengthDTO Getdetails1(CLGCHStudentStrengthDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "CLGCHStudentStrengthFacade/Getdetails1/");
        }
    }
}
