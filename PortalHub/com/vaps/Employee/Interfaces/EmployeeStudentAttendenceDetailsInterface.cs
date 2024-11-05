using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeStudentAttendenceDetailsInterface
    {
        EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data);
        EmployeeDashboardDTO getclass(EmployeeDashboardDTO data);
        EmployeeDashboardDTO Getsection(EmployeeDashboardDTO data);
        EmployeeDashboardDTO GetAttendence(EmployeeDashboardDTO data);
        Task<EmployeeDashboardDTO> GetIndividualAttendence(EmployeeDashboardDTO data);
    }
}
