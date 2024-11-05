using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface EmployeeInOutReportInterface
    {
        EmployeeInOutReportDTO getdata(EmployeeInOutReportDTO data);
        EmployeeInOutReportDTO get_departments(EmployeeInOutReportDTO data);
        EmployeeInOutReportDTO get_designation(EmployeeInOutReportDTO data);
        EmployeeInOutReportDTO get_employee(EmployeeInOutReportDTO data);
        EmployeeInOutReportDTO getreport(EmployeeInOutReportDTO data);
        Task<EmployeeInOutReportDTO> lateIn_details(EmployeeInOutReportDTO mi_id);
    }
}
