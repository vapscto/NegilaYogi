
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class PrincipalDefaulterFeeFacadeController : Controller
    {
        public PrincipalDefaulterFeeInterface _PrincipalDashboardReport;

        public PrincipalDefaulterFeeFacadeController(PrincipalDefaulterFeeInterface data)
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
        public PrincipalDefaulterFeeDTO Getdetails([FromBody] PrincipalDefaulterFeeDTO data)
        {


            return _PrincipalDashboardReport.Getdetails(data);

        }

        [HttpPost]
        [Route("getclass")]
        public PrincipalDefaulterFeeDTO getclass([FromBody] PrincipalDefaulterFeeDTO data)
        {


            return _PrincipalDashboardReport.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public PrincipalDefaulterFeeDTO Getsection([FromBody] PrincipalDefaulterFeeDTO data)
        {


            return _PrincipalDashboardReport.Getsection(data);

        }
        [HttpPost]
        [Route("Getreport")]
        public PrincipalDefaulterFeeDTO Getreport([FromBody] PrincipalDefaulterFeeDTO data)
        {


            return _PrincipalDashboardReport.Getreport(data);

        }

        [HttpPost]
        [Route("Getstudentdetails")]
        public PrincipalDefaulterFeeDTO Getstudentdetails([FromBody] PrincipalDefaulterFeeDTO data)
        {


            return _PrincipalDashboardReport.Getstudentdetails(data);

        }



    }
}
