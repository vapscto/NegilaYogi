using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgcoursebranchmappingController : Controller
    {
        ClgcoursebranchmappingDelegate objdelegate = new ClgcoursebranchmappingDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("editdeatils/{id:int}")]
        public Exm_Col_CourseBranchDTO editdeatils(int ID)
        {
            return objdelegate.editdeatils(ID);
        }
        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public Exm_Col_CourseBranchDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Exm_Col_CourseBranchDTO obj = objdelegate.getdetails(id);
            return obj;
        }
        [HttpPost]

        [Route("getbranch")]
        public Exm_Col_CourseBranchDTO getbranch([FromBody] Exm_Col_CourseBranchDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getbranch(categorypage);
        }

        [Route("savedetail2")]
        public Exm_Col_CourseBranchDTO savedetail2([FromBody] Exm_Col_CourseBranchDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }
        [HttpPost]
        [Route("get_subjects")]
        public Exm_Col_CourseBranchDTO get_subjects([FromBody] Exm_Col_CourseBranchDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_subjects(categorypage);
        }
        [Route("getalldetailsviewrecords")]
        public Exm_Col_CourseBranchDTO getalldetailsviewrecords([FromBody] Exm_Col_CourseBranchDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords(categorypage);
        }
        [Route("deactivate")]
        public Exm_Col_CourseBranchDTO deactivate([FromBody] Exm_Col_CourseBranchDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);

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
