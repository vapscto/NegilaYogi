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
    public class CLGStaffPeriodTransformController : Controller
    {
        CLGStaffPeriodTransformDelegate objdelegate = new CLGStaffPeriodTransformDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public CLGStaffPeriodTransformDTO Get([FromQuery] int id)
        {
            CLGStaffPeriodTransformDTO data = new CLGStaffPeriodTransformDTO();
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
        public CLGStaffPeriodTransformDTO get_catg([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [Route("getrpt")]
        public CLGStaffPeriodTransformDTO getrpt([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(categorypage);
        }

        [Route("gettimetable")]
        public CLGStaffPeriodTransformDTO gettimetable([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.gettimetable(categorypage);
        }
        [HttpPost]
        [Route("getpossiblePeriod")]
        public CLGStaffPeriodTransformDTO getpossiblePeriod([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
         
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpossiblePeriod(categorypage);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGStaffPeriodTransformDTO savedetail([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
         
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
        }

        [HttpPost]
        [Route("deleteperiod")]
        public CLGStaffPeriodTransformDTO deleteperiod([FromBody] CLGStaffPeriodTransformDTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deleteperiod(categorypage);
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
