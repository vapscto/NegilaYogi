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
    public class NAACCriteriaFiveReportController : Controller
    {

        public NAACCriteriaFiveReportDelegate objdel = new NAACCriteriaFiveReportDelegate();

        [Route("getdata/{id:int}")]
        public NAACCriteriaFiveReportDTO getdata(int id)
        {
            NAACCriteriaFiveReportDTO data = new NAACCriteriaFiveReportDTO();
            data.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.getdata(data);
        }


        [Route("get_report")]
        public NAACCriteriaFiveReportDTO get_report([FromBody]NAACCriteriaFiveReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report(data);
        }
        [Route("HSU511")]
        public NAACCriteriaFiveReportDTO HSU511([FromBody]NAACCriteriaFiveReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.HSU511(data);
        }

        [Route("get_report513")]
        public NAACCriteriaFiveReportDTO get_report513([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report513(data);
        }
        [Route("get_report514")]
        public NAACCriteriaFiveReportDTO get_report514([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report514(data);
        }
        [Route("get_report513med")]
        public NAACCriteriaFiveReportDTO get_report513med([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report513med(data);
        }
        [Route("get_report516")]
        public NAACCriteriaFiveReportDTO get_report516([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report516(data);
        }
        [Route("get_report515med")]
        public NAACCriteriaFiveReportDTO get_report515med([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report515med(data);
        }
          [Route("get_report521")]
        public NAACCriteriaFiveReportDTO get_report521([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report521(data);
        }
        [Route("get_report522")]
        public NAACCriteriaFiveReportDTO get_report522([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report522(data);
        }
          [Route("get_report531")]
        public NAACCriteriaFiveReportDTO get_report531([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report531(data);
        }
        [Route("get_report533")]
        public NAACCriteriaFiveReportDTO get_report533([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report533(data);
        }
         [Route("get_report542")]
        public NAACCriteriaFiveReportDTO get_report542([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report542(data);
        }

        [Route("get_report542HSU")]
        public NAACCriteriaFiveReportDTO get_report542HSU([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report542HSU(data);
        }
        
         [Route("get_report543")]
        public NAACCriteriaFiveReportDTO get_report543([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report543(data);
        }
        
         [Route("get_report523")]
        public NAACCriteriaFiveReportDTO get_report523([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report523(data);
        }


        
        
         [Route("get_report515")]
        public NAACCriteriaFiveReportDTO get_report515([FromBody]NAACCriteriaFiveReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report515(data);
        }





    }
}
