using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class PromotionReportDetailsController : Controller
    {
        PromotionReportDetailsDelegate del = new PromotionReportDetailsDelegate();

        [Route("getdata/{id:int}")]
        public PromotionReportDetailsDTO getdata(int id)
        {
            PromotionReportDetailsDTO data = new PromotionReportDetailsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }

        [Route("onchangeyear")]
        public PromotionReportDetailsDTO onchangeyear([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public PromotionReportDetailsDTO onchangeclass([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangeclass(data);
        }

        [Route("onchangesection")]
        public PromotionReportDetailsDTO onchangesection([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangesection(data);
        }

        [Route("Report")]
        public PromotionReportDetailsDTO Report([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Report(data);
        }

        [Route("getpromotioncumulativereport")]
        public PromotionReportDetailsDTO getpromotioncumulativereport([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getpromotioncumulativereport(data);
        }

        [Route("getpromotioncumulativereport_format2")]
        public PromotionReportDetailsDTO getpromotioncumulativereport_format2([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getpromotioncumulativereport_format2(data);
        }

        [Route("onpageload/{id:int}")]
        public PromotionReportDetailsDTO onpageload(int id)
        {
            PromotionReportDetailsDTO data = new PromotionReportDetailsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onpageload(data);
        }

        [Route("PromotionPerformanceReport")]
        public PromotionReportDetailsDTO PromotionPerformanceReport([FromBody] PromotionReportDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.PromotionPerformanceReport(data);
        }
    }
}