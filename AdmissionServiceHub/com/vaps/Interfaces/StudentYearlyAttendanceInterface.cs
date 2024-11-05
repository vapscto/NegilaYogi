using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentYearlyAttendanceInterface
    {
        Task<StudentAttendanceReportDTO> getInitailData(int mi_id);
        Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getclass(StudentAttendanceReportDTO dto);
    }
}
