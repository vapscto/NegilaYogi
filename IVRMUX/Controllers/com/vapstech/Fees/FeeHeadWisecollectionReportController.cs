using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeHeadWisecollectionReportController : Controller
    {

        FeeHeadWisecollectionReportDelegate FHCR = new FeeHeadWisecollectionReportDelegate();
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
        [Route("getalldetails/{id:int}")]
        public FeeHeadWisecollectionReportDTO getalldetails( int id)
        {
            FeeHeadWisecollectionReportDTO data = new FeeHeadWisecollectionReportDTO();
            return FHCR.getalldetails(data);
        }

        [Route ("getreport")]
        public FeeHeadWisecollectionReportDTO getreport ([FromBody] FeeHeadWisecollectionReportDTO data)
        {
            return FHCR.getreport(data);
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
