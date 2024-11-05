using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Principal;
//using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class CareerReportFacadeController : Controller
    {
        public CareerReportInterface _PrincipalDashboardReport;

        public CareerReportFacadeController(CareerReportInterface data)
        {
            _PrincipalDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpPost]
        [Route("getalldetails")]
        public CareerReportDTO getalldetails([FromBody] CareerReportDTO data)
        {
            return _PrincipalDashboardReport.getalldetails(data);
        }

        //==========================home/class work upload

        [HttpPost]
        [Route("get_home_classwork")]
        public IVRM_Homework_DTO get_home_classwork([FromBody] IVRM_Homework_DTO data)
        {
            return _PrincipalDashboardReport.get_home_classwork(data);
        }
    }
}
       

