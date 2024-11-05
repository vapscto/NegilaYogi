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
    public class ClgStudentDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgStudentDashboardDTO, ClgStudentDashboardDTO> COMMM = new CommonDelegate<ClgStudentDashboardDTO, ClgStudentDashboardDTO>();
        public ClgStudentDashboardDTO Getdetails(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/Getdetails/");
        }


        public ClgStudentDashboardDTO ViewStudentProfile(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/ViewStudentProfile/");
        }
        public ClgStudentDashboardDTO onclick_syllabus(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/onclick_syllabus/");
        }
        public ClgStudentDashboardDTO onclick_notice(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/onclick_notice/");
        }
        public ClgStudentDashboardDTO onclick_noticeboard_seen(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/onclick_noticeboard_seen/");
        }
        public ClgStudentDashboardDTO ViewMonthWiseAttendance(ClgStudentDashboardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentDashboardFacade/ViewMonthWiseAttendance/");
        }
    }
}
