using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeAdmssionCancelProcessController : Controller
    {
        CollegeAdmssionCancelProcessDelegate _delg = new CollegeAdmssionCancelProcessDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails/{id:int}")]
        public CollegeAdmssionCancelProcessDTO getalldetails(int id)
        {
            CollegeAdmssionCancelProcessDTO data = new CollegeAdmssionCancelProcessDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetails(data);
        }
        [Route("onyearchange")]
        public CollegeAdmssionCancelProcessDTO onyearchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onyearchange(id);
        }
        [Route("onCoursechange")]
        public CollegeAdmssionCancelProcessDTO onCoursechange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onCoursechange(id);
        }
        [Route("onBranchchange")]
        public CollegeAdmssionCancelProcessDTO onBranchchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onBranchchange(id);
        }
        [Route("onSemchange")]
        public CollegeAdmssionCancelProcessDTO onSemchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onSemchange(id);
        }
        [Route("get_Studentdetails")]
        public CollegeAdmssionCancelProcessDTO get_Studentdetails([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_Studentdetails(id);
        }
        [Route("saveatt")]
        public CollegeAdmssionCancelProcessDTO saveatt([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveatt(id);
        }
        [Route("getStudentdetails")]
        public CollegeAdmssionCancelProcessDTO getStudentdetails([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getStudentdetails(id);
        }        

    }
}
