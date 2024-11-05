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
    public class StudentProjectController : Controller
    {
        StudentProjectDelegate del = new StudentProjectDelegate();
        [Route("loaddata/{id:int}")]
        public StudentProject_DTO loaddata(int id)
        {
            StudentProject_DTO data = new StudentProject_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            return del.loaddata(data);

        }
        [Route("savedata")]
        public StudentProject_DTO savedata([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedata(data);
        }
        [Route("editdata")]
        public StudentProject_DTO editdata([FromBody] StudentProject_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.editdata(data);
        }
        [Route("deactiveStudent")]
        public StudentProject_DTO deactiveStudent([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactiveStudent(data);
        }
        
        [Route("get_branch")]
        public StudentProject_DTO get_branch([FromBody]StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_branch(data);
        }
        [Route("get_student")]
        public StudentProject_DTO get_student([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_student(data);
        }
        [Route("viewuploadflies")]
        public StudentProject_DTO viewuploadflies([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public StudentProject_DTO deleteuploadfile([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }



        [Route("MC_Savedata_134")]
        public StudentProject_DTO MC_Savedata_134([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.MC_Savedata_134(data);
        }
        [Route("MC_editdata_134")]
        public StudentProject_DTO MC_editdata_134([FromBody] StudentProject_DTO data)
        {          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.MC_editdata_134(data);
        }
        [Route("MC_viewuploadflies_134")]
        public StudentProject_DTO MC_viewuploadflies_134([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.MC_viewuploadflies_134(data);
        }
        [Route("MC_deleteuploadfile_134")]
        public StudentProject_DTO MC_deleteuploadfile_134([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.MC_deleteuploadfile_134(data);
        }



        [Route("savemedicaldatawisecomments")]
        public StudentProject_DTO savemedicaldatawisecomments([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public StudentProject_DTO savefilewisecomments([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public StudentProject_DTO getcomment([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public StudentProject_DTO getfilecomment([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }



        [Route("savedatawisecommentsAffi")]
        public StudentProject_DTO savedatawisecommentsAffi([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedatawisecommentsAffi(data);
        }

        [Route("savefilewisecommentsAffi")]
        public StudentProject_DTO savefilewisecommentsAffi([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecommentsAffi(data);
        }
        [Route("getcommentAffi")]
        public StudentProject_DTO getcommentAffi([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcommentAffi(data);
        }
        [Route("getfilecommentAffi")]
        public StudentProject_DTO getfilecommentAffi([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecommentAffi(data);
        }


        [Route("deactiveY")]
        public StudentProject_DTO deactiveY([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactiveY(data);
        }
        // addedbysanjeev
        [Route("saveadvance")]
        public StudentProject_DTO saveadvance([FromBody] StudentProject_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.saveadvance(data);
        }
    }
}
