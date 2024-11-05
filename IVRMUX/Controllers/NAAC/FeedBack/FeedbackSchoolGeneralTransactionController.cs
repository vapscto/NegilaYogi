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
    public class FeedbackSchoolGeneralTransactionController : Controller
    {
        public FeedbackSchoolGeneralTransactionDelegate _delgate = new FeedbackSchoolGeneralTransactionDelegate();

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
        public FeedbackSchoolGeneralTransactionDTO getdetails([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.getdetails(data);
        }


        [Route("getfeedback")]
        public FeedbackSchoolGeneralTransactionDTO getfeedback([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.getfeedback(data);
        }   
        [Route("savedata")]
        public FeedbackSchoolGeneralTransactionDTO savedata([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.savedata(data);
        }

        [Route("getstudentstaffdetails")]
        public FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.getstudentstaffdetails(data);
        }
        [Route("getstaffname")]
        public FeedbackSchoolGeneralTransactionDTO getstaffname([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.getstaffname(data);
        }
        [Route("getfeedbacksubject")]
        public FeedbackSchoolGeneralTransactionDTO getfeedbacksubject([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delgate.getfeedbacksubject(data);
        }
        [Route("studentstaffsavedata")]
        public FeedbackSchoolGeneralTransactionDTO studentstaffsavedata([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delgate.studentstaffsavedata(data);
        }



    }
}
