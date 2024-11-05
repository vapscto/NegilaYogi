using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SRKVSReport : Controller
    {

        SRKVSSportsReportDelagte crStr = new SRKVSSportsReportDelagte();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public SRKVSSportsReportDTO Getdetails(SRKVSSportsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public SRKVSSportsReportDTO showdetails([FromBody] SRKVSSportsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public SRKVSSportsReportDTO get_class([FromBody]SRKVSSportsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_class(data);
        }


        [Route("get_classs")]
        public SRKVSSportsReportDTO get_classs([FromBody]SRKVSSportsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_classs(data);
        }

        [Route("get_section")]
        public SRKVSSportsReportDTO get_section([FromBody]SRKVSSportsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_section(data);
        }

        [Route("get_student")]
        public SRKVSSportsReportDTO get_student([FromBody]SRKVSSportsReportDTO data)

        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_student(data);
        }
    }
}
