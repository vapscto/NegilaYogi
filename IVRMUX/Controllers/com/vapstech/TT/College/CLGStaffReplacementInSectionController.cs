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
    public class CLGStaffReplacementInSectionController : Controller
    {
        CLGStaffReplacementInSectionDelegate objdelegate = new CLGStaffReplacementInSectionDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public CLGStaffReplacementInSectionDTO Get([FromQuery] int id)
        {
            CLGStaffReplacementInSectionDTO data = new CLGStaffReplacementInSectionDTO();
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
        public CLGStaffReplacementInSectionDTO get_catg([FromBody] CLGStaffReplacementInSectionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [HttpPost]
        [Route("getclass_catg")]
        public CLGStaffReplacementInSectionDTO getclass_catg([FromBody] CLGStaffReplacementInSectionDTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(categorypage);
        }

        [HttpPost]
        [Route("getpossiblePeriod")]
        public CLGStaffReplacementInSectionDTO getpossiblePeriod([FromBody] CLGStaffReplacementInSectionDTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpossiblePeriod(categorypage);
        }

        [Route("getrpt")]
        public CLGStaffReplacementInSectionDTO getrpt([FromBody] CLGStaffReplacementInSectionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return objdelegate.getrpt(categorypage);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGStaffReplacementInSectionDTO savedetail([FromBody] CLGStaffReplacementInSectionDTO categorypage)
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
