using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class StaffParticipationController : Controller
    {
        public StaffParticipationDelegate _objdel = new StaffParticipationDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_TParticipation_113_DTO loaddata(int id)
        {
            NAAC_AC_TParticipation_113_DTO data = new NAAC_AC_TParticipation_113_DTO();
            data.MI_Id = id;
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_AC_TParticipation_113_DTO savedata([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }

        [Route("editdata")]
        public NAAC_AC_TParticipation_113_DTO editdata([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_AC_TParticipation_113_DTO deactivY([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivY(data);
        }

        [Route("get_designation")]
        public NAAC_AC_TParticipation_113_DTO get_designation([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_designation(data);
        }

        [Route("get_emp")]
        public NAAC_AC_TParticipation_113_DTO get_emp([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_emp(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_TParticipation_113_DTO viewuploadflies([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_TParticipation_113_DTO deleteuploadfile([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deleteuploadfile(data);
        }



        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_TParticipation_113_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public NAAC_AC_TParticipation_113_DTO savefilewisecomments([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_TParticipation_113_DTO getcomment([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_TParticipation_113_DTO getfilecomment([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }
        //saveadvance
        [Route("saveadvance")]
        public NAAC_AC_TParticipation_113_DTO saveadvance([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveadvance(data);
        }
    }
}
