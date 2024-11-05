using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.HOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODPaycareStaffDetailsController : Controller
    {

        HODPaycareStaffDetailsDelegate crStr = new HODPaycareStaffDetailsDelegate();


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails")]
        public HODPaycareStaffDetails_DTO Getdetails(HODPaycareStaffDetails_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("ondptchange")]
        public HODPaycareStaffDetails_DTO ondatechange([FromBody] HODPaycareStaffDetails_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("Getemppop")]
        public HODPaycareStaffDetails_DTO Getsectionpop([FromBody] HODPaycareStaffDetails_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getemppop(data);
        }



    }
}
