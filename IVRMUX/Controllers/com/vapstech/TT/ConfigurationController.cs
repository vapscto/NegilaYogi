using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [Route("api/[controller]")]
    public class ConfigurationController : Controller
    {

        ConfigurationDelegate TCD = new ConfigurationDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("getalldetails")]
        public TTConfigurationDTO Get([FromQuery] int id)
        {
            TTConfigurationDTO data = new TTConfigurationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TCD.getdetails(data);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [HttpPost]
        [Route("savedetail")]
        public TTConfigurationDTO savedetail([FromBody] TTConfigurationDTO page1)
        {
            page1.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TCD.savedetail(page1);
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