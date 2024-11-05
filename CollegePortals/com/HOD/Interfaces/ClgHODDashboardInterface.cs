using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgHODDashboardInterface
    {
        Task<ClgStudentDashboardDTO> Getdetails(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO savedata(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO mappHODdata(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO updateHOD(ClgStudentDashboardDTO data);
        ClgStudentDashboardDTO deactiveY(ClgStudentDashboardDTO data);


    }

}
