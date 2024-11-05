using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeAttendanceEntryNewInterface
    {
        CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getsubjectslist(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getStudentdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO saveatt(CollegeMultiHoursAttendanceEntryDTO data);
        CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview(CollegeMultiHoursAttendanceEntryDTO data);
    }
}
