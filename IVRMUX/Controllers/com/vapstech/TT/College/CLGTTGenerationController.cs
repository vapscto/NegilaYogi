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
    public class CLGTTGenerationController : Controller
    {
        CLGTTGenerationDelegate objdelegate = new CLGTTGenerationDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public CLGTTGenerationDTO Get([FromQuery] CLGTTGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getdetails(data);
        }


        // POST api/values
        [HttpPost]
        [Route("generate")]
        public CLGTTGenerationDTO generate([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.generate(categorypage);
        }
        [Route("get_catg")]
        public CLGTTGenerationDTO get_catg([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [Route("get_count")]
        public CLGTTGenerationDTO get_count([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_count(categorypage);
        }
        [Route("resetTT")]
        public CLGTTGenerationDTO resetTT([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.resetTT(categorypage);
        }
        [Route("Get_temp_data")]
        public CLGTTGenerationDTO Get_temp_data([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.Get_temp_data(categorypage);
        }
        [HttpPost]
        [Route("getalldetailsviewrecords")]
        public CLGTTGenerationDTO getalldetailsviewrecords([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords(categorypage);
        }

        [Route("getreplacemntdetailsviewrecords")]
        public CLGTTGenerationDTO getreplacemntdetailsviewrecords([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getreplacemntdetailsviewrecords(categorypage);
        }

        [Route("saveTemptomain")]
        public CLGTTGenerationDTO saveTemptomain([FromBody] CLGTTGenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.saveTemptomain(categorypage);
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
        public CLGTTGenerationDTO deactvate([FromBody] CLGTTGenerationDTO id)
        {
            return objdelegate.deactivate(id);
        }

    }
}
