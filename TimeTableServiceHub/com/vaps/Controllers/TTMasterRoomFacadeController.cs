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
    public class TTMasterRoomFacadeController : Controller
    {
        public TTMasterRoomInterface _ttcategory;

        public TTMasterRoomFacadeController(TTMasterRoomInterface maspag)
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
        public TTMasterRoomDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
        [Route("Viewfacility/{id:int}")]
        public TTMasterRoomDTO Viewfacility(int id)
        {
            return _ttcategory.Viewfacility(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTMasterRoomDTO Post([FromBody] TTMasterRoomDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTMasterRoomDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit(id);
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
        public TTMasterRoomDTO deactivateAcdmYear([FromBody] TTMasterRoomDTO id)
        {
            // id = 12;
            return _ttcategory.deactivate(id);
        }
    }
}
