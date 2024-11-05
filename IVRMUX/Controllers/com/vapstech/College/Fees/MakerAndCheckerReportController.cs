using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Reports
{
    [Route("api/[controller]")]
    public class MakerAndCheckerReportController : Controller
    {
        MakerAndCheckerReportDelegate od = new MakerAndCheckerReportDelegate();



        [Route("getalldetails")]
        public CollegePaymentApprovalDTO getalldetails([FromBody] CollegePaymentApprovalDTO data)
        {


            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getdetails(data);
        }




        [Route("get_courses")]
        public CollegePaymentApprovalDTO get_courses([FromBody] CollegePaymentApprovalDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_courses(data);
        }

        [Route("get_branches")]
        public CollegePaymentApprovalDTO get_branches([FromBody] CollegePaymentApprovalDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_branches(data);
        }

        [Route("get_semisters")]
        public CollegePaymentApprovalDTO get_semisters([FromBody] CollegePaymentApprovalDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_semisters(data);
        }
        [Route("get_semisters_new")]
        public CollegePaymentApprovalDTO get_semisters_new([FromBody] CollegePaymentApprovalDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_semisters_new(data);
        }


        [HttpPost]
        [Route("getdata")]
        public CollegePaymentApprovalDTO getdata([FromBody]CollegePaymentApprovalDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(data);
        }



        [HttpPost]
        [Route("getgroupmappedheads")]
        public CollegePaymentApprovalDTO getgroupheaddetails([FromBody]CollegePaymentApprovalDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getgroupheaddetails(data);
        }

        [HttpPost]
        [Route("getgroupheadsid")]
        public CollegePaymentApprovalDTO getgroupheadsid([FromBody]CollegePaymentApprovalDTO data)
        {

            return od.getgroupheadsid(data);
        }

        [HttpPost]
        [Route("Getreportdetails")]
        public CollegePaymentApprovalDTO Getreportdetails([FromBody] CollegePaymentApprovalDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_ID = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.Getreportdetails(MMD);
        }
    }
}
