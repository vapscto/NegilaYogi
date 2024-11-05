using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamsubjectGroupMappingController : Controller
    {
       // public ExamsubjectGroupMappingDelegate _exmgrp;
        ExamsubjectGroupMappingDelegate _exmgrp = new ExamsubjectGroupMappingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("Getdetails/{id:int}")]
        public ExamsubjectGroupMappingDTo Getdetails(int id)
        {
            ExamsubjectGroupMappingDTo data = new ExamsubjectGroupMappingDTo();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmgrp.Getdetails(data);
        }

        [Route("getcategory")]
        public ExamsubjectGroupMappingDTo getcategory([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmgrp.getcategory(data);
        }
        [Route("getexam")]
        public ExamsubjectGroupMappingDTo getexam([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmgrp.getexam(data);
        }
        [Route("getsubject")]
        public ExamsubjectGroupMappingDTo getsubject([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmgrp.getsubject(data);
        }
        [Route("savedetails")]
        public ExamsubjectGroupMappingDTo savedetails([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmgrp.savedetails(data);
        }
        [Route("getlist")]
        public ExamsubjectGroupMappingDTo getlist([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _exmgrp.getlist(data);
        }
        [Route("Editexammasterdata1")]
        public ExamsubjectGroupMappingDTo Editexammasterdata1([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _exmgrp.Editexammasterdata1(data);
        }
        [Route("viewrecordspopup")]
        public ExamsubjectGroupMappingDTo viewrecordspopup([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _exmgrp.viewrecordspopup(data);
        }
        [Route("deactivate")]
        public ExamsubjectGroupMappingDTo deactivate([FromBody] ExamsubjectGroupMappingDTo data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _exmgrp.deactivate(data);
        }
        

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
