using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Staff.Interfaces
{
    public interface ClgEmployeePunchAttendenceInterface
    {
        ClgStaffDashboardDTO getdata(ClgStaffDashboardDTO data);
       
       Task< ClgStaffDashboardDTO> getreport(ClgStaffDashboardDTO data);
        
    }
}
