
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamCalculationController : Controller
    {


        ExamCalculationDelegates crStr = new ExamCalculationDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public ExamReportDTO Getdetails(ExamReportDTO data)
        {
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }
        [HttpPost]
        [Route("get_cls_sections")]
        public ExamReportDTO get_cls_sections([FromBody] ExamReportDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_cls_sections(categorypage);

        }

        [Route("Calculation")]
        public ExamReportDTO Calculation([FromBody] ExamReportDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Calculation(categorypage);

        }
         
        [Route("getexam")]
        public ExamReportDTO getexam([FromBody] ExamReportDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getexam(categorypage);
        }
        [Route("getclass")]
        public ExamReportDTO getclass([FromBody] ExamReportDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getclass(categorypage);
        }
        [Route("saveapprove")]
        public ExamReportDTO saveapprove([FromBody] ExamReportDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.saveapprove(categorypage);
        }

    }

}
