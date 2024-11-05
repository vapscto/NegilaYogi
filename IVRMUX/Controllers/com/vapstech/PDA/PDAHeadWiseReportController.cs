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
using PreadmissionDTOs.com.vaps.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.PDA
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PDAHeadWiseReportController:Controller
    {

        PDATransactionDTO data = new PDATransactionDTO();

        PDAHeadWiseReportDelegate pda = new PDAHeadWiseReportDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PDATransactionDTO getalldetails(int id)
        {
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
        

    }
}
