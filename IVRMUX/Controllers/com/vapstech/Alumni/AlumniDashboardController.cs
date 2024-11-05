using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Alumni;
using IVRMUX.Delegates.com.vapstech.Alumni;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [Route("api/[controller]")]
    public class AlumniDashboardController : Controller
    {

        AlumniDashboardDelegate objdelegate = new AlumniDashboardDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getalldetails")]
        public AlumniStudentDTO getalldetails(AlumniStudentDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.getalldetails(data);
        }

        [Route("saveakpkfile")]
        public AlumniStudentDTO saveakpkfile([FromBody]AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdelegate.saveakpkfile(data);
        }

        [Route("yearwiselist")]
        public AlumniStudentDTO yearwiselist([FromBody]AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.yearwiselist(data);
        }

        [Route("classwisestudent")]
        public AlumniStudentDTO classwisestudent([FromBody]AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.classwisestudent(data);
        }


        [Route("getgallery")]
        public AlumniStudentDTO getgallery(AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getgallery(data);
        }
        [Route("viewgallery")]
        public AlumniStudentDTO viewgallery([FromBody]AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.viewgallery(data);
        }
        [Route("alumninotice/{id:int}")]
        public AlumniStudentDTO alumninotice(int id)
        {
            AlumniStudentDTO data = new AlumniStudentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.alumninotice(data);
        }
        [Route("viewnotice")]
        public AlumniStudentDTO viewnotice([FromBody]AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.viewnotice(data);
        }




    }
}
