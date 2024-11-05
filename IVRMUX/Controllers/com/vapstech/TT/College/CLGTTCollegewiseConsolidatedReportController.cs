using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT.College;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGTTCollegewiseConsolidatedReportController : Controller
    {
        CLGTTCollegewiseConsolidatedReportDelegate del = new CLGTTCollegewiseConsolidatedReportDelegate();
        [Route("loaddata/{id:int}")]
        public CLGTTCollegewiseConsolidatedReportDTO loaddata(int id)
        {
            CLGTTCollegewiseConsolidatedReportDTO data = new CLGTTCollegewiseConsolidatedReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }

      [Route("report")]
      public CLGTTCollegewiseConsolidatedReportDTO report([FromBody] CLGTTCollegewiseConsolidatedReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.report(data);
        }
      

    }
}
