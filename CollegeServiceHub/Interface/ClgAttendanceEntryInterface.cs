using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgAttendanceEntryInterface
    {
        ClgAttendanceEntryDTO getalldetails(ClgAttendanceEntryDTO data);
   //     ClgAttendanceEntryDTO getCoursedata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getBranchdata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getSemesterdata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getSectiondata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getSubjdata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getBatchdata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO getStudentdata(ClgAttendanceEntryDTO data);
        ClgAttendanceEntryDTO saveatt(ClgAttendanceEntryDTO data);       

    }

}
