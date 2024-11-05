using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.College.Fees;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class CollegeHeadWiseCollectionReportController : Controller
    {
        CollegeHeadWiseCollectionReportDelegate od = new CollegeHeadWiseCollectionReportDelegate();

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


        [HttpPost]
        [Route("getgroupmappedheads")]
        public CollegeConcessionDTO getgroupheaddetails([FromBody]CollegeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getgroupheaddetails(data);
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
    }
}
