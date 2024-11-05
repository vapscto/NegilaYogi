using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [Route("api/[controller]")]
    public class TimeTableController : Controller
    {
        TimeTableDelegate objdelegate1 = new TimeTableDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    
        [Route("getalldetails")]
        public TimeTableDTO getalldetails(TimeTableDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getalldetails(data);
        }

        [Route("getdaily_data")]
        public TimeTableDTO getdaily_data([FromBody] TimeTableDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getdaily_data(data);
        }
    }
}
