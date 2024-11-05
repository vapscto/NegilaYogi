using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HolidayReportController : Controller
    {
        HolidayReportDelegate dele = new HolidayReportDelegate();
        // GET: api/values
        [HttpGet("{id:int}")]
        public MasterHolidayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdata(id);
        }
        [HttpPost]
        [Route("Report")]
        // GET api/values/5
        public MasterHolidayDTO HolidayReport([FromBody]MasterHolidayDTO report)
        {
            report.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.HolidayReport(report);
        }

    }
}
