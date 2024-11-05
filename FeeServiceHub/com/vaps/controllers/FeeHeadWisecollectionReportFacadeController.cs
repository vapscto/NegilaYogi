using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using FeeServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeHeadWisecollectionReportFacadeController : Controller
    {
        public FeeHeadWisecollectionReportInterface _feeheadcollection;


        public FeeHeadWisecollectionReportFacadeController(FeeHeadWisecollectionReportInterface maspag)
        {
            _feeheadcollection = maspag;
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
        [Route("getdetails")]
        public FeeHeadWisecollectionReportDTO getdetails (FeeHeadWisecollectionReportDTO data)
        {
            return _feeheadcollection.getdetails(data);
        }
        [Route ("getreport")]
        public Task<FeeHeadWisecollectionReportDTO> getreport([FromBody]FeeHeadWisecollectionReportDTO data)
        {
            return _feeheadcollection.getreport(data);
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
