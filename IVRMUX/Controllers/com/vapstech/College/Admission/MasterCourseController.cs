using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class MasterCourseController : Controller
    {
        MasterCourseDelegate objDel = new MasterCourseDelegate();

        [HttpPost]
        [Route("Savedetails")]
        public MasterCourseDTO Savedetails([FromBody]MasterCourseDTO id)
        {
            //id.MI_Id = 4;
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails")]
        public MasterCourseDTO getalldetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getalldetails(id);
        }
        [HttpPost]
        [Route("Deletedetails")]
        public MasterCourseDTO Deletedetails([FromBody]MasterCourseDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Deletedetails(id);
        }

        [Route("getOrder")]
        public MasterCourseDTO getOrder([FromBody]MasterCourseDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getOrder(id);
        }
        [Route("EditData")]
        public MasterCourseDTO EditData([FromBody]MasterCourseDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objDel.EditData(id);
        }


    }
}
