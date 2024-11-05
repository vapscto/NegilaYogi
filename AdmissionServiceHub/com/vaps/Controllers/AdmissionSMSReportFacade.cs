using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AdmissionSMSReportFacade : Controller
    {

        public AdmissionSMSReportInterface _feegrouppagee;
        // GET: api/values

        public AdmissionSMSReportFacade(AdmissionSMSReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public AdmissionSMSReportDTO getdetails([FromBody]AdmissionSMSReportDTO data)
        {
            return _feegrouppagee.getdetails(data);
        }


      


        [Route("Getreportdetails")]
        public AdmissionSMSReportDTO Getreportdetails([FromBody]AdmissionSMSReportDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
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
