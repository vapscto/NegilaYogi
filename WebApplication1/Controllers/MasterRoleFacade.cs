using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterRoleFacade : Controller
    {
        public MasterRoleInterface _maspage;

        public MasterRoleFacade(MasterRoleInterface maspag)
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
        public MasterRoleDTO getorgdet(int id)
        {
            // id = 12;
            return _maspage.getdetails(id);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterRoleDTO getpagedetails(int id)
        {
            // id = 12;
            return _maspage.getpageedit(id);
        }

        // POST api/values
        [HttpPost]
        public MasterRoleDTO Post([FromBody] MasterRoleDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _maspage.saveorgdet(org);
            // return det;
        }

        [HttpPost("{id}")]
        public MasterRoleDTO Put(int id, [FromBody]MasterRoleDTO value)
        {
            return _maspage.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterRoleDTO Deleterec(int id)
        {
            return _maspage.deleterec(id);
        }
    }
}
