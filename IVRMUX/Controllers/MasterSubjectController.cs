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
    public class MasterSubjectController : Controller
    {
        // GET: api/MasterSubject
        MasterSubjectDelegate masterSub = new MasterSubjectDelegate();
        // GET: api/MasterReference
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterSubjectDTO Get(int id)
        {
            MasterSubjectDTO maste = new MasterSubjectDTO();
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return masterSub.GetmasterSubdetails(maste);
        }
        [Route("Editdetails/{id:int}")]
        public MasterSubjectDTO EditDetails(int id)
        {
            return masterSub.EditDetails(id);
        }

        [HttpPost]
        public MasterSubjectDTO Savedetails([FromBody] MasterSubjectDTO maste)
        {
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return masterSub.savedetails(maste);
        }

        // PUT: api/MasterSection/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Route("Deletedetails/{id:int}")]
        public MasterSubjectDTO Delete(int id)
        {
            return masterSub.DeleteMasterRecord(id);

        }
    }
}
