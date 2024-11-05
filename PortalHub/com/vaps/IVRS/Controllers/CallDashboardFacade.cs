using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRS.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRS.Controllers
{
    [Route("api/[controller]")]
    public class CallDashboardFacade : Controller
    {
        public CallDashboardInterface _inter; 
        public CallDashboardFacade(CallDashboardInterface c)
        {
            _inter = c;
        }

        [Route("loadData")]
        public Task<CallDashboardDTO> loadData([FromBody] CallDashboardDTO data)
        {
            return _inter.loadData(data);
        }
    }
}
