using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgMonthEndReportController : Controller
    {
        MonthEndReportDelegate delobj = new MonthEndReportDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MonthEndReportDTO Get123(int id)
        {
            MonthEndReportDTO data = new MonthEndReportDTO();
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mi_id;
            return delobj.getdata(data);
        }

        [HttpPost]
        [Route("getreport")]
        public MonthEndReportDTO getreport([FromBody] MonthEndReportDTO data123)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mi_id;
            return delobj.getreport(data123);
        }

        [Route("getyear")]
        public MonthEndReportDTO getyear()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            var aa = delobj.getyear(mi_id);
            MonthEndReportDTO cdto = aa;
            return cdto;

            //MonthEndReportDTO data = new MonthEndReportDTO();
            //int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_ID = mi_id;
            //return delobj.getyear(data);

        }

        [Route("Studdetails")]
        public MonthEndReportDTO Studdetails([FromBody] MonthEndReportDTO data123)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mi_id;
            return delobj.Studdetails(data123);
        }

        [Route("getbranch")]
        public MonthEndReportDTO getbranch([FromBody] MonthEndReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getbranch(data);
        }

        [Route("getsemester")]
        public MonthEndReportDTO getsemester([FromBody] MonthEndReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getsemester(data);
        }
    }
}
