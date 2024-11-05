using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamPromotionRemarksFacadeController : Controller
    {
        ExamPromotionRemarksInterface _intf;
        public ExamPromotionRemarksFacadeController(ExamPromotionRemarksInterface intf)
        {
            _intf = intf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails")]
        public ExamPromotionRemarksDTO Getdetails([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.Getdetails(data);
        }
        [Route("get_class")]
        public ExamPromotionRemarksDTO get_class([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.get_class(data);
        }
        [Route("get_section")]
        public ExamPromotionRemarksDTO get_section([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.get_section(data);
        }
        [Route("get_group")]
        public ExamPromotionRemarksDTO get_group([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.get_group(data);
        }
        [Route("get_exam")]
        public ExamPromotionRemarksDTO get_exam([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.get_exam(data);
        }
        [Route("search_student")]
        public ExamPromotionRemarksDTO search_student([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.search_student(data);
        }
        [Route("search_groupwise_student")]
        public ExamPromotionRemarksDTO search_groupwise_student([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.search_groupwise_student(data);
        }
        [Route("save_details")]
        public ExamPromotionRemarksDTO save_details([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.save_details(data);
        }
        [Route("save_groupwise_details")]
        public ExamPromotionRemarksDTO save_groupwise_details([FromBody] ExamPromotionRemarksDTO data)
        {
            return _intf.save_groupwise_details(data);
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
