using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClgCOEServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClgCOEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgMasterCOEFacadeController : Controller
    {
        public ClgMasterCOEInterface _ttcategory;

        public ClgMasterCOEFacadeController(ClgMasterCOEInterface maspag)
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
        //[HttpGet]
        //[Route("getdetails/{id:int}")]
        //public ClgMasterCOEDTO getorgdet(int id)
        //{
        //    return _ttcategory.getdetails(id);
        //}
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        
        [Route("courseselect")]
        public ClgMasterCOEDTO courseselect([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.courseselect(org);
        }
        [Route("branchselect")]
        public ClgMasterCOEDTO branchselect([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.branchselect(org);
        }
        
        [HttpPost]
        [Route("savedetail1")]
        public ClgMasterCOEDTO Post1([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.savedetail1(org);
        }
        [Route("getdetails")]
        public ClgMasterCOEDTO getdetails([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.getdetails(org);
        }
        [HttpPost]
        [Route("savedetail2")]
        public ClgMasterCOEDTO Post2([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.savedetail2(org);
        }
        [HttpPost]
        [Route("geteventdetails")]
        public ClgMasterCOEDTO geteventdetails([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.geteventdetails(org);
        }

        [HttpPost]
        [Route("deactivate1")]
        public ClgMasterCOEDTO deactivate1([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.deactivate1(org);
        }
        [HttpPost]
        [Route("deactivate2")]
        public ClgMasterCOEDTO deactivate2([FromBody] ClgMasterCOEDTO org)
        {
            return _ttcategory.deactivate2(org);
        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        //[Route("getenquirycontroller")]
        public ClgMasterCOEDTO getalldetailsviewrecords1(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        //[Route("getenquirycontroller")]
        public ClgMasterCOEDTO getalldetailsviewrecords2(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords2(id);
        }
        [Route("getpagedetails1/{id:int}")]
        //[Route("getenquirycontroller")]
        public ClgMasterCOEDTO getpagedetails1(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit1(id);
        }
        [Route("getpagedetails2/{id:int}")]
        //[Route("getenquirycontroller")]
        public ClgMasterCOEDTO getpagedetails2(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit2(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public ClgMasterCOEDTO Deleterec(int id)
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
