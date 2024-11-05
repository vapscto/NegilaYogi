
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
    public class SNSPROGRESSCARDReportController : Controller
    {


        SNSPROGRESSCARDReportDelegates crStr = new SNSPROGRESSCARDReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public SNSPROGRESSCARDReportDTO Getdetails(SNSPROGRESSCARDReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);            
        }


        [Route("savedetails")]
        public SNSPROGRESSCARDReportDTO savedetails([FromBody] SNSPROGRESSCARDReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }
        [Route("yearchange")]
        public SNSPROGRESSCARDReportDTO yearchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.yearchange(data);
        }
        [Route("classchange")]
        public SNSPROGRESSCARDReportDTO classchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.classchange(data);
        }
        [Route("sectionchange")]
        public SNSPROGRESSCARDReportDTO sectionchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.sectionchange(data);
        }


    }

}
