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
    public class MasterDayFacadeController : Controller
    {

        private MasterDayInterface inter;
        
        public MasterDayFacadeController(MasterDayInterface maspag)
        {
            inter = maspag;
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [Route("getdetails/{id:int}")]
        public TT_Master_DayDTO getorgdet(int id)
        {
            return inter.getdetails(id);
        }
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [HttpPost]
        [Route("savedetail")]
        public TT_Master_DayDTO Post([FromBody] TT_Master_DayDTO org)
        {
            return inter.savedetail(org);
        }

        [HttpPost]
        [Route("savedaydetail")]
        public TT_Master_DayDTO savedaydetail([FromBody] TT_Master_DayDTO org)
        {
            return inter.savedaydetail(org);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TT_Master_DayDTO getpagedetails(int id)
        {
            // id = 12;
            return inter.getpageedit(id);
        }
        [Route("getdaydetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TT_Master_DayDTO getdaydetails(int id)
        {
            // id = 12;
            return inter.getdayedit(id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TT_Master_DayDTO Deleterec(int id)
        {
            return inter.deleterec(id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("deactivate")]
        public TT_Master_DayDTO deactivateAcdmYear([FromBody] TT_Master_DayDTO id)
        {
            // id = 12;
            return inter.deactivate(id);
        }
        [Route("deactivate1")]
        public TT_Master_DayDTO deactivateAcdmYear1([FromBody] TT_Master_DayDTO id)
        {
            // id = 12;
            return inter.deactivate1(id);
        }
        [Route("getorder")]
        public TT_Master_DayDTO getorder([FromBody] TT_Master_DayDTO id)
        {
            // id = 12;
            return inter.getorder(id);
        }
        [Route("saveorder")]
        public TT_Master_DayDTO saveorder([FromBody] TT_Master_DayDTO id)
        {
            // id = 12;
            return inter.saveorder(id);
        }

        [HttpPost]
        [Route("getavdata")]
        public TT_Master_DayDTO getavdata([FromBody]TT_Master_DayDTO data)
        {
            return inter.getavdata(data);
        }
        [HttpPost]
        [Route("getPeriods")]
        public TT_Master_DayDTO getPeriods([FromBody]TT_Master_DayDTO data)
        {
            return inter.getPeriods(data);
        }
        [HttpPost]
        [Route("allocateperiod")]
        public TT_Master_DayDTO allocateperiod([FromBody]TT_Master_DayDTO data)
        {
            return inter.allocateperiod(data);
        }



    }
}
