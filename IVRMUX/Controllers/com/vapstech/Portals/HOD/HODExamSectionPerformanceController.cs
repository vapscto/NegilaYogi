using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.HOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODExamSectionPerformanceController : Controller
    {

        public HODExamSectionPerformanceDelegate _delobj = new HODExamSectionPerformanceDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public HODExamSectionPerformance_DTO Getdetails(HODExamSectionPerformance_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.Getdetails(data);
        }

        [Route("getcategory")]
        public HODExamSectionPerformance_DTO getcategory([FromBody]HODExamSectionPerformance_DTO data)
        {
            //HODExamSectionPerformance_DTO data = new HODExamSectionPerformance_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getcategory(data);
        }

        [HttpPost]
        [Route("getclassexam")]
        public HODExamSectionPerformance_DTO getclassexam([FromBody] HODExamSectionPerformance_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getclassexam(data);
        }

        [HttpPost]
        [Route("showreport")]
        public HODExamSectionPerformance_DTO showreport([FromBody] HODExamSectionPerformance_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.showreport(data);
        }

        [HttpPost]
        [Route("getsubject")]
        public HODExamSectionPerformance_DTO getsubject([FromBody] HODExamSectionPerformance_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getsubject(data);

        }

    }
}
