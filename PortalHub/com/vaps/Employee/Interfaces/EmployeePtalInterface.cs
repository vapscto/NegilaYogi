using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface EmployeePtalInterface
    {
        EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto);
        EmployeeDashboardDTO saveakpkfile(EmployeeDashboardDTO data);
        EmployeeDashboardDTO viewnotice(EmployeeDashboardDTO data);
        EmployeeDashboardDTO onclick_notice(EmployeeDashboardDTO data);
        EmployeeDashboardDTO onclick_events(EmployeeDashboardDTO data);

        EmployeeDashboardDTO onclick_Homework_datewise(EmployeeDashboardDTO data);
        EmployeeDashboardDTO onclick_classwork_datewise(EmployeeDashboardDTO data);
        EmployeeDashboardDTO onclick_noticeboard_datewise(EmployeeDashboardDTO data);
        EmployeeDashboardDTO onclick_asset(EmployeeDashboardDTO data);
    }
}
