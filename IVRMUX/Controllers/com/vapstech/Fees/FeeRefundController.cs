using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeRefundController : Controller
    {

        FeeRefundDelegate frd = new FeeRefundDelegate();

        // GET: api/values
        //[HttpGet]
        //[Route("getalldetails/{id:int}")]
        //public FeeRefundDTO Get123(int id)
        //{
        //    FeeRefundDTO data = new FeeRefundDTO();
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    //data.MI_ID = mid;

        //    //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    //data.asmyid = ASMAY_Id;

        //    return frd.getalldetails(mid);
        //}
        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeRefundDTO getalldetails(int id)
        {
            FeeRefundDTO data = new FeeRefundDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmyid = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;


            return frd.getalldetails(data);
        }

        [HttpPost]
        [Route("getsection")]
        public FeeRefundDTO getsection([FromBody]FeeRefundDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmyid = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return frd.getsection(data);
        }

        
        [HttpPost]
        [Route("getstudent")]
        public FeeRefundDTO getstudent([FromBody]FeeRefundDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return frd.getstudent(data);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeRefundDTO getreport([FromBody]FeeRefundDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmyid = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return frd.getradiofiltereddata(data);
        }


    }
}
