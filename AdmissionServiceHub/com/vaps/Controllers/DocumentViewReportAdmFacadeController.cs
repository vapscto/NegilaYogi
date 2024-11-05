using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DocumentViewReportAdmFacadeController : Controller
    {
        public DocumentViewReportAdmInterface _int;
         
        public DocumentViewReportAdmFacadeController(DocumentViewReportAdmInterface _intj)
        {
            _int = _intj;
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
        [Route("getdetails/{id:int}")]
        public DocumentViewReportAdmDTO getdetails(int id)
        {
            return _int.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("getstudent")]
        public DocumentViewReportAdmDTO getstudent([FromBody] DocumentViewReportAdmDTO data)
        {
            return _int.getstudent(data);
        }
        [Route("getreport")]
        public DocumentViewReportAdmDTO getreport([FromBody] DocumentViewReportAdmDTO data)
        {
            return _int.getreport(data);
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
