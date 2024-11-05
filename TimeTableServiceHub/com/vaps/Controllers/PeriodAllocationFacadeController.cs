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
    public class PeriodAllocationFacadeController : Controller
    {
        public PeriodAllocationInterface _ttperiod;

        public PeriodAllocationFacadeController(PeriodAllocationInterface maspag)
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
        public TTPeriodAllocationDTO getorgdet([FromBody] TTPeriodAllocationDTO data)
        {
            return _ttperiod.getdetails(data);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("saveperiod")]
        public TTPeriodAllocationDTO saveperiod([FromBody] TTPeriodAllocationDTO org)
        {
            return _ttperiod.saveperiod(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public TTPeriodAllocationDTO Post([FromBody] TTPeriodAllocationDTO org)
        {
            return _ttperiod.savedetail(org);
        }
        [HttpPost]
        [Route("getclasses")]
        public TTPeriodAllocationDTO getclasses([FromBody] TTPeriodAllocationDTO data)
        {
            return _ttperiod.getclasses(data);
        }
        [HttpPost]
        [Route("getcategories")]
        public TTPeriodAllocationDTO getcategories([FromBody] TTPeriodAllocationDTO data)
        {
            return _ttperiod.getcategories(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public TTPeriodAllocationDTO getperiod_class([FromBody] TTPeriodAllocationDTO data)
        {
            return _ttperiod.getperiod_class(data);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTPeriodAllocationDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TTPeriodAllocationDTO Deleterec(int id)
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

        [Route("deactivate")]
        public TTPeriodAllocationDTO deactivateAcdmYear([FromBody] TTPeriodAllocationDTO id)
        {
            return _ttperiod.deactivate(id);
        }
        [Route("deactivate1")]
        public TTPeriodAllocationDTO deactivateAcdmYear1([FromBody] TTPeriodAllocationDTO id)
        {
            return _ttperiod.deactivate1(id);
        }
    }
}
