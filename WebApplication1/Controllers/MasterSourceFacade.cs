using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterSourceFacade : Controller
    {
        public MasterSourceInterface _maspage;

        public MasterSourceFacade(MasterSourceInterface maspag)
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
        public MasterSourceDTO getorgdet(int id)
        {
            // id = 12;
            return _maspage.getdetails(id);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterSourceDTO getpagedetails(int id)
        {
            // id = 12;
            return _maspage.getpageedit(id);
        }

        // POST api/values
        [HttpPost]
        public MasterSourceDTO Post([FromBody] MasterSourceDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _maspage.saveorgdet(org);
            // return det;
        }

        [HttpPost("{id}")]
        public MasterSourceDTO Put(int id, [FromBody]MasterSourceDTO value)
        {
            return _maspage.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpGet]
        [Route("deletedetails/{id:int}")]
        public MasterSourceDTO Deleterec(int id)
        {
            return _maspage.deleterec(id);
        }
    }
}
