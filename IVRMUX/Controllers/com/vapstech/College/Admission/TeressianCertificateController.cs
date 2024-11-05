using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class TeressianCertificateController : Controller
    {
        TeressianCertificateDelegate _delobj = new TeressianCertificateDelegate();
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

        [Route("getalldetails/{id:int}")]
        public TeressianCertificateDTO getalldetails(int id)
        {
            TeressianCertificateDTO data = new TeressianCertificateDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getalldetails(data);
        }
        [Route("getcoursedata")]
        public TeressianCertificateDTO getcoursedata([FromBody]TeressianCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getcoursedata(data);
        }

        [Route("getbranchdata")]
        public TeressianCertificateDTO getbranchdata([FromBody]TeressianCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getbranchdata(data);
        }

        [Route("getsemisterdata")]
        public TeressianCertificateDTO getsemisterdata([FromBody]TeressianCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getsemisterdata(data);
        }
        [Route("getsstudentdata")]
        public TeressianCertificateDTO getsstudentdata([FromBody]TeressianCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getsstudentdata(data);
        }
        [Route("GetCertificate")]
        public TeressianCertificateDTO GetCertificate([FromBody]TeressianCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.GetCertificate(data);
        }       
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
