using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeStudentExamResultsInterface
    {
        EmployeeDashboardDTO getdata(EmployeeDashboardDTO obj);

        EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_class(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_section(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_student(EmployeeDashboardDTO data);
        EmployeeDashboardDTO get_exam(EmployeeDashboardDTO data);
        EmployeeDashboardDTO saveRemark(EmployeeDashboardDTO data);
        EmployeeDashboardDTO getremarkdetails(EmployeeDashboardDTO data);
    }
}
