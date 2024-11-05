using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface Student_TTInterface
    {
        StudentDashboardDTO getloaddata(StudentDashboardDTO data);
        Task<StudentDashboardDTO> getStudentTT(StudentDashboardDTO sddto);
    }
}
