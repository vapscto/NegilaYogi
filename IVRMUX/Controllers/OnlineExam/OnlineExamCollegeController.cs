using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.OnlineExam;
using PreadmissionDTOs.com.vaps.OnlineExam;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class OnlineExamCollegeController : Controller
    {
        OnlineExamCollegeDelegate oed = new OnlineExamCollegeDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("getloaddata/{id:int}")]
        public OnlineExamDTO getloaddata(int id)
        {
            OnlineExamDTO data = new OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return oed.getloaddata(data);
        }

        [Route("getclass")]
        public OnlineExamDTO getSubjects([FromBody]OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.getSubjects(data);
        }

        [Route("getQuestion")]
        public OnlineExamDTO getQuestion([FromBody]OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.getQuestion(data);
        }

        [Route("Saveanswer")]
        public OnlineExamDTO Saveanswer([FromBody]OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         //   data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.Saveanswer(data);
        }

        

        [Route("savedanswers/{id:int}")]
        public OnlineExamDTO savedanswers(int id)
        {
            OnlineExamDTO data = new OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.savedanswers(data);
        }


        [Route("submitexam")]
        public OnlineExamDTO submitexam([FromBody]OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         //   data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.submitexam(data);
        }

    }
}
