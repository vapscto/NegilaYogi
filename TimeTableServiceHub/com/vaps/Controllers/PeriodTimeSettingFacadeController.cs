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
    public class PeriodTimeSettingFacadeController : Controller
    {
        public PeriodTimeSettingInterface _ttbreaktime;

        public PeriodTimeSettingFacadeController(PeriodTimeSettingInterface maspag)
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
        public TT_Master_Day_Period_TimeDTO getorgdet(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("deletedetails")]
        public TT_Master_Day_Period_TimeDTO Deleterec([FromBody] TT_Master_Day_Period_TimeDTO id)
        {
            return _ttbreaktime.deleterec(id);
        }
        [Route("savedetail")]
        public TT_Master_Day_Period_TimeDTO Post([FromBody] TT_Master_Day_Period_TimeDTO org)
        {
            return _ttbreaktime.savedetail(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TT_Master_Day_Period_TimeDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttbreaktime.getpageedit(id);
        }
        [HttpDelete]


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
