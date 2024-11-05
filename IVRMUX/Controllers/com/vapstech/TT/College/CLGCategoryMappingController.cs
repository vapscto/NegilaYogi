using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT.College;


namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGCategoryMappingController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGCategoryMappingDelegate ad = new CLGCategoryMappingDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGCategoryMappingDTO Get([FromQuery] int id)
        {
            CLGCategoryMappingDTO data = new CLGCategoryMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;

            return ad.getalldetails(data);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("getBranch")]
        public CLGCategoryMappingDTO getBranch([FromBody] CLGCategoryMappingDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.getBranch(data);
        }

      [HttpPost]
        [Route("savedata")]
        public CLGCategoryMappingDTO savedetails([FromBody] CLGCategoryMappingDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.savedetails(data);
        }
      

        [Route("deletedetails/{id:int}")]
        public CLGCategoryMappingDTO Delete(int id)
        {
            CLGCategoryMappingDTO data = new CLGCategoryMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.TTCC_Id = id;
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ad.deleterec(data);
        }


    }
}
