using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff
{
    public class ClgEmployeePunchAttendenceDelegate
    {
        CommonDelegate<ClgStaffDashboardDTO, ClgStaffDashboardDTO> COMFRNT = new CommonDelegate<ClgStaffDashboardDTO, ClgStaffDashboardDTO>();
        public ClgStaffDashboardDTO getdata(ClgStaffDashboardDTO data)
        {
            return COMFRNT.CLGPortalPOSTData(data, "ClgEmployeePunchAttendenceFacade/getalldetails/");
        }
       
        public ClgStaffDashboardDTO getrpt(ClgStaffDashboardDTO student)
        {
            return COMFRNT.CLGPortalPOSTData(student, "ClgEmployeePunchAttendenceFacade/getrpt/");
        }
    }
}
