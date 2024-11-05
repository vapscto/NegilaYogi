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
    public class ExamWiseTermReportController : Controller
    {

        ExamWiseTermReportDelegate crStr = new ExamWiseTermReportDelegate();


        [Route("Getdetails/{id:int}")]
        public ExamWiseTermReport_DTO Getdetails(int id)
        {
            ExamWiseTermReport_DTO data = new ExamWiseTermReport_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }
        
        [Route("onchangeyear")]
        public ExamWiseTermReport_DTO onchangeyear([FromBody] ExamWiseTermReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public ExamWiseTermReport_DTO onchangeclass([FromBody] ExamWiseTermReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeclass(data);
        }
        [Route("onchangesection")]
        public ExamWiseTermReport_DTO onchangesection([FromBody] ExamWiseTermReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesection(data);
        }





    }
}
