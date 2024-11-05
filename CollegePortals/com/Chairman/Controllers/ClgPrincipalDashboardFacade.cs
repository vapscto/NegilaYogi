using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePortals.com.Chairman.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePortals.com.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class ClgPrincipalDashboardFacade : Controller
    {

        public ClgPrincipalDashboardInterface _principal;

        public ClgPrincipalDashboardFacade(ClgPrincipalDashboardInterface adstu)
        {
            _principal = adstu;
        }

        [HttpPost]
        [Route("Getdetails")]
        public Task<clgChairmanDashboardDTO> Getdetails([FromBody]clgChairmanDashboardDTO data)
        {
            return _principal.Getdetails(data);
        }

    }
}
