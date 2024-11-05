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
    public class ExamPromotionReportController : Controller
    {

        public ExamPromotionReportDelegate _delegate = new ExamPromotionReportDelegate();

        [Route("Getdetails/{id:int}")]
        public ExamPromotionReport_DTO Getdetails(int id)
        {
            ExamPromotionReport_DTO data = new ExamPromotionReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delegate.Getdetails(data);
        }

        [Route("get_class")]
        public ExamPromotionReport_DTO get_class([FromBody]ExamPromotionReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delegate.get_class(data);
        }
        [Route("get_section")]
        public ExamPromotionReport_DTO get_section([FromBody]ExamPromotionReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delegate.get_section(data);
        }
        [Route("get_exam")]
        public ExamPromotionReport_DTO get_exam([FromBody]ExamPromotionReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delegate.get_exam(data);
        }
        [Route("get_reports")]
        public ExamPromotionReport_DTO get_reports([FromBody]ExamPromotionReport_DTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delegate.get_reports(data);
        }
    }
}
