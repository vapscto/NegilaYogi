using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface COEInterface
    {
       StudentDashboardDTO getloaddata(StudentDashboardDTO data);
        StudentDashboardDTO getcoedata(StudentDashboardDTO sddto);
    }
}
