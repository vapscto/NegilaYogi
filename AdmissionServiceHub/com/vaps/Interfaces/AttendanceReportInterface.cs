using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface AttendanceReportInterface
    {
        Task<StudentAttendanceReportDTO> getInitailData(StudentAttendanceReportDTO id);
        Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);
        Task<StudentAttendanceReportDTO> getsection(StudentAttendanceReportDTO id);
        Task<StudentAttendanceReportDTO> getclass(StudentAttendanceReportDTO id);
        Task<StudentAttendanceReportDTO> shortageOfAttendanceAlert(StudentAttendanceReportDTO id);
    }
}