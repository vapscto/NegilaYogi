using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeePtalFacade : Controller
    {
        public EmployeePtalInterface _empl;
        public EmployeePtalFacade(EmployeePtalInterface empl)
        {
            _empl = empl;
        }

        [Route("getdata")]
        public EmployeeDashboardDTO getdata([FromBody] EmployeeDashboardDTO obj)
        {
            return _empl.getdata(obj);
        }

        [Route("saveakpkfile")]
        public EmployeeDashboardDTO saveakpkfile([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.saveakpkfile(data);
        }
        [Route("viewnotice")]
        public EmployeeDashboardDTO viewnotice([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.viewnotice(data);
        }
        [Route("onclick_notice")]
        public EmployeeDashboardDTO onclick_notice([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_notice(data);
        }
        [Route("onclick_events")]
        public EmployeeDashboardDTO onclick_events([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_events(data);
        }
        [Route("onclick_Homework_datewise")]
        public EmployeeDashboardDTO onclick_Homework_datewise([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_Homework_datewise(data);
        }

        [Route("onclick_classwork_datewise")]
        public EmployeeDashboardDTO onclick_classwork_datewise([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_classwork_datewise(data);
        }

        [Route("onclick_noticeboard_datewise")]
        public EmployeeDashboardDTO onclick_noticeboard_datewise([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_noticeboard_datewise(data);
        }
        [Route("onclick_asset")]
        public EmployeeDashboardDTO onclick_asset([FromBody] EmployeeDashboardDTO data)
        {
            return _empl.onclick_asset(data);
        }
    }
}
