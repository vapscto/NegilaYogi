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
    public class ExamGraphController : Controller
    {
        ExamGraphDelegates _delobj = new ExamGraphDelegates();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ExamGraphDTO getdetails(ExamGraphDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        // POST api/<controller>
        [HttpPost]

        [Route("onreport")]
        public ExamGraphDTO onreport([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
        }

        [Route("OnAcdyear")]
        public ExamGraphDTO OnAcdyear([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.OnAcdyear(data);
        }
        [Route("onclasschange")]
        public ExamGraphDTO onclasschange([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onclasschange(data);
        }
        [Route("onsectionchange")]
        public ExamGraphDTO onsectionchange([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onsectionchange(data);
        }
        [Route("onchangeexam")]
        public ExamGraphDTO onchangeexam([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onchangeexam(data);
        }
        [Route("onchangecategory")]
        public ExamGraphDTO onchangecategory([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onchangecategory(data);
        }
        [Route("onchangesubject")]
        public ExamGraphDTO onchangesubject([FromBody]ExamGraphDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onchangesubject(data);
        }

    }
}
