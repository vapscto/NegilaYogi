using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface EmployeeLogReportInterface
    {
        EmployeeLogReportDTO getdata(EmployeeLogReportDTO data);
        EmployeeLogReportDTO get_departments(EmployeeLogReportDTO data);
        EmployeeLogReportDTO get_designation(EmployeeLogReportDTO data);
        EmployeeLogReportDTO get_employee(EmployeeLogReportDTO data);
        Task<EmployeeLogReportDTO> getreport(EmployeeLogReportDTO data);
        Task<EmployeeLogReportDTO> getsiglerpt(EmployeeLogReportDTO data);
    }
}
