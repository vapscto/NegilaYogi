using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.BirthDay;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FrontOfficeController : Controller
    {
        FrontOfficeDelegate dele = new FrontOfficeDelegate();
        // GET: api/values
        [HttpGet("{id:int}")]
        public BirthDayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdata(id);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getmonthreport(data);
        }

    }
}
