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
    public class Medical_Criteria3ReportsController : Controller
    {
        public Medical_Criteria3ReportsDelegate objdel = new Medical_Criteria3ReportsDelegate();

        [Route("getdata/{id:int}")]
        public Medical_Criteria3Reports_DTO getdata(int id)
        {
            Medical_Criteria3Reports_DTO data = new Medical_Criteria3Reports_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.getdata(data);
        }

        [Route("MC_311_Report")]
        public Medical_Criteria3Reports_DTO MC_311_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_311_Report(data);
        }
        [Route("MC_312_Report")]
        public Medical_Criteria3Reports_DTO MC_312_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_312_Report(data);
        }
        [Route("MC_313_Report")]
        public Medical_Criteria3Reports_DTO MC_313_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_313_Report(data);
        }
        [Route("MC_322_Report")]
        public Medical_Criteria3Reports_DTO MC_322_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_322_Report(data);
        }
        [Route("MC_331_report")]
        public Medical_Criteria3Reports_DTO MC_331_report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_331_report(data);
        }
        [Route("MC_332_Report")]
        public Medical_Criteria3Reports_DTO MC_332_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_332_Report(data);
        }
        [Route("MC_333_Report")]
        public Medical_Criteria3Reports_DTO MC_333_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_333_Report(data);
        }
        [Route("MC_334_Report")]
        public Medical_Criteria3Reports_DTO MC_334_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_334_Report(data);
        }
        [Route("MC_341_Report")]
        public Medical_Criteria3Reports_DTO MC_341_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_341_Report(data);
        }
        [Route("MC_342_Report")]
        public Medical_Criteria3Reports_DTO MC_342_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_342_Report(data);
        }
        [Route("MC_351_Report")]
        public Medical_Criteria3Reports_DTO MC_351_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_351_Report(data);
        }
        [Route("MC_352_Report")]
        public Medical_Criteria3Reports_DTO MC_352_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_352_Report(data);
        }
        
    }
}
