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
    public class OnlineExamConfigController : Controller
    {
        OnlineExamConfigDelegate oed = new OnlineExamConfigDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("getloaddata/{id:int}")]
        public MasterQuestionDTO getloaddata(int id)
        {
            MasterQuestionDTO data = new MasterQuestionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return oed.getloaddata(data);
        }
        //----------------------------1stTab
        [Route("savedata")]
        public MasterQuestionDTO savedata([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.savedata(data);
        }
        [Route("editQuestion")]
        public MasterQuestionDTO editQuestion([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.editQuestion(data);
        }
        [Route("Deletedetails")]
        public MasterQuestionDTO Deletedetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.Deletedetails(data);
        }

        [Route("getreport")]
        public MasterQuestionDTO getreport([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getreport(data);
        }

        //=====================online exam report new


        [Route("getsection")]
        public MasterQuestionDTO getsection([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.getsection(data);
        }
        
        [Route("getonlinereport")]
        public MasterQuestionDTO getonlinereport([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.getonlinereport(data);
        }
        


    }
}
