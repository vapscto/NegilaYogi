using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HODAttendanceDetailsController : Controller
    {
        HODAttendanceDetailsDelegate crStr = new HODAttendanceDetailsDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
       
        [Route("Getdetails")]
        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = 5;
            return crStr.Getdetails(data);
        }


        [Route("getclass")]
        public ADMAttendenceDTO getclass([FromBody]ADMAttendenceDTO data)
        {
            //ADMAttendenceDTO data = new ADMAttendenceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = id;
            return crStr.getclass(data);
        }
        
        [Route("Getsection")]
        public ADMAttendenceDTO Getsection([FromBody] ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);

        }

       
        [Route("GetAttendence")]
        public ADMAttendenceDTO GetAttendence([FromBody] ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.GetAttendence(data);

        }
        
        [Route("GetIndividualAttendence")]
        public ADMAttendenceDTO GetIndividualAttendence([FromBody] ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.GetIndividualAttendence(data);

        }

    }
}
