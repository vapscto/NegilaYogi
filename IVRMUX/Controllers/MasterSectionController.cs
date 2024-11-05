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
    public class MasterSectionController : Controller
    {
         MasterSectionDelegate mastersect = new MasterSectionDelegate();
        // GET: api/MasterSection
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public  MasterSectionDTO Get(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.Getmastersectiondetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public MasterSectionDTO EditDetails(int id)
        {
            return mastersect.EditDetails(id);
        }

        // POST: api/MasterSection
        [HttpPost]
        public MasterSectionDTO Savedetails([FromBody] MasterSectionDTO maste)
        {
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersect.savedetails(maste);
        }

        [HttpPost("{id}")]
        public MasterSectionDTO Put(int id, [FromBody]MasterSectionDTO value)
        {
            return mastersect.getsearchdata(id, value);
        }

        // PUT: api/MasterSection/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpGet("{id}")]
        [Route("Deletedetails/{id:int}")]
        public  MasterSectionDTO Delete(int id)
        {
            MasterSectionDTO dto = new MasterSectionDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMC_Id = id;
            return mastersect.DeleteMasterRecord(dto);
            
    }
    }
}
