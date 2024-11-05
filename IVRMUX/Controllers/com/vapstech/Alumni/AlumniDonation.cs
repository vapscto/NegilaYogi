using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [Route("api/[controller]")]
    public class AlumniDonation : Controller
    {
        AlumniDonationDelegate delg = new AlumniDonationDelegate();

        [Route("Pageload/{id:int}")]
        public AlumniStudentDTO Pageload(int id)
        {
            AlumniStudentDTO MMD = new AlumniStudentDTO();
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return delg.Pageload(MMD);
        }
        [Route("getamount")]
        public AlumniStudentDTO getamount([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return delg.getamount(dto);
        }

        [Route("getpayment_details")]
        public AlumniStudentDTO getpayment_details([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return delg.getpayment_details(dto);
        }

        [Route("paymentsave")]
        public AlumniStudentDTO paymentsave([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delg.paymentsave(dto);
        }

        [Route("getdonationreport")]
        public AlumniStudentDTO getdonationreport([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            

            return delg.getdonationreport(dto);
        }

        //=============master donation 
        [Route("getdata_donation/{id:int}")]
        public AlumniStudentDTO getdata_donation(int id)
        {
            AlumniStudentDTO MMD = new AlumniStudentDTO();
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return delg.getdata_donation(MMD);
        }


        [Route("save_donation")]
        public AlumniStudentDTO save_donation([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delg.save_donation(dto);
        }

        [Route("deactive_donation")]
        public AlumniStudentDTO deactive_donation([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delg.deactive_donation(dto);
        }

        [Route("edit_donation")]
        public AlumniStudentDTO edit_donation([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delg.edit_donation(dto);
        }
         [Route("alumnidetails")]
        public AlumniStudentDTO alumnidetails([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delg.alumnidetails(dto);
        }

        [Route("getdonationprint")]
        public AlumniStudentDTO getdonationprint([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delg.getdonationprint(dto);
        }

    }
}
