using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClassMasterFacadeController : Controller
    {
        public ClassMasterInterface _ttperiod;

        public ClassMasterFacadeController(ClassMasterInterface maspag)
        {
            _ttperiod = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public TTClassMasterDTO getorgdet([FromBody] TTClassMasterDTO data)
        {
            return _ttperiod.getdetails(data);
        }
        [HttpPost]
        [Route("getcategories")]
        public TTClassMasterDTO getcategories([FromBody] TTClassMasterDTO data)
        {
            return _ttperiod.getcategories(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public TTClassMasterDTO getalldetailsviewrecords(int id)
        {
            return _ttperiod.getalldetailsviewrecords(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTClassMasterDTO Post([FromBody] TTClassMasterDTO org)
        {
            return _ttperiod.savedetail(org);
        }
        [HttpPost]
        [Route("getclasses")]
        public TTClassMasterDTO getclasses([FromBody] TTClassMasterDTO data)
        {
            return _ttperiod.getclasses(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public TTClassMasterDTO deactivate([FromBody] TTClassMasterDTO org)
        {
            return _ttperiod.deactivate(org);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTClassMasterDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TTClassMasterDTO Deleterec(int id)
        {
            return _ttperiod.deleterec(id);
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
