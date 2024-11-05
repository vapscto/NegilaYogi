using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Transport;
using corewebapi18072016.Delegates.com.vapstech.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StdRouteLocationMapController : Controller
    {
        StdRouteLocationMapDelegate rtfd = new StdRouteLocationMapDelegate();
        // GET: api/values
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


        [HttpPost]
        [Route("savedata")]
        public StdRouteLocationMapDTO savedata([FromBody]StdRouteLocationMapDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.savedata(student);
        }

        [Route("get_loca_sches1")]
        public StdRouteLocationMapDTO check_feegroup([FromBody] StdRouteLocationMapDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return rtfd.check_feegroup(data);
        }
        [Route("deactivate")]
        public StdRouteLocationMapDTO deactivate([FromBody] StdRouteLocationMapDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            student.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return rtfd.deactivate(student);
        }
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public StdRouteLocationMapDTO getdata( int id)
        {
            StdRouteLocationMapDTO data = new StdRouteLocationMapDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

          
            return rtfd.getdata(data);
        }

        [HttpPost]
        [Route("get_cls_secs")]
        public StdRouteLocationMapDTO get_cls_secs([FromBody] StdRouteLocationMapDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return rtfd.get_cls_secs(data123);
        }

        [HttpPost]
        [Route("get_sections")]
        public StdRouteLocationMapDTO get_sections([FromBody] StdRouteLocationMapDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.ASMAY_Id = ASMAY_Id;

            return rtfd.get_sections(value);
        }


        [HttpPost]
        [Route("Getreportdetails")]
        public StdRouteLocationMapDTO Getreportdetails([FromBody] StdRouteLocationMapDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.Getreportdetails(data);
        }
        

        [HttpPost]
        [Route("on_pic_route_change")]
        public StdRouteLocationMapDTO on_pic_route_change([FromBody] StdRouteLocationMapDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.on_pic_route_change(data);
        }



        [HttpPost]
        [Route("getreport")]
        public StdRouteLocationMapDTO getreport([FromBody] StdRouteLocationMapDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.getreport(data);
        }
        [HttpPost]
        [Route("getreportedit")]
        public StdRouteLocationMapDTO getreportedit([FromBody] StdRouteLocationMapDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.getreportedit(data);
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

        [HttpPost]
        [Route("get_data")]
        public StdRouteLocationMapDTO get_data([FromBody]StdRouteLocationMapDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.get_data(student);
        }
    }
}
