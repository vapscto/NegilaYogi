using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class MC_121_IntDept_CourseController : Controller
    {
        public MC_121_IntDept_CourseDelegate _delobj = new MC_121_IntDept_CourseDelegate();

        [Route("loaddata/{id:int}")]
        public MC_121_IntDept_Course_DTO loaddata(int id)
        {
            MC_121_IntDept_Course_DTO data = new MC_121_IntDept_Course_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.loaddata(data);
        }


        [Route("savedata")]
        public MC_121_IntDept_Course_DTO savedata([FromBody] MC_121_IntDept_Course_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savedata(data);
        }

        [Route("editdata")]
        public MC_121_IntDept_Course_DTO editdata([FromBody] MC_121_IntDept_Course_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.editdata(data);
        }

        [Route("deactivY")]
        public MC_121_IntDept_Course_DTO deactivY([FromBody] MC_121_IntDept_Course_DTO data)
        {

            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.deactivY(data);
        }

        [Route("get_Course")]
        public MC_121_IntDept_Course_DTO get_Course([FromBody] MC_121_IntDept_Course_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.get_Course(data);
        }

        [Route("viewuploadflies")]
        public MC_121_IntDept_Course_DTO viewuploadflies([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public MC_121_IntDept_Course_DTO deleteuploadfile([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public MC_121_IntDept_Course_DTO savemedicaldatawisecomments([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public MC_121_IntDept_Course_DTO savefilewisecomments([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public MC_121_IntDept_Course_DTO getcomment([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public MC_121_IntDept_Course_DTO getfilecomment([FromBody] MC_121_IntDept_Course_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getfilecomment(data);
        }

    }
}
