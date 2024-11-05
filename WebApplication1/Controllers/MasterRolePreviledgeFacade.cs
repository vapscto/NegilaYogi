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
    public class MasterRolePreviledgeFacade : Controller
    {
        public MasterRolePreviledgeInterface _pgmod;
        public MasterRolePreviledgeFacade(MasterRolePreviledgeInterface pagemod)
        {
            _pgmod = pagemod;
        }
        // GET: api/values
        [HttpGet]
        [Route("getmoduledetails/{id:int}")]
        public MasterRolePreviledgeDTO Get(int id)
        {
            return _pgmod.getmoduledet(id);
        }

       

        [Route("mobilegetalldetails/{id:int}")]
        public MasterRolePreviledgeDTO mobilegetalldetails (int id)
        {
            return _pgmod.mobilegetalldetails(id);
        }

        [HttpPost]
        [Route("getmodulepages")]
        public MasterRolePreviledgeDTO getpages([FromBody] MasterRolePreviledgeDTO id)
        {
            return _pgmod.getmodulepagedata(id);
        }

        [Route("mobilegetmodulepages")]
        public MasterRolePreviledgeDTO mobilegetmodulepages([FromBody] MasterRolePreviledgeDTO id)
        {
            return _pgmod.mobilegetmodulepages(id);
        }
        // POST api/values
        [HttpPost]
        public MasterRolePreviledgeDTO Post([FromBody]MasterRolePreviledgeDTO pgmod)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _pgmod.saveorgdet(pgmod);
            // return det;
        }
        [Route("mobilesaveorgdet")]
        public MasterRolePreviledgeDTO mobilesaveorgdet([FromBody]MasterRolePreviledgeDTO pgmod)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _pgmod.mobilesaveorgdet(pgmod);
            // return det;
        }

        [HttpPost("{id}")]
        public MasterRolePreviledgeDTO Put(int id, [FromBody]MasterRolePreviledgeDTO value)
        {
            return _pgmod.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public MasterRolePreviledgeDTO Delete(int id)
        {
            return _pgmod.deleterec(id);
        }


        [Route("mobiledeletemodpages")]
        public MasterRolePreviledgeDTO mobiledeletemodpages([FromBody] MasterRolePreviledgeDTO dTO)
        {
            return _pgmod.mobiledeletemodpages(dTO);
        }
    }
}
