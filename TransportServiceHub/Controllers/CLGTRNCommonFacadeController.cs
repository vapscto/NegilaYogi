using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGTRNCommonFacadeController : Controller
    {
        public CLGTRNCommonInterface _areaint;

        public CLGTRNCommonFacadeController(CLGTRNCommonInterface areaz)
        {
            _areaint = areaz;
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
      

        [Route("get_course")]
        public CLGTRNCommonDTO get_course([FromBody]CLGTRNCommonDTO data)
        {
            return _areaint.get_course(data);
        }
        [Route("getBranch")]
        public CLGTRNCommonDTO getBranch([FromBody]CLGTRNCommonDTO data)
        {
            return _areaint.getBranch(data);
        }

        [Route("get_semister")]
        public CLGTRNCommonDTO get_semister([FromBody]CLGTRNCommonDTO data)
        {
            return _areaint.get_semister(data);
        }

        [Route("get_section")]
        public CLGTRNCommonDTO get_section([FromBody]CLGTRNCommonDTO data)
        {
            return _areaint.get_section(data);
        }
      [Route("get_location")]
        public CLGTRNCommonDTO get_location([FromBody]CLGTRNCommonDTO data)
        {
            return _areaint.get_location(data);
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
