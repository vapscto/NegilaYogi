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
    public class StudentParticipationController : Controller
    {
        public StudentParticipationDelegate _objdel = new StudentParticipationDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_SParticipation_123_Students_DTO loaddata(int id)
        {
            NAAC_AC_SParticipation_123_Students_DTO data = new NAAC_AC_SParticipation_123_Students_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_AC_SParticipation_123_Students_DTO savedata([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }

        [Route("editdata")]
        public NAAC_AC_SParticipation_123_Students_DTO editdata([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_AC_SParticipation_123_Students_DTO deactivY([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {

            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivY(data);
        }

        [Route("get_branch")]
        public NAAC_AC_SParticipation_123_Students_DTO get_branch([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_branch(data);
        }

        [Route("get_student")]
        public NAAC_AC_SParticipation_123_Students_DTO get_student([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_student(data);
        }

        [Route("get_MappedStudentList")]
        public NAAC_AC_SParticipation_123_Students_DTO get_MappedStudentList([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_MappedStudentList(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_SParticipation_123_Students_DTO viewuploadflies([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_SParticipation_123_Students_DTO deleteuploadfile([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deleteuploadfile(data);
        }
        [Route("get_coursebrnch")]
        public NAAC_AC_SParticipation_123_Students_DTO get_coursebrnch([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));           
            return _objdel.get_coursebrnch(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_SParticipation_123_Students_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public NAAC_AC_SParticipation_123_Students_DTO savefilewisecomments([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_SParticipation_123_Students_DTO getcomment([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_SParticipation_123_Students_DTO getfilecomment([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }
    }
}
