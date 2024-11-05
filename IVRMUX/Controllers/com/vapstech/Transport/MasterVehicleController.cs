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
    public class MasterVehicleController : Controller
    {
        MasterVehicleDelegate _vehicle = new MasterVehicleDelegate();
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
        public MasterVehicleDTO getdata (int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _vehicle.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public MasterVehicleDTO savedata([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.savedata(id);
        }
        [Route("edit")]
        public MasterVehicleDTO edit([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.edit(id);
        }

        [Route("activedeactive")]
        public MasterVehicleDTO activedeactive([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.activedeactive(id);
        }

        [Route("validaevehicleno")]
        public MasterVehicleDTO validaevehicleno([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.validaevehicleno(id);
        }
           [Route("rcreport")]
        public MasterVehicleDTO rcreport([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.rcreport(id);
        }

        [Route("validaevhassiseno")]
        public MasterVehicleDTO validaevhassiseno([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.validaevhassiseno(id);
        }

        [Route("validaeengineno")]
        public MasterVehicleDTO validaeengineno([FromBody] MasterVehicleDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.validaeengineno(id);
        }


        [Route("viewuploadflies")]
        public MasterVehicleDTO viewuploadflies([FromBody]MasterVehicleDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public MasterVehicleDTO deleteuploadfile([FromBody]MasterVehicleDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _vehicle.deleteuploadfile(data);
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
