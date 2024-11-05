using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates;
using corewebapi18072016.Delegates.com.vapstech.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HHSStudyCertificateController : Controller
    {
        HHSStudyCertificateDelegate adsd = new HHSStudyCertificateDelegate();
        // GET: api/values

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        [Route("getdata")]
        public HHSStudyCertificateDTO getinitialdata([FromBody]HHSStudyCertificateDTO id)
        {
            id.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }
        
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
