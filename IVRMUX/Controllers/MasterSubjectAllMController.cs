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
    public class MasterSubjectAllMController : Controller
    {
        // GET: api/MasterSubject
        MasterSubjectAllMDelegate masterSub = new MasterSubjectAllMDelegate();
        // GET: api/MasterReference
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterSubjectAllMDTO getalldetails(int id)
         {
           // MasterSubjectAllMDTO maste = new MasterSubjectAllMDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return masterSub.getdetails(id);
        }

        [Route("Editdetails/{id:int}")]
        public MasterSubjectAllMDTO EditDetails(int id)
        {
            return masterSub.EditDetails(id);
        }

        [HttpPost]
        [Route("savedetail")]
        public MasterSubjectAllMDTO Savedetails([FromBody] MasterSubjectAllMDTO maste)
        {
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return masterSub.savedetails(maste);
        }

        [Route("validateordernumber")]
        public MasterSubjectAllMDTO validateordernumber([FromBody] MasterSubjectAllMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return masterSub.validateordernumber(data);
        }

        [Route("getdetails/{id:int}")]
        public MasterSubjectAllMDTO Get(int id)
        {
            MasterSubjectAllMDTO maste = new MasterSubjectAllMDTO();
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return masterSub.GetmasterSubdetails(maste);
        }

        // PUT: api/MasterSection/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //[Route("Deletedetails/{id:int}")]
        //public MasterSubjectAllMDTO Delete(int id)
        //{
        //    return masterSub.DeleteMasterRecord(id);

        //}
        [HttpGet]
        [Route("Deletedetails/{id:int}")]
        public MasterSubjectAllMDTO Delete(int id)
        {
            return masterSub.DeleteMasterRecord(id);

        }
    }
}
