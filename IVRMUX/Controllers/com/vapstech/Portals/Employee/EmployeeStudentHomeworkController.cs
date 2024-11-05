using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeStudentHomeworkController : Controller
    {
        EmployeeStudentHomeworkDelegate _notic = new EmployeeStudentHomeworkDelegate();
        // GET: api/values
        [Route("savedetail")]
        public IVRM_Homework_DTO savedetail([FromBody]IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public IVRM_Homework_DTO Getdetails(int id)
        {
            IVRM_Homework_DTO data = new IVRM_Homework_DTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_Homework_DTO deactivate([FromBody]IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _notic.deactivate(data);
        }
        [Route("get_classes")]
        public IVRM_Homework_DTO get_classes([FromBody]IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //      data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_Homework_DTO getsectiondata([FromBody]IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //   data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_Homework_DTO getsubject([FromBody]IVRM_Homework_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //    data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.getsubject(data);
        }

        [Route("editData")]
        public IVRM_Homework_DTO editData([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //     data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.editData(data);
        }
        [Route("viewData")]
        public IVRM_Homework_DTO viewData([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //    data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.viewData(data);
        }
        //===================Home work marks enter=========


        [Route("gethomework_student")]
        public IVRM_Homework_DTO gethomework_student([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.gethomework_student(data);
        }

        [Route("gethomework_list")]
        public IVRM_Homework_DTO gethomework_list([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.gethomework_list(data);
        }

        [Route("getsubjectlist")]
        public IVRM_Homework_DTO getsubjectlist([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.getsubjectlist(data);
        }

        [Route("homework_marks_update")]
        public IVRM_Homework_DTO homework_marks_update([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.homework_marks_update(data);
        }
        [Route("edit_homework_mark")]
        public IVRM_Homework_DTO edit_homework_mark([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.edit_homework_mark(data);
        }
        [Route("viewhomework")]
        public IVRM_Homework_DTO viewhomework([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.viewhomework(data);
        }
        [Route("viewstudentupload")]
        public IVRM_Homework_DTO viewstudentupload([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.viewstudentupload(data);
        }
        [Route("stfupload")]
        public IVRM_Homework_DTO stfupload([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.stfupload(data);
        }
        [Route("gethomework_listTopic")]
        public IVRM_Homework_DTO gethomework_listTopic([FromBody] IVRM_Homework_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.gethomework_listTopic(data);
        }
        //[Route("callnotification")]
        //public IVRM_Homework_DTO callnotification([FromBody] IVRM_Homework_DTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _notic.callnotification(data);
        //}
    }
}
