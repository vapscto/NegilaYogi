using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.COE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.COE;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.COE
{
    [Route("api/[controller]")]
    public class CoeReportGraph : Controller
    {
        CoeReportGraphDelegate coedel = new CoeReportGraphDelegate();    
        [HttpGet]      
         [Route("getdata/{id:int}")]
        public COEReportDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return coedel.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        [Route("getReport")]
        public COEReportDTO getReport([FromBody]COEReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return coedel.getReport(data);
        }
        [Route("mothreport")]
        public COEReportDTO mothreport([FromBody]COEReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return coedel.mothreport(data);
        }
    }
}
