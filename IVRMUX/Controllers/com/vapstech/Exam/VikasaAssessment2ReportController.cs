using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaAssessment2ReportController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        VikasaAssessment2ReportDelegates crStr = new VikasaAssessment2ReportDelegates();
        
        [Route("Getdetails/{id:int}")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(int id)
        {
            VikasaSubjectwiseCumulativeReportDTO data = new VikasaSubjectwiseCumulativeReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return crStr.Getdetails(data);
        }

        [Route("savedetails")]
        public VikasaSubjectwiseCumulativeReportDTO savedetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {          
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;           
            return crStr.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_section(data);
        }
       
        [Route("get_exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_exam([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        { 
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_exam(data);
        }
        [Route("getcategory")]
        public VikasaSubjectwiseCumulativeReportDTO getcategory([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.getcategory(data);
        }
    }
}