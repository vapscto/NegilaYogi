using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers.College
{
    [Route("api/[controller]")]
    public class CLGTTCollegewiseConsolidatedReportFacadeController : Controller
    {
        public CLGTTCollegewiseConsolidatedReportInterface inter;

        public CLGTTCollegewiseConsolidatedReportFacadeController(CLGTTCollegewiseConsolidatedReportInterface u)
        {
            inter = u;
        }
        [Route("loaddata")]
        public CLGTTCollegewiseConsolidatedReportDTO loaddata([FromBody] CLGTTCollegewiseConsolidatedReportDTO data)
        {
            return inter.loaddata(data);
        }
       [Route("report")]
       public CLGTTCollegewiseConsolidatedReportDTO report([FromBody] CLGTTCollegewiseConsolidatedReportDTO data)
        {
            return inter.report(data);
        }
    }
}
