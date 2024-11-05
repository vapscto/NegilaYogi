using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.NAAC.OnlineProgram;

namespace IVRMUX.Controllers.NAAC.OnlineProgram
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CompletedEventController : Controller
    {
        CompletedEventDelegate oed = new CompletedEventDelegate();

        [Route("getloaddata/{id:int}")]
        public OnlineProgramDTO getloaddata(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getloaddata(data);
        }

        [Route("Savedata")]
        public OnlineProgramDTO Savedata([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return oed.Savedata(data);
        }
        [HttpPost]
        [Route("getdetails")]
        public OnlineProgramDTO getdetails([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getdetails(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public OnlineProgramDTO deactivate([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.deactivate(data);
        }
    }
}
