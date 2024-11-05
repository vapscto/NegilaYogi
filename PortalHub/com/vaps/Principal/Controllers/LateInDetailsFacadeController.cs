using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;
//using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class LateInDetailsFacadeController : Controller
    {
        public LateInDetailsInterface _PrincipalDashboardReport;

        public LateInDetailsFacadeController(LateInDetailsInterface data)
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
        public LateInDetailsDTO getalldetails([FromBody] LateInDetailsDTO data)
        {
            return _PrincipalDashboardReport.getalldetails(data);
        }

    }
}
