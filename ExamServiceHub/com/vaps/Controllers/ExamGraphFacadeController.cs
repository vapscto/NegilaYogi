using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamGraphFacadeController : Controller
    {
        private ExamGraphInterface _inter;
        public ExamGraphFacadeController(ExamGraphInterface obj)
        {
            _inter = obj;
        }
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

        // POST api/<controller>
        [HttpPost]
        [Route("getdetails")]
        public ExamGraphDTO getdetails([FromBody] ExamGraphDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public ExamGraphDTO onreport([FromBody] ExamGraphDTO data)
        {
            return _inter.onreport(data);
        }
        [Route("OnAcdyear")]
        public ExamGraphDTO OnAcdyear([FromBody]ExamGraphDTO data)
        {
            return _inter.OnAcdyear(data);
        }
        [Route("onclasschange")]
        public ExamGraphDTO onclasschange([FromBody]ExamGraphDTO data)
        {
            return _inter.onclasschange(data);
        }
        [Route("onsectionchange")]
        public ExamGraphDTO onsectionchange([FromBody]ExamGraphDTO data)
        {
            return _inter.onsectionchange(data);
        }
        [Route("onchangeexam")]
        public ExamGraphDTO onchangeexam([FromBody]ExamGraphDTO data)
        {
            return _inter.onchangeexam(data);
        }
        [Route("onchangecategory")]
        public ExamGraphDTO onchangecategory([FromBody]ExamGraphDTO data)
        {
            return _inter.onchangecategory(data);
        }
        [Route("onchangesubject")]
        public ExamGraphDTO onchangesubject([FromBody]ExamGraphDTO data)
        {
            return _inter.onchangesubject(data);
        }

        

    }
}
