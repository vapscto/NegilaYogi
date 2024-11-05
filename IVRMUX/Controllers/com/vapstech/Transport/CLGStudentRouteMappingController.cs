using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class CLGStudentRouteMappingController : Controller
    {
        CLGStudentRouteMappingDelegate _area = new CLGStudentRouteMappingDelegate();

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
        [Route("getdata/{id:int}")]
        public CLGStudentRouteMappingDTO getdata(int id)
        {
            CLGStudentRouteMappingDTO data = new CLGStudentRouteMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getdata(data);
        }

        [Route("savedata")]
        public CLGStudentRouteMappingDTO savedata([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _area.savedata(data);
        }

        [Route("geteditdata")]
        public CLGStudentRouteMappingDTO geteditdata([FromBody]CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.geteditdata(data);
        }

        [Route("getstudents")]
        public CLGStudentRouteMappingDTO activedeactive([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getstudents(data);
        }
        [Route("checkduplicateno")]
        public CLGStudentRouteMappingDTO checkduplicateno([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.checkduplicateno(data);
        }
        [Route("viewrecordspopup")]
        public CLGStudentRouteMappingDTO viewrecordspopup([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.viewrecordspopup(data);
        }
        [Route("getreportedit")]
        public CLGStudentRouteMappingDTO getreportedit([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getreportedit(data);
        }
        [Route("getreporteditbuspass")]
        public CLGStudentRouteMappingDTO getreporteditbuspass([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getreporteditbuspass(data);
        }
        [Route("savedatabuspass")]
        public CLGStudentRouteMappingDTO savedatabuspass([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.savedatabuspass(data);
        }



        [Route("deactivate")]
        public CLGStudentRouteMappingDTO deactivate([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.deactivate(data);
        }
        [Route("SearchByColumn")]
        public CLGStudentRouteMappingDTO SearchByColumn([FromBody] CLGStudentRouteMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.SearchByColumn(data);
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
    }
}
