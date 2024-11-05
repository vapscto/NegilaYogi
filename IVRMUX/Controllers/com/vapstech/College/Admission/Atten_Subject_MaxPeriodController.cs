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
    public class Atten_Subject_MaxPeriodController : Controller
    {
        Atten_Subject_MaxPeriodDelegate _del = new Atten_Subject_MaxPeriodDelegate();
          // GET api/values/5
          [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Atten_Subject_MaxPeriodDTO getalldetails(int id)
        {
            Atten_Subject_MaxPeriodDTO data = new Atten_Subject_MaxPeriodDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getalldetails(data);
        }

        // POST api/values
        [HttpPost]       
        [Route("get_courses")]
        public Atten_Subject_MaxPeriodDTO get_courses([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Subject_MaxPeriodDTO get_branches([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Subject_MaxPeriodDTO get_semisters([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_semisters(data);
        }        
        [Route("get_subjects")]
        public Atten_Subject_MaxPeriodDTO get_subjects([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_subjects(data);
        }
        [Route("savedata")]
        public Atten_Subject_MaxPeriodDTO savedata([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.savedata(data);
        }
        [Route("Deletedetails")]
        public Atten_Subject_MaxPeriodDTO Deletedetails([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Deletedetails(data);
        }
        [Route("showmodaldetails")]
        public Atten_Subject_MaxPeriodDTO showmodaldetails([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.showmodaldetails(data);
        }
        [Route("deactivesem")]
        public Atten_Subject_MaxPeriodDTO deactivesem([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactivesem(data);
        }
        
    }
}
