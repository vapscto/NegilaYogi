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
    public class EmployeeStudentReportCardFacade : Controller
    {
        public EmployeeStudentReportCardInterface _PCReportContext;

        public EmployeeStudentReportCardFacade(EmployeeStudentReportCardInterface data)
        {
            _PCReportContext = data;
        }


        [Route("Getdetails")]
        public EmployeeDashboardDTO Getdetails([FromBody]EmployeeDashboardDTO data)//int IVRMM_Id
        {

            return _PCReportContext.Getdetails(data);

        }

      
        [Route("showdetails")]
        public EmployeeDashboardDTO showdetails([FromBody] EmployeeDashboardDTO data)
        {
            return _PCReportContext.showdetails(data);
        }

        [Route("get_class")]
        public EmployeeDashboardDTO get_class([FromBody] EmployeeDashboardDTO data)
        {
            return _PCReportContext.get_class(data);
        }
        [Route("get_section")]
        public EmployeeDashboardDTO get_section([FromBody] EmployeeDashboardDTO data)
        {
            return _PCReportContext.get_section(data);
        }
        [Route("get_student")]
        public EmployeeDashboardDTO get_student([FromBody] EmployeeDashboardDTO data)
        {
            return _PCReportContext.get_student(data);
        }
        [Route("get_exam")]
        public EmployeeDashboardDTO get_exam([FromBody] EmployeeDashboardDTO data)
        {
            return _PCReportContext.get_exam(data);
        }
       

    }
}
