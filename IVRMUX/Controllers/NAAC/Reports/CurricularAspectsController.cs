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
    public class CurricularAspectsController : Controller
    {

        public CurricularAspectsDelegate objdel = new CurricularAspectsDelegate();

        [Route("getdata/{id:int}")]
        public CurricularAspects_DTO getdata(int id)
        {
            CurricularAspects_DTO data = new CurricularAspects_DTO();
            data.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }


        [Route("get_report")]
        public CurricularAspects_DTO get_report([FromBody]CurricularAspects_DTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report(data);
        }


        [Route("get_nCourse_report")]
        public CurricularAspects_DTO get_nCourse_report([FromBody]CurricularAspects_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_nCourse_report(data);
        }

        [Route("get_report_113")]
        public CurricularAspects_DTO get_report_113([FromBody]CurricularAspects_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report_113(data);
        }

        [Route("get_report_123")]
        public CurricularAspects_DTO get_report_123([FromBody]CurricularAspects_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report_123(data);
        }

        [Route("get_report_133")]
        public CurricularAspects_DTO get_report_133([FromBody]CurricularAspects_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report_133(data);
        }

        [Route("get_report_132")]
        public CurricularAspects_DTO get_report_132([FromBody]CurricularAspects_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report_132(data);
        }

        [Route("get_122CBCSsystemReport")]
        public CurricularAspects_DTO get_122CBCSsystemReport([FromBody]CurricularAspects_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_122CBCSsystemReport(data);
        }

        

    }
}
