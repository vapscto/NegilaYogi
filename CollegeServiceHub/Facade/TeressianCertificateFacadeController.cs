using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class TeressianCertificateFacadeController : Controller
    {
        private TeressianCertificateInterface _inter;

        public TeressianCertificateFacadeController(TeressianCertificateInterface obj)
        {
            _inter = obj;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public TeressianCertificateDTO Getdetails([FromBody] TeressianCertificateDTO data)
        {
            return _inter.getalldetails(data);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("getcoursedata")]
        public TeressianCertificateDTO getcoursedata([FromBody] TeressianCertificateDTO data)
        {
            return _inter.getcoursedata(data);
        }

        [Route("getbranchdata")]
        public TeressianCertificateDTO getbranchdata([FromBody] TeressianCertificateDTO data)
        {
            return _inter.getbranchdata(data);
        }

        [Route("getsemisterdata")]
        public TeressianCertificateDTO getsemisterdata([FromBody] TeressianCertificateDTO data)
        {
            return _inter.getsemisterdata(data);
        }
        [Route("getsstudentdata")]
        public TeressianCertificateDTO getsstudentdata([FromBody] TeressianCertificateDTO data)
        {
            return _inter.getsstudentdata(data);
        }
        [Route("GetCertificate")]
        public TeressianCertificateDTO GetCertificate([FromBody] TeressianCertificateDTO data)
        {
            return _inter.GetCertificate(data);
        }
       
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
