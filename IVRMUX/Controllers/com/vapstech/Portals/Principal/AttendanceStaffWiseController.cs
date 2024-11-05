
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AttendanceStaffWiseController : Controller
    {
        AttendanceStaffWiseDelegates SendStr = new AttendanceStaffWiseDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}")]
        public string Getdetails(int id)
        {
            return "value";
        }
        [Route("Getdetails")]
        public AttendanceStaffWiseDTO Getdetails([FromBody]AttendanceStaffWiseDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }

        [Route("Getdepartment/{id}")]
        public AttendanceStaffWiseDTO Getdepartment(int id)
        {
            AttendanceStaffWiseDTO data = new AttendanceStaffWiseDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return SendStr.Getdepartment(data);
        }

        // POST api/values
        [Route("get_designation")]
        public AttendanceStaffWiseDTO get_designation([FromBody]AttendanceStaffWiseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_designation(data);
        }
        [Route("get_department")]
        public AttendanceStaffWiseDTO get_department([FromBody]AttendanceStaffWiseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_department(data);
        }

        [Route("get_employee")]
        public AttendanceStaffWiseDTO get_employee([FromBody]AttendanceStaffWiseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_employee(data);
        }
    }

}
