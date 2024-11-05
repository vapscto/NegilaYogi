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
    public class MasterCategoryController : Controller
    {
        // GET: api/MasterCategory
        MasterCategoryDelegate mcd = new MasterCategoryDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterCategoryDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.getAll(id);
        }

        [Route("getdetails")]
        public MasterCategoryDTO getdetail([FromBody]MasterCategoryDTO id)
        {            
            return mcd.categoryDet(id);
        }
        
        // POST api/values
        [HttpPost]
        public MasterCategoryDTO savedetail([FromBody] MasterCategoryDTO org)
        {
           
            return mcd.savedetails(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        //DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterCategoryDTO Delete(int id)
        {
            return mcd.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public MasterCategoryDTO deactvate([FromBody] MasterCategoryDTO data)
        {
            return mcd.deactivate(data);
        }
    }
}
