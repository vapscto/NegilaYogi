using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Portals.IVRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Portals.IVRM
{
    [Route("api/[controller]")]
    public class Clg_IVRM_InteractionsController : Controller
    {
        Clg_IVRM_InteractionsDelegate delgte = new Clg_IVRM_InteractionsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public IVRM_School_InteractionsDTO getloaddata(int id)
        {
            IVRM_School_InteractionsDTO data = new IVRM_School_InteractionsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getloaddata(data);
        }


        [Route("getdetails")]
        public IVRM_School_InteractionsDTO getstaff([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getdetails(data);
        }

        [Route("Getbranch")]
        public IVRM_School_InteractionsDTO Getbranch([FromBody] IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.Getbranch(data);
        }

        [Route("Getsemester")]
        public IVRM_School_InteractionsDTO Getsemester([FromBody] IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.Getsemester(data);
        }
        [Route("Getsection")]
        public IVRM_School_InteractionsDTO Getsection([FromBody] IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.Getsection(data);
        }
        [Route("getstudent")]
        public IVRM_School_InteractionsDTO getstudent([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delgte.getstudent(data);
        }
        [Route("savedetails")]
        public IVRM_School_InteractionsDTO savedetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savedetails(data);
        }

        [Route("savereply")]
        public IVRM_School_InteractionsDTO savereply([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savereply(data);
        }

        [Route("reply")]
        public IVRM_School_InteractionsDTO reply([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.reply(data);
        }

        [Route("deletemsg")]
        public IVRM_School_InteractionsDTO deletemsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.deletemsg(data);
        }

        [Route("deleteinboxmsg")]
        public IVRM_School_InteractionsDTO deleteinboxmsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.deleteinboxmsg(data);
        }
    }
}
