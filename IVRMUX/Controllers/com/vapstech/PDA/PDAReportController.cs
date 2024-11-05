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
    public class PDAReportController:Controller
    {

        PDAReportDelegate pda = new PDAReportDelegate();
        PdaDTO data = new PdaDTO();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PDATransactionDTO getalldetails(int id)
        {
            PDATransactionDTO data = new PDATransactionDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return pda.getalldetails(data);

        }

        [Route("radiobtndata")]
        public PDATransactionDTO radiobtndata([FromBody]PDATransactionDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return pda.radiobtndata(data);

        }


        [Route("getstudent")]
        public PDATransactionDTO getclass([FromBody]PDATransactionDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return pda.getstudent(data);

        }

        [Route("getsection")]
        public PDATransactionDTO getsection([FromBody]PDATransactionDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return pda.getsection(data);

        }

        //[Route("Deletedetails")]
        //public PDATransactionDTO Deletedetails([FromBody]PDATransactionDTO data)
        //{

        //    return pda.Deletedetails(data);
        //}

    }
}
