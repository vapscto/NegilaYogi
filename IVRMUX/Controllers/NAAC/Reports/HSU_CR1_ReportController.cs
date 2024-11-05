using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class HSU_CR1_ReportController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_CR1_ReportDelegate objdel = new HSU_CR1_ReportDelegate();

        [Route("getdata/{id:int}")]
        public HSU_CR1_Report_DTO getdata(int id)
        {
            HSU_CR1_Report_DTO data = new HSU_CR1_Report_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }


        [Route("HSU_112_Report")]
        public HSU_CR1_Report_DTO HSU_112_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_112_Report(data);
        }
        [Route("HSU_132_133_Report")]
        public HSU_CR1_Report_DTO HSU_132_133_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_132_133_Report(data);
        }
        [Route("HSU_141_Report")]
        public HSU_CR1_Report_DTO HSU_141_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_141_Report(data);
        }
        [Route("HSU_142_Report")]
        public HSU_CR1_Report_DTO HSU_142_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_142_Report(data);
        }
        [Route("HSU_121_Report")]
        public HSU_CR1_Report_DTO HSU_121_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_121_Report(data);
        }
        [Route("HSU_122_Report")]
        public HSU_CR1_Report_DTO HSU_122_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_122_Report(data);
        }
        [Route("HSU_123_Report")]
        public HSU_CR1_Report_DTO HSU_123_Report([FromBody]HSU_CR1_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_123_Report(data);
        }
        
    }
}
