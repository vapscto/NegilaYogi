using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission.Criteria8;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class Naac_M_811StudentsEnrolledInProgrammeController : Controller
    {
        public Naac_M_811StudentsEnrolledInProgrammeDelegate objdel = new Naac_M_811StudentsEnrolledInProgrammeDelegate();

        [Route("getdata/{id:int}")]
        public NAAC_811MC_NEET_DTO getdata(int id)
        {
            NAAC_811MC_NEET_DTO data = new NAAC_811MC_NEET_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }


        [Route("get_811M_report")]
        public NAAC_811MC_NEET_DTO get_811M_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_811M_report(data);
        }
        [Route("get_813M_report")]
        public NAAC_811MC_NEET_DTO get_813M_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_813M_report(data);
        }
        [Route("get_819M_report")]
        public NAAC_811MC_NEET_DTO get_819M_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_819M_report(data);
        }
        [Route("get_8110M_report")]
        public NAAC_811MC_NEET_DTO get_8110M_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_8110M_report(data);
        }
        [Route("get_813D_report")]
        public NAAC_811MC_NEET_DTO get_813D_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_813D_report(data);
        }
        [Route("get_815D_report")]
        public NAAC_811MC_NEET_DTO get_815D_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_815D_report(data);
        }
        [Route("get_816D_report")]
        public NAAC_811MC_NEET_DTO get_816D_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_816D_report(data);
        }
        //[Route("get_817D_report")]
        //public NAAC_811MC_NEET_DTO get_817D_report([FromBody]NAAC_811MC_NEET_DTO data)
        //{

        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return objdel.get_817D_report(data);
        //}
        [Route("get_8111D_report")]
        public NAAC_811MC_NEET_DTO get_8111D_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_8111D_report(data);
        }
        [Route("get_818N_report")]
        public NAAC_811MC_NEET_DTO get_818N_report([FromBody]NAAC_811MC_NEET_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_818N_report(data);
        }
    }
}
