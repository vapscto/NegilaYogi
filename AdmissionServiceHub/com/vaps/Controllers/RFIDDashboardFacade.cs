using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]
    public class RFIDDashboardFacade : Controller
    {
        public RFIDDashboardInterface _ads;

        public RFIDDashboardFacade(RFIDDashboardInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
       

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

     
    
        [Route("Getdetails")]
        public RFIDDashboardDTO Getdetails([FromBody] RFIDDashboardDTO id)
        {
            return _ads.Getdetails(id);
        }
        [Route("showstudentGrid")]
        public RFIDDashboardDTO showstudentGrid([FromBody] RFIDDashboardDTO id)
        {
            return _ads.showstudentGrid(id);
        }
        [Route("cleardata")]
        public RFIDDashboardDTO cleardata([FromBody] RFIDDashboardDTO id)
        {
            return _ads.cleardata(id);
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
