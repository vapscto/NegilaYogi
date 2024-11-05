using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface AttendanceDetailsInterface
    {
       StudentDashboardDTO getloaddata(StudentDashboardDTO data);
        Task<StudentDashboardDTO> getAttdata(StudentDashboardDTO data);
    }
}
