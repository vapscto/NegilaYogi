using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class ManagementDashboardMonthlyFacade : Controller
    {
        public ManagementDashboardMonthlyInterface _ChairmanDashboardReport;

        public ManagementDashboardMonthlyFacade(ManagementDashboardMonthlyInterface data)
        {
            _ChairmanDashboardReport = data;
        }
        [HttpPost]
        [Route("Getdetails")]
        public ManagementDashboardMonthlyDTO Getdetails([FromBody] ManagementDashboardMonthlyDTO data)
        {
          
            return _ChairmanDashboardReport.Getdetails(data);

        }
        [HttpPost]
        [Route("getreport")]
        public ManagementDashboardMonthlyDTO getreport([FromBody] ManagementDashboardMonthlyDTO data)
        {
            
            return _ChairmanDashboardReport.getreport(data);

        }
    }
}
