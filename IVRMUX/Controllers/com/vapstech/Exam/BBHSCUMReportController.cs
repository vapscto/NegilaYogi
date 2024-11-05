
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
    [Route("api/[controller]")]
    public class BBHSCUMReportController : Controller
    {


        BBHSCUMReportDelegates crStr = new BBHSCUMReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
      
        [HttpGet]
        [Route("Getdetails")]
        public BBHSCUMReportDTO Getdetails(BBHSCUMReportDTO data)
         {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.MI_Id = 5;
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public BBHSCUMReportDTO savedetails([FromBody] BBHSCUMReportDTO data)
        {
            // data.MI_Id = 5;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);

        }

        [Route("getclass/{id}")]
        public BBHSCUMReportDTO getclass(int id)
        {
            BBHSCUMReportDTO data = new BBHSCUMReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            //data.MI_Id = 5;
            return crStr.getclass(data);
        }
        [HttpPost]
        [Route("Getsection")]
        public BBHSCUMReportDTO Getsection([FromBody] BBHSCUMReportDTO data)
        {
            // data.MI_Id = 5;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);

        }

        [HttpPost]
        [Route("GetAttendence")]
        public BBHSCUMReportDTO GetAttendence([FromBody] BBHSCUMReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = 5;
            return crStr.GetAttendence(data);

        }
        //[HttpPost]
        //[Route("GetIndividualAttendence")]
        //public BBHSCUMReportDTO GetIndividualAttendence([FromBody] BBHSCUMReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    // data.MI_Id = 5;
        //    return crStr.GetIndividualAttendence(data);

        //}

    }

}
