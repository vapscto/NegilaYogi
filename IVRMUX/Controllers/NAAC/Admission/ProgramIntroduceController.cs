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
    public class ProgramIntroduceController : Controller
    {

        public ProgramIntroduceDelegate _objdel = new ProgramIntroduceDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_Programs_112_DTO loaddata(int id)
        {
            NAAC_AC_Programs_112_DTO data = new NAAC_AC_Programs_112_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public NAAC_AC_Programs_112_DTO savedata([FromBody] NAAC_AC_Programs_112_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }

        [Route("editdata")]
        public NAAC_AC_Programs_112_DTO editdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_AC_Programs_112_DTO deactivY([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivY(data);
        }
        [Route("get_Discontinuedflagdata")]
        public NAAC_AC_Programs_112_DTO get_Discontinuedflagdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_Discontinuedflagdata(data);
        }
        [Route("saveContinued")]
        public NAAC_AC_Programs_112_DTO saveContinued([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveContinued(data);
        }
        [Route("savemappingdata")]
        public NAAC_AC_Programs_112_DTO savemappingdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemappingdata(data);
        }
        [Route("deactivYTab2")]
        public NAAC_AC_Programs_112_DTO deactivYTab2([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab2(data);
        }
        [Route("edittab2")]
        public NAAC_AC_Programs_112_DTO edittab2([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.edittab2(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_Programs_112_DTO viewuploadflies([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_Programs_112_DTO deleteuploadfile([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }

        [Route("get_branch")]
        public NAAC_AC_Programs_112_DTO get_branch([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_branch(data);
        }

        [Route("get_program")]
        public NAAC_AC_Programs_112_DTO get_program([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_program(data);
        }
        [Route("get_Course")]
        public NAAC_AC_Programs_112_DTO get_Course([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_Course(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_Programs_112_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_Programs_112_DTO savefilewisecomments([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_Programs_112_DTO getcomment([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_Programs_112_DTO getfilecomment([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }
        //added by sanjeev
        [Route("saveadvance")]
        public NAAC_AC_Programs_112_DTO saveadvance([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveadvance(data);
        }
        
    }
}
