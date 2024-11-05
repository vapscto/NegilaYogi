
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PrincipalDefaulterFeeController : Controller
    {
        PrincipalDefaulterFeeDelegate crStr = new PrincipalDefaulterFeeDelegate();


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
        public PrincipalDefaulterFeeDTO Getdetails(PrincipalDefaulterFeeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getdetails(data);
        }


        [Route("getclass/{id}")]
        public PrincipalDefaulterFeeDTO getclass(int id)
        {
            PrincipalDefaulterFeeDTO data = new PrincipalDefaulterFeeDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return crStr.getclass(data);
        }

        [HttpPost]
        [Route("Getsection")]
        public PrincipalDefaulterFeeDTO Getsection([FromBody] PrincipalDefaulterFeeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);

        }

        [HttpPost]
        [Route("Getreport")]
        public PrincipalDefaulterFeeDTO Getreport([FromBody] PrincipalDefaulterFeeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getreport(data);

        }
        [HttpPost]
        [Route("Getstudentdetails")]
        public PrincipalDefaulterFeeDTO Getstudentdetails([FromBody] PrincipalDefaulterFeeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getstudentdetails(data);

        }

    }

}

