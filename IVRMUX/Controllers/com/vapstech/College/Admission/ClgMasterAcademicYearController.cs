using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgMasterAcademicYearController : Controller
    {
        ClgMasterAcademicYearDelegate _yeardelg = new ClgMasterAcademicYearDelegate();
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
        public ClgMasterAcademicYearDTO getalldetails(int id)
        {
            ClgMasterAcademicYearDTO data = new ClgMasterAcademicYearDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ACMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ACMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _yeardelg.getalldetails(data);
        }
        [Route("saveaccyear")]
        public ClgMasterAcademicYearDTO saveaccyear([FromBody] ClgMasterAcademicYearDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yeardelg.saveaccyear(data);
        }
        [Route("edit")]
        public ClgMasterAcademicYearDTO edit([FromBody] ClgMasterAcademicYearDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yeardelg.edit(data);
        }
        [Route("deactivate")]
        public ClgMasterAcademicYearDTO deactivate([FromBody] ClgMasterAcademicYearDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yeardelg.deactivate(data);
        }
        [Route("saveorder")]
        public ClgMasterAcademicYearDTO saveorder([FromBody] ClgMasterAcademicYearDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yeardelg.saveorder(data);
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
