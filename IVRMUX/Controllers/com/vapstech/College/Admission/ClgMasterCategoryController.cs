using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgMasterCategoryController : Controller
    {
        ClgMasterCategoryDelegate mcd = new ClgMasterCategoryDelegate();
        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCategoryDTO Savedetails([FromBody]ClgMasterCategoryDTO id)
        {           
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails")]
        public ClgMasterCategoryDTO getalldetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.getalldetails(id);
        }
        [HttpPost]
        [Route("Deletedetails")]
        public ClgMasterCategoryDTO Deletedetails([FromBody]ClgMasterCategoryDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.Deletedetails(id);
        }
        [Route("deactivate")]
        public ClgMasterCategoryDTO deactivate([FromBody]ClgMasterCategoryDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.deactivate(id);
        }
        
    }
}

