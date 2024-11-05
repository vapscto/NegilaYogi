using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT.College;


namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGTTCommonController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGTTCommonDelegate ad = new CLGTTCommonDelegate();

        [HttpPost]
        [Route("getBranch")]
        public CLGTTCommonDTO getBranch([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getBranch(data);
        }
      [HttpPost]
        [Route("getcourse_catg")]
        public CLGTTCommonDTO getcourse_catg([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getcourse_catg(data);
        }
        [HttpPost]
        [Route("getbranch_catg")]
        public CLGTTCommonDTO getbranch_catg([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getbranch_catg(data);
        }
        [HttpPost]
        [Route("multplegetbranch_catg")]
        public CLGTTCommonDTO multplegetbranch_catg([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.multplegetbranch_catg(data);
        }
        [HttpPost]
        [Route("get_semister")]
        public CLGTTCommonDTO get_semister([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_semister(data);
        }
        [HttpPost]
        [Route("multget_semister")]
        public CLGTTCommonDTO multget_semister([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.multget_semister(data);
        }
        [HttpPost]
        [Route("get_section")]
        public CLGTTCommonDTO get_section([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_section(data);
        }
        [HttpPost]
        [Route("get_staff")]
        public CLGTTCommonDTO get_staff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_staff(data);
        }
        [HttpPost]
        [Route("get_subject")]
        public CLGTTCommonDTO get_subject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_subject(data);
        }
        [HttpPost]
        [Route("get_subject_onsec")]
        public CLGTTCommonDTO get_subject_onsec([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_subject_onsec(data);
        }

        [HttpPost]
        [Route("get_semday")]
        public CLGTTCommonDTO get_semday([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_semday(data);
        }

        [HttpPost]
        [Route("get_staffaca")]
        public CLGTTCommonDTO get_staffaca([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_staffaca(data);
        }

        [HttpPost]
        [Route("get_course_onstaff")]
        public CLGTTCommonDTO get_course_onstaff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_course_onstaff(data);
        }
        [HttpPost]
        [Route("get_branch_onstaff")]
        public CLGTTCommonDTO get_branch_onstaff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_branch_onstaff(data);
        }
        [HttpPost]
        [Route("get_sem_onstaff")]
        public CLGTTCommonDTO get_sem_onstaff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_sem_onstaff(data);
        }
        [HttpPost]
        [Route("get_sec_onstaff")]
        public CLGTTCommonDTO get_sec_onstaff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_sec_onstaff(data);
        }
        [HttpPost]
        [Route("get_subject_onstaff")]
        public CLGTTCommonDTO get_subject_onstaff([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_subject_onstaff(data);
        }
      [HttpPost]
        [Route("get_subjecttab3")]
        public CLGTTCommonDTO get_subjecttab3([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_subjecttab3(data);
        }

        [HttpPost]
        [Route("get_course_onsubject")]
        public CLGTTCommonDTO get_course_onsubject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_course_onsubject(data);
        }
         [HttpPost]
        [Route("get_branch_onsubject")]
        public CLGTTCommonDTO get_branch_onsubject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_branch_onsubject(data);
        }

        [HttpPost]
        [Route("get_sem_onsubject")]
        public CLGTTCommonDTO get_sem_onsubject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_sem_onsubject(data);
        }
        [HttpPost]
        [Route("get_sec_onsubject")]
        public CLGTTCommonDTO get_sec_onsubject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_sec_onsubject(data);
        }
        [HttpPost]
        [Route("get_staff_onsubject")]
        public CLGTTCommonDTO get_staff_onsubject([FromBody] CLGTTCommonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_staff_onsubject(data);
        }
      

    }
}
