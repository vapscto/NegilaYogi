using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class RoomMappingController : Controller
    {
        RoomMappingDelegate objdelegate = new RoomMappingDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public RoomMappingDTO Get([FromQuery] int id)
        {
            RoomMappingDTO data = new RoomMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("get_catg")]
        public RoomMappingDTO get_catg([FromBody] RoomMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(data);
        }
        [HttpPost]
        [Route("deactiveY")]
        public RoomMappingDTO deactiveY([FromBody] RoomMappingDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.deactiveY(data);
        }

        [HttpPost]
        [Route("getpossiblePeriod")]
        public RoomMappingDTO getpossiblePeriod([FromBody] RoomMappingDTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.getpossiblePeriod(data);
        }

        [Route("getrpt")]
        public RoomMappingDTO getrpt([FromBody] RoomMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return objdelegate.getrpt(data);
        }
        [HttpPost]
        [Route("savedetail")]
        public RoomMappingDTO savedetail([FromBody] RoomMappingDTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return objdelegate.savedetail(data);
        }
        [Route("editdata")]
        public RoomMappingDTO editdata([FromBody] RoomMappingDTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return objdelegate.editdata(data);
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
