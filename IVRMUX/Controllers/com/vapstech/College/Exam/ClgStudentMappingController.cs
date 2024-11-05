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
    public class ClgStudentMappingController : Controller
    {
        ClgStudentMappingDelegate objdelegate = new ClgStudentMappingDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public Exm_Col_Studentwise_SubjectsDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Exm_Col_Studentwise_SubjectsDTO obj = objdelegate.getdetails(id);
            return obj;
        }

     
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("Studentdetails")]
        public Exm_Col_Studentwise_SubjectsDTO Studentdetails([FromBody]Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.Studentdetails(data);
        }
        [Route("savedetails")]
        public Exm_Col_Studentwise_SubjectsDTO savedetails([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetails(data);
        }
        [Route("getcourse")]
        public Exm_Col_Studentwise_SubjectsDTO getcourse([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcourse(data);
        }
        [Route("getbranch")]
        public Exm_Col_Studentwise_SubjectsDTO getbranch([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getbranch(data);
        }
        [Route("getsemester")]
        public Exm_Col_Studentwise_SubjectsDTO getsemester([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getsemester(data);
        }
        [Route("getsection")]
        public Exm_Col_Studentwise_SubjectsDTO getsection([FromBody] Exm_Col_Studentwise_SubjectsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getsection(data);
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
