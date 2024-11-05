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
    public class StaffPeriodTransformFacadeController : Controller
    {

        public StaffPeriodTransformInterface _ttcategory;

        public StaffPeriodTransformFacadeController(StaffPeriodTransformInterface maspag)
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
        public StaffPeriodTransformDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public StaffPeriodTransformDTO get_catg([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getrpt")]
        public StaffPeriodTransformDTO getrpt([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [Route("gettimetable")]
        public StaffPeriodTransformDTO gettimetable([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.gettimetable(org);
        }
        [Route("getpossiblePeriod")]
        public StaffPeriodTransformDTO getpossiblePeriod([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public StaffPeriodTransformDTO Post([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [HttpPost]
        [Route("deleteperiod")]
        public StaffPeriodTransformDTO deleteperiod([FromBody] StaffPeriodTransformDTO org)
        {
            return _ttcategory.deleteperiod(org);
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
