using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface DatewiseAttendanceReportInterface
    {
        DatewiseAttendanceReportDTO getdata(DatewiseAttendanceReportDTO data);
        DatewiseAttendanceReportDTO onchangeyear(DatewiseAttendanceReportDTO data);
        DatewiseAttendanceReportDTO onchangeclass(DatewiseAttendanceReportDTO data);
        DatewiseAttendanceReportDTO getreport(DatewiseAttendanceReportDTO data);
        DatewiseAttendanceReportDTO getcountreport(DatewiseAttendanceReportDTO data);
        DatewiseAttendanceReportDTO Reportnew(DatewiseAttendanceReportDTO data);
        
    }
}
