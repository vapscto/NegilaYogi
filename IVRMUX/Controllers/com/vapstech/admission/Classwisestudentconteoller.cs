using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Classwisestudentconteoller : Controller
    {
       
        ClasswisestudentdetailsDelegate cwsd = new ClasswisestudentdetailsDelegate();
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

        [Route("getalldetails/{id:int}")]
        
        public ClasswisestudentdetailsDTO getalldetails(int id)
        {
            ClasswisestudentdetailsDTO dto = new ClasswisestudentdetailsDTO();
            dto.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return cwsd.getdetails(dto);

        }


        [HttpPost]
        [Route("Getreportdetails")]
        public ClasswisestudentdetailsDTO Getreportdetails ([FromBody] ClasswisestudentdetailsDTO data)
        {
            data.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
         
            return cwsd.Getreportdetails(data);
        }
        [Route("getsection")]
        public ClasswisestudentdetailsDTO getsection([FromBody] ClasswisestudentdetailsDTO data)
        {
            data.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            
            return cwsd.getsection(data);
        }
        [Route("fetchclassbyYearId")]
        public ClasswisestudentdetailsDTO fetchclassbyYearId([FromBody] ClasswisestudentdetailsDTO data)
        {
            data.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return cwsd.fetchclassbyYearId(data);
        }
        

        // POST api/values

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
