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
    public class StaffReplacementUnalocatedPeriodFacadeController : Controller
    {

        public StaffReplacementUnalocatedPeriodInterface _ttcategory;

        public StaffReplacementUnalocatedPeriodFacadeController(StaffReplacementUnalocatedPeriodInterface maspag)
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
        public StaffReplacementUnalocatedPeriodDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public StaffReplacementUnalocatedPeriodDTO get_catg([FromBody] StaffReplacementUnalocatedPeriodDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getrpt")]
        public Task<StaffReplacementUnalocatedPeriodDTO> getrpt([FromBody] StaffReplacementUnalocatedPeriodDTO org)
        {
            return _ttcategory.getrpt(org);
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
