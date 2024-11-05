using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeDailyAttendanceInterface
    {
        CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectAcdYear(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectCourse(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO onselectBranch(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO getsection(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO getsubject(CollegeDailyAttendanceDTO data);        
        Task<CollegeDailyAttendanceDTO> onreport(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> onreportpercentage(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> getAttendetails(CollegeDailyAttendanceDTO data);
        CollegeDailyAttendanceDTO getStudentAbsentDetails(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> GetAttendancedetails(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> absentsendsms(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> onreportshortagepercentage(CollegeDailyAttendanceDTO data);
        Task<CollegeDailyAttendanceDTO> onreporttotalattendance(CollegeDailyAttendanceDTO data);
        
    }
}
