using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class VehicalDriverSubstituteController : Controller
    {
        VehicalDriverSubstituteDelegate _driveremp = new VehicalDriverSubstituteDelegate();
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

        //[HttpPost]
        [Route("getdata/{id:int}")]
        public VehicalDriverSubstituteDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.getdata(id);
        }
        [Route("get_driver_data/{id:int}")]
        public VehicalDriverSubstituteDTO get_driver_data(int id)
        {
            VehicalDriverSubstituteDTO data = new VehicalDriverSubstituteDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TRMV_Id = id;
            return _driveremp.get_driver_data(data);
        }
        [Route("savedata")]
        public VehicalDriverSubstituteDTO savedata([FromBody] VehicalDriverSubstituteDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.savedata(id);
        }
        

        [Route("editdata")]
        public VehicalDriverSubstituteDTO editdata([FromBody]VehicalDriverSubstituteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.editdata(data);
        }
    }
}
