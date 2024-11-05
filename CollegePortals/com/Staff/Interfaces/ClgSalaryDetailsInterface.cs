using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;

namespace CollegePortals.com.Staff.Interfaces
{
    public interface ClgSalaryDetailsInterface
    {
        ClgPortalHRMSDTO getloaddata(ClgPortalHRMSDTO data);
        ClgPortalHRMSDTO getSalary(ClgPortalHRMSDTO data);
        ClgPortalHRMSDTO getsalaryalldetails(ClgPortalHRMSDTO data);
        

    }
}
