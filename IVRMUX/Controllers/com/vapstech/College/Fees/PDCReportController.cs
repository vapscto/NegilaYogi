using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Fees;


namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Reports
{
    [Route("api/[controller]")]
    public class PDCReportController : Controller
    {
        PDCReportDelegate od = new PDCReportDelegate();



        [Route("getalldetails")]
        public PDC_EntryFormDTO getalldetails([FromBody] PDC_EntryFormDTO data)
        {


            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getdetails(data);
        }




        [Route("get_courses")]
        public PDC_EntryFormDTO get_courses([FromBody] PDC_EntryFormDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = user_id;

            return od.get_courses(data);
        }

        [Route("get_branches")]
        public PDC_EntryFormDTO get_branches([FromBody] PDC_EntryFormDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = user_id;

            return od.get_branches(data);
        }

        [Route("get_semisters")]
        public PDC_EntryFormDTO get_semisters([FromBody] PDC_EntryFormDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = user_id;

            return od.get_semisters(data);
        }
        [Route("get_semisters_new")]
        public PDC_EntryFormDTO get_semisters_new([FromBody] PDC_EntryFormDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = user_id;

            return od.get_semisters_new(data);
        }


        [HttpPost]
        [Route("getdata")]
        public PDC_EntryFormDTO getdata([FromBody]PDC_EntryFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(data);
        }



        [HttpPost]
        [Route("getgroupmappedheads")]
        public PDC_EntryFormDTO getgroupheaddetails([FromBody]PDC_EntryFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getgroupheaddetails(data);
        }

        [HttpPost]
        [Route("getgroupheadsid")]
        public PDC_EntryFormDTO getgroupheadsid([FromBody]PDC_EntryFormDTO data)
        {

            return od.getgroupheadsid(data);
        }

        [HttpPost]
        [Route("Getreportdetails")]
        public PDC_EntryFormDTO Getreportdetails([FromBody] PDC_EntryFormDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            MMD.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.Getreportdetails(MMD);
        }
    }
}
