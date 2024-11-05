using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Chairman;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Chairman
{
    [Route("api/[controller]")]
    public class ManagementDashboardMonthly : Controller
    {
        ManagementDashboardMonthlyDelegate crStr = new ManagementDashboardMonthlyDelegate();
        [HttpGet]
        [Route("Getdetails")]
        public ManagementDashboardMonthlyDTO Getdetails(ManagementDashboardMonthlyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("getreport")]
        public ManagementDashboardMonthlyDTO getclassexam([FromBody] ManagementDashboardMonthlyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getreport(data);

        }


    }
}
