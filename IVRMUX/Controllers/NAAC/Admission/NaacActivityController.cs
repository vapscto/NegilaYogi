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
    public class NaacActivityController : Controller
    {
        NaacActivityDelegate del = new NaacActivityDelegate();

        [Route("loaddata/{id:int}")]
        public NaacActivity_DTO loaddata(int id)
        {
            NaacActivity_DTO data = new NaacActivity_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("get_course")]
        public NaacActivity_DTO get_course([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_course(data);
        }

        [Route("get_branch")]
        public NaacActivity_DTO get_branch([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_branch(data);
        }

        [Route("get_sems")]
        public NaacActivity_DTO get_sems([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_sems(data);
        }

        [Route("get_section")]
        public NaacActivity_DTO get_section([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_section(data);
        }

        [Route("GetStudentDetails")]
        public NaacActivity_DTO GetStudentDetails([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GetStudentDetails(data);
        }

        [Route("get_Designation")]
        public NaacActivity_DTO get_Designation([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Designation(data);
        }

        [Route("get_Employee")]
        public NaacActivity_DTO get_Employee([FromBody]NaacActivity_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Employee(data);
        }

        [Route("saverecord")]
        public NaacActivity_DTO saverecord([FromBody]NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saverecord(data);
        }

        [Route("deactiveStudent")]
        public NaacActivity_DTO deactiveStudent([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactiveStudent(data);
        }

        [Route("EditData")]
        public NaacActivity_DTO EditData([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("get_MappedStudent")]
        public NaacActivity_DTO get_MappedStudent([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_MappedStudent(data);
        }

        [Route("get_MappedStaff")]
        public NaacActivity_DTO get_MappedStaff([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_MappedStaff(data);
        }

        [Route("deactive_student")]
        public NaacActivity_DTO deactive_student([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_student(data);
        }

        [Route("deactive_staff")]
        public NaacActivity_DTO deactive_staff([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_staff(data);
        }

        [Route("viewdocument_MainActUploadFiles")]
        public NaacActivity_DTO viewdocument_MainActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NaacActivity_DTO delete_MainActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_MainActUploadFiles(data);
        }

        [Route("viewdocument_StudentActUploadFiles")]
        public NaacActivity_DTO viewdocument_StudentActUploadFiles([FromBody] NaacActivity_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_StudentActUploadFiles(data);
        }

        [Route("delete_StudentActUploadFiles")]
        public NaacActivity_DTO delete_StudentActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_StudentActUploadFiles(data);
        }

        [Route("viewdocument_StaffActUploadFiles")]
        public NaacActivity_DTO viewdocument_StaffActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NaacActivity_DTO delete_StaffActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_StaffActUploadFiles(data);
        }

    }
}
