using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentDashboardController : Controller
    {
        StudentDashboardDelegate sdd = new StudentDashboardDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public StudentDashboardDTO Getdetails(StudentDashboardDTO sddto)
        {
            // StudentDashboardDTO sddto = new StudentDashboardDTO();
            sddto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            sddto.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            sddto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            sddto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            sddto.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            sddto.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            sddto.stuonlineexam = Convert.ToInt32(HttpContext.Session.GetInt32("Student_OnlineExam"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            HttpContext.Session.SetInt32("Student_OnlineExam", 1);
            return sdd.Getdetails(sddto);
        }

        [Route("getImages")]
        public StudentDashboardDTO getImages([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.getImages(data);
        }

        [Route("saverequest")]
        public StudentDashboardDTO saverequest([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.saverequest(data);
        }

        [Route("saveakpkfile")]
        public StudentDashboardDTO saveakpkfile([FromBody]StudentDashboardDTO data)
        {
            // StudentDashboardDTO data = new StudentDashboardDTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return sdd.saveakpkfile(data);
        }

        [Route("viewData")]
        public StudentDashboardDTO viewData([FromBody]StudentDashboardDTO data)
        {


            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return sdd.viewData(data);
        }

        [Route("conformdata")]
        public StudentDashboardDTO conformdata([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.conformdata(data);
        }

        [Route("savecls_doc")]
        public StudentDashboardDTO savecls_doc([FromBody]StudentDashboardDTO data)
        {


            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return sdd.savecls_doc(data);
        }

        [Route("savehome_doc")]
        public StudentDashboardDTO savehome_doc([FromBody]StudentDashboardDTO data)
        {


            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return sdd.savehome_doc(data);
        }

        [Route("viewData_doc")]
        public StudentDashboardDTO viewData_doc([FromBody]StudentDashboardDTO data)
        {


            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return sdd.viewData_doc(data);
        }

        [Route("viewnotice")]
        public StudentDashboardDTO viewnotice([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.viewnotice(data);
        }

        [Route("onclick_notice")]
        public StudentDashboardDTO onclick_notice([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }           
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_notice(data);
        }

        [Route("onclick_TT")]
        public StudentDashboardDTO onclick_TT([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
             
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_TT(data);
        }

        [Route("onclick_syllabus")]
        public StudentDashboardDTO onclick_syllabus([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_syllabus(data);
        }

        [Route("onclick_LIB")]
        public StudentDashboardDTO onclick_LIB([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_LIB(data);
        }

        [Route("onclick_Homework")]
        public StudentDashboardDTO onclick_Homework([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Homework(data);
        }

        [Route("onclick_Classwork")]
        public StudentDashboardDTO onclick_Classwork([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Classwork(data);
        }

        [Route("onclick_Homework_load")]
        public StudentDashboardDTO onclick_Homework_load([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Homework_load(data);
        }

        [Route("onclick_Classwork_load")]
        public StudentDashboardDTO onclick_Classwork_load([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Classwork_load(data);
        }

        [Route("onclick_Sports")]
        public StudentDashboardDTO onclick_Sports([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Sports(data);
        }

        [Route("onclick_Inventory")]
        public StudentDashboardDTO onclick_Inventory([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Inventory(data);
        }

        [Route("onclick_PDA")]
        public StudentDashboardDTO onclick_PDA([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_PDA(data);
        }

        [Route("onclick_Gallery")]
        public StudentDashboardDTO onclick_Gallery([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Gallery(data);
        }


        [Route("onclick_Displaymessage")]
        public StudentDashboardDTO onclick_Displaymessage([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Gallery(data);
        }

        [Route("ViewStudentProfile")]
        public StudentDashboardDTO ViewStudentProfile([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            if (data.student_staffflag == "Student")
            {
                data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            }            
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            return sdd.ViewStudentProfile(data);
        }

        [Route("ViewMonthWiseAttendance")]
        public StudentDashboardDTO ViewMonthWiseAttendance([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.student_staffflag == "Student")
            {
                data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            }
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            return sdd.ViewMonthWiseAttendance(data);
        }

        [Route("ViewYearWiseFee")]
        public StudentDashboardDTO ViewYearWiseFee([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.student_staffflag == "Student")
            {
                data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            }
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            return sdd.ViewYearWiseFee(data);
        }

        [Route("ViewExamSubjectWiseDetails")]
        public StudentDashboardDTO ViewExamSubjectWiseDetails([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.student_staffflag == "Student")
            {
                data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            }
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            return sdd.ViewExamSubjectWiseDetails(data);
        }



        [Route("onclick_Homework_seen")]
        public StudentDashboardDTO onclick_Homework_seen([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //if (data.OnClickOrOnChange == "OnClick")
            //{
            //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //}
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Homework_seen(data);

        }
        [Route("onclick_classwork_seen")]
        public StudentDashboardDTO onclick_classwork_seen([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //if (data.OnClickOrOnChange == "OnClick")
            //{
            //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //}
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_classwork_seen(data);
        }


        [Route("onclick_noticeboard_seen")]
        public StudentDashboardDTO onclick_noticeboard_seen([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_noticeboard_seen(data);
        }


        

        [Route("onclick_Staff_details")]
        public StudentDashboardDTO onclick_Staff_details([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return sdd.onclick_Staff_details(data);
        }
    }
}