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
    public class BreaktimesettingsFacadeController : Controller
    {
        public BreaktimesettingsInterface _ttbreaktime;

        public BreaktimesettingsFacadeController(BreaktimesettingsInterface maspag)
        {
            _ttbreaktime = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public TTBreakTimesettingDTO getdetails(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
      
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTBreakTimesettingDTO Post([FromBody] TTBreakTimesettingDTO org)
        {
            return _ttbreaktime.savedetail(org);
        }

        [Route("getmaximumperiodscount")]
        public TTBreakTimesettingDTO getmaximumperiodscount([FromBody] TTBreakTimesettingDTO org)
        {
            return _ttbreaktime.getmaximumperiodscount(org);
        }
        [Route("getclass_catg")]
        public TTBreakTimesettingDTO getclass_catg([FromBody] TTBreakTimesettingDTO org)
        {
            return _ttbreaktime.getclass_catg(org);
        }
        [Route("get_catg")]
        public TTBreakTimesettingDTO get_catg([FromBody] TTBreakTimesettingDTO org)
        {
            return _ttbreaktime.get_catg(org);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTBreakTimesettingDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttbreaktime.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TTBreakTimesettingDTO Deleterec(int id)
        {
            return _ttbreaktime.deleterec(id);
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
        public TTBreakTimesettingDTO deactivateAcdmYear([FromBody] TTBreakTimesettingDTO id)
        {
            // id = 12;
            return _ttbreaktime.deactivate(id);
        }
    }
}
