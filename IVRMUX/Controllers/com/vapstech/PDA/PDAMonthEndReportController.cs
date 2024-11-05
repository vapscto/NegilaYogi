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
    public class PDAMonthEndReportController:Controller
    {

        PDATransactionDTO data = new PDATransactionDTO();

        PDAMonthEndReportDelegate pda = new PDAMonthEndReportDelegate();


        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public PDATransactionDTO getalldetails123(int id)
        {
            PDATransactionDTO data = new PDATransactionDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return pda.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public PDATransactionDTO getreport([FromBody] PDATransactionDTO data123)
        {
            data123.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data123.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return pda.getreport(data123);
        }



    }
}
