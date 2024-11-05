using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODStudentSearchController : Controller
    {

        HODStudentSearchDelegate objdelegate = new HODStudentSearchDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public StudentSearchDTO getdetails(StudentSearchDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
           // data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getalldetails(data);
        }

        [Route("getstudentdetails")]
        public StudentSearchDTO getstudentdetails([FromBody]StudentSearchDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getstudentdetails(data);
        }
    }
}
