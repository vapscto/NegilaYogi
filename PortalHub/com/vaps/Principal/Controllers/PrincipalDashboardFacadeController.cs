
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;
//using AdmissionServiceHub.com.vaps.Interfaces;


using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class PrincipalDashboardFacadeController : Controller
    {
        public PrincipalDashboardInterface _PrincipalDashboardReport;

        public PrincipalDashboardFacadeController(PrincipalDashboardInterface data)
        {
            _PrincipalDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public PrincipalDashboardDTO Getdetails([FromBody] PrincipalDashboardDTO data)//int IVRMM_Id
        {           
            return _PrincipalDashboardReport.Getdetails(data);          
        }

        [Route("onclick_notice")]
        public PrincipalDashboardDTO onclick_notice([FromBody] PrincipalDashboardDTO data)
        {
            return _PrincipalDashboardReport.onclick_notice(data);
        }

        [Route("viewnotice")]
        public PrincipalDashboardDTO viewnotice([FromBody] PrincipalDashboardDTO data)
        {
            return _PrincipalDashboardReport.viewnotice(data);
        }
    }
}
