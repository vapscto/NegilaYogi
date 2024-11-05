using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeMasterSectionController : Controller
    {
        CollegeMasterSectionDelegate mastersect = new CollegeMasterSectionDelegate();
        // GET: api/values
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

        [Route("getalldetails/{id:int}")]
        public CollegeMasterSectionDTO getalldetails(int id)
        {          
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.getalldetails(id);
        }
        [Route("Editdetails")]
        public CollegeMasterSectionDTO Editdetails([FromBody] CollegeMasterSectionDTO id)
        {           
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.Editdetails(id);
        }

        [Route("saveMasterdata")]
        public CollegeMasterSectionDTO saveMasterdata([FromBody] CollegeMasterSectionDTO id)
        {
            
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.saveMasterdata(id);
        }
        [Route("saveorder")]
        public CollegeMasterSectionDTO saveorder([FromBody] CollegeMasterSectionDTO id)
        {            
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.saveorder(id);
        }
        [Route("Deletedetails")]
        public CollegeMasterSectionDTO Deletedetails([FromBody] CollegeMasterSectionDTO id)
        {           
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.Deletedetails(id);
        }
    }
}
