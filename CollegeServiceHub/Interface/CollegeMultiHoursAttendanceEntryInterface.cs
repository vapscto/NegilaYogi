using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeMultiHoursAttendanceEntryInterface
    {
        CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO bal_getalldetails(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getCoursedata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getBranchdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getSemesterdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getSectiondata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getSubjdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getStudentdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO saveatt(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO delete(CollegeMultiHoursAttendanceEntryDTO data);
        Task<CollegeMultiHoursAttendanceEntryDTO> autoscheduler(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview(CollegeMultiHoursAttendanceEntryDTO data);
        
    }
}
