using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeDueDateReportController : Controller
    {
        private static FacadeUrl _config;
        FeeDueDateReportDelegate clsdel = new FeeDueDateReportDelegate();
        private FacadeUrl fdu = new FacadeUrl();


        // GET: api/ClassCategoryReport
        [Route("getinitialfeedata")]
        public FeeDueDateReportDTO getinitialfeedata()
        {
            FeeDueDateReportDTO data = new FeeDueDateReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clsdel.getInitailData(data);
        }



        [HttpPost]
        // POST: api/ClassCategoryReport

        public FeeDueDateReportDTO saveData([FromBody] FeeDueDateReportDTO studentdata)
        {

            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            studentdata.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clsdel.SearchData(studentdata);
        }

        [HttpPost]
        [Route("getdata")]
        public FeeDueDateReportDTO getdata([FromBody] FeeDueDateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clsdel.getdata(data);
        }

        //Income Report
        [HttpPost]
        [Route("getreport/")]
        public FeeDueDateReportDTO getreport([FromBody] FeeDueDateReportDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clsdel.getreport(MMD);
        }
    }
}
