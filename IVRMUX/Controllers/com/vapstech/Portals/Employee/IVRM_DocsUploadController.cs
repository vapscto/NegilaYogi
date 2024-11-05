using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class IVRM_DocsUploadController : Controller
    {
        // GET: api/values
        IVRM_DocsUploadDelegate _notic = new IVRM_DocsUploadDelegate();

        [Route("Getdetails/{id:int}")]
        public IVRM_DocsUploadDTO Getdetails(int id)
        {
            IVRM_DocsUploadDTO obj = new IVRM_DocsUploadDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.Getdetails(obj);
        }
        
        [Route("savedetail")]
        public IVRM_DocsUploadDTO savedetail([FromBody]IVRM_DocsUploadDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = mid;
            return _notic.savedetail(data);
        }
        
        [Route("get_classes")]
        public IVRM_DocsUploadDTO get_classes([FromBody]IVRM_DocsUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _notic.get_classes(data);
        }

        [Route("getsectiondata")]
        public IVRM_DocsUploadDTO getsectiondata([FromBody]IVRM_DocsUploadDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.getsectiondata(data);
        }

        [Route("editData")]
        public IVRM_DocsUploadDTO editData([FromBody]IVRM_DocsUploadDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.editData(data);
        }


        [Route("deactivate")]
        public IVRM_DocsUploadDTO deactivate([FromBody]IVRM_DocsUploadDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.deactivate(data);
        }

    }
}
