using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClassTeacherMappingController : Controller
    {
        ClassTeacherMappingDelegate _classteacher = new ClassTeacherMappingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public ClassTeacherMappingDTO getdetails (int id)
        {
            //ClassTeacherMappingDTO data = new ClassTeacherMappingDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _classteacher.getdetails(id);
        }
        [Route("save")]
        public ClassTeacherMappingDTO save ([FromBody]ClassTeacherMappingDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _classteacher.save(data);
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public ClassTeacherMappingDTO GetSelectedRowDetails(int id)
        {
            ClassTeacherMappingDTO data = new ClassTeacherMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMCT_Id = id;
            return _classteacher.GetSelectedRowDetails(data);
        }
       
        [Route("onchangestaff1")]
        public ClassTeacherMappingDTO onchangestaff1 ([FromBody] ClassTeacherMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _classteacher.onchangestaff1(data);
        }

        [Route("onchangestaff2")]
        public ClassTeacherMappingDTO onchangestaff2([FromBody] ClassTeacherMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _classteacher.onchangestaff2(data);
        }
        [Route("exchangesave")]
        public ClassTeacherMappingDTO exchangesave([FromBody] ClassTeacherMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _classteacher.exchangesave(data);
        }

        [Route("deleterecord")]
        public ClassTeacherMappingDTO deleterecord([FromBody]ClassTeacherMappingDTO data)
        {
          //  ClassTeacherMappingDTO data = new ClassTeacherMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _classteacher.deleterecord(data);
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
    }
}
