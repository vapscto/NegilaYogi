using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MarksEntry_SFacadeController : Controller
    {
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public MarksEntry_SInterface int_ssse;

        public MarksEntry_SFacadeController(MarksEntry_SInterface inter)
        {

            int_ssse = inter;
        }


        [HttpPost]
        [Route("Getdetails")]
        public MarksEntry_SDTO Getdetails([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.getdetails(id);
        }
        [Route("get_classes")]
        public MarksEntry_SDTO get_classes([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.get_classes(id);
        }
        [Route("get_sections")]
        public MarksEntry_SDTO get_sections([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.get_sections(id);
        }
        [Route("get_exams")]
        public MarksEntry_SDTO get_exams([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.get_exams(id);
        }
        [Route("get_subjects")]
        public MarksEntry_SDTO get_subjects([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.get_subjects(id);
        }
        [Route("onsearch")]
        public MarksEntry_SDTO onsearch([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.onsearch(id);
        }

        [Route("SaveMarks")]
        public MarksEntry_SDTO SaveMarks([FromBody] MarksEntry_SDTO id)
        {
            return int_ssse.SaveMarks(id);
        }





    }
}
