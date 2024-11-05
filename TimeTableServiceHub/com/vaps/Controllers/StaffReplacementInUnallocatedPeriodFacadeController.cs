using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StaffReplacementInUnallocatedPeriodFacadeController : Controller
    {

        public StaffReplacementInUnallocatedPeriodInterface _ttcategory;

        public StaffReplacementInUnallocatedPeriodFacadeController(StaffReplacementInUnallocatedPeriodInterface maspag)
        {
            _ttcategory = maspag;
        }

        // GET: api/valuesz
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public TTStaffReplacementInUnallocatedPeriodDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public TTStaffReplacementInUnallocatedPeriodDTO get_catg([FromBody] TTStaffReplacementInUnallocatedPeriodDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getrpt")]
        public TTStaffReplacementInUnallocatedPeriodDTO getrpt([FromBody] TTStaffReplacementInUnallocatedPeriodDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public TTStaffReplacementInUnallocatedPeriodDTO Post([FromBody] TTStaffReplacementInUnallocatedPeriodDTO org)
        {
            return _ttcategory.savedetail(org);
        }

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
