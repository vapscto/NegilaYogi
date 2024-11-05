using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgCOEInterface
    {
        ClgStudentDashboardDTO getloaddata(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO getcoedata(ClgStudentDashboardDTO data);
    }
}
