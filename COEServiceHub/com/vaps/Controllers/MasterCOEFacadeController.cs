using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoeServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoeServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterCOEFacadeController : Controller
    {
        public MasterCOEInterface _ttcategory;

        public MasterCOEFacadeController(MasterCOEInterface maspag)
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
        public MasterCOEDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail1")]
        public MasterCOEDTO Post1([FromBody] MasterCOEDTO org)
        {
            return _ttcategory.savedetail1(org);
        }
        [HttpPost]
        [Route("savedetail2")]
        public MasterCOEDTO Post2([FromBody] MasterCOEDTO org)
        {
            return _ttcategory.savedetail2(org);
        }
        [HttpPost]
        [Route("geteventdetails")]
        public MasterCOEDTO geteventdetails([FromBody] MasterCOEDTO org)
        {
            return _ttcategory.geteventdetails(org);
        }

        [HttpPost]
        [Route("deactivate1")]
        public MasterCOEDTO deactivate1([FromBody] MasterCOEDTO org)
        {
            return _ttcategory.deactivate1(org);
        }
        [HttpPost]
        [Route("deactivate2")]
        public MasterCOEDTO deactivate2([FromBody] MasterCOEDTO org)
        {
            return _ttcategory.deactivate2(org);
        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterCOEDTO getalldetailsviewrecords1(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterCOEDTO getalldetailsviewrecords2(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords2(id);
        }
        [Route("getpagedetails1/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterCOEDTO getpagedetails1(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit1(id);
        }
        [Route("getpagedetails2/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterCOEDTO getpagedetails2(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit2(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterCOEDTO Deleterec(int id)
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
