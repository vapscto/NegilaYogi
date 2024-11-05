using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Baldwin_Subj_G_F_ReportFacadeController : Controller
    {
      public  Baldwin_Subj_G_F_ReportInterface inter_fr;
        public Baldwin_Subj_G_F_ReportFacadeController(Baldwin_Subj_G_F_ReportInterface inter)
        {
            inter_fr = inter;
        }

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

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("Getdetails")]
        public Baldwin_Subj_G_F_ReportDTO Getdetails([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            return inter_fr.Getdetails(data);
        }
        [Route("get_classes")]
        public Baldwin_Subj_G_F_ReportDTO get_classes([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            return inter_fr.get_classes(data);
        }
        [Route("get_sections")]
        public Baldwin_Subj_G_F_ReportDTO get_sections([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            return inter_fr.get_sections(data);
        }
        [Route("get_students")]
        public Baldwin_Subj_G_F_ReportDTO get_students([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            return inter_fr.get_students(data);
        }
        [Route("get_report")]
        public Baldwin_Subj_G_F_ReportDTO get_report([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            return inter_fr.get_report(data);
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
