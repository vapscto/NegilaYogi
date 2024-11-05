using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.FeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.FeedBack
{
    [Route("api/[controller]")]
    public class FeedbackTransactionController : Controller
    {

        public FeedbackTransactionDelegate _delgate = new FeedbackTransactionDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdetails")]
        public FeedbackTransactionDTO getdetails([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.getdetails(data);
        }
        [Route("getfeedback")]
        public FeedbackTransactionDTO getfeedback([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.getfeedback(data);
        }

        
        [Route("savedata")]
        public FeedbackTransactionDTO savedata([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.savedata(data);
        }

        [Route("getstudentstaffdetails")]
        public FeedbackTransactionDTO getstudentstaffdetails([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.getstudentstaffdetails(data);
        }
        [Route("getstaffname")]
        public FeedbackTransactionDTO getstaffname([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.getstaffname(data);
        }
        [Route("getfeedbacksubject")]
        public FeedbackTransactionDTO getfeedbacksubject([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.getfeedbacksubject(data);
        }
        [Route("studentstaffsavedata")]
        public FeedbackTransactionDTO studentstaffsavedata([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.studentstaffsavedata(data);
        }

        // Feedbcak Modulewise 17-02-2024


        [Route("Savefeedback")]
        public FeedbackTransactionDTO modulewisefeedback([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
           
            return _delgate.modulewisefeedback(data);
        }

        //load data

        [Route("loadfeedbackquestion")]
        public FeedbackTransactionDTO loadfeedbackquestion([FromBody] FeedbackTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delgate.loadfeedbackquestion(data);
        }


    }
}
