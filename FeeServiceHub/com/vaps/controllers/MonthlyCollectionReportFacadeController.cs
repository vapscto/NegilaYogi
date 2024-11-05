using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class MonthlyCollectionReportFacadeController : Controller
    {
        public MonthlyCollectionReportInterface _feegroup;

        public MonthlyCollectionReportFacadeController(MonthlyCollectionReportInterface maspag)
        {
            _feegroup = maspag;
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
        public MonthlyCollectionReportDTO getdetails([FromBody]MonthlyCollectionReportDTO daat)
        {
            return _feegroup.getdetails(daat);
        }

        [Route("getregnoname")]
        public MonthlyCollectionReportDTO getregnoname([FromBody]MonthlyCollectionReportDTO value)
        {
            return _feegroup.getstuddet(value);
        }

        [Route("getreport")]
        public Task<MonthlyCollectionReportDTO> getreport([FromBody] MonthlyCollectionReportDTO valuere)
        {
            return _feegroup.getreport(valuere);
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
