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
    public class MasterSourceController : Controller
    {
        MasterSourceDelegate od = new MasterSourceDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //public MasterSourceDTO Getsearchdata([FromQuery] string data)
        //{
        //    return od.getsearchdata(data);
        //}

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterSourceDTO Get([FromQuery] int id)
        {
            return od.getdetails(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterSourceDTO getdetail(int id)
        {
            
            return od.getpagedetails(id);
            
        }

        // POST api/values
        [HttpPost]
        public MasterSourceDTO savedetail([FromBody] MasterSourceDTO maspage)
        {
            
            return od.savedetails(maspage);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public MasterSourceDTO Put(int id, [FromBody]MasterSourceDTO value)
        {
            return od.getsearchdata(id,value);
        }

        // DELETE api/values/5
        [HttpGet]
        [Route("deletepages/{id:int}")]
        public MasterSourceDTO Delete(int id)
        {
            return od.deleterec(id);
        }
    }
}
