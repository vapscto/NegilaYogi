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
    public interface ClgLiveMeetingScheduleInterface
    {
        ClgLiveMeetingScheduleDTO getloaddata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getcoursedata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getbranchdata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getsection(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getsemdata(ClgLiveMeetingScheduleDTO data);
        ClgLiveMeetingScheduleDTO editdata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> savedata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> deactive(ClgLiveMeetingScheduleDTO data);

        //STAFF PROFILE
        Task<ClgLiveMeetingScheduleDTO> getempdetails(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> ondatechange(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> onstartmeeting(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> endmainmeeting(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> joinmeeting(ClgLiveMeetingScheduleDTO data);

        //STUDENT
        Task<ClgLiveMeetingScheduleDTO> getstudentdetails(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> endmainmeetingstudent(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> joinmeetingStudent(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> ondatechangestudent(ClgLiveMeetingScheduleDTO data);

        Task<ClgLiveMeetingScheduleDTO> getschrptdetails(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getschrptdetailsprofile(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getschedulereport(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getstaffprofilereport(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getstudentprofiledata(ClgLiveMeetingScheduleDTO data);
        Task<ClgLiveMeetingScheduleDTO> getstudentprofilereport(ClgLiveMeetingScheduleDTO data);
 
        
    }
}
