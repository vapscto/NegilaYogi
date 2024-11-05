using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class CollegeDefaultersReportController : Controller
    {
        CollegeDefaultersReportDelegate od = new CollegeDefaultersReportDelegate();

        [Route("getalldetails")]
        public CollegeConcessionDTO getalldetails([FromBody] CollegeConcessionDTO data)
        {
            // CollegeConcessionDTO dt = new CollegeConcessionDTO();

            //id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getdetails(data);
        }
        // POST api/values



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

        [Route("radiobtndata")]
        public CollegeConcessionDTO radiobtndata([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.radiobtndata(data);
        }

        [Route("SendSms/")]
        public FeetransactionSMS SendSms([FromBody]FeetransactionSMS data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            data.FMT_ID = Convert.ToInt32(HttpContext.Session.GetInt32("FMT"));
            return od.sendsms(data);
        }
        [Route("Sendmail/")]
        public FeetransactionSMS Sendmail([FromBody]FeetransactionSMS data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.sendemail(data);
        }
    }
}
