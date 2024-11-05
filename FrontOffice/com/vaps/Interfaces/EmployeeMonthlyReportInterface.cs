using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface EmployeeMonthlyReportInterface
    {

        EmployeeMonthlyReportDTO getdata(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_departments(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_designation(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_employee(EmployeeMonthlyReportDTO data);
        Task<EmployeeMonthlyReportDTO> getreport(EmployeeMonthlyReportDTO data);
        Task<EmployeeMonthlyReportDTO> getOTrpt(EmployeeMonthlyReportDTO data);
        Task<EmployeeMonthlyReportDTO> getrptStJames(EmployeeMonthlyReportDTO data);
    }
}
