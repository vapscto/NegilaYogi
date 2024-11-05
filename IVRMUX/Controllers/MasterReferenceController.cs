using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterReferenceController : Controller
    {

         MasterReferenceDelegate masterrefer = new MasterReferenceDelegate();
        // GET: api/MasterReference
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterRefernceDTO Get(int id)
        {
            return masterrefer.GetmasterReferendetails(id);
            //HttpContext.Session.SetString("institutionid","0"); //Set
        }
        [Route("Editdetails/{id:int}")]
        public MasterRefernceDTO EditDetails(int id)
        {
            return masterrefer.EditDetails(id);
        }

        // GET: api/MasterReference/5

        // POST: api/MasterReference
        [HttpPost]
        public MasterRefernceDTO Savedetails([FromBody] MasterRefernceDTO maste)
        {
            return masterrefer.savedetails(maste);
        }

        [HttpPost("{id}")]
        public MasterRefernceDTO Put(int id, [FromBody]MasterRefernceDTO value)
        {
            return masterrefer.getsearchdata(id, value);
        }

        // PUT: api/MasterSection/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpGet("{id}")]
        [Route("Deletedetails/{id:int}")]
        public  MasterRefernceDTO Delete(int id)
        {
            return masterrefer.DeleteMasterRecord(id);

        }
    }
}
