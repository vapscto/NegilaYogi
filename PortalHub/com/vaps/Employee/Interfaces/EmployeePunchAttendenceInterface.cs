using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeePunchAttendenceInterface
    {
        EmployeeDashboardDTO getdata(EmployeeDashboardDTO data);
       
       Task< EmployeeDashboardDTO> getreport(EmployeeDashboardDTO data);
        
    }
}
