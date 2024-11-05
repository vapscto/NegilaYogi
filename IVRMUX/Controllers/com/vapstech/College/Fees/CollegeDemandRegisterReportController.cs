using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Fees;



namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CollegeDemandRegisterReportController : Controller
    {
        CollegeDemandRegisterReportDelegate od = new CollegeDemandRegisterReportDelegate();



        [Route("getalldetails")]
        public CollegeConcessionDTO getalldetails([FromBody] CollegeConcessionDTO data)
        {


            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getdetails(data);
        }




        [Route("get_courses")]
        public CollegeConcessionDTO get_courses([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_courses(data);
        }

        [Route("get_branches")]
        public CollegeConcessionDTO get_branches([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_branches(data);
        }

        [Route("get_semisters")]
        public CollegeConcessionDTO get_semisters([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_semisters(data);
        }
        [Route("get_semisters_new")]
        public CollegeConcessionDTO get_semisters_new([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_semisters_new(data);
        }


        [HttpPost]
        [Route("getdata")]
        public CollegeConcessionDTO getdata([FromBody]CollegeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(data);
        }



        [HttpPost]
        [Route("getgroupmappedheads")]
        public CollegeConcessionDTO getgroupheaddetails([FromBody]CollegeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getgroupheaddetails(data);
        }

        [HttpPost]
        [Route("getgroupheadsid")]
        public CollegeConcessionDTO getgroupheadsid([FromBody]CollegeConcessionDTO data)
        {

            return od.getgroupheadsid(data);
        }

        [HttpPost]
        [Route("Getreportdetails")]
        public CollegeConcessionDTO Getreportdetails([FromBody] CollegeConcessionDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.Getreportdetails(MMD);
        }
    }
}
