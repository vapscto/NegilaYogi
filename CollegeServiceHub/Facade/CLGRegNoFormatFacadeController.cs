using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLGRegNoFormatFacadeController : Controller
    {
        public CLGRegNoFormatInterface _MsCInter;

        public CLGRegNoFormatFacadeController(CLGRegNoFormatInterface scadm)
        {
            _MsCInter = scadm;
        }
        //// GET: api/values
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

        [HttpPost]
       [Route("Savedetails")]
        public CLGAdm_College_RegNo_FormatDTO Savedetails([FromBody]CLGAdm_College_RegNo_FormatDTO id)
        {
            return _MsCInter.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGAdm_College_RegNo_FormatDTO getalldetails(int id)
        {
            return _MsCInter.getalldetails(id);
        }

        [HttpPost]
        [Route("Deletedetails")]
        public CLGAdm_College_RegNo_FormatDTO Deletedetails([FromBody]CLGAdm_College_RegNo_FormatDTO id)
        {
            return _MsCInter.Deletedetails(id);
        }
    }
}
