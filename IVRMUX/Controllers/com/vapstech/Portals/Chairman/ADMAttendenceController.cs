
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ADMAttendenceController : Controller
    {


        ADMAttendenceDelegates crStr = new ADMAttendenceDelegates();


        // GET api/values
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
        [HttpGet]
        [Route("Getdetails")]
        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.MI_Id = 5;
            return crStr.Getdetails(data);            
        }


        [Route("getclass/{id}")]
        public ADMAttendenceDTO getclass(int id)
        {
            ADMAttendenceDTO data = new ADMAttendenceDTO();
          data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            //data.MI_Id = 5;
            return crStr.getclass(data);
        }
        [HttpPost]
        [Route("Getsection")]
        public  ADMAttendenceDTO Getsection([FromBody] ADMAttendenceDTO data)
        {
           // data.MI_Id = 5;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);

        }

        [HttpPost]
        [Route("GetAttendence")]
        public ADMAttendenceDTO GetAttendence([FromBody] ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = 5;
            return crStr.GetAttendence(data);

        }
        [HttpPost]
        [Route("GetIndividualAttendence")]
        public ADMAttendenceDTO GetIndividualAttendence([FromBody] ADMAttendenceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.MI_Id = 5;
            return crStr.GetIndividualAttendence(data);

        }
        
    }

}
