using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeSalaryDetailsInterface
    {
        EmployeeDashboardDTO getdata(EmployeeDashboardDTO obj);

        EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO data);
        EmployeeDashboardDTO getsalaryalldetails(EmployeeDashboardDTO data);
    }
}
