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
    public class DriverEmployeeMappingController : Controller
    {
        DriverEmployeeMappingDelegate _driveremp = new DriverEmployeeMappingDelegate();
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

        // [HttpPost]
        [Route("getdata/{id:int}")]
        public DriverEmployeeMappingDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.getdata(id);
        }

        [Route("savedata")]
        public DriverEmployeeMappingDTO savedata([FromBody] DriverEmployeeMappingDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.savedata(id);
        }


        [Route("edit")]
        public DriverEmployeeMappingDTO edit([FromBody] DriverEmployeeMappingDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.edit(id);
        }

        [Route("deletedata")]
        public DriverEmployeeMappingDTO deletedata([FromBody] DriverEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.deletedata(data);
        }

    }
}
