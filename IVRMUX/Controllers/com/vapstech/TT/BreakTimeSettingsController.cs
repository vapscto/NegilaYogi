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
    public class BreakTimeSettingsController : Controller
    {
        BreaktimesettingDelegate objdelegate = new BreaktimesettingDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTBreakTimesettingDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTBreakTimesettingDTO savedetail([FromBody] TTBreakTimesettingDTO categorypage)
        {           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
        }
        //for maximum periods count get
        [Route("getmaximumperiodscount")]
        public TTBreakTimesettingDTO getmaximumperiodscount([FromBody] TTBreakTimesettingDTO categorypage)
        {          
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getmaximumperiodscount(categorypage);
        }
        //for get class
        [Route("getclass_catg")]
        public TTBreakTimesettingDTO getclass_catg([FromBody] TTBreakTimesettingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(categorypage);
        }
        [Route("get_catg")]
        public TTBreakTimesettingDTO get_catg([FromBody] TTBreakTimesettingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }

        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TTBreakTimesettingDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [Route("getdetails/{id:int}")]
        public TTBreakTimesettingDTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);

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
        [HttpPost]
        [Route("deactivate")]
        public TTBreakTimesettingDTO deactvate([FromBody] TTBreakTimesettingDTO id)
        {
            return objdelegate.deactivate(id);
        }


    }
}
