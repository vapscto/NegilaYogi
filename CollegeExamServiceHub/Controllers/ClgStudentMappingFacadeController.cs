using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgStudentMappingFacadeController : Controller
    {
        public ClgStudentMappingInterface _inte;

        public ClgStudentMappingFacadeController(ClgStudentMappingInterface data)
        {
            _inte = data;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public Exm_Col_Studentwise_SubjectsDTO getorgdet(int id)
        {
            return _inte.getdetails(id);
        }

      
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("Studentdetails")]
        public Exm_Col_Studentwise_SubjectsDTO Studentdetails([FromBody]Exm_Col_Studentwise_SubjectsDTO data)
        {
            return _inte.Studentdetails(data);
        }
        [Route("savedetails")]
        public Exm_Col_Studentwise_SubjectsDTO savedetails([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            return _inte.savedetails(data);
        }
        [Route("getcourse")]
        public Exm_Col_Studentwise_SubjectsDTO getcourse([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {            
            return _inte.getcourse(data);
        }
        [Route("getbranch")]
        public Exm_Col_Studentwise_SubjectsDTO getbranch([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {            
            return _inte.getbranch(data);
        }
        [Route("getsemester")]
        public Exm_Col_Studentwise_SubjectsDTO getsemester([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {            
            return _inte.getsemester(data);
        }
        [Route("getsection")]
        public Exm_Col_Studentwise_SubjectsDTO getsection([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {            
            return _inte.getsection(data);
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
