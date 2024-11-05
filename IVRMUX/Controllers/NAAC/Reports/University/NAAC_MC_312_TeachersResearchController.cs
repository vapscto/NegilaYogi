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
    public class NAAC_MC_312_TeachersResearchController : Controller
    {
        public NAAC_MC_312_TeachersResearchDelegate objdel = new NAAC_MC_312_TeachersResearchDelegate();

        [Route("getdata/{id:int}")]
        public UC_312_TeachersResearchDTO getdata(int id)
        {
            UC_312_TeachersResearchDTO data = new UC_312_TeachersResearchDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }


        [Route("get_312U_report")]
        public UC_312_TeachersResearchDTO get_312U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_312U_report(data);
        }
        [Route("get_313U_report")]
        public UC_312_TeachersResearchDTO get_313U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_313U_report(data);
        }
        [Route("get_314U_report")]
        public UC_312_TeachersResearchDTO get_314U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_314U_report(data);
        }
        [Route("get_315U_report")]
        public UC_312_TeachersResearchDTO get_315U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_315U_report(data);
        }
        [Route("get_316U_report")]
        public UC_312_TeachersResearchDTO get_316U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_316U_report(data);
        }
        [Route("get_342U_report")]
        public UC_312_TeachersResearchDTO get_342U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_342U_report(data);
        }
        [Route("get_343U_report")]
        public UC_312_TeachersResearchDTO get_343U_report([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_343U_report(data);
        }
        [Route("get_372U_report")]
        public UC_312_TeachersResearchDTO get_372U_report([FromBody]UC_312_TeachersResearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_372U_report(data);
        }
        [Route("get_362U_report")]
        public UC_312_TeachersResearchDTO get_362U_report([FromBody]UC_312_TeachersResearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_362U_report(data);
        }
        [Route("get_352U_report")]
        public UC_312_TeachersResearchDTO get_352U_report([FromBody]UC_312_TeachersResearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_352U_report(data);
        }
        [Route("get_371U_report")]
        public UC_312_TeachersResearchDTO get_371U_report([FromBody]UC_312_TeachersResearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_371U_report(data);
        }
        [Route("get_341U_report")]
        public UC_312_TeachersResearchDTO get_341U_report([FromBody]UC_312_TeachersResearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_341U_report(data);
        }
        [Route("NAAC_MC_345_TeachersResearchReport")]
        public UC_312_TeachersResearchDTO NAAC_MC_345_TeachersResearchReport([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.NAAC_MC_345_TeachersResearchReport(data);
        }
        [Route("NAAC_MC_346_TeachersResearchReport")]
        public UC_312_TeachersResearchDTO NAAC_MC_346_TeachersResearchReport([FromBody]UC_312_TeachersResearchDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.NAAC_MC_346_TeachersResearchReport(data);
        }
    }
}
