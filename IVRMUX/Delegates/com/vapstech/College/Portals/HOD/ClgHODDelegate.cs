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
    public class ClgHODDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgStudentDashboardDTO, ClgStudentDashboardDTO> COMMM = new CommonDelegate<ClgStudentDashboardDTO, ClgStudentDashboardDTO>();
        public ClgStudentDashboardDTO Getdetails(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgHODDashboardFacade/Getdetails/");
        }

        public ClgStudentDashboardDTO save(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgHODDashboardFacade/save/");
        }
        public ClgStudentDashboardDTO mappHOD(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgHODDashboardFacade/mappHOD/");
        }
        public ClgStudentDashboardDTO updateHOD(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgHODDashboardFacade/updateHOD/");
        }
        public ClgStudentDashboardDTO deactiveY(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgHODDashboardFacade/deactiveY/");
        }


    }
}
