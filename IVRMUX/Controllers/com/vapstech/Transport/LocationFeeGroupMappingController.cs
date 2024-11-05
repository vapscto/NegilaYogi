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
    public class LocationFeeGroupMappingController : Controller
    {
        LocationFeeGroupMappingDelegate _area = new LocationFeeGroupMappingDelegate();

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
        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {
           
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getdata(id);
        }

        [Route("savedata")]
        public TR_Location_FeeGroup_MappingDTO savedata([FromBody] TR_Location_FeeGroup_MappingDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.savedata(data);
        }

        [Route("geteditdata")]
        public TR_Location_FeeGroup_MappingDTO geteditdata([FromBody]TR_Location_FeeGroup_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.geteditdata(data);
        }

        [Route("activedeactive")]
        public TR_Location_FeeGroup_MappingDTO activedeactive([FromBody] TR_Location_FeeGroup_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.activedeactive(data);
        }


        [Route("savedataamount")]
        public TR_Location_AmountDTO savedataamount([FromBody] TR_Location_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return _area.savedataamount(data);
        }

        [Route("geteditdataamount")]
        public TR_Location_AmountDTO geteditdataamount([FromBody]TR_Location_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return _area.geteditdataamount(data);
        }

        [Route("activedeactiveamount")]
        public TR_Location_AmountDTO activedeactiveamount([FromBody] TR_Location_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return _area.activedeactiveamount(data);
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
