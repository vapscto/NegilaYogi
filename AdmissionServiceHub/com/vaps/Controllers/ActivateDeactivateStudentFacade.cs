using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]
    public class ActivateDeactivateStudentFacade : Controller
    {
        public ActivateDeactivateStudentInterface _ads;

        public ActivateDeactivateStudentFacade(ActivateDeactivateStudentInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public ActivateDeactivateStudentDTO getinitialdata(int id)
        {
            return _ads.getdetails(id);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

     
      
        [Route("getACS")]
        public ActivateDeactivateStudentDTO getacademicclasssectionstudentlist([FromBody] ActivateDeactivateStudentDTO id)
        {
            return _ads.getlistone(id);
        }

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public Task<ActivateDeactivateStudentDTO> getclassstudentlist([FromBody]ActivateDeactivateStudentDTO student)
        {
            return _ads.getlisttwo(student);
        }

       
        [Route("getS")]
        public ActivateDeactivateStudentDTO getclasssectionstudentlist([FromBody]ActivateDeactivateStudentDTO student)
        {
            return _ads.getlistthree(student);
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
