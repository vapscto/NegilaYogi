using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports.Medical
{
    [Route("api/[controller]")]
    public class Medical_Criteria2ReportsController : Controller
    {
        public Medical_Criteria2ReportsDelegate objdel = new Medical_Criteria2ReportsDelegate();

        [Route("getdata/{id:int}")]
        public Medical_Criteria2Reports_DTO getdata(int id)
        {
            Medical_Criteria2Reports_DTO data = new Medical_Criteria2Reports_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.getdata(data);
        }
        [Route("MC_221_Report")]
        public Medical_Criteria2Reports_DTO MC_221_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_221_Report(data);
        }
        [Route("MC_254_Report")]
        public Medical_Criteria2Reports_DTO MC_254_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_254_Report(data);
        }
        [Route("MC_232_Report")]
        public Medical_Criteria2Reports_DTO MC_232_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_232_Report(data);
        }
        [Route("MC_212_Report")]
        public Medical_Criteria2Reports_DTO MC_212_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_212_Report(data);
        }
        [Route("MC_213_report")]
        public Medical_Criteria2Reports_DTO MC_213_report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_213_report(data);
        }
        [Route("MC_222_Report")]
        public Medical_Criteria2Reports_DTO MC_222_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_222_Report(data);
        }
        [Route("MC_234_Report")]
        public Medical_Criteria2Reports_DTO MC_234_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_234_Report(data);
        }
        [Route("MC_241_Report")]
        public Medical_Criteria2Reports_DTO MC_241_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_241_Report(data);
        }
        [Route("MC_242_Report")]
        public Medical_Criteria2Reports_DTO MC_242_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_242_Report(data);
        }
        [Route("MC_243_Report")]
        public Medical_Criteria2Reports_DTO MC_243_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_243_Report(data);
        }
        [Route("MC_244_Report")]
        public Medical_Criteria2Reports_DTO MC_244_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_244_Report(data);
        }
        [Route("MC_245_Report")]
        public Medical_Criteria2Reports_DTO MC_245_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_245_Report(data);
        }
        [Route("MC_262_Report")]
        public Medical_Criteria2Reports_DTO MC_262_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_262_Report(data);
        }
        [Route("MC_271_Report")]
        public Medical_Criteria2Reports_DTO MC_271_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_271_Report(data);
        }

    }
}
