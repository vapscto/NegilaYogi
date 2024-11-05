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
    public class VikasaFinalClasswisecumulativeController : Controller
    {
        VikasaFinalClasswisecumulativeDelegate _delg = new VikasaFinalClasswisecumulativeDelegate();

       
        [Route("Getdetails")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }


        [Route("showdetails")]
        public VikasaSubjectwiseCumulativeReportDTO showdetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delg.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {           
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delg.get_section(data);
        }
        
        [Route("get_subject")]
        public VikasaSubjectwiseCumulativeReportDTO get_subject([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {           
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delg.get_subject(data);
        }
        [Route("get_category")]
        public VikasaSubjectwiseCumulativeReportDTO get_category([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delg.get_category(data);
        }
        [Route("get_subject_group")]
        public VikasaSubjectwiseCumulativeReportDTO get_subject_group([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delg.get_subject_group(data);
        }
        
    }
}
