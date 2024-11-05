using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public interface OverallDailyAttendanceInterface
    {
      Task<StudentAttendanceReportDTO> getInitailData(StudentAttendanceReportDTO id);
      Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);
      Task<StudentAttendanceReportDTO> getoveallattendance(StudentAttendanceReportDTO data);
      StudentAttendanceReportDTO getStudentDetails(StudentAttendanceReportDTO data);
      StudentAttendanceReportDTO getStudentAllDetails(StudentAttendanceReportDTO data);
        
    }
}
