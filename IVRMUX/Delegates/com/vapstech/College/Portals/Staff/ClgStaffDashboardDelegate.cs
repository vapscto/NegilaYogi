using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff
{
    public class  ClgStaffDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgStaffDashboardDTO, ClgStaffDashboardDTO> COMMM = new CommonDelegate<ClgStaffDashboardDTO, ClgStaffDashboardDTO>();
        public ClgStaffDashboardDTO getloaddata(ClgStaffDashboardDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgStaffDashboardFacade/getloaddata/");
        }
        

    }
}
