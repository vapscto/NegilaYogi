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
    public class MasterPageModuleMappingFacade : Controller
    {
        public MasterPageModuleMappingInterface _pgmod;
        public MasterPageModuleMappingFacade(MasterPageModuleMappingInterface pagemod)
        {
            _pgmod = pagemod;
        }
        // GET: api/values
        [HttpGet]
        [Route("getmoduledetails/{id:int}")]
        public MasterPageModuleMappingDTO Get(int id)
        {
            return _pgmod.getmoduledet(id);
        }

        [Route("getsaveddetails/{id:int}")]
        public MasterPageModuleMappingDTO getsavdata(int id)
        {
            return _pgmod.getsaveddata(id);
        }
        // POST api/values
        [HttpPost]
        public MasterPageModuleMappingDTO Post([FromBody]MasterPageModuleMappingDTO pgmod)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _pgmod.saveorgdet(pgmod);
            // return det;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public MasterPageModuleMappingDTO Delete(int id)
        {
            return _pgmod.deleterec(id);
        }
    }
}
