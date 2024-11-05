using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface CategoryWiseAttendanceInterface
    {
        Task<StudentAttendanceReportDTO> getInitailData(int id);
        Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);

    }
}
