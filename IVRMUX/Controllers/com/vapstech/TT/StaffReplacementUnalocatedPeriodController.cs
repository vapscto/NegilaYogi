﻿using System;
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
    public class StaffReplacementUnalocatedPeriodController : Controller
    {
        StaffReplacementUnalocatedPeriodDelegate objdelegate = new StaffReplacementUnalocatedPeriodDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public StaffReplacementUnalocatedPeriodDTO Get([FromQuery] int id)
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
        [Route("get_catg")]
        public StaffReplacementUnalocatedPeriodDTO get_catg([FromBody] StaffReplacementUnalocatedPeriodDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [Route("getrpt")]
        public StaffReplacementUnalocatedPeriodDTO getrpt([FromBody] StaffReplacementUnalocatedPeriodDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getrpt(categorypage);
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
