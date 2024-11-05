using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface EmployeeYearlyReportInterface
    {
        EmployeeYearlyReportDTO getdata(EmployeeYearlyReportDTO data);
        EmployeeYearlyReportDTO get_departments(EmployeeYearlyReportDTO data);
        EmployeeYearlyReportDTO get_designation(EmployeeYearlyReportDTO data);
        EmployeeYearlyReportDTO get_employee(EmployeeYearlyReportDTO data);
        Task<EmployeeYearlyReportDTO> getreport(EmployeeYearlyReportDTO data);
    }
}
