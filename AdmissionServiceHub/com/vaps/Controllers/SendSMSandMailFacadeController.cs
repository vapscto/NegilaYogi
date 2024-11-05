using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using AdmissionServiceHub.com.vaps.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SendSMSandMailFacadeController : Controller
    {
        public SendingSMSandMailsInterface _acd;
        public SendSMSandMailFacadeController(SendingSMSandMailsInterface acdm)
        {
            _acd = acdm;
        }

      
        [HttpGet]
        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public CommonDTO getorgdet(int id)
        {
            return _acd.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        [Route("getdetailsstudentorstaff/")]
        public CommonDTO getdetailsstudentorstaff([FromBody]CommonDTO data)
        {
            return _acd.getdetailsstudentorstaff(data);
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
