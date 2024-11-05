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
    public class CLGStaffRplInTheirTTController : Controller
    {
        CLGStaffRplInTheirTTDelegate objdelegate = new CLGStaffRplInTheirTTDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public CLGStaffRplInTheirTTDTO Get([FromQuery] int id)
        {
            CLGStaffRplInTheirTTDTO data = new CLGStaffRplInTheirTTDTO();
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
        public CLGStaffRplInTheirTTDTO get_catg([FromBody] CLGStaffRplInTheirTTDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [Route("getrpt")]
        public CLGStaffRplInTheirTTDTO getrpt([FromBody] CLGStaffRplInTheirTTDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(categorypage);
        }
        [HttpPost]
        [Route("getpossiblePeriod")]
        public CLGStaffRplInTheirTTDTO getpossiblePeriod([FromBody] CLGStaffRplInTheirTTDTO categorypage)
        {
          
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpossiblePeriod(categorypage);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGStaffRplInTheirTTDTO savedetail([FromBody] CLGStaffRplInTheirTTDTO categorypage)
        {
          
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
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
