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
    public class Atten_Batch_MappingController : Controller
    {
        Atten_Batch_MappingDelegate _del = new Atten_Batch_MappingDelegate();
          // GET api/values/5
          [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Atten_Batch_MappingDTO getalldetails(int id)
        {
            Atten_Batch_MappingDTO data = new Atten_Batch_MappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getalldetails(data);
        }

        // POST api/values
        [HttpPost]
        [Route("savedata1")]
        public Atten_Batch_MappingDTO savedata1([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.savedata1(data);
        }
        [Route("get_courses")]
        public Atten_Batch_MappingDTO get_courses([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Batch_MappingDTO get_branches([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Batch_MappingDTO get_semisters([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_semisters(data);
        }
        [Route("get_students")]
        public Atten_Batch_MappingDTO get_students([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_students(data);
        }
        [Route("savedata2")]
        public Atten_Batch_MappingDTO savedata2([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.savedata2(data);
        }
        [Route("view_subjects")]
        public Atten_Batch_MappingDTO get_subjects([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.view_subjects(data);
        }
        [Route("Deletedetails")]
        public Atten_Batch_MappingDTO Deletedetails([FromBody] Atten_Batch_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Deletedetails(data);
        }
    }
}
