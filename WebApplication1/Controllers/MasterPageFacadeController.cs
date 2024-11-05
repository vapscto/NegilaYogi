using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterPageFacadeController : Controller
    {
        public MasterPageInterface _maspage;

        public MasterPageFacadeController(MasterPageInterface maspag)
        {
            _maspage = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterPageDTO getorgdet(int id)
        {
            // id = 12;
            return _maspage.getdetails(id);
        }

        [Route("getalldetailsmobile/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterPageDTO getalldetailsmobile(int id)
        {
            // id = 12;
            return _maspage.getalldetailsmobile(id);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterPageDTO getpagedetails(int id)
        {
            // id = 12;
            return _maspage.getpageedit(id);
        }

        [Route("mobilegetdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterPageDTO mobilegetdetails(int id)
        {
            // id = 12;
            return _maspage.mobilegetdetails(id);
        }

        // POST api/values
        [HttpPost]
        public MasterPageDTO Post([FromBody] MasterPageDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _maspage.saveorgdet(org);
            // return det;
        }
        [Route("mobilesaveorgdet")]
        public MasterPageDTO mobilesaveorgdet([FromBody] MasterPageDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _maspage.mobilesaveorgdet(org);
            // return det;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpPost("{id}")]
        public MasterPageDTO Put(int id, [FromBody]MasterPageDTO value)
        {
            return _maspage.getsearchdata(id, value);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterPageDTO Deleterec(int id)
        {
            return _maspage.deleterec(id);
        }

        [Route("mobiledeleterec")]
        public MasterPageDTO mobiledeleterec([FromBody] MasterPageDTO id)
        {
            return _maspage.mobiledeleterec(id);
        }
    }
}
