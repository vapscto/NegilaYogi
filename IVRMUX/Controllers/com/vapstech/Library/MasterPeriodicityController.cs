using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using corewebapi18072016.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterPeriodicityController : Controller
    {
        
        MasterPeriodicityDelegate _delobj = new MasterPeriodicityDelegate();
        [Route("Savedata")]
        public MasterPeriodicityDTO Savedata([FromBody]MasterPeriodicityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterPeriodicityDTO getdetails(int id)
        {
           
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterPeriodicityDTO deactiveY([FromBody]MasterPeriodicityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
    }
}
