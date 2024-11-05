using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class MarksEntry_SController : Controller
    {
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
         MarksEntry_SDelegates del_ssse =new MarksEntry_SDelegates();
        

        [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        [Route("Getdetails")]
        public MarksEntry_SDTO Getdetails(MarksEntry_SDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.Getdetails(data);
        }
        [HttpPost]
        [Route("get_classes")]
        public MarksEntry_SDTO onselectAcdYear([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.get_classes(dto);
        }
        [Route("get_sections")]
        public MarksEntry_SDTO get_sections([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.get_sections(dto);
        }
        [Route("get_exams")]
        public MarksEntry_SDTO get_exams([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.get_exams(dto);
        }
        [Route("get_subjects")]
        public MarksEntry_SDTO get_subjects([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.get_subjects(dto);
        }
        [Route("onsearch")]
        public MarksEntry_SDTO onsearch([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_ssse.onsearch(dto);
        }
        [Route("SaveMarks")]
        public MarksEntry_SDTO SaveMarks([FromBody] MarksEntry_SDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return del_ssse.SaveMarks(dto);
        }
    }
}
