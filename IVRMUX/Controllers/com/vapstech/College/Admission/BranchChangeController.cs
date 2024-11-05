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
    public class BranchChangeController : Controller
    {
        BranchChangeDelegate objDel = new BranchChangeDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public BranchChangeDTO getalldetails(int id)
        {
            BranchChangeDTO ddd = new BranchChangeDTO();
            ddd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getdetails(ddd);
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
        [Route("Studentdetails")]
        public BranchChangeDTO Studentdetails([FromBody] BranchChangeDTO ddd)
        {          
            ddd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Studentdetails(ddd);
        }
        [Route("Savedetails")]
        public BranchChangeDTO Savedetails([FromBody] BranchChangeDTO ddd)
        {
            ddd.userid= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ddd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Savedetails(ddd);
        }
        [Route("deactive")]
        public BranchChangeDTO deactive([FromBody] BranchChangeDTO ddd)
        {
            ddd.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ddd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.deactive(ddd);
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
