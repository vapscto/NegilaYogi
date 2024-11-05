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
    public class TTConsolidatedFacadeController : Controller
    {
        public TTConsolidatedInterface _ttcategory;
        public TTConsolidatedFacadeController(TTConsolidatedInterface maspag)
        {
            _ttcategory = maspag;
        }

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
        public TTConsolidatedDTO getalldetails(int id)
        {
            return _ttcategory.getalldetails(id);
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
       
        [Route("getrpt")]
        public TTConsolidatedDTO getrpt([FromBody] TTConsolidatedDTO org)
        {
            return _ttcategory.getreport(org);
        }

        [Route("getclass_catg")]
        public TTConsolidatedDTO getclass_catg([FromBody] TTConsolidatedDTO org)
        {
            return _ttcategory.getclass_catg(org);
        }

    }
}
