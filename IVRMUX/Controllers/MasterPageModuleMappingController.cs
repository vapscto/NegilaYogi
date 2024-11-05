using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterPageModuleMappingController : Controller
    {
        MasterPageModuleMappingDelegate pgmod = new MasterPageModuleMappingDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterPageModuleMappingDTO Get(int id)
        {
            return pgmod.getmoduledet(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterPageModuleMappingDTO Getpagedata(int id)
        {
            return pgmod.getselectedpg(id);
        }

        [Route("getsaveddetails/{id:int}")]
        public MasterPageModuleMappingDTO getsavdata(int id)
        {
            return pgmod.getsaveddata(id);
        }

        // POST api/values
        [HttpPost]
        public MasterPageModuleMappingDTO savedata([FromBody] MasterPageModuleMappingDTO pgmodu)
        {
            return pgmod.savedetails(pgmodu);
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
            return pgmod.deleterec(id);
        }
    }
}
