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
    public class CLGStaffPeriodTransformFacadeController : Controller
    {

        public CLGStaffPeriodTransformInterface _ttcategory;

        public CLGStaffPeriodTransformFacadeController(CLGStaffPeriodTransformInterface maspag)
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
        [Route("getdetails")]
        public CLGStaffPeriodTransformDTO getorgdet([FromBody] CLGStaffPeriodTransformDTO data)
        {
            return _ttcategory.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public CLGStaffPeriodTransformDTO get_catg([FromBody] CLGStaffPeriodTransformDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getrpt")]
        public CLGStaffPeriodTransformDTO getrpt([FromBody] CLGStaffPeriodTransformDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [Route("gettimetable")]
        public CLGStaffPeriodTransformDTO gettimetable([FromBody] CLGStaffPeriodTransformDTO org)
        {
            return _ttcategory.gettimetable(org);
        }
        [Route("getpossiblePeriod")]
        public CLGStaffPeriodTransformDTO getpossiblePeriod([FromBody] CLGStaffPeriodTransformDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGStaffPeriodTransformDTO Post([FromBody] CLGStaffPeriodTransformDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [HttpPost]
        [Route("deleteperiod")]
        public CLGStaffPeriodTransformDTO deleteperiod([FromBody] CLGStaffPeriodTransformDTO org)
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
