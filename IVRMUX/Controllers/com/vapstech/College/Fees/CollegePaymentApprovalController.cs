using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CollegePaymentApprovalController : Controller
    {
        CollegePaymentApprovalDelegate FDR = new CollegePaymentApprovalDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CollegePaymentApprovalDTO getalldetails(int id)
        { 

            CollegePaymentApprovalDTO data = new CollegePaymentApprovalDTO();

            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_ID = mid;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.userid = UserId;

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //  data.ASMAY_Id = ASMAY_Id;

            return FDR.getdetails(data);
        }


        [HttpPost]
        [Route("Getreportdetails")]
        public CollegePaymentApprovalDTO Getreportdetails([FromBody]CollegePaymentApprovalDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // = mid;
            value.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //= UserId;
            return FDR.Getreportdetails(value);
        }


        [HttpPost]
        [Route("savedetails")]
        public CollegePaymentApprovalDTO savedetails([FromBody]CollegePaymentApprovalDTO value)
        {
            value.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            value.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            value.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //= UserId;
            return FDR.savedetails(value);
        }
    }
}
