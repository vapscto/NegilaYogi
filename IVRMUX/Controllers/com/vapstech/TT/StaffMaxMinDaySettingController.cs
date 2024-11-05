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
    public class StaffMaxMinDaySettingController : Controller
    {
        StaffMaxMinDaySettingDelegate _maxmin = new StaffMaxMinDaySettingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public StaffMaxMinDaySettingDTO getdetails(StaffMaxMinDaySettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _maxmin.getdetails(data);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetail")]
        public StaffMaxMinDaySettingDTO savedetail([FromBody] StaffMaxMinDaySettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _maxmin.savedetail(data);
        }

        [Route("getdetail/{id:int}")]
        public StaffMaxMinDaySettingDTO getdetail(int id)
        {
            return _maxmin.getdetail(id);

        }

        [Route("deactive")]
        public StaffMaxMinDaySettingDTO deactive([FromBody] StaffMaxMinDaySettingDTO id)
        {
            return _maxmin.deactive(id);
        }
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
