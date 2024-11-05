
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterSubjectGroupFacadeController : Controller
    {
        public MasterSubjectGroupInterface _ttcategory;

        public MasterSubjectGroupFacadeController(MasterSubjectGroupInterface maspag)
        {
            _ttcategory = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterSubjectGroupDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public MasterSubjectGroupDTO Post([FromBody] MasterSubjectGroupDTO org)
        {
            return _ttcategory.savedetail(org);
        }


        [HttpPost]
        [Route("deactivate")]
        public MasterSubjectGroupDTO deactivate([FromBody] MasterSubjectGroupDTO org)
        {
            return _ttcategory.deactivate(org);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterSubjectGroupDTO getalldetailsviewrecords(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords(id);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterSubjectGroupDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit(id);
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterSubjectGroupDTO Deleterec(int id)
        {
            return _ttcategory.deleterec(id);
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
