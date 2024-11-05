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
    public class TimeTableGenerationController : Controller
    {
        TimeTableGenerationDelegate objdelegate = new TimeTableGenerationDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public TT_Final_GenerationDTO Get([FromQuery] TT_Final_GenerationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getdetails(data);
        }


        // POST api/values
        [HttpPost]
        [Route("generate")]
        public TT_Final_GenerationDTO generate([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.generate(categorypage);
        }
        [Route("get_catg")]
        public TT_Final_GenerationDTO get_catg([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        [Route("get_count")]
        public TT_Final_GenerationDTO get_count([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_count(categorypage);
        }
        [Route("resetTT")]
        public TT_Final_GenerationDTO resetTT([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.resetTT(categorypage);
        }
        [Route("Get_temp_data")]
        public TT_Final_GenerationDTO Get_temp_data([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.Get_temp_data(categorypage);
        }
        [HttpPost]
        [Route("getalldetailsviewrecords")]
        public TT_Final_GenerationDTO getalldetailsviewrecords([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords(categorypage);
        }
        [Route("getreplacemntdetailsviewrecords")]
        public TT_Final_GenerationDTO getreplacemntdetailsviewrecords([FromBody] TT_Final_GenerationDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getreplacemntdetailsviewrecords(categorypage);
        }

        [Route("saveTemptomain")]
        public TT_Final_GenerationDTO saveTemptomain([FromBody] TT_Final_GenerationDTO categorypage)
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
        public TT_Final_GenerationDTO deactvate([FromBody] TT_Final_GenerationDTO id)
        {
            return objdelegate.deactivate(id);
        }

    }
}
