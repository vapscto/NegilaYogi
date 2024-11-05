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
    public class MasterRoleTypeController : Controller
    {
        MasterRoleTypeDelegate od = new MasterRoleTypeDelegate();
        // GET: api/values

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterRoleTypeDTO Get([FromQuery] int id)
        {
            return od.getdetails(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterRoleTypeDTO getdetail(int id)
        {
           
            return od.getpagedetails(id);
            //HttpContext.Session.SetString("institutionid","0"); //Set
        }

        // POST api/values
        [HttpPost]
        public MasterRoleTypeDTO savedetail([FromBody] MasterRoleTypeDTO maspage)
        {
            return od.savedetails(maspage);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public MasterRoleTypeDTO Put(int id, [FromBody]MasterRoleTypeDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterRoleTypeDTO Delete(int id)
        {
            return od.deleterec(id);
        }
    }
}
