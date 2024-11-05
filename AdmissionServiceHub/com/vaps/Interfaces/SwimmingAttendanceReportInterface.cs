using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SwimmingAttendanceReportInterface
    {
        SwimmingAttendanceReportDTO loaddata(SwimmingAttendanceReportDTO data);
        SwimmingAttendanceReportDTO onchnageyear(SwimmingAttendanceReportDTO data);
        SwimmingAttendanceReportDTO onchangeclass(SwimmingAttendanceReportDTO data);
        SwimmingAttendanceReportDTO search(SwimmingAttendanceReportDTO data);
    }
}
