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
    public class NaacExtnActivitiesController : Controller
    {
        NaacExtnActivitiesDelegate del = new NaacExtnActivitiesDelegate();

       [Route("loaddata/{id:int}")]
       public NAAC_AC_344_ExtnActivities_DTO loaddata(int id)
        {
            NAAC_AC_344_ExtnActivities_DTO data = new NAAC_AC_344_ExtnActivities_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
       
        [Route("get_branch")]
        public NAAC_AC_344_ExtnActivities_DTO get_branch([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_branch(data);
        }

        [Route("get_sems")]
        public NAAC_AC_344_ExtnActivities_DTO get_sems([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_sems(data);
        }

        [Route("get_section")]
        public NAAC_AC_344_ExtnActivities_DTO get_section([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_section(data);
        }

        [Route("GetStudentDetails")]
        public NAAC_AC_344_ExtnActivities_DTO GetStudentDetails([FromBody]NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GetStudentDetails(data);
        }
        [Route("saverecord")]
        public NAAC_AC_344_ExtnActivities_DTO saverecord([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saverecord(data);
        }
        [Route("getcomment")]
        public NAAC_AC_344_ExtnActivities_DTO getcomment([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_344_ExtnActivities_DTO getfilecomment([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_344_ExtnActivities_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_344_ExtnActivities_DTO savefilewisecomments([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }

        [Route("deactiveStudent")]
        public NAAC_AC_344_ExtnActivities_DTO deactiveStudent([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAAC_AC_344_ExtnActivities_DTO EditData([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }


        [Route("get_MappedStudent")]
        public NAAC_AC_344_ExtnActivities_DTO get_MappedStudent([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_MappedStudent(data);
        }

        [Route("deactive_student")]
        public NAAC_AC_344_ExtnActivities_DTO deactive_student([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_student(data);
        }

        [Route("viewdocument_MainActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_MainActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_MainActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_MainActUploadFiles(data);
        }

        [Route("viewdocument_StudentActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StudentActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_StudentActUploadFiles(data);
        }

        [Route("delete_StudentActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_StudentActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_StudentActUploadFiles(data);
        }

        [Route("get_Designation")]
        public NAAC_AC_344_ExtnActivities_DTO get_Designation([FromBody]NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Designation(data);
        }

        [Route("get_Employee")]
        public NAAC_AC_344_ExtnActivities_DTO get_Employee([FromBody]NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Employee(data);
        }

        [Route("viewdocument_StaffActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StaffActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_StaffActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_StaffActUploadFiles(data);
        }

        [Route("get_MappedStaff")]
        public NAAC_AC_344_ExtnActivities_DTO get_MappedStaff([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_MappedStaff(data);
        }

        [Route("deactive_staff")]
        public NAAC_AC_344_ExtnActivities_DTO deactive_staff([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_staff(data);
        }
    }
}
