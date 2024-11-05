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
    public class MaldaProgressReportExamController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        MaldaProgressReportExamDelegate crStr = new MaldaProgressReportExamDelegate();


        [Route("Getdetails/{id:int}")]
        public MaldaProgressReportExam_DTO Getdetails(int id)
        {
           MaldaProgressReportExam_DTO data = new MaldaProgressReportExam_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("savedetails")]
        public MaldaProgressReportExam_DTO savedetails([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.showdetails(data);
        }


        [Route("onchangeyear")]
        public MaldaProgressReportExam_DTO onchangeyear([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public MaldaProgressReportExam_DTO onchangeclass([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeclass(data);
        }
        [Route("onchangesection")]
        public MaldaProgressReportExam_DTO onchangesection([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesection(data);
        }
        [Route("getreportpromotion")]
        public MaldaProgressReportExam_DTO getreportpromotion([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getreportpromotion(data);
        }
        [Route("ixpromotionreport")]
        public MaldaProgressReportExam_DTO ixpromotionreport([FromBody] MaldaProgressReportExam_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.ixpromotionreport(data);
        }   

    }
}
