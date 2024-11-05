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
    public interface ClgStudentSearchInterface
    {
        ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> getcoursedata(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> getbranchdata(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> getsemdata(ClgPortalAttendanceDTO data);
        ClgPortalAttendanceDTO getstudent(ClgPortalAttendanceDTO data);
        Task<ClgPortalAttendanceDTO> getreport(ClgPortalAttendanceDTO data);
 
        
    }
}
