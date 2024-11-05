
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
    public class HHSAllReportController : Controller
    {


        HHSAllReportDelegates crStr = new HHSAllReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public HHSAllReportDTO Getdetails(HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);            
        }


        [Route("savedetails")]
        public HHSAllReportDTO savedetails([FromBody] HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }
        [Route("yearchange")]
        public HHSAllReportDTO yearchange([FromBody]HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.yearchange(data);
        }
        [Route("classchange")]
        public HHSAllReportDTO classchange([FromBody]HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.classchange(data);
        }
        [Route("sectionchange")]
        public HHSAllReportDTO sectionchange([FromBody]HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.sectionchange(data);
        }
        [Route("getbbkvreport")]
        public HHSAllReportDTO getbbkvreport([FromBody]HHSAllReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getbbkvreport(data);
        }
    }
}
