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
    public class CollegeUsernameCreationController : Controller
    {
        CollegeUsernameCreationDelegate _delg = new CollegeUsernameCreationDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails/{id:int}")]
        public CollegeUsernameCreationDTO getalldetails(int id)
        {
            CollegeUsernameCreationDTO data = new CollegeUsernameCreationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetails(data);
        }
        [Route("onyearchange")]
        public CollegeUsernameCreationDTO onyearchange([FromBody] CollegeUsernameCreationDTO id)
        {            
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onyearchange(id);
        }
        [Route("onCoursechange")]
        public CollegeUsernameCreationDTO onCoursechange([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onCoursechange(id);
        }
        [Route("onBranchchange")]
        public CollegeUsernameCreationDTO onBranchchange([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onBranchchange(id);
        }
        [Route("onSemchange")]
        public CollegeUsernameCreationDTO onSemchange([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onSemchange(id);
        }
        [Route("get_Studentdetails")]
        public CollegeUsernameCreationDTO get_Studentdetails([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_Studentdetails(id);
        }
        [Route("saveatt")]
        public CollegeUsernameCreationDTO saveatt([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.saveatt(id);
        }
        [Route("getStudentusername")]
        public CollegeUsernameCreationDTO getStudentusername([FromBody] CollegeUsernameCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getStudentusername(id);
        }
        [Route("SendSMS")]
        public CollegeUsernameCreationDTO SendSMS([FromBody] CollegeUsernameCreationDTO id)
        {
            id.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.SendSMS(id);
        }
        
    }
}
