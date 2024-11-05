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
    public class TT_ConsecutiveFacadeController : Controller
    {
        public TT_ConsecutiveInterface _ttcategory;

        public TT_ConsecutiveFacadeController(TT_ConsecutiveInterface maspag)
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
        public TT_ConsecutiveDTO getorgdet(int id)
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
        public TT_ConsecutiveDTO Post([FromBody] TT_ConsecutiveDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TT_ConsecutiveDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit(id);
        }
        [Route("getclass_catg")]
        public TT_ConsecutiveDTO getclass_catg([FromBody] TT_ConsecutiveDTO org)
        {
            return _ttcategory.getclass_catg(org);
        }
        [Route("get_catg")]
        public TT_ConsecutiveDTO get_catg([FromBody] TT_ConsecutiveDTO org)
        {
            return _ttcategory.get_catg(org);
        }

        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TT_ConsecutiveDTO Deleterec(int id)
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

        [HttpPost]
        [Route("deactivate")]
        public TT_ConsecutiveDTO deactivateAcdmYear([FromBody] TT_ConsecutiveDTO id)
        {
            // id = 12;
            return _ttcategory.deactivate(id);
        }
    }
}
