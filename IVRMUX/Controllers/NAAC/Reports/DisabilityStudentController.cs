using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class DisabilityStudentController : Controller
    {

        public DisabilityStudentDelegate objdel = new DisabilityStudentDelegate();

        [Route("getdata/{id:int}")]
        public Criteria2_DTO getdata(int id)
        {
            Criteria2_DTO data = new Criteria2_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.getdata(data);
        }


        [Route("get_report")]
        public Criteria2_DTO get_report([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report(data);
        }

        [Route("Demand_Ratio_212_Report")]
        public Criteria2_DTO Demand_Ratio_212_Report([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.Demand_Ratio_212_Report(data);
        }

        [Route("Exm_P_Stud_Report")]
        public Criteria2_DTO Exm_P_Stud_Report([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.Exm_P_Stud_Report(data);
        }

        [Route("EMPLOYEE_AWARD_REPORT244")]
        public Criteria2_DTO EMPLOYEE_AWARD_REPORT244([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.EMPLOYEE_AWARD_REPORT244(data);
        }

        [Route("get_desination")]
        public Criteria2_DTO get_desination([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_desination(data);
        }

        [Route("Teacher_Recognised_242_Report")]
        public Criteria2_DTO Teacher_Recognised_242_Report([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.Teacher_Recognised_242_Report(data);
        }

        [Route("T_ProfileAndQuality_Report24")]
        public Criteria2_DTO T_ProfileAndQuality_Report24([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.T_ProfileAndQuality_Report24(data);
        }

        [Route("Student_Enrolment_Profile_Report21")]
        public Criteria2_DTO Student_Enrolment_Profile_Report21([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.Student_Enrolment_Profile_Report21(data);
        }

        [Route("StudentSat_Survey_Report27")]
        public Criteria2_DTO StudentSat_Survey_Report27([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.StudentSat_Survey_Report27(data);
        }

        [Route("sanctioned_posts_Report245")]
        public Criteria2_DTO sanctioned_posts_Report245([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.sanctioned_posts_Report245(data);
        }

        [Route("DeclrofResult_Report251")]
        public Criteria2_DTO DeclrofResult_Report251([FromBody]Criteria2_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.DeclrofResult_Report251(data);
        }

        
    }
}
