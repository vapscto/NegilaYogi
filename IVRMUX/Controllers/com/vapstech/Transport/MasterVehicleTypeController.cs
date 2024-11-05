using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;
namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class MasterVehicleTypeController : Controller
    {
        MasterVehicleTypeDelegate _area = new MasterVehicleTypeDelegate();

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
        [Route("getdata/{id:int}")]
        public MasterVehicleTypeDTO getdata(int id)
        {

            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getdata(id);
        }

        [Route("savedata")]
        public MasterVehicleTypeDTO savedata([FromBody] MasterVehicleTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.savedata(data);
        }

        [Route("geteditdata")]
        public MasterVehicleTypeDTO geteditdata([FromBody]MasterVehicleTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.geteditdata(data);
        }

        [Route("activedeactive")]
        public MasterVehicleTypeDTO activedeactive([FromBody] MasterVehicleTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.activedeactive(data);
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
