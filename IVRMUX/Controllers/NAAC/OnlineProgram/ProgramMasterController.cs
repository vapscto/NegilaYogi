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
    public class ProgramMasterController : Controller
    {
        ProgramMasterDelegate oed = new ProgramMasterDelegate();

        [Route("getloaddata/{id:int}")]
        public OnlineProgramDTO getloaddata(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getloaddata(data);

        }

        [Route("savedatatype")]
        public OnlineProgramDTO savedatatype([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return oed.savedatatype(data);
        }
        [HttpPost]
        
        [Route("savedatalevel")]
        public OnlineProgramDTO savedatalevel([FromBody]OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            
            return oed.savedatalevel(data);

        }
        [HttpPost]
        [Route("editlevel")]
        public OnlineProgramDTO editlevel([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.editlevel(data);
        }

        [HttpPost]
        [Route("edittype")]
        public OnlineProgramDTO edittype([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.edittype(data);
        }

        [HttpPost]
        [Route("deactivelevel")]
        public OnlineProgramDTO deactivelevel([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.deactivelevel(data);
        }

        [HttpPost]
        [Route("deactivetype")]
        public OnlineProgramDTO deactivetype([FromBody] OnlineProgramDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.deactivetype(data);
        }



    }
}
