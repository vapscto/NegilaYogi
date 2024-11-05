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
    public class StudentAchievementReportFacade : Controller
    {
        public StudentAchievementReportInterface _ads;

        public StudentAchievementReportFacade(StudentAchievementReportInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        //[HttpGet]
        [Route("getdata")]
        public Adm_M_StudentDTO getinitialdata([FromBody]Adm_M_StudentDTO stud)
        {
            return _ads.getdetails(stud);
        }

        [Route("stdnamesyear")]
        public Task<Adm_M_StudentDTO> stdnamesyear([FromBody]Adm_M_StudentDTO stud)
        {
            return _ads.stdnamesyear(stud);
        }


        //[HttpGet]
        //[Route("getStudData")]
        //public Task<Adm_M_StudentDTO> getStudData([FromBody] Adm_M_StudentDTO studData)
        //{
        //    return _ads.getStudDetails(studData);
        //}

        [HttpPost]
        [Route("getStudData")]
        public Task<Adm_M_StudentDTO> Post([FromBody] Adm_M_StudentDTO studData)
        {
            return _ads.getStudDetails(studData);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
