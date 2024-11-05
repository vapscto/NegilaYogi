using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface CollegeAttendanceAbsentSMSInterface
    {
        CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectAcdYear(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectCourse(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectBranch(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO getsection(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> getAttendetails(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> absentsendsms(CollegeDailyAttendanceDTO data);


    }
}
