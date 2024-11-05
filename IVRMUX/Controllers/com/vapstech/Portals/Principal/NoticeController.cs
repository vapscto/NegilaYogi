using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [Route("api/[controller]")]
    public class NoticeController : Controller
    {
        NoticeDelegates _notic = new NoticeDelegates();
        // GET: api/values
        [Route("savedetail")]
        public Notice_DTO savedetail([FromBody]Notice_DTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public Notice_DTO Getdetails(int id)
        {
            Notice_DTO obj = new Notice_DTO();
           // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _notic.Getdetails(obj);
        }
        [Route("deactivate")]
        public Notice_DTO deactivate([FromBody]Notice_DTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.deactivate(data);
        }
    }
}
