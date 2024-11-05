using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Library;
using corewebapi18072016.Delegates.com.vapstech.Library;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterCategoryController : Controller
    {
        MasterCategoryDelegates _delobj = new MasterCategoryDelegates();

       //[HttpPost]
        [Route("Savedata")]
       public MasterCategory_DTO Savedata([FromBody]MasterCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("deactiveY")]
        public MasterCategory_DTO deactiveY([FromBody] MasterCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterCategory_DTO getdetails(int id)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(mid);
        }
    }
}
