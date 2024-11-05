using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PromotionReportDetailsFacade : Controller
    {

        public PromotionReportDetailsInterface inter;
        public PromotionReportDetailsFacade(PromotionReportDetailsInterface y)
        {
            inter = y;
        }

        [Route("getdata")]
        public PromotionReportDetailsDTO getdata([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.getdata(data);
        }

        [Route("onchangeyear")]
        public PromotionReportDetailsDTO onchangeyear([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public PromotionReportDetailsDTO onchangeclass([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.onchangeclass(data);
        }        

        [Route("onchangesection")]
        public PromotionReportDetailsDTO onchangesection([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.onchangesection(data);
        }

        [Route("Report")]
        public PromotionReportDetailsDTO Report([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.Report(data);
        }

        [Route("getpromotioncumulativereport")]
        public PromotionReportDetailsDTO getpromotioncumulativereport([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.getpromotioncumulativereport(data);
        }

        [Route("getpromotioncumulativereport_format2")]
        public PromotionReportDetailsDTO getpromotioncumulativereport_format2([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.getpromotioncumulativereport_format2(data);
        }

        [Route("onpageload")]
        public PromotionReportDetailsDTO onpageload([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.onpageload(data);
        }

        [Route("PromotionPerformanceReport")]
        public PromotionReportDetailsDTO PromotionPerformanceReport([FromBody] PromotionReportDetailsDTO data)
        {
            return inter.PromotionPerformanceReport(data);
        }

    }
}
