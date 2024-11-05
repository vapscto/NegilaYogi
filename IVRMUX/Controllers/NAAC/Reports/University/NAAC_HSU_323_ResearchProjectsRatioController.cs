using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports.University
{
    [Route("api/[controller]")]
    public class NAAC_HSU_323_ResearchProjectsRatioController : Controller
    {
        public NAAC_HSU_323_ResearchProjectsRatioDelegate objdel = new NAAC_HSU_323_ResearchProjectsRatioDelegate();

        [Route("getdata/{id:int}")]
        public HSU_323_ResearchProjectsRatioDTO getdata(int id)
        {
            HSU_323_ResearchProjectsRatioDTO data = new HSU_323_ResearchProjectsRatioDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }
        [Route("get_323U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_323U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_323U_report(data);
        }
        [Route("get_334U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_334U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_334U_report(data);
        }
        [Route("get_344U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_344U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_344U_report(data);
        }
        [Route("get_333U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_333U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_333U_report(data);
        }
        [Route("get_349U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_349U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_349U_report(data);
        }
        [Route("get_348U_report")]
        public HSU_323_ResearchProjectsRatioDTO get_348U_report([FromBody]HSU_323_ResearchProjectsRatioDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_348U_report(data);
        }
    }
}
