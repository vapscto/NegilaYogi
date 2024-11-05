using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using CommonLibrary;
using corewebapi18072016.Delegates.com.vapstech.PDA;
using PreadmissionDTOs.com.vaps.PDA;

namespace corewebapi18072016.Controllers.com.vapstech.PDA
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PDAMasterHeadController : Controller
    {

        PDAMasterHeadDelegate pda = new PDAMasterHeadDelegate();
        PdaDTO data = new PdaDTO();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PdaDTO getalldetails(int id)
        {

           
            
            data.mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return pda.getdetails(data);

        }
        [HttpPost]
        [Route("savedetails")]
        public PdaDTO savedetails([FromBody] PdaDTO data)
        {
            data.mi_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return pda.savedetails(data);
        }

        [Route("getdetails")]
        public PdaDTO getdetail([FromBody]PdaDTO data)
        {
            data.mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return pda.getpagedetails(data);

        }

        [HttpPost]
        [Route("deactivate")]
        public PdaDTO deactivate([FromBody]PdaDTO id)
        {
           
            return pda.deactivateAcademicYear(id);
        }


    }
}
