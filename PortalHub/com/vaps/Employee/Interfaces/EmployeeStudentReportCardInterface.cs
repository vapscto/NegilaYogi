using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeStudentReportCardInterface
    {
        EmployeeDashboardDTO showdetails(EmployeeDashboardDTO data);
       

        EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data);

        EmployeeDashboardDTO get_class(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_section(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_student(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_exam(EmployeeDashboardDTO data);
       

    }
}
