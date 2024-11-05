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
    public class TTGenerationFromPreviousVersionFacadeController : Controller
    {
        public TTGenerationFromPreviousVersionInterface _ttcategory;

        public TTGenerationFromPreviousVersionFacadeController(TTGenerationFromPreviousVersionInterface maspag)
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
        public TTTTGenerationFromPreviousVersionDTO getorgdet(int id)
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
        public TTTTGenerationFromPreviousVersionDTO Post([FromBody] TTTTGenerationFromPreviousVersionDTO org)
        {
            return _ttcategory.savedetail_(org);
        }

        [HttpPost]
        [Route("deactivate")]
        public TTTTGenerationFromPreviousVersionDTO deactivate([FromBody] TTTTGenerationFromPreviousVersionDTO org)
        {
            return _ttcategory.deactivate(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTTTGenerationFromPreviousVersionDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TTTTGenerationFromPreviousVersionDTO Deleterec(int id)
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
