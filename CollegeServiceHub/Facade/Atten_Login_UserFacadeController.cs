using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Atten_Login_UserFacadeController : Controller
    {
        public Atten_Login_UserInterface _inter;
        public Atten_Login_UserFacadeController(Atten_Login_UserInterface inter)
        {
            _inter = inter;
        }

        [HttpPost]
        [Route("getalldetails")]
        public Atten_Login_UserDTO getalldetails([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("get_courses")]
        public Atten_Login_UserDTO get_courses([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Login_UserDTO get_branches([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Login_UserDTO get_semisters([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.get_semisters(data);
        }        
        [Route("savedata")]
        public Atten_Login_UserDTO savedata([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("view_subjects")]
        public Atten_Login_UserDTO view_subjects([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.view_subjects(data);
        }
        [Route("Deletedetails")]
        public Atten_Login_UserDTO Deletedetails([FromBody] Atten_Login_UserDTO data)
        {
            return _inter.Deletedetails(data);
        }
    }
}
