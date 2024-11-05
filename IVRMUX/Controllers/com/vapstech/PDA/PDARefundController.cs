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
    public class PDARefundController:Controller
    {
        PDATransactionDTO data = new PDATransactionDTO();

        PDARefundDelegate pda = new PDARefundDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PDATransactionDTO getalldetails(int id)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return pda.getdetails(data);

        }


        [Route("searchfilter")]
        public FeeStudentTransactionDTO searchfilter([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            return pda.getsearchfilter(data);
        }

        [Route("getstuddetails")]
        public PDATransactionDTO getstuddetails([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return pda.getstuddetails(data);
        }

        [Route("Savedata")]
        public PDATransactionDTO Savedata([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return pda.Savedata(data);
        }


        [Route("searching")]
        public PDATransactionDTO searching([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            return pda.searching(data);
        }

        [Route("Deletedetails")]
        public PDATransactionDTO Deletedetails([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            return pda.Deletedetails(data);
        }

        [Route("getacademicyear")]
        public PDATransactionDTO Getstudacademic([FromBody]PDATransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.user_id = UserId;

            return pda.getdatastuacad(value);
        }
    }  
}
