using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgAttendanceDetailsInterface
    {
        ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> getAttdata(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> MblgetAttdata(ClgPortalAttendanceDTO data);
    }
}
