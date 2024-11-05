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
    public class ClgcoursebranchmappingFacadeController : Controller
    {
        public ClgcoursebranchmappingInterface _inte;

        public ClgcoursebranchmappingFacadeController(ClgcoursebranchmappingInterface data)
        {
            _inte = data;
        }
        [Route("editdeatils/{id:int}")]
        public Exm_Col_CourseBranchDTO editdeatils(int ID)
        {
            return _inte.editdeatils(ID);
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public Exm_Col_CourseBranchDTO getorgdet(int id)
        {
            return _inte.getdetails(id);
        }
        [HttpPost]


        [Route("getbranch")]
        public Exm_Col_CourseBranchDTO getbranch([FromBody] Exm_Col_CourseBranchDTO org)
        {
            return _inte.getbranch(org);
        }
        [Route("savedetail2")]
        public Exm_Col_CourseBranchDTO Post2([FromBody] Exm_Col_CourseBranchDTO org)
        {
            return _inte.savedetail2(org);
        }
        [HttpPost]
        [Route("get_subjects")]
        public Exm_Col_CourseBranchDTO Post3([FromBody] Exm_Col_CourseBranchDTO org)
        {
            return _inte.get_subjects(org);
        }
        [Route("getalldetailsviewrecords")]
        public Exm_Col_CourseBranchDTO getalldetailsviewrecords([FromBody] Exm_Col_CourseBranchDTO org)
        {
            return _inte.getalldetailsviewrecords(org);
        }
        [Route("deactivate")]
        public Exm_Col_CourseBranchDTO deactivate([FromBody] Exm_Col_CourseBranchDTO org)
        {
            return _inte.deactivate(org);
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
