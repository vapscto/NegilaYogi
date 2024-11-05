using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeStudentDetailsInterface
    {
        EmployeeDashboardDTO getdata(EmployeeDashboardDTO obj);
        
        EmployeeDashboardDTO get_class(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_section(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_student(EmployeeDashboardDTO data);
    }
}
