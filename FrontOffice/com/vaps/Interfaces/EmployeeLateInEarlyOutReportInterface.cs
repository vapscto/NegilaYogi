using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface EmployeeLateInEarlyOutReportInterface
    {
        EmployeeLateInEarlyOutReportDTO getdata(EmployeeLateInEarlyOutReportDTO data);
        EmployeeLateInEarlyOutReportDTO get_departments(EmployeeLateInEarlyOutReportDTO data);
        EmployeeLateInEarlyOutReportDTO get_designation(EmployeeLateInEarlyOutReportDTO data);
        EmployeeLateInEarlyOutReportDTO get_employee(EmployeeLateInEarlyOutReportDTO data);
        Task<EmployeeLateInEarlyOutReportDTO> getreport(EmployeeLateInEarlyOutReportDTO data);
    }
}
