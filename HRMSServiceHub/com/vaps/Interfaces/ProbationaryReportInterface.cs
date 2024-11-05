using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface ProbationaryReportInterface
    {
        EmployeeProfileReportDTO getalldetails(EmployeeProfileReportDTO dto);
        Task<EmployeeProfileReportDTO> getProbationaryReport(EmployeeProfileReportDTO dto);
        EmployeeProfileReportDTO get_departments(EmployeeProfileReportDTO dto);
        EmployeeProfileReportDTO get_designation(EmployeeProfileReportDTO dto);
    }
}
