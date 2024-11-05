using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterSubjectController : Controller
    {
        MasterSubjectDelegates _delobj = new MasterSubjectDelegates();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterSubject_DTO getdetails(int id)
        {
            int mi_id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }
        [HttpPost]
         [Route("Savedata")]
        public MasterSubject_DTO Savedata([FromBody]MasterSubject_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("deactiveY")]
        public MasterSubject_DTO deactiveY([FromBody] MasterSubject_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
    }
}
