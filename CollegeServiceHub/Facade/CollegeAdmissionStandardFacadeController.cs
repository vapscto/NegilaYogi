using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeAdmissionStandardFacadeController : Controller
    {
        public CollegeAdmissionStandardInterface _ads;

        public CollegeAdmissionStandardFacadeController(CollegeAdmissionStandardInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public CollegeAdmissionStandardDTO getclassstudentlist([FromBody]CollegeAdmissionStandardDTO student)
        {
            return _ads.getlisttwo(student);
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        // [Route("loaddata")]
        public CollegeAdmissionStandardDTO getdata(int id)
        {
            return _ads.getlistdata(id);
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
