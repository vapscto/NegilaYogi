using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamsubjectGroupMappingFacadeController : Controller
    {
        public ExamsubjectGroupMappingInterface _intf;
        public ExamsubjectGroupMappingFacadeController(ExamsubjectGroupMappingInterface intf)
        {
            _intf = intf;
        }
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("Getdetails")]
        public ExamsubjectGroupMappingDTo Getdetails([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.Getdetails(data);
        }
        [Route("getcategory")]
        public ExamsubjectGroupMappingDTo getcategory([FromBody] ExamsubjectGroupMappingDTo data)
        {
            return _intf.getcategory(data);
        }
        [Route("getexam")]
        public ExamsubjectGroupMappingDTo getexam([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.getexam(data);
        }
        [Route("getsubject")]
        public ExamsubjectGroupMappingDTo getsubject([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.getsubject(data);
        }
        [Route("savedetails")]
        public ExamsubjectGroupMappingDTo savedetails([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.savedetails(data);
        }
        [Route("getlist")]
        public ExamsubjectGroupMappingDTo getlist([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.getlist(data);
        }
        [Route("Editexammasterdata1")]
        public ExamsubjectGroupMappingDTo Editexammasterdata1([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.Editexammasterdata1(data);
        }
        [Route("viewrecordspopup")]
        public ExamsubjectGroupMappingDTo viewrecordspopup([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.viewrecordspopup(data);
        }
        [Route("deactivate")]
        public ExamsubjectGroupMappingDTo deactivate([FromBody]ExamsubjectGroupMappingDTo data)
        {
            return _intf.deactivate(data);
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
