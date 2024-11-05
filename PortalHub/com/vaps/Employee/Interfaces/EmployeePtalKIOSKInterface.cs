using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface EmployeePtalKIOSKInterface
    {
        Task<EmployeeKioskLEAVEDTO> getleave_report(EmployeeKioskLEAVEDTO data);
        EmployeeKIOSKPortalDTO getEmployeedata(EmployeeKIOSKPortalDTO dto);
        Task<EmployeeKioskPunchDTO> getPunchreport(EmployeeKioskPunchDTO data);
        EmployeeKIOSKPortalDTO getEmployeeFullDetails(EmployeeKIOSKPortalDTO dto);
        EmployeeKioskSalaryDTO getyeardata(EmployeeKioskSalaryDTO dto);
        EmployeeKioskSalaryDTO getsalarydetailsdata(EmployeeKioskSalaryDTO dto);
        EmployeeKioskTimeTableDTO getTTdata(EmployeeKioskTimeTableDTO dto);
    }
}
