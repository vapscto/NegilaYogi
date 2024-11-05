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
    public class MarksEntry_SSFacadeController : Controller
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

        public MarksEntry_SSInterface int_ssse;

        public MarksEntry_SSFacadeController(MarksEntry_SSInterface inter)
        {

            int_ssse = inter;
        }


        [HttpPost]
        [Route("Getdetails")]
        public MarksEntry_SSDTO Getdetails([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.getdetails(id);
        }
        [Route("get_classes")]
        public MarksEntry_SSDTO get_classes([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.get_classes(id);
        }
        [Route("get_sections")]
        public MarksEntry_SSDTO get_sections([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.get_sections(id);
        }
        [Route("get_exams")]
        public MarksEntry_SSDTO get_exams([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.get_exams(id);
        }
        [Route("get_subjects")]
        public MarksEntry_SSDTO get_subjects([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.get_subjects(id);
        }
        [Route("onsearch")]
        public MarksEntry_SSDTO onsearch([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.onsearch(id);
        }

        [Route("SaveMarks")]
        public MarksEntry_SSDTO SaveMarks([FromBody] MarksEntry_SSDTO id)
        {
            return int_ssse.SaveMarks(id);
        }





    }
}
