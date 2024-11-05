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
    public class LabconstraintsFacadeController : Controller
    {
        public LabconstraintsInterface _ttcategory;

        public LabconstraintsFacadeController(LabconstraintsInterface maspag)
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
        [Route("getdetails/{id:int}")]
        public TT_LABLIB_DTO getorgdet(int id)
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
        public TT_LABLIB_DTO Post([FromBody] TT_LABLIB_DTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TT_LABLIB_DTO getpagedetails(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TT_LABLIB_DTO Deleterec(int id)
        {
            return _ttcategory.deleterec(id);
        }
        [Route("getclass_catg")]
        public TT_LABLIB_DTO getclass_catg([FromBody] TT_LABLIB_DTO org)
        {
            return _ttcategory.getclass_catg(org);
        }
        [Route("get_catg")]
        public TT_LABLIB_DTO get_catg([FromBody] TT_LABLIB_DTO org)
        {
            return _ttcategory.get_catg(org);
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

        [Route("getalldetailsviewrecords/{id:int}")]
        public TT_LABLIB_DTO getalldetailsviewrecords(int id)
        {
            return _ttcategory.getalldetailsviewrecords(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public TT_LABLIB_DTO deactivateAcdmYear([FromBody] TT_LABLIB_DTO id)
        {
            // id = 12;
            return _ttcategory.deactivate(id);
        }
    }
}
