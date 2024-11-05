using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.PAOnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.PAOnlineExam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.PAOnlineExam
{
    [Route("api/[controller]")]
    public class PAOnlineExamController : Controller
    {
        PAOnlineExamDelegate oed = new PAOnlineExamDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getloaddata/{id:int}")]
        public PAOnlineExamDTO getloaddata(int id)
        {
            PAOnlineExamDTO data = new PAOnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return oed.getloaddata(data);
        }

        [Route("getclass")]
        public PAOnlineExamDTO getSubjects([FromBody]PAOnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.getSubjects(data);
        }

        [Route("getQuestion")]
        public PAOnlineExamDTO getQuestion([FromBody]PAOnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.getQuestion(data);
        }

        [Route("Saveanswer")]
        public PAOnlineExamDTO Saveanswer([FromBody]PAOnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //   data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.Saveanswer(data);
        }

        [Route("savedanswers/{id:int}")]
        public PAOnlineExamDTO savedanswers(int id)
        {
            PAOnlineExamDTO data = new PAOnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //  data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.savedanswers(data);
        }

        [Route("submitexam")]
        public PAOnlineExamDTO submitexam([FromBody]PAOnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //   data.Amst_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return oed.submitexam(data);
        }
    }
}
