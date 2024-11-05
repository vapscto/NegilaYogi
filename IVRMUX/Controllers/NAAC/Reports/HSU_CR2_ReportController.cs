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
    public class HSU_CR2_ReportController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_CR2_ReportDelegate objdel = new HSU_CR2_ReportDelegate();

        [Route("getdata/{id:int}")]
        public HSU_CR2_Report_DTO getdata(int id)
        {
            HSU_CR2_Report_DTO data = new HSU_CR2_Report_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }
        [Route("HSU_211_Report")]
        public HSU_CR2_Report_DTO HSU_211_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_211_Report(data);
        }
        [Route("HSU_212_Report")]
        public HSU_CR2_Report_DTO HSU_212_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_212_Report(data);
        }
        [Route("HSU_213_Report")]
        public HSU_CR2_Report_DTO HSU_213_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_213_Report(data);
        }
        [Route("HSU_221_Report")]
        public HSU_CR2_Report_DTO HSU_221_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_221_Report(data);
        }
        [Route("HSU_222_Report")]
        public HSU_CR2_Report_DTO HSU_222_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_222_Report(data);
        }
        [Route("HSU_232_Report")]
        public HSU_CR2_Report_DTO HSU_232_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_232_Report(data);
        }
        [Route("HSU_234_Report")]
        public HSU_CR2_Report_DTO HSU_234_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_234_Report(data);
        }
            [Route("HSU_241_Report")]
        public HSU_CR2_Report_DTO HSU_241_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_241_Report(data);
        }
        [Route("HSU_242_Report")]
        public HSU_CR2_Report_DTO HSU_242_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_242_Report(data);
        }
        [Route("HSU_243_Report")]
        public HSU_CR2_Report_DTO HSU_243_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_243_Report(data);
        }
        [Route("HSU_244_Report")]
        public HSU_CR2_Report_DTO HSU_244_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_244_Report(data);
        }
        [Route("HSU_245_Report")]
        public HSU_CR2_Report_DTO HSU_245_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_245_Report(data);
        }
        [Route("HSU_251_Report")]
        public HSU_CR2_Report_DTO HSU_251_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_251_Report(data);
        }
        [Route("HSU_252_Report")]
        public HSU_CR2_Report_DTO HSU_252_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_252_Report(data);
        }
        [Route("HSU_253_Report")]
        public HSU_CR2_Report_DTO HSU_253_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_253_Report(data);
        }
        [Route("HSU_255_Report")]
        public HSU_CR2_Report_DTO HSU_255_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_255_Report(data);
        }
        [Route("HSU_262_Report")]
        public HSU_CR2_Report_DTO HSU_262_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_262_Report(data);
        }
        [Route("HSU_271_Report")]
        public HSU_CR2_Report_DTO HSU_271_Report([FromBody]HSU_CR2_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU_271_Report(data);
        }
    }
   
}
