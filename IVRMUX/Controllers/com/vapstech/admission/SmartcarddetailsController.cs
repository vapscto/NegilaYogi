using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SmartcarddetailsController : Controller
    {
        SmartcarddetailsDelegate sad = new SmartcarddetailsDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]

        [Route("getdetails")]
        public Adm_M_StudentDTO getdetails()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return sad.getinitialdata(mi_id);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public Adm_M_StudentDTO getAttendetails([FromBody] Adm_M_StudentDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mi_id;
            return sad.getserdata(data);
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
        [Route("getstudentdetails")]
        public Adm_M_StudentDTO getstudentdetails([FromBody] Adm_M_StudentDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mi_id;
            return sad.getstudentdetails(data);
        }


    }
}
