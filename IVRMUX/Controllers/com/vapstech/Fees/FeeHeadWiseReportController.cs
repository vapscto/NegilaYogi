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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeHeadWiseReportController : Controller
    {
        private static FacadeUrl _config;
        FeeHeadWiseReportDelegate clsdel = new FeeHeadWiseReportDelegate();
        private FacadeUrl fdu = new FacadeUrl();


        // GET: api/ClassCategoryReport
        [Route("getinitialfeedata")]
        public FeeHeadWiseReportDTO getinitialfeedata()
        {

            FeeHeadWiseReportDTO data = new FeeHeadWiseReportDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clsdel.getInitailData(data);
            
        }



        [HttpPost]
        // POST: api/ClassCategoryReport
       
        public FeeHeadWiseReportDTO saveData([FromBody] FeeHeadWiseReportDTO studentdata)
        {

            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            studentdata.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clsdel.SearchData(studentdata);
        }

        [HttpPost]
        [Route("getdata")]
        public FeeHeadWiseReportDTO getdata([FromBody] FeeHeadWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clsdel.getdata(data);
        }

        // Sudarshan 02-12-2023


        [HttpPost]
        [Route("getreport")]
        public FeeHeadWiseReportDTO getreport([FromBody] FeeHeadWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clsdel.getreport(data);
        }

    }
}
