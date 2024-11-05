using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterRoleController : Controller
    {
        MasterRoleDelegate od = new MasterRoleDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterRoleDTO Get([FromQuery] int id)
        {
            return od.getdetails(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterRoleDTO getdetail(int id)
        {
           
            return od.getpagedetails(id);
        }

        // POST api/values
        [HttpPost]
        public MasterRoleDTO savedetail([FromBody] MasterRoleDTO maspage)
        {
            return od.savedetails(maspage);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public MasterRoleDTO Put(int id, [FromBody]MasterRoleDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterRoleDTO Delete(int id)
        {
            return od.deleterec(id);
        }
    }
}
