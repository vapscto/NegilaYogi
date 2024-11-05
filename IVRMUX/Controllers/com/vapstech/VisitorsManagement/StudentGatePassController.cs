using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class StudentGatePassController : Controller
    {
        // GET: api/<controller>
        public DomainModelMsSqlServerContext _context;
        StudentGatePassDelegate delegat = new StudentGatePassDelegate();

        [Route("getdetails/{id:int}")]
        public StudentGatePass_DTO getdetails(int id)
        {
            StudentGatePass_DTO dto = new StudentGatePass_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.getdetails(dto);
        }

        [Route("get_class")]
        public StudentGatePass_DTO get_class([FromBody] StudentGatePass_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_class(d);
        }

        [Route("get_section")]
        public StudentGatePass_DTO get_section([FromBody] StudentGatePass_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_section(d);
        }

        [Route("get_student")]
        public StudentGatePass_DTO get_student([FromBody] StudentGatePass_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_student(d);
        }

        [Route("saverecord")]
        public StudentGatePass_DTO saverecord([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.saverecord(dto);
        }

        [Route("editrecord")]
        public StudentGatePass_DTO editrecord([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.editrecord(dto);
        }

        [Route("deactive")]
        public StudentGatePass_DTO deactive([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactive(dto);
        }


        [Route("checkstudentdata")]
        public StudentGatePass_DTO checkstudentdata([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.checkstudentdata(dto);
        }

        [Route("get_otpverification")]
        public StudentGatePass_DTO get_otpverification([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.get_otpverification(dto);
        }

        [Route("resendotp")]
        public StudentGatePass_DTO resendotp([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.resendotp(dto);
        }

        [Route("get_otpverification22")]
        public StudentGatePass_DTO get_otpverification22([FromBody] StudentGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.get_otpverification22(dto);
        }

        [Route("printbutton")]
        public StudentGatePass_DTO printbutton([FromBody]StudentGatePass_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.printbutton(data);
        }
        [Route("GetStudDetails")]
        public StudentGatePass_DTO GetStudDetails([FromBody]StudentGatePass_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.GetStudDetails(data);
        }

        [Route("getotp")]
        public StudentGatePass_DTO getotp([FromBody]StudentGatePass_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.SetString("exptimevisitorstudentgatepass", DateTime.Now.ToString());

            var details = delegat.getotp(data);

            HttpContext.Session.SetString("VISITORSTUDENTGATEPASSOTP", details.genotp);

            return details;
        }

        [Route("getnewotpverification")]
        public StudentGatePass_DTO getnewotpverification([FromBody]StudentGatePass_DTO data)
        {
            var GPHS_OTP = HttpContext.Session.GetString("VISITORSTUDENTGATEPASSOTP");

            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimevisitorstudentgatepass"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 15)
            {
                data.message = "Your Time is Over. Please Resend Your OTP!";
            }
            else
            {
                if (data.GPHS_OTP == GPHS_OTP)
                {
                    data.message = "Success";
                }
                else
                {
                    data.message = "Please Enter Valid OTP Number!";
                }
            }

            HttpContext.Session.SetString("VISITORSTUDENTGATEPASSOTPStatus", data.message);

            return data;
        }
    }
}
