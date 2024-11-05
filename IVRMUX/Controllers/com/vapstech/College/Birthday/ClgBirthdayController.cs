using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using PreadmissionDTOs.com.vaps.FrontOffice;
//using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.BirthDay;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using corewebapi18072016.Delegates.com.vapstech.College.BirthDay;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Birthday
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgBirthdayController : Controller
    {
        ClgBirthdayDelegate dele = new ClgBirthdayDelegate();
        // GET: api/values
        [HttpGet]
        public ClgBirthDayDTO getloaddata(int id)
        {
            ClgBirthDayDTO data = new ClgBirthDayDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getloaddata(data);
        }
        [Route("radiochange")]
        public ClgBirthDayDTO radiochange([FromBody] ClgBirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return dele.radiochange(data);
        }
        [Route("sendmsg")]
        public ClgBirthDayDTO sendmsg([FromBody] ClgBirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return dele.sendmsg(data);
        }

        

    }
}
