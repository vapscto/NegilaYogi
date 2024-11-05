using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface PercentageWiseAttendanceReportInterface
    {
        PercentageWiseAttendanceReportDTO getloaddata(PercentageWiseAttendanceReportDTO data);
        PercentageWiseAttendanceReportDTO getclass(PercentageWiseAttendanceReportDTO data);
        PercentageWiseAttendanceReportDTO getsection(PercentageWiseAttendanceReportDTO data);
        PercentageWiseAttendanceReportDTO showreport(PercentageWiseAttendanceReportDTO data);
        Task<PercentageWiseAttendanceReportDTO> SendAttendanceSMS(PercentageWiseAttendanceReportDTO data);
    }
}
