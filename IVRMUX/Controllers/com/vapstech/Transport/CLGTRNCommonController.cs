using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class CLGTRNCommonController : Controller
    {
        CLGTRNCommonDelegate _area = new CLGTRNCommonDelegate();

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
        public CLGTRNCommonDTO get_course([FromBody] CLGTRNCommonDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.get_course(data);
        }
        [Route("getBranch")]
        public CLGTRNCommonDTO getBranch([FromBody] CLGTRNCommonDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getBranch(data);
        }
         [Route("get_semister")]
        public CLGTRNCommonDTO get_semister([FromBody] CLGTRNCommonDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.get_semister(data);
        }
         [Route("get_section")]
        public CLGTRNCommonDTO get_section([FromBody] CLGTRNCommonDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.get_section(data);
        }
        [Route("get_location")]
        public CLGTRNCommonDTO get_location([FromBody] CLGTRNCommonDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.get_location(data);
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
