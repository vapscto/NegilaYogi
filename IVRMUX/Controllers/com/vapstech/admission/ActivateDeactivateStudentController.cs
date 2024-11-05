using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ActivateDeactivateStudentController : Controller
    {
        ActivateDeactivateStudentDelegate adsd = new ActivateDeactivateStudentDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public ActivateDeactivateStudentDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }

        [Route("getACS")]
        public ActivateDeactivateStudentDTO getacademicclasssectionstudentlist([FromBody]ActivateDeactivateStudentDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlistone(id);
        }

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public ActivateDeactivateStudentDTO getclassstudentlist([FromBody]ActivateDeactivateStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlisttwo(student);
        }

        [HttpPost]
        [Route("getS")]
        public ActivateDeactivateStudentDTO getclasssectionstudentlist([FromBody]ActivateDeactivateStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getlistthree(student);
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
