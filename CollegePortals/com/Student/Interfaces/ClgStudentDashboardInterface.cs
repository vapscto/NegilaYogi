using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgStudentDashboardInterface
    {
        Task<ClgStudentDashboardDTO> Getdetails(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO ViewStudentProfile(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO onclick_syllabus(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO onclick_notice(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO onclick_noticeboard_seen(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO ViewMonthWiseAttendance(ClgStudentDashboardDTO data);
    }

}
