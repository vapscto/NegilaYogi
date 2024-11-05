using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.NAAC.OnlineProgram;

namespace IVRMUX.Controllers.OnlineProgram
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class YearlyProgramController : Controller
    {
        YearlyProgramDelegate oed = new YearlyProgramDelegate();

        [Route("getloaddata/{id:int}")]
        public OnlineProgramDTO getloaddata(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getloaddata(data);
        }

        [Route("Savedata")]
        public OnlineProgramDTO Savedata([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return oed.Savedata(data);
        }

        [Route("removeNewsiblinguest")]
        public OnlineProgramDTO removeNewsiblinguest([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return oed.removeNewsiblinguest(data);
        }
        [HttpPost]
        [Route("getdetails")]
        public OnlineProgramDTO getdetails([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getdetails(data);
        }
        [HttpPost]
        [Route("delete")]
        public OnlineProgramDTO delete([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.delete(data);
        }

        [Route("viewuploadflies")]
        public OnlineProgramDTO viewuploadflies([FromBody]   OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.viewuploadflies(data);
        }

        [Route("editguest")]
        public OnlineProgramDTO editguest([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.editguest(data);
        }
    }
}
