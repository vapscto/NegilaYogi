using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgFeeReceiptController : Controller
    {
        ClgFeeReceiptDelegate fdd = new ClgFeeReceiptDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return fdd.getloaddata(data);
        }

        [Route("getdetails")]
        public CollegeFeeTransactionDTO getdetails([FromBody]CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getdetails(data);
        }

        [Route("printreceipt")]
        public CollegeFeeTransactionDTO printrec([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return fdd.printrecdelegate(data);
        }

        [Route("getrecdetails")]
        public ClgPortalFeeDTO getrecdetails([FromBody]ClgPortalFeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getrecdetails(data);
        }

    }
}
