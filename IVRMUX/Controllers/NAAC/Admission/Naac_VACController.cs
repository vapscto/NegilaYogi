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
    public class Naac_VACController : Controller
    {

        public Naac_VACDelegate _objdel = new Naac_VACDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_VAC_DTO loaddata(int id)
        {
            NAAC_AC_VAC_DTO data = new NAAC_AC_VAC_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.loaddata(data);
        }
        [Route("savedatatab1")]
        public NAAC_AC_VAC_DTO savedatatab1([FromBody] NAAC_AC_VAC_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedatatab1(data);
        }
        [Route("getcommentmaster")]
        public NAAC_AC_VAC_DTO getcommentmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcommentmaster(data);
        }
        [Route("savemedicaldatawisecommentsmaster")]
        public NAAC_AC_VAC_DTO savemedicaldatawisecommentsmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecommentsmaster(data);
        }
        [Route("savefilewisecommentsmaster")]
        public NAAC_AC_VAC_DTO savefilewisecommentsmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecommentsmaster(data);
        }
        
        [Route("getfilecommentmaster")]
        public NAAC_AC_VAC_DTO getfilecommentmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecommentmaster(data);
        }

        [Route("editTab1")]
        public NAAC_AC_VAC_DTO editTab1([FromBody] NAAC_AC_VAC_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editTab1(data);
        }

        [Route("deactivYTab1")]
        public NAAC_AC_VAC_DTO deactivYTab1([FromBody] NAAC_AC_VAC_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab1(data);
        }

        [Route("get_student")]
        public NAAC_AC_VAC_DTO get_student([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.get_student(data);
        }
        [Route("savedatatab2")]
        public NAAC_AC_VAC_DTO savedatatab2([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedatatab2(data);
        }
        [Route("getcomment")]
        public NAAC_AC_VAC_DTO getcomment([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_VAC_DTO getfilecomment([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_VAC_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_VAC_DTO savefilewisecomments([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("viewstudent")]
        public NAAC_AC_VAC_DTO viewstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.viewstudent(data);
        }
        [Route("deactivYTabstudent")]
        public NAAC_AC_VAC_DTO deactivYTabstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTabstudent(data);
        }

        [Route("edittab2")]
        public NAAC_AC_VAC_DTO edittab2([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.edittab2(data);
        }

        [Route("deactivYTab2")]
        public NAAC_AC_VAC_DTO deactivYTab2([FromBody] NAAC_AC_VAC_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab2(data);
        }

        [Route("get_Mappedstudentlist")]
        public NAAC_AC_VAC_DTO get_Mappedstudentlist([FromBody] NAAC_AC_VAC_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_Mappedstudentlist(data);
        }

        [Route("get_Continuedflagdata")]
        public NAAC_AC_VAC_DTO get_Continuedflagdata([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_Continuedflagdata(data);
        }
        [Route("saveContinued")]
        public NAAC_AC_VAC_DTO saveContinued([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveContinued(data);
        }
        [Route("get_Completedflagdata")]
        public NAAC_AC_VAC_DTO get_Completedflagdata([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_Completedflagdata(data);
        }
        [Route("saveCompletedflag")]
        public NAAC_AC_VAC_DTO saveCompletedflag([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveCompletedflag(data);
        }
        [Route("viewuploadfliesmain")]
        public NAAC_AC_VAC_DTO viewuploadfliesmain([FromBody] NAAC_AC_VAC_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadfliesmain(data);
        }
        [Route("deletemainfile")]
        public NAAC_AC_VAC_DTO deletemainfile([FromBody] NAAC_AC_VAC_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deletemainfile(data);
        }
        [Route("viewuploadfliesstudent")]
        public NAAC_AC_VAC_DTO viewuploadfliesstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadfliesstudent(data);
        }
        [Route("deletestudentfiles")]
        public NAAC_AC_VAC_DTO deletestudentfiles([FromBody] NAAC_AC_VAC_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deletestudentfiles(data);
        }
        //ADDED BY SANJEEV saveadvance
        [Route("saveadvance")]
        public NAAC_AC_VAC_DTO saveadvance([FromBody] NAAC_AC_VAC_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveadvance(data);
        }
    }
}
