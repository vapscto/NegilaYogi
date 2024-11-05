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
    public class ClgMasterCourseBranchMapController : Controller
    {
        ClgMasterBranchMapDelegate objDel =new ClgMasterBranchMapDelegate();
        
       
        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCourseBranchMapDTO Savedetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            //id.MI_Id = 4;
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Savedetails(id);
        }

        [Route("getalldetails/{id:int}")]
        public ClgMasterCourseBranchMapDTO getalldetails(int id)
        {
            ClgMasterCourseBranchMapDTO data = new ClgMasterCourseBranchMapDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getalldetails(data);
        }
        [HttpPost]
        [Route("Deletedetails")]
        public ClgMasterCourseBranchMapDTO Deletedetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Deletedetails(id);
        }
        [Route("showmodaldetails")]
        public ClgMasterCourseBranchMapDTO showmodaldetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.showmodaldetails(id);
        }
        [Route("deactivesem")]
        public ClgMasterCourseBranchMapDTO deactivesem([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.deactivesem(id);
        }
        [Route("edit")]
        public ClgMasterCourseBranchMapDTO edit([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.edit(id);
        }

        


    }
}
