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
using PreadmissionDTOs.com.vaps.Portals.Principal;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals
{
    public class ClgPrincipalDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<clgChairmanDashboardDTO, clgChairmanDashboardDTO> COMMM = new CommonDelegate<clgChairmanDashboardDTO, clgChairmanDashboardDTO>();
        public clgChairmanDashboardDTO Getdetails(clgChairmanDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgPrincipalDashboardFacade/Getdetails/");
        }



    }
}
