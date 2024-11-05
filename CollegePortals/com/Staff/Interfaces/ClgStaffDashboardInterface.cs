using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace CollegePortals.com.Staff.Interfaces
{
    public interface  ClgStaffDashboardInterface
    {
       Task<ClgStaffDashboardDTO> getloaddata(ClgStaffDashboardDTO data);
     
        
    }
}
