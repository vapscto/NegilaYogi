using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class Atten_Login_UserController : Controller
    {
        Atten_Login_UserDelegate _del = new Atten_Login_UserDelegate();
          // GET api/values/5
          [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Atten_Login_UserDTO getalldetails(int id)
        {
            Atten_Login_UserDTO data = new Atten_Login_UserDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getalldetails(data);
        }

        // POST api/values
        [HttpPost]       
        [Route("get_courses")]
        public Atten_Login_UserDTO get_courses([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Login_UserDTO get_branches([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Login_UserDTO get_semisters([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_semisters(data);
        }       
        [Route("savedata")]
        public Atten_Login_UserDTO savedata([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.savedata(data);
        }
        [Route("view_subjects")]
        public Atten_Login_UserDTO get_subjects([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.view_subjects(data);
        }
        [Route("Deletedetails")]
        public Atten_Login_UserDTO Deletedetails([FromBody] Atten_Login_UserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Deletedetails(data);
        }
    }
}
