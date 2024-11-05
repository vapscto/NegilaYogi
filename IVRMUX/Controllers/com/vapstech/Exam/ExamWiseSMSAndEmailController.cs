using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamWiseSMSAndEmailController : Controller
    {
        ExamWiseSMSAndEmailDelegate _delg = new ExamWiseSMSAndEmailDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public ExamWiseSMSAndEmailDTO loaddata(int id)
        {
            ExamWiseSMSAndEmailDTO data = new ExamWiseSMSAndEmailDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddata(data);
        }
        [Route("getclass")]
        public ExamWiseSMSAndEmailDTO getclass([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getclass(data);
        }
        [Route("getsection")]
        public ExamWiseSMSAndEmailDTO getsection([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getsection(data);
        }
        [Route("getexam")]
        public ExamWiseSMSAndEmailDTO getexam([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getexam(data);
        }
        [Route("searchDetails")]
        public ExamWiseSMSAndEmailDTO searchDetails([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.searchDetails(data);
        }
        [Route("SendSmsEmail")]
        public ExamWiseSMSAndEmailDTO SendSmsEmail([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SendSmsEmail(data);
        }
    }
}
