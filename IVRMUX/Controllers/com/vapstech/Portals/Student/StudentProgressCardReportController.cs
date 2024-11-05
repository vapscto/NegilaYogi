using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class StudentProgressCardReportController : Controller
    {
        StudentProgressCardReportDelegate _delg = new StudentProgressCardReportDelegate();

        [Route("getdetails/{id:int}")]
        public StudentProgressCardReportDTO getdetails(int id)
        {
            StudentProgressCardReportDTO data = new StudentProgressCardReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.getdetails(data);
        }

        [Route("onchangeclass")]
        public StudentProgressCardReportDTO onchangeclass([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.onchangeclass(data);
        }

        [Route("getreport")]
        public StudentProgressCardReportDTO getreport([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.getreport(data);
        }

        // BGHS
        [Route("Bghsgetdetails/{name}")]
        public StudentProgressCardReportDTO Bghsgetdetails(string name)
        {
            StudentProgressCardReportDTO data = new StudentProgressCardReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.EPCFT_ExamFlag = name;
            return _delg.Bghsgetdetails(data);
        }

        [Route("Bghsonchangeclass")]
        public StudentProgressCardReportDTO Bghsonchangeclass([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.Bghsonchangeclass(data);
        }

        [Route("Bghsgetreport")]
        public StudentProgressCardReportDTO Bghsgetreport([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.Bghsgetreport(data);
        }

        // Stmary 
        
        [Route("stmarygetdetails/{id:int}")]
        public StudentProgressCardReportDTO stmarygetdetails(int id)
        {
            StudentProgressCardReportDTO data = new StudentProgressCardReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.stmarygetdetails(data);
        }

        [Route("stmaryonchangeclass")]
        public StudentProgressCardReportDTO stmaryonchangeclass([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.stmaryonchangeclass(data);
        }

        [Route("stmarygetreport")]
        public StudentProgressCardReportDTO stmarygetreport([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.stmarygetreport(data);
        }

        // HHS
        [Route("HHSStudentProgressCardReport")]
        public StudentProgressCardReportDTO HHSStudentProgressCardReport([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.HHSStudentProgressCardReport(data);
        }

        // Stjames
        [Route("Get_Stjames_Progresscard_Report")]
        public StudentProgressCardReportDTO Get_Stjames_Progresscard_Report([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.Get_Stjames_Progresscard_Report(data);
        }

        // Notredame
        [Route("NDS_Get_Progresscard_Report")]
        public StudentProgressCardReportDTO NDS_Get_Progresscard_Report([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.NDS_Get_Progresscard_Report(data);
        }

        // BCEHS
        [Route("Get_BCEHS_Progresscard_Report")]
        public StudentProgressCardReportDTO Get_BCEHS_Progresscard_Report([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.Get_BCEHS_Progresscard_Report(data);
        }       

        // BIS
        [Route("BISStudentProgressCardReport")]
        public StudentProgressCardReportDTO BISStudentProgressCardReport([FromBody] StudentProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.BISStudentProgressCardReport(data);
        }
    }
}