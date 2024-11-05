using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgMasterCourseCategoryMapController : Controller
    {
        ClgMasterCourseCategoryMapDelegate objDel =new ClgMasterCourseCategoryMapDelegate();
        
      
        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCourseCategoryMapDTO Savedetails([FromBody]ClgMasterCourseCategoryMapDTO id)
        {
            //id.MI_Id = 4;
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Savedetails(id);
        }
       
        [Route("getalldetails/{id:int}")]
        public ClgMasterCourseCategoryMapDTO getalldetails(int id)
        {
            ClgMasterCourseCategoryMapDTO data = new ClgMasterCourseCategoryMapDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getalldetails(data);
        }
        [HttpPost]
        [Route("deactive")]
        public ClgMasterCourseCategoryMapDTO deactive([FromBody]ClgMasterCourseCategoryMapDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.deactive(id);
        }
    }
}
