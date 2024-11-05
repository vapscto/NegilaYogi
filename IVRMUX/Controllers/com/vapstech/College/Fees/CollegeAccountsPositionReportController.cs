using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Reports
{
    [Route("api/[controller]")]
    public class CollegeAccountsPositionReportController : Controller
    {
        CollegeAccountsPositionReportDelegate del = new CollegeAccountsPositionReportDelegate();
      
        [Route("loaddata/{id:int}")]
        public CollegeConcessionDTO loaddata(int id)
        {
            CollegeConcessionDTO data = new CollegeConcessionDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("getgroupByCG")]
        public CollegeConcessionDTO getgroupByCG([FromBody] CollegeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getgroupByCG(data);
        }
        [Route("getReport")]
        public CollegeConcessionDTO getReport([FromBody] CollegeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getReport(data);
        }

    }
}
