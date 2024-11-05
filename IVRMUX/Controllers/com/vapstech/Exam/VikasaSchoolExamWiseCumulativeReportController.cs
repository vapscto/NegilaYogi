using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaSchoolExamWiseCumulativeReportController : Controller
    {
        VikasaSchoolExamWiseCumulativeReportDelegates crStr = new VikasaSchoolExamWiseCumulativeReportDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/{id:int}")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(int id)
        {
            VikasaSubjectwiseCumulativeReportDTO data = new VikasaSubjectwiseCumulativeReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public VikasaSubjectwiseCumulativeReportDTO showdetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            return crStr.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_section(data);
        }
        [Route("get_Exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_Exam([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_Exam(data);
        }

        [Route("get_subject")]
        public VikasaSubjectwiseCumulativeReportDTO get_subject([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_subject(data);
        }

        [Route("savedetails")]
        public VikasaSubjectwiseCumulativeReportDTO savedetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }

    }
}
