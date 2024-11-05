using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeAccountsPositionController : Controller
    {
        FeeAccountsPositionDelegate del = new FeeAccountsPositionDelegate();
        [Route("loaddata/{id:int}")]
        public FeeAccountsPositionReportDTO loaddata(int id)
        {
            FeeAccountsPositionReportDTO data = new FeeAccountsPositionReportDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("getgroupByCG")]
        public FeeAccountsPositionReportDTO getgroupByCG([FromBody] FeeAccountsPositionReportDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getgroupByCG(data);
        }
        [Route("getReport")]
        public FeeAccountsPositionReportDTO getReport([FromBody] FeeAccountsPositionReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getReport(data);
        }

    }
}
